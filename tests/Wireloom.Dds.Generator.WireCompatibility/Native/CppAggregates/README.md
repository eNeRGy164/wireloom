# C019 aggregate wire fixture

This RTI 7.7 C++11 fixture compares the retained `06-aggregates` oracle with
Wireloom output. It registers `Base`, inherited `Derived`, and `Composed` and
checks empty/default plus representative values under XCDR1 and XCDR2. CTest
compares the complete peer output byte-for-byte for each value.

The retained Wireloom output uses `--unbounded-support`, matching RTI's
`omg::types::sequence<Recursive>` storage and unbounded DynamicType bound. The
parent-aware resource-node lookup lets the RTI Modern C++ interpreter serialize
`Recursive` values. CTest compares empty and depth-three recursive values under
both XCDR1 and XCDR2 against the RTI oracle.

RTI Connext DDS 7.7.0 rejects registering a recursive type because TypeObjectV2
does not support strongly connected components. The fixture verifies this
limitation independently for the RTI oracle and Wireloom output. Recursive
registration and live DDS therefore remain unavailable even though local CDR
parity is verified. `Base`, `Derived`, and `Composed` retain their existing
registration and CDR checks.

Configure and run on a host with the RTI 7.7.0 SDK, architecture environment,
and license configured:

```powershell
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppAggregates `
    -B artifacts/cpp-native-aggregates -G "Visual Studio 18 2026" `
    -DCONNEXTDDS_ARCH=x64Win64VS2017
cmake --build artifacts/cpp-native-aggregates --config Release
ctest --test-dir artifacts/cpp-native-aggregates -C Release --output-on-failure
```

See [RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md)
and the [Modern C++ generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md).
