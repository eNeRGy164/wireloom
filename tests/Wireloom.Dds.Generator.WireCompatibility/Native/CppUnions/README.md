# C022 union consumer

This fixture uses the retained `07-unions.idl` corpus input. CMake builds the same `peer.cpp` against the RTI 7.7.0 oracle and Wireloom generated output, so one public API consumer compiles and runs against both headers.

The consumer asserts RTI's discriminator and branch reference accessors, default construction, default branch setter preconditions, member comparisons and swap. It inspects `UnionType` metadata, registers `Choice`, and creates a typed `Topic` and `DataWriter`.

CTest runs four states against RTI and Wireloom under XCDR1 and XCDR2, then compares the complete captures byte for byte. Passing checks cover the shared consumer API, local TypeCode metadata, registration and writer construction, and local CDR. They do not claim a live DDS sample exchange.

Configure with RTI Connext DDS 7.7.0 installed and `NDDSHOME` set, build both peers, and run CTest from the build directory. Use `-DWIRELOOM_CPP_UNIONS_RTI_ONLY=ON` when building only the retained RTI control peer.
