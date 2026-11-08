# `10-annotations` `Sample` Modern C++ consumer

This fixture compiles one C++11 consumer against the retained RTI 7.7.0 and
Wireloom outputs for the exact corpus IDL. It tests only
`CorpusAnnotations::Sample`.

The checks cover default construction, the keyed `id` member, the defaulted
`value` member, DynamicType and native TypeCode annotations, key registration,
XCDR1/XCDR2 bytes for default and alternate samples, and all six Linux DDS peer
exchanges. Run it in the pinned RTI Connext 7.7.0 Linux amd64 image with the
repository at `/work` and the RTI license mounted read-only. Neither generator
is invoked.
