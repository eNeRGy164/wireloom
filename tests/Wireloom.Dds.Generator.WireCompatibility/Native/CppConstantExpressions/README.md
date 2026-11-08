# C006 constant expressions, sequences, and fixed arrays

This fixture compiles consumers against the RTI C++ oracle and retained
Wireloom output. Its API checks cover evaluated constants, the bounded sequence
and fixed-array C++ types, DynamicType member kinds, default construction,
reset/allocation, and array-only equality/stream behavior with the sequence left
empty. Those checks pass for both peers.

The registration case creates a participant, registers the sequence-bearing
`Sample` with the guarded RTI 7.7.0.0 STL-aware plugin, and creates a typed
topic and DataWriter. The CDR controls exercise sequence lengths 0, 1, and the
IDL bound 5 in XCDR1 and XCDR2. All twelve CTest cases pass, including RTI and
Wireloom CDR byte equality for each size. The sample's array CDR is not
independently tested because the sequence is part of the same IDL type. No live
DDS sample exchange was run.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
