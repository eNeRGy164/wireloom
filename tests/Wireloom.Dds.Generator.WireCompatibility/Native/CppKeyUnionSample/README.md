# `08-key-union` `Sample` Modern C++ consumer

This fixture uses the retained `08-key-union.idl` and compiles one consumer
against RTI 7.7.0 generated C++ and Wireloom generated C++. Evidence is scoped
to `CorpusUnionKeys::Sample`; the `Identity` union is inspected only as a nested
member of this topic root. The standalone `Identity` row has separate evidence.

The consumer verifies the shared Sample/Identity API, nested DynamicType and
native TypeCode links, registration/DataWriter, and that the nested union key
uses both its discriminator and selected branch value while ignoring payload.
Default, number, and text key states are round-tripped in XCDR1 and XCDR2 with
exact peer byte comparison. Six Linux DDS pairings exchange a Sample with the
nested text key under both representations.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
