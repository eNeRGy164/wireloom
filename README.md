# Wireloom

[![Coverage Status](https://coveralls.io/repos/github/eNeRGy164/wireloom/badge.svg?branch=main)](https://coveralls.io/github/eNeRGy164/wireloom?branch=main)

Wireloom is a managed .NET source generator for the RTI Connext DDS IDL subset.
It turns IDL generation roots into C# data types and the RTI type-specific
support needed by a consuming application—at build time, inside the Roslyn
compiler, without launching Java, a native compiler, or `rtiddsgen`.

The project is deliberately evidence-driven. It is a preview implementation
with a growing compatibility surface, not a claim of drop-in replacement or
complete wire interoperability.

## Why Wireloom?

Traditional DDS code generation adds an external toolchain to a .NET build.
Wireloom keeps the generation step in the project that consumes the types:

```text
Connext IDL ──► Roslyn incremental generator ──► C# data contract
                                             ├──► native conversion helpers
                                             ├──► TypeSupport and plugin
                                             └──► DynamicType metadata
```

The RTI Connext DDS runtime remains responsible for DDS communication,
serialization, and representation negotiation. Wireloom owns parsing,
preprocessing, semantic validation, and generated C# source.

## Quick start

Wireloom targets the .NET 10 SDK pinned by [`global.json`](global.json). A
consumer references the RTI runtime explicitly and declares each IDL generation
root explicitly:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
    <PackageReference Include="Wireloom.Dds.Generator"
                      Version="0.1.0-preview.72"
                      PrivateAssets="all" />

    <DdsIdl Include="Contracts\Telemetry.idl" />
  </ItemGroup>

  <PropertyGroup>
    <DdsIdlIncludeDirectories>
      $(MSBuildProjectDirectory)\Contracts\Shared
    </DdsIdlIncludeDirectories>
  </PropertyGroup>
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

`DdsIdl` is an explicit generation root. Files reached through `#include` are
tracked inputs, but are not independently generated as roots. Project-level
`DdsIdlIncludeDirectories` and per-root `IncludeDirectories` control include
search. `Defines`, `Undefines`, and `Strict` can be supplied as item metadata
when a root needs different preprocessing or validation settings.

Generated documents appear as normal Roslyn generated documents. To write them
to disk for inspection, use the standard compiler options:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

## What is implemented

The managed front end and emitters currently cover substantial parts of the
Connext IDL data-type surface, including:

- modules, structs, nested declarations, aliases, enums, unions, and constants;
- primitive values, narrow and wide strings, bounded strings, sequences, and
  fixed or multidimensional arrays;
- collection composition, include graphs, conditional preprocessing, and
  configured macros;
- generated managed types with constructors, equality, hashing, and C# keyword
  escaping;
- RTI-native companion types, type support, interpreted plugins, DynamicType
  metadata, keys, bounds, and extensibility where the retained compatibility
  evidence supports the shape;
- source-located diagnostics for unsupported syntax and invalid input.

The exact status is maintained in the [feature coverage index](docs/corpus/FEATURE-COVERAGE.md).
“Implemented” there means source-generation behavior and, where applicable,
generated contract shape—not automatic proof of runtime or wire compatibility.

## Compatibility evidence

Wireloom compares small, attributable IDL cases with retained RTI Connext
7.7.0 / `rtiddsgen` 4.7.0 reference output. The checked-in corpus currently
contains:

| Evidence set | Cases |
| --- | ---: |
| Positive feature cases | 54 |
| Negative and diagnostic probes | 53 |
| Integration entry points | 4 |
| Total corpus cases | 111 |
| Implemented positive/integration cases | 44 / 58 |
| Compliance checks | 225 total, 224 passing |

One cyclic-alias probe is intentionally isolated while the compiler-process
harness is being completed. The snapshot above was recorded on 2026-09-24;
consult the feature index for the current result.

The corpus is source-generation evidence. It does not by itself establish
serialization-byte equivalence, live DDS behavior, or C++ interoperability.
Those claims require the additional runtime and interoperability verification
described in the [testing documentation](docs/testing/README.md) and
[evidence records](evidence/README.md).

## Build and test

Restore the locked dependencies, then run the focused unit and corpus suites:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

Pack the analyzer/source-generator package with:

```powershell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet pack src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj `
  --no-restore --configuration Release --output artifacts
```

The package integration suite consumes that packed `.nupkg` from an isolated
local feed. Its complete restore and test procedure is documented in
[`tests/Wireloom.Dds.Generator.PackageIntegration/README.md`](tests/Wireloom.Dds.Generator.PackageIntegration/README.md).

## Architecture

The implementation is organized as a small compiler pipeline:

```text
Generator
  └── IdlCompiler
      ├── IdlInputGraph
      ├── preprocessor and parser
      ├── symbol table, type resolver, and semantic validator
      ├── resolved emission plans
      └── managed/native/type-support emitters
```

The parser and semantic model are independent of Roslyn. Roslyn integration is
kept at the generator boundary, where it supplies AdditionalFiles, compiler
diagnostics, cancellation, and generated documents. The source layout mirrors
these responsibilities under [`src/Wireloom.Dds.Generator`](src/Wireloom.Dds.Generator).

The architecture is documented with a compact [arc42 baseline](docs/architecture/arc42/index.md).

## Repository map

| Path | Purpose |
| --- | --- |
| [`src/Wireloom.Dds.Generator`](src/Wireloom.Dds.Generator) | Packable Roslyn incremental generator and managed compiler |
| [`tests/Wireloom.Dds.Generator.Tests`](tests/Wireloom.Dds.Generator.Tests) | Fast in-memory front-end and emitter tests |
| [`tests/Wireloom.Dds.Generator.CorpusCompliance`](tests/Wireloom.Dds.Generator.CorpusCompliance) | Manifest-driven corpus and RTI C# shape checks |
| [`tests/Wireloom.Dds.Generator.PackageIntegration`](tests/Wireloom.Dds.Generator.PackageIntegration) | Tests against the packed NuGet analyzer |
| [`docs/corpus`](docs/corpus) | Authored IDL cases, oracle sources, and feature status |
| [`docs/architecture`](docs/architecture) | Architecture baseline and terminology |
| [`docs/testing`](docs/testing) | Test strategy and verification boundaries |
| [`evidence`](evidence) | Supporting records and retained evidence |

The design grows out of the managed-generator research in the companion
`rtiddsgen-gen` repository. That research established the Roslyn-hosted,
managed front end and the feature-level oracle approach used here.

## Boundaries and expectations

- The consumer owns the `Rti.ConnextDds` runtime reference; Wireloom does not
  bundle or transitively add the runtime.
- A resolved RTI runtime reference of version 7.3.1 or later and C# 12 or later
  are required by the generator host.
- Unsupported declarations and directives are rejected with diagnostics rather
  than silently omitted.
- Support status is feature- and evidence-specific. Collections, unions, keys,
  annotations, and extensibility are not blanket compatibility claims.
- The project does not currently promise verified Visual Studio live behavior,
  Linux builds, or C++ peer interoperability for every supported feature.

## Package publishing

Preview packages are produced from `main` and published to GitHub Packages.
Release packages are published to NuGet.org only from validated `v*.*.*` tags.
The workflows also run the test suites, generate an SBOM, and attest package
and provenance artifacts.

## License and RTI reference material

RTI-generated oracle sources and related reference material are retained for
compatibility verification under the repository's licensing and review rules.
Review the [corpus guidance](docs/corpus/README.md) before adding, regenerating,
or redistributing reference artifacts.
