# `01-int8-boundaries` Modern C++ consumer

This fixture compiles the exact corpus `01-int8-boundaries.idl` against retained
RTI 7.7.0 and Wireloom C++ output. It checks the `int8_t` member mappings,
`int8Min`/`int8Max` constants, DynamicType metadata, explicit registration and
DataWriter construction, and default/minimum/maximum/alternate round trips
under XCDR1 and XCDR2. It compares all eight local CDR captures byte-for-byte.
The live test runs RTI-to-RTI, RTI-to-Wireloom, and Wireloom-to-RTI exchanges
with both representations.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppInt8Boundaries -B .tmp/cpp-int8-boundaries -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-int8-boundaries --parallel 4 && \
    ctest --test-dir .tmp/cpp-int8-boundaries --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime qualification is limited to this IDL, RTI profile, Linux
architecture, listed states, and peer pairings.
