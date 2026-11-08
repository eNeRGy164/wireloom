# C005 scopes and bounded sequence C++ comparison

This fixture compiles a consumer against the retained RTI C++ output and the
retained Wireloom output. It checks reopened modules, relative and absolute
nested references, the bounded scalar sequence, RTI topic traits, DynamicType
member metadata, sample reset/allocation, and XCDR1/XCDR2 CDR round trips for
the nested, sequence-bearing, and reopened structs. It exercises sequence
lengths 0, 1, and the IDL bound 4. The registration case registers
`KeywordRecord` and `Reopened` and creates a typed topic and DataWriter for each;
their sequence-bearing value graphs use the guarded RTI 7.7.0.0 STL-aware
plugin, while `Point` remains on the ordinary C-layout path. All ten CTest cases
pass, including RTI/Wireloom CDR byte equality at every tested length. This
qualifies these local CDR and registration shapes only; the fixture does not
exchange samples between live DDS peers.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
