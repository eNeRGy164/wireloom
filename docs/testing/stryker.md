# Stryker mutation testing

Wireloom uses Stryker.NET to check whether unit tests detect small, deliberate
changes to generator code. Mutation testing complements the unit-test and
corpus-compliance suites; it does not replace them.

## Prerequisites

Use the repository-pinned .NET SDK from [`global.json`](../../global.json).
The Stryker.NET 5.0.0 tool is pinned in
[`dotnet-tools.json`](../../.config/dotnet-tools.json). Restore the local tool
and both unit-test projects before running Stryker:

```shell
dotnet tool restore
dotnet restore tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
```

Run commands from the repository root so Stryker finds the central
[`stryker-config.json`](../../stryker-config.json).

## Standard run

Run the full mutation campaign with:

```shell
dotnet stryker
```

[`stryker-config.json`](../../stryker-config.json) targets both maintained
projects through the committed [`Wireloom.slnx`](../../Wireloom.slnx). Its
test-project list includes the dedicated compiler and generator unit-test
projects.
The advisory workflow runs one combined campaign using
Microsoft.Testing.Platform. Mutation thresholds are zero because this run
inspects test strength rather than gates CI. Stryker writes reports below
`StrykerOutput/`.

## GitHub Actions workflow

The [Mutation testing workflow](../../.github/workflows/mutation.yml) is an
advisory, manually dispatched workflow. Open the repository's Actions tab,
select **Mutation testing**, and choose **Run workflow** for the branch to
analyze. It restores the pinned .NET SDK dependencies and Stryker tool, then
runs the combined campaign against the adapter and compiler core. The workflow
has a six-hour job limit and appends the latest Stryker `mutation-report.md` to
the run summary. It also uploads `StrykerOutput/**/reports/*` as a
`mutation-reports` artifact, retained for 14 days; open `mutation-report.html`
from that artifact for per-mutant details. The workflow is not a required pull
request check and has no scheduled trigger.

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
its per-scope configs are not checked into the repository. Run a focused config
from the unit-test project directory so Stryker uses test-project context
instead of the root solution context. For a full local partition, keep the
temporary scopes non-overlapping so running all batches covers the same
production source as the central config.

Use these responsibility areas to divide production C# files. Compiler
responsibilities belong to `Wireloom.Dds.Compiler`; adapter responsibilities
belong to `Wireloom.Dds.Generator`.

| Local batch scope          | Owning project                   | Production source patterns                                                                                                   |
| -------------------------- | -------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| API and Roslyn integration | `Wireloom.Dds.Generator`         | `**/Api/**/*.cs`, `**/Roslyn/**/*.cs`                                                                                        |
| Compiler core and naming   | `Wireloom.Dds.Compiler`          | `**/Compiler/Compilation*.cs`, `**/Compiler/GeneratedName.cs`, `**/Compiler/IdlCompilation.cs`, `**/Compiler/Naming/**/*.cs` |
| Preprocessing              | `Wireloom.Dds.Compiler`          | `**/Compiler/FrontEnd/Preprocessing/**/*.cs`                                                                                 |
| IDL parsing                | `Wireloom.Dds.Compiler`          | `**/Compiler/FrontEnd/IdlBuiltinTypeSyntax.cs`, `**/Compiler/FrontEnd/Parsing/**/*.cs`                                       |
| Semantic analysis          | `Wireloom.Dds.Compiler`          | `**/Compiler/FrontEnd/Semantic/**/*.cs`, `**/Compiler/FrontEnd/Symbols/**/*.cs`                                              |
| Emission planning          | `Wireloom.Dds.Compiler`          | `**/Compiler/Emission/Model/**/*.cs`, `**/Compiler/Emission/Planning/**/*.cs`                                                |
| Emission generation        | `Wireloom.Dds.Compiler`          | `**/Compiler/Emission/Emitters/**/*.cs`, `**/Compiler/Emission/Writers/**/*.cs`, `**/Compiler/Emission/*****.cs`             |

Together, these scopes cover the maintained C# source in both production
projects.
When files are added or moved, update this map so every production file still
belongs to exactly one scope. Set each local config's `project` to the owning
project filename (for example, `Wireloom.Dds.Compiler.csproj`), and keep its
runner, mutation level, reporters, thresholds, and timeout values aligned with the central
[`stryker-config.json`](../../stryker-config.json). Add only the `mutate`
patterns for the selected scope. Stryker treats the selected config as the run
configuration rather than inheriting the central config.

After restoring the pinned tool and test project, change to
`tests/Wireloom.Dds.Compiler.Tests`, save the temporary config outside the
repository, and pass its path to Stryker:

```shell
dotnet stryker --config-file C:/temp/wireloom-preprocessing.stryker.json
```

For example, a temporary preprocessing config uses the test-project context,
the compiler project filename, and only that subsystem in its `mutate` filter:

```json
{
  "stryker-config": {
    "project": "Wireloom.Dds.Compiler.csproj",
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
