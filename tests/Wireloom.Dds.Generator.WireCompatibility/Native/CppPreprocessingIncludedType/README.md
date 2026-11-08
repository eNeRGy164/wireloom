# `11-preprocessing` `IncludedType` Modern C++ consumer

This fixture compiles the same C++11 consumer against the retained RTI 7.7.0
include output and Wireloom output for the exact corpus IDL include closure.
It qualifies global `IncludedType` as a standalone Modern C++ topic; the
separate `CorpusPreprocessing::Sample` row is not part of these assertions.

The checks cover the included type's API, DynamicType and native TypeCode,
standalone registration, XCDR1/XCDR2 bytes, and all six Linux DDS peer
exchanges. Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the
repository at `/work` and the RTI license mounted read-only. Neither generator
is invoked.
