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
suite passes; full profile wire qualification remains pending. The 7.7.0 test
loader reads its RTI C# oracle tree from `7.7.0/rtiddsgen/csharp/`.
C040 (`09-autoid-hash`) checks hash member IDs on both LTS profiles. The
separate C142 (`09-must-understand`) checks `@must_understand`, which is not
applicable to 7.3.1 and is accepted under 7.7.0. Explicitly compiling C142
against 7.3.1 reports `DDSG0004`; this target diagnostic is not a compatibility
failure for a feature unavailable in that release.

RTI-generated oracle sources are retained as reference output from the corpus
IDL, with exact tool versions, input/output hashes, and original file-level
notices. They are not relicensed under the repository's MIT License. Wireloom
does not claim ownership of RTI material. The captures contain generated output,
not the RTI generator, its templates, or its runtime libraries.

## Case inventory

The manifest currently contains **91 positive cases**, **77 negative probes**,
and **4 integration entry points** (172 entries total). These counts follow
`manifest.json`, the source of truth for the corpus inventory.

Dedicated C149, `05-array-of-sequences-idl`, uses a separately named IDL file
whose contents match C018 (`05-array-of-sequences`). Its
`sequenceArrayMapping: "Idl"` setting is an ordinary per-case input. Both
profile exports retain its generated sources under
`<version>/generator/features/05-array-of-sequences-idl/`; ordinary compliance
tests compare those sources with fresh generation. The case deliberately has
no RTI C# oracle because that binding flattens this direct IDL form.

| Group                    | Count | Location                                                   |
| ------------------------ | ----: | ---------------------------------------------------------- |
| Positive feature cases   |    91 | [`idl/features`](idl/features)                             |
| Negative probes          |    77 | [`idl/negative`](idl/negative) and feature-specific probes |
| Integration entry points |     4 | [`idl/integration`](idl/integration)                       |
| Total                    |   172 | —                                                          |

C163–C172 add grouped annotation, constant, declarator, union, string-bound,
and typed-default cases. Exact RTI C# and C++ generation outcomes are recorded
in each profile manifest with capture hashes. Their RTI-generated C# and C++
sources are retained under each profile's `rtiddsgen/` directory. Wireloom C#
exports are retained and compiled against the matching RTI API profiles.

Counts follow the manifest classifications. Some negative probes use an IDL
under `idl/features` because they characterize syntax that a generator accepts
but Wireloom must reject.

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

The optional `strict` case setting defaults to `false`. The compliance loader
and source exporter apply it to the generating root; RTI capture maps `true`
to `-strict`. C150/C151 and C152/C153 use separately named IDL files with
identical contents to compare default and strict validation. C154–C156 cover
exact reserved keywords, legal identifier controls, and strict derived keys.
C157–C162 extend those checks to ordinary and DDS-service interface bodies,
including operation/parameter scopes and nested declarations.
See the [configuration reference](../guides/configuration.md#enable-strict-validation)
for the validation rules and diagnostic policy.

## Oracle library

Each LTS directory's `rtiddsgen/csharp/` tree mirrors RTI-generated C# for that
exact version. RTI acceptance is reference data only; it does not imply that
the managed generator supports the same feature. The
[7.7.0 profile](7.7.0/FEATURE-COVERAGE.md) is the current retained comparison
baseline. The [7.3.1 profile](7.3.1/FEATURE-COVERAGE.md) records the retained
oracle results and current qualification status.

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

## LTS evidence and reproduction

Use the [LTS runbook](LTS-PROFILES.md) for complete, profile-specific commands
and evidence attribution. It covers locked restore, both corpus profiles,
fresh Wireloom exports, RTI C# and C++ capture, exact-package compilation,
runtime/package verification, and licensed wire runs. PowerShell and shell
scripts are checked in under `scripts/`; no Python interpreter is required.

Capture either or both oracle languages into a fresh artifact directory:

```powershell
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 7.3.1 -CaseId 09-autoid-hash,09-must-understand
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 7.7.0 -CaseId 09-autoid-hash,09-must-understand
```

Omit `-CaseId` to capture all manifest roots, or select `-Language csharp` or
`-Language cpp`. Captures verify the selected pinned image and generator,
record canonical and adapted input hashes, command arguments, toolchain
versions, exit codes, output hashes, and raw logs. They never overwrite retained
oracles automatically. Compare and review the capture before promoting sources
or updating version manifests. Generation alone does not establish compilation,
runtime behavior, or wire interoperability.

Commit reviewed corpus sources and manifests.

## Wire matrix generation profiles

Positive-case entries may define a `wireMatrix` object in `manifest.json` for
case-specific reference-peer requirements. `rtiCppCodegenOptions` supplies
additional arguments to RTI C++ `rtiddsgen` for that case only, and
`qualification` explains constraints on interpreting its result. The wire
runner copies this configuration into each affected scenario in `results.json`
and lists it in the generated `results.md` under **Case-specific
generation and peer scope**. Use this for documented experimental RTI features or required
code-generator switches; keep ordinary shared IDL and Wireloom generation
independent of those RTI-specific options.

The C149 `Idl` case selects Wireloom C# and RTI C++, with four fixtures and
16 exchanges per exact LTS profile. RTI C# is explicitly excluded because its
direct-form binding describes the flattened type. Reports show those five
pairings as N/A, rather than missing exchanges. The original C018 case retains
all nine pairings and its known limitations. C149 results retain the actual
peer build's mode, runtime, IDL hash, and generated-source hashes through shard
merging. A completed exchange without matching provenance is rejected.

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

C018 retains its default RTI C# flattened exports and oracle comparisons.
C149 separately checks the opt-in `Idl` mapping under each selected profile,
using its duplicate IDL source and normal generated-source folders. Typed
runtime and package fixtures cover the setting and generated collection
behavior; corpus generation does not establish DDS wire qualification.

The package preview and release workflows restore and run this suite before
packing. The Quality workflow also runs both exact LTS corpus profiles and
the runtime/package consumer matrix; see the [LTS runbook](LTS-PROFILES.md).

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
- Retain generated reference output with its original notices and provenance.

## Exporting managed-generator sources

Prefer `Export-CorpusGeneratorSources.ps1` for locked restore and retained
source/output hashes, SDK details, and test logs. The direct command below is
an export-only alternative and does not create that provenance bundle.

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
refresh that profile. The shared IDL inventory stays the same; applicability
and expected generation outcomes follow the selected profile. In particular,
C142 is omitted from the 7.3.1 export because `@must_understand` is unavailable.

Unbounded typedef cases C135/C136 retain their RTI C# oracle pairs under
`7.7.0/rtiddsgen/csharp/features`. Their generated API behavior is covered by the runtime
contract suite.
