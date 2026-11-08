# `09-evolution-optional` `Message` Modern C++ consumer

This fixture compiles a shared consumer against the retained RTI 7.7.0 and
Wireloom C++ outputs for the exact corpus IDL. Its scope is
`CorpusOptionalEvolution::Message` only.

It checks required and optional Modern C++ members, absent/present values
(including explicit zero and empty string), DynamicType member IDs and bounds,
registration/DataWriter, exact XCDR1/XCDR2 bytes, and six Linux DDS peer
exchanges. It does not qualify cross-schema evolution.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
