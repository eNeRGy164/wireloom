# Configuration

This is the consumer configuration reference for Wireloom IDL generation.
Settings live in the project that generates the contracts, or in shared
MSBuild files such as `Directory.Build.props`. The examples are project-file
fragments; add them inside the consuming project's `<Project>` element.

- [Settings at a glance](#settings-at-a-glance)
- [Select generation roots and included files](#select-generation-roots-and-included-files)
- [Configure include directories](#configure-include-directories)
- [Define and undefine preprocessor symbols](#define-and-undefine-preprocessor-symbols)
- [Enable strict validation](#enable-strict-validation)
- [Choose the sequence-array mapping](#choose-the-sequence-array-mapping)
- [Select the consumer framework, language, and RTI runtime](#select-the-consumer-framework-language-and-rti-runtime)
- [Control diagnostics](#control-diagnostics)
- [Inspect generated files](#inspect-generated-files)
- [Configure the programmatic compiler](#configure-the-programmatic-compiler)
- [Fixed behavior and limits](#fixed-behavior-and-limits)

## Settings at a glance

Wireloom has two project properties and six input metadata settings:

| Setting                      | Where to set it                        | Default                                                           | Effect and scope                                                                                                                   |
| ---------------------------- | -------------------------------------- | ----------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| `DdsIdlIncludeDirectories`   | Project property                       | Empty                                                             | Adds semicolon-separated include search directories and tracks their IDL files. Directories are available to the generation batch. |
| `DdsIdlSequenceArrayMapping` | Project property                       | `RtiCSharp`                                                       | Sets the default mapping for direct arrays of sequences in all tracked files.                                                      |
| `Generate`                   | `AdditionalFiles` metadata             | `false` for tracked includes; forced to `true` for `DdsIdl` items | Marks a file as a generation root.                                                                                                 |
| `IncludeDirectories`         | Generating-root metadata               | Empty                                                             | Adds semicolon-separated include directories to the batch's search path.                                                           |
| `Defines`                    | Generating-root metadata               | Empty                                                             | Supplies semicolon-separated preprocessor definitions to every root's initial macro environment.                                   |
| `Undefines`                  | Generating-root metadata               | Empty                                                             | Removes semicolon-separated symbols after the batch's external defines are applied.                                                |
| `Strict`                     | Generating-root metadata               | `false`                                                           | Enables strict validation for the whole batch when any root sets `true`.                                                           |
| `SequenceArrayMapping`       | `DdsIdl` or `AdditionalFiles` metadata | Inherits the project default                                      | Overrides mapping for declarations physically in that file. Values: `RtiCSharp`, `Idl`.                                            |

Use `true` or `false` for Boolean metadata. Mapping values are case-insensitive;
blank mapping metadata inherits the project property, and an unset or blank
project property selects `RtiCSharp`. An unknown mapping value fails generation
with source-located `DDSG0001`.

**Batch settings and file settings have different precedence rules.**
`Defines`, `Undefines`, `IncludeDirectories`, and `Strict` are combined from
all generation roots. Setting them on an included file with `Generate="false"`
does not configure the batch. `SequenceArrayMapping` instead belongs to each
declaring file, including non-root files. A root's mapping override does not
flow through its includes.

Standard .NET settings also affect the consumer build:

| Setting                                                                      | Default or requirement                 | Effect                                                                                                  |
| ---------------------------------------------------------------------------- | -------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| `TargetFramework`                                                            | Consumer targets .NET 8 or higher      | Selects the application's framework, independently of the generator's `netstandard2.0` target.          |
| `LangVersion`                                                                | C# 12 or later required                | An older language version produces `DDSG0002`.                                                          |
| Resolved `Rti.ConnextDds` reference                                          | Explicit compatible reference required | Its assembly version selects the generated RTI API profile; there is no target-profile override.        |
| `EmitCompilerGeneratedFiles`                                                 | `false`                                | Adds physical generated files to the normal IDE-visible generated documents when enabled.               |
| `CompilerGeneratedFilesOutputPath`                                           | SDK-controlled                         | Chooses where physical generated files are written when emission is enabled.                            |
| `.editorconfig` `dotnet_diagnostic.<ID>.severity`                            | Generator descriptor severity          | Changes how a diagnostic is reported.                                                                   |
| `WarningsAsErrors`, `WarningsNotAsErrors`, `TreatWarningsAsErrors`, `NoWarn` | Consumer's normal compiler policy      | Promote, exempt, or suppress diagnostics; they do not change generated mappings or semantic validation. |

## Select generation roots and included files

Declare each entry point with `DdsIdl`:

```xml
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" />
  <DdsIdl Include="Contracts/Commands.idl" />
</ItemGroup>
```

Both files start generation. Their reachable `#include` files contribute
declarations to the same generated assembly. Identical shared include output
is emitted once. Other discovered `.idl` files are tracked for changes, but
their declarations are not generated unless a root reaches them.

Use `AdditionalFiles` for an explicitly tracked include or an alternative
root declaration:

```xml
<ItemGroup>
  <AdditionalFiles Include="Contracts/Shared/Common.idl" Generate="false" />
  <AdditionalFiles Include="Contracts/Extra.idl" Generate="true" />
</ItemGroup>
```

`Extra.idl` starts generation; `Common.idl` contributes declarations only when
included. `DdsIdl` is the usual root syntax. Its package target always sets
`Generate="true"`, so adding `Generate="false"` to a `DdsIdl` item does not
disable that root. Remove the `DdsIdl` declaration and use a non-generating
`AdditionalFiles` item instead. Avoid declaring the same additional file
multiple times; put its metadata on its existing explicit item.

## Configure include directories

Use absolute paths based on `MSBuildProjectDirectory` for reproducible search
paths. Both the project property and root metadata accept semicolon-separated
directories:

```xml
<PropertyGroup>
  <DdsIdlIncludeDirectories>$(MSBuildProjectDirectory)/Contracts/Shared;$(MSBuildProjectDirectory)/Vendor</DdsIdlIncludeDirectories>
</PropertyGroup>
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl"
          IncludeDirectories="$(MSBuildProjectDirectory)/Contracts/Product" />
  <DdsIdl Include="Contracts/Commands.idl" />
</ItemGroup>
```

The package tracks IDL under these directories so changes can invalidate
generation. `Contracts/Product` is available to **both roots** because root
include-directory metadata is collected for the batch.

Search behavior depends on the IDL operand:

```idl
#include "Common.idl"
#include <VendorTypes.idl>
```

- Quoted includes search the including file's directory first, then the
  configured directories.
- Angle includes search only the configured directories.
- Project-property directories precede root-item directories. Duplicate
  directories keep their first occurrence; the first matching tracked file
  wins. A missing include produces source-located `DDSG0001`.

With the example configuration, a quoted `Common.idl` next to `Telemetry.idl`
wins over `Contracts/Shared/Common.idl`. An angle include searches
`Contracts/Shared`, then `Vendor`, then the additional root directories.
Use distinct include names rather than relying on conflicting root settings.

## Define and undefine preprocessor symbols

`Defines` accepts a symbol name, which supplies value `1`, or `NAME=value`.
`Undefines` accepts symbol names. Separate entries with semicolons:

```xml
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl"
          Defines="ENABLE_LABEL;SAMPLE_BOUND=8;LEGACY_LAYOUT"
          Undefines="LEGACY_LAYOUT" />
  <DdsIdl Include="Contracts/Commands.idl" />
</ItemGroup>
```

Given this IDL:

```idl
struct Sample {
    sequence<long, SAMPLE_BOUND> values;
#if ENABLE_LABEL
    string<32> label;
#endif
#if defined(LEGACY_LAYOUT)
    long legacyCode;
#endif
};
```

The generated contract has a sequence with maximum length eight and a
`label` member; it has no `legacyCode`. The same external symbols are available
when preprocessing `Commands.idl`.

All roots' external defines are applied before all roots' external undefines,
so an undefine removes a symbol even if a different root defines it. In-file
`#define` and `#undef` can subsequently change that root's macro state. Each
root starts again from the batch's external state; in-file changes do not
carry into the next root. Avoid conflicting values for the same external
symbol. Use separate projects or builds for independently configured variants.

These settings configure the IDL preprocessor. They do not set C#
`DefineConstants`, and C# symbols do not automatically become IDL symbols.

## Enable strict validation

```xml
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" Strict="true" />
  <DdsIdl Include="Contracts/Commands.idl" Strict="false" />
</ItemGroup>
```

Strict validation is enabled for **both roots** because at least one root
requests it. `Strict="false"` cannot opt one root out of a strict batch.

The current additional strict rule rejects a derived struct or valuetype
that declares another key member:

```idl
struct Base { @key long tenant; };
struct Derived : Base { @key long localId; };
```

With the default `Strict="false"`, this declaration is accepted. With strict
validation enabled, generation fails with source-located `DDSG0001`.
Ordinary syntax and semantic checks run in either mode. `Strict` does not
enable a general IDL-conformance mode, turn warnings into errors, or change
sequence-array mapping. It is not established as equivalent to RTI's
`rtiddsgen -strict` option.

## Choose the sequence-array mapping

This setting affects direct sequence-array members such as:

```idl
struct Sample {
    sequence<long, 4> values[2];
};
```

IDL describes exactly two sequences, each containing zero to four integers.

| Mode                  | Generated member shape                                           | Example value                     | Wire member metadata                      | Diagnostic            |
| --------------------- | ---------------------------------------------------------------- | --------------------------------- | ----------------------------------------- | --------------------- |
| `RtiCSharp` (default) | `ISequence<int> values`                                          | One sequence: `[10, 20, 30]`      | Sequence of integers; outer array omitted | `DDSG0105`            |
| `Idl`                 | `Sample_valuesSequence[] values`, with each sequence in `.Value` | Two sequences: `[[10, 20], [30]]` | Fixed array of two anonymous sequences    | No flattening warning |

Opt in one file while keeping other files on the default mapping:

```xml
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" SequenceArrayMapping="Idl" />
  <DdsIdl Include="Contracts/Legacy.idl" />
</ItemGroup>
```

Or opt in project-wide and override a legacy file:

```xml
<PropertyGroup>
  <DdsIdlSequenceArrayMapping>Idl</DdsIdlSequenceArrayMapping>
</PropertyGroup>
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" />
  <DdsIdl Include="Contracts/Legacy.idl" SequenceArrayMapping="RtiCSharp" />
</ItemGroup>
```

Both examples generate structured sequence arrays in `Telemetry.idl` and
flattened members in `Legacy.idl`. For the IDL above, structured access is:

```csharp
var sample = new Sample();
sample.values[0].Value.Add(10);
sample.values[0].Value.Add(20);
sample.values[1].Value.Add(30);
```

The array is allocated at its declared dimensions, and each element starts
with its own empty sequence. Generated wrapper names gain trailing underscores
if needed to avoid type or companion-name collisions.

### Included files own their mapping

An override on `Telemetry.idl` applies to declarations in `Telemetry.idl`, not
to types declared in `Shared.idl`. Configure the shared file explicitly when
it should differ from the project default:

```xml
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" SequenceArrayMapping="Idl" />
  <AdditionalFiles Include="Contracts/Shared.idl"
                   Generate="false" SequenceArrayMapping="Idl" />
</ItemGroup>
```

This keeps the shared type's mapping consistent when several roots include
it. Precedence for each declaring file is: file metadata with a value, then
the project property with a value, then `RtiCSharp`.

### Compatibility effects

Changing the mode changes the affected C# API and wire schema. The default
preserves RTI C#'s flattened mapping. `Idl` preserves the structure supported
by RTI C++ and Java; it differs from RTI C#/Python's flattened direct-form
bindings. A flat sequence cannot retain the boundaries between two sequences.

Ordinary sequences, other arrays, and explicit sequence typedefs retain their
existing mappings. For example, `typedef sequence<long, 4> Numbers;` followed
by `Numbers values[2];` already preserves both layers in either mode.
The setting does not select an RTI version or promise general IDL compliance.
Recursive direct sequence arrays are currently rejected in `Idl` mode with
`DDSG0001`; there is no silent fallback to flattening.

Both supported RTI profiles have typed runtime coverage and C149 C++ payload
checks. The dedicated corpus case `05-array-of-sequences-idl` also passes all 16
planned live DDS exchanges between Wireloom C# and RTI C++ on each exact RTI
7.3.1 and 7.7.0 release. These results qualify the four recorded fixtures;
they do not establish interoperability for every IDL shape or language.
See the [verification record](../testing/sequence-array-mapping.md).

## Select the consumer framework, language, and RTI runtime

Use the normal project and package settings:

```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <LangVersion>12.0</LangVersion>
</PropertyGroup>
<ItemGroup>
  <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
</ItemGroup>
```

Consumers support .NET 8 and higher with C# 12 or later. The generator targets
`netstandard2.0`; the repository's .NET 10 development target does not change
your application's target. In projects with central package management, select the
RTI version in `Directory.Packages.props` instead of the package-reference item.

Wireloom selects its API profile from the **resolved RTI assembly version**:

| Resolved version              | Effect                           |
| ----------------------------- | -------------------------------- |
| Missing or below `7.3.1`      | `DDSG0003`; no generated sources |
| `7.3.1` through below `7.7.0` | Selects the 7.3.1 API profile    |
| `7.7.0` and later             | Selects the 7.7.0 API profile    |

Changing profiles can change generated runtime API calls and feature
availability. For example, `@must_understand` fails with `DDSG0004` under the
7.3.1 profile and is supported under 7.7.0. Regenerate and review contracts
when changing the RTI reference. Only exact 7.3.1 and 7.7.0 releases are
qualification targets; a mapped version is not automatically qualified.
See the [support policy](support-and-compatibility.md).

`RootNamespace` does not override IDL module-to-C# namespace mapping.
`Nullable` controls the consumer's nullable analysis; it does not choose the
IDL mapping. DDS QoS, domain IDs, discovery, and licensing remain RTI runtime
configuration; they are not Wireloom generation settings.

## Control diagnostics

Generator defaults are errors for `DDSG0001`–`DDSG0004`, warnings for
`DDSG0101`–`DDSG0106`, and informational messages for `DDSG0107`.
The [diagnostic reference](../../src/Wireloom.Dds.Generator/README.md#diagnostics)
explains the individual causes.

For example, reject unknown IDL annotations through `.editorconfig`:

```ini
[*.idl]
dotnet_diagnostic.DDSG0101.severity = error
```

The build now treats ignored unknown annotations as errors. This does not
teach the generator how to implement those annotations.

To reject potentially flattened sequence arrays, use a project warning policy:

```xml
<PropertyGroup>
  <WarningsAsErrors>$(WarningsAsErrors);DDSG0105</WarningsAsErrors>
</PropertyGroup>
```

For an intentional legacy mapping in a project that promotes all warnings:

```xml
<PropertyGroup>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <WarningsNotAsErrors>$(WarningsNotAsErrors);DDSG0105</WarningsNotAsErrors>
</PropertyGroup>
```

`DDSG0105` remains visible as a warning. Alternatively,
`<NoWarn>$(NoWarn);DDSG0105</NoWarn>` suppresses it. Neither exemption nor
suppression corrects the flattened type; use the mapping setting to change
that behavior. Suppressing a generation error does not restore the sources
that failed to generate.

## Inspect generated files

Generated documents are normally available in the IDE without writing source
files to a custom directory. To inspect physical output:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

This writes the compiler's generated documents under the intermediate output
directory, normally `obj/generated`. It does not change types, namespaces,
wire metadata, or the normal compilation. Keep the location under `obj` so
the SDK's ordinary C# source glob does not compile the generated files again.
Wireloom has no separate `rtiddsgen`-style `-d` output setting.

## Configure the programmatic compiler

Callers of `IdlCompiler` supply `IdlInput` objects directly. Constructor options
correspond to the build settings, with one important default difference:
`IdlInput.generate` defaults to `true`; automatically tracked MSBuild includes
have `Generate="false"`.

This API is for tools referencing the compiler assembly directly. The normal
analyzer package reference does not add it as an application runtime library.

```csharp
using System.Collections.Generic;
using System.Threading;
using Wireloom;

var inputs = new List<IdlInput>
{
    new(
        path: "Telemetry.idl",
        text: """
            #include "Shared.idl"
            struct Sample { sequence<long, SAMPLE_BOUND> values[2]; };
            """,
        sequenceArrayMapping: IdlSequenceArrayMapping.Idl,
        generate: true,
        strict: false,
        defines: ["SAMPLE_BOUND=4"],
        undefines: ["LEGACY_LAYOUT"],
        includeDirectories: []),
    new(
        path: "Shared.idl",
        text: "struct Shared { long id; };",
        generate: false)
};

var sources = IdlCompiler.CompileSources(inputs, CancellationToken.None);
```

`path` provides input identity and diagnostic/include locations; `text`
provides the source. Supply included files as inputs too: an include directory
searches the supplied inputs rather than loading arbitrary files from disk.
`strict`, `defines`, `undefines`, and `includeDirectories` retain the same
batch semantics. `sequenceArrayMapping` remains file-owned; omitting it selects
`RtiCSharp`. Pass the caller's cancellation token instead of
`CancellationToken.None` when the caller should be able to cancel compilation.

The public programmatic compiler currently uses the 7.7.0 emission profile.
Consumer builds using the Roslyn generator instead select a profile from
their resolved runtime reference. The profile-selection implementation is
internal; there is no public version-setting argument.

## Fixed behavior and limits

The package emits C# namespaces from IDL modules, integrates through Roslyn,
and generates the supported type-specific support alongside data types.
It has no switches for another output language, a namespace override, an
external preprocessor executable, or disabling the standard support emitters.
Wireloom does not accept `rtiddsgen` command-line options; consult the
[migration option map](../migration/rtiddsgen-to-wireloom.md#rtiddsgen-option-map).

Unbounded sequences currently have an effective limit of 100 elements, and
unbounded strings have an effective limit of 255 bytes/code units according
to their narrow/wide mapping. Use explicit IDL bounds to declare application
limits. There is no public `-unboundedSupport` equivalent or MSBuild setting
for changing these defaults.

Preprocessing safety limits are also fixed:

| Limit                        | Maximum                               |
| ---------------------------- | ------------------------------------: |
| Preprocessed output          | 4,194,304 UTF-16 code units per input |
| Macro expansion work         | 100,000 operations per input          |
| Macro expansion depth        | 64 nested expansions                  |
| Conditional expression depth | 256 nested expressions                |
| Include nesting              | 128 levels                            |
| Source-origin alignment work | 100,000 work units per expanded line  |

Exceeding a limit fails generation with a source-located error. Simplify macro
expansion, reduce nesting, or split inputs while keeping each resulting input
within the limits. Neither `Strict="false"` nor diagnostic suppression removes
these limits. See the [preprocessor feature matrix](../PREPROCESSOR-FEATURES.md).

Repository-only test, corpus-export, and wire-runner properties/environment
variables are documented with their test projects. They are not consumer
generator configuration options.
