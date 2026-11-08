# C023 Modern C++ enum union qualification

This fixture compiles the same consumer source against the retained RTI 7.7.0
`rtiddsgen` output and Wireloom output for the exact corpus IDL
`docs/corpus/idl/features/07-union-enum.idl`. It checks the generated public
union API, enum and aggregate branch DynamicType metadata, typed topic
registration/DataWriter construction, CDR round trips, and exact local XCDR1
and XCDR2 byte equality for default construction and each active union branch.

The fixture does not exchange live DDS samples. Such interoperability remains
unqualified until a participant-to-participant exchange is run.

Configure and run on Windows with RTI Connext DDS 7.7.0 installed and licensed:

```powershell
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppEnumUnion `
  -B artifacts/cpp-enum-union -G 'Visual Studio 18 2026' `
  -DCONNEXTDDS_ARCH=x64Win64VS2017
cmake --build artifacts/cpp-enum-union --config Release
ctest --test-dir artifacts/cpp-enum-union -C Release --output-on-failure
```
