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

## F4 investigation baseline

[F4 evidence](F4-EVIDENCE.md) compares Wireloom, RTI C#, and RTI Modern C++
across 12 union shapes, including negative integer labels and negative/mixed
enum values, plus two populated-copy controls. Windows and Linux C# observations
match for each implementation. At this baseline, both C# generators skipped
initialization for a null source; C++ has no null-source constructor equivalent and preserves all
tested default/ordinary-copy states. The evidence distinguishes parameterless
default policy from null-copy consistency and documents the scratch-only
compile adaptations needed for the probe.

Review of RTI CODEGENII-1550 and DDS-XTypes 1.3 Table 9 established the
parameterless integer discriminator difference as a separate conformance
defect (F10). The probe also exposed invalid negative enum default casts
(F9, CS0075). These findings and the null-source initialization gap were then
repaired together, as documented below. The investigation baseline had
475 runtime tests passing and two skips (F4/F5); those counts describe the
pre-repair state, not current validation. The temporary F9 opt-in input has
been promoted into the regular construction fixture.

## F4, F9, and F10 repair

Null-source union copies now use the parameterless constructor's initialization
through a shared emission helper. Non-null copies initialize only the copied
branch, avoiding a temporary default aggregate or collection. Parameterless
discriminators follow XTypes type defaults; default-branch setters retain
unused-label selection. Negative enum casts parenthesize their operands.
The F4 skip and F9 opt-in failure are replaced by regular runtime fixtures.

Validated October 15, 2026 with SDK 10.0.401 / .NET 10.0.12 on Windows and
SDK 10.0.400 / .NET 10.0.11 in the existing Linux container:

| Gate                                                | Result                                                                                                                          |
| --------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| Locked restores / Release builds                    | Passed                                                                                                                          |
| Full generator suite                                | 1,242 passed, no skips                                                                                                          |
| Runtime suite, Windows and Linux                    | 484 passed and one existing F5 skip on each platform                                                                            |
| Corpus compliance with export                       | 287 passed, no skips; includes seven constructor-default oracle comparisons                                                     |
| Fresh `0.3.0-f4` packed analyzer integration        | 17 passed                                                                                                                       |
| Original typed comparison probe, both platforms     | 12 null copies equal default instances; two populated controls preserve values; no Wireloom scratch adaptations                 |
| Maintained-source branch coverage                   | 4,713 / 5,012 (94.03%)                                                                                                          |
| Maintained-source line coverage                     | 6,942 / 7,010 (99.03%)                                                                                                          |
| Changed initialization/default-value methods        | 100% branch and line coverage for `EmitDefaultInitialization`, `FindDefaultDiscriminatorValue`, and `ManagedDiscriminatorValue` |
| Generated-fixture coverage, both platforms          | 1,211 / 1,942 branches (62.36%); 5,051 / 6,638 lines (76.09%); 435 classes                                                      |
| Generated report validation / Coveralls preparation | Passed; generator and both runtime reports normalize to existing sources                                                        |
| Community Qodana                                    | Exit 0; successful fresh SARIF invocation; zero findings; `Done`                                                                |
| Architecture memory validation                      | Current, no issues                                                                                                              |
| CI / Coveralls                                      | Pending push and workflow execution                                                                                             |

Commands use the existing root `coverage.settings.xml` and the README's runner
and verification scripts. Reports are under `coverage/f4-generator`,
`coverage/f4-runtime`, and `coverage/f4-runtime-linux`; normalized copies used
to verify Coveralls preparation are under `coverage/f4-coveralls-check`.
Generated branch coverage fell as the construction fixture added support,
metadata, and value APIs beyond the constructor paths being tested; this is
not complete API coverage. The maintained-source changed methods are fully
covered, and remaining generated gaps must still be reviewed against the
matrix. F5–F8 remain deferred, so #39 is not complete.

The fresh package was built with:

```powershell
dotnet pack src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj `
  --no-restore --configuration Release --output package-test-feed -p:PackageVersion=0.3.0-f4
dotnet restore tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --configfile tests/Wireloom.Dds.Generator.PackageIntegration/NuGet.Test.Config `
  -p:WireloomDdsGeneratorPackageVersion=0.3.0-f4
dotnet run --project tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --no-restore --configuration Release -p:WireloomDdsGeneratorPackageVersion=0.3.0-f4
```

Corpus export used `CORPUS_GENERATOR_EXPORT_ROOT=docs/corpus/generator` and the
normal Release corpus run. Qodana used the documented Community command with
`global.json` visible and separate Linux intermediates. The original comparison
probe ran against freshly exported Wireloom IDL output; its parsed Windows and
Linux observations match. No serialization or DDS wire claim is added.

## F4 wire regression run

On October 15, 2026, the existing exact-RTI-7.7.0 three-peer matrix was run
against the uncommitted F4/F9/F10 repair. All 72 selected positive cases were
attempted, scheduling 1,224 scenarios across nine directed peer pairings.
Four repository copies with independent build/scratch directories ran inside
one cached `wireloom-rti-7.7.0` container. Initial separate-container attempts
hit WSL mount failures; the single-container run avoided those startup errors.
The user-provided license was mounted read-only; no license data is retained
in reports. Orchestration and analysis used shell and file-based C#, without Python.

| Observation                               | Result                                                                                                      |
| ----------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| Original full attempt                     | 1,131 PASS, 75 FAIL, 18 NOT_RUN                                                                             |
| Readiness-related failures                | Six; startup text preceded `READY` on the same line, defeating the harness's `^READY ` check                |
| Current-source isolated readiness recheck | All 54 scenarios in the four affected cases passed                                                          |
| Focused pre-fix comparison                | 198 scenarios: 129 PASS and the same 69 remaining FAIL outcomes                                             |
| Status regressions versus pre-fix source  | Zero after the isolated readiness recheck                                                                   |
| Comparison with older two-peer archive    | All 372 archived scenarios matched current case/fixture/peer keys; zero regressions after readiness recheck |

The focused runs changed only readiness detection from `^READY ` to `READY `
in scratch harness copies. They did not change generated peers, fixture values,
or payload assertions. The pre-fix run substituted the two committed pre-repair
generator files from `a3e7136` (`UnionEmitter.cs` and `EmissionUnionPlan.cs`) into the same
source/harness snapshot. All remaining failing scenario identities and statuses
match that pre-fix run:

- 60 RTI C# reference-peer setup failures: 15 each for `07-union-octet`
  (CS0246 for `octet`), `07-union-ulong` and `07-union-uint32` (CS0221 for
  unsigned metadata labels), and `07-union-wchar-label` (RTI parser rejection).
- Five `07-union-char-boundaries` high-bit payload failures involving RTI C#.
- Four `05-array-of-sequences` cross-peer type mismatches, including the two
  registered expected Wireloom/C++ failures and the corresponding RTI C#/C++
  failures. Only two of the 69 failures are registered as expected in the
  current exclusion registry; all 69 were reproduced before the repair.

The 18 NOT_RUN scenarios are the nine pairings for each of
`04-unbounded-string-aliases` and `05-unbounded-sequence-aliases`. Their C++
harness setup selects a standalone string/collection typedef as a topic root;
RTI provides no applicable `dynamic_type<T>::get()` for those root types.
These cases need topic-capable aggregate wrapper fixtures in a separate
harness change. They do not establish failed DDS exchanges or a generator
wire regression.

The raw full attempt remains preserved. Two shard scripts exited on C++ setup
failure without publishing updated scenario rows. A file-based C# utility
recovered their 594 completed statuses from sanitized endpoint exit-code/setup
failure logs; missing exchanges remain NOT_RUN. The seeded type-name columns
for those shards remain empty rather than inferred. The normal merger rejects
incomplete runs, so the recovered report is explicitly partial and is not
presented as a successful combined matrix.

Local artifacts are under `artifacts/wire-compatibility-f4/`: original shard
logs, `partial/results.{json,md}`, `recheck-current/results.json`,
`recheck-before/results.json`, and `verification-summary.json`. The latter
checks exact remaining failure sets and both baseline comparisons. The older
archive predates several harness changes; the focused committed-source run
is the direct pre-fix control.

No wire regression attributable to the repair was observed in the existing
fixtures. This is not a fully green matrix or complete interoperability claim:
69 pre-existing failures and 18 blocked scenarios remain visible. Dedicated
negative-enum/null-copy wire fixtures were not added; their new API contracts
remain established by typed managed/native tests.
