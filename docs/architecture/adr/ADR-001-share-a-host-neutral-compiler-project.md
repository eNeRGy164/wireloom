# ADR-001 Share a host-neutral compiler project

- **Date:** 2026-10-25
- **Status:** Accepted
- **Approved by:** Michaël Hompus, October 25, 2026

## Context

Wireloom's IDL compiler already separates preprocessing, parsing, binding,
validation, emission planning, and C# emission. The compiler still ships in the
Roslyn generator project, and four compiler files use Roslyn's C# syntax and
literal-formatting helpers. A future Playground and the existing generator
must call the same compiler without importing a second parser or emitter.

The generator must remain loadable in the supported .NET 8 C# 12 host. The
existing `Wireloom.Dds.Generator` NuGet package is the sole published package;
the compiler is for repository callers that move together and is not an
independently published library. Covered inputs must retain their generated
document names and text, diagnostics, and batch option behavior.

Compilation currently has two fatal-error paths: `IdlException` is converted
to `DDSG0001` by the Roslyn adapter, while an unsupported target-profile
feature returns an error diagnostic with no documents. Collected diagnostics
can precede a fatal exception. The host-neutral result needs to expose these
outcomes together while preserving first-fatal-error behavior.

## Decision

**Move the compiler into a non-packable `netstandard2.0` project that the
Roslyn generator and repository hosts reference, and bundle its assembly next
to the generator in the existing NuGet package.**

The shared API returns generated documents and diagnostics in one result and
requires the caller to select an RTI profile explicitly. The Roslyn adapter
continues to discover the resolved RTI assembly, map its version to a supported
profile, translate input configuration errors, and present compiler
diagnostics. Compiler diagnostics own their IDs, severities, messages, and
input offsets; the adapter maps them to Roslyn descriptors and source
locations.

The core accepts an in-memory input collection. `IdlInput` defensively copies
its define, undefine, and include-directory collections and exposes them as
read-only. Compilation snapshots the input collection at entry. Preserve the
current first-fatal-error behavior, cancellation behavior, include path
semantics, and batch option rules. Do not change emitted C# names or text for
covered inputs.

## Consequences

- Parsing, binding, validation, planning, and emission have one Roslyn-free
  assembly that repository C# projects can share.
- The generator and compiler must be built and tested as one version. Analyzer
  package tests must prove the compiler dependency loads from the package.
- The generator package stays the only published package; consumers do not
  receive the compiler as an application runtime dependency.
- The compiler project targets `netstandard2.0` to preserve the analyzer's
  minimum host compatibility. Repository projects continue to use the pinned
  SDK and `net10.0` defaults.
- Existing public compile entry points can be replaced; repository callers
  migrate together. No API or binary compatibility promise is added for direct
  callers of the current generator assembly.
- Core-owned diagnostic severity avoids duplicating the diagnostic policy in
  another host. Roslyn remains responsible for IDE/build presentation.
- C# keyword and literal formatting become core implementation details.
  Parity tests against the previous Roslyn helpers guard emitted text.
- Input copies provide stable compiler inputs per Playground edit while
  retaining current root aggregation, strictness, escape, and file-owned
  mapping/limit behavior.
- The extraction does not prove browser execution or DDS wire compatibility.

## Considered options

1. Keep the compiler inside the generator project and expose more of its API.
   - **Pros**: No project split or analyzer dependency-loading change.
   - **Cons**: The shared compiler remains in the Roslyn adapter assembly and
     compiler code cannot be built as a host-neutral project.
2. Compile or link the compiler separately for each host.
   - **Pros**: Each host can choose its own dependencies and deployment shape.
   - **Cons**: Builds and changes can diverge across compiler copies, defeating
     the single implementation requirement.
3. Create a separate compiler project referenced by repository hosts and
   bundle its assembly with the existing analyzer package.
   - **Pros**: Gives both in-repository hosts one compiler implementation,
     keeps Roslyn at the adapter boundary, and preserves the single published
     package and release version.
   - **Cons**: Requires moving Roslyn helper usage behind core-owned formatting,
     updating test/project references, and verifying analyzer dependency
     loading.
4. Publish a standalone compiler NuGet package as well as the generator.
   - **Pros**: Supports direct hosts outside the repository without a project
     reference to source.
   - **Cons**: Creates an additional public package, release and compatibility
     policy, outside the agreed scope of issue #107.

## More information

- Issue: [#107 Separate the IDL compiler core from the Roslyn adapter](https://github.com/eNeRGy164/wireloom/issues/107).
- Follow-on browser compiler work: [#108 WebAssembly-compatible member-ID hashing](https://github.com/eNeRGy164/wireloom/issues/108).

## References

- Related building block: [Compiler and preprocessing](../arc42/05-building-block-view.md#52-level-2-compiler-and-preprocessing).
- Related runtime flow: [Consumer build and generation](../arc42/06-runtime-view.md#61-consumer-build-and-generation).
- Related concept: [Diagnostics and source locations](../arc42/08-crosscutting-concepts.md#82-diagnostics-and-source-locations).
- Related code: [`IdlCompilation`](../../../src/Wireloom.Dds.Compiler/Compiler/IdlCompilation.cs).
