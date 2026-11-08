# `09-defaults-ranges` `Color` Modern C++ consumer

This fixture compiles the same standalone enum consumer against the retained
RTI 7.7.0 and Wireloom C++ outputs. Evidence is scoped to
`CorpusDefaultsAndRanges::Color`; the sibling `Sample` is not tested here.

The fixture compares enum values, the declared default enumerator, topic-type
applicability, DynamicType literals, and exact stream output. RTI's Modern C++
topic trait is false for this standalone enum, so registration, topic CDR, and
Live DDS are not applicable.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
