# C003 full-width C++ comparison

This fixture builds a typed consumer against retained RTI and Wireloom output
using the exact Connext 7.7.0 Modern C++ API. Compile-time assertions check all
eight signed and unsigned fixed-width member types. Both peers round-trip one
sample through XCDR1 and XCDR2 type support, then retain their serialized bytes
for a byte-for-byte comparison. The checked-in Windows x64 qualification uses
Visual Studio 18 2026, MSVC 19.51.36260.0, and RTI architecture
`x64Win64VS2017`.

Configure with the RTI SDK and a C++11 toolchain, then build and run CTest.
For Linux x64:

```sh
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppFullWidths \
  -B artifacts/cpp-full-widths \
  -DCONNEXTDDS_ARCH=x64Linux4gcc8.5.0
cmake --build artifacts/cpp-full-widths --parallel 2
ctest --test-dir artifacts/cpp-full-widths --output-on-failure
```

For Windows x64 with Visual Studio 2026 and the RTI `x64Win64VS2017` library
profile, set `NDDSHOME` to the Connext 7.7.0 installation, then run:

```powershell
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppFullWidths `
  -B artifacts/cpp-full-widths -G "Visual Studio 18 2026" `
  -DCONNEXTDDS_ARCH=x64Win64VS2017
cmake --build artifacts/cpp-full-widths --config Release --parallel 2
ctest --test-dir artifacts/cpp-full-widths -C Release --output-on-failure
```

The fixture uses the retained RTI and Wireloom generated files. Run
`src/Wireloom.Dds.Cpp.Generator/Verify-Cli.ps1` separately to regenerate C004 and confirm
the current generator matches the retained Wireloom output. The comparison is
CDR helper evidence; it does not claim live DDS transport interoperability.
