# ADR-002 Inject member-ID MD5 per compilation

- **Date:** 2026-10-25
- **Status:** Accepted
- **Approved by:** Michaël Hompus, October 26, 2026

## Context

The shared compiler computes DDS member IDs using MD5 over UTF-8, the first
four digest bytes interpreted as a little-endian integer, and a 28-bit mask.
Struct members and union branches share `IdlMemberIdHash`. Initially, that
helper called platform MD5 directly; it now receives the provider through the
per-compilation service context.

[Issue #108](https://github.com/eNeRGy164/wireloom/issues/108) requires browser
execution without a native crypto provider while preserving existing member
IDs and generated output. Browser .NET and standard JavaScript Web Crypto do
not provide MD5. A browser host will need a managed or custom JavaScript
implementation, while the current Roslyn host can continue using BCL MD5.
See the [.NET 10 MD5 implementation](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Security.Cryptography/src/System/Security/Cryptography/MD5.cs)
and [Web Cryptography specification](https://www.w3.org/TR/2017/REC-WebCryptoAPI-20170126/)
for the platform limitations.

[ADR-001](ADR-001-share-a-host-neutral-compiler-project.md) keeps one
`netstandard2.0` compiler independent of Roslyn, MSBuild, and RTI and permits
repository callers to migrate together. Keeping digest production outside
the core lets each host select its implementation without changing protocol
semantics, introducing browser APIs into the core, or making parsing async.
The interface needed here is a callable contract; a separate C# interface
type or dependency injection container is unnecessary.

## Decision

**Require each compiler host to supply a synchronous `Func<byte[], byte[]>`
MD5 delegate per compilation while the shared compiler retains all DDS
member-ID semantics.**

The API is `Compile(inputs, profile, computeMd5, cancellationToken)`.
The delegate accepts UTF-8 bytes and returns the complete 16-byte MD5 digest.
The generator supplies a stateless BCL lambda. A future WASM host can supply
a browser-compatible implementation. The reference is stored only in the
compilation's service context and reaches both struct and union hash paths.
There is no optional native fallback, global setter, platform detection, or
task-returning hash contract. The compiler signature changes together with
all repository callers, following ADR-001's direct-caller policy.

## Consequences

- The compiler owns hash-source selection, decoded strings, UTF-8 encoding,
  digest validation, byte ordering, and the 28-bit mask. Hosts cannot replace
  the complete member-ID calculation through this contract.
- The native generator retains current MD5 behavior. A future browser adapter
  can register or load JS during startup, then supply a synchronous callable
  without changing compiler stages.
- A required callback removes the maintained core's platform MD5 call path
  and makes a missing host capability explicit, including for batches that
  contain no hash-based members.
- Delegate references are isolated per compilation. Hosts must supply a
  deterministic implementation and make captured state safe if reused across
  concurrent compilations; injecting a delegate does not enforce purity.
- Null callbacks fail at API entry with `ArgumentNullException`. Null or
  non-16-byte digest results fail with `InvalidOperationException`. Callback
  exceptions are wrapped as `InvalidOperationException` with their cause;
  `OperationCanceledException` propagates unchanged. This prevents an
  `IdlException` thrown by a callback from becoming an IDL diagnostic.
- Digest failures produce no compilation result and are not reported as
  `DDSG0001`. There is no retry, alternate provider, or substituted member ID.
  Existing IDL error results and diagnostic IDs remain unchanged.
- Compiler callers, internal construction sites, test support, and corpus
  tools now supply the provider. No compatibility overload retains an
  implicit native provider in the compiler.
- Compiler and generator tests cover injected input, ID conversion, invalid
  digests, callback failures, and existing corpus output.
- This change prepares WASM integration but does not implement a browser MD5
  provider, deploy a browser host, or satisfy #108's real-browser acceptance
  criterion by itself. No new package, project, target, or dependency was
  needed for the core/native change.

## Considered options

### 1. Inject a required MD5 delegate per compilation

- **Pros:** Small public contract, explicit host ownership, isolated
  composition, and synchronous compiler behavior; keeps native MD5 available.
- **Cons:** Requires caller migration, runtime result validation, and parity
  evidence for each host implementation.

### 2. Use one portable managed MD5 implementation in the compiler

- **Pros:** One implementation and no compiler API change or JS initialization.
- **Cons:** Moves algorithm maintenance into the compiler and replaces native
  hashing on all hosts; does not provide the selected host extension point.

### 3. Inject a dedicated provider interface

- **Pros:** A named type can describe a larger capability with multiple methods.
- **Cons:** Adds a public interface and implementation types for one stateless
  operation; a documented delegate expresses the current contract adequately.

### 4. Set a mutable static delegate

- **Pros:** Requires less constructor plumbing.
- **Cons:** Creates process-wide initialization and concurrency hazards across
  generator instances and tests; ownership does not match one compilation.

### 5. Select implementations inside the compiler

- **Pros:** Can preserve an implicit native calling convention.
- **Cons:** Runtime checks, conditional builds, or JS imports couple the core
  to host selection and can hide a missing browser capability; async hashing
  would also change parsing without providing standard Web Crypto MD5.

## More information

- [Issue #108](https://github.com/eNeRGy164/wireloom/issues/108).
- [Compiler host boundary](../arc42/images/05/level-3-browser-compilation-host.puml).

## References

- Related building block: [Shared IDL compiler](../arc42/05-building-block-view.md#531-shared-idl-compiler).
- Related runtime flow: [Consumer build and generation](../arc42/06-runtime-view.md#611-generate-or-regenerate-application-contracts).
- Related API contract: [Shared compiler API](../arc42/05-building-block-view.md#compiler-contract-and-payload-examples).
- Related code: [`IdlCompiler`](../../../src/Wireloom.Dds.Compiler/Api/IdlCompiler.cs),
  [`IdlParseContext`](../../../src/Wireloom.Dds.Compiler/Compiler/FrontEnd/Parsing/IdlParseContext.cs),
  and [`IdlMemberIdHash`](../../../src/Wireloom.Dds.Compiler/Compiler/FrontEnd/Semantic/IdlMemberIdHash.cs).
