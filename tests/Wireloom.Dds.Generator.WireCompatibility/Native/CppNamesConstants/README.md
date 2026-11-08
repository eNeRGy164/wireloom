# C004 names, constants, and nested C++ comparison

This fixture compiles one consumer against the retained RTI C++ output and the
retained Wireloom output. It checks integral constants, primitive mapping,
scoped enum and nested struct fields, RTI topic traits for both structs,
default construction, and CDR round trips for Point and KeywordRecord. It
compares the RTI and Wireloom XCDR1/XCDR2 bytes.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest. The retained
Windows x64 qualification used MSVC 19.51.36260.0 and RTI Connext 7.7.0.0.
Local CDR checks do not claim live DDS discovery or transport interoperability.
