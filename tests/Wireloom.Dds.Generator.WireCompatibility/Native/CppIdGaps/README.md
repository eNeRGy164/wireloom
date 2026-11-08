# `09-id-gaps` `Message` Modern C++ consumer

This fixture uses the retained `09-id-gaps.idl` and compiles the same consumer
against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions are
scoped to `CorpusIdGaps::Message` only.

The consumer checks the appendable topic API, member IDs 1 and 7 in
DynamicType, registration/DataWriter, four values exercising each member and
both together, exact XCDR1/XCDR2 captures, and six Linux DDS peer exchanges.
It does not test cross-schema evolution.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
