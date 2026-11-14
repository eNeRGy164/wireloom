# Contributing to Wireloom

This guide covers repository development and maintenance. For consumer setup,
see the [C# package guide](src/Wireloom.Dds.CSharp.Generator/README.md) or
[C++ package guide](src/Wireloom.Dds.Cpp.Generator/README.md).

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
checked-in NuGet lock files. The C# analyzer targets `netstandard2.0`; the
managed C++ development CLI targets `net10.0`. Consumer and verification
projects use the target frameworks documented in the architecture baseline.

Restore and build the generator:

```shell
dotnet restore src/Wireloom.Dds.CSharp.Compiler/Wireloom.Dds.CSharp.Compiler.csproj --locked-mode
dotnet restore src/Wireloom.Dds.CSharp.Generator/Wireloom.Dds.CSharp.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.CSharp.Compiler/Wireloom.Dds.CSharp.Compiler.csproj --no-restore --configuration Release
dotnet build src/Wireloom.Dds.CSharp.Generator/Wireloom.Dds.CSharp.Generator.csproj --no-restore --configuration Release
```

Run the fast unit tests and manifest-driven corpus compliance suite:

```shell
dotnet restore tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.CSharp.CorpusCompliance/Wireloom.Dds.CSharp.CorpusCompliance.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.CorpusCompliance/Wireloom.Dds.CSharp.CorpusCompliance.csproj --no-restore --configuration Release
```

For package integration changes, follow the isolated-feed restore and test
procedure in the [package integration guide](tests/Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md).
The repository uses Microsoft.Testing.Platform; pass test runner options after
`--` and use its `--filter-method`, `--filter-class`, `--filter-namespace`, or
`--filter-trait` options. Do not use the legacy `--filter` option.
For example, run the focused preprocessor review gate with:

```shell
dotnet run --project tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj `
  --no-build --configuration Release -- `
  --filter-namespace Wireloom.Dds.CSharp.Compiler.FrontEnd.Preprocessing.Tests
```

### Build the Modern C++ generator

From the repository root, build the managed development CLI and verify its
output against the retained corpus. This uses the repository SDK and does not
require an RTI SDK or license:

```shell
dotnet restore src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj --configuration Release --no-restore
dotnet src/Wireloom.Dds.Cpp.Generator/bin/Release/net10.0/Wireloom.Dds.Cpp.dll generate docs/corpus/idl/features/01-primitives.idl artifacts/cpp-generated
pwsh -NoProfile -File src/Wireloom.Dds.Cpp.Generator/Verify-Cli.ps1
```

The verifier checks the shared manifest against retained C++ exports and its
explicit generation/rejection cases, compares all four emitted documents,
and checks source-located diagnostics and a changed IDL default. It writes
`verification.json` under `artifacts/cpp-cli-check/<run>/` and fails on retained
source differences; review and qualify those differences before refreshing
corpus evidence. Defines and unbounded settings come from the shared manifest,
using the same include directories and RTI profile as the corpus export. The
[C++ generator tests](tests/Wireloom.Dds.Cpp.Generator.Tests/README.md) cover
CLI and filesystem behavior; the
[package integration checks](tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)
exercise the packed native tools. Use the package guide's
[CLI settings reference](src/Wireloom.Dds.Cpp.Generator/README.md#cli-and-build-settings)
when reproducing a case with preprocessing or per-file options.

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
- [Runtime contract tests](tests/Wireloom.Dds.CSharp.RuntimeContracts/README.md)
  compile focused IDL fixtures during the build and execute generated APIs
  directly against exact RTI 6.1.2, 7.3.1, and 7.7.0 profiles on Windows and Linux. Their behavior matrix tracks
  runtime contracts; Coveralls measures maintained compiler and generator
  source while excluding generated fixtures and test-project code.
  Keep known failures visible and distinguish API behavior from serialization
  and DDS wire evidence.

C++ follows the same test-layer layout as C#:

| Layer             | C++ suite                                                                                                             |
| :---------------- | :-------------------------------------------------------------------------------------------------------------------- |
| Compiler          | [In-memory API, planning, and source primitives](tests/Wireloom.Dds.Cpp.Compiler.Tests/README.md)                     |
| Corpus            | [Supported generation-corpus checks](tests/Wireloom.Dds.Cpp.CorpusCompliance/README.md)                               |
| Fuzzing           | [Copied C# IDL seeds with C++ compiler assertions](tests/Wireloom.Dds.Cpp.Fuzzing.Tests/README.md)                    |
| Generator package | [Native package consumption through CMake and MSBuild](tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md) |
| Generator         | [CLI, filesystem generation, and emitted surfaces](tests/Wireloom.Dds.Cpp.Generator.Tests/README.md)                  |

The native package layer uses PowerShell/CMake rather than a managed test
project. C++ corpus generation checks do not replace native SDK, CDR, or live
DDS qualification. The compiler, generator, and fuzzing projects are in the
main solution; corpus and package consumers remain independently runnable.

When adding or changing a corpus case, follow the detailed [corpus authoring
rules](docs/corpus/README.md). Keep each case focused on one feature, preserve
append-only case ordering for stable `C###` tags, update the manifest and
feature index, and respect the licensing rules for RTI-generated sources.
Exported C# sources belong under the matching profile's `generatorSourceRoot`.
Use the corpus guide's export command to refresh
`docs/corpus/7.7.0/generator/csharp/` or the earlier profiles' `generator/` folders.

## Choose validation

Start with the smallest focused check, then complete the applicable quality
gates before making the corresponding claim.

| Change                                             | Validation to select                                                                                                                                                                                 |
| :------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Compiler parsing, binding, validation, or emission | Compiler unit tests, both corpus profiles, generated-source exports, relevant runtime/package tests, coverage, and Qodana; wire checks for affected interoperability claims                          |
| Roslyn adapter or incremental inputs               | Generator unit tests and incremental baseline; fresh-package checks for consumer effects                                                                                                             |
| MSBuild targets or analyzer packaging              | [Package integration](tests/Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md), including minimum-host and negative diagnostic cases                                                        |
| Generated APIs or native lifecycle                 | [Runtime contracts](tests/Wireloom.Dds.CSharp.RuntimeContracts/README.md) and affected packed-consumer checks on applicable profiles/targets                                                         |
| Robustness or error boundaries                     | [Fuzzing](docs/testing/fuzzing.md), focused regressions, and applicable compiler/adapter checks                                                                                                      |
| Corpus, peer fixture, or report handling           | [LTS reproduction](docs/corpus/LTS-PROFILES.md), [wire harness checks](tests/Wireloom.Dds.Generator.WireCompatibility/README.md#maintainer-checks), and licensed exchanges when the claim needs them |
| Playground client or host                          | [Frontend checks](playground/README.md#build-and-checks), [browser qualification](docs/testing/playground-qualification.md), adapter/compiler checks for their affected boundaries                   |
| Publication                                        | [Package checklist](docs/releases/PREPARING-A-RELEASE.md) or [Playground runbook](docs/guides/playground-deployment.md)                                                                              |
| Documentation only                                 | Check local paths/anchors, Markdown, examples against current APIs/settings, dated claims, and architecture memory when source contracts change                                                      |

[Stryker](docs/testing/stryker.md) is additional advisory evidence. Test counts,
coverage, and report completeness do not replace behavioral assertions or prove
that every DDS exchange passed.

## Continuous LTS verification

The [Quality workflow](.github/workflows/quality.yml) checks the RTI 6.1.2,
7.3.1, and 7.7.0 corpus profiles. Its runtime/package matrix combines all three RTI packages,
.NET 8 and .NET 10 consumer targets, and Windows and Ubuntu. Each job tests a
freshly packed analyzer with C# 12 and retains its output. The pinned repository
SDK builds every cell; install .NET 8 alongside it for `net8.0` execution.
See the [verification scope](docs/guides/support-and-compatibility.md#continuous-lts-verification)
and the runtime/package guides for local commands and lock-file selection.
Licensed DDS wire qualification remains a separate manual workflow.

## Quality evidence

Generator-quality changes require successful build and relevant tests, branch
and line coverage review, Community Qodana, and a passing CI Coveralls check.
The existing C# compiler and combined C++ backend coverage checks use 90%
branches and 95% lines; Coveralls evaluates the uploaded reports. Follow the complete
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

### Review package READMEs

Read each package README as a consumer installing that exact version from
NuGet.org. Keep prerequisites, installation, application build integration,
configuration, generated API usage, troubleshooting, upgrade/rollback, and
consumer-relevant limits there. Keep repository builds, publishing commands,
test execution, CI host details, oracle comparisons, and adapter-review work
in maintainer documentation. Use the
[release checklist](docs/releases/PREPARING-A-RELEASE.md) before packing.

Use these sources when checking package documentation:

- [C# mapping and annotation review](docs/corpus/README.md#review-package-mapping-and-annotation-claims)
  owns the retained RTI/generated-source comparisons and annotation evidence.
- [Runtime contracts](tests/Wireloom.Dds.CSharp.RuntimeContracts/README.md) and
  [C# package integration](tests/Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md)
  establish API and package behavior. The minimum-host check uses SDK 8.0.100;
  the regular build uses the repository SDK. Record exact-source results in
  the release record instead of describing CI setup in the package README.
- [Native package checks](tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)
  and the [wire harness](tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
  own native installation, incremental builds, consumer execution, and peer
  reproduction. Keep generation, API, lifecycle, CDR, and live DDS claims
  separate when reviewing the release scope.
- [ADR-006](docs/architecture/adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md)
  and the [native qualification risks](docs/architecture/arc42/11-risks-and-technical-debt.md#116-modern-c-qualification-risks)
  own adapter decisions, private SDK dependencies, and platform findings.
  Apply the [RTI distribution and maintenance policy](docs/guides/support-and-compatibility.md#rti-distribution-and-internal-interfaces)
  when updating internal calls for a supported SDK. Summarize actionable limits for consumers in the
  exact-version release record and package guide.

### C# analyzer package

Pack the analyzer/source-generator package locally with:

```shell
dotnet restore src/Wireloom.Dds.CSharp.Generator/Wireloom.Dds.CSharp.Generator.csproj --locked-mode
dotnet pack src/Wireloom.Dds.CSharp.Generator/Wireloom.Dds.CSharp.Generator.csproj --no-restore --configuration Release --output artifacts
```

### Native C++ package

Publish each Native AOT executable on its matching host OS using the same
source revision and explicit package version. Choose a unique local version:

```powershell
$version = "1.1.0-local.$(Get-Date -Format yyyyMMddHHmmss)"
pwsh -NoProfile -File scripts/Publish-WireloomCppNative.ps1 -RuntimeIdentifier win-x64 -PackageVersion $version
```

On Linux x64, run the script with `-RuntimeIdentifier linux-x64` and the same
version. Assemble both hosts' `tools/` and `metadata/` outputs under
`artifacts/cpp-native-package/`, then pack:

```powershell
dotnet pack src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj --configuration Release --output artifacts/package-test-feed "-p:Version=$version" "-p:PackageVersion=$version"
```

The publish script retains build logs, warnings, and hashes under
`metadata/<rid>/`. Packing requires both native executables and rejects
managed assemblies, PDBs, deps files, and runtimeconfig files in `tools/`.
Run the [native package checks](tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)
on both hosts against the same package. The reusable
[C++ package workflow](.github/workflows/cpp-package.yml) automates these steps;
the publication workflows attest and publish the exact tested bytes.

### Preview and retained candidates

Preview NuGet packages use `https://nuget.pkg.github.com/eNeRGy164/index.json`.
Select an exact prerelease version and configure package-read credentials in
your user or CI credential store. Keep credentials out of source control.

For a retained preview candidate, download the publishing artifact and verify
its hashes before extracting the C++ package:

```bash
run_id='<publishing-run-id>'
version='<exact-prerelease-version>'
gh run download "$run_id" --repo eNeRGy164/wireloom \
  --name "wireloom-dds-generator-preview-$version" --dir candidate
(cd candidate && sha256sum --check SHA256SUMS)
mkdir -p "packages/Wireloom.Dds.Cpp.Generator.$version"
(
  cd "packages/Wireloom.Dds.Cpp.Generator.$version"
  cmake -E tar xf "../../candidate/Wireloom.Dds.Cpp.Generator.$version.nupkg"
)
```

Preview and native consumer artifacts expire after 14 days. Retain packages,
hashes, and provenance when needed longer. The standalone C++ package
workflow's `cpp-nuget` artifact is a build candidate; it does not indicate
feed publication or release qualification.

### Publication

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
