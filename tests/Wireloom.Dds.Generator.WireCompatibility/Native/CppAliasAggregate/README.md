# `03-alias-aggregate` C++11 runtime fixture

This fixture compiles the retained RTI 7.7 C++ oracle and Wireloom output for
the exact `docs/corpus/idl/features/03-alias-aggregate.idl` corpus case. It
checks typed registration and `DataWriter` creation, then round-trips the
`Sample` through XCDR1 and XCDR2 and compares the local CDR bytes with RTI's
generated C++ output.

The two states cover the default-initialized `Point` and a non-default value
with `x = 17` and `y = -23`. The fixture exercises both typedef identity layers
(`PointAlias` and `PointAlias2`) around an aggregate. It does not claim live DDS
exchange.

Configure with the Visual Studio 18 2026 generator and
`CONNEXTDDS_ARCH=x64Win64VS2017`, build Release, then run CTest.
