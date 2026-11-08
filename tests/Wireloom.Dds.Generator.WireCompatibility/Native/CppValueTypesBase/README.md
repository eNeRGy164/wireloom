# `06-valuetypes` `BaseValue` Modern C++ consumer

This fixture compiles the exact corpus IDL against retained RTI 7.7.0 and
Wireloom C++ outputs. RTI supports `BaseValue` as a Modern C++ topic type. The
consumer compares the topic name and DynamicType, default and alternate
samples, XCDR1/XCDR2 local CDR, registration/DataWriter creation, and actual
RTI-to-Wireloom and Wireloom-to-RTI DDS exchanges.

The public APIs are not drop-in compatible: RTI exposes the IDL member through
`baseValue()` getter/setter accessors, while Wireloom emits a public
`baseValue` field. The peer uses a small compile-time adapter only to exercise
the remaining runtime capabilities; the fixture records and asserts the
different public API shapes directly.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppValueTypesBase -B .tmp/cpp-valuetypes-base -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-valuetypes-base --parallel 4 && \
    ctest --test-dir .tmp/cpp-valuetypes-base --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime evidence covers `BaseValue` only for this IDL, RTI profile,
and Linux architecture. A first run mounted an unrelated Downloads file as the
license and could not create participants; after mounting the configured
license file read-only, the clean rerun passed all 11 CTests.
