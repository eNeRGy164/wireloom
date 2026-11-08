# `11-include-search` `CorpusIncludeSearch::Sample` consumer

This fixture uses the exact corpus IDL, which includes `common.idl` through both
`<common.idl>` and `"includes/common.idl"`. RTI retains the included type under
`includes/common.hpp` and emits separate included implementation/plugin files;
its root header also emits both `common.hpp` include spellings. Wireloom
flattens the included type and constant declarations into the root output.
Both output layouts compile directly without a fixture shim.

The consumer checks the included constant, Sample's nested IncludedType API
and DynamicType, registration, XCDR1/XCDR2 bytes, and all six Linux DDS peer
exchanges. Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the
repository at `/work` and the RTI license mounted read-only. Neither generator
is invoked.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image with the repository at
`/work` and the RTI license mounted read-only. Neither generator is invoked.
