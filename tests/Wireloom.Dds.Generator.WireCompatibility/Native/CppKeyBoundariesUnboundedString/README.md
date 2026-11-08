# `08-key-boundaries` UnboundedStringKey Modern C++ consumer

This fixture uses the retained `08-key-boundaries.idl` and compiles one
consumer against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions
are scoped to `CorpusKeyBoundaries::UnboundedStringKey`; ArrayKey and
BoundedStringKey are not directly tested by this fixture.

The consumer covers the shared `std::string` API, DynamicType and key metadata,
registration/DataWriter key identity, empty/short/255-character local CDR
round trips and exact peer bytes, RTI/Wireloom acceptance probes at lengths 255
and 256, and six Linux live DDS pairings using the 255-character value.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and RTI license mounted read-only. Neither generator is invoked.
