# AGENTS.md

Repo-wide working rules for contributors to this repository.

## Mission

Help contributors make safe, consistent progress in this repository.

## Defaults

- Discover the current repo structure and conventions before making changes.
- Keep docs, contracts, and evidence aligned with affected changes.
- Do not add stacks, projects, or dependencies unless explicitly requested.
- Use Markdown for new documentation and PlantUML with `.puml` source files for diagrams.
- Use the repository-pinned .NET SDK and central package management for .NET projects.
- Keep package versions in `Directory.Packages.props`; the source-generator package is packable while test projects should opt out.

## Guidance

- Project-owned workflow guidance lives under `.agents/instructions/`.

## Git

- Use short, imperative, sentence-case commit subjects that state intent.
- Keep one logical concern per commit; prefer `rebase` and `autosquash` over merge or squash commits.
- Use `--fixup` for corrections to earlier branch commits and do not leave fixup commits in final history.

## Enforcement

Commit policy is documented, but no local hooks or CI enforcement are configured yet.

## .NET defaults

- Target `net10.0` with root namespace `Wireloom`.
- Use `.agents/architecture-memory.yaml` as the machine-readable source for current .NET defaults.

## Test filtering

- Test projects use Microsoft.Testing.Platform. Pass runner options after `--` when invoking `dotnet test`.
- Filter by method with `dotnet test <project> --no-restore -- --filter-method FullyQualifiedName~Namespace.Type.Method`.
- Use `--filter-class`, `--filter-namespace`, or `--filter-trait` for the corresponding broader filters.
- Do not pass the legacy `--filter` option directly to `dotnet test`; it is forwarded to the test executable and is not recognized by this runner.

## Test style

- Structure tests with explicit Arrange, Act, and Assert sections, using `// Arrange`, `// Act`, and `// Assert` comments even when a section is brief.
- Represent multiline IDL test bodies with C# raw string literals (`"""..."""`); use raw interpolated strings when test data must be inserted.

## Quality validation

- After a successful build and test run, validate the generator with Community Qodana from the repository root. The local Docker command must use the same linter image and fail threshold as CI:

  ```powershell
  $repoRoot = (Get-Location).Path
  $qodanaCache = Join-Path $repoRoot '.qodana\cache'
  docker run --rm `
    --volume "${repoRoot}:/data/project" `
    --volume "${qodanaCache}:/data/cache" `
    jetbrains/qodana-cdnet:2026.2-privileged `
    --cache-dir /data/cache `
    --results-dir /data/project/.qodana/results `
    --fail-threshold 0
  ```

- Keep `global.json` visible during Qodana. The repository bootstrap uses it to install and analyze with the pinned SDK; hiding it makes the result depend on whichever SDK happens to be in the image. The mounted Qodana cache stores analysis state and does not cache the SDK installation. If SDK bootstrap time becomes material, use a deliberately maintained Qodana image containing the pinned SDK rather than hiding `global.json`.
- CI explicitly enables the Qodana action cache at `${{ runner.temp }}/qodana/caches`; this is separate from the NuGet cache configured for the test job.
- Treat branch coverage as the primary coverage signal for control-flow-heavy generator code; line coverage remains a useful secondary signal. The Cobertura report must contain both rates and a positive `branches-valid` count. To inspect them locally after the coverage test:

  ```powershell
  [xml]$coverageReport = Get-Content -LiteralPath 'coverage\coverage.cobertura.xml'
  $coverageReport.coverage | Select-Object branch-rate, line-rate, branches-covered, branches-valid, lines-covered, lines-valid
  ```

- Coverage validation has three parts: confirm the local `dotnet test` coverage command succeeds, confirm the report contains branch coverage, and confirm the Coveralls GitHub check after CI completes. When branch tracking is enabled, Coveralls can include branch coverage in its aggregate; verify this in the Coveralls run details. Coveralls can fail a change when aggregate coverage is below the repository threshold or when coverage decreases beyond the configured decrease threshold, so a green local test run alone is not sufficient.
- Treat a successful Qodana run and a passing Coveralls check as required evidence before declaring a quality change complete.

## Architecture guardrails

Before proposing or implementing architecture-affecting changes:

- Read `.agents/architecture-memory.yaml` first when it exists.
- Open the arc42 chapters in `docs/architecture/arc42/`, linked decisions, and contracts when more detail is needed.
- Treat the arc42 chapters and linked source documents as the source of truth; refresh the derived memory after architecture changes.
- If a request conflicts with the architecture documentation, explain the conflict and propose a documentation, ADR, code, or combined change.

## Documentation conventions

- Documentation language: US English
- Documentation format: Markdown
- Preferred diagram tool: PlantUML
- Store PlantUML sources under `docs/architecture/arc42/images/<chapter>/`.
