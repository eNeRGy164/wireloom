# `06-valuetypes` `Holder` Modern C++ consumer

This fixture compiles the exact corpus IDL against retained RTI 7.7.0 and
Wireloom outputs. It checks the direct `Holder.value` API, nested DynamicType
link, registration/DataWriter, default and alternate XCDR1/XCDR2 local CDR,
and RTI/Wireloom live DDS exchanges.

The direct Holder shell API matches: both expose `value` as a public
`DerivedValue` field and accept a `const DerivedValue&` in the constructor.
The nested `DerivedValue` type has an accessor-versus-field API difference
recorded by its own row. Its inherited aggregate runtime bridge also fails
closed; this fixture probes whether that limitation propagates through
`Holder`.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppValueTypesHolder -B .tmp/cpp-valuetypes-holder -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-valuetypes-holder --parallel 4 && \
    ctest --test-dir .tmp/cpp-valuetypes-holder --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime evidence covers `Holder` only for this IDL, RTI profile, and
Linux architecture.
