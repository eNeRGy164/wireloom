# C025 multi-label union

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-multilabel.idl`. CMake compiles the same
`peer.cpp` consumer against retained RTI 7.7.0 output and Wireloom output with
C++11 on Windows x64.

The consumer verifies RTI's one-argument `number(value)` default label and
`number(value, discriminator)` overload for labels 1 and 5, including invalid
label exceptions. It checks inactive access, the `text` branch at label 2,
`UnionType` and native TypeCode labels, and typed topic registration with a
`DataWriter<Choice>`. Default, both number labels, and text values round-trip
locally under XCDR1 and XCDR2; CTest compares complete capture files byte for
byte.

This is local metadata, registration, and CDR evidence for this exact corpus
shape. It does not test live DDS exchange or qualify other union forms. See
[`C025-07-union-multilabel.windows-x64.json`](../../../../docs/corpus/7.7.0/cpp-evidence/C025-07-union-multilabel.windows-x64.json)
and the [C++ coverage matrix](../../../../docs/corpus/CPP-COVERAGE.md).
