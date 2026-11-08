# `08-key-inherited` Base Modern C++ consumer

This fixture uses the retained corpus IDL and compiles one consumer source
against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions are
scoped to `CorpusInheritedKeys::Base`; the sibling `Derived` declaration is
not directly tested by this fixture.

The checks cover the Base public API, member defaults and value construction,
DynamicType names/order/kinds/key flags, native TypeCode member flags, type
registration and DataWriter construction, key-based instance identity, local
XCDR1 and XCDR2 round trips and byte-for-byte peer equality, and Linux live DDS
exchange among RTI and Wireloom peers for both representations.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image. Mount the repository at
`/work`; configure the RTI license as a read-only secret mount. The fixture does
not invoke either generator.
