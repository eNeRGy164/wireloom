# Runtime contract detection evidence

Validated October 15, 2026 against RTI Connext DDS 7.7.0. This change adds tests
and evidence; generator implementation and existing oracle sources are unchanged.
Both suites use the shared root `coverage.settings.xml`. Revalidation after
consolidating the settings preserved the maintained-source coverage scope and
generated-fixture counters on Windows; Linux runtime coverage also matched.

| Gate                                               | Result                                                                                                                                                    |
| -------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Runtime project locked restore / Release build     | Passed                                                                                                                                                    |
| Windows runtime suite, SDK 10.0.401 / .NET 10.0.12 | 371 passed, 4 explicitly skipped                                                                                                                          |
| Linux runtime suite, SDK 10.0.400 / .NET 10.0.11   | 371 passed, 4 explicitly skipped                                                                                                                          |
| Full generator suite                               | 1,211 passed, 0 skipped                                                                                                                                   |
| Corpus compliance                                  | 275 passed, 1 expected export-only skip                                                                                                                   |
| Fresh 0.3.0 packed analyzer integration            | 13 passed                                                                                                                                                 |
| Maintained-source branch coverage                  | 4,685 / 4,982 (94.04%)                                                                                                                                    |
| Maintained-source line coverage                    | 6,822 / 6,890 (99.01%)                                                                                                                                    |
| Windows generated-fixture branch coverage          | 756 / 1,012 (74.70%)                                                                                                                                      |
| Windows generated-fixture line coverage            | 2,914 / 3,590 (81.17%)                                                                                                                                    |
| Linux generated-fixture coverage                   | Same branch and line counters as Windows                                                                                                                  |
| Generated fixture report validation                | 237 generated classes; all four module families present; positive branch counters; no authored test sources                                               |
| Coveralls report preparation                       | Passed against copies of generator, Windows, and Linux reports; all sources exist; both platform reports normalize to the same 186 generated source files |
| Community Qodana                                   | Exit 0; fresh SARIF; 0 problems; log ends with `Done`                                                                                                     |
| CI and Coveralls                                   | Pending push and CI; not established by these local runs                                                                                                  |

Commands for runtime execution and generated coverage are in [README.md](README.md).
The generator coverage command was:

```powershell
dotnet run --project tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj `
  --no-restore --configuration Release -- `
  --coverage --coverage-settings coverage.settings.xml `
  --coverage-output coverage.cobertura.xml --coverage-output-format cobertura `
  --results-directory coverage
```

Corpus compliance used the documented locked restore and Release `dotnet run`.
Package integration used a fresh local `dotnet pack`, an isolated-feed restore
with the checked-in `NuGet.Config` and `WireloomDdsGeneratorPackageVersion=0.3.0`,
and a Release executable test run. Community Qodana used the documented `wslc`
command with `global.json` visible and isolated Linux build outputs.

Linux execution uses the existing cached
`jetbrains/qodana-cdnet:2026.2-privileged` image with its entry point overridden
to `/bin/bash`; the test runs through `dotnet run`, not through InspectCode.
Generator and test `obj`/`bin` directories are mounted separately under
`.qodana/runtime-linux`, preserving Windows intermediates. The NuGet cache is
read-only. The workflow also defines Windows 2025 and Ubuntu 24.04 jobs; those
hosted CI runs remain pending.

All three opt-in compile reproductions failed independently as expected:
unbounded strings with CS0246, unbounded sequence with CS1525/CS0839, numeric
character labels with CS0266. The next normal build passed. Logs are retained
locally under ignored `coverage/` paths. They are findings, not passing runtime
contracts. Four skipped runtime tests retain F2–F5; skipped theories are counted
once by MTP even when they describe multiple rows.

The generated report remains below full coverage. It includes unused fixture
support/value methods and unresolved formatting/support paths, and does not
replace the named contract matrix. Required gaps are listed in
[known findings](README.md#known-findings); #39 remains incomplete while those
contracts are deferred. F5 is an ownership improvement proposal, not an assumed
compatibility defect. No serialization or DDS wire claims are added.
