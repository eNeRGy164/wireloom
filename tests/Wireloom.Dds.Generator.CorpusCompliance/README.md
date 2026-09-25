# Corpus compliance tests

`Wireloom.Dds.Generator.CorpusCompliance.csproj` is the manifest-driven test
project for the central IDL corpus under `docs/corpus`.

The tests deliberately load the JSON manifest and discover the IDL and oracle
files from the repository. They therefore cover every positive, negative, and
integration entry without duplicating fixture lists in the project file.

- Positive and integration roots are required to compile.
- Negative roots are required to produce an `IdlException`.
- Accepted oracle cases are checked for data-contract type and property shape.
- The inventory test checks that the IDL and oracle case sets remain aligned.

RTI classifications are retained in the oracle manifest. They do not change
the managed test expectation for entries placed in the negative corpus: those
are rejection probes for the managed front end, including probes that RTI
accepted or ignored.

The `02-alias-cycle` probe is currently skipped during in-process compilation
because the managed alias resolver overflows the process instead of returning
an `IdlException`. It remains covered by the corpus inventory and should be
moved to an isolated compiler-process test when that harness is added.
