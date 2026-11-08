# 09-optional-string-sequences native fixture

This fixture compiles the retained RTI 7.7.0 C++ oracle and retained Wireloom
C++ output as separate C++11 peers. The Wireloom peer registers
`CorpusOptionalStringSequences::Sample`, creates a typed topic and DataWriter,
and serializes local samples through the guarded RTI interpreter bridge.

For each peer, the CDR checks round-trip the sample and compare the exact CDR
bytes for XCDR1 and XCDR2. States cover both members absent, both present-empty,
narrow-only partial, wide-only partial, both partial, and both full. Partial
values contain an empty element; full values also contain a 16-character
boundary element. The sequences use bounds of 4 and each string uses bound 16.

This qualifies only optional direct
`sequence<string<16>, 4>` and `sequence<wstring<16>, 4>` members with RTI 7.7.0.0.
It does not qualify live DDS exchange, other string-sequence shapes, or other
RTI profiles.

Configure with Visual Studio 18 2026 and `CONNEXTDDS_ARCH=x64Win64VS2017`,
build Release, then run CTest.
