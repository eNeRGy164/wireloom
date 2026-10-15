# Runtime contract evidence

## Initial detection baseline

Validated October 15, 2026 against RTI Connext DDS 7.7.0. The initial detection
change added tests and evidence without changing the generator or oracle sources.
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
contracts are deferred. At the initial detection stage, F5 was an ownership improvement proposal; the
subsequent retained RTI oracle confirms it is a compatibility defect. No serialization or DDS wire claims are added.

## F1 repair

The generator now imports `Omg.Types` for unbounded string/wstring typedefs and
resolves unbounded sequence typedefs to the existing effective limit of 100.
The managed attribute, native allocation, and plugin metadata share that limit.
Explicit bounds and bounded corpus output remain unchanged. The two original
F1 reproduction inputs now live under `Fixtures` and compile in every normal
runtime suite and fresh-package integration build.

Before the fix, both compile reproductions failed and 10 of the 16 new focused
generator cases failed. After the fix:

| Gate                                              | Result                                                                                    |
| ------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| Focused generator regressions                     | 16 passed                                                                                 |
| New runtime regression cases                      | 92 passed                                                                                 |
| Full generator suite                              | 1,227 passed                                                                              |
| Full runtime suite, Windows and Linux             | 463 passed, 4 existing explicit skips on each platform                                    |
| Corpus compliance with source export enabled      | 276 passed, no skips; no generated corpus diffs                                           |
| Fresh `0.3.0-f1` package integration              | 15 passed, including 2 new F1 regressions                                                 |
| Maintained-source branch coverage                 | 4,684 / 4,980 (94.06%)                                                                    |
| Maintained-source line coverage                   | 6,825 / 6,893 (99.01%)                                                                    |
| Generated-fixture branch coverage, both platforms | 810 / 1,132 (71.55%)                                                                      |
| Generated-fixture line coverage, both platforms   | 3,310 / 4,170 (79.38%)                                                                    |
| Generated report validation                       | 305 generated classes on each platform; positive branch counters; authored tests excluded |
| Community Qodana                                  | Exit 0; fresh SARIF; successful invocation; 0 problems; log ends with `Done`              |
| CI and Coveralls                                  | Pending push and CI                                                                       |

Coverage used the shared root settings and the documented commands with result
directories `coverage/f1-generator`, `coverage/f1-runtime`, and
`coverage/f1-runtime-linux`. The expanded fixture assembly includes unexercised
support/value methods and the existing F2/F3 formatting/support gaps; its lower
aggregate percentage reflects those additional generated sources. Generator
branch coverage increased from the initial detection baseline.

Package verification packed version `0.3.0-f1`, restored with the isolated
`NuGet.Test.Config` and explicit `WireloomDdsGeneratorPackageVersion`, built in
Release with that property, then executed the built test DLL. This unique
version avoids reusing an earlier cached package. No package was published.

The equivalent-case check also found the separate F7 string-alias-chain issue;
its opt-in reproduction is retained under `KnownFailures`. F2–F7 remain
follow-up findings. F1 adds no DDS wire compatibility claim.

## Retained unbounded typedef oracles

Cases C135/C136 now retain actual RTI-generated C# and Wireloom snapshots.
The typed reference probe passed 205 assertions and 88 native flag/shape
combinations on Windows and Linux. The expanded corpus passed 280 checks with
source export enabled and no skips. RTI confirms the F1 effective defaults,
succeeds for wide-string metadata, and deep-copies aggregate sequence elements
(F5 baseline). Its scalar sequence element API erases typedef wrappers, while
Wireloom retains them. See [commands and findings](../../docs/corpus/UNBOUNDED-ALIASES.md).
No additional serialization or DDS wire claim is made.

## F2 RTI comparison

A typed probe compiled the exact alias fixture through RTI Code Generator
4.7.0.1 and compared its formatting with freshly emitted Wireloom types.
On Windows (.NET 10.0.12) and Linux (.NET 10.0.11), RTI reproduced all 15 F2
failures; three struct/struct-alias controls formatted successfully. Wireloom
matched, with wide-string formatting blocked earlier by F3's metadata error.
These are 18 observations per implementation/platform, not passing formatting
contracts. The generator remains unchanged; [full evidence and commands](F2-EVIDENCE.md)
retain F2 as a desired API improvement beyond the observed RTI C# baseline.

## F2 repair and support-class layout

The original RTI C# comparison above is historical. F2 is now implemented:
managed aliases retain `Support.Instance.ToString(this)`, and their existing
support classes declare `public new string ToString(Alias sample)` for the
unsupported root kinds. Loops and escaping stay out of the managed data class.
Struct and union roots/elements retain their RTI formatter. Base-typed calls
and explicit print-format overloads retain RTI behavior; a separate runtime
characterization verifies nonvirtual base dispatch. Null samples throw
`ArgumentNullException`, while null underlying values format as `null`.

The original RTI factory, singleton, and support constructors are unchanged.
F3 is fixed as a prerequisite: wide-string aliases now use RTI's
`TypeKind.WideString` and `WideStringValue` annotations. Both metadata
visibilities, support initialization, and formatting pass regular tests.
The RTI-generated oracle remains unchanged.

Exact runtime checks cover all 15 original F2 shapes, null/empty values,
Unicode and string escaping, invariant numbers, nested sequences, three array
dimensions, the unbounded element families, original aggregate output, support
singleton identity, and the explicit-format overload's availability.
The unrelated F8 string-array native generic compilation failure is isolated
in `KnownFailures/string-array-aliases.idl`; no new string-array runtime
capability or DDS wire compatibility is claimed.
Final support-instance implementation validation:

| Gate                                              | Result                                                                   |
| ------------------------------------------------- | ------------------------------------------------------------------------ |
| Locked restores / Release builds                  | Passed                                                                   |
| Full generator suite                              | 1,240 passed                                                             |
| Runtime suite, Windows and Linux                  | 475 passed, 2 existing skips (F4/F5) on each platform                    |
| Corpus compliance with source export              | 280 passed, no skips                                                     |
| Fresh `0.3.0-f2-instance` package integration     | 16 passed                                                                |
| Maintained-source branch coverage                 | 4,711 / 5,010 (94.03%)                                                   |
| Maintained-source line coverage                   | 6,942 / 7,010 (99.03%)                                                   |
| New formatting emitter branch/line coverage       | 100% / 100% for both focused emitters                                    |
| Generated-fixture branch coverage, both platforms | 1,005 / 1,468 (68.46%)                                                   |
| Generated-fixture line coverage, both platforms   | 3,833 / 4,906 (78.13%)                                                   |
| Generated report validation                       | 341 generated classes; positive branch counters; authored tests excluded |
| Community Qodana                                  | Exit 0; successful SARIF invocation; 0 findings; `Done`                  |
| CI / Coveralls                                    | Pending push and workflow execution                                      |

Coverage uses the existing root `coverage.settings.xml`; results are under
`coverage/f2-generator`, `coverage/f2-runtime`, and `coverage/f2-runtime-linux`.
The new formatter emitters have full branch coverage. The expanded generated
fixture report includes additional support/value methods and repeated
formatting branches that the suite does not execute for every fixture type;
its aggregate percentage is not full API coverage. Review it against the
matrix and remaining findings rather than treating F2/F3 as completion of #39.
The expected native diagnostic in the base-dispatch characterization records
RTI's original failure; the generated support call in the same test succeeds.
