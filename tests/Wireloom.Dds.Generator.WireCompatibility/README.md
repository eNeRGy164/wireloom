# Wire compatibility tests

This project checks DDS interoperability for the wire-testable corpus. It runs
Wireloom-generated C#, RTI-generated C#, and RTI-generated C++ peers against
each other. It complements the [runtime contract suite](../Wireloom.Dds.Generator.RuntimeContracts/README.md),
which checks generated APIs and runtime behavior without claiming wire
compatibility.

The matrix targets the exact RTI Connext LTS releases 7.3.1 and 7.7.0. Each
fixture is an independent exchange across the nine directed writer/reader
pairings among the three peers, unless an explicit corpus case records a
narrower peer scope and its exclusion reasons. A reader must receive
a valid sample and match
the full expected fixture; discovery or lifecycle notifications alone do not
count as a pass. These runs provide evidence for the selected corpus cases,
not qualification of every feature or RTI release.

Version-specific registries separate `notApplicableCases` from
`notImplementedCases`. A feature introduced after the selected RTI version is
not applicable: it contributes no exchanges, failures, or implementation gaps.
JSON and Markdown reports retain its case ID and reason separately. For
example, `09-must-understand` (C142) is not applicable to 7.3.1; explicitly
requesting it is rejected before peer builds or type-support discovery.
`09-autoid-hash` (C040) independently checks hash member IDs on both versions;
C142 is exercised on 7.7.0 only. A green workflow means all planned exchanges
completed and the reports were combined. Individual pairings can still fail,
so inspect the combined report before making a profile-qualification claim.

## Run the matrix

### GitHub Actions

Manually dispatch the **Wire compatibility** workflow. It runs both RTI
versions in the same workflow. Each version has eight case shards and a separate
report job, summary, and combined artifact. Up to eight shard jobs per version
can run concurrently (16 total). The workflow requires the `RTI_CONNEXT_LICENSE`
secret.

The combined artifacts are named `wire-compatibility-rti-7.3.1` and
`wire-compatibility-rti-7.7.0`. Shard artifacts use the corresponding
`-shard-<index>` suffix and retain raw reference copies and staged IDL hash
mappings under `raw/`. Partial evidence is uploaded even when a shard fails;
the report job validates available evidence and retains a failing workflow
status for setup errors or incomplete exchanges.

The workflow shares version-scoped Docker build layers and restores C++
compiler outputs from GitHub Actions cache. The ccache key includes the runner
OS, RTI profile, actual `g++` version, shard index, and a hash of the wire
container definition, native runner sources, runner scripts, corpus manifest,
oracle manifests, and retained C++ oracle sources. The key stays stable across
workflow runs while those inputs are unchanged; restore prefixes allow
compatible prior caches to seed a changed key and retain compatibility with
the earlier key format during migration. NuGet packages use a separate cache
key based on the .NET SDK and package/project manifests. GitHub applies its
usual branch scope to these caches, so stable keys improve reuse within the
available scope but do not make caches globally shared.

### Container variants and local wire run

`Containerfile.dev` keeps the full RTI base, .NET SDK, and native build tools
for local development and debugging. Oracle capture continues to use the
version-pinned full RTI base directly through
[`Capture-RtiOracles.ps1`](../../docs/corpus/scripts/Capture-RtiOracles.ps1).

`Containerfile.wire` is the execution image used by GitHub Actions. It starts
from the pinned .NET 10 Noble SDK image and installs the RTI C/C++ runtime and
development libraries plus native build tools. It does not include Java or
`rtiddsgen`; wire peers come from the checked-in corpus. Package revisions
must match the selected RTI profile:

| RTI version | RTI package revision  |
| ----------- | --------------------- |
| 7.3.1       | `7.3.1.0-1-1bullseye` |
| 7.7.0       | `7.7.0.0-1-1bookworm` |

### Direct recursive sequence matrix fixture

The standard `06-aggregates` matrix case includes both its composed aggregate
fixture and a direct recursive sequence fixture. To run that case by itself
inside the matching RTI helper container:

```bash
export WIRE_COMPATIBILITY_RTI_VERSION=7.7.0 # Or 7.3.1
export WIRE_COMPATIBILITY_CASE_FILTER=06-aggregates
export WIRE_COMPATIBILITY_OUTPUT_DIR=/workspace/artifacts/recursive-sequence-matrix-$WIRE_COMPATIBILITY_RTI_VERSION
bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/run-matrix.sh
```

The fixture scopes its directed exchanges to Wireloom C# and typed RTI C++;
the other `06-aggregates` fixtures retain their existing peer matrix. RTI C++
source selection comes from the profile's `wire-oracles.json`, and the
profile-specific correction stays in the checked-in
`cpp/wire-adjusted/06-aggregates` source with its usual hashes and adjustment
manifest. Wire execution does not invoke `rtiddsgen`. The typed C++ peer and
Wireloom C# peer exchange a finite three-node
`CorpusAggregates.Recursive` value in both directions and verify values 6101,
6102, and 6103. Both participants set
`type_object_max_serialized_length=8192` and enable only
`SERVICE_REQUEST_CHANNEL`, so RTI 7.7.0 uses TypeObject v1. RTI 7.3.1's
TypeCode correction is already present in the retained adjusted source; see
[WORKAROUNDS.md](WORKAROUNDS.md#recursive-aggregate-qualification).

The direct recursive sequence member has a working typed C# / RTI C++ wire
path on both LTS versions with that documented discovery profile.
This qualification covers direct self-recursion only; mutually recursive
struct graphs remain unqualified.
RTI 7.7.0's default TypeObject v2 profile rejects recursive types at type
registration with `Strongly Connected Components (recursive types) are not
yet supported with TypeObjectV2`; setting
`WIRELOOM_RECURSIVE_TYPEOBJECT_PROFILE=v2` on the C# peer reproduces this
failure. RTI 7.3.1's default profile is TypeObject v1. This evidence covers
the direct sequence member only; recursive sequence typedef aliases remain
unsupported by Wireloom because RTI C++11 generation does not compile for
those IDL shapes.

For a complete local reproduction, use a clean Linux checkout of the recorded
revision; do not reuse Windows `obj`/`bin` directories or a Windows worktree's
`.git` pointer inside Linux. The PowerShell corpus capture/export recipes in
the [LTS runbook](../../docs/corpus/LTS-PROFILES.md) run directly on Windows.
From a clean Linux checkout with Docker available, build and run one profile:

```bash
version=7.3.1
package_version=7.3.1.0-1-1bullseye
# For 7.7.0 use package_version=7.7.0.0-1-1bookworm.
license_file="$HOME/rti_license.dat" # Keep the license outside the checkout.
[[ -f "$license_file" && -z "$(git status --porcelain)" ]] || exit 1
run_dir="$PWD/artifacts/wire-rti-$version-$(date -u +%Y%m%dT%H%M%SZ)"
mkdir -p "$run_dir"
git rev-parse HEAD > "$run_dir/source-commit.txt"
printf '%s\n' "$package_version" > "$run_dir/rti-package-version.txt"
date -u +%FT%TZ > "$run_dir/started-at.txt"
docker build --file tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile.wire \
  --build-arg "RTI_VERSION=$version" \
  --build-arg "RTI_PACKAGE_VERSION=$package_version" \
  --tag "wireloom-wire-$version:repro" \
  --iidfile "$run_dir/image-id.txt" \
  tests/Wireloom.Dds.Generator.WireCompatibility
status=0
docker run --rm --user 0:0 --workdir /workspace \
  --volume "$PWD:/workspace" \
  --volume "$license_file:/run/secrets/rti_license.dat:ro" \
  --env GITHUB_WORKSPACE=/workspace \
  --env RTI_LICENSE_FILE=/run/secrets/rti_license.dat \
  --env "WIRE_COMPATIBILITY_RTI_VERSION=$version" \
  --env "WIRE_COMPATIBILITY_OUTPUT_DIR=/workspace/${run_dir#"$PWD/"}" \
  --entrypoint bash "wireloom-wire-$version:repro" -lc \
  'eval "$(rtienv -l "$RTI_LICENSE_FILE")";
   dotnet --info > "$WIRE_COMPATIBILITY_OUTPUT_DIR/dotnet-info.txt";
   bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/run-matrix.sh' || status=$?
date -u +%FT%TZ > "$run_dir/completed-at.txt"
printf '%s\n' "$status" > "$run_dir/exit-code.txt"
# Keep the entire run directory, including failed/partial results and logs.
```

A dirty-tree run needs the actual source snapshot/patch and untracked inputs,
not just `git rev-parse HEAD`; the example deliberately uses a clean checkout.
Keep the provenance sidecars with the report, and record case filters, shard
settings, host/compiler versions, and any deviations from the runbook. Never
add license files or secret values to the evidence bundle. RTI package
revisions and the .NET base image are pinned; retain the image ID, tool
versions, and build logs when reproducing old results. A successful build or
capture does not imply that a licensed exchange ran.

Useful runner settings:

| Variable                              | Purpose                                                                           | Default                        |
| ------------------------------------- | --------------------------------------------------------------------------------- | ------------------------------ |
| `WIRE_COMPATIBILITY_RTI_VERSION`      | Selects the exact RTI NuGet package, exclusions, and toolchain expectation.       | `7.7.0`                        |
| `WIRE_COMPATIBILITY_OUTPUT_DIR`       | Directory for scenario reports, logs, and shard metadata.                         | `artifacts/wire-compatibility` |
| `WIRE_COMPATIBILITY_CASE_FILTER`      | Comma-separated corpus case IDs for a focused run.                                | All eligible cases             |
| `WIRE_COMPATIBILITY_SHARD_INDEX`      | Zero-based shard to execute. Set together with shard count.                       | `0`                            |
| `WIRE_COMPATIBILITY_SHARD_COUNT`      | Number of deterministic shards. Use the same count and case filter across shards. | `1`                            |
| `WIRE_COMPATIBILITY_CPP_COMPILE_ONLY` | Compile C++ peers without starting DDS exchanges when set to `1`.                 | `0`                            |
| `WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY` | Maximum concurrent case-specific Wireloom C# builds.                         | `4`                            |
| `WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY`   | Maximum concurrent native C++ builds; each build uses two compiler jobs.     | `2`                            |
| `WIRE_COMPATIBILITY_MAX_CONCURRENCY`  | Maximum independent exchanges active across all built cases in a shard.            | `16`                           |

Each shard needs its own working copy and output directory when run locally in
parallel. Case-specific managed outputs are isolated, but restore assets and
the bootstrap build use shared project paths. The selected case set must
contain at least as many cases as the shard count.

## Read the results

The runner writes `results.json` and `results.md`, plus `logs/`, `scenarios.tsv`,
`plan.json`, and `shard.json`. A complete run combines the shard reports into
one version-specific report. Use the JSON for structured outcomes and the
Markdown report for review. Its title identifies the RTI profile; shared
adaptations list all affected peers once. Technical terms use code spans, and
observed error bodies are quoted verbatim.

Each result records the case, fixture, writer and reader peers, runtime
versions, and outcome. The Markdown report summarizes pairings and includes
observed problems. Failed exchanges remain visible in both reports. Not-run,
not-implemented, and not-applicable cases remain distinct from observed
exchange failures.

The report merger checks that shards share the same plan and RTI version and
that their exchanges are complete, unique, and assigned to the correct shard.
Missing, duplicated, misassigned, inconsistent, or not-run exchanges prevent a
complete report. Peer setup failures are recorded and preserve a nonzero
workflow result even when partial reports are available.

The C# matrix utility owns fixture selection and creates the complete directed
exchange plan before builds begin. The case catalog is maintained in
`scripts/WireCompatibilityCaseCatalog.cs`; `matrix-tools.cs` includes it and
the planner from `scripts/WireCompatibilityScenarioPlanner.cs` using the pinned
.NET 10 SDK, without another project or package dependency. The coordinator
builds one Wireloom case first to warm shared project references, then builds
the remaining case peers concurrently. Each case has its own intermediate,
generated-source, and output directories; package restore assets and project
references are shared read-only. `WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY`
controls this queue. RTI C# setup and CMake configuration then prepare each
case, after which a bounded C# process queue builds native peers concurrently;
each CMake build uses two compiler jobs and
`WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY` controls the number of active
cases. The shared NuGet cache mounted by CI is used through `NUGET_PACKAGES`.

One C# exchange runner then draws from every built case in the shard, up to
`WIRE_COMPATIBILITY_MAX_CONCURRENCY` exchanges at once (16 by default). Each active
writer/reader pair leases one DDS domain ID from 20 upward; IDs are unique among
active exchanges and returned after both peers exit. The runner records the
domain ID and topic in the sanitized exchange log, checks both process exit
codes and `PASS` output, and continues after handled peer failures. Existing
case shards remain the outer build partition so independent case builds do not
share their scratch paths.

The runner starts each peer process with `RTI_MONITORING2_ENABLE=false`. RTI
Monitoring Library 2.0 is otherwise eligible for automatic activation in these
shared-library applications; its dedicated observability participant is not
used by the matrix and can contend for local discovery ports. This setting
disables the telemetry participant without disabling the peers' DDS
communication transports.

The four harness responsibilities are kept explicit: the catalog owns fixture
values and profile exceptions; the planner owns deterministic pairings and
whole-case shard assignment; the scheduler owns bounded reader-before-writer
execution and domain leases; the evidence verifier owns completeness,
uniqueness, profile, and report checks. The report verifier is shared with the
existing shard merger, so scheduling changes do not weaken evidence rules.

## Fixtures and peer setup

### Reliable-writer timing

All managed and native writers use explicit 50 ms `heartbeat_period`,
`fast_heartbeat_period`, and `late_joiner_heartbeat_period` settings with
`max_heartbeat_retries` set to 600, retaining a 30-second inactivity window.
RTI 7.3.1 defaults to 3-second heartbeat periods; RTI changed these defaults
to 200 ms in [7.6.0](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/release_notes/whats_new/whats_new_in_760.html#updated-default-heartbeat-periods-for-faster-repairs-of-lost-samples).
Explicit settings keep one-sample acknowledgment waits comparable across the
two LTS profiles. Writers still wait for reliable acknowledgments, and readers
still verify the complete payload before an exchange can pass.

### Case-specific fixture construction

Use case-specific typed sample construction and independent value checks for
new or revised wire cases. Share participant/writer/reader lifetime, matching,
timeouts, and report capture. Keep member assignments, optional presence, collection
contents, and union selection in the case's fixture code. Avoid reflection or
string paths where generated member access can make a wrong mapping fail compilation.
Exercise each builder and its independent verifier before starting DDS participants.
A fixture should fail with the case and member identified, rather than becoming
a reader-readiness timeout or an apparent vendor compatibility failure.

The existing harness still has a generic DynamicData fixture path. It is a test
convenience, not an independent oracle for generated defaults or a prerequisite
for using generated types. A fixture-building failure must be investigated as
a harness/API-path problem before it is attributed to DDS interoperability.

Managed case helpers are in `Fixtures/ManagedFixtures.*.cs`; C++ case-specific
construction is in `Native/typed-fixture-peer.hpp.in`. Typed sample construction
and topic wrappers are normal test setup and do not produce adaptation markers.
The [workaround guide](WORKAROUNDS.md) covers changes beyond that baseline.

### Annotation and value-binding cases

C163–C172 use typed fixtures in `ManagedFixtures.IdlGaps.cs` and the C++ peer.
Their sixteen fixtures produce 144 directed exchanges per exact RTI profile.
They exercise explicit values, constructor defaults, all declared branches in
the added unions, every fixed-array element, and exact-boundary strings.
C164 and C172 use distinctly named topic wrappers that include unchanged
canonical IDL and combine its declarations into one sample. Generation
provenance records the canonical inputs and wrapper/dependency hashes.

For C164, the runner reuses RTI C# outputs only when one source and one plugin
with the exact expected names share a directory and contain the expected IDL
types. It preserves them with hashes. If RTI emits two copies, it removes a
nested copy only when both generated files match the expected paths and are
byte-identical, preserving both raw files first. RTI C++ dependency traversal
is recursive: the runner reuses the unique RTI 7.7.0 outputs at their exact
relative paths, or stages the RTI 7.3.1 generated headers at those paths after
byte comparison. It keeps generated and staged copies with hashes. Scenario
endpoint metadata reports these harness adaptations.

For RTI 7.3.1 C#, `rtiddsgen` resolves these canonical includes by short
basename beside the wrapper. The runner supplies byte-identical wrapper,
canonical, and C164 dependency copies from an isolated scratch tree, then keeps
the staged inputs and source/staged hashes in the evidence. The canonical IDL
files in the snapshot remain unchanged.

The C171 type declares `inherited=7`, `overridden=9`, `text="alias"`,
`ratio=1.5`, and `single=1.25`. Its focused sparse case uses an older appendable
C++ DynamicData writer type containing only member ID 0 (`inherited`) and sends
`inherited=17101`. The full Wireloom C# reader supplies the declared defaults
for the four absent trailing members. This sparse schema is compatible through
RTI 7.7.0's member-annotation path; RTI 7.3.1 does not expose that metadata API.

The in-memory `ClearMember` probe is a separate runtime-contract check. On
RTI 7.7.0 it restores integral, narrow-string, and signed-zero defaults, but
fractional defaults are truncated and non-ASCII wide-string defaults are
returned as `??`. A separate runtime metadata check confirms the exact
fractional and wide-string annotation values on 7.7.0, and managed/native
initialization is checked on both profiles; the clear behavior is retained as
a profile limitation.

### DynamicData fixture API limits

These observations concern the fixture-building API, not the tested typed wire path:

- RTI `7.3.1` DynamicData exposes IDL `int8` as `Uint8`; negative `sbyte` assignment
  fails. Scalar fixtures assign generated members directly and readers check all values.
- RTI `7.3.1` DynamicData does not carry the member-default annotation used by C042.
  The fixture uses the generated constructor, assigns `value` and `ranged`, and leaves
  `color` untouched. The reader independently requires `BLUE` (`2`); setting it explicitly
  would hide a constructor-default defect.
- The tested C++ libraries do not link `DynamicData::value<std::wstring>` on either LTS.
  String and collection cases construct generated typed samples; this is ordinary C++
  usage and leaves their serializer and IDL unchanged.

The negative `int8` union discriminator issue [CORE-14660](#rti-731-negative-int8-union-discriminator)
affects the actual wire/serializer path; the report shows its observed failures.

### Recursive fixture metadata

RTI explicitly rejects public `TypeSupport.DynamicType` access for recursive types
with `NotSupportedException`; see the [documented C# limitation](https://community.rti.com/static/documentation/connext-dds/7.3.1/doc/manuals/connext_dds_professional/release_notes/known_issues/known_issues_with_dynamic_data.html).
Constructing a finite typed tree avoids that unsupported API and is normal fixture
setup. Absent, present, and nested fixtures still require independent checks of every node.
Generated plugin initialization, TypeCode traversal, discovery limits, and writer memory
are separate adjustments documented in the workaround guide.

### Reference generation settings

RTI C# and C++ peers are selected from the checked-in corpus and copied into
scratch build directories. Wire execution does not invoke `rtiddsgen`. Wrapper
specific peers and known corrections have dedicated corpus folders with input
and generated-source hashes. Some cases record reference generation settings
in `docs/corpus/manifest.json`:

- `rtiCppCodegenOptions` records options used to capture that case's RTI C++
  oracle. Any documented qualification, such as RTI's experimental
  recursive-structure support, is copied into scenario evidence.
- `rtiCppIdlAdaptations` records changes used only when capturing the RTI C++
  oracle. They do not change the corpus IDL or Wireloom inputs.

The exclusion registries `wire-case-exclusions-7.3.1.json` and
`wire-case-exclusions-7.7.0.json` record cases that are not applicable to each
RTI profile. They do not classify exchange failures.

### Numeric character-union expected values

Both managed and C++ peers require the same discriminator and selected payload:

| Case                        | Fixture         | Discriminator | Selected member | Payload |
| --------------------------- | --------------- | ------------: | --------------- | ------: |
| `07-union-char-numeric`     | `union-127`     | 127           | `value`         | 13701   |
| `07-union-char-numeric-255` | `union-255`     | 255           | `value`         | 13702   |
| Both cases                  | `union-default` | 65 (`'A'`)    | `fallback`      | 13703   |

The C++ default fixture explicitly selects 65 after assigning its fallback value;
its generated constructor otherwise retains 0. Readers check these fixed values
independently of fixture construction and generated equality. Selecting the fixture's
discriminator is ordinary sample construction.

### Reproducible RTI character-binding workarounds

See [numeric character unions](WORKAROUNDS.md#numeric-character-unions) for the
failing generated code, guarded post-generation commands, compiler settings,
and exact discriminator/payload checks.

### Recursive-array writer memory

For `09-optional-recursive-array`, all three peers set the writer property
`dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size`
to `4096` bytes on both LTS versions. RTI 7.3.1 otherwise attempts to
preallocate about 64 GiB from the recursive type's maximum serialized size,
preventing writer creation before any sample is sent. The threshold controls
fast-pool allocation; it does not cap serialized sample size, and larger
samples can allocate separately. Reader QoS and fixture values are unchanged.

JSON endpoint adaptations and the Markdown Adaptations column disclose this
case-specific writer policy alongside the existing recursive metadata
adaptations. This harness configuration does not modify generated bindings.

### RTI 7.3.1 negative int8 union discriminator

`07-union-int8` remains valid positive IDL. Its `union-minimum` fixture uses
`-128` and has eight failing pairings on RTI 7.3.1. RTI stores the negative
discriminator as an unsigned
value, causing managed DynamicData branch lookup failures and interpreted C#
serializer payload loss. RTI documents this as
[CORE-14660, fixed in 7.6.0](https://community.rti.com/static/documentation/connext-dds/7.7.0.1/doc/manuals/connext_dds_professional/release_notes/whats_fixed/760/fixes_dynamic_data.html).
The observed failures are documented in
[wire run 39131568513](https://github.com/eNeRGy164/wireloom/actions/runs/39131568513)
at revision `3e9450f2` and focused serializer probes against both the unchanged
RTI oracle and Wireloom output.

The observed failures apply only to `union-minimum` and the affected pairings.
This runtime defect does not make the feature not applicable.

## IDL sequence-array corpus case

`05-array-of-sequences-idl` uses a separately named IDL file with the same
declarations as C018 and `DdsIdlSequenceArrayMapping=Idl`. It is an ordinary
corpus case, so default C018 generation and results remain unchanged. Wireloom
C# and unmodified RTI C++ run all four directed pairings
for each fixture:

| Fixture                | Values checked                                                                                         |
| ---------------------- | ------------------------------------------------------------------------------------------------------ |
| `sequence-empty`       | Both array positions hold empty numeric and string sequences.                                          |
| `sequence-unequal`     | Numeric lengths are two/one; string lengths are one/two, with distinct payloads.                       |
| `sequence-mixed-empty` | An empty numeric sequence precedes a populated one; a populated string sequence precedes an empty one. |
| `sequence-boundary`    | Signed numeric limits, a 16-character string, an empty string, and the string sequence bound of three. |

The managed reader checks every position and sequence against literal expected
values without generated equality. The typed C++ reader compares the complete
independently populated generated sample. Each profile plans 16 exchanges.
The direct-form RTI C# peer is explicitly excluded because it flattens the
array-of-sequences type; its five pairings show N/A with the reason in the
report. This does not qualify Java, Python, typedef-based C# peers, or all
possible sequence-array shapes.

The runner rebuilds each case into a separate generated-source directory.
`logs/<case>-generation.json` records the selected mode, IDL, exact runtime,
and SHA-256 hashes of the IDL and generated source files. C149 scenarios retain
this provenance in `results.json`, including after shard merging.
Missing or mismatched provenance prevents completed exchanges from being
published as valid evidence.

To run only the dedicated case inside the licensed helper container:

```bash
export WIRE_COMPATIBILITY_CASE_FILTER=05-array-of-sequences-idl
export WIRE_COMPATIBILITY_RTI_VERSION=7.7.0 # Repeat with 7.3.1.
bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/run-matrix.sh
```

## Maintainer checks

These checks validate shard planning, artifact merging, and harness exit
behavior without an RTI license:

```powershell
dotnet build tests/Wireloom.Dds.Generator.WireCompatibility/scripts/matrix-tools.cs --output tmp/shard-tools
dotnet run --file tests/Wireloom.Dds.Generator.WireCompatibility/scripts/verify-shards.cs -- tmp/shard-tools/matrix-tools.dll
```

On Linux, also run:

```bash
bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/verify-harness.sh
```

`verify-shards.cs` compares sharded and unsharded report output and exercises
invalid or incomplete shard sets. `verify-harness.sh` checks that unexpected
setup errors keep their exit status while handled peer failures remain
reportable outcomes. It also checks that a wrapper's generated files are not
mistaken for an included IDL's files when their basenames match, while incomplete
or duplicated dependency output remains visible to the guards. The GitHub
report job runs these checks before merging.

### Concurrent DDS-domain spike

The experimental `scripts/concurrent-domain-spike.cs` compares two managed
exchanges sequentially and concurrently. Each pair uses one topic, distinct DDS
domains, and fixtures that make the reader validate the payload received on its
domain. The expanded Docker run used RTI 7.7.0 C# reference peers:

| Case | Exchange fixtures | Sequential | Concurrent | Speedup |
| --- | --- | ---: | ---: | ---: |
| `01-boundaries` | `non-default` / `non-default` | 3.329 s | 1.050 s | 3.17× |
| `04-strings` | `string-empty` / `string-boundary` | 3.026 s | 0.977 s | 3.09× |
| `05-collections` | `sequence-empty` / `sequence-single` | 3.285 s | 1.590 s | 2.06× |
| `07-unions` | `union-number` / `union-text` | 2.497 s | 1.178 s | 2.12× |

Each exchange passed. Across these four single runs, elapsed time summed to
12.137 seconds sequentially and 4.795 seconds concurrently (2.53×). Treat this
as a smoke-level comparison rather than a stable benchmark: there was no warmup
or repeated trial, and each row includes process startup, discovery, and a
one-sample exchange. The selected RTI C# peer sources compiled with zero
warnings and errors. These results demonstrate domain isolation across
primitive, string, sequence, and union payloads, but do not qualify Wireloom
serialization, native C++, sustained throughput, or the complete matrix.

The aggregate case was not counted. The recursive-sequence fixture overflowed
during RTI plugin initialization, and the inherited `Derived` fixture did not
match the RTI type's member layout. That case needs an aggregate-specific
fixture setup before it can provide useful evidence. A separate Wireloom peer
build also hit duplicate generated definitions before DDS startup.

The production harness now uses bounded domain scheduling for every planned
exchange. The smoke results above do not replace a full licensed run of both
profiles and do not qualify the complete matrix.

### Full-matrix throughput validation

On 2026-10-27, the complete RTI 7.7.0 matrix ran on the pinned Docker image
with eight domain-isolated exchanges active per scheduler. All 1,555 exchanges
were recorded: 1,549 passed and the same six known interoperability failures
remained. Elapsed time was 11m45s after sharing project references, compared
with 13m17s at concurrency eight without reference reuse and 18m35s in the
preceding single-shard run at concurrency four. Representative shard 0/8 runs
passed all 198 exchanges in 2m00s before reference reuse and 1m46s afterward.
A 7.3.1 shard 0/8 run completed in 1m37s with 191 exchanges recorded: 181
passed, 10 failed, and none were unrun. These are single-host measurements; the
combined GitHub workflow time still depends on runner startup, image/cache
state, and scheduling and needs confirmation on a dispatched workflow.

With the bounded C# and native build queues enabled, a fresh 7.7.0 shard 0/8
completed all 198 exchanges in 108 seconds; a fresh 7.3.1 shard 0/8 completed
191 exchanges in 102 seconds, with the same 181 passes and 10 known failures.
The 7.7.0 shard's 10 non-bootstrap Wireloom peer builds each took 2.1–3.1
seconds while four builds ran concurrently. A four-case 7.7.0 smoke run passed
54/54 exchanges in 51 seconds. These timings include local container execution
with the NuGet cache mounted; the full combined GitHub workflow has not yet been
measured on this implementation.

The complete unsharded 7.7.0 rerun with the parallel build queues completed in
12m03s: 1,549 of 1,555 exchanges passed, with the same six known failures and no
unrun exchanges. This is a full correctness run, not a proxy for the eight-shard
workflow critical path: peer setup spans all 91 cases in one container. The
shard measurements above are the relevant local comparison, while the combined
GitHub workflow still needs a dispatch to confirm the five-minute target.

The previous GitHub run (40254314909) identifies the shard critical path more
precisely. Its slowest 7.3.1 shard (index 6) took 9m34s end to end, with 7m16s
inside the matrix runner; its slowest 7.7.0 shard (index 6) took 8m58s end to
end, with 6m25s inside the runner. Replaying those same shard assignments with
the initial concurrency-eight setting took 2m12s for 7.3.1 (194 exchanges, 4
known failures) and 2m00s for 7.7.0 (198/198 passed). These local runs confirm
the heavy shard cases no longer have the old serial-build cost.

The scheduler was then raised to 16 exchanges. Under a four-CPU Docker limit,
7.3.1 shard 6 took 2m13s at concurrency eight and 2m03s at concurrency 16,
with the same 194 passes and four known failures. The 7.7.0 shard 6 took 1m56s
at concurrency 16 with all 198 exchanges passing. These local runs exclude
GitHub job startup, image/cache restoration, teardown, and report-job time; the
overall five-minute workflow target still needs confirmation from a dispatched
workflow.

## Code map

- `scripts/run-matrix.sh` prepares peers and invokes the C# exchange scheduler.
- `scripts/wire-case-build-runner.cs` builds case-specific managed and native peers with bounded concurrency.
- `scripts/matrix-tools.cs` owns fixtures, creates plans, and builds reports.
- `scripts/wire-exchange-runner.cs` runs bounded, domain-isolated peer exchanges.
- `scripts/verify-shards.cs` rejects incomplete, duplicated, or misassigned evidence.
- `FixtureCatalog.cs` and `Fixtures/` define managed fixture values.
- `Native/main.cpp.in` implements the DynamicData C++ peer;
  `Native/typed-fixture-peer.hpp.in` supports typed C++ fixtures.
- `Native/peer-qos.hpp` shares reliable-writer timing across both native paths.
- `RtiOracleProbe/` contains focused RTI C# API probes outside the wire matrix.
