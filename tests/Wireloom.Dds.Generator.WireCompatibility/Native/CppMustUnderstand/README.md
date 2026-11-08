# `09-must-understand` `Sample` Modern C++ consumer

This fixture compiles the same consumer against the retained RTI 7.7.0 and
Wireloom C++ outputs for the exact corpus IDL. It is scoped only to
`CorpusMustUnderstand::Sample`.

It checks the required scalar Modern C++ API, DynamicType member ID 0 and
`must_understand(true)`, appendable type metadata, keyless registration,
XCDR1/XCDR2 byte equality, and six Linux DDS peer exchanges.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
