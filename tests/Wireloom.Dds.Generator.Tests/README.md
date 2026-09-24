# Wireloom generator tests

This project is the fast, self-contained unit-test layer for the IDL front end
and managed source emitters. Test inputs are created in memory with
`CompilerTestSupport`; the tests do not depend on the repository corpus, RTI
generated files, or a live DDS runtime.

When an inline test directly characterizes a central corpus case, it carries
an xUnit trait such as `Trait("Corpus", "C039")`. The synthetic input file name
also starts with the same tag so failures retain useful provenance without
introducing a file-system dependency.

The exact corpus files and their RTI-generated oracle sources are exercised by
`tests/CorpusCompliance`. That project is the authoritative check for complete
positive, negative, and integration corpus coverage; this project intentionally
does not duplicate its manifest-driven loading or oracle comparison.

Tests that cover a general compiler invariant rather than one specific corpus
case may have no `Corpus` trait.
