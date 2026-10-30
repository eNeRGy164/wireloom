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
dotnet run --project tests/Wireloom.Dds.Generator.WireCompatibility.Runner/Wireloom.Dds.Generator.WireCompatibility.Runner.csproj --configuration Release
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
  'dotnet --info > "$WIRE_COMPATIBILITY_OUTPUT_DIR/dotnet-info.txt";
   dotnet run --project tests/Wireloom.Dds.Generator.WireCompatibility.Runner/Wireloom.Dds.Generator.WireCompatibility.Runner.csproj --configuration Release' || status=$?
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

| Variable                                      | Purpose                                                                           | Default                        |
| --------------------------------------------- | --------------------------------------------------------------------------------- | ------------------------------ |
| `WIRE_COMPATIBILITY_RTI_VERSION`              | Selects the exact RTI NuGet package, exclusions, and toolchain expectation.       | `7.7.0`                        |
| `WIRE_COMPATIBILITY_OUTPUT_DIR`               | Directory for scenario reports, logs, and shard metadata.                         | `artifacts/wire-compatibility` |
| `WIRE_COMPATIBILITY_CASE_FILTER`              | Comma-separated corpus case IDs for a focused run.                                | All eligible cases             |
| `WIRE_COMPATIBILITY_SHARD_INDEX`              | Zero-based shard to execute. Set together with shard count.                       | `0`                            |
| `WIRE_COMPATIBILITY_SHARD_COUNT`              | Number of deterministic shards. Use the same count and case filter across shards. | `1`                            |
| `WIRE_COMPATIBILITY_CPP_COMPILE_ONLY`         | Compile C++ peers without starting DDS exchanges when set to `1`.                 | `0`                            |
| `WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY` | Maximum concurrent case-specific Wireloom C# builds.                              | `4`                            |
| `WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY`   | Maximum concurrent native C++ builds; each build uses two compiler jobs.          | `2`                            |
| `WIRE_COMPATIBILITY_MAX_CONCURRENCY`          | Maximum independent exchanges active across all built cases in a shard.           | `16`                           |

Each shard needs its own working copy and output directory when run locally in
parallel. Case-specific managed outputs are isolated, but restore assets and
the bootstrap build use shared project paths. The selected case set must
contain at least as many cases as the shard count.

## Read the results

The runner writes `results.json` and `results.md`, plus `logs/`, `scenarios.tsv`,
`plan.json`, `shard.json`, and an `execution-provenance.json` captured before peer
builds. The provenance manifest records the exact dirty source paths and SHA-256
hashes, with their source bytes retained under `source-files/`. Reporting keeps
that execution identity; it does not infer it from the checkout doing the render.
A complete run combines the shard reports into
one version-specific report. Use the JSON for structured outcomes and the
Markdown report for review. Its title identifies the RTI profile; shared
adaptations list all affected peers once. Technical terms use code spans, and
observed error bodies are quoted verbatim.

Each result records the case, fixture, writer and reader peers, runtime
versions, and outcome. The Markdown report summarizes pairings and includes
observed problems. Failed exchanges remain visible in both reports. Not-run,
not-implemented, and not-applicable cases remain distinct from observed
exchange failures.

The report merger checks that shards share the same plan, RTI version, and exact
execution source identity, and
that their exchanges are complete, unique, and assigned to the correct shard.
Missing, duplicated, misassigned, inconsistent, or not-run exchanges prevent a
complete report. Fatal setup and worker failures preserve a nonzero
workflow result even when partial reports are available.

Planning passes selected case IDs, scenario records, and expected counts in
memory. Build requests and outcomes also stay in memory. Only these planning
files are retained:

| Artifact        | Consumer and purpose                                                                                                   |
| :-------------- | :--------------------------------------------------------------------------------------------------------------------- |
| `plan.json`     | The shard merger verifies the shared whole-case assignment and complete set of expected exchange keys.                 |
| `shard.json`    | The shard merger verifies profile, shard index/count, and expected exchange count.                                     |
| `scenarios.tsv` | One canonical file in the output directory, updated by the exchange scheduler and read by reporting and shard merging. |

`selected-cases.txt`, the duplicate scratch `scenarios.tsv`, and the managed
and native build request/result TSVs are removed. Managed and native builds
run directly through bounded in-process scheduling.

`logs/<case>-generation.json` also remains as retained evidence. The report
renderer validates it and the shard merger carries it into combined results.
The runner creates it directly from a typed provenance record, with no
intermediate evidence file, and clears old provenance at run initialization.

For planning, scheduling, and component ownership, see the
[harness reference](../../docs/testing/wire-harness.md).

## Fixtures and peer setup

The [fixture reference](../../docs/testing/wire-fixtures.md) owns construction,
expected values, reference selection, and runtime-specific setup. The
[workaround catalog](WORKAROUNDS.md) explains exceptional peer adaptations.

<a id="reliable-writer-timing"></a>

### Reliable-writer timing

See [reliable-writer timing](../../docs/testing/wire-fixtures.md#reliable-writer-timing).

<a id="case-specific-fixture-construction"></a>

### Case-specific fixture construction

See [case-specific fixture construction](../../docs/testing/wire-fixtures.md#case-specific-fixture-construction).

<a id="annotation-and-value-binding-cases"></a>

### Annotation and value-binding cases

See [annotation and value-binding cases](../../docs/testing/wire-fixtures.md#annotation-and-value-binding-cases).

<a id="dynamicdata-fixture-api-limits"></a>

### DynamicData fixture API limits

See [dynamicdata fixture api limits](../../docs/testing/wire-fixtures.md#dynamicdata-fixture-api-limits).

<a id="recursive-fixture-metadata"></a>

### Recursive fixture metadata

See [recursive fixture metadata](../../docs/testing/wire-fixtures.md#recursive-fixture-metadata).

<a id="reference-generation-settings"></a>

### Reference generation settings

See [reference generation settings](../../docs/testing/wire-fixtures.md#reference-generation-settings).

<a id="numeric-character-union-expected-values"></a>

### Numeric character-union expected values

See [numeric character-union expected values](../../docs/testing/wire-fixtures.md#numeric-character-union-expected-values).

<a id="reproducible-rti-character-binding-workarounds"></a>

### Reproducible RTI character-binding workarounds

See [reproducible rti character-binding workarounds](../../docs/testing/wire-fixtures.md#reproducible-rti-character-binding-workarounds).

<a id="recursive-array-writer-memory"></a>

### Recursive-array writer memory

See [recursive-array writer memory](../../docs/testing/wire-fixtures.md#recursive-array-writer-memory).

<a id="rti-731-negative-int8-union-discriminator"></a>

### RTI 7.3.1 negative int8 union discriminator

See [rti 7.3.1 negative int8 union discriminator](../../docs/testing/wire-fixtures.md#rti-731-negative-int8-union-discriminator).

<a id="idl-sequence-array-corpus-case"></a>

### IDL sequence-array corpus case

See [idl sequence-array corpus case](../../docs/testing/wire-fixtures.md#idl-sequence-array-corpus-case).

## Maintainer checks

These checks validate shard planning, artifact merging, and harness exit
behavior without an RTI license:

```powershell
dotnet run --project tests/Wireloom.Dds.Generator.WireCompatibility.Verifier/Wireloom.Dds.Generator.WireCompatibility.Verifier.csproj --configuration Release
```

Also run the C# harness checks without an RTI license:

```powershell
dotnet run --project tests/Wireloom.Dds.Generator.WireCompatibility.Runner/Wireloom.Dds.Generator.WireCompatibility.Runner.csproj --configuration Release -- --verify-harness
```

The Verifier project compares sharded and unsharded report output and exercises
invalid or incomplete shard sets. The Runner harness checks finalization,
partial-report recovery, stale-artifact isolation, handled native build
failures, and profile-specific fixture selection without RTI peers or a
license.
The GitHub report job runs both checks before combining shard evidence.

### Full-matrix throughput validation

The [October 27 measurements](../../docs/testing/evidence/wire-runner-2026-10-27.md#full-matrix-throughput-validation)
are historical single-host observations, not acceptance budgets.

## Code map

The [harness reference](../../docs/testing/wire-harness.md) describes the Runner,
Verifier, shared report contract, and fixture assets. These are qualification
tools outside the Wireloom product. Their migration comparisons are retained
in the [dated validation record](../../docs/testing/evidence/wire-runner-2026-10-27.md).

### Runner structure validation (2026-10-27)

See the [historical validation record](../../docs/testing/evidence/wire-runner-2026-10-27.md#runner-structure-validation-2026-10-27).
