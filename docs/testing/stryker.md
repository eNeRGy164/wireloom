# Stryker mutation testing

Wireloom uses Stryker.NET to check whether unit tests detect small, deliberate
changes to generator code. Mutation testing complements the unit-test and
corpus-compliance suites; it does not replace them.

## Prerequisites

Use the repository-pinned .NET SDK from [`global.json`](../../global.json).
The Stryker.NET 5.0.0 tool is pinned in
[`dotnet-tools.json`](../../.config/dotnet-tools.json). Restore the local tool
and the generator test project before running Stryker:

```shell
dotnet tool restore
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
```

Run commands from the repository root so Stryker finds the central
[`stryker-config.json`](../../stryker-config.json).

## Standard run

Run the full mutation campaign with:

```shell
dotnet stryker
```

The central config targets the generator project and its unit-test project,
uses the Microsoft.Testing.Platform runner, and writes HTML, JSON, Markdown,
and progress reports. The current mutation thresholds are zero because this campaign is
used to inspect and improve test strength; a score threshold is not a CI gate.
Stryker writes reports below `StrykerOutput/`.

## GitHub Actions workflow

The [Mutation testing workflow](../../.github/workflows/mutation.yml) is an
advisory, manually dispatched workflow. Open the repository's Actions tab,
select **Mutation testing**, and choose **Run workflow** for the branch to
analyze. It restores the pinned .NET SDK dependencies and Stryker tool, then
runs the central config against the complete generator. The workflow has a
six-hour job limit and appends Stryker's `mutation-report.md` to the run
summary. It also uploads `StrykerOutput/**/reports/*` as a `mutation-reports`
artifact, retained for 14 days; open `mutation-report.html` from that artifact
for per-mutant details. The workflow is not a required pull request check and
has no scheduled trigger.

Our timeout settings are:

- `additional-timeout`: 4000 milliseconds.
- `timeout-ratio`: 1.1 (Stryker requires a value greater than 1).

Stryker estimates each mutant's timeout from the measured test initialization
and the tests covering that mutant, applies the ratio, then adds the additional
timeout. These values shorten waits for non-terminating mutations while
allowing a small margin for normal test-run variation. A slow valid mutant can
still be classified as a timeout; inspect its test and report before treating
that status as a surviving defect.

## Focused batches

The full project can produce thousands of candidate mutations. For shorter,
focused runs, create a local Stryker config with a `mutate` filter for one
responsibility area. The whole-application partition below is a local workflow;
its per-scope configs are not checked into the repository. The existing
checked-in focused configs under [`.stryker/batches`](../../.stryker/batches/)
remain useful for their specific targets, but do not partition the whole
application. For a full local partition, keep the temporary scopes
non-overlapping so running all batches covers the same production source as
the central config.

Use these responsibility areas to divide the generator's production C# files:

| Local batch scope          | Production source patterns                                                                                                   |
| -------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| API and Roslyn integration | `**/Api/**/*.cs`, `**/Roslyn/**/*.cs`                                                                                        |
| Compiler core and naming   | `**/Compiler/Compilation*.cs`, `**/Compiler/GeneratedName.cs`, `**/Compiler/IdlCompilation.cs`, `**/Compiler/Naming/**/*.cs` |
| Preprocessing              | `**/Compiler/FrontEnd/Preprocessing/**/*.cs`                                                                                 |
| IDL parsing                | `**/Compiler/FrontEnd/IdlBuiltinTypeSyntax.cs`, `**/Compiler/FrontEnd/Parsing/**/*.cs`                                       |
| Semantic analysis          | `**/Compiler/FrontEnd/Semantic/**/*.cs`, `**/Compiler/FrontEnd/Symbols/**/*.cs`                                              |
| Emission planning          | `**/Compiler/Emission/Model/**/*.cs`, `**/Compiler/Emission/Planning/**/*.cs`                                                |
| Emission generation        | `**/Compiler/Emission/Emitters/**/*.cs`, `**/Compiler/Emission/Writers/**/*.cs`, `**/Compiler/Emission/*****.cs`             |

Together, these scopes cover all production C# files in the generator project.
When files are added or moved, update this map so every production file still
belongs to exactly one scope. Keep a local config's project, test project,
runner, mutation level, reporters, thresholds, and timeout values aligned with
the central [`stryker-config.json`](../../stryker-config.json); add only the
`mutate` patterns for the selected scope. Stryker treats the selected config as
the run configuration rather than inheriting the central config.

After restoring the pinned tool and test project, save the temporary config
outside the repository and pass its path to Stryker:

```shell
dotnet stryker --config-file C:/temp/wireloom-preprocessing.stryker.json
```

For example, a temporary preprocessing config contains the shared project,
runner, timeout, and reporting settings, with only that subsystem in its
`mutate` filter:

```json
{
  "stryker-config": {
    "project": "src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj",
    "test-projects": ["tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj"],
    "test-runner": "mtp",
    "additional-timeout": 4000,
    "timeout-ratio": 1.1,
    "mutate": ["**/Compiler/FrontEnd/Preprocessing/**/*.cs"],
    "mutation-level": "Basic",
    "reporters": ["html", "json", "progress"],
    "thresholds": {
      "high": 0,
      "low": 0,
      "break": 0
    }
  }
}
```

Run each scope separately so its report and surviving mutants are easy to
investigate. Each invocation writes a timestamped report directory under
`StrykerOutput/`. Use `--concurrency 2` when limiting parallel test sessions is
helpful; otherwise Stryker chooses a default based on available processors. A
batch should run the normal unit-test suite through Stryker's coverage
analysis, not a hand-selected test command.

## Reviewing results

Read the summary and JSON/HTML report for every run. Review killed mutants as
evidence that tests detect the changed behavior. Investigate survivors to
decide whether they reveal a missing assertion, an equivalent mutation, or an
intentional behavior that is not observable. Review timeouts and errors
separately; neither status proves that tests detected the mutation. Record the
target scope, counts by status, and any investigated survivors when reporting
mutation evidence.

For generator-quality changes, Stryker is additional focused evidence. Follow
the build, unit tests, corpus checks, branch coverage, Qodana, and CI/Coveralls
requirements in [`CONTRIBUTING.md`](../../CONTRIBUTING.md#quality-evidence).
