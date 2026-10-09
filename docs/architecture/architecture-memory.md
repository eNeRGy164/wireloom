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
- **Constraints:** The analyzer targets `netstandard2.0`; consumer and test
  projects use .NET 10; C# 12+ and an RTI runtime reference of at least 7.3.1
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
  Optional struct members whose value is a struct or union preserve null
  presence with `NativeManagedOptional` conversions and optional DynamicType
  metadata. Their DynamicType preserves declared typedef aliases to match RTI,
  while managed properties and native conversions use the underlying
  aggregate. Optional union branches follow RTI's required selected-branch
  shape.
  Parsing, binding, and
  validation are explicit front-end phases, and chapter 5 includes a level-2
  zoom of the compiler and preprocessing boundary.
  See [chapter 5](arc42/05-building-block-view.md).
- **Runtime and deployment:** The wire workflow covers Wireloom-positive,
  wire-testable cases with C#/C++ peers and exact RTI 7.7.0. The last complete
  fixture run before optional aggregate fixtures covered 372 scenarios across
  93 case/fixture rows: 364 passed
  and 8 failed as expected, with no unexpected or unrun scenarios. Two expected
  failures are the array-of-sequences cross-language mismatches. Six are the
  present-wide-value pairings for optional string sequences. AddressSanitizer
  locates the C++ writer crash in RTI's `std::wstring` sequence serializer; the
  C++ reader discovers the C# writer but returns no sample for these fixtures.
  Absent, empty, and narrow-only controls pass. C# optional-sequence
  verification is fixture-driven; C++ comparisons check optional presence and
  content, and logs retain child exit codes. Each
  case/fixture has an independent exchange for each language pairing; Markdown
  summarizes pairing outcomes and defects while JSON and sanitized logs retain
  details. RTI 7.3.1 needs future version-aware emission. See
  [chapters 6](arc42/06-runtime-view.md) and [7](arc42/07-deployment-view.md).
- **Cross-cutting concerns:** Evidence vocabulary, deterministic source
  identity, explicit compiler phases, diagnostics, test separation, measured
  invalidation, and supply-chain controls apply across the system. See
  [chapter 8](arc42/08-crosscutting-concepts.md) and the [incremental baseline](../performance/incremental-generator-baseline.md).
- **Decisions:** Roslyn hosting, explicit roots, independent semantic models,
  project-wide root metadata, explicit parse/bind/validate phases, explicit
  runtime ownership, case-level compatibility evidence, and separated
  compiler orchestration/resolution/emission are current decisions. The
  initial wire baseline is exact 7.7.0; future older-runtime support needs a
  generator target API version distinct from the package version. Wire-level
  compatibility remains a manual licensed evidence tier. Optional aggregate
  DynamicType metadata preserves declared aliases to match the RTI oracle. See
  [chapter 9](arc42/09-architectural-decisions.md).
- **Quality and risk:** The last complete exact 7.7.0 fixture matrix before
  optional aggregate additions completed 372
  scenarios with 364 passes and 8 expected failures; there were no unexpected
  failures or unrun scenarios.
  `05-array-of-sequences` is included with defaults because DDSG0105 is a warning;
  current C# output flattens its array-of-sequences members, so a wire pass does
  not prove array-shape fidelity. Both same-language pairings pass, while both
  C#↔C++ pairings fail endpoint discovery because the generated C# flat sequence
  and C++ array-of-sequences have different type kinds. A direct probe using
  RTI's retained C# oracle reproduces the same cross-language failure, so this
  is an RTI C# binding limitation rather than a Wireloom-only regression.
  `03-alias-aggregate` and both `07-union-aliases` variants now pass all four
  pairings after aggregate member aliases were projected to their underlying
  struct or union type in managed APIs and native conversions.
  `09-optional-aggregate-member` now generates optional struct and union values,
  retains declared aliases in optional aggregate DynamicType metadata to match
  RTI, and has absent/present fixtures with nested payloads. Those eight wire
  exchanges await an exact RTI 7.7.0 run. Consumers verify expected fixtures,
  and peers request reliable delivery.
  The wchar union uses a manually constructed RTI C++ `DynamicType` matching
  the discriminator, labels, IDs, and extensibility. FlatData uses standard
  C++ peer generation after removing its C#-ignored mapping annotation; this
  does not verify the RTI FlatData-specific C++ layout. Six present-wide-value
  optional-string pairings are expected failures: the C++ writer crashes
  inside RTI's `sequence_helper<std::wstring>::get_value_pointer`, and the C++
  reader returns no sample from C# for these fixtures. RTI 7.3.1 target
  compatibility, licensed exact-version environments, optional aggregate wire
  evidence, coarse per-root output invalidation, and coverage outside
  the retained corpus remain open risks. See
  [chapters 10](arc42/10-quality-requirements.md) and
  [11](arc42/11-risks-and-technical-debt.md).

## Repository defaults and decisions

| Area                        | Value                                                         | Provenance                                    |
| :-------------------------- | :------------------------------------------------------------ | :-------------------------------------------- |
| Consumer/test target        | `net10.0`                                                     | `global.json`, chapter 2                      |
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
