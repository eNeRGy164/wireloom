# Architecture memory

This is a derived summary of the arc42 source documents under
[`arc42/`](arc42/index.md). It is a retrieval aid for implementation agents,
not the source of architectural decisions. Edit the arc42 chapters, contracts,
or ADRs first, then refresh this file and
[`.agents/architecture-memory.yaml`](../../.agents/architecture-memory.yaml).

## Architecture summary

- **Goals:** Keep Connext IDL generation inside the .NET build, emit C# data
  contracts and RTI type-specific support, and advance compatibility through
  small oracle-backed cases. See [chapter 1](arc42/01-introduction-and-goals.md).
- **Constraints:** The analyzer targets `netstandard2.0`; consumer applications
  support .NET 8 and higher, while repository projects default to .NET 10.
  C# 12+ and an RTI runtime reference of at least 7.3.1
  are required. That package floor is not proof that current generated source
  compiles against 7.3.1. Retained oracle sources use RTI 7.7.0 /
  `rtiddsgen` 4.7.0.
  See [chapter 2](arc42/02-architecture-constraints.md).
- **Context:** Wireloom runs at compile time between the consumer project and
  generated C#; the RTI runtime remains outside the generator boundary. See
  [chapter 3](arc42/03-context-and-scope.md).
- **Strategy:** Use an in-process Roslyn generator with a managed compiler
  pipeline, explicit `DdsIdl` roots, project-wide batch configuration,
  source-located diagnostics, and feature-level evidence. See [chapter 4](arc42/04-solution-strategy.md).
- **Building blocks:** The pipeline separates Roslyn hosting, input graph and
  preprocessing, front-end parsing and semantics, compilation orchestration and
  resolution, emission models and plans, and managed/native/support emitters.
  Aggregate members and raw collection elements declared through struct or
  union typedef chains use the underlying aggregate type in generated APIs and
  native conversions; standalone typedef declarations remain generated.
  Sequence typedef plans share the effective 100-element fallback across
  managed attributes, native allocation, and metadata. String typedefs import
  their bound attribute namespace and retain the effective 255-byte/code-unit
  limit. Alias data classes retain their RTI formatting delegate; support
  classes hide the nonvirtual default formatter for unsupported root kinds.
  Loops and escaping stay in support documents. Wide-string alias annotations
  preserve their wide kind/value so support instances initialize.
  Union constructors use XTypes discriminator type defaults, including negative
  first enum literals. Null-source copies share parameterless initialization;
  non-null copies avoid temporary default-branch allocations. Default-branch
  setters independently select unused labels, and negative enum casts are
  parenthesized.
  Optional struct members whose value is a struct or union preserve null
  presence with `NativeManagedOptional` conversions and optional DynamicType
  metadata. Their DynamicType preserves declared typedef aliases to match RTI,
  while managed properties and native conversions use the underlying
  aggregate. Optional union branches follow RTI's required selected-branch
  shape. Union metadata uses type-specific signed labels matching RTI C++
  while managed discriminators retain their IDL ranges. Narrow character 254
  uses signed 8-bit metadata label -2; its managed value and wire byte stay 254. Narrow char labels and default selection are limited to 0..255; wchar retains 0..65535.
  Parsing, binding, and
  validation are explicit front-end phases, and chapter 5 includes a level-2
  zoom of the compiler and preprocessing boundary.
  See [chapter 5](arc42/05-building-block-view.md).
- **Runtime and deployment:** The manually triggered wire workflow targets RTI
  Connext 7.7.0 and compares Wireloom-generated C#, RTI-generated C#, and
  RTI-generated C++ across all nine directed pairings for each fixture. Four
  isolated runners balance whole cases by exchange count; a .NET-only report
  job validates matching plans and complete coverage before merging evidence. Incomplete exchanges prevent merging; unexpected setup errors retain a nonzero job exit.
  Shards load images through parallel cached Docker builds; shard zero exports
  the shared build cache. Only RTI shard jobs receive the runtime license secret. Readers
  require and verify valid payloads while counting ordinary instance lifecycle
  notifications separately. The verifier normalizes discriminator widths;
  character fixtures use independent typed values and a single-byte C++
  execution charset. Present optional wide-string
  sequences use the DynamicData C API peer; absent, empty, and narrow-only
  controls use typed peers. Optional aggregate values are supported by the
  generator. RTI 7.3.1 generated-code compatibility is not established.
  See [chapters 6](arc42/06-runtime-view.md) and [7](arc42/07-deployment-view.md).
- **Cross-cutting concerns:** Evidence vocabulary, deterministic source
  identity, explicit compiler phases, diagnostics, test separation, measured
  invalidation, and supply-chain controls apply across the system. Build-time
  generated runtime contract fixtures execute typed managed/native/support
  APIs on Windows and Linux. Coveralls measures maintained generator source;
  generated fixtures and test project code are excluded. The behavior matrix
  keeps unresolved contracts visible. Retained RTI C# oracle sources and
  typed runtime fixtures cover unbounded typedef behavior.
  RTI C# is a baseline, not an API ceiling;
  improvements supported by RTI C++ evidence retain separate wire verification.
  See
  [chapter 8](arc42/08-crosscutting-concepts.md) and the [incremental baseline](../performance/incremental-generator-baseline.md).
- **Decisions:** Roslyn hosting, explicit roots, independent semantic models,
  project-wide root metadata, explicit parse/bind/validate phases, explicit
  runtime ownership, case-level compatibility evidence, and separated
  compiler orchestration/resolution/emission are current decisions. The manual
  wire matrix targets exact RTI 7.7.0. A package reference floor of 7.3.1 does
  not establish generated-code compatibility with that release. Wire-level
  compatibility is a licensed evidence tier. Optional aggregate DynamicType
  metadata preserves declared aliases to match the RTI oracle. See
  [chapter 9](arc42/09-architectural-decisions.md).
- **Quality and risk:** `05-array-of-sequences` is included with DDSG0105 as a
  warning; both retained C# mappings flatten the IDL array while RTI C++ preserves it,
  so pairings involving Wireloom C# fail discovery and do not establish array
  shape fidelity. Generated reports contain the outcome for each selected
  fixture and pairing. The wchar union uses manually constructed RTI C++
  `DynamicType` metadata, and the FlatData case validates standard C++ mapping,
  not RTI's FlatData-specific C++ layout. The C++ peer uses DynamicData C API
  setters for present optional wide-string fixtures. Coverage outside the
  retained corpus, licensed exact-version environments, per-root output
  invalidation, RTI 7.3.1 generated-code compatibility, and host qualification
  remain risks. See [chapters 10](arc42/10-quality-requirements.md) and
  [11](arc42/11-risks-and-technical-debt.md).

## Repository defaults and decisions

| Area                        | Value                                                         | Provenance                                    |
| :-------------------------- | :------------------------------------------------------------ | :-------------------------------------------- |
| Supported consumer minimum  | `net8.0`, C# 12+                                              | Consumer requirements, chapter 2              |
| Repository target default   | `net10.0`                                                     | `Directory.Build.props`, chapter 2            |
| Generator target            | `netstandard2.0`                                              | `Wireloom.Dds.Generator.csproj`, chapter 2    |
| SDK policy                  | `10.0.400`; prerelease disabled; `latestFeature` roll-forward | `global.json`; Qodana compatibility           |
| Package output              | NuGet analyzer/source-generator package                       | Generator project and workflows               |
| Runtime package reference floor | RTI Connext DDS `7.3.1+`                                  | Generator diagnostic `DDSG0003`, chapter 2; not wire-compatibility proof |
| Oracle capture baseline     | RTI Connext DDS `7.7.0` / `rtiddsgen 4.7.0`                   | Corpus manifest, chapter 2                    |
| Runtime identifiers         | None; generator is runtime-neutral                            | Architecture memory and project configuration |
| Test platform               | Microsoft Testing Platform with xUnit v3                      | Project files and chapter 2                   |
| Assertions                  | Shouldly                                                      | Central package management                    |
| Package management          | Central versions, lock files, NuGet audit                     | `Directory.Packages.props`, chapter 8         |

No provisional stack defaults are currently carried forward when the repository
does not use them.

## Open gaps

- Visual Studio live behavior, Linux builds, and C++ peer interoperability are
  not verified for every supported feature.
- Runtime and wire-level evidence must be expanded before broader compatibility
  claims are made. Current generation does not compile against 7.3.1 despite
  the package reference floor; version-aware emission remains planned.
- The retained oracle capture remains marked pending review in the corpus
  manifest; source-shape evidence must not be treated as runtime or wire proof.

## Source map

- [arc42 index](arc42/index.md) and chapters 1–12
- [corpus guide](../corpus/README.md) and [feature coverage](../corpus/FEATURE-COVERAGE.md)
- [incremental generator baseline](../performance/incremental-generator-baseline.md)
- [repository architecture guidance](../../AGENTS.md)
- [machine-readable architecture memory](../../.agents/architecture-memory.yaml)
