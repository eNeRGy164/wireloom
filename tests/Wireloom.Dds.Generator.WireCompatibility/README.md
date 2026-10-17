# Wire compatibility peers

This project compares Wireloom-generated C# with RTI-generated C# and C++ DDS
peers for selected positive corpus cases. The matrix supports exact RTI
Connext 7.3.1 and 7.7.0 runs. The peer package version is selected by
`WireCompatibilityRtiVersion` (default `7.7.0`); use the matching RTI image,
NuGet package, and `rtiddsgen` toolchain. Exclusions and output paths are
version-scoped so evidence and expected failures do not carry across profiles.

The [current recorded evidence baseline](../../docs/corpus/FEATURE-COVERAGE.md#runtime-evidence-baseline)
identifies the main-branch run, exact revision, combined artifact, and scenario
totals. It includes both C137 and C141. Workflow success means the report was
collected and validated; failed scenarios and peer adaptations remain part of
the result.

Version-specific registries separate `notApplicableCases` from
`notImplementedCases`. A feature introduced after the selected RTI version is
not applicable: it contributes no exchanges, failures, or implementation gaps.
JSON and Markdown reports retain its case ID and reason separately. The
combined `09-autoid-hash` fixture requires `@must_understand` and is N/A on
7.3.1; explicitly requesting it is rejected before type-support discovery.

The nested `RtiOracleProbe` also supports typed, local API probes of retained
unbounded typedef oracles C135/C136. The corresponding managed contracts are
covered by the [runtime contract suite](../Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix).

Each case has a curated fixture catalog. Every fixture is sent in a separate
producer/consumer exchange with its own topic and report row across all nine
directed pairings of Wireloom C#, RTI C#, and RTI C++. Fixtures contrast optional presence per member,
empty/single/multiple sequence lengths, union branches, and string boundaries
where the IDL allows them. Readers compare the complete generated sample with
the fixture and report valid-sample and discovery counters. The JSON report
records case, fixture, pairing, endpoint runtime versions, and pass/fail status.
The Markdown report presents one row per case and fixture, with a status cell
for each pairing. For failures, its problem/defect column reports the observed
sanitized symptom and lists any registered expected-failure explanation
separately. The JSON keeps these in `observedProblem` and `expectedFailure`.

Wireloom-generated C# types and RTI-generated typed C# readers/writers exercise
generated conversions and DDS transport. The RTI C# peer is generated from the
same IDL with the pinned code generator, and generated files remain in scratch.
The RTI C++ reference peer normally uses `DynamicData`; typed generated C++ is
used where RTI omits DynamicData string template specializations. For present
optional wide-string sequence fixtures, the C++ peer uses DynamicData and RTI's
C API setters to avoid a crash in RTI 7.7.0's typed `std::wstring` sequence
serializer. Absent, empty, and narrow-only controls remain typed. All peers
request reliable delivery, and representation cases select XCDR2 explicitly.
The optional aggregate-member case selects `Holder` explicitly and uses typed
fixtures on both managed and C++ peers because RTI 7.7.0 DynamicData resolves
the nested typedef members against the wrong type. Readers compare the public
optional values and selected union branch directly.

## Recorded reference-peer findings

The named main-branch run includes RTI C# compilation failures in
`07-union-octet` (`CS0246`, generated `octet` discriminator type) and
`07-union-ulong` / `07-union-uint32` (`CS0221`, unchecked conversion missing
for the maximum unsigned label). The retained RTI oracle pairs reproduce these
compiler errors. Scenarios requiring those peers fail before DDS exchange;
the Wireloom C#/RTI C++ pairings still execute and pass for their fixtures.

`07-union-char-boundaries` compiles, but its maximum-character fixture fails
payload verification in all pairings involving RTI C#. The corresponding
Wireloom C#/RTI C++ controls pass. `07-union-wchar-label` cannot generate the
RTI C# reference peer; the passing Wireloom C#/RTI C++ profile uses the documented
manually constructed C++ DynamicType. These outcomes do not establish complete
three-peer interoperability for either case.

Consult the linked baseline and combined report for the exact revision,
fixtures, pairings, failure classification, and adaptations. An unexpected
report outcome is not by itself an attribution of a defect to Wireloom.

## Case shards and combined reports

This is the operational source of truth for wire-matrix sharding, peer
execution, artifacts, and report assembly. Architecture chapters summarize the
qualification boundary and link here for procedure.

The GitHub workflow runs four case shards concurrently (`fail-fast: false`).
Each shard loads its image through a cached Docker build. Package installation
and the .NET SDK installation are reused when the layer cache hits; only shard
zero exports the shared build cache. No shared-image job delays shard startup.
Every case and all its fixtures/pairings belong to exactly one shard. The
partitioner balances exchange counts deterministically, assigning larger
cases first; ties use case ID and shard index. No peer builds share a runner
or mutable output directory across shards.

Local execution remains unsharded by default. To select a shard, set both
`WIRE_COMPATIBILITY_SHARD_INDEX` (zero-based) and
`WIRE_COMPATIBILITY_SHARD_COUNT`. Apply the same optional
`WIRE_COMPATIBILITY_CASE_FILTER` to every shard. The selected set must contain
at least as many cases as shards. Separate local shard runs need independent
repository copies and output directories because case builds use shared
project `obj` and `bin` paths.

Each shard artifact contains `results.json`, `results.md`, sanitized `logs/`,
`scenarios.tsv`, `plan.json`, and `shard.json`. The report job requires all
shards, verifies their shared plan and RTI version, and rejects missing,
duplicate, or misassigned exchanges and inconsistent counts. Any not-run row
prevents publication of a complete combined report, including seeded rows
left by an interrupted C++ setup. Unexpected setup errors retain their nonzero
job exit even when a partial report is generated. Otherwise it regenerates
the combined report from validated rows and logs, preserving failure details,
adaptations, not-run outcomes, and the exclusion registry once. The combined
artifact is named `wire-compatibility-rti-<version>`; partial shard artifacts
use a `-shard-<index>` suffix. Setup failures fail the workflow even if another
shard completes; observed peer failures remain reported outcomes as before.
Only the combined report job publishes a workflow summary; shards retain
their evidence artifacts without publishing duplicate summaries.

The report job uses GitHub's official `actions/download-artifact` v8.0.2,
pinned to commit `9000827ccba6bdab643e8b6fd33ac0654aef8333`, to collect same-run
artifacts from the already used GitHub Actions publisher. It requires no RTI
secret and introduces no application or generator package dependency. The
existing weekly GitHub Actions Dependabot updates cover this action.

Validate partitioning and merging without an RTI license:

```powershell
dotnet build tests/Wireloom.Dds.Generator.WireCompatibility/scripts/matrix-tools.cs --output tmp/shard-tools
dotnet run --file tests/Wireloom.Dds.Generator.WireCompatibility/scripts/verify-shards.cs -- tmp/shard-tools/matrix-tools.dll
```

On Linux, also run `bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/verify-harness.sh`
to verify late setup errors keep their exit status while handled peer failures
remain reportable outcomes.

These checks compare sharded and unsharded JSON/Markdown and cover malformed
or incomplete artifacts. The same verification runs in the report job.

## Implementation map for C# contributors

Readers count lifecycle notifications (`ValidData=false`) separately from
payloads. Writer shutdown may deliver `NotAliveNoWriters` alongside valid data;
this does not fail an exchange. A reader still requires a valid sample and
checks its complete fixture. Lifecycle-only traffic times out, and payload
mismatches remain failures.

The independent verifier compares discriminator values using their declared
width, so RTI's signed label storage does not overflow or change a selected
branch. Character boundary fixtures use typed assignments and independent
expected values. The `07-union-char-boundaries` case now uses NUL and ASCII
DEL (U+007F). Focused local runs on both LTS versions passed 22 of 27 exchanges
with numeric labels in the RTI C++ scratch IDL. The five remaining failures are
default-branch exchanges involving RTI C#: the received sample selects the NUL
branch instead of fallback. An unadapted 7.7.0 run showed that RTI C++ also
needs numeric scratch labels for the escaped DEL source.

Wireloom's generated C# type is the implementation under test. The shell
runner builds the Wireloom C# peer, asks `rtiddsgen` for RTI C# and C++ reference
peers, and launches independent exchanges. Expected fixture assignments exist in
`FixtureCatalog.cs` and the native fixture helpers; keep values, optional
states, branch selection, and member paths aligned when changing a fixture.
`Native/main.cpp.in` owns the DynamicData C++ peer, while
`Native/typed-fixture-peer.hpp.in` handles typed C++ string cases that
DynamicData cannot populate.
`RtiOracleProbe/probe.sh` remains a focused diagnostic helper outside this
matrix.

The scenario total is computed from the fixture catalog and recorded as
`expectedScenarioCount` and `scenarioCount` in `results.json`. The human-readable
`results.md` uses distinct status cells for passes, adapted passes, expected and
unexpected failures, not-run scenarios, and not-implemented cases. It also
includes Adaptations and Problem / defect columns. Expected failures use ⚠️;
the problem column separates observed symptoms from their expected-failure
explanations, and `results.json` records both fields and their aggregate count.
Identical observed problems with the same expected-failure explanation are
grouped within each Markdown row, listing the affected pairings once alongside
one copy of the message. JSON retains the separate result for every exchange.
RTI C# generation, type-discovery, and compilation failures mark every pairing
that requires that peer as failed (⛔), with the failed setup stage and the first
compiler diagnostic when available. This records a confirmed peer setup defect;
it does not claim that a DDS exchange executed. Wireloom and C++ pairings still
run independently. Not-run status is reserved for exchanges without an observed
failure, such as seeded scenarios left incomplete by an interrupted job.
Cases that Wireloom cannot generate appear as not-implemented rows in both
reports. Their pairing cells show ⛔, while the problem column identifies the
diagnostic and links the tracked support issue. They are not counted as
executed DDS exchanges or as observed interoperability failures.
The IDL scenario cell uses `"` to repeat the case shown in the preceding row,
keeping fixture variations grouped without repeating the same filename.
Wireloom-positive selection is independent of RTI oracle status. Each run's
report records outcomes for the selected fixtures and pairings.

Case-specific RTI C++ code-generator requirements are configured on their
positive-case entries in `docs/corpus/manifest.json` under `wireMatrix`.
`rtiCppCodegenOptions` is passed only to that case's C++ `rtiddsgen` invocation.
The `qualification` note is copied into each affected scenario in `results.json`
and rendered in the Markdown report's **Case-specific RTI C++ generation**
section. The recursive aggregate and optional recursive array cases use
`-unboundedSupport`; RTI documents recursive-structure support as experimental,
so their results must be read with that qualification. This setting affects the
RTI C++ reference peer only; it does not change the IDL, Wireloom output, or RTI
C# peer generation.

`rtiCppIdlAdaptations` configures narrowly scoped, version-qualified changes to
the scratch IDL used only by RTI C++ generation. The 7.3.1
`07-union-char-boundaries` C++ peers use equivalent numeric char labels for
both LTS versions. The unadapted 7.7.0 C++ peer did not select the expected
branches for escaped DEL; wire reports identify the scratch-only adaptation.

For a C++ compilation sweep without DDS exchanges, set
`WIRE_COMPATIBILITY_CPP_COMPILE_ONLY=1` in the pinned RTI container and run
`scripts/run-matrix.sh`. The runner records each case in `cpp-compilation.tsv`
and continues after failures. A Wireloom C# build failure does not prevent the
independent RTI C++ compilation check. The current 7.3.1 sweep compiles 76 of
77 peers: 75 RTI-generated peers plus the custom DynamicType peer for
`07-union-wchar-label`. `09-autoid-hash` remains rejected because `rtiddsgen`
4.3.1 reports `@must_understand` as unsupported.

The recently added positive corpus cases C135–C141 have fixtures that exercise
their declared variations rather than default-only samples:

| Case                                           | Wire fixtures                                                                                                                      | Variation checked                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ---------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| C135 `04-unbounded-string-aliases`             | `string-empty`, `string-boundary`                                                                                                  | Empty narrow/wide aliases and 255-character narrow/wide values.                                                                                                                                                                                                                                                                                                                                                                                                       |
| C136 `05-unbounded-sequence-aliases`           | `sequence-empty`, `sequence-multiple`                                                                                              | Empty and two-element sequences across primitive, enum, aggregate, scalar-alias, narrow/wide, and bounded-string element aliases.                                                                                                                                                                                                                                                                                                                                     |
| C137 `07-union-char-numeric`                   | `union-127`, `union-default`                                                                                                       | A 127 numeric label and a default-label branch. RTI 7.7.0 accepts the IDL but its C# output assigns integer 127 to `char`, producing CS0266. For `union-127`, Wireloom C# and RTI C++ exchanges pass in both directions and RTI C++ self-exchange passes. Default-branch exchanges pass only within each implementation because the peers observe different discriminator values.                                                                                     |
| C141 `07-union-char-numeric-255`               | `union-255`, `union-default`                                                                                                       | The maximum `char` label and a default-label branch. RTI 7.7.0 accepts the IDL but its C# output assigns integer 255 to `char`, producing CS0266. The C++ peer uses unsigned-char compilation. In the current recorded main-branch run, Wireloom C# self-exchange passed for label 255, but RTI C++ pairings failed type discovery or value verification; default-branch exchanges passed only within each implementation because the union definitions have no common non-default label. |
| C138 `04-string-alias-chains`                  | `string-empty`, `string-boundary`                                                                                                  | Empty values and exact narrow/wide bounds through bounded and unbounded alias chains.                                                                                                                                                                                                                                                                                                                                                                                 |
| C139 `07-union-string-sequence-aliases`        | `union-direct-narrow`, `union-direct-wide`, `union-alias-narrow`, `union-alias-wide`, `union-bounded-narrow`, `union-bounded-wide` | Direct, alias, and bounded-alias branches with three sequence elements; bounded branches include strings at their declared length limits. Wireloom C# and RTI C++ pairings pass (24/24); the 30 RTI C# pairings fail with `FromDynamicData` `NullReferenceException`.                                                                                                                                                                                                 |
| C140 `07-union-string-sequence-bounded-direct` | narrow/wide `empty` and `multiple`                                                                                                 | Empty and three-element direct bounded sequence branches, including strings at the element bounds. All nine pairings pass.                                                                                                                                                                                                                                                                                                                                            |

Expected failures remain individual matrix outcomes with fixture and pairing
scope in the matching `wire-case-exclusions-<version>.json`; they do not suppress the exchange or turn
an observed peer failure into a pass. Check the generated report when an oracle
failure changes, because Wireloom self-coverage, RTI API behavior, and DDS wire
interoperability are separate evidence.

`05-array-of-sequences` is included in the matrix. DDSG0105 warns that its
generated C# binding flattens
the IDL array shape, and RTI rejects the resulting C# and C++ types during
cross-language discovery. The report marks those failures as expected; the
same-language controls remain useful evidence. This is an unsupported
Wireloom mapping rather than a negative IDL syntax case.
`09-optional-aggregate-member` includes absent and present fixtures for an
optional struct value and an optional union value whose selected branch
contains a nested payload, including typedef aliases. Wireloom generates these
values and the matrix schedules each fixture across all nine directed pairings.
Aggregate aliases use the underlying
generated struct or union type in managed APIs and native conversions, while
optional aggregate DynamicType metadata retains the declared typedef alias.
Expected `05-array-of-sequences`
failures involve Wireloom C# pairings because its generated binding flattens
the IDL array shape; RTI C# and C++ provide independent reference pairings. For
present wide-string fixtures in `09-optional-string-sequences`, the C++ peer
uses DynamicData and RTI's C API setters to exercise the same wire shape without
invoking RTI's crashing typed serializer. Absent, empty, and narrow-only
controls remain typed. The report identifies peer adaptations and expected
failures per scenario.
For the wide-character union, the
C++ peer uses a manually constructed RTI `DynamicType` that preserves the
corpus discriminator, labels, member IDs, and extensibility because
`rtiddsgen` cannot parse `wchar` union labels. For the C#-ignored
`@language_binding(FLAT_DATA)` annotation, C++ peer
generation drops only that annotation while preserving the type shape and
XCDR2 requirement; the result report records this adaptation. This verifies
standard C++ wire mapping, not RTI's FlatData-specific C++ layout. Consumers
verify received data against the expected fixture, not only endpoint
discovery.

To compare the retained RTI-generated C# binding directly, run
`RtiOracleProbe/probe.sh` inside the helper container with the repository
mounted at `/workspace`, `GITHUB_WORKSPACE=/workspace`, and the RTI license
mounted read-only at `/run/secrets/rti_license.dat`. It compiles the retained
RTI C# oracle sources and pairs them with an RTI-generated C++ peer. The probe
reports endpoint and discovery results for that control.

The runner writes `artifacts/wire-compatibility-rti-<version>/results.md` and
`results.json`; these generated files are local run output and are not
committed. The manually triggered GitHub Actions workflow uploads them as the
`wire-compatibility-rti-<version>` artifact, even when a build or exchange fails.
The manually triggered workflow runs all wire-testable
Wireloom-positive corpus cases and all nine same-version directed pairings,
with each fixture as an independent scenario, using the selected RTI LTS image.
Choose 7.3.1 or 7.7.0 from the workflow's `rti_version` input. Configure the repository secret
`RTI_CONNEXT_LICENSE` with the license contents before dispatching it. The
secret is written to a runtime-only file inside the job and excluded from
artifacts. The workflow retains the structured and Markdown results plus status-only
sanitized endpoint logs for 14 days. Each scenario result records its fixture
kind; C++ endpoint logs additionally record match counters and reader sample
counters.

For local 7.7.0 runs, build the helper image from the repository root:

```powershell
wslc build -f tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile `
  -t wireloom-rti-7.7.0 .
```

The workflow pins both RTI base images by digest: 7.3.1 uses
`rticom/connext-base:7.3.1@sha256:f33a61ec6381a8fb73cbb019fbb5c081f434dc5217eb14283fad995306be0f04`,
and 7.7.0 uses
`rticom/connext-base:7.7.0@sha256:efe4d35807a4aadaa59f0e84545d4c6b70328af5b0c0ca0498e63ecc1e16e1ca`.
Both install .NET 10.0.400 plus
the C++ build tools, including ccache. GitHub Actions builds this same image
in each shard with BuildKit's GitHub Actions cache, scoped to the selected RTI version,
so its package installation and .NET setup layers can be reused on later runs.
Each fresh runner still retrieves and loads the cached image layers. The image does
not contain a license. The shell harness uses the .NET 10 C# file-based utility
for manifest reading, type discovery, IDL include discovery, and report
generation; Python is not required. Peer-build and exchange failures are
recorded as failed or not-run rows, and the workflow succeeds if the report is
generated. Matrix utility setup and locked-restore failures preserve a nonzero
workflow status even when a report is available. Run the container
with the repository mounted at `/workspace`, the license mounted read-only at
`/run/secrets/rti_license.dat`, and `WIRE_COMPATIBILITY_CASE_FILTER=05-collections`
for the focused non-default fixture. Omit the filter to schedule all currently
selected positive cases. The unbounded string and sequence typedef cases use
aggregate wrapper roots so the C++ topic harness can build samples containing
those aliases. The string case has 18 successful exchanges across Wireloom C#,
RTI C#, and RTI C++. The sequence case exercises empty and two-element fixtures
across all sequence element families: Wireloom C# and RTI C++ exchanges pass,
while RTI C# combinations fail with `NullReferenceException` in
`FromDynamicData`. Managed API coverage is listed in the
[runtime contract matrix](../Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix).
Generated build outputs and artifacts are written to the mounted repository;
the runner accepts only exact 7.3.1 and 7.7.0 toolchains. Local runs set
`WIRE_COMPATIBILITY_RTI_VERSION` to select the profile and matching
`wire-case-exclusions-<version>.json` registry. The 7.3.1 registry starts with
no expected-failure classifications; record evidence before adding any.

## Build and discover one C# case

From the repository root:

```powershell
dotnet build tests/Wireloom.Dds.Generator.WireCompatibility/Wireloom.Dds.Generator.WireCompatibility.csproj `
  -c Release `
  -p:WireCompatibilityCase=01-primitives `
  -p:WireCompatibilityRtiVersion=7.7.0

dotnet run --file tests/Wireloom.Dds.Generator.WireCompatibility/scripts/matrix-tools.cs -- discover-cases `
  docs/corpus/manifest.json `
  tests/Wireloom.Dds.Generator.WireCompatibility/obj/wireloom-generated `
  tests/Wireloom.Dds.Generator.WireCompatibility/obj/cases.json `
  01-primitives
```

The utility discovers generated C# and corresponding C++ type names from
Wireloom output and the corpus manifest. The case filter can select one case
or all wire-testable positive cases.

## Run two local C# processes

Make the RTI license available to the process through `RTI_LICENSE_FILE`, then
start one peer with `--role reader` and another with `--role writer`. Give both
the same unique topic and pass the case, generated type name, and runtime
version. Both endpoints must exit successfully before recording a pass.

The matrix covers RTI Connext 7.7.0. Although the generator's runtime package
reference floor is 7.3.1, generated-code compatibility with that release is
not established because current output uses APIs absent from its C# package.

`09-optional-recursive-array` selects `Node` and runs `optional-absent`,
`optional-present`, and `optional-nested` fixtures (27 exchanges). Root value 9801,
child values 9811/9812, and nested child values 9821/9822 make element ordering
observable. Present arrays always contain two initialized children; leaves have
absent children. Managed readers inspect the complete tree independently, and
C++ readers compare generated typed values.

Recursive types do not expose public DynamicType. This case bypasses DynamicData
and disables TypeObject/TypeCode propagation on all peers because default RTI
7.7.0 TypeObject v2 rejects recursive types. Wireloom uses ordinary type-support
discovery and serializer creation. Only the RTI C# reference peer bootstraps its
plugin before accessing the singleton; scratch RTI C++ generation forwards the
visited-node chain for the recursive array TypeCode. Retained C# oracle sources
are unmodified. All 27 exchanges pass with ordinary Wireloom initialization.
Reports mark the remaining adaptations explicitly. Cold-start and serialization
checks are documented in the [runtime startup comparison](../Wireloom.Dds.Generator.RuntimeContracts/README.md#recursive-support-startup).
