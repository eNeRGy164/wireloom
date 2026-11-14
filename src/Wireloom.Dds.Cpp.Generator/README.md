# Wireloom.Dds.Cpp.Generator 1.1.0

Wireloom.Dds.Cpp.Generator generates Modern C++ contracts from RTI Connext DDS
IDL during native CMake and Visual Studio/MSBuild builds. Version 1.1.0 bundles
Windows x64 and Linux x64 executables; generation needs no installed .NET
SDK/runtime or RTI SDK.

Use the [1.1.0 release record](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
for supported configurations, package hashes, and known limits. The
[C++ support and stability contract](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#modern-c-support-and-compatibility)
defines the public API and compatibility policy. Source and reference links
target `v1.1.0`; the release record retains later qualification clarifications.

For a complete first application, follow the
[C++ Hello World walkthrough](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/hello-world.md#modern-c-walkthrough):
install the package, generate during a native build, and run a publisher and
subscriber in two terminals.

## Obtain and pin the generator

Install `Wireloom.Dds.Cpp.Generator` **1.1.0** from NuGet.org. Pin this exact
version in your build configuration.

Build your application with a compatible RTI native SDK and C++ toolchain.
Use CMake 3.20 or later, or Visual Studio's native MSBuild integration.
Running DDS requires the RTI runtime and applicable license.

### CMake

Download the exact `.nupkg` and extract it into a version-specific directory.
For a stable package on Linux, using `curl` and CMake 3.20 or later:

```bash
version='1.1.0'
package="wireloom.dds.cpp.generator.$version.nupkg"
curl --fail --location --output "$package" \
  "https://api.nuget.org/v3-flatcontainer/wireloom.dds.cpp.generator/$version/$package"
# Check the package SHA-256 against the version's retained release evidence.
sha256sum "$package"
mkdir -p "packages/Wireloom.Dds.Cpp.Generator.$version"
(
  cd "packages/Wireloom.Dds.Cpp.Generator.$version"
  cmake -E tar xf "../../$package"
)
```

On Windows, the NuGet CLI can perform the download and extraction:

```powershell
$version = '1.1.0'
nuget install Wireloom.Dds.Cpp.Generator -Version $version `
  -Source https://api.nuget.org/v3/index.json -OutputDirectory packages -NonInteractive
```

Add generation to an existing target that already has its RTI include paths,
libraries, and application sources configured:

```cmake
find_package(WireloomCpp CONFIG REQUIRED)
wireloom_add_idl(my_app IDL "${CMAKE_CURRENT_SOURCE_DIR}/Contract.idl")
```

This attaches the four generated files and their include directory to
`my_app`. Include `Contract.hpp` from your application. The integration accepts
one IDL root per target; that root may include other files.

Configure and build from your application directory, pointing `WireloomCpp_DIR`
at the installed package's `build/cmake` directory:

```shell
cmake -S . -B build "-DWireloomCpp_DIR=$PWD/packages/Wireloom.Dds.Cpp.Generator.$version/build/cmake" -DWIRELOOM_CPP_RTI_VERSION=7.7.0
cmake --build build
```

Keep the generator package and RTI profile pinned independently; match
`WIRELOOM_CPP_RTI_VERSION` to the native SDK used by your application. CMake
selects the host executable and restores its Linux execute permission, so keep
the package directory writable. Rebuild after editing a root or included IDL
file. Unchanged builds skip generation; missing outputs are regenerated.

### Visual Studio / MSBuild

For an existing native `.vcxproj`, use Visual Studio's NuGet Package Manager
with `packages.config`. In its Package Manager Console, select that project:

```powershell
$version = '1.1.0'
Install-Package Wireloom.Dds.Cpp.Generator -Version $version -Source https://api.nuget.org/v3/index.json
```

Commit `packages.config` and the `.vcxproj` changes, including the package's
`.props` and `.targets` imports. Add IDL inputs to the project:

```xml
<ItemGroup>
  <WireloomIdl Include="Contract.idl" />
</ItemGroup>
<PropertyGroup>
  <WireloomCppRtiVersion>7.7.0</WireloomCppRtiVersion>
</PropertyGroup>
```

Restore on CI using Visual Studio MSBuild, then build with your RTI SDK
configuration:

```powershell
MSBuild.exe MyApp.vcxproj -t:Restore -p:RestorePackagesConfig=true
MSBuild.exe MyApp.vcxproj -t:Build -p:Configuration=Release -p:Platform=x64
```

Keep feed and trust settings in your team's NuGet configuration. The package
adds generation and generated sources; the application supplies RTI compiler
and linker settings.

### Update and roll back

Read the target version's release notes and support matrix. Keep the previous
package and build configuration until validation of the replacement completes.
For CMake, acquire the new package alongside the old one, update the pinned
`WireloomCpp_DIR`, and configure a clean build directory. For Visual Studio,
update the native project through Package Manager Console:

```powershell
$version = '1.1.0'
Update-Package Wireloom.Dds.Cpp.Generator -Version $version -Source https://api.nuget.org/v3/index.json
```

Commit the updated version and imports together, restore, and rebuild all
generated sources. Recheck application compilation and your DDS compatibility
tests with the same RTI SDK/profile. Change the RTI version separately when
possible so each change has clear evidence.

To roll back, restore the previous committed pin and imports (or CMake package
path), restore that exact package, and rebuild in a clean directory. Do not mix
generated sources or plugin files from different generator versions.

## Generator reference

For each IDL root, generation emits `<name>.hpp`, `<name>.cxx`,
`<name>Plugin.hpp`, and `<name>Plugin.cxx`. Public values, operations, and topic
traits have Doxygen documentation. The executable follows the build host;
the application's target architecture does not select the generator.

### CLI and build settings

You can also generate sources directly from your application directory. On
Windows, run the installed executable in PowerShell:

```powershell
.\packages\Wireloom.Dds.Cpp.Generator.1.1.0\tools\win-x64\Wireloom.Dds.Cpp.exe generate Contract.idl generated --rti-version 7.7.0
```

On Linux, restore the execute permission after extracting the package:

```bash
tool='./packages/Wireloom.Dds.Cpp.Generator.1.1.0/tools/linux-x64/Wireloom.Dds.Cpp'
chmod +x "$tool"
"$tool" generate Contract.idl generated --rti-version 7.7.0
```

Pass `--help` for the complete syntax. Set `--rti-version` to match the native
SDK profile; the implemented profiles are 6.1.2, 7.3.1, and 7.7.0. Add the
generated sources and include directory to your application build when using
the CLI directly; CMake and MSBuild perform that integration automatically.

| Setting                    | CLI                                                                      | CMake                                                                |
| :------------------------- | :----------------------------------------------------------------------- | :------------------------------------------------------------------- |
| RTI profile                | `--rti-version`                                                          | `WIRELOOM_CPP_RTI_VERSION`                                           |
| Includes and preprocessing | `--include-directory`, `--define`, `--undefine`                          | `INCLUDE_DIRECTORIES`, `DEFINES`, `UNDEFINES`                        |
| Validation and identifiers | `--strict`, `--enable-escape-char`                                       | `STRICT`, `ENABLE_ESCAPE_CHAR`                                       |
| Finite fallback sizes      | `--sequence-size`, `--string-size`                                       | `SEQUENCE_SIZE`, `STRING_SIZE`                                       |
| Unbounded or finite mode   | `--unbounded-support`, `--finite-support`                                | `UNBOUNDED_SUPPORT`, `FINITE_SUPPORT`                                |
| Declaring-file overrides   | `--file-sequence-size`, `--file-string-size`, `--file-unbounded-support` | `FILE_SEQUENCE_SIZES`, `FILE_STRING_SIZES`, `FILE_UNBOUNDED_SUPPORT` |

Quoted includes are searched beside the including file; include directories
supply additional search locations. Per-file overrides use path/value pairs
and apply to declarations in that file. The bundled native MSBuild target
exposes `WireloomIdl`, `WireloomCppRtiVersion`, and `WireloomCppGeneratedDir`.
Use the CLI or CMake when you need the additional generation options above;
they are not forwarded by that MSBuild target.

MSBuild tracks root IDL and transitive includes automatically. Comment-only
or timestamp-only edits run generation once, then unchanged builds skip it.
Deleting a generated file regenerates it; `Clean` removes generated files
and build state. For custom build integrations, `--dependency-file <path>`
writes the absolute input paths, one per line, after successful generation.

### Generated APIs and bounds

The generator handles documented scalar and enum values, constants, aliases,
strings, collections, aggregates, keys, unions, optional members, and
annotations. Availability and runtime support depend on the exact shape and
RTI profile. See [C++ value semantics](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/cpp-generated-value-semantics.md)
for public fields, optional presence, copying, and union branch operations.

Source-unbounded sequences default to 100 elements and source-unbounded
strings to 255. Explicit IDL bounds remain effective. Opt-in unbounded mode
uses RTI unbounded metadata and the supported dynamic storage paths; it does
not qualify arbitrary sample sizes or recursive graphs.

### Capability and qualification limits

Use the [1.1.0 release record](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
and [support policy](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#modern-c-support-and-compatibility)
before adopting an IDL shape or native environment. Accepted IDL does not
guarantee that every generated type can be registered or serialized on every
RTI profile.

- Match the generation profile, native SDK, and application toolchain. Rebuild
  generated contracts when upgrading either the generator or RTI SDK.
- Recursive sequences and recursive optional arrays have separate limits.
  RTI 7.7.0 TypeObject v2 rejects recursive strongly connected components;
  follow the documented recursion restrictions.
- FlatData is limited to the documented final, unkeyed, fixed-size two-`long`,
  XCDR2-only shape on RTI 7.7.0.0.
- Optional sequences of wide strings have known Linux failures. Review the
  version-specific limitations before using that shape.
- Use public value APIs and topic traits. Generated plugin callbacks and
  native adapter internals are not application extension points.

## Troubleshooting

| Symptom                                       | Check and next step                                                                                                                                                |
| :-------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| CMake cannot find `WireloomCpp`               | Set `WireloomCpp_DIR` to the extracted 1.1.0 package's `build/cmake` directory, then configure again.                                                              |
| Linux reports `Permission denied` for the CLI | Run `chmod +x` on the packaged Linux executable, or use the CMake integration to restore its permission.                                                           |
| MSBuild does not generate sources             | Restore `packages.config`, check the package's `.props` and `.targets` imports, and declare a `WireloomIdl` item in the native project. Use Visual Studio MSBuild. |
| An IDL include cannot be resolved             | Check quoted includes relative to their declaring file. Use CLI `--include-directory` or CMake `INCLUDE_DIRECTORIES` for additional search directories.            |
| Generated sources do not compile against RTI  | Match `--rti-version`, `WIRELOOM_CPP_RTI_VERSION`, or `WireloomCppRtiVersion` to the application's native SDK and verify its compiler/linker configuration.        |
| Registration or serialization is rejected     | Check the release limits for the exact IDL shape and RTI profile. Generation alone does not establish runtime support.                                             |
| The CLI exits with a nonzero code             | Exit `1` reports generation failure; exit `2` reports invalid usage. Read the diagnostic path/message and use `--help` for accepted options.                       |

For DDS runtime, license, discovery, and peer setup, follow the
[C++ Hello World troubleshooting guide](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/hello-world.md#troubleshoot-the-c-walkthrough).

## Learn more

- [Documentation by task](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/README.md)
- [Report a compatibility or integration problem](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/reporting-issues.md)
- [1.1.0 release qualification](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
