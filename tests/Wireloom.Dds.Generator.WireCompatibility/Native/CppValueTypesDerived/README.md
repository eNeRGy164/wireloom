# `06-valuetypes` `DerivedValue` Modern C++ consumer

This fixture compiles the exact corpus IDL against retained RTI 7.7.0 and
Wireloom outputs. It checks C++ inheritance from `BaseValue`, the derived
DynamicType parent link and string bound, registration/DataWriter creation,
default and alternate XCDR1/XCDR2 local CDR, and RTI/Wireloom live DDS
exchanges in both directions.

The consumer API is not drop-in compatible. RTI exposes inherited `baseValue`
and local `name` through accessors and accepts `(int32, string_view)` in its
value constructor. Wireloom exposes both as public fields and accepts
`(BaseValue, string)` in its constructor. The fixture asserts these API shapes
separately, then uses narrow peer-specific access helpers to test the remaining
runtime behavior without claiming source compatibility. DynamicType metadata
and inheritance match. Wireloom's retained runtime deliberately fails closed
for inherited aggregate value graphs: `topic_type_support::register_type` and
local CDR serialization report that this graph is outside the RTI C-layout
plugin bridge. Therefore mixed-peer Live DDS and exact local CDR parity are not
qualified for this type.

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
    cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppValueTypesDerived -B .tmp/cpp-valuetypes-derived -DCMAKE_BUILD_TYPE=Release && \
    cmake --build .tmp/cpp-valuetypes-derived --parallel 4 && \
    ctest --test-dir .tmp/cpp-valuetypes-derived --output-on-failure'
```

The fixture compiles retained outputs directly and does not invoke or modify the
generator. Runtime evidence covers `DerivedValue` only for this IDL, RTI
profile, and Linux architecture.
