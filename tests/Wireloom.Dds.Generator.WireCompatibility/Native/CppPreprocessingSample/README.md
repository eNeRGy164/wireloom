# `11-preprocessing` `Sample` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom output for the exact corpus IDL and its included `common.idl` closure.
The matrix row and assertions qualify `CorpusPreprocessing::Sample`; the
included `IncludedType` is inspected only as the nested member type.

Checks cover constants selected during preprocessing, default values, the
nested DynamicType graph, registration, XCDR1/XCDR2 bytes, and all six Linux
DDS peer exchanges. Run in the pinned RTI Connext 7.7.0 Linux amd64 image with
the repository at `/work` and the RTI license mounted read-only. Neither
generator is invoked.
