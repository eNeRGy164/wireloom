# `05-collections` C++11 runtime fixture

This fixture compiles the retained RTI oracle and Wireloom output generated
from the same `docs/corpus/idl/features/05-collections.idl` corpus case. It
checks typed registration and `DataWriter` creation, then round-trips local
CDR through each generated type and compares complete XCDR1 and XCDR2 bytes.

The empty, single, and full states exercise every member. The full state reaches
the RTI default finite fallback of 100 elements, the explicit bounded sequence
length of 4, and both `Item` elements, including a 16-character string bound.
Both multidimensional arrays and the bounded sequence typedef carry non-default
values in all states.

The fixture qualifies typed API setup and local CDR behavior for this exact
C016 `05-collections` value graph with RTI Connext DDS 7.7.0.0 and C++11. It
does not claim live DDS exchange, other RTI profiles, unbounded-support mode, or
sequence-element aggregate shapes outside the generated support boundary.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
