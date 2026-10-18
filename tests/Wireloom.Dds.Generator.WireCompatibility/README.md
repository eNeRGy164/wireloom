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
