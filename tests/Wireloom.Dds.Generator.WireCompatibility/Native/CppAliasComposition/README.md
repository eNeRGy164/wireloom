# `06-alias-composition` C++11 runtime fixture

This fixture compiles the retained RTI 7.7 C++ oracle and Wireloom output for
the exact corpus IDL at `docs/corpus/idl/features/06-alias-composition.idl`.
It checks `Identifier` alias identity, the `State` enum ordinals, the bounded
sequence member and nested `Named::sample` DynamicType metadata. It registers
both struct topic roots (`Sample` and `Named`) and creates typed DataWriters.

The default and non-default values are serialized as both `Sample` and nested
`Named` values under XCDR1 and XCDR2. Each peer round-trips its values, and
CTest compares the complete RTI and Wireloom byte streams. This local CDR
fixture does not claim live DDS exchange.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
