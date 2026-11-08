# `09-extensibility` `Appendable` Modern C++ consumer

This fixture uses the retained `09-extensibility.idl` and compiles the same
consumer against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions
are scoped to `CorpusExtensibility::Appendable`; sibling `Final` and `Mutable`
are not tested here.

The consumer checks the public fields and value operations, RTI's extensible
topic trait, DynamicType extensibility and member IDs, keyless registration and
DataWriter creation. Default, populated, and 16-character-bound values are
round-tripped under XCDR1 and XCDR2 with exact peer byte comparison. Six Linux
live DDS pairings send a populated Appendable value under both representations.
This fixture does not claim cross-schema appendable evolution behavior.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
