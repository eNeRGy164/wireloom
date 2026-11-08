# C# corpus compliance tests

`Wireloom.Dds.CSharp.CorpusCompliance.csproj` is the manifest-driven test
project for the central IDL corpus under `docs/corpus`.

The Quality workflow runs generation and exact-runtime compilation for all
three profiles on every pull request and push to `main`, using explicit
`CORPUS_RTI_PROFILE` values. Detailed oracle comparisons additionally run for
7.3.1 and 7.7.0. Runtime and fresh-package jobs separately cover those two
packages on .NET 8 and .NET 10 across Windows and Ubuntu.

See the [LTS reproduction runbook](../../docs/corpus/LTS-PROFILES.md) for fresh
oracle capture and export evidence. `Export-CorpusGeneratorSources.ps1` performs
locked restore, runs Release tests, and retains per-profile source/input/output
hashes and test logs. Select a new output root to avoid stale export files.
The exporter defaults to all three profiles; `-Profile` selects one version.
[CorpusProfile.cs](CorpusProfile.cs) defines each compiler profile and its
unavailable cases. Generation and export use
[CorpusGenerationSpecs.cs](CorpusGenerationSpecs.cs); detailed oracle checks
use [CorpusComplianceSpecs.cs](CorpusComplianceSpecs.cs).

The tests deliberately load the JSON manifest and discover the IDL and oracle
files from the repository. They therefore cover every positive, negative, and
integration entry without duplicating fixture lists in the project file.

The manifest buckets describe how cases are organized; they do not determine
the managed generator's expected result. The explicit
[managed acceptance map](CorpusRepository.cs) is authoritative for that
expectation:

- Cases in the map are expected to generate C# successfully. This includes all
  positive feature cases and integration entry points, plus selected probes in
  the negative bucket, such as `12-unknown-annotation`, `12-interface`, and
  `12-union-wchar-rti-limit`.
- Acceptance is profile-aware where RTI APIs impose a target limitation:
  `09-must-understand` (C142) is rejected with `DDSG0004` and no generated sources under
  the 6.1.2 and 7.3.1 profiles, while it remains accepted under 7.7.0. Hash member IDs
  remain independently covered by `09-autoid-hash` (C040) in all three profiles.
- Every oracle read uses the selected `CORPUS_RTI_PROFILE` (7.7.0 by default),
  including inventory, constructors, general shape checks, and targeted metadata
  comparisons. The 7.3.1 capture manifest supplies its source directories and
  generation outcomes; it is never compared against 7.7.0 sources.
- The optional-recursive-array comparison uses the matching RTI oracle. It
  expects the 7.7.0 `isRecursiveType` flag and member-annotation APIs only in
  that profile; 7.3.1 instead uses its `xTypesComplianceMask` and has no member
  annotation metadata API.
- Cases outside the map are expected to throw `IdlException`, regardless of
  their manifest bucket.
- For accepted cases with retained RTI oracle output, the suite checks required
  data-contract types, property shapes, and enum members. This is structural
  evidence, not exact source, runtime, or wire equivalence.
- The [acceptance test](CorpusComplianceSpecs.cs) checks each case against the
  map. The inventory test checks that manifest cases and oracle directories
  remain aligned.

RTI outcomes remain separate evidence recorded in the manifest. RTI accepting
or ignoring a declaration does not determine whether Wireloom accepts it, and
the negative folder is not a list of constructs that Wireloom must reject.

## Generation and exact-runtime compilation

`CorpusGenerationSpecs` uses C# 12 and the exact downloaded assembly for the
selected profile. All 175 inventory entries use the same expected-acceptance
and compilation checks. Unsupported cases must produce `DDSG0004` and no
documents; their feature names come from `CorpusProfile` for the selected
version. Export is opt-in through
`CORPUS_GENERATOR_EXPORT_ROOT` and uses the same compilation check before
writing each accepted case. Generation and compilation do not establish
runtime or wire qualification.

Use `-- --filter-trait CorpusSuite=Generation` for any profile, or
`-- --filter-trait CorpusSuite=Oracle` for detailed 7.3.1/7.7.0 oracle checks.
