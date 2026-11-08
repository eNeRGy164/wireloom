# `04-boundaries` C++11 runtime fixture

This fixture compiles the retained RTI oracle and Wireloom output generated
from the same `docs/corpus/idl/features/04-boundaries.idl` corpus case. It
checks typed registration and `DataWriter` creation, then round-trips local
CDR through both generated types and compares complete XCDR1 and XCDR2 bytes.

The empty, partial, and boundary samples cover both `string<8>` and
`wstring<8>`. Boundary samples contain exactly eight characters in each
field. This qualifies the exact C015 value graph under RTI Connext DDS 7.7.0.0
and C++11; it does not claim live DDS exchange or other RTI profiles.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
