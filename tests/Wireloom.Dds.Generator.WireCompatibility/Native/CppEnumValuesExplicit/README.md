# `03-enum-values-explicit` Modern C++ enum consumer

This fixture compiles the exact corpus IDL against the retained RTI 7.7.0 and
Wireloom C++ outputs. It checks the enum type, explicit and implicit values,
default enumerator, topic trait, DynamicType literal order and ordinals, and
the generated stream operator. The API and DynamicType match. RTI adds one
trailing space to each streamed literal name; Wireloom does not.

RTI's Modern C++ topic trait is false for this standalone enum. Registration,
DataWriter, root XCDR serialization, and Live DDS therefore do not apply to
this declaration. Enum values are normally serialized as members of a struct
or union, which this IDL does not contain.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppEnumValuesExplicit -B .tmp/cpp-enum-values-explicit -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-enum-values-explicit --parallel 4 && \
    ctest --test-dir .tmp/cpp-enum-values-explicit --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime evidence covers enum API and DynamicType for this exact IDL,
RTI profile, and Linux architecture. The stream parity test is expected to
report the retained trailing-space difference as a failure.
