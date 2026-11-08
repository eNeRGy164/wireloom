# `09-defaults-ranges` `Sample` Modern C++ consumer

This fixture compiles the same consumer against the retained RTI 7.7.0 and
Wireloom outputs for the exact corpus IDL. It tests only
`CorpusDefaultsAndRanges::Sample`; `Color` is not directly qualified here.

It checks IDL field defaults, inclusive min/max metadata, `@range` metadata,
member IDs and types, the `@default(BLUE)` enum field default, keyless
registration, XCDR1/XCDR2 exact bytes, and six Linux DDS peer exchanges.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
