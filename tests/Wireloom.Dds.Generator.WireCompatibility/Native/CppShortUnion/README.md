# C026 short-discriminator union

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-short.idl`. CMake compiles one shared
`peer.cpp` consumer against the retained RTI 7.7.0 generated files and the
Wireloom output using C++11 on Windows x64.

The consumer checks the public discriminator and branch APIs, default
construction, inactive-branch exceptions, the `UnionType` labels and native
short/long/string/boolean TypeCodes, and typed `Choice` registration with
`Topic<Choice>` and `DataWriter<Choice>`. Local captures cover the default
value, the negative branch, the text branch, and a selected default branch
under XCDR1 and XCDR2. CTest compares each complete RTI and Wireloom capture
byte for byte.

This qualifies local metadata, registration, and CDR only for this exact IDL
shape. It does not test live DDS exchange or qualify other discriminator or
branch combinations. See the
[RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md)
report and the [Modern C++ generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md).
