# Managed generator compatibility corpus

This directory contains the IDL cases and reference outputs used to evaluate
the managed source generator. It is the shared boundary between authored IDL
feature cases, RTI-generated C# reference sources, and compatibility tests.

The corpus covers IDL parsing, preprocessing, semantic diagnostics, RTI
annotations, and RTI and Wireloom generated-source shape. It does not cover
runtime behavior, serialization bytes, wire compatibility, keyhashes, or live
DDS communication.
Those concerns belong in the relevant implementation and integration tests.
Generated API runtime behavior is covered by the
[runtime contract suite](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md).

## Contents

```text
corpus/
├── manifest.json       authored case inventory and input configuration
├── FEATURE-COVERAGE.md feature index and implementation status
├── csharp-quality.md   retained C# generated-source quality evidence
├── idl/
│   ├── features/       positive feature cases
│   ├── negative/       rejection and diagnostic probes
│   ├── integration/    integration entry points and support IDL
│   └── includes/       shared IDL included by feature cases
├── 7.3.1/              version manifest, coverage, RTI oracles, and Wireloom exports
├── 7.7.0/              version manifest, coverage, RTI oracles, and Wireloom exports
├── scripts/            corpus evidence capture tools
└── LTS-PROFILES.md     versioned evidence overview
```

The root `idl/` inventory is shared by both profiles. Versioned files record
RTI C# and C++ generation outcomes and Wireloom output independently. RTI
outputs live under `<version>/rtiddsgen/`; Wireloom exports live under
`<version>/generator/`. The 7.3.1 generated
contracts compile against the exact NuGet package, and its runtime-contract
suite passes; exact-version wire qualification remains pending. The 7.7.0 test
loader reads its RTI C# oracle tree from `7.7.0/rtiddsgen/csharp/`.
Acceptance can also vary by target profile: `09-autoid-hash` is a Wireloom
negative case under 7.3.1 because `@must_understand` is unavailable there, and
is accepted under 7.7.0.

The repository's MIT License does not apply to the checked-in RTI-generated
oracle sources. They are third-party reference material, provided as-is for
compatibility testing, and remain subject to any applicable RTI rights and
terms. Do not redistribute or regenerate them except as permitted by the
applicable RTI license and repository review rules. Retain all file-level
notices.

## Case inventory

The manifest currently contains **77 positive cases**, **60 negative probes**,
and **4 integration entry points** (141 entries total). These counts follow
`manifest.json`, the source of truth for the corpus inventory.

| Group                    | Count | Location                             |
| ------------------------ | ----: | ------------------------------------ |
| Positive feature cases   |    77 | [`idl/features`](idl/features)       |
| Negative probes          |    60 | [`idl/negative`](idl/negative)       |
| Integration entry points |     4 | [`idl/integration`](idl/integration) |
| Total                    |   141 | —                                    |

Each case has a manifest ID, such as `01-primitives`, and a stable `C###`
provenance tag. Tags are used by the feature index and test diagnostics.
Tags normally follow positive, negative, and integration manifest order. An
explicit `tag` preserves existing references when a classified case is added to
an earlier group.

When RTI results differ by host preprocessor or platform, record separate cases
with distinct IDs and the same IDL path. Keep the environment and tool versions
in each case's `theme` and `observedDiagnostics`; retain generated sources only
for environments where RTI accepts the input.

The compliance loader derives tags from manifest order unless a case has an
explicit `tag` in `manifest.json`. Give a newly inserted case an explicit next
tag when existing later cases must retain their `C###` references. The
authoritative mapping is [FEATURE-COVERAGE.md](FEATURE-COVERAGE.md).

## Oracle library

Each LTS directory's `rtiddsgen/csharp/` tree mirrors RTI-generated C# for that
exact version. RTI acceptance is reference data only; it does not imply that
the managed generator supports the same feature. The
[7.7.0 profile](7.7.0/FEATURE-COVERAGE.md) is the current retained comparison
baseline. The [7.3.1 profile](7.3.1/FEATURE-COVERAGE.md) records the captured
run and its outstanding qualification evidence.

Rejected cases may have no managed generated C# source, while remaining in the
case inventory so accepted, rejected, and observed outcomes stay explicit.
An RTI-accepted case may still be deliberately rejected by Wireloom when its
names would force invalid generated C#; the RTI oracle remains available as
comparison evidence.

Record RTI rejection details in `observedDiagnostics` as well as the outcome,
so later investigations can reuse the exact diagnostic without rerunning the
probe. When source generation does not establish runtime behavior, record that
separately with `runtimeSupport` (`uncertain` or an observed result) and a
`runtimeSupportNote`; a code-generation rejection alone is not runtime evidence.

## Reproducing RTI results

Run `rtiddsgen` through `wslc` and the RTI container image; RTI does not need to
be installed on the host. From PowerShell at the repository root, generate a
case without preprocessor directives into a mounted workspace directory:

```powershell
New-Item -ItemType Directory -Force .tmp/rti-output | Out-Null
wslc run --rm -v "${PWD}:/work" -w /work rticom/connext-base:7.7.0.1 rtiddsgen -ppDisable -language "C#" -d /work/.tmp/rti-output /work/docs/corpus/idl/features/09-optional-aggregate-member.idl
```

The container is removed after the run; output under `/work` remains in the
mounted repository. `-ppDisable` is suitable for IDL without preprocessor
directives. For IDL using `#include` or macros, enable the C/C++ preprocessor.
If the image does not contain `cpp`, install `g++` inside the disposable
container in the same invocation. This example records the preprocessor version
and reproduces a preprocessor rejection without a host install:

```powershell
wslc run --rm -v "${PWD}:/work" -w /work rticom/connext-base:7.7.0.1 sh -lc 'apt-get update && apt-get install -y g++ && cpp --version && rtiddsgen -language "C#" -d /work/.tmp/rti-output /work/docs/corpus/idl/negative/12-error-directive.idl'
```

Record the RTI and `cpp` versions, outcome, and diagnostic in `manifest.json`.
Copy generated sources into `<version>/rtiddsgen/csharp/` only when RTI accepts
the case and the license permits retaining them.

## Capturing C++ oracle output

Generate C++11 output for every shared positive, negative, and integration case
with the matching RTI LTS image. The script writes version-scoped sources,
per-case logs, and a generation manifest under
`<version>/rtiddsgen/cpp/`. Case manifests refer to the canonical input under
`idl/`; rejected cases retain their diagnostic log without partial generated
sources. Generation evidence does not assert that the output compiles or that
the feature is wire-compatible.

From PowerShell at the repository root, mount the local license file and run
the generator once for each profile:

```powershell
$licenseFile = Join-Path $env:USERPROFILE 'Downloads/rti_license.dat'

wslc run --rm --user 0:0 `
  -v "${PWD}:/work" `
  -v "${licenseFile}:/run/secrets/rti_license.dat:ro" `
  -e RTI_LICENSE_FILE=/run/secrets/rti_license.dat `
  --entrypoint bash `
  rticom/connext-base:7.3.1@sha256:f33a61ec6381a8fb73cbb019fbb5c081f434dc5217eb14283fad995306be0f04 `
  -lc 'apt-get update -qq && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends g++ python3 && python3 /work/docs/corpus/scripts/generate-rtiddsgen-cpp.py --version 7.3.1'

wslc run --rm --user 0:0 `
  -v "${PWD}:/work" `
  -v "${licenseFile}:/run/secrets/rti_license.dat:ro" `
  -e RTI_LICENSE_FILE=/run/secrets/rti_license.dat `
  --entrypoint bash `
  rticom/connext-base:7.7.0@sha256:efe4d35807a4aadaa59f0e84545d4c6b70328af5b0c0ca0498e63ecc1e16e1ca `
  -lc 'apt-get update -qq && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends g++ python3 && python3 /work/docs/corpus/scripts/generate-rtiddsgen-cpp.py --version 7.7.0'
```

The script verifies the expected `rtiddsgen` version before generating. It
uses `-generateIncludeFiles` for 7.7.0 and applies the experimental
`-unboundedSupport` option where the shared manifest requires it. The C++
FlatData case uses the same narrow annotation removal as the wire harness.
Check each generated `manifest.json` for per-case results and any generator
failures. Each case points to its canonical source under `idl/`; the output
folders do not contain copies of the IDL. The FlatData adaptation exists only
in the temporary generation workspace and is recorded with source and adapted
input hashes in the manifest. The generator captures per-case rejections and
continues through the inventory; its process exit status reports whether the
capture completed, while case outcomes remain in the manifest.

## Wire matrix generation profiles

Positive-case entries may define a `wireMatrix` object in `manifest.json` for
case-specific reference-peer requirements. `rtiCppCodegenOptions` supplies
additional arguments to RTI C++ `rtiddsgen` for that case only, and
`qualification` explains constraints on interpreting its result. The wire
runner copies this configuration into each affected scenario in `results.json`
and lists it in the generated `results.md` under **Case-specific RTI C++
generation**. Use this for documented experimental RTI features or required
code-generator switches; keep ordinary shared IDL and Wireloom generation
independent of those RTI-specific options.

`rtiCppIdlAdaptations` lists narrowly scoped, version-qualified changes to the
scratch IDL used only by RTI C++ generation. The `07-union-char-boundaries`
case covers NUL and printable ASCII tilde (U+007E, 126). Both exact LTS
profiles pass all 27 exchanges with the shared IDL and no case-specific C++
IDL adaptation. These focused results apply to this case and do not qualify an
entire RTI profile.

The recursive aggregate and optional recursive array cases request
`-unboundedSupport`. RTI describes recursive-structure support as experimental;
successful compilation or an exchange under that profile does not establish
that recursion is production-qualified for RTI. See the [wire matrix
procedure](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md) for
the runner and result semantics.

## Test responsibilities

Keep fast unit tests separate from exact corpus comparisons:

- Unit tests may use inline IDL strings and should not depend on corpus files.
- Corpus compliance tests should load `manifest.json`, validate every corpus
  root, check expected acceptance or rejection, and compare accepted generated
  C# shape with the matching oracle sources.
- Package and runtime integration tests should consume only the corpus cases
  relevant to their scope.

Run the complete manifest-driven compliance suite with:

```text
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

The package preview and release workflows restore and run this suite before
packing. The quality workflow remains focused on the fast unit-test and
coverage path.

Use the feature index to find the exact IDL, oracle files, managed status, and
test entry points: [FEATURE-COVERAGE.md](FEATURE-COVERAGE.md).

## Feature coverage document

`FEATURE-COVERAGE.md` is a maintained feature index, not generated output from
the corpus tests. It records the feature description, `C###` tag, IDL link, RTI
oracle link, RTI classification, and current managed-generator status. The
status snapshot is updated from test results and implementation review; tests
do not rewrite this Markdown table.

## Adding or changing a case

1. Add a feature-bound IDL under the appropriate `idl/` group. Keep one theme
   per case and make the root explicit in `manifest.json`.
2. Add required defines, include roots, observed outcomes, and normalized
   `observedDiagnostics` captured from RTI to `manifest.json` where applicable.
3. Update [FEATURE-COVERAGE.md](FEATURE-COVERAGE.md) with the case's feature,
   `C###` tag, IDL link, oracle link, and managed status.
4. Run the applicable corpus, package, and integration tests.

Keep one feature theme per case, use explicit roots, and keep exact corpus
evidence separate from runtime or interoperability claims.

## Review rules

- Do not use dated directories inside `idl/` or `<version>/rtiddsgen/csharp/`.
- Do not make tests depend on external capture history.
- Keep RTI acceptance separate from managed-generator support status.
- Keep runtime, serialization, and wire claims out of source-generation status.
- Review the RTI license status before committing or sharing generated output.

## Exporting managed-generator sources

Set `CORPUS_RTI_PROFILE` to `7.3.1` or `7.7.0` and set
`CORPUS_GENERATOR_EXPORT_ROOT` to the matching version's `generator/` folder.
The export test writes sources under `<profile>/generator/<source-kind>/<case-id>`:

```powershell
$env:CORPUS_RTI_PROFILE = "7.7.0"
$env:CORPUS_GENERATOR_EXPORT_ROOT = "docs/corpus/7.7.0/generator"
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --configuration Release -- --filter-method Wireloom.Dds.Generator.CorpusCompliance.CorpusComplianceSpecs.ExportAcceptedCorpusSources
```

Rejected cases are omitted because they do not produce generated sources. Run
the same command with `CORPUS_RTI_PROFILE=7.3.1` and the 7.3.1 export root to
refresh that profile. The profile choice affects RTI-specific emission only;
the shared IDL inventory and Wireloom acceptance classifications stay the
same.

Unbounded typedef cases C135/C136 retain their RTI C# oracle pairs under
`7.7.0/rtiddsgen/csharp/features`. Their generated API behavior is covered by the runtime
contract suite.
