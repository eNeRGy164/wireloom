# 03-alias-collections nested bounded sequence fixture

This fixture compiles the retained RTI output and retained Wireloom output as
separate C++11 peers. It registers `CorpusCollectionAliases::Sample` and
creates a typed topic and DataWriter. The sample's `NestedSequence` alias
contains `LongSequence` aliases; the test compares XCDR1 and XCDR2 CDR bytes
for empty outer storage, one empty inner sequence, one partial inner sequence,
and full outer/inner lengths (2 and 3).

The fixture qualifies nested bounded sequence aliases with primitive leaves
for RTI 7.7 typed registration/writer creation and local serialization only.
It does not exchange samples between live DDS peers or qualify other RTI
profiles.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
