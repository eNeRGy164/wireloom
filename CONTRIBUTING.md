# Contributing to Wireloom

Thanks for helping improve Wireloom. This guide covers repository development
and maintenance. For consumer setup, see the [package usage guide](src/Wireloom.Dds.Generator/README.md).

## Before changing code

- Read the root [`AGENTS.md`](AGENTS.md) for repository-wide working rules.
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

```powershell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --no-restore --configuration Release
```

Run the fast unit tests and manifest-driven corpus compliance suite:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

For package integration changes, follow the isolated-feed restore and test
procedure in the [package integration guide](tests/Wireloom.Dds.Generator.PackageIntegration/README.md).
The repository uses Microsoft.Testing.Platform; pass test runner options after
`--` and use its `--filter-method`, `--filter-class`, `--filter-namespace`, or
`--filter-trait` options. Do not use the legacy `--filter` option.

## Tests and fixtures

Structure tests with explicit `// Arrange`, `// Act`, and `// Assert` sections.
Use C# raw string literals for multiline IDL test bodies.

Keep the test layers focused:

- Unit tests cover front-end and emitter behavior, usually with inline IDL.
- Corpus compliance tests load the shared manifest and check accepted,
  rejected, and generated-shape evidence.
- Package integration tests verify that the packed analyzer works in a
  consuming project.

When adding or changing a corpus case, follow the detailed [corpus authoring
rules](docs/corpus/README.md). Keep each case focused on one feature, preserve
append-only case ordering for stable `C###` tags, update the manifest and
feature index, and respect the licensing rules for RTI-generated sources.
Exported managed-generator sources belong under `docs/corpus/generator` and
should be refreshed using the corpus guide's documented export command.

## Quality evidence

For changes that affect generator quality, the repository requires a successful
build and relevant test runs, a successful Community Qodana run, and a passing
Coveralls check after CI completes. Coverage review must include branch coverage
as the primary signal for control-flow-heavy code and line coverage as a
secondary signal.

After a successful build and test run, run Community Qodana from the repository
root with `global.json` visible:

```bash
wslc run --rm --volume ${PWD}:/data/project --volume ${PWD}\.qodana\cache:/data/cache -e DOTNET_NOLOGO=1 jetbrains/qodana-cdnet:2026.2-privileged --results-dir /data/project/.qodana/results
```

The CI Qodana cache is separate from NuGet's cache. The mounted local cache
stores analysis state, not the bootstrapped SDK; keep `global.json` visible so
analysis uses the pinned SDK.

For local coverage, the Cobertura report must contain both branch and line
rates and a positive `branches-valid` count. Check its counters with:

```powershell
[xml]$coverageReport = Get-Content -LiteralPath 'coverage\coverage.cobertura.xml'
$coverageReport.coverage | Select-Object branch-rate, line-rate, branches-covered, branches-valid, lines-covered, lines-valid
```

Coverage validation also requires the local coverage command to succeed and the
Coveralls GitHub check to pass after CI. Review Coveralls run details to confirm
branch tracking is included when enabled. A successful local test run alone is
not sufficient quality evidence.

## Pull requests

- Describe the behavior or evidence change and the reason for it.
- Link relevant feature cases, diagnostics, or architecture decisions.
- Include the validation performed and any compatibility limits that remain.
- Keep commit subjects short, imperative, and sentence case. Use one logical
  concern per commit; repository commit guidance is in
  [`.agents/instructions/git-commit-workflow.md`](.agents/instructions/git-commit-workflow.md).

## Packages and releases

Pack the analyzer/source-generator package locally with:

```powershell
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
