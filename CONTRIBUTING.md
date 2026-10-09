# Contributing to Wireloom

Thanks for helping improve Wireloom. This guide covers repository development
and maintenance. For consumer setup, see the [package usage guide](src/Wireloom.Dds.Generator/README.md).

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

```powershell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --no-restore --configuration Release
```

Run the fast unit tests and manifest-driven corpus compliance suite:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

For package integration changes, follow the isolated-feed restore and test
procedure in the [package integration guide](tests/Wireloom.Dds.Generator.PackageIntegration/README.md).
The repository uses Microsoft.Testing.Platform; pass test runner options after
`--` and use its `--filter-method`, `--filter-class`, `--filter-namespace`, or
`--filter-trait` options. Do not use the legacy `--filter` option.
For example, run the focused preprocessor review gate with:

```powershell
dotnet run --project tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj `
  --no-build --configuration Release -- `
  --filter-namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests
```

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
root with `global.json` visible. The command below reuses the host NuGet global
packages cache. Qodana's Linux restore outputs are kept in ignored directories
under `.qodana`; do not let them overwrite the Windows `obj` or `bin` folders.

```powershell
$nugetCache = Join-Path $env:USERPROFILE '.nuget\packages'
$qodanaObj = Join-Path $PWD '.qodana\obj'
$qodanaBin = Join-Path $PWD '.qodana\bin'
New-Item -ItemType Directory -Force -Path $qodanaObj, $qodanaBin | Out-Null

wslc run --rm `
  --volume "${PWD}:/data/project" `
  --volume "${PWD}\.qodana\cache:/data/cache" `
  --volume "${qodanaObj}:/data/project/src/Wireloom.Dds.Generator/obj" `
  --volume "${qodanaBin}:/data/project/src/Wireloom.Dds.Generator/bin" `
  --volume "${nugetCache}:/root/.nuget/packages:ro" `
  -e NUGET_PACKAGES=/root/.nuget/packages `
  -e DOTNET_NOLOGO=1 `
  jetbrains/qodana-cdnet:2026.2-privileged `
  --results-dir /data/project/.qodana/results
```

The CI Qodana cache is separate from NuGet's cache. The mounted local cache
stores analysis state, not the bootstrapped SDK; keep `global.json` visible so
analysis uses the pinned SDK. The NuGet package cache is safe to share, but
restore and build intermediates are platform-specific and must remain isolated.

### Interpreting local WSLC runs

When Qodana runs against the Windows bind mount through WSLC, InspectCode may
repeat a `FileSystemTrackerImpl` warning that a watcher would reach the “Drive
root.” This warning is not by itself a failed scan. InspectCode can continue
analyzing and inspecting files, then write `.qodana/results/qodana.sarif.json`.
Let the WSLC command exit normally; do not interrupt it only because this
warning repeats or because output pauses while the scan is running.

Do not start another Qodana container while the first `wslc run` is still
active. If the command appears stalled, check `.qodana/results/log/` from a
second terminal and compare file timestamps with the scan start time. Progress
in the current `JetBrainsLog.*.inspectcode*.log` file means InspectCode is
still running. Use these commands to inspect the latest log activity:

```powershell
Get-ChildItem .qodana/results/log -File |
  Sort-Object LastWriteTimeUtc -Descending |
  Select-Object -First 5 Name, Length, LastWriteTimeUtc
Get-Content .qodana/results/log/code-inspection.log -Tail 20
```

After the command exits, confirm that `qodana.sarif.json` was updated during
this run and that `code-inspection.log` ends with `Done`. Read the final
summary in `baseline-out.log` and check the WSLC command's exit code; a
completed report can still contain findings, and CI uses a zero-finding
threshold. A report left by an earlier run is not evidence for the current
source.

The local and CI configurations target only
`src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj`. The wire
compatibility projects are not loaded by this Qodana scan, so excluding them
does not address WSLC watcher warnings or a local container stall.

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

When changing generator inputs or invalidation behavior, also run the focused
[incremental generator baseline](docs/performance/incremental-generator-baseline.md)
and update its evidence before considering a caching redesign.

For the bounded IDL compiler fuzzing gate, run the [fuzzing guide](docs/testing/fuzzing.md)
and preserve FsCheck's reported input and replay information when investigating a failure.

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
