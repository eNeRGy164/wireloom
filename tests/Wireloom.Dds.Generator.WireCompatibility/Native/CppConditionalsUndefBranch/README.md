# `11-conditionals` `UndefBranch` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom output for the exact conditional corpus IDL. The IDL defines
`CORPUS_N` as `2` and emits `Enabled`, then undefines the macro and emits
`UndefBranch` under `#ifndef CORPUS_N`; the `Disabled` branch is not emitted.
This fixture qualifies only `CorpusConditionals::UndefBranch`; `Enabled` is
present in the same generated translation unit but is outside these assertions.

The checks cover the selected branch's API, DynamicType, registration, local
XCDR1/XCDR2 bytes, and all six Linux DDS peer exchanges. Run in the pinned RTI
Connext 7.7.0 Linux amd64 image with the repository at `/work` and the RTI
license mounted read-only. Neither generator is invoked.
