# 09-optional-collections optional sequence and array fixture

This fixture compiles retained RTI and Wireloom C++ output as separate C++11
peers. It registers `CorpusOptionalCollections::Sample` and creates a typed
topic and DataWriter. It compares local XCDR1 and XCDR2 CDR bytes, then checks
each round trip for value equality, across absent, present-empty, partial, and
full bounded sequence states and absent/present fixed-array states.

The optional sequence uses RTI 7.7's sequence sample-access helper together
with optional-member metadata. The optional fixed primitive array uses
`get_opt_array_sample_access_info<T>()`. This fixture qualifies only the C039
bounded primitive sequence and fixed primitive array for RTI 7.7 typed
registration/writer creation and local CDR. It does not exchange samples
between live DDS peers, qualify optional string sequences, or establish
compatibility with other RTI profiles.

The six CDR combinations are absent sequence with absent/present array,
present-empty sequence with absent/present array, and partial/full sequence
with present array.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
