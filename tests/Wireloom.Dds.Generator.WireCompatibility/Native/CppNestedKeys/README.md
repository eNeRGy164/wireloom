# `08-key-nested` RTI/Wireloom consumer

This isolated C++11 fixture compiles one consumer against both the retained RTI
7.7.0 Modern C++ output and Wireloom output for the exact
`docs/corpus/idl/features/08-key-nested.idl` source.

The fixture checks the key flags and STL plugin binding for `Inner` and `Outer`,
the nested `Outer.identity` DynamicType shape, typed registration and
Topic/DataWriter construction, `register_instance` identity for nested key
paths, and XCDR1/XCDR2 local CDR round trips. RTI establishes that the key path
for `Outer` is `identity.code`: changing `identity.revision` or `payload` does
not change the instance handle, while changing `identity.code` does. Complete
RTI and Wireloom capture files are compared byte for byte.

Build and test in the repository-pinned `rticom/connext-base:7.7.0` Docker
image with the RTI license mounted read-only at
`/run/secrets/rti_license.dat`. No live DDS exchange or other nested key
shapes are claimed.
