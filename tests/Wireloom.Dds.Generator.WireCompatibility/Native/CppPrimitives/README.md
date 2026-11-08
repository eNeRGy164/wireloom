# `01-primitives` Modern C++ consumer

This fixture compiles the shared `CppFirstCase/peer.cpp` consumer against the
retained RTI 7.7.0 and Wireloom output for the exact corpus IDL. It checks the
public Modern C++ member types, DynamicType member order and kinds, registration
and DataWriter construction, local XCDR1/XCDR2 round trips, and exact equality
of the ten RTI/Wireloom CDR captures. Its live test runs RTI-to-RTI,
RTI-to-Wireloom, and Wireloom-to-RTI DDS exchanges with both representations.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image. Replace the two host
path placeholders; the license is mounted read-only and its contents are not
copied into the repository:

```sh
docker run --rm --platform linux/amd64 --user 0:0 \
  -v "<HOST_REPOSITORY_PATH>:/work" \
  -v "<HOST_LICENSE_PATH>:/run/secrets/rti_license.dat:ro" \
  -w /work \
  -e NDDSHOME=/opt/rti.com/rti_connext_dds-7.7.0 \
  -e CONNEXTDDS_ARCH=x64Linux4gcc8.5.0 \
  -e RTI_LICENSE_FILE=/run/secrets/rti_license.dat \
  -e LD_LIBRARY_PATH=/opt/rti.com/rti_connext_dds-7.7.0/lib/x64Linux4gcc8.5.0 \
  --entrypoint /bin/bash \
  rticom/connext-base@sha256:efe4d35807a4aadaa59f0e84545d4c6b70328af5b0c0ca0498e63ecc1e16e1ca \
  -lc 'apt-get update -qq && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends cmake g++ make && \
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppPrimitives -B .tmp/cpp-primitives -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-primitives --parallel 4 && \
    ctest --test-dir .tmp/cpp-primitives --output-on-failure'
```

The fixture uses only the retained `01-primitives.idl`, RTI oracle, and
Wireloom-generated files. It does not invoke the generator. A passing CTest
live exchange demonstrates application-level DDS interoperability between
the named peer pairings in this RTI profile and Linux architecture; it does not
claim coverage of other platforms or IDL types.
