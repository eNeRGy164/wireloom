# Maintainer quality procedure

Run commands from the repository root with its pinned SDK. The multiline
commands use PowerShell continuation syntax. For selecting the appropriate
test layers, see [Contributing](../../CONTRIBUTING.md#choose-validation).

For changes that affect generator quality, the repository requires a successful
build and relevant test runs, a successful Community Qodana run, and a passing
Coveralls check after CI completes. Coverage review must include branch coverage
as the primary signal for control-flow-heavy code and line coverage as a
secondary signal.

## GitHub Actions

[Quality](../../.github/workflows/quality.yml) runs compiler and generator
tests with coverage, fuzzing, corpus, wire harness self-tests, LTS consumer
checks, the published-package sample, and Playground verification. Independent
test jobs start after change classification. Browser coverage and Chromium
qualification consume the same Release WASM and bundle artifact on separate
runners. Coveralls waits for the .NET and browser coverage files. A main push
also retains the validated Pages candidate for the separate publisher.

## Test-layer ownership

Keep behavioral tests in the layer that owns the contract. The C# and C++
projects share the compiler, corpus, fuzzing, and package-consumer boundaries;
their generator layers differ because C# exposes a Roslyn analyzer while C++
exposes a command-line generator.

| Layer                   | C#                                                                                            | C++                                                                                        | Evidence owned by the layer                                                           |
| :---------------------- | :-------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------ |
| IDL front end           | [IDL tests](../../tests/Wireloom.Dds.Idl.Tests/README.md)                                     | Shared through compiler tests                                                              | Direct preprocessing, parsing, and shared front-end behavior                          |
| Compiler                | [C# compiler tests](../../tests/Wireloom.Dds.CSharp.Compiler.Tests/README.md)                 | [Compiler tests](../../tests/Wireloom.Dds.Cpp.Compiler.Tests/README.md)                    | Backend type mapping, emission behavior, diagnostics, and focused regressions         |
| Generator adapter / CLI | [Generator tests](../../tests/Wireloom.Dds.CSharp.Generator.Tests/README.md)                  | [Generator tests](../../tests/Wireloom.Dds.Cpp.Generator.Tests/README.md)                  | Roslyn discovery and incremental behavior for C#; CLI and filesystem behavior for C++ |
| Corpus                  | [Corpus compliance](../../tests/Wireloom.Dds.CSharp.CorpusCompliance/README.md)               | [Corpus compliance](../../tests/Wireloom.Dds.Cpp.CorpusCompliance/README.md)               | Manifest-driven profile behavior and retained oracle comparisons                      |
| Fuzzing                 | [Fuzzing tests](../../tests/Wireloom.Dds.CSharp.Fuzzing.Tests/README.md)                      | [Fuzzing tests](../../tests/Wireloom.Dds.Cpp.Fuzzing.Tests/README.md)                      | Bounded property checks and crash or invalid-output discovery                         |
| Package consumer        | [Package integration](../../tests/Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md) | [Package integration](../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md) | Fresh-package build and consumer integration                                          |
| Runtime contracts       | [Runtime contracts](../../tests/Wireloom.Dds.CSharp.RuntimeContracts/README.md)               | —                                                                                          | Generated managed runtime lifecycle and API contracts                                 |

These layers are separate evidence. Corpus, fuzzing, runtime-contract, and
package-consumer checks do not add counters to the unit-test coverage reports.
Use `Wireloom.Dds.CSharp.TestSupport` for genuinely shared C# test helpers; keep
domain assertions with the layer that owns them.

The workflow stays present on all PRs. For changes limited to the explicit
narrative Markdown allowlist, Quality skips product tests and still runs Qodana
and dependency review. A missing diff or unknown path runs the full suite.
Main pushes and manual runs also run the full suite.

After a hosted PR verifies the new gate, require **Quality result** in the
main ruleset in place of **Build and publish preview**. Keep CodeQL and Qodana
required during the transition. The preview workflow retains a skipped PR
check for the current ruleset. Remove that trigger after the ruleset migration.
Licensed wire checks run independently as described in the
[wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#github-actions).

## Local verification

After a successful build and test run, run Community Qodana from the repository
root with `global.json` visible. The command below reuses the host NuGet global
packages cache. Qodana's Linux restore outputs are kept in ignored directories
under `.qodana`; do not let them overwrite the Windows `obj` or `bin` folders.

The packaged Linux executable ends in `.Cpp`; treating that binary as C++
source can crash the inspection/report writer. The C++ project adds native
payload items only during packing so IDE and analysis hosts never load them
as project source. Native package generation and
consumer builds are verified by the
[C++ package checks](../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md).

```powershell
$nugetCache = Join-Path $env:USERPROFILE '.nuget\packages'
$qodanaObj = Join-Path $PWD '.qodana\obj'
$qodanaBin = Join-Path $PWD '.qodana\bin'
$qodanaCompilerObj = Join-Path $PWD '.qodana\compiler-obj'
$qodanaCompilerBin = Join-Path $PWD '.qodana\compiler-bin'
$qodanaIdlObj = Join-Path $PWD '.qodana\idl-obj'
$qodanaIdlBin = Join-Path $PWD '.qodana\idl-bin'
$qodanaCppObj = Join-Path $PWD '.qodana\cpp-obj'
$qodanaCppBin = Join-Path $PWD '.qodana\cpp-bin'
$qodanaCppCompilerObj = Join-Path $PWD '.qodana\cpp-compiler-obj'
$qodanaCppCompilerBin = Join-Path $PWD '.qodana\cpp-compiler-bin'
$qodanaBrowserObj = Join-Path $PWD '.qodana\browser-obj'
$qodanaBrowserBin = Join-Path $PWD '.qodana\browser-bin'
$qodanaCompilerTestsObj = Join-Path $PWD '.qodana\compiler-tests-obj'
$qodanaCompilerTestsBin = Join-Path $PWD '.qodana\compiler-tests-bin'
$qodanaIdlTestsObj = Join-Path $PWD '.qodana\idl-tests-obj'
$qodanaIdlTestsBin = Join-Path $PWD '.qodana\idl-tests-bin'
$qodanaCppGeneratorTestsObj = Join-Path $PWD '.qodana\cpp-generator-tests-obj'
$qodanaCppGeneratorTestsBin = Join-Path $PWD '.qodana\cpp-generator-tests-bin'
$qodanaGeneratorTestsObj = Join-Path $PWD '.qodana\generator-tests-obj'
$qodanaGeneratorTestsBin = Join-Path $PWD '.qodana\generator-tests-bin'
$qodanaFuzzingTestsObj = Join-Path $PWD '.qodana\fuzzing-tests-obj'
$qodanaFuzzingTestsBin = Join-Path $PWD '.qodana\fuzzing-tests-bin'
$qodanaRuntimeTestsObj = Join-Path $PWD '.qodana\runtime-tests-obj'
$qodanaRuntimeTestsBin = Join-Path $PWD '.qodana\runtime-tests-bin'
$qodanaTestSupportObj = Join-Path $PWD '.qodana\test-support-obj'
$qodanaTestSupportBin = Join-Path $PWD '.qodana\test-support-bin'
$qodanaWireSharedObj = Join-Path $PWD '.qodana\wire-shared-obj'
$qodanaWireSharedBin = Join-Path $PWD '.qodana\wire-shared-bin'
$qodanaWireRunnerObj = Join-Path $PWD '.qodana\wire-runner-obj'
$qodanaWireRunnerBin = Join-Path $PWD '.qodana\wire-runner-bin'
$qodanaWireVerifierObj = Join-Path $PWD '.qodana\wire-verifier-obj'
$qodanaWireVerifierBin = Join-Path $PWD '.qodana\wire-verifier-bin'
$qodanaCppCompilerTestsObj = Join-Path $PWD '.qodana\cpp-compiler-tests-obj'
$qodanaCppCompilerTestsBin = Join-Path $PWD '.qodana\cpp-compiler-tests-bin'
$qodanaCppFuzzingTestsObj = Join-Path $PWD '.qodana\cpp-fuzzing-tests-obj'
$qodanaCppFuzzingTestsBin = Join-Path $PWD '.qodana\cpp-fuzzing-tests-bin'
New-Item -ItemType Directory -Force -Path `
  $qodanaObj, $qodanaBin, $qodanaCompilerObj, $qodanaCompilerBin, `
  $qodanaIdlObj, $qodanaIdlBin, $qodanaCppObj, $qodanaCppBin, `
  $qodanaCppCompilerObj, $qodanaCppCompilerBin, `
  $qodanaBrowserObj, $qodanaBrowserBin, `
  $qodanaCompilerTestsObj, $qodanaCompilerTestsBin, `
  $qodanaIdlTestsObj, $qodanaIdlTestsBin, `
  $qodanaCppGeneratorTestsObj, $qodanaCppGeneratorTestsBin, `
  $qodanaCppCompilerTestsObj, $qodanaCppCompilerTestsBin, `
  $qodanaCppFuzzingTestsObj, $qodanaCppFuzzingTestsBin, `
  $qodanaGeneratorTestsObj, $qodanaGeneratorTestsBin, `
  $qodanaFuzzingTestsObj, $qodanaFuzzingTestsBin, `
  $qodanaRuntimeTestsObj, $qodanaRuntimeTestsBin, `
  $qodanaTestSupportObj, $qodanaTestSupportBin, `
  $qodanaWireSharedObj, $qodanaWireSharedBin, `
  $qodanaWireRunnerObj, $qodanaWireRunnerBin, `
  $qodanaWireVerifierObj, $qodanaWireVerifierBin | Out-Null

docker run --rm `
  --volume "${PWD}:/data/project" `
  --volume "${PWD}\.qodana\cache:/data/cache" `
  --volume "${qodanaObj}:/data/project/src/Wireloom.Dds.CSharp.Generator/obj" `
  --volume "${qodanaBin}:/data/project/src/Wireloom.Dds.CSharp.Generator/bin" `
  --volume "${qodanaCompilerObj}:/data/project/src/Wireloom.Dds.CSharp.Compiler/obj" `
  --volume "${qodanaCompilerBin}:/data/project/src/Wireloom.Dds.CSharp.Compiler/bin" `
  --volume "${qodanaIdlObj}:/data/project/src/Wireloom.Dds.Idl/obj" `
  --volume "${qodanaIdlBin}:/data/project/src/Wireloom.Dds.Idl/bin" `
  --volume "${qodanaCppObj}:/data/project/src/Wireloom.Dds.Cpp.Generator/obj" `
  --volume "${qodanaCppBin}:/data/project/src/Wireloom.Dds.Cpp.Generator/bin" `
  --volume "${qodanaCppCompilerObj}:/data/project/src/Wireloom.Dds.Cpp.Compiler/obj" `
  --volume "${qodanaCppCompilerBin}:/data/project/src/Wireloom.Dds.Cpp.Compiler/bin" `
  --volume "${qodanaBrowserObj}:/data/project/src/Wireloom.Playground.Browser/obj" `
  --volume "${qodanaBrowserBin}:/data/project/src/Wireloom.Playground.Browser/bin" `
  --volume "${qodanaCompilerTestsObj}:/data/project/tests/Wireloom.Dds.CSharp.Compiler.Tests/obj" `
  --volume "${qodanaCompilerTestsBin}:/data/project/tests/Wireloom.Dds.CSharp.Compiler.Tests/bin" `
  --volume "${qodanaIdlTestsObj}:/data/project/tests/Wireloom.Dds.Idl.Tests/obj" `
  --volume "${qodanaIdlTestsBin}:/data/project/tests/Wireloom.Dds.Idl.Tests/bin" `
  --volume "${qodanaCppGeneratorTestsObj}:/data/project/tests/Wireloom.Dds.Cpp.Generator.Tests/obj" `
  --volume "${qodanaCppGeneratorTestsBin}:/data/project/tests/Wireloom.Dds.Cpp.Generator.Tests/bin" `
  --volume "${qodanaCppCompilerTestsObj}:/data/project/tests/Wireloom.Dds.Cpp.Compiler.Tests/obj" `
  --volume "${qodanaCppCompilerTestsBin}:/data/project/tests/Wireloom.Dds.Cpp.Compiler.Tests/bin" `
  --volume "${qodanaCppFuzzingTestsObj}:/data/project/tests/Wireloom.Dds.Cpp.Fuzzing.Tests/obj" `
  --volume "${qodanaCppFuzzingTestsBin}:/data/project/tests/Wireloom.Dds.Cpp.Fuzzing.Tests/bin" `
  --volume "${qodanaGeneratorTestsObj}:/data/project/tests/Wireloom.Dds.CSharp.Generator.Tests/obj" `
  --volume "${qodanaGeneratorTestsBin}:/data/project/tests/Wireloom.Dds.CSharp.Generator.Tests/bin" `
  --volume "${qodanaFuzzingTestsObj}:/data/project/tests/Wireloom.Dds.CSharp.Fuzzing.Tests/obj" `
  --volume "${qodanaFuzzingTestsBin}:/data/project/tests/Wireloom.Dds.CSharp.Fuzzing.Tests/bin" `
  --volume "${qodanaRuntimeTestsObj}:/data/project/tests/Wireloom.Dds.CSharp.RuntimeContracts/obj" `
  --volume "${qodanaRuntimeTestsBin}:/data/project/tests/Wireloom.Dds.CSharp.RuntimeContracts/bin" `
  --volume "${qodanaTestSupportObj}:/data/project/tests/Wireloom.Dds.CSharp.TestSupport/obj" `
  --volume "${qodanaTestSupportBin}:/data/project/tests/Wireloom.Dds.CSharp.TestSupport/bin" `
  --volume "${qodanaWireSharedObj}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Shared/obj" `
  --volume "${qodanaWireSharedBin}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Shared/bin" `
  --volume "${qodanaWireRunnerObj}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Runner/obj" `
  --volume "${qodanaWireRunnerBin}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Runner/bin" `
  --volume "${qodanaWireVerifierObj}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Verifier/obj" `
  --volume "${qodanaWireVerifierBin}:/data/project/tests/Wireloom.Dds.Generator.WireCompatibility.Verifier/bin" `
  --volume "${nugetCache}:/root/.nuget/packages:ro" `
  -e NUGET_PACKAGES=/root/.nuget/packages `
  -e DOTNET_NOLOGO=1 `
  jetbrains/qodana-cdnet:2026.2-privileged `
  --solution /data/project/Wireloom.slnx `
  --configuration Release `
  --fail-threshold 0 `
  --results-dir /data/project/.qodana/results
```

The CI Qodana cache is separate from NuGet's cache. The mounted local cache
stores analysis state, not the bootstrapped SDK; keep `global.json` visible so
analysis uses the pinned SDK. The NuGet package cache is safe to share, but
restore and build intermediates are platform-specific and must remain isolated.

## Interpreting local Docker runs

When Qodana runs against the Windows bind mount through Docker Desktop,
InspectCode may repeat a `FileSystemTrackerImpl` warning that a watcher would
reach the “Drive root.” This warning is not by itself a failed scan. InspectCode
can continue analyzing and inspecting files, then write
`.qodana/results/qodana.sarif.json`.
Let the Docker command exit normally; do not interrupt it only because this
warning repeats or because output pauses while the scan is running.

Do not start another Qodana container while the first `docker run` is still
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
summary in `baseline-out.log` and check the Docker command's exit code; a
completed report can still contain findings, and CI uses a zero-finding
threshold. A report left by an earlier run is not evidence for the current
source.

## Coverage and scan scope

The local and CI configurations scan the committed `Wireloom.slnx`, which
contains the shared IDL front end, C# compiler, Roslyn generator, C++ CLI,
IDL, compiler and generator unit tests, C# and C++ fuzzing suites, runtime contract
tests, shared test support, and the wire Shared, Runner, and Verifier projects.
Loading the solution lets
InspectCode resolve the project references. The managed wire-peer and package-
integration consumers remain outside this solution and are verified separately.

Generate local Cobertura coverage after restoring the unit-test projects. The
commands match the CI coverage invocations and write isolated C# compiler,
Roslyn generator, shared IDL, and C++ backend reports:

```shell
dotnet restore tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Idl.Tests/Wireloom.Dds.Idl.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.CSharp.Fuzzing.Tests/Wireloom.Dds.CSharp.Fuzzing.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Cpp.Generator.Tests/Wireloom.Dds.Cpp.Generator.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Cpp.Compiler.Tests/Wireloom.Dds.Cpp.Compiler.Tests.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Cpp.CorpusCompliance/Wireloom.Dds.Cpp.CorpusCompliance.csproj --locked-mode
dotnet restore tests/Wireloom.Dds.Cpp.Fuzzing.Tests/Wireloom.Dds.Cpp.Fuzzing.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj `
  --no-restore `
  --configuration Release `
  -- `
  --results-directory coverage `
  --coverage `
  --coverage-settings coverage.generator.settings.xml `
  --coverage-output generator.cobertura.xml `
  --coverage-output-format cobertura
dotnet run --project tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj `
  --no-restore `
  --configuration Release `
  -- `
  --results-directory coverage `
  --coverage `
  --coverage-settings coverage.compiler.settings.xml `
  --coverage-output compiler.cobertura.xml `
  --coverage-output-format cobertura
dotnet run --project tests/Wireloom.Dds.Idl.Tests/Wireloom.Dds.Idl.Tests.csproj `
  --no-restore `
  --configuration Release `
  -- `
  --results-directory coverage `
  --coverage `
  --coverage-settings coverage.idl.settings.xml `
  --coverage-output idl.cobertura.xml `
  --coverage-output-format cobertura
dotnet run --project tests/Wireloom.Dds.Cpp.Generator.Tests/Wireloom.Dds.Cpp.Generator.Tests.csproj `
  -p:WireloomCppCombinedCoverage=true `
  --no-restore `
  --configuration Release `
  -- `
  --results-directory coverage `
  --coverage `
  --coverage-settings tests/Wireloom.Dds.Cpp.Generator.Tests/coverage.settings.xml `
  --coverage-output cpp.cobertura.xml `
  --coverage-output-format cobertura
```

The C++ command uses `WireloomCppCombinedCoverage=true` to link the compiler
and corpus test sources into an isolated coverage-only generator test host.
Normal builds of all three projects remain independent. One collector preserves
exact branch counters; binary/Cobertura merging is not used because the
installed merger can lose branch information. Fuzzing stays outside coverage.
The combined output retains the existing `cpp.cobertura.xml` name and 90%
branch / 95% line gates. Require a positive `branches-valid` count as well.

Run the independent suites with their normal project paths when investigating
compiler, corpus, or CLI failures; see [the C++ test-layer map](../../tests/Wireloom.Dds.Cpp.Generator.Tests/README.md).
Each Cobertura report must contain both branch and line rates and a positive
`branches-valid` count. Check its counters with:

```powershell
[xml]$coverageReport = Get-Content -LiteralPath 'coverage\compiler.cobertura.xml'
$coverageReport.coverage | Select-Object branch-rate, line-rate, branches-covered, branches-valid, lines-covered, lines-valid
```

The C# compiler and combined C++ backend reports retain their existing local
90% branch / 95% line checks. Coveralls evaluates the uploaded coverage reports
as the repository's aggregate coverage job. The Playground adapter has a
focused report check for its source class and positive branch/line counters so
a stale filter cannot yield an empty passing report. Fuzzing lives in its own
test project and runs without instrumentation; its purpose is to find crashes
and invalid generated code, not to contribute coverage:

```shell
dotnet run --project tests/Wireloom.Dds.CSharp.Fuzzing.Tests/Wireloom.Dds.CSharp.Fuzzing.Tests.csproj `
  --no-restore `
  --configuration Release `
  -- `
  --filter-class Wireloom.Dds.CSharp.Fuzzing.Tests.FsCheckIdlCompilerFuzzingSpecs
```

Coverage validation also requires the local coverage command to succeed and the
Coveralls GitHub check to pass after CI. Review Coveralls run details to confirm
branch tracking is included when enabled. A successful local test run alone is
not sufficient quality evidence.

When changing generator inputs or invalidation behavior, also run the focused
[incremental generator baseline](../performance/incremental-generator-baseline.md)
and update its evidence before considering a caching redesign.

For the bounded IDL compiler fuzzing gate, run the [fuzzing guide](fuzzing.md)
and preserve FsCheck's reported input and replay information when investigating a failure.

For mutation testing commands, shared timeout settings, focused batch configs,
and result interpretation, see the [Stryker guide](stryker.md).

[Documentation](../README.md) · [Contributing](../../CONTRIBUTING.md)
