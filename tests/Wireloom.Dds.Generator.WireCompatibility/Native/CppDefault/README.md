# `09-default` `Message` Modern C++ consumer

This fixture uses the retained `09-default.idl` and compiles the same consumer
against RTI 7.7.0 generated C++ and Wireloom generated C++. Assertions are
scoped to `CorpusDefaultExtensibility::Message` only.

The consumer verifies the default RTI extensibility profile for an unannotated
struct, implicit member IDs 0 and 1, bounded-string metadata, keyless
registration, and DataWriter creation. Default, populated, and 16-character
values are round-tripped in XCDR1 and XCDR2 with exact peer byte comparison.
Six Linux live DDS pairings exchange a populated Message under both
representations. This fixture does not test cross-schema evolution.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
