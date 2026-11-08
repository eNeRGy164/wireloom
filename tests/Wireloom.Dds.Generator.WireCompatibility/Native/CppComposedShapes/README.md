# `05-shapes` C++11 runtime fixture

This fixture compiles the retained RTI 7.7 C++ oracle and Wireloom output for
the exact `docs/corpus/idl/features/05-shapes.idl` corpus case. It checks typed
registration and `DataWriter` creation, then compares local XCDR1 and XCDR2 CDR
bytes after round-tripping through both generated types.

The four states exercise unbounded-sequence finite fallback, bounded sequence
aliases, sequences of `Item`, nested sequences, fixed-array aliases, sequences
of arrays, sequences of aliased sequences of arrays, and the bounded string
sequence alias. The boundary state reaches all declared finite bounds: 100
items, 3 bounded items, 2 nested sequences each containing 4 values, 2 rows in
the sequence-of-arrays, 2 aliased arrays containing 2 rows each, and 4 names of
32 characters. `Item.label` reaches its 12-character bound.

The evidence qualifies this exact value graph for typed API setup and local
XCDR1/XCDR2 CDR parity with RTI Connext DDS 7.7.0.0 under C++11. It does not
claim live DDS exchange or support for unrelated nested aggregate graphs.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
