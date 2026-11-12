# RTI 6.1.2 key and collection SDK probe

This fixture compiles freshly generated Wireloom C++ output against the RTI
6.1.2 Modern C++ SDK, then runs a local DataWriter registration check. It
covers scalar, composite bounded-string, and array keys, keyless types, optional
sequence/array metadata, bounded string/sequence aliases, and the collection
shapes already exercised by `Rti612CollectionSpecs`. Only the `keys` IDL is
used for the runtime instance-handle checks. The other generated translation
units are compiled as an additional API compatibility probe.

Generate each IDL with the selected API profile into the same output directory:

```bash
generated=/tmp/wireloom-rti612-generated
mkdir -p "$generated"
for idl in keys collections optional-collections; do
  dotnet run --project src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj \
    --configuration Release -- generate \
    "tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppKeysRti612/$idl.idl" \
    "$generated" --rti-version 6.1.2
done
cmake -S tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppKeysRti612 \
  -B /tmp/wireloom-rti612-keys-build \
  -DWIRELOOM_CPP_GENERATED_DIR="$generated"
cmake --build /tmp/wireloom-rti612-keys-build --parallel 2
ctest --test-dir /tmp/wireloom-rti612-keys-build --output-on-failure
```

Run these commands inside the RTI 6.1.2 helper container with its license and
SDK environment configured. The container supplies the RTI SDK, not the RTI
IDL code generator, so this fixture checks the generated Wireloom plugin API
and does not claim byte-for-byte comparison against RTI-generated C++ output.
