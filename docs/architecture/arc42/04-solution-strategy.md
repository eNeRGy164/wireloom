# 4. Solution strategy

The following choices shape how Wireloom realizes its
[goals](01-introduction-and-goals.md) within the
[constraints](02-architecture-constraints.md) and
[product boundary](03-context-and-scope.md). Each choice explains the approach,
its rationale, and the architectural consequences. Detailed behavior and
implementation remain in the linked chapters, contracts, and ADRs.

## 4.1 Integrate generation into the .NET compiler

Use a managed [Roslyn](12-glossary.md#roslyn)
[incremental generator](12-glossary.md#incremental-generator), distributed as a NuGet analyzer.
MSBuild `DdsIdl` items declare generation roots and options; Roslyn
[`AdditionalFiles`](12-glossary.md#additionalfiles) carries the tracked inputs into compilation. Following an
include does not automatically make that file a generation root; the same
file can also be selected explicitly as a root.

This makes generation part of normal restore, build, and IDE operation,
removing the separate generator toolchain and custom invocation scripts.
Roslyn manages the generated documents, so consumers can build directly from
IDL without maintaining generated C# files in source control. The adapter
targets `netstandard2.0` to preserve the
[minimum analyzer-host baseline](02-architecture-constraints.md#21-product-and-execution-boundaries).
Input discovery and invalidation must remain aligned with the
[project configuration contract](../../guides/configuration.md).
Per-file sequence/string semantics travel with each tracked IDL input, so an
included declaration uses the policy of its declaring file. The compiler
keeps finite fallback sizes separate from RTI unbounded metadata and allocation.

## 4.2 Share one compiler and keep host integration in adapters

Use one host-neutral, non-packable `netstandard2.0` compiler for the generator,
repository callers, and Playground. It accepts in-memory inputs and an explicit
RTI profile and returns generated documents and diagnostics. Host adapters own
input discovery, platform services, and presentation; the core has no Roslyn,
MSBuild, RTI assembly, or filesystem dependency.

This keeps language behavior and generated output consistent across entry
points and allows focused compiler tests without a build or browser host.
The generator bundles the compiler assembly in the existing analyzer package;
the two are built and tested as one version. Platform-dependent services such
as [MD5](12-glossary.md#md5) digest production are supplied per compilation while DDS member-ID
semantics remain in the core. These boundaries follow
[ADR-001](../adr/ADR-001-share-a-host-neutral-compiler-project.md) and
[ADR-002](../adr/ADR-002-inject-member-id-md5-per-compilation.md).

## 4.3 Resolve language meaning before planning generated code

Use OMG IDL and [DDS-XTypes](12-glossary.md#dds-xtypes) to define language meaning. Separate preprocessing,
parsing, binding, semantic validation, emission planning, and rendering.
Resolve names, values, annotations, and source origins once, then let output
paths consume the resolved model and plans.

This localizes semantic decisions, avoids inconsistent rules across emitters,
and supports the [diagnostic and maintainability goals](01-introduction-and-goals.md#12-quality-goals).
Unsafe input fails before emission with diagnostics at the original IDL
location. Accepted ignored or potentially lossy behavior remains explicitly
diagnosed. Host adapters present the core's results through their own UI.
Specification meaning, RTI generator limitations, and Wireloom support limits
remain distinct; see [diagnostic policy](08-crosscutting-concepts.md#82-source-provenance-and-failure-reporting)
and [semantic binding](08-crosscutting-concepts.md#81-idl-meaning-and-identity).

## 4.4 Isolate RTI version differences in explicit profiles

Select the [API profile](12-glossary.md#rti-api-profile) from the runtime assembly resolved by the consumer
build. Direct compiler hosts supply their profile explicitly. Keep parsing,
binding, and semantic validation independent of that selection; apply
profile-specific API and runtime-support choices during emission planning.

This matches generated support to the runtime the application actually uses
and contains version differences without duplicating the compiler front end.
A mapped profile and exact-release qualification have separate meanings,
following the [RTI version policy](../../guides/support-and-compatibility.md#rti-version-qualification).
The adapter owns version discovery, and generated API and native-support paths
must use the same selected profile. See the
[compiler structure](05-building-block-view.md) and
[profile decision](09-architectural-decisions.md).

## 4.5 Generate C# that improves the developer experience

Generate modern C# with explicit generated-code identification, useful XML
documentation, [nullable annotations](12-glossary.md#nullable-annotation), and
[partial types](12-glossary.md#partial-type). Keep handwritten
extensions in separate maintained files. Together with compiler-managed
generated documents, this supports the
[generated C# quality goal](01-introduction-and-goals.md#12-quality-goals)
without source-control noise or manual edits that regeneration would replace.

Preserve working application API usage as the drop-in baseline. Improvements
to the developer experience are acceptable even when they introduce observable
differences, provided they do not affect DDS wire operations. Reliable alias
`ToString` output and richer IntelliSense are examples; reproducing RTI's
limitations in those areas would not serve the product's goals.

Document deliberate differences and verify their behavior separately from
serialization and interoperability. The
[value-semantics concept](08-crosscutting-concepts.md#84-generated-values-and-resource-ownership)
and [migration guide](../../migration/rtiddsgen-to-wireloom.md#generated-c-api-differences)
record the current behavior and its impact.

## 4.6 Verify different contracts with separate test layers

Use focused in-memory tests for compiler behavior, attributable corpus cases
for language acceptance and API shape, packed-package tests for real consumer
integration, runtime-contract tests for generated API behavior, and directed
peer exchanges for DDS interoperability. Analyze emitted C# independently of
its normal generated-code classification.

This gives maintainers fast feedback while keeping each compatibility claim
linked to evidence that establishes it. API shape alone cannot establish
application behavior or wire compatibility. Independent output analysis checks
the code itself alongside the normal treatment of generated documents in
consumer tooling. Exact-profile wire checks remain separate from the
license-free test and report-verification paths. See the
[evidence constraints](02-architecture-constraints.md#22-evidence-and-provenance-policies),
[evidence scope](08-crosscutting-concepts.md#85-compatibility-and-evidence), and
[generated C# quality report](../../corpus/csharp-quality.md).

## 4.7 Run the supporting Playground locally in a worker

Reuse the shared compiler in a dedicated .NET WebAssembly module worker behind
a TypeScript/Vite client, following
[ADR-003](../adr/ADR-003-run-playground-compilation-in-a-browser-worker.md).
This provides local evaluation without a compilation service and isolates
synchronous compiler work from page interaction.

The client owns workspace editing and presentation; the worker hosts compiler
execution. Results must belong to the current workspace, and worker failures
must allow recovery. This boundary preserves a responsive, useful demo while
keeping language meaning in the shared compiler. Preview behavior and DDS
qualification remain separate. Scheduling and recovery interactions are in the
[runtime view](06-runtime-view.md#62-playground-evaluation); protocol and
workspace models are in the
[browser concepts](08-crosscutting-concepts.md#86-workspace-consistency).
Browser evidence belongs in the [qualification guide](../../testing/playground-qualification.md).

---

[< 3. Context and scope](03-context-and-scope.md) | [Index](index.md) | [5. Building block view >](05-building-block-view.md)
