# Wireloom documentation

Wireloom replaces the C# generation step for supported RTI DDS contracts.
Choose the task you need to complete. For product goals and boundaries, start
with the [architecture introduction](architecture/arc42/01-introduction-and-goals.md).

## Use Wireloom

| Task                                                    | Start here                                                                                               |
| :------------------------------------------------------ | :------------------------------------------------------------------------------------------------------- |
| Try generation without installation                     | [Playground](https://energy164.github.io/wireloom/) · [workspace guide](guides/playground-workspaces.md) |
| Generate contracts in a .NET project                    | [Package quick start](../src/Wireloom.Dds.Generator/README.md#quick-start)                               |
| Build a publisher and subscriber                        | [Hello World tutorial](guides/hello-world.md) · [repository sample](../samples/HelloWorld/README.md)     |
| Replace an existing RTI generation step                 | [Migration procedure](migration/rtiddsgen-to-wireloom.md)                                                |
| Edit, inspect, extend, and analyze generated code       | [Developer workflow](guides/developer-workflow.md)                                                       |
| Configure roots, includes, symbols, mappings, or bounds | [Configuration reference](guides/configuration.md)                                                       |
| Understand initialization, copying, and ownership       | [Generated value semantics](guides/generated-value-semantics.md)                                         |
| Look up annotations or a diagnostic                     | [Package reference](../src/Wireloom.Dds.Generator/README.md#annotation-reference)                        |
| Check support, versions, and known limits               | [Support policy](guides/support-and-compatibility.md) · [feature coverage](corpus/FEATURE-COVERAGE.md)   |
| Report a mismatch or integration failure                | [Reporting guide](guides/reporting-issues.md)                                                            |
| Find a term or abbreviation                             | [Glossary](architecture/arc42/12-glossary.md)                                                            |

## Maintain and qualify Wireloom

| Task                                           | Start here                                                                                              |
| :--------------------------------------------- | :------------------------------------------------------------------------------------------------------ |
| Set up the repository and choose validation    | [Contributing](../CONTRIBUTING.md)                                                                      |
| Apply code conventions                         | [Code style](CODE-STYLE.md)                                                                             |
| Change parsing, binding, or validation         | [IDL semantic reference](guides/idl-semantics.md) · [preprocessor coverage](PREPROCESSOR-FEATURES.md)   |
| Call the compiler from a repository host       | [Shared compiler API](reference/compiler-api.md)                                                        |
| Maintain the browser client or protocol        | [Playground development](../playground/README.md) · [worker protocol](reference/playground-protocol.md) |
| Run tests, coverage, and Qodana                | [Quality procedure](testing/quality.md)                                                                 |
| Reproduce exact-profile compatibility evidence | [Corpus guide](corpus/README.md) · [LTS runbook](corpus/LTS-PROFILES.md)                                |
| Run DDS exchanges and interpret reports        | [Wire runbook](../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)                             |
| Maintain wire fixtures or orchestration        | [Fixture reference](testing/wire-fixtures.md) · [harness reference](testing/wire-harness.md)            |
| Qualify the Playground                         | [Browser qualification](testing/playground-qualification.md)                                            |
| Publish a package                              | [Release checklist](releases/PREPARING-A-RELEASE.md)                                                    |
| Publish or roll back the Playground            | [Deployment runbook](guides/playground-deployment.md)                                                   |
| Understand boundaries and decisions            | [Architecture](architecture/README.md)                                                                  |

## Read evidence for the right scope

For an installed package, use its [release qualification record](releases/README.md).
Current source guides can describe behavior added after that release.
Generation, application API execution, native serialization, and DDS exchanges
establish different facts.

- [Generated-C# static-analysis snapshot](corpus/csharp-quality.md)
- [Observed RTI code-generation findings](corpus/RTI-CODEGEN-DEFECTS.md)
- [Incremental-generator measurements](performance/incremental-generator-baseline.md)
- [Focused sequence-array evidence](testing/sequence-array-mapping.md)
- [Retained 0.5.0 wire evidence](testing/wire-compatibility/evidence/0.5.0/README.md)
- [Historical Playground results](testing/evidence/playground-2026-10-27.md)

[Project README](../README.md) · [Security reporting](../SECURITY.md)
