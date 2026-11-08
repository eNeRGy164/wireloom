# C++ array-of-sequences wire fixture

This fixture compiles the RTI 7.7.0 C++ oracle for `05-array-of-sequences`
against Wireloom's retained C018 and C149 outputs. C018's and C149's IDL files
are text-identical. The corpus contains an RTI C++ oracle for C018 only, so that
oracle is the comparison peer for both Wireloom outputs; C149 is reported as an
IDL-equivalent Wireloom run, not as a separate RTI capture.

The test executables register each Wireloom topic and create a typed
`DataWriter<Sample>`. They then round-trip `sequence-empty`,
`sequence-unequal`, `sequence-mixed-empty`, and `sequence-boundary` values with
XCDR1 and XCDR2 through RTI's local CDR helpers. Each Wireloom byte stream is
compared with the RTI-generated peer. These checks do not exchange samples
between DDS participants.

Configure with the RTI 7.7.0 environment and run CTest:

```powershell
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppArraySequences `
  -B artifacts/cpp-array-sequences -A x64
cmake --build artifacts/cpp-array-sequences --config Release
ctest --test-dir artifacts/cpp-array-sequences -C Release --output-on-failure
```
