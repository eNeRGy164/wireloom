# Wire compatibility tests

This project checks DDS interoperability for the wire-testable corpus. It runs
Wireloom-generated C#, RTI-generated C#, and RTI-generated C++ peers against
each other. It complements the [runtime contract suite](../Wireloom.Dds.Generator.RuntimeContracts/README.md),
which checks generated APIs and runtime behavior without claiming wire
compatibility.

The matrix targets the exact RTI Connext LTS releases 7.3.1 and 7.7.0. Each
fixture is an independent exchange across the nine directed writer/reader
pairings among the three peers. A reader must receive a valid sample and match
the full expected fixture; discovery or lifecycle notifications alone do not
count as a pass. These runs provide evidence for the selected corpus cases,
not qualification of every feature or RTI release.

Version-specific registries separate `notApplicableCases` from
`notImplementedCases`. A feature introduced after the selected RTI version is
not applicable: it contributes no exchanges, failures, or implementation gaps.
JSON and Markdown reports retain its case ID and reason separately. For
example, `09-must-understand` (C142) is not applicable to 7.3.1; explicitly
requesting it is rejected before peer builds or type-support discovery.
`09-autoid-hash` (C040) independently checks hash member IDs on both versions.
Both cases have non-default fixtures. Wire results recorded before the split
do not qualify these revised inputs; fresh exchanges remain pending.

## Run the matrix

### GitHub Actions

Manually dispatch the **Wire compatibility** workflow. It runs both RTI
versions in the same workflow. Each version has four case shards and a separate
report job, summary, and combined artifact. Up to four shard jobs can run at
once for each version. The workflow requires the `RTI_CONNEXT_LICENSE` secret.

The combined artifacts are named `wire-compatibility-rti-7.3.1` and
`wire-compatibility-rti-7.7.0`. Shard artifacts use the corresponding
`-shard-<index>` suffix. Partial evidence is uploaded even when a shard fails;
the report job validates available evidence and retains a failing workflow
status for setup errors or incomplete exchanges.

### Local container run

Run `scripts/run-matrix.sh` inside the helper image built from
`Containerfile`. Set the RTI version to exactly `7.3.1` or `7.7.0`, provide a
matching RTI license through `RTI_LICENSE_FILE`, and make the repository
available inside the container. Use a version-specific output directory so
local evidence cannot be confused across profiles.

For example, inside the container:

```bash
export WIRE_COMPATIBILITY_RTI_VERSION=7.7.0
export WIRE_COMPATIBILITY_OUTPUT_DIR=/workspace/artifacts/wire-compatibility-rti-7.7.0
bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/run-matrix.sh
```

The 7.3.1 run uses the same command with `WIRE_COMPATIBILITY_RTI_VERSION=7.3.1`
and a 7.3.1 output directory. For local runs, copy the checked-out source into
the helper container and copy or mount the license as a read-only file; do not
include the license in the source tree or generated artifacts.

For a complete local reproduction, use a clean Linux checkout of the recorded
revision; do not reuse Windows `obj`/`bin` directories or a Windows worktree's
`.git` pointer inside Linux. The PowerShell corpus capture/export recipes in
the [LTS runbook](../../docs/corpus/LTS-PROFILES.md) run directly on Windows.
For example, from that Linux checkout with Docker available:

```bash
version=7.3.1
base='rticom/connext-base:7.3.1@sha256:f33a61ec6381a8fb73cbb019fbb5c081f434dc5217eb14283fad995306be0f04'
# For 7.7.0 use the matching base digest from docs/corpus/LTS-PROFILES.md.
license_file="$HOME/rti_license.dat" # Keep the license outside the checkout.
[[ -f "$license_file" && -z "$(git status --porcelain)" ]] || exit 1
run_dir="$PWD/artifacts/wire-rti-$version-$(date -u +%Y%m%dT%H%M%SZ)"
mkdir -p "$run_dir"
git rev-parse HEAD > "$run_dir/source-commit.txt"
printf '%s\n' "$base" > "$run_dir/base-image.txt"
date -u +%FT%TZ > "$run_dir/started-at.txt"
docker build --file tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile \
  --build-arg "RTI_BASE_IMAGE=$base" --tag "wireloom-rti-$version:repro" \
  --iidfile "$run_dir/helper-image-id.txt" .
status=0
docker run --rm --user 0:0 --workdir /workspace \
  --volume "$PWD:/workspace" \
  --volume "$license_file:/run/secrets/rti_license.dat:ro" \
  --env GITHUB_WORKSPACE=/workspace \
  --env RTI_LICENSE_FILE=/run/secrets/rti_license.dat \
  --env "WIRE_COMPATIBILITY_RTI_VERSION=$version" \
  --env "WIRE_COMPATIBILITY_OUTPUT_DIR=/workspace/${run_dir#"$PWD/"}" \
  --entrypoint bash "wireloom-rti-$version:repro" -c \
  'dotnet --info > "$WIRE_COMPATIBILITY_OUTPUT_DIR/dotnet-info.txt";
   "$NDDSHOME/bin/rtiddsgen" -version > "$WIRE_COMPATIBILITY_OUTPUT_DIR/rtiddsgen-version.txt" 2>&1;
   bash tests/Wireloom.Dds.Generator.WireCompatibility/scripts/run-matrix.sh' || status=$?
date -u +%FT%TZ > "$run_dir/completed-at.txt"
printf '%s\n' "$status" > "$run_dir/exit-code.txt"
# Keep the entire run directory, including failed/partial results and logs.
```

A dirty-tree run needs the actual source snapshot/patch and untracked inputs,
not just `git rev-parse HEAD`; the example deliberately uses a clean checkout.
The report JSON alone does not currently record every source/image identity.
Keep the provenance sidecars above with it, and record case filters, shard
settings, host/compiler versions, and any deviations from the runbook. Do not
add license files or secret values to the evidence bundle. The helper base is
pinned, but compiler packages installed while building it can change: retain
the helper image ID, tool versions, and build logs when reproducing old results.
No licensed exchange is implied by a successful capture, build, or dry check.

Useful runner settings:

| Variable                              | Purpose                                                                           | Default                        |
| ------------------------------------- | --------------------------------------------------------------------------------- | ------------------------------ |
| `WIRE_COMPATIBILITY_RTI_VERSION`      | Selects the exact RTI NuGet package, exclusions, and toolchain expectation.       | `7.7.0`                        |
| `WIRE_COMPATIBILITY_OUTPUT_DIR`       | Directory for scenario reports, logs, and shard metadata.                         | `artifacts/wire-compatibility` |
| `WIRE_COMPATIBILITY_CASE_FILTER`      | Comma-separated corpus case IDs for a focused run.                                | All eligible cases             |
| `WIRE_COMPATIBILITY_SHARD_INDEX`      | Zero-based shard to execute. Set together with shard count.                       | `0`                            |
| `WIRE_COMPATIBILITY_SHARD_COUNT`      | Number of deterministic shards. Use the same count and case filter across shards. | `1`                            |
| `WIRE_COMPATIBILITY_CPP_COMPILE_ONLY` | Compile C++ peers without starting DDS exchanges when set to `1`.                 | `0`                            |

Each shard needs its own working copy and output directory when run locally in
parallel because peer builds share project `obj` and `bin` paths. The selected
case set must contain at least as many cases as the shard count.

## Read the results

The runner writes `results.json` and `results.md`, plus `logs/`, `scenarios.tsv`,
`plan.json`, and `shard.json`. A complete run combines the shard reports into
one version-specific report. Use the JSON for structured outcomes and the
Markdown report for review.

Each result records the case, fixture, writer and reader peers, runtime
versions, and outcome. The Markdown report summarizes pairings and includes
observed problems and any registered expected-failure explanation. Expected
failures remain visible outcomes; an exclusion does not skip an exchange or
convert a failure into a pass. Not-run and not-implemented cases are distinct
from observed exchange failures.

The report merger checks that shards share the same plan and RTI version and
that their exchanges are complete, unique, and assigned to the correct shard.
Missing, duplicated, misassigned, inconsistent, or not-run exchanges prevent a
complete report. Peer setup failures are recorded and preserve a nonzero
workflow result even when partial reports are available.

## Fixtures and peer setup

`FixtureCatalog.cs` and the native fixture helpers define expected values and
variations for each case. Keep fixtures aligned with their corpus IDL, including
optional presence, collection lengths, selected union branches, and member
paths. Readers verify complete values rather than only checking endpoint
discovery.

The managed peers construct `09-defaults-ranges` through its generated typed
constructor, set only `value` and `ranged`, and leave `color` untouched. The
reader independently requires the IDL-declared `BLUE` value (`2`), so a wrong
constructor default still fails. RTI 7.3.1 DynamicData otherwise substitutes
`0` because that profile cannot attach the newer member-default annotations.

The managed peers also construct `01-boundaries` and `01-full-widths` through
typed public members. This preserves negative int8 fixture values without
passing them through RTI 7.3.1 DynamicData, which reports int8 as `Uint8` and
rejects negative `sbyte` assignments. Independent reader checks cover every
signed and unsigned scalar member; generated serializers remain unchanged.
Both profiles use the same typed fixture path. JSON endpoint adaptations and
the Markdown Adaptations column identify these choices on managed endpoints;
the C++ fixture path is unchanged. The negative int8 union discriminator bug
CORE-14660 remains a separate, fixture-specific expected failure on 7.3.1.

RTI C# sources are generated in scratch with the selected `rtiddsgen`. RTI C++
peers use `DynamicData` by default; typed generated peers are used when a needed
DynamicData string specialization is unavailable. Some cases configure a
peer-specific setup in `docs/corpus/manifest.json`:

- `rtiCppCodegenOptions` passes options only to that case's C++ generator. Any
  documented qualification, such as RTI's experimental recursive-structure
  support, is copied into scenario evidence.
- `rtiCppIdlAdaptations` changes only the scratch IDL used for RTI C++
  generation. It does not change the corpus IDL or Wireloom and RTI C# inputs.

The exclusion registries `wire-case-exclusions-7.3.1.json` and
`wire-case-exclusions-7.7.0.json` keep expected outcomes separate by RTI
version. Add or change an expected-failure entry only when supported by the
matching version's evidence, and keep its case, fixture, pairing, and reason
scope as narrow as possible.

### Numeric character-union expected values

Both managed and C++ peers require the same discriminator and selected payload:

| Case | Fixture | Discriminator | Selected member | Payload |
| --- | --- | ---: | --- | ---: |
| `07-union-char-numeric` | `union-127` | 127 | `value` | 13701 |
| `07-union-char-numeric-255` | `union-255` | 255 | `value` | 13702 |
| Both cases | `union-default` | 65 (`'A'`) | `fallback` | 13703 |

The C++ default fixture explicitly selects 65 after assigning its fallback value;
its generated constructor otherwise retains 0. Readers check these fixed values
independently of fixture construction and generated equality. The C++ report
adaptation describes the expected values.

### Reproducible RTI character-binding workarounds

For these two numeric character cases, the runner applies guarded corrections
to freshly generated RTI reference sources on both LTS versions:

- RTI C#: add the missing numeric-to-`char` casts so the bindings compile,
  and deserialize fallback branches through `Setfallback` to preserve the
  incoming discriminator instead of resetting it to the constructor default.
  The reference uses typed public-member fixture construction.
- Label 255 only, RTI C# and C++: store the internal union label as signed-byte
  `-1`. Public values remain 255, the wire byte remains `FF`, and the payload
  remains 13702. This corrects the unadapted C++ serializer's zero payload and
  cross-peer discovery mismatch.
- RTI C++: compile these generated bindings with `-funsigned-char` on GCC,
  preserving the generated branch guards for the full IDL char range.

These are post-generation binding corrections that an RTI application can apply
in its production build after each `rtiddsgen` invocation. They retain RTI's
generated data classes and actual serializers; no fixture value, wire schema,
runtime binary, or reader check is replaced. The utility validates the expected
generated source shape before writing and supports repeated application.
Unexpected generator output stops the build for review.

Build `scripts/matrix-tools.cs` as described below, then apply the same commands
to a generated-source directory:

```sh
dotnet matrix-tools.dll adapt-rti-characters csharp 07-union-char-numeric <generated-csharp-dir>
dotnet matrix-tools.dll adapt-rti-characters csharp 07-union-char-numeric-255 <generated-csharp-dir>
dotnet matrix-tools.dll adapt-rti-characters cpp 07-union-char-numeric-255 <generated-cpp-dir>
```

For C++, the runner's generated source is named `idl.cxx`; use that basename
when reproducing its command. Compile the generated C++ sources and application
translation units that include these bindings consistently with
`-funsigned-char`. The C# commands use the case ID as the generated basename.

These commands reproduce the two retained IDL examples. For other type or member
names, scope the equivalent post-generation edits to the affected bindings and
repeat the discriminator/payload contract checks.

The baseline defects are reproduced in
[wire run 39161010741](https://github.com/eNeRGy164/wireloom/actions/runs/39161010741)
at `10dd169b`. No published RTI issue ID has been established for these
character defects; the negative-int8 CORE-14660 issue is separate. Retained
oracle captures stay unmodified. JSON writer/reader adaptations and Markdown
Adaptations disclose each correction, so an adapted pass does not claim that
the original RTI-generated bindings work unchanged.

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
`-128` and has eight registered expected failures on RTI 7.3.1; the RTI C++
self-exchange passes. RTI stores the negative discriminator as an unsigned
value, causing managed DynamicData branch lookup failures and interpreted C#
serializer payload loss. RTI documents this as
[CORE-14660, fixed in 7.6.0](https://community.rti.com/static/documentation/connext-dds/7.7.0.1/doc/manuals/connext_dds_professional/release_notes/whats_fixed/760/fixes_dynamic_data.html).
The classification is supported by
[wire run 39131568513](https://github.com/eNeRGy164/wireloom/actions/runs/39131568513)
at revision `3e9450f2` and focused serializer probes against both the unchanged
RTI oracle and Wireloom output.

These entries apply only to `union-minimum` and the observed failing pairings.
The maximum and default fixtures remain positive controls on 7.3.1, and all
three fixtures pass on 7.7.0. Expected failures remain executed and reported as
failures; this runtime defect does not make the feature not applicable.

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
reportable outcomes. The GitHub report job runs these checks before merging.

## Code map

- `scripts/run-matrix.sh` builds peers, starts exchanges, and records outcomes.
- `scripts/matrix-tools.cs` discovers cases, plans shards, and builds reports.
- `FixtureCatalog.cs` and `Fixtures/` define managed fixture values.
- `Native/main.cpp.in` implements the DynamicData C++ peer;
  `Native/typed-fixture-peer.hpp.in` supports typed C++ fixtures.
- `RtiOracleProbe/` contains focused RTI C# API probes outside the wire matrix.
