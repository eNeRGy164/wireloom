# Wireloom Modern C++ generator

Start with the [C++ support and stability contract](../../docs/guides/support-and-compatibility.md#modern-c-support-and-compatibility)
for the first-release promise, platform/profile targets, and qualification
requirements. This reference describes implemented generation and case-specific
evidence; it does not certify an unpublished package.

For a complete first application, follow the
[C++ Hello World walkthrough](../../docs/guides/hello-world.md#modern-c-walkthrough):
install the package, generate during a native build, and run a publisher and
subscriber in two terminals.

## Obtain and pin the generator

The package ID is `Wireloom.Dds.Cpp.Generator`. Stable releases go to
NuGet.org; previews go to the repository's GitHub Packages feed. The first C++
package has not been published yet. In the examples below, replace
`<published-version>` with an exact version from its release record; do not use
a floating version or assume that the C# package's latest version is available
for C++.

The package bundles Windows x64 and Linux x64 executables. Running generation
needs no .NET SDK/runtime or RTI installation. Building the generated sources
requires your selected RTI native SDK and C++ toolchain. The
[support contract](../../docs/guides/support-and-compatibility.md#modern-c-support-and-compatibility)
describes host requirements and the separately qualified application scope.

### CMake

Download the exact `.nupkg` and extract it into a version-specific directory.
For a stable package on Linux, using `curl` and CMake 3.20 or later:

```bash
version='<published-version>'
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
$version = '<published-version>'
nuget install Wireloom.Dds.Cpp.Generator -Version $version `
  -Source https://api.nuget.org/v3/index.json -OutputDirectory packages -NonInteractive
```

Keep the chosen version in your build configuration. Point `WireloomCpp_DIR`
at that package's `build/cmake` directory when configuring your application:

```bash
cmake -S . -B build "-DWireloomCpp_DIR=$PWD/packages/Wireloom.Dds.Cpp.Generator.$version/build/cmake" -DWIRELOOM_CPP_RTI_VERSION=7.7.0
cmake --build build
```

Add generation to an existing target that already has its RTI include paths,
libraries, and application sources configured:

```cmake
find_package(WireloomCpp CONFIG REQUIRED)
wireloom_add_idl(my_app IDL "${CMAKE_CURRENT_SOURCE_DIR}/Contract.idl")
```

This attaches all four generated files and their include directory to
`my_app`. CMake selects the generator by build host and restores the Linux
execute permission after extraction. Keep the package directory writable for
that permission step. The generator package version and the
`WIRELOOM_CPP_RTI_VERSION` generation profile are independent pins; match the
latter to the native SDK used by the application.

CMake records a completion stamp after successful generation. A timestamp-only
or comment-only IDL edit runs generation once even when emitted text is
unchanged; the next unchanged build skips it. Missing generated outputs still
trigger generation, and failures remain eligible for retry.

### Visual Studio / MSBuild

For an existing native `.vcxproj`, use Visual Studio's NuGet Package Manager
with `packages.config`. In its Package Manager Console, select that project:

```powershell
$version = '<published-version>'
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

### Preview packages and retained candidates

For NuGet clients, use the exact prerelease version with the source
`https://nuget.pkg.github.com/eNeRGy164/index.json`. Configure GitHub Packages
authentication in your user/CI credential store, with package-read access;
keep credentials out of the project and source control. Visual Studio's
`Install-Package` and `Update-Package` accept this source too.

For CMake users, the publishing run also retains the tested `.nupkg` files,
`SHA256SUMS`, and SBOM. With GitHub CLI access to that run:

```bash
run_id='<publishing-run-id>'
version='<published-version>'
gh run download "$run_id" --repo eNeRGy164/wireloom \
  --name "wireloom-dds-generator-preview-$version" --dir candidate
(cd candidate && sha256sum --check SHA256SUMS)
mkdir -p "packages/Wireloom.Dds.Cpp.Generator.$version"
(
  cd "packages/Wireloom.Dds.Cpp.Generator.$version"
  cmake -E tar xf "../../candidate/Wireloom.Dds.Cpp.Generator.$version.nupkg"
)
```

Configure CMake with the extracted package directory as above. Preview artifacts have
14-day retention, so retain the exact package and its provenance in your
artifact store when you need a longer-lived pin. A standalone **C++ native
package** workflow also produces a `cpp-nuget` candidate; that artifact is
build evidence and does not indicate feed publication or release qualification.

### Update and roll back

Read the target version's release notes and support matrix. Keep the previous
package and build configuration until validation of the replacement completes.
For CMake, acquire the new package alongside the old one, update the pinned
`WireloomCpp_DIR`, and configure a clean build directory. For Visual Studio,
update the native project through Package Manager Console:

```powershell
$version = '<published-version>'
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

`Wireloom.Dds.Cpp.Generator` hosts the CLI and produces the native NuGet package.
`Wireloom.Dds.Cpp.Compiler` owns in-memory IDL compilation, C++ planning, and
source emission. The package's `tools/` directory contains self-contained
NativeAOT executables for Windows x64 and Linux x64. Consumer builds need neither .NET nor an RTI
license to run the generator. The CMake package invokes the executable for the
build host; C++ target architecture does not select the generator executable.
Generated C++ compilation and execution use the native RTI SDK. The generator
supports primitive scalar members, narrow and wide string members, integral and narrow
string constants, scoped enums, scalar and enum typedef aliases plus direct
narrow/wide string aliases and bounded/unbounded string alias chains, bounded
enum sequences, nested bounded-sequence typedef aliases, aggregate aliases, bounded
sequences of structs, and multidimensional primitive arrays with typedef aliases. It
also emits composed sequence shapes containing arrays, structs, bounded strings,
and collection aliases, including fixed arrays whose elements are sequences,
while preserving declared alias names in C++ storage. The Modern C++ generator
emits RTI-shaped C++11 value
classes and public `UnionType` metadata for primitive or enum discriminators,
scalar, string,
aggregate, and bounded-sequence branches, and unions with or without a default
branch. Collection-bearing union metadata accessors initialize the type shells
and members without eagerly creating the generic type plugins; registration
and CDR paths still initialize plugins. C139 also preserves four bounded
sequence typedef names and their `AliasType` layers, and has an RTI 7.7.0
consumer/API metadata check. This does not qualify generic union plugins, CDR,
or wire behavior for C139. Qualification of other union shapes remains specific
to the consumer and wire fixtures that exercise them. Generated types also
include topic name/support traits and generic-factory registration, lifecycle,
and CDR callback source. A
source-unbounded string uses the resolved profile bound by default (255
characters). The standalone CLI preserves that finite behavior unless
`--unbounded-support` is supplied. In that opt-in mode, unbounded string and
sequence DynamicType bounds use `RTIXCdrLong_MAX`; supported unbounded sequence
fields use `omg::types::sequence<T>`. Wide string members use `std::wstring` and the RTI `WStringType`
metadata expression; stream output uses an escaped `L"..."` representation.
Direct narrow/wide string aliases and bounded/unbounded one- and two-hop alias
chains retain their public `using` names. Parsed typedef facts provide the
effective metadata bound: 255 by default for omitted bounds,
`RTIXCdrLong_MAX` for unbounded-support inputs, and the explicit IDL bound
otherwise.
Source-unbounded sequences use the current CLI profile's finite fallback bound
of 100 by default; `--unbounded-support` enables the scoped C173 sequence and
string shape, including wide-string and simple struct sequence elements, plus
C019's self-recursive aggregate sequence. C019 uses the
unbounded profile to match RTI's `omg::types::sequence<Self>` storage. Its
metadata path is exercised indirectly by local CDR tests; DynamicType is not
directly tested. RTI 7.7.0 does not permit registering that recursive type
because TypeObjectV2 rejects strongly connected components.
Qualification varies by string, alias, and collection shape. The `05-shapes`
case has native consumer and live-wire checks; other cases retain their
case-specific consumer, CDR, or generation checks in the wire compatibility
fixtures. See the [wire harness guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
The C044 keyed topic remains a separate regression fixture.

`@data_representation(XCDR2)` and its RTI-documented synonym
`@allowed_data_representation(XCDR2)` are supported on the C041/C043 appendable
scalar struct shapes. The generated CDR helper retains `auto_id`, supports XCDR2, and rejects
explicit XCDR1. RTI 7.7.0 has no public Modern C++ TypeCode setter for the
representation mask, so this shape uses its internal TypeCode layout before
generic plugin creation. Consumer evidence covers the mask and scalar CDR
helper only; use RTI 7.7.0 for this generated shape and recheck the layout
before an SDK upgrade. Topic registration, endpoint negotiation, and live
wire behavior remain unqualified.

C047 `10-flat-data-binding` emits RTI 7.7 FlatData's final `Sample<Offset>`
shape, offset accessors, a plain helper, DynamicType metadata, and the
`type_programs` specialization used by `Sample::create_data()`. Its CMake
consumer exercises sample allocation, offset reads/writes, and direct copying
of the fixed-size XCDR2 buffer through the generated CDR helpers; explicit
XCDR1 is rejected. The FlatData kind flag and XCDR2 mask depend on private RTI
7.7.0.0 TypeCode fields guarded at compile time. PRESTypePlugin callbacks,
topic registration, DataWriter loans, endpoint negotiation, and live wire
behavior remain unqualified. See the
[RTI 7.7 feature coverage](../../docs/corpus/7.7.0/FEATURE-COVERAGE.md).
The [package verification guide](../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)
describes NuGet, MSBuild, and CMake checks.

Generated value fields, constructors, topic traits, and documented sample
operations are the application-facing API. Generated plugin callbacks,
sample-access metadata, and DynamicType construction are implementation ABI
selected for an RTI profile; they are not a customer extension surface. The
current C-layout `CTypePlugin` path is not qualified for STL-backed string
or array samples. Sequence-bearing value graphs with bounded primitive or enum
sequences use an RTI 7.7.0.0 guarded STL-aware `PRESTypePlugin`; ordinary value
graphs continue to use the C-layout plugin. Unsupported aggregate and sequence
graphs fail closed for registration and CDR. C019 `06-aggregates` validates its
`Base`, inherited `Derived`, and `Composed` nonrecursive graph against RTI 7.7:
typed registration/DataWriter creation pass, and default plus representative
values match RTI byte-for-byte for local XCDR1/XCDR2 round trips. The recursive
`Recursive.children` graph uses parent-aware `RTIXCdrResourceNode` traversal.
With `--unbounded-support`, its local XCDR1/XCDR2 empty and depth-three CDR
matches RTI byte-for-byte. RTI 7.7.0 rejects TypeObjectV2 registration for
recursive strongly connected components, so Wireloom fails recursive
registration closed with a targeted diagnostic. DynamicType is not directly
tested for this shape, and live DDS remains unqualified. C134's optional
recursive fixed array is distinct; generic plugin, CDR, and live-DDS runtime
behavior remain separately unqualified for that case.
C021 `06-alias-composition` passes DynamicType checks for its typedef, enum,
two structs, and nested bounded-sequence graph; `Sample` and `Named` register
as typed topic roots, and default/full-bound values match RTI byte-for-byte
under local XCDR1 and XCDR2 serialization. Live DDS remains unqualified.
C005, C006,
`03-enums-aliases`, and `03-alias-collections` pass typed registration and
writer creation plus local XCDR1/XCDR2 CDR byte equality for their tested
sequence lengths. C039 `09-optional-collections` also passes typed
registration/writer creation and local XCDR1/XCDR2 byte equality for absent,
present-empty, partial, and full optional bounded primitive sequences with
absent/present optional fixed primitive arrays; it attaches RTI's sequence and
optional-array access helpers. C114 `09-optional-string-sequences` also passes
typed registration/writer creation and local XCDR1/XCDR2 CDR byte equality for
exact optional `sequence<string<16>, 4>` and `sequence<wstring<16>, 4>` members.
It exercises absent, empty, each member independently present, both partial,
and both full states with empty and 16-character boundary elements. Other
string-sequence graphs fail closed. The alias cases walk RTI 7.7 TypeCode aliases to attach
sequence sample access to `Record.colors` and each nested sequence node in
`Sample.values`. Unsupported sequence graph shapes fail
explicitly. These results do not qualify live sample exchange, other string
sequence shapes, other array shapes, other nested collection shapes, or older
RTI profiles. See the
[C039 native fixture](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalCollections/README.md)
and the [wire harness guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md). The
[ADR-006](../../docs/architecture/adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md)
records the adapter boundary and remaining RTI support/licensing review.

All generated C++ files use deterministic LF line endings. Public value types,
members, value operations, and topic/type-support traits include concise Doxygen
documentation; private generated plugin implementation details remain
undocumented.

For optional presence, public-field usage, and union branch operations, see
[Generated C++ value semantics](../../docs/guides/cpp-generated-value-semantics.md).

## Build the generator from source

Run from the repository root:

```powershell
dotnet restore src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj --locked-mode
dotnet build src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj -c Release --no-restore
dotnet src/Wireloom.Dds.Cpp.Generator/bin/Release/net10.0/Wireloom.Dds.Cpp.dll generate docs/corpus/idl/features/01-primitives.idl artifacts/cpp-generated
# Add preprocessor symbols and IDL include directories when required by the input.
dotnet src/Wireloom.Dds.Cpp.Generator/bin/Release/net10.0/Wireloom.Dds.Cpp.dll generate `
  docs/corpus/idl/features/11-preprocessing.idl artifacts/cpp-preprocessing `
  --define CORPUS_CAPTURE_DEFINE --include-directory docs/corpus/idl
pwsh -NoProfile -File src/Wireloom.Dds.Cpp.Generator/Verify-Cli.ps1
```

`--include-directory` adds a search location for reachable IDL includes.
Quoted includes are also resolved beside the including file. `--define` supplies
a preprocessor symbol to the root IDL. `--unbounded-support` opts into RTI-style
unbounded sequence and string metadata; it is also available as the
`UNBOUNDED_SUPPORT` option to `wireloom_add_idl`. The CLI also accepts
`--undefine`, `--strict`, `--sequence-size`, `--string-size`,
`--enable-escape-char`, and `--finite-support`. Per-file size and unbounded
overrides use `--file-sequence-size`, `--file-string-size`, and
`--file-unbounded-support` with a path/value pair. CMake exposes corresponding
`UNDEFINES`, `STRICT`, `SEQUENCE_SIZE`, `STRING_SIZE`, `ENABLE_ESCAPE_CHAR`, and
`FINITE_SUPPORT` arguments alongside `DEFINES`, `INCLUDE_DIRECTORIES`, and the
`FILE_SEQUENCE_SIZES`, `FILE_STRING_SIZES`, and `FILE_UNBOUNDED_SUPPORT` path/value lists.

Publish a native executable on its matching host OS and pack after both RID
outputs have been assembled under the same package root:

```powershell
$version = '1.0.1-preview.42' # Example local candidate, not a published version.
pwsh -NoProfile -File scripts/Publish-WireloomCppNative.ps1 -RuntimeIdentifier win-x64 -PackageVersion $version
# On a Linux x64 host, use -RuntimeIdentifier linux-x64.
dotnet pack src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj -c Release -o artifacts/package-test-feed "-p:Version=$version" "-p:PackageVersion=$version"
```

The publish script stages the native build under `artifacts/cpp-native-package/publish/<rid>/`,
copies only the executable into `tools/<rid>/`, and retains its publish log,
warning summary, and SHA256 manifest under `metadata/<rid>/`. The pack target
requires both `tools/win-x64/Wireloom.Dds.Cpp.exe` and
`tools/linux-x64/Wireloom.Dds.Cpp`; it rejects managed assemblies, PDBs, deps
files, and runtimeconfig files in the native tool directories and excludes
managed build output and runtime dependencies from the NuGet package.

Use the same explicit version for both host publishes and packing. Omitting
`-PackageVersion` uses `WireloomVersion` from `Directory.Build.props`.
The reusable [package workflow](../../.github/workflows/cpp-package.yml) builds
both payloads and verifies the combined candidate on both operating systems.
The preview and release publishers select one source revision and version,
wait for those checks, and attest and publish the exact tested C++ package
alongside the C# package. They retain package hashes and an SBOM. General
release qualification remains a separate gate.

The CLI verifier compares all four generated files against retained Wireloom
output for each corpus case it lists, including bounded and source-unbounded
narrow and wide string members, `05-collections` arrays and sequences,
`05-shapes` compositions, and arrays of sequences in `05-array-of-sequences`.
It also checks source-located diagnostics for unsupported
fields and annotations and confirms that the generated type uses a changed IDL
default.
