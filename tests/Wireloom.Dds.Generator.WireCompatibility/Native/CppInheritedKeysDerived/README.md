# `08-key-inherited` Derived Modern C++ consumer

This fixture uses the retained corpus IDL and compiles a Derived consumer
against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions are
scoped to `CorpusInheritedKeys::Derived` and its inherited Base subobject.

The fixture checks each peer's public constructor shape, DynamicType and native
TypeCode inheritance/key metadata, type registration and DataWriter creation,
instance handles for inherited and local key changes, XCDR1/XCDR2 captures, and
the six RTI/Wireloom live DDS pairings on Linux. The report records a public
constructor-shape difference as a Modern C++ parity failure if reproduced.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
