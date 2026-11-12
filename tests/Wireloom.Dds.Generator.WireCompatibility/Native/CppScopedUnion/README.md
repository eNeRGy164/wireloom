# C024 scoped aggregate and sequence union

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-scoped.idl`. CMake compiles the same
`peer.cpp` consumer against the retained RTI 7.7.0 generated files and the
Wireloom generated files. It requires RTI Connext DDS 7.7.0 and a Windows x64
MSVC toolchain.

The shared consumer checks the public union API, unmatched default
discriminator zero, inactive-branch exceptions, the `Payload` and bounded
sequence DynamicType metadata, native TypeCode links and flags, and typed
registration with `Topic<Choice>` and `DataWriter<Choice>`. Its local CDR
captures cover the default value, the aggregate branch, and empty, partial,
and full `sequence<long, 4>` branches under XCDR1 and XCDR2. CTest compares each
complete RTI and Wireloom capture byte for byte.

This is local metadata, registration, and CDR evidence for this corpus shape.
The fixture does not test live DDS exchange or qualify other aggregate or
collection-bearing union forms. See
[RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md)
and the [Modern C++ generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md).
