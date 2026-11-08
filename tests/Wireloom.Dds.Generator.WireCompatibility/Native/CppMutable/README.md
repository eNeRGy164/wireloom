# `09-extensibility` `Mutable` Modern C++ consumer

This fixture uses the retained `09-extensibility.idl` and compiles the same
consumer against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions
are scoped to `CorpusExtensibility::Mutable`; sibling `Appendable` and `Final`
are not tested here.

The consumer checks the public fields and optional presence/reset/value API,
RTI's mutable topic trait, DynamicType extensibility and member IDs, native
optional flags, keyless registration and DataWriter creation. It also verifies
that RTI's `omg::types::optional<int32_t>` is an alias of
`dds::core::optional<int32_t>`. Default, absent, explicitly present zero, and
present nonzero values are round-tripped under XCDR1 and XCDR2 with exact peer
byte comparison. Six Linux live DDS pairings send both absent and present
optional values under both representations. This fixture does not test
cross-schema behavior.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
