# Managed generator compatibility corpus

This directory contains the IDL cases and reference outputs used to evaluate
the managed source generator. It is the shared boundary between authored IDL
feature cases, RTI-generated C# reference sources, and compatibility tests.

The corpus covers IDL parsing, preprocessing, semantic diagnostics, RTI
annotations, and generated C# source shape. It does not cover runtime behavior,
serialization bytes, wire compatibility, keyhashes, or live DDS communication.
Those concerns belong in the relevant implementation and integration tests.

## Contents

```text
corpus/
├── manifest.json       authored case inventory and input configuration
├── FEATURE-COVERAGE.md feature index and implementation status
├── idl/
│   ├── features/       positive feature cases
│   ├── negative/       rejection and diagnostic probes
│   ├── integration/    integration entry points and support IDL
│   └── includes/       shared IDL included by feature cases
└── oracles/            RTI-generated C# reference sources
└── generator/          Wireloom-generated C# sources exported from the corpus
```

The checked-in oracle sources are generated reference material. They are
license-controlled and must not be redistributed or regenerated outside the
applicable RTI license and repository review rules.

## Case inventory

The current inventory contains:

| Group                    | Count | Location                             |
| ------------------------ | ----: | ------------------------------------ |
| Positive feature cases   |    54 | [`idl/features`](idl/features)       |
| Negative cases           |    53 | [`idl/negative`](idl/negative)       |
| Integration entry points |     4 | [`idl/integration`](idl/integration) |
| Total                    |   111 | —                                    |

Each case has a manifest ID, such as `01-primitives`, and a stable `C###`
provenance tag. Tags are used by the feature index and test diagnostics.
Positive cases are numbered first, followed by negative cases, then integration
cases.

The compliance loader currently derives tags from manifest order. Keep existing
case order append-only when adding cases so existing `C###` references do not
move. The authoritative mapping is [FEATURE-COVERAGE.md](FEATURE-COVERAGE.md).

## Oracle library

The `oracles/` tree mirrors the IDL case structure and contains the RTI
Connext DDS 7.7.0 / `rtiddsgen` 4.7.0 generated C# used for source-shape
comparison. RTI acceptance is reference data only; it does not imply that the
managed generator supports the same feature.

Rejected cases may have no generated C# source, while remaining in the case
inventory so accepted, rejected, and observed outcomes stay explicit.

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
dotnet test tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
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
2. Add required defines, include roots, and observed outcomes to
   `manifest.json` where applicable.
3. Update [FEATURE-COVERAGE.md](FEATURE-COVERAGE.md) with the case's feature,
   `C###` tag, IDL link, oracle link, and managed status.
4. Run the applicable corpus, package, and integration tests.

Keep one feature theme per case, use explicit roots, and keep exact corpus
evidence separate from runtime or interoperability claims.

## Review rules

- Do not use dated directories inside `idl/` or `oracles/`.
- Do not make tests depend on external capture history.
- Keep RTI acceptance separate from managed-generator support status.
- Keep runtime, serialization, and wire claims out of source-generation status.
- Review the RTI license status before committing or sharing generated output.

## Exporting managed-generator sources

Set `CORPUS_GENERATOR_EXPORT_ROOT` and run the corpus export test to publish
generated sources under `generator/<source-kind>/<case-id>`:

```powershell
$env:CORPUS_GENERATOR_EXPORT_ROOT = "docs/corpus/generator"
dotnet test tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --filter-method ExportAcceptedCorpusSources
```

Rejected cases are omitted because they do not produce generated sources.
