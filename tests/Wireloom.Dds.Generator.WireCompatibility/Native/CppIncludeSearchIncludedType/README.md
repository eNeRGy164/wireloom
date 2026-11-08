# `11-include-search` `IncludedType` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom outputs for the exact include-search corpus IDL. It qualifies global
`IncludedType` as a standalone Modern C++ topic. The separate
`CorpusIncludeSearch::Sample` row is not part of these assertions.

The checks cover the type's public API, DynamicType and native TypeCode,
standalone registration, XCDR1/XCDR2 bytes, and all six Linux DDS peer
exchanges. RTI retains the common include closure as generated files under
`includes/`; Wireloom includes the common declarations in the case's root
output. Both are compiled directly. Run in the pinned RTI Connext 7.7.0 Linux
amd64 image with the repository at `/work` and the RTI license mounted
read-only. Neither generator is invoked.