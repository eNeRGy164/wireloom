# Unbounded typedef oracle evidence

Cases C135 (`04-unbounded-string-aliases`) and C136
(`05-unbounded-sequence-aliases`) cover the forms missed before runtime finding
F1. The sequence case includes primitive, enum, aggregate, scalar typedef,
narrow/wide string, explicitly bounded string elements, and a sequence typedef
chain. Their stable tags leave earlier corpus references unchanged.

## Reference capture

RTI Connext DDS 7.7.0.1 / rtiddsgen 4.7.0.1 accepted both inputs on
2026-10-15 with preprocessing disabled. The four retained files under
`oracles/features` preserve the generated notices. No oracle edits or runtime
adaptations were needed. Nested string type arguments use separate `> >`
tokens because RTI rejects `>>` in this context.

```powershell
New-Item -ItemType Directory -Force tmp/rti-unbounded | Out-Null
wslc run --rm -v "${PWD}:/work" -w /work rticom/connext-base:7.7.0.1 `
  rtiddsgen -replace -ppDisable -language 'C#' -d /work/tmp/rti-unbounded `
  /work/docs/corpus/idl/features/04-unbounded-string-aliases.idl `
  /work/docs/corpus/idl/features/05-unbounded-sequence-aliases.idl
```

## Executable RTI C# baseline

The existing RTI oracle probe project can select a typed entry point and the
two retained source/plugin pairs. It compiles them at build time against the
existing locked `Rti.ConnextDds` 7.7.0 dependency. It uses direct typed calls;
there is no runtime source compilation or reflection-based invocation.

```powershell
$probe = 'tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/RtiOracleProbe.csproj'
dotnet restore $probe --locked-mode
dotnet run --project $probe --no-restore --configuration Release `
  -p:RtiOracleProgram=UnboundedAliasProbe.cs `
  '-p:RtiOracleSourceDir=../../../docs/corpus/oracles/features/*-unbounded-*-aliases'

$env:CORPUS_GENERATOR_EXPORT_ROOT = 'docs/corpus/generator'
dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance --no-restore --configuration Release
```

The Quality workflow runs this probe on Windows and Linux. Prerequisite or
assertion failures fail the job. The probe checks constructors, null copies,
value equality/hash consistency, mutation independence, both metadata
visibilities, and native conversion for all 11 shapes across eight
initialization/key-only combinations. It owns native storage locally and uses
`try/finally`; `allocateMemory=false` starts with allocated backing storage.
Optional-only cleanup is followed by another conversion check before full
destruction.

| Observation                          | RTI C# 7.7.0                                                                      | Wireloom contract / disposition                                                                                               |
| ------------------------------------ | --------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| Unbounded narrow/wide string typedef | Empty default; effective bound 255 in attributes, native allocation, and metadata | F1 fix matches                                                                                                                |
| Unbounded sequence typedef           | Empty default; effective bound 100 in attributes, native allocation, and metadata | F1 fix matches                                                                                                                |
| Unbounded string sequence elements   | Element bound 255; explicit element bounds 8/4 preserved                          | F1 fix matches                                                                                                                |
| Null-source sequence typedef copy    | `Value` remains null                                                              | Existing characterization retained                                                                                            |
| Primitive sequence copy              | Independent container                                                             | Matches                                                                                                                       |
| Aggregate sequence copy              | Independent mutable elements                                                      | F5 is now a confirmed RTI baseline defect; Wireloom currently shares elements                                                 |
| Scalar typedef sequence elements     | Public `ISequence<int>`; element typedef erased                                   | Wireloom intentionally retains `ISequence<Scalar>`; corpus asserts both exact shapes rather than normalizing aliases globally |
| Wide string typedef metadata/support | Both metadata visibilities and support instance succeed                           | F3 remains a Wireloom defect for its affected bounded fixtures                                                                |

The local Windows (.NET 10.0.12) and Linux (.NET 10.0.11) runs each passed
205 assertions, including 88 native flag/shape combinations. Both new RTI
source/plugin pairs compile as captured. Corpus compliance and source export
passed all 280 checks, with no skips.

This is generated-source and runtime API evidence. These cases have no new
serialization or DDS wire exchange evidence; retaining scalar wrappers does
not by itself establish interoperability. CI and Coveralls confirmation await
push and workflow execution.
