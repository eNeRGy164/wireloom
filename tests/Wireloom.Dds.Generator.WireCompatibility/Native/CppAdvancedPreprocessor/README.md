# `11-preprocessor-advanced` `CorpusAdvancedPreprocessor::Sample` consumer

This fixture compiles one C++11 consumer against the retained RTI 7.7.0 and
Wireloom outputs for the exact corpus IDL. It verifies the macro arithmetic,
line continuation, variadic first-argument macro, and conditional branch by
checking the generated global `Selected` constant is 9. No command-line define
is supplied, so the `#elif 1` branch evaluates `CORPUS_BASE` as 7 and adds 2.

The consumer also checks Sample's public API, DynamicType, registration,
XCDR1/XCDR2 bytes, and all six Linux DDS peer exchanges. Run in the pinned RTI
Connext 7.7.0 Linux amd64 image with the repository at `/work` and the RTI
license mounted read-only. Neither generator is invoked.