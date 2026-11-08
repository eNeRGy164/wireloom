# `03-enum-values-prefix` Modern C++ enum consumer

This fixture compiles the exact corpus IDL against retained RTI 7.7.0 and
Wireloom output. It checks the C++ enum values, stream names, default
enumerator, Modern C++ topic trait, and DynamicType members against the RTI
oracle. It records the stream operator difference: RTI appends a trailing
space to each enumerator name and Wireloom does not. The enum is not a
standalone topic type in RTI's Modern C++ API, so
registration/DataWriter, root XCDR serialization, and Live DDS do not apply to
this declaration. It is normally serialized as a member of a struct or union,
which is outside this IDL case.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppEnumValuePrefix -B .tmp/cpp-enum-values-prefix -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-enum-values-prefix --parallel 4 && \
    ctest --test-dir .tmp/cpp-enum-values-prefix --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime evidence covers only the enum API and DynamicType for this
declaration, RTI profile, and Linux architecture.
