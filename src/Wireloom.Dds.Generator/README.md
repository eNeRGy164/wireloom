# Wireloom.Dds.Generator

Wireloom.Dds.Generator is a Roslyn source generator and MSBuild integration for
generating C# types from the RTI Connext DDS IDL subset. Add it to a .NET
project that owns DDS data contracts; the package generates source during the
normal build and does not invoke Java, native tooling, or `rtiddsgen`.

This is a preview package. Support is feature-specific and evidence-driven;
successful compilation of one IDL shape is not a blanket claim of RTI or wire
compatibility.

## Install

Add the generator and the RTI runtime explicitly. The generator package does
not add `Rti.ConnextDds` transitively because the application owns its runtime
version and configuration.

```xml
<ItemGroup>
  <PackageReference Include="Rti.ConnextDds" Version="7.3.1" />
  <PackageReference Include="Wireloom.Dds.Generator"
                    Version="0.1.0"
                    PrivateAssets="all" />
</ItemGroup>
```

The repository is tested against RTI Connext DDS 7.3.1 and C# 12 or later. The
consumer project must resolve a compatible `Rti.ConnextDds` reference and use
C# 12 or later.

## Declare an IDL generation root

Declare each file that starts generation with a `DdsIdl` item:

```xml
<ItemGroup>
  <DdsIdl Include="Contracts\Telemetry.idl" />
</ItemGroup>
```

The IDL root can include other files. Included files are tracked inputs and are
not independently generated as roots.

```idl
module Telemetry {
  struct Sample {
    long id;
    string<64> label;
    sequence<float, 8> values;
  };
};
```

The generated API follows the IDL module structure. For the example above, the
consumer can use the generated `Telemetry.Sample` data type and, when the
selected feature shape has type support, `Telemetry.SampleSupport`:

```csharp
var sample = new Telemetry.Sample
{
    Id = 42,
    Label = "temperature",
    Values = [20.5f, 21.0f]
};

var typeSupport = Telemetry.SampleSupport.Instance;
```

Generated types include the managed data contract and the RTI-specific support
surface where the IDL shape is supported: native companions, type support,
interpreted plugins, and DynamicType metadata.

## Configure includes and preprocessing

Use project properties for defaults shared by roots, or item metadata for one
root:

```xml
<PropertyGroup>
  <DdsIdlIncludeDirectories>
    $(MSBuildProjectDirectory)\Contracts\Shared
  </DdsIdlIncludeDirectories>
</PropertyGroup>

<ItemGroup>
  <DdsIdl Include="Contracts\Telemetry.idl"
          IncludeDirectories="$(MSBuildProjectDirectory)\Contracts\Telemetry"
          Defines="ENABLE_DIAGNOSTICS;PRODUCT_VARIANT=Enterprise"
          Undefines="LEGACY_LAYOUT"
          Strict="true" />
</ItemGroup>
```

Supported metadata:

| Setting                    | Scope         | Meaning                                                                                       |
| :------------------------- | :------------ | :-------------------------------------------------------------------------------------------- |
| `DdsIdlIncludeDirectories` | Project       | Semicolon-separated directories searched for configured includes and tracked for invalidation |
| `IncludeDirectories`       | `DdsIdl` item | Additional include directories for that root                                                  |
| `Defines`                  | `DdsIdl` item | Semicolon-separated preprocessor symbols                                                      |
| `Undefines`                | `DdsIdl` item | Semicolon-separated symbols removed before preprocessing                                      |
| `Strict`                   | `DdsIdl` item | Enables the stricter RTI-compatible semantic validation boundary                              |

Quoted relative includes resolve from the including file's directory. Angle
includes use the configured include directories. Keep a shared file as an
included input unless it is intentionally a separate generation root.

## Inspect generated source

Roslyn exposes generated documents through the IDE. To also write physical
generated files under `obj`, use the standard compiler options:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

The package does not create a separate command-line output directory or run a
pre-generation step.

## Diagnostics

| ID         | Meaning                                | Typical action                                                                |
| :--------- | :------------------------------------- | :---------------------------------------------------------------------------- |
| `DDSG0001` | IDL generation failed                  | Fix the source-located parse, include, semantic, or unsupported-feature error |
| `DDSG0002` | C# 12 or later is required             | Set the consumer's language version to C# 12 or later                         |
| `DDSG0003` | A compatible RTI runtime was not found | Add an explicit `Rti.ConnextDds` reference at the tested compatible version   |

Diagnostics are owned by the generator and use the original IDL/include
location where one is available. Configure severity through standard
`.editorconfig` entries, for example:

```ini
dotnet_diagnostic.DDSG0001.severity = error
```

## Supported boundary

The current corpus exercises modules, structs, nested declarations, aliases,
enums, unions, constants, primitive values, narrow and wide strings, bounded
strings, sequences, arrays, include graphs, conditional preprocessing, keys,
annotations, and extensibility. Several of these are supported only for the
specific shapes covered by the retained RTI oracle evidence.

The package intentionally rejects unsupported declarations and directives
instead of emitting an incomplete contract. Before adopting a feature, check
the [feature coverage matrix](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/FEATURE-COVERAGE.md)
and the [corpus rules](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/README.md).

The compatibility corpus compares source-generation behavior and generated C#
shape. It does not automatically prove serialization-byte equivalence, live DDS
behavior, or C++ interoperability.

## What this package does not do

- It does not provide the RTI DDS runtime, transport, or serialization engine.
- It does not invoke or forward arbitrary arguments to `rtiddsgen`.
- It does not implicitly generate every `.idl` file in a project; roots are
  explicit `DdsIdl` items.
- It does not promise complete Connext IDL coverage or universal drop-in
  compatibility with RTI-generated C#.
- It does not silently omit unsupported declarations.

## Troubleshooting

### The generator reports `DDSG0003`

Add the runtime package to the consuming project. The generator package is
private build tooling; the application must reference `Rti.ConnextDds` itself.

### An include cannot be resolved

Check whether the include is quoted or angle-bracketed. Quoted includes are
resolved relative to the including file. For angle includes, add the directory
to `DdsIdlIncludeDirectories` or the root's `IncludeDirectories` metadata.

### A shared file is generated twice

Keep the shared file as an included input and declare only the intended root in
`DdsIdl`. A physical IDL file should normally have one generation identity per
consumer project.

### A feature is rejected

Read the source-located `DDSG0001` diagnostic, then check the feature coverage
matrix. Rejection is intentional when the current managed compiler does not
have evidence for a safe generated contract.

## Learn more

- [Wireloom repository](https://github.com/eNeRGy164/wireloom)
- [Architecture overview](https://github.com/eNeRGy164/wireloom/blob/main/docs/architecture/arc42/index.md)
- [Testing and verification](https://github.com/eNeRGy164/wireloom/blob/main/docs/testing/README.md)
- [Package integration test](https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.PackageIntegration/README.md)

RTI-generated reference material in the repository is retained under the
applicable RTI license and review rules. It is not redistributed by this NuGet
package.
