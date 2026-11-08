# `08-key-boundaries` BoundedStringKey Modern C++ consumer

This fixture uses the retained `08-key-boundaries.idl` and compiles one
consumer against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions
are scoped to `CorpusKeyBoundaries::BoundedStringKey`; ArrayKey and
UnboundedStringKey are not directly tested by this fixture.

The checks cover the shared `std::string` consumer API, values at the empty,
single-character, and 12-character IDL bound, DynamicType/native key flags,
registration/DataWriter and key-handle behavior, default/short/maximum-bound
XCDR1/XCDR2 byte equality, and six Linux live DDS peer pairings.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and RTI license mounted read-only. Neither generator is invoked.
