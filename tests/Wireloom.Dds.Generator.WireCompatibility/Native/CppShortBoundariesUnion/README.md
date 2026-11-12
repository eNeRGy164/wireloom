# Short discriminator boundaries (C132 corpus tag)

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-short-boundaries.idl`. CMake compiles the
same `peer.cpp` consumer against retained RTI 7.7.0 output and Wireloom output
using C++11 on Windows x64.

The consumer checks the signed-short discriminator API, the explicit minimum
and maximum labels, the default long branch, `UnionType` and native TypeCode
metadata, and typed registration with `Topic<Choice>` and
`DataWriter<Choice>`. Local captures cover the default value, both boundary
branches, and the fallback branch at an unused discriminator under XCDR1 and
XCDR2. CTest compares every complete RTI and Wireloom capture byte for byte.

This qualifies local metadata, registration, and CDR only for this exact IDL
shape. It does not test live DDS exchange or qualify other discriminator or
branch combinations. See the
[RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md)
report and the [Modern C++ generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md).
