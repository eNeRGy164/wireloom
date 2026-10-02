# Wireloom

[![Quality](https://github.com/eNeRGy164/wireloom/actions/workflows/quality.yml/badge.svg)](https://github.com/eNeRGy164/wireloom/actions/workflows/quality.yml)
[![Coverage Status](https://coveralls.io/repos/github/eNeRGy164/wireloom/badge.svg?branch=main)](https://coveralls.io/github/eNeRGy164/wireloom?branch=main)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/eNeRGy164/wireloom/badge)](https://scorecard.dev/viewer/?uri=github.com/eNeRGy164/wireloom)

Wireloom is a preview .NET source generator for the RTI Connext DDS IDL subset.
It generates C# data types and RTI type-specific support during a normal .NET
build, inside the Roslyn compiler. Consumer builds do not need Java, a native
C/C++ compiler, or `rtiddsgen` for IDL generation.

The RTI Connext DDS runtime still provides serialization and DDS communication.
Wireloom handles IDL preprocessing, parsing, validation, and C# generation; it
does not sit in the DDS data path.

## Get started

Add the generator and RTI runtime to the project that owns your DDS contracts.
The runtime reference is explicit because your application controls its runtime
version and configuration.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="7.3.1" />
    <PackageReference Include="Wireloom.Dds.Generator"
                      Version="0.2.0"
                      PrivateAssets="all" />
    <DdsIdl Include="Contracts\Telemetry.idl" />
  </ItemGroup>
</Project>
```

For example, `Contracts\Telemetry.idl` can contain:

```idl
module Telemetry {
  struct Sample {
    long id;
    string<64> label;
    sequence<float, 8> values;
  };
};
```

The `DdsIdl` item identifies a generation root. Files included by that root are
tracked as inputs but are not generated as independent roots. For include
directories and preprocessing options, see the [package usage guide](src/Wireloom.Dds.Generator/README.md).

Generated source appears in the IDE as Roslyn generated documents. To write it
under `obj` for inspection, add:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

## Support and compatibility

Wireloom is a preview implementation with a growing, feature-specific
compatibility surface. It supports substantial parts of the Connext IDL data
type model, including modules, structs, aliases, enums, unions, constants,
collections, arrays, and preprocessing. Unsupported declarations are reported
with diagnostics instead of being silently omitted.

Check the [feature coverage index](docs/corpus/FEATURE-COVERAGE.md) for the
current status of individual IDL shapes. The checked compatibility corpus uses
RTI Connext DDS 7.7.0 / `rtiddsgen` 4.7.0 reference output; the documented
runtime compatibility floor is RTI 7.3.1. Current consumer evidence targets
`net10.0` and C# 12 or later.

Corpus results compare source-generation behavior and generated C# shape. They
do not establish serialization-byte equivalence, live DDS behavior, or C++
interoperability. Those require separate runtime and interoperability evidence.

## Documentation

- [Package usage and configuration](src/Wireloom.Dds.Generator/README.md)
- [Feature coverage](docs/corpus/FEATURE-COVERAGE.md)
- [Preprocessor feature matrix](docs/PREPROCESSOR-FEATURES.md)
- [Code style](docs/CODE-STYLE.md)
- [Architecture overview](docs/architecture/arc42/index.md)
- [Incremental generator baseline](docs/performance/incremental-generator-baseline.md)
- [Contributing and maintainer guide](CONTRIBUTING.md)

## License

Wireloom is licensed under the [MIT License](LICENSE). That license does not
apply to the RTI-generated oracle sources in [`docs/corpus/oracles`](docs/corpus/oracles)
or other third-party materials. Review the [corpus guidance](docs/corpus/README.md)
before redistributing or changing those reference artifacts.
