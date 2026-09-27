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
  are required. Retained oracle sources use RTI 7.7.0 / `rtiddsgen` 4.7.0.
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
  See [chapter 5](arc42/05-building-block-view.md).
- **Runtime and deployment:** The meaningful runtime scenario is the consumer
  build; the output is packaged into the application, which uses its selected
  RTI runtime. See [chapters 6](arc42/06-runtime-view.md) and
  [7](arc42/07-deployment-view.md).
- **Cross-cutting concerns:** Evidence vocabulary, deterministic source
  identity, diagnostics, test separation, and supply-chain controls apply
  across the system. See [chapter 8](arc42/08-crosscutting-concepts.md).
- **Decisions:** Roslyn hosting, explicit roots, independent semantic models,
  project-wide root metadata, explicit runtime ownership, case-level
  compatibility evidence, and separated compiler orchestration/resolution/
  emission are current decisions. See [chapter 9](arc42/09-architectural-decisions.md).
- **Quality and risk:** Corpus shape checks are stronger than the current
  runtime/interoperability evidence; coverage outside the retained corpus
  remains a visible risk. See [chapters 10](arc42/10-quality-requirements.md)
  and [11](arc42/11-risks-and-technical-debt.md).

## Repository defaults and decisions

| Area                        | Value                                       | Provenance                                    |
| :-------------------------- | :------------------------------------------ | :-------------------------------------------- |
| Consumer/test target        | `net10.0`                                   | `global.json`, chapter 2                      |
| Generator target            | `netstandard2.0`                            | `Wireloom.Dds.Generator.csproj`, chapter 2    |
| SDK                         | `10.0.401`                                  | `global.json`                                 |
| Package output              | NuGet analyzer/source-generator package     | Generator project and workflows               |
| Runtime compatibility floor | RTI Connext DDS `7.3.1+`                    | Generator diagnostic `DDSG0003`, chapter 2    |
| Oracle capture baseline     | RTI Connext DDS `7.7.0` / `rtiddsgen 4.7.0` | Corpus manifest, chapter 2                    |
| Runtime identifiers         | None; generator is runtime-neutral          | Architecture memory and project configuration |
| Test platform               | Microsoft Testing Platform with xUnit v3    | Project files and chapter 2                   |
| Assertions                  | Shouldly                                    | Central package management                    |
| Package management          | Central versions, lock files, NuGet audit   | `Directory.Packages.props`, chapter 8         |

No provisional stack defaults are currently carried forward when the repository
does not use them.

## Open gaps

- Visual Studio live behavior, Linux builds, and C++ peer interoperability are
  not verified for every supported feature.
- Runtime and wire-level evidence must be expanded before broader compatibility
  claims are made.
- The retained oracle capture remains marked pending review in the corpus
  manifest; source-shape evidence must not be treated as runtime or wire proof.

## Source map

- [arc42 index](arc42/index.md) and chapters 1–12
- [corpus guide](../corpus/README.md) and [feature coverage](../corpus/FEATURE-COVERAGE.md)
- [repository architecture guidance](../../AGENTS.md)
- [machine-readable architecture memory](../../.agents/architecture-memory.yaml)
