# Contributing to Wireloom

This guide covers repository development and maintenance. For consumer setup, see the [package usage guide](src/Wireloom.Dds.Generator/README.md).

## Before changing code

- Read the root [`AGENTS.md`](AGENTS.md) for repository-wide working rules.
- Follow the [code style guide](docs/CODE-STYLE.md) for formatting,
  readability, API documentation, and maintainability conventions.
- Read [architecture memory](.agents/architecture-memory.yaml) before changing
  architecture. Consult the linked [arc42 chapters](docs/architecture/arc42/index.md),
  decisions, and contracts when the change affects documented boundaries.
- Keep compatibility statements aligned with the [feature coverage index](docs/corpus/FEATURE-COVERAGE.md)
  and [corpus guide](docs/corpus/README.md).
- Keep changes focused, explain the user-visible behavior, and include the
  evidence needed to review changes to generated output or compatibility.

## Development setup

Use the repository-pinned .NET SDK from [`global.json`](global.json) and the
checked-in NuGet lock files. The generator targets `netstandard2.0`; consumer
and verification projects use the target frameworks documented in the
architecture baseline.

Restore and build the generator:

```shell
dotnet restore src/Wireloom.Dds.Compiler/Wireloom.Dds.Compiler.csproj --locked-mode
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.Compiler/Wireloom.Dds.Compiler.csproj --no-restore --configuration Release
dotnet build src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --no-restore --configuration Release
```

Run the fast unit tests and manifest-driven corpus compliance suite:

```shell
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

For package integration changes, follow the isolated-feed restore and test
procedure in the [package integration guide](tests/Wireloom.Dds.Generator.PackageIntegration/README.md).
The repository uses Microsoft.Testing.Platform; pass test runner options after
`--` and use its `--filter-method`, `--filter-class`, `--filter-namespace`, or
`--filter-trait` options. Do not use the legacy `--filter` option.
For example, run the focused preprocessor review gate with:

```shell
dotnet run --project tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj `
  --no-build --configuration Release -- `
  --filter-namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests
```

## Tests and fixtures

The current test suites use xUnit v3 with Shouldly assertions on
Microsoft.Testing.Platform. Dependency versions are maintained in
[`Directory.Packages.props`](Directory.Packages.props).

Structure tests with explicit `// Arrange`, `// Act`, and `// Assert` sections.
Use C# raw string literals for multiline IDL test bodies.

Keep the test layers focused:

- Unit tests cover front-end and emitter behavior, usually with inline IDL.
- Corpus compliance tests load the shared manifest and check accepted,
  rejected, and generated-shape evidence.
- Package integration tests verify that the packed analyzer works in a
  consuming project.
- [Runtime contract tests](tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
  compile focused IDL fixtures during the build and execute generated APIs
  directly against both exact RTI 7.3.1 and 7.7.0 profiles on Windows and Linux. Their behavior matrix tracks
  runtime contracts; Coveralls measures maintained compiler and generator
  source while excluding generated fixtures and test-project code.
  Keep known failures visible and distinguish API behavior from serialization
  and DDS wire evidence.

When adding or changing a corpus case, follow the detailed [corpus authoring
rules](docs/corpus/README.md). Keep each case focused on one feature, preserve
append-only case ordering for stable `C###` tags, update the manifest and
feature index, and respect the licensing rules for RTI-generated sources.
Exported managed-generator sources belong under the matching version's
`docs/corpus/<version>/generator/` folder and should be refreshed using the
corpus guide's documented export command.

## Choose validation

Start with the smallest focused check, then complete the applicable quality
gates before making the corresponding claim.

| Change                                             | Validation to select                                                                                                                                                                                 |
| :------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Compiler parsing, binding, validation, or emission | Compiler unit tests, both corpus profiles, generated-source exports, relevant runtime/package tests, coverage, and Qodana; wire checks for affected interoperability claims                          |
| Roslyn adapter or incremental inputs               | Generator unit tests and incremental baseline; fresh-package checks for consumer effects                                                                                                             |
| MSBuild targets or analyzer packaging              | [Package integration](tests/Wireloom.Dds.Generator.PackageIntegration/README.md), including minimum-host and negative diagnostic cases                                                               |
| Generated APIs or native lifecycle                 | [Runtime contracts](tests/Wireloom.Dds.Generator.RuntimeContracts/README.md) and affected packed-consumer checks on applicable profiles/targets                                                      |
| Robustness or error boundaries                     | [Fuzzing](docs/testing/fuzzing.md), focused regressions, and applicable compiler/adapter checks                                                                                                      |
| Corpus, peer fixture, or report handling           | [LTS reproduction](docs/corpus/LTS-PROFILES.md), [wire harness checks](tests/Wireloom.Dds.Generator.WireCompatibility/README.md#maintainer-checks), and licensed exchanges when the claim needs them |
| Playground client or host                          | [Frontend checks](playground/README.md#build-and-checks), [browser qualification](docs/testing/playground-qualification.md), adapter/compiler checks for their affected boundaries                   |
| Publication                                        | [Package checklist](docs/releases/PREPARING-A-RELEASE.md) or [Playground runbook](docs/guides/playground-deployment.md)                                                                              |
| Documentation only                                 | Check local paths/anchors, Markdown, examples against current APIs/settings, dated claims, and architecture memory when source contracts change                                                      |

[Stryker](docs/testing/stryker.md) is additional advisory evidence. Test counts,
coverage, and report completeness do not replace behavioral assertions or prove
that every DDS exchange passed.

## Continuous LTS verification

The [Quality workflow](.github/workflows/quality.yml) checks both RTI 7.3.1 and
7.7.0 corpus profiles. Its runtime/package matrix combines both RTI packages,
.NET 8 and .NET 10 consumer targets, and Windows and Ubuntu. Each job tests a
freshly packed analyzer with C# 12 and retains its output. The pinned repository
SDK builds every cell; install .NET 8 alongside it for `net8.0` execution.
See the [verification scope](docs/guides/support-and-compatibility.md#continuous-lts-verification)
and the runtime/package guides for local commands and lock-file selection.
Licensed DDS wire qualification remains a separate manual workflow.

## Quality evidence

Generator-quality changes require successful build and relevant tests, branch
and line coverage review, Community Qodana, and a passing CI Coveralls check.
The compiler gates are 90% branches and 95% lines. Follow the complete
[quality procedure](docs/testing/quality.md) for commands, report freshness,
Qodana isolation, and result interpretation.

For Markdown changes, use the existing pinned Node runtime and run:

```shell
node docs/scripts/check-links.mjs
```

The dependency-free checker covers common inline/reference links, local images,
explicit/heading anchors, same-repository `main` links, and literal oracle capture
paths. It includes untracked Markdown during editing. It does not check external
sites or historical-revision URLs, or replace Markdown rendering review.

## Pull requests

- Describe the behavior or evidence change and the reason for it.
- Link relevant feature cases, diagnostics, or architecture decisions.
- Include the validation performed and any compatibility limits that remain.
- Investigate review feedback against the current source and tests before
  acting on it. Check related code for equivalent defects rather than fixing
  only the reported line. Keep fixes that are part of the PR's purpose in the
  PR; record intentional limitations and unrelated capability work as issues.
- Do not resolve a review conversation merely because a newer commit exists.
  Resolve it only after verifying that the relevant behavior and quality gates
  pass, and only when authorized to update GitHub.
- Keep commit subjects short, imperative, and sentence case. Use one logical
  concern per commit; repository commit guidance is in
  [`.agents/instructions/git-commit-workflow.md`](.agents/instructions/git-commit-workflow.md).

## Packages and releases

Pack the analyzer/source-generator package locally with:

```shell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet pack src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --no-restore --configuration Release --output artifacts
```

Preview packages are produced from `main` and published to GitHub Packages.
Release packages are published to NuGet.org from validated `v*.*.*` tags. The
preview and release workflows run unit, corpus, and package integration
validation, then produce package and SBOM attestations. See
[`publish-preview.yml`](.github/workflows/publish-preview.yml) and
[`publish-release.yml`](.github/workflows/publish-release.yml) for the
authoritative release process.

Follow the [release-preparation checklist](docs/releases/PREPARING-A-RELEASE.md)
and update the [release qualification records](docs/releases/README.md) for
each package. GitHub Releases and NuGet package readmes are immutable after
publication, so make the package README link to the stable record index before
tagging.
