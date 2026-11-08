# Numeric character discriminator union

This fixture uses the exact C137 corpus IDL at
`docs/corpus/idl/features/07-union-char-numeric.idl`. CMake compiles the same
`peer.cpp` consumer against retained RTI 7.7.0 output and Wireloom output with
C++11 on Windows x64.

The consumer verifies the plain `char` discriminator API, numeric label 127,
default `long` branch, `UnionType` and native TypeCode metadata, and typed
registration with `Topic<Choice>` and `DataWriter<Choice>`. It exercises
default construction, the explicit 127 branch, and the fallback branch at an
unmatched discriminator. Local captures cover all three states under XCDR1
and XCDR2; CTest compares complete RTI and Wireloom frames byte for byte.

The API check also compares RTI and Wireloom swap behavior between values on
different branches, using the RTI two-pass active-branch algorithm. This
qualifies local API, metadata, registration, and CDR only for this exact IDL
shape. It makes no live DDS or Linux claim. See the
[`C137 evidence report`](../../../../docs/corpus/7.7.0/cpp-evidence/C137-07-union-char-numeric.windows-x64.json)
and [C++ coverage matrix](../../../../docs/corpus/CPP-COVERAGE.md).
