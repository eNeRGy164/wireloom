# `11-conditionals` `Enabled` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom output for the exact conditional corpus IDL. It qualifies only
`CorpusConditionals::Enabled`; `UndefBranch` and the alternate `Disabled`
branch are outside this fixture's assertions.

The checks cover the selected branch's API, DynamicType, registration, local
XCDR1/XCDR2 bytes, and all six Linux DDS peer exchanges. Run in the pinned RTI
Connext 7.7.0 Linux amd64 image with the repository at `/work` and the RTI
license mounted read-only. Neither generator is invoked.
