# `08-key-boundaries` ArrayKey Modern C++ consumer

This fixture uses the retained `08-key-boundaries.idl` and compiles one
consumer against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions
are scoped to `CorpusKeyBoundaries::ArrayKey`; the bounded and unbounded string
key declarations are not directly tested by this fixture.

Checks cover the fixed-array C++ API, DynamicType/native key flags, type
registration and DataWriter construction, instance-handle identity for both
array coordinates, default and populated XCDR1/XCDR2 round trips and exact peer
byte equality, plus the Linux live DDS peer matrix. Run in the pinned RTI
Connext 7.7.0 Linux amd64 image, with repository mounted at `/work` and RTI
license mounted read-only. Neither generator is invoked.
