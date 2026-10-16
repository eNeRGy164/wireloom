# Corpus compliance tests

`Wireloom.Dds.Generator.CorpusCompliance.csproj` is the manifest-driven test
project for the central IDL corpus under `docs/corpus`.

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
