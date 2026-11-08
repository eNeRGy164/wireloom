# `08-key-union` `Identity` Modern C++ consumer

This fixture uses the retained `08-key-union.idl` and compiles the same consumer
against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions and
evidence are scoped to `CorpusUnionKeys::Identity`; the sibling `Sample` topic
is not tested.

The consumer checks the union API and active-branch rules, DynamicType/native
TypeCode metadata, standalone type registration and DataWriter creation, and
keyless instance registration. Default, number, and text states are round-
tripped in XCDR1 and XCDR2 with exact RTI/Wireloom byte comparison. Six Linux
live DDS pairings exercise both encodings and directions using the text branch.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
