# `09-autoid-hash` `Sample` Modern C++ consumer

This fixture compiles a shared consumer against retained RTI 7.7.0 and Wireloom
C++ outputs for the exact corpus IDL. It tests
`CorpusAutoId::Sample` only.

It checks the hashed DynamicType member IDs (75475440 and 206680604), the
`@hashid` fields' Modern C++ API, keyless registration, bounded string<16>,
XCDR1/XCDR2 byte captures, and six Linux DDS peer exchanges. The IDL has no
`@key` member, so member hash IDs are not qualified as DDS key behavior.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
