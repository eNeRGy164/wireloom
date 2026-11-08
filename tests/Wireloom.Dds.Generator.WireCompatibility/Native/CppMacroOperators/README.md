# `11-macro-operators` `CorpusMacroOperators::Sample` Modern C++ consumer

This fixture compiles the same C++11 consumer against retained RTI 7.7.0 and
Wireloom output for the exact corpus IDL. `CORPUS_CAT(fi,eld)` expands the
member name to `field`, and `CORPUS_STR(sample)` expands the `Name` constant to
`"sample"`. The consumer checks those results and the public API shape of the
constant from each generator.

The generated APIs differ for `Name`: RTI declares `omg::types::string_view`,
while Wireloom declares `const char*`. Both carry the same string value, but a
consumer using the RTI string-view API is not source-compatible with the
Wireloom declaration. This mismatch is recorded as a Modern C++ binding parity
failure; it does not block the separate runtime and wire checks.

The remaining checks cover Sample's API, DynamicType, registration, local
XCDR1/XCDR2 bytes, and all six Linux DDS peer exchanges. Run in the pinned RTI
Connext 7.7.0 Linux amd64 image with the repository at `/work` and the RTI
license mounted read-only. Neither generator is invoked.