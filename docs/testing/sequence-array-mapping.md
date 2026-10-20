# Sequence-array mapping verification

Verified locally on October 20, 2026, using SDK 10.0.402 under the repository's
10.0.400 SDK policy. This is local implementation and focused live DDS
evidence; CI, Coveralls, and full-profile wire qualification remain pending.

## Contract

`DdsIdlSequenceArrayMapping` sets the project default. File metadata
`SequenceArrayMapping` takes precedence. `RtiCSharp` is the unchanged default;
`Idl` preserves the fixed array and independently sized sequence elements.
The declaring file owns the mode, including shared includes. Invalid values
and recursive direct sequence-array elements in `Idl` mode fail with
source-located `DDSG0001` before any documents are returned.

See the [consumer settings](../../src/Wireloom.Dds.Generator/README.md#choose-the-array-of-sequences-mapping)
and [source-planning boundary](../architecture/arc42/05-building-block-view.md).

## Local results

| Check                                           | Result                                                                                          |
| ----------------------------------------------- | ----------------------------------------------------------------------------------------------- |
| Locked restore and Release build                | Passed; generator build has zero warnings/errors                                                |
| Unit suite with Cobertura coverage              | 1,359 passed                                                                                    |
| RTI 7.3.1 corpus                                | 323 passed; one intentional export skip                                                         |
| RTI 7.7.0 corpus                                | 323 passed; one intentional export skip                                                         |
| RTI 7.3.1 generated runtime suite               | 537 passed                                                                                      |
| RTI 7.7.0 generated runtime suite               | 537 passed                                                                                      |
| Fresh package, RTI 7.3.1, C# 12                 | 24 passed                                                                                       |
| Fresh package, RTI 7.7.0, C# 12                 | 24 passed                                                                                       |
| Incremental baseline and setting transitions    | Two tests passed; metadata changes update output/diagnostics and unchanged reruns remain cached |
| Community Qodana                                | Successful final scan; zero findings                                                            |
| RTI 7.3.1 C149 `Idl` live DDS case              | 16 / 16 exchanges passed                                                                        |
| RTI 7.7.0 C149 `Idl` live DDS case              | 16 / 16 exchanges passed                                                                        |
| Wire shard/report verification                  | 1,071 checks passed on each profile; 393 synthetic exchanges                                    |
| Wire harness finalization and fixture selection | Three failure/finalization checks and four fixture selections passed                            |
| Architecture memory                             | Current                                                                                         |

Branch coverage is **4,965 / 5,267 (94.27%)**; line coverage is
**7,282 / 7,348 (99.10%)**. The new sequence-array planner has 100% line
coverage and 97.37% branch coverage. The remaining planner branch is the null
source-input alternative used only by manually constructed semantic members;
parsed members retain their declaring input. Coveralls has not been run for
these changes before pushing.

The fresh-package tests use an isolated `0.4.0-issue58.1` package. They select
`Idl` project-wide and explicitly override the legacy fixture to `RtiCSharp`;
that fixture intentionally produces one non-fatal `DDSG0105` warning. Default
C018 source exports and oracle comparisons remain flattened and unchanged.

Qodana used the documented `WSLC` image with `global.json` visible and isolated
Linux intermediates. The final verified report is under local ignored
`.qodana/results-issue58-verified/`; analysis ends with `Done` and zero
findings. A preceding cache run produced stale-symbol findings despite a
successful compiler build; a fresh-cache scan and the final scan both passed.

## Reproduction

Use the locked restore, coverage, and `WSLC` Qodana commands in
[CONTRIBUTING](../../CONTRIBUTING.md#quality-evidence). Run the runtime suite
with `-p:RtiConnextDdsVersion=7.3.1` and `7.7.0`; select corpus profiles with
`CORPUS_RTI_PROFILE`. For a fresh package use the
[isolated-feed procedure](../../tests/Wireloom.Dds.Generator.PackageIntegration/README.md),
passing the same RTI version to restore and run. No projects or dependencies
were added.

The opt-in C149 corpus source export is reproduced with the ordinary corpus:

```powershell
$env:CORPUS_RTI_PROFILE = '7.7.0' # Repeat with 7.3.1.
$env:CORPUS_GENERATOR_EXPORT_ROOT = 'artifacts/c149-idl-7.7.0-fresh'
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj `
  --no-restore --configuration Release -- --filter-method '*ExportAcceptedCorpusSources'
```

Use a fresh output directory. The export includes C149 in its normal
`features/05-array-of-sequences-idl` directory and does not constitute a
licensed DDS exchange.

## C++ payload evidence and qualification limits

The production generator's C149 sources compile against each exact RTI
package. Independent typed probes preserve two numeric sequences and two
bounded-string sequences. RTI-generated C++ from C149's duplicate IDL
decodes the Wireloom payload; Wireloom decodes an independently constructed
C++ payload. Both directions passed with RTI 7.3.1 and 7.7.0. The C++ generated
sources were not patched; the test harness uses each version's normal field
or accessor API. Local scratch sources, binaries, and results are under
ignored `artifacts/issue-58/implementation/`.

These checks establish CDR payload compatibility for the exercised values.
The retained live corpus case below additionally checks DDS discovery and payload
exchange. Neither result establishes all representations, Java/Python
interoperability, or recursive sequence-array support. RTI C#/Python's default
flattened direct-form bindings describe a different type.

## Retained corpus case and live DDS results

The shared manifest's C149 entry, `05-array-of-sequences-idl`, uses a
separately named IDL file with the same declarations as C018 and sets
`sequenceArrayMapping: "Idl"`. Both profile trees retain its 12 generated
sources in ordinary feature folders. The compliance suite compares those
sources with fresh generation. Default C018 exports and RTI C# oracle
comparisons remain unchanged.

The production wire runner builds typed Wireloom C# and unmodified RTI C++
peers. Four fixtures cover empty sequences, unequal per-element lengths,
mixed empty/populated elements, signed numeric limits, and the string and
sequence bounds. The managed reader checks array dimensions and each sequence
against literal values; the C++ reader compares a separately populated typed
sample. RTI C# is explicitly excluded because its direct-form binding
describes the flattened type. Reports show the five excluded pairings as N/A
and retain the exclusion reason.

On October 20, 2026, all four directed Wireloom/C++ pairings passed for every
fixture on exact RTI 7.3.1 and 7.7.0: **16 / 16 per profile, 32 / 32 total**.
Each completed exchange received a valid sample and verified its payload.
These are focused results for the recorded fixtures, not full-profile or
all-language qualification.

Local reports, plans, build logs, sanitized endpoint logs, and the actual
generated-source hashes are under ignored
`artifacts/issue-58/wire-variant/<version>/wire-verified/`. The runs used the
cached `wireloom-rti-<version>:scalar-verification` helper images, with image
IDs `c61e8c972dcf` (7.3.1) and `6db432346e9f` (7.7.0), isolated Linux source
copies, and a read-only license mount outside the repository. No license
contents are retained. Initial setup-failure evidence is preserved separately
under each profile's `wire/` directory; successful reports are in
`wire-verified/`.

Compiler-emitted files include a UTF-8 byte-order mark; programmatic corpus
exports do not. At the time of the live runs, the 12 actual wire-generated
sources on each profile matched the exports after accounting for that prefix.
Provenance hashes identify the original file bytes and retain that difference.

A subsequent formatting update groups array allocations and clone assignments
with their initialization/copy loops. Nine retained source files changed only
in whitespace, and the C149 exports were refreshed. The original
live-run hashes remain unchanged; the live exchanges were not repeated for
this formatting update.

Generated documentation now mentions the effective unbounded-sequence limit
only when the IDL omits a sequence bound. Explicitly bounded sequences,
including a declared bound of 100, omit that explanation. This applies to
ordinary sequence members, explicit typedefs, and generated sequence-array
wrappers. Retained documentation and C149 sources were refreshed; executable
declarations and the original live-run evidence remain unchanged.
The unbounded-sequence note uses the neutral wording "an effective limit of
100 elements is set"; both emitters and their retained outputs use that text.

Reproduce with the [focused wire command](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#idl-sequence-array-corpus-case)
on each exact profile. The runner records mode, runtime, IDL SHA-256, and
generated-source hashes; shard merging preserves this provenance and rejects
missing or mismatched completed evidence. Reports keep the default case's
known failures separate. The new fixture verifier also passed a local smoke
check that rejects swapped array elements. Community Qodana reported zero
findings; it targets maintained generator source, not the wire harness.

## Architecture impact

Architecture is ready for this opt-in implementation. Chapters 2, 4, 5, 6, 8,
9, 10, 11, and 12 document configuration ownership, emission planning, error
paths, the compatibility trade-off, evidence, and terminology. Chapters 1 and
3 retain the same goals and external owners; chapter 7 retains the same
compile-time NuGet deployment and RTI runtime boundary. No new service,
runtime dependency, or environment assumption is introduced. Derived memory
is refreshed from those sources. Chapters 6 and 10 also describe the retained
corpus case, peer exclusions, source provenance, and completed
focused exchanges. Other architecture boundaries and deployment assumptions
remain unchanged. Release qualification remains limited by the outstanding
CI/Coveralls and broader DDS checks above.
