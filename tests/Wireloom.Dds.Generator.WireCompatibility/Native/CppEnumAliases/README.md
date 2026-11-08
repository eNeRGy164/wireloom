# 03-enums-aliases bounded sequence alias fixture

This fixture compiles the retained RTI output and the retained Wireloom output
as separate C++11 peers. It checks typed registration and writer creation for
`CorpusEnums::Record`, then compares local XCDR1 and XCDR2 serialization for
`Record.colors` lengths 0, 1, and the IDL bound 3. The sample field is the
`ColorSequence` alias, so this exercises the RTI 7.7 alias TypeCode chain and
the underlying sequence sample-access helper.

All ten CTest cases pass. This qualifies the checked alias-bearing shape for
RTI 7.7 local CDR and typed registration/writer creation. The fixture does not
exchange samples between live DDS peers or qualify other RTI profiles.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
