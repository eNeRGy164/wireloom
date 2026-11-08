# `02-formatting` Modern C++ consumer

This fixture compiles the exact corpus IDL against retained RTI 7.7.0 and
Wireloom output. It checks the public `std::string` mapping, DynamicType member
metadata, explicit registration and DataWriter construction, and default,
partial, 16-byte boundary, and alternate string values under XCDR1/XCDR2. It
compares all eight RTI/Wireloom CDR captures byte-for-byte. The live test runs
RTI-to-RTI, RTI-to-Wireloom, and Wireloom-to-RTI exchanges under both
representations.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppFormatting -B .tmp/cpp-formatting -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-formatting --parallel 4 && \
    ctest --test-dir .tmp/cpp-formatting --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime qualification is limited to this exact IDL, RTI profile,
Linux architecture, listed values, and peer pairings.
