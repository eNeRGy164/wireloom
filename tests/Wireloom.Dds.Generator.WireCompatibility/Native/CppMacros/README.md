# `11-macros` `CorpusMacros::Sample` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom output for the exact corpus IDL. Its object-like macro expands the
member type to `long`, and its function-like macro expands the member name to
`value`. The consumer checks that those expansions produce the expected public
Modern C++ value and topic API.

The checks cover the sample API, DynamicType, registration, local XCDR1/XCDR2
bytes, and all six Linux DDS peer exchanges. Run in the pinned RTI Connext
7.7.0 Linux amd64 image with the repository at `/work` and the RTI license
mounted read-only. Neither generator is invoked.