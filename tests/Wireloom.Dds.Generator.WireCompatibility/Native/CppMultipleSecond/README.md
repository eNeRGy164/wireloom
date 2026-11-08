# `02-multiple` Modern C++ consumers

The two declaration-specific fixtures compile the exact corpus IDL against
retained RTI 7.7.0 and Wireloom output. The [`First` consumer](../CppMultiple/README.md)
qualifies its `int32` topic. This consumer qualifies global struct `Second`:
public `std::string` binding, DynamicType metadata, explicit
registration/DataWriter creation, and default, one-byte minimum, 255-byte
maximum, and alternate values under XCDR1/XCDR2. Each fixture compares eight
local CDR captures and runs six Live DDS peer exchanges. Evidence reports name
which declaration each result covers.

Run in the pinned RTI Connext 7.7.0 Linux amd64 image. Replace the host path
placeholders; the license is mounted read-only and its contents are not copied
into the repository:

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppMultipleSecond -B .tmp/cpp-multiple-second -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-multiple-second --parallel 4 && \
    ctest --test-dir .tmp/cpp-multiple-second --output-on-failure'
```

This fixture compiles retained outputs directly and does not invoke or modify
the generator. Its runtime qualification is limited to `Second`, this IDL, RTI
profile, Linux architecture, named values, and peer pairings.
