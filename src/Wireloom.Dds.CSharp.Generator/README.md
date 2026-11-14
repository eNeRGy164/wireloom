# Wireloom.Dds.Generator 1.1.0

Wireloom.Dds.Generator 1.1.0 is a Roslyn source generator and MSBuild integration for
generating C# types from the RTI Connext DDS IDL subset. Add it to a .NET
project that owns DDS data contracts; the package generates source during the
normal build and does not invoke Java, native tooling, or `rtiddsgen`.

Use a .NET 8 or later SDK and target framework, C# 12 or later, and an explicit
`Rti.ConnextDds` reference. Running a DDS application also requires RTI's native
runtime assets and applicable license. See the
[1.1.0 release record](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
for supported configurations and known limits.

## Quick start

Create a .NET console project and add the generator, the RTI runtime, and an
IDL root to its project file:

```powershell
dotnet new console --name WireloomQuickStart --framework net8.0
cd WireloomQuickStart
```

Replace `WireloomQuickStart.csproj` with:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>12.0</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
    <PackageReference Include="Wireloom.Dds.Generator" Version="1.1.0" PrivateAssets="all" />
    <DdsIdl Include="Telemetry.idl" />
  </ItemGroup>
</Project>
```

Create `Telemetry.idl` beside the project file:

```idl
module Telemetry {
  struct Sample {
    long id;
    string<64> label;
    sequence<float, 8> values;
  };
};
```

Replace `Program.cs` with code that uses the generated contract:

```csharp
var sample = new Telemetry.Sample { id = 42, label = "temperature" };
sample.values.Add(20.5F);
Console.WriteLine(sample.label);
```

Build the project to generate and compile `Telemetry.Sample` and its use site:

```shell
dotnet build
```

After editing a root or included IDL file, build the project to regenerate its
C# contracts. In VS Code, use **Ctrl+Shift+B** with a build task; IntelliSense
may show the previous generated types until you build. If it stays stale,
run **CSharp: Rerun Source Generators**
from the Command Palette. See the
[developer workflow](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/developer-workflow.md#route-vs-code-build-diagnostics)
for build-task setup.

The generator runs during the normal build; there is no separate code
generation command. To run communicating publisher and subscriber processes,
follow the [Hello World guide](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/hello-world.md),
which also covers RTI runtime and license prerequisites.

For a project replacing existing RTI-generated C#, follow the
[migration guide](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/migration/rtiddsgen-to-wireloom.md). It maps
common `rtiddsgen` options, identifies known API differences, and gives a
validation and rollback procedure.

This README describes version 1.1.0. Source and reference links target
`v1.1.0`; the [1.1.0 qualification record](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
retains release results, known limits, and later clarifications.

From 1.0.0, the [1.x compatibility contract](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#compatibility-and-stability)
protects supported generated APIs, configuration, behavior, defaults, and
consumer minimums. Patch releases correct defects without adding features;
corrections to faulty output may require regeneration and coordinated peer
upgrades. Review the installed version's release notes before upgrading.

## Find your task

- [Build and run Hello World](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/hello-world.md) with a publisher and subscriber.
- [Install the package](#install) and [declare an IDL root](#declare-an-idl-generation-root).
- [Share compiled contracts](#share-generated-contracts) between applications.
- [Understand IDL-to-C# mappings](#common-idl-to-c-mappings).
- [Configure generation settings, defaults, and overrides](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md).
- [Inspect generated source](#inspect-generated-source), [extend partial types, and use IDE/analysis tooling](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/developer-workflow.md).
- Look up [annotations](#annotation-reference), [diagnostics](#diagnostics), and [effective limits](#effective-limits-for-unbounded-values).
- [Troubleshoot generation](#troubleshooting).
- [Upgrade or roll back](#upgrade-or-roll-back).

## Install

Add the generator and the RTI runtime explicitly. The generator package does
not add `Rti.ConnextDds` transitively because the application owns its runtime
version and configuration.

Running the DDS application also requires the RTI runtime's native assets and
a valid license for the NuGet.org distribution. Follow RTI's
[license setup](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/installation_guide/license_management.html#installing-license-file-for-c-or-python)
and the [Hello World prerequisites](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/hello-world.md#before-you-start).

```xml
<ItemGroup>
  <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
  <PackageReference Include="Wireloom.Dds.Generator" Version="1.1.0" PrivateAssets="all" />
</ItemGroup>
```

The generator selects its API profile from the resolved `Rti.ConnextDds`
assembly. Versions 6.1.2 through versions below 7.3.1 select the 6.1.2
profile; versions 7.3.1 through versions below 7.7.0 select the 7.3.1 profile;
7.7.0 and later select the 7.7.0 profile. Missing references and
versions below 6.1.2 produce `DDSG0003`. Test results apply to the exact RTI
release used in each run. The consumer project must resolve the RTI version
whose API and generated contracts it intends to use.

Feature availability follows the selected RTI profile; see the
[support policy](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#rti-version-qualification).

Profile selection follows your resolved runtime reference; there is no public
profile override. Rebuild and review generated contracts when changing RTI
profiles.

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
    id = 42,
    label = "temperature"
};

sample.values.Add(20.5F);
sample.values.Add(21.0F);

var typeSupport = Telemetry.SampleSupport.Instance;
```

For ordinary typed DDS code, use the generated data type with RTI's normal
participant, topic, and writer APIs:

```csharp
using Rti.Dds.Domain;
using Rti.Dds.Topics;

using var participant = DomainParticipantFactory.Instance.CreateParticipant(domainId: 0);
using var topic = participant.CreateTopic<Telemetry.Sample>("Telemetry");
using var writer = participant.ImplicitPublisher.CreateDataWriter(topic);

var sample = new Telemetry.Sample { id = 42, label = "temperature" };
sample.values.Add(20.5F);
writer.Write(sample);
```

RTI's typed topic API locates the generated type support for `Telemetry.Sample`.
Use `Telemetry.SampleSupport.Instance` directly when application code needs
its utilities, such as `DynamicType`, a serializer, or formatted output. The
support instance does not create a topic or publish data. This example shows
the API handoff; it still requires the RTI runtime, its deployment/licensing
prerequisites, matching topic/type/QoS on a peer, and a running DDS domain.
See RTI's [C# publication examples](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_csharp/group__DDSPublicationExampleModule.html)
for the broader publish/subscribe lifecycle.

Generated types include the managed data contract and the RTI-specific support
surface where the IDL shape is supported: native companions, type support,
interpreted plugins, and DynamicType metadata.

## Share generated contracts

For types used by several applications, put the `DdsIdl` roots in one class
library and reference that library from each application. The library's build
compiles generated C# into its assembly; consumers of the compiled library do
not need the generator package unless they also declare their own IDL roots.

```xml
<!-- Contracts.csproj -->
<ItemGroup>
  <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
  <PackageReference Include="Wireloom.Dds.Generator" Version="1.1.0" PrivateAssets="all" />
</ItemGroup>

<ItemGroup>
  <DdsIdl Include="Contracts\Telemetry.idl" />
</ItemGroup>
```

```xml
<!-- Application.csproj: consume the compiled contracts package -->
<ItemGroup>
  <PackageReference Include="Company.Contracts" Version="1.0.0" />
  <!-- Optional explicit reference to pin or override the runtime version. -->
  <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
</ItemGroup>
```

The normal `Rti.ConnextDds` reference in the contracts project is a package
dependency when that project is packed. Applications can rely on that
dependency or reference the runtime explicitly to control resolution. The
generator reference uses `PrivateAssets="all"`, so the contracts package does
not configure source generation for downstream projects. Generated `.g.cs`
documents are build intermediates; the compiled contracts assembly is the
shareable artifact. Check the contracts package's target frameworks against
the application before publishing it.

## Common IDL-to-C# mappings

These examples show how to use the generated public API. RTI supplies the
sequence containers and DDS runtime. See the
[value-semantics guide](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/generated-value-semantics.md)
for copying, ownership, optional presence, and union behavior.
The sequence examples use RTI's `Omg.Types` and `Rti.Types` namespaces.

| IDL declaration                                                     | Generated C# API and use                                                                                                          |
| :------------------------------------------------------------------ | :-------------------------------------------------------------------------------------------------------------------------------- |
| `long id;`                                                          | `int id { get; set; }`<br>Assign with `sample.id = 42;`.                                                                          |
| `typedef sequence<long, 4> BoundedLongs;`                           | `ISequence<int> Value` with `[Bound(4)]`.<br>Use `var bounded = new BoundedLongs(); bounded.Value.Add(17);`.                      |
| `@optional sequence<long, 4> values;`<br>`@optional long items[2];` | `ISequence<int>? values`; `int[]? items`.<br>Set `items = [1, 2]`; set `values = new Sequence<int> { 17 }` or `null` for absence. |
| `typedef long Scalar;`<br>`typedef sequence<Scalar> Scalars;`       | `ISequence<int> Value`; scalar aliases erase in collection elements.                                                              |
| `union Choice switch(Kind) { case Number: long number; ... };`      | `int number`; assigning selects the branch. Reading an inactive branch throws `InvalidOperationException`.                        |

See [effective limits](#effective-limits-for-unbounded-values) before sending
strings or collections through DDS, and the
[migration guide](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/migration/rtiddsgen-to-wireloom.md)
when replacing RTI-generated contracts.

## Configure includes and preprocessing

Use project settings and `DdsIdl` metadata in the project owning the contracts:

```xml
<PropertyGroup>
  <DdsIdlIncludeDirectories>$(MSBuildProjectDirectory)/Contracts/Shared</DdsIdlIncludeDirectories>
</PropertyGroup>
<ItemGroup>
  <DdsIdl Include="Contracts/Telemetry.idl" Defines="PRODUCT=Widget" />
</ItemGroup>
```

Defines, undefines, include directories, and strict validation aggregate across
all generating roots. They are not isolated per-root environments. Overrides
for mapping, sizes, and unbounded support belong to declarations in each
input file. A root's file overrides do not flow into its includes. Use the
[configuration reference](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md)
for the complete settings, defaults, precedence, strictness, and identifier rules.

### Multiple roots and shared IDL

Use multiple roots when one project owns several types that include shared
declarations. For example:

```text
Contracts/
  Telemetry.idl
  Commands.idl
  Shared/
    Common.idl
```

```idl
// Shared/Common.idl
#ifndef WIRELOOM_SHARED_COMMON_IDL
#define WIRELOOM_SHARED_COMMON_IDL
module Shared {
  struct Identity { long id; };
};
#endif
```

```idl
// Telemetry.idl
#include <Common.idl>
module Telemetry { struct Sample { Shared::Identity source; long value; }; };
```

```idl
// Commands.idl
#include <Common.idl>
module Commands { struct Request { Shared::Identity source; long action; }; };
```

Configure the include directory and declare only the two roots:

```xml
<PropertyGroup>
  <DdsIdlIncludeDirectories>$(MSBuildProjectDirectory)\Contracts\Shared</DdsIdlIncludeDirectories>
</PropertyGroup>

<ItemGroup>
  <DdsIdl Include="Contracts\Telemetry.idl" />
  <DdsIdl Include="Contracts\Commands.idl" />
</ItemGroup>
```

The shared file is a tracked input and contributes `Shared.Identity` to the
same generated assembly. Identical shared include output is emitted once for
that project. For types used by several application projects, put the roots in
one contracts project and reference its assembly from each application. If
separate projects independently generate a shared type, each assembly owns a
copy with that type's fully qualified name; avoid referencing both copies from
the same consumer. Macro-selected variants require separate builds because
root metadata is batch-wide.

## Choose the array-of-sequences mapping

The default `RtiCSharp` mapping flattens a direct array of sequences and reports
`DDSG0105`. Select `Idl` to preserve the fixed array and each independently
sized sequence:

```xml
<PropertyGroup>
  <DdsIdlSequenceArrayMapping>Idl</DdsIdlSequenceArrayMapping>
</PropertyGroup>
```

For `struct Sample { sequence<long, 4> values[2]; };`, the IDL-preserving API
uses `sample.values[0].Value.Add(10)`. Selecting this mapping changes the
C# API and wire schema; it differs from flattened RTI C# direct-form bindings.
File metadata can override the project setting for declarations in that file.
See [configuration and compatibility effects](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md#choose-the-sequence-array-mapping)
and [focused wire evidence](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/testing/sequence-array-mapping.md).

### Recursive type support

Direct self-recursive sequence members require the documented TypeObject v1
participant setup. Recursive sequence typedefs are rejected; mutually recursive
graphs are outside the supported scope. See the
[profile limitations](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#rti-version-qualification)
and [recursive-type setup](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/testing/wire-fixtures.md#recursive-fixture-metadata)
before relying on recursive contracts.

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

### Preprocessor limits

Preprocessing enforces deterministic per-input output, work, and nesting
limits. Exceeding one fails generation with a source-located error. See the
[limits reference](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md#fixed-behavior-and-configurable-limits)
and [feature matrix](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/PREPROCESSOR-FEATURES.md)
for values, scope, and supported constructs.

## Annotation reference

These annotations affect the generated contract or RTI type metadata as
described below. Annotation support depends on the declaration and RTI profile.

| Annotation or group                                                                                                                          | Behavior and use                                                                                                                                                                                                                |
| :------------------------------------------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `@topic`                                                                                                                                     | Marks a struct as a DDS topic data type in generated documentation. It does not create a topic or publish samples.                                                                                                              |
| `@key`                                                                                                                                       | Marks a member as a DDS instance key and is reflected in type support metadata. Key identity is distinct from equality of complete C# samples.                                                                                  |
| `@optional`                                                                                                                                  | Marks optional members in type support and produces nullable C# members for supported optional shapes. An optional union branch is ignored because a union always selects one branch.                                           |
| `@id`, `@hashid`, `@autoid(HASH)`                                                                                                            | Set explicit or hash-derived DDS member IDs in supported aggregate contexts. Other `@autoid` mappings are not separately qualified here.                                                                                        |
| `@final`, `@appendable`, `@mutable`                                                                                                          | Set the corresponding extensibility kind in generated DynamicType metadata for covered declarations.                                                                                                                            |
| `@default`, `@min`, `@max`, `@range`, `@unit`                                                                                                | Supported on primitive/enum members as applicable. Defaults initialize supported values; ranges and units are represented in generated type support annotations. Ranges require primitive members; defaults also support enums. |
| `@value`, `@default_literal`                                                                                                                 | Set explicit enum numeric values and the default enum literal for covered enum shapes.                                                                                                                                          |
| `@must_understand`                                                                                                                           | Requires the 7.7.0 profile. The 6.1.2 and 7.3.1 profiles report `DDSG0004` and emit no code.                                                                                                                                    |
| `@external`                                                                                                                                  | Leaves the managed C# member shape unchanged. Check the support policy before relying on external-member serialization behavior.                                                                                                |
| `@language_binding`, `@transfer_mode`, `@data_representation`, `@allowed_data_representation`, `@nested`, `@default_nested`, `@resolve_name` | Recognized syntax with profile-specific effects. `FLAT_DATA` is ignored for C#. Consult the support policy before using these annotations to select wire representation or naming.                                              |
| Unknown annotation                                                                                                                           | Emits warning `DDSG0101` and continues while ignoring the annotation. Verify that ignoring it does not change the contract you need.                                                                                            |
| `@position`, `@bit_bound`; invalid `@service` target                                                                                         | Recognized but unsupported uses emit `DDSG0102` and reject generation. A `@service("DDS")` interface is instead ignored with `DDSG0103`; interfaces do not become C# types.                                                     |

For enum arrays, `@default_literal` initializes managed and native array
elements from the declared enum member. Bounds must resolve to positive
integer values. Review ignored-annotation warnings before relying on the
resulting contract; an annotation accepted by RTI may have no effect in C#.

## Diagnostics

| ID         | Meaning                                                              | Typical action                                                                                             |
| :--------- | :------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------- |
| `DDSG0001` | IDL generation failed                                                | Fix the source-located parse, include, semantic, or unsupported-feature error                              |
| `DDSG0002` | C# 12 or later is required                                           | Set the consumer's language version to C# 12 or later                                                      |
| `DDSG0003` | A compatible RTI runtime was not found                               | Add an explicit `Rti.ConnextDds` reference at version 6.1.2 or later                                       |
| `DDSG0004` | An IDL feature is unavailable in the selected RTI API profile        | Remove the unsupported feature or use a compatible RTI version; `@must_understand` requires 7.7.0 or later |
| `DDSG0101` | An unknown IDL annotation was ignored                                | Verify the annotation is supported by the managed generator if it affects the generated contract           |
| `DDSG0102` | A recognized but unsupported annotation use was reported             | Remove the annotation or use a supported declaration context; this warning accompanies rejected generation |
| `DDSG0103` | An IDL interface was ignored because no C# interface type is emitted | Names are checked before bindings are omitted; ordinary and DDS-service body rules differ                  |
| `DDSG0104` | A function-like macro was invoked with the wrong number of arguments | Correct the macro invocation; RTI-compatible expansion continues after the warning                         |
| `DDSG0105` | A direct array of sequences may not preserve IDL semantics           | Use a sequence typedef or opt into `SequenceArrayMapping="Idl"`                                            |
| `DDSG0106` | An active `#warning` directive was encountered                       | Review the warning in the IDL or included file; preprocessing and generation continue                      |
| `DDSG0107` | An active `#pragma message` directive was encountered                | Review the informational message in the IDL or included file; generation continues                         |
| `DDSG0108` | An IDL name conflicts with a keyword or another name ignoring case   | Rename the identifier or exempt the default warning; strict validation rejects the declaration             |
| `DDSG0109` | An escaped identifier normalizes to `TRUE` or `FALSE`                | Rename it to avoid ambiguity with IDL boolean literals; Wireloom still resolves it as an identifier        |

Diagnostics are owned by the generator and use the original IDL/include
location where one is available. Review the table's typical actions before
adoption; ignored annotations or interfaces and mappings flagged as potentially
lossy can affect the generated contract. Configure an individual diagnostic's
severity through the consumer's `.editorconfig`, for example:

```ini
[*.idl]
dotnet_diagnostic.DDSG0101.severity = error
```

The default severities are grouped by diagnostic family: `DDSG0001`–`DDSG0004`
are errors, `DDSG0101`–`DDSG0106` and `DDSG0108`–`DDSG0109` are warnings, and `DDSG0107`
is informational. Warnings do not all mean generation continues: `DDSG0102`
reports an unsupported annotation use and is followed by a generation error.
`DDSG0101`, `DDSG0103`,
`DDSG0104`, `DDSG0105`, `DDSG0106`, `DDSG0108`, and `DDSG0109` report cases where
generation continues in default mode. The diagnostics table describes the
action for each one.

Diagnostics point to the originating IDL file, including when the problem is
inside an included file. For example, if `Contracts/Telemetry.idl` contains:

```idl
#include "shared/Shared.idl"
```

and `Contracts/shared/Shared.idl` contains:

```idl
struct Shared {
  long values[0];
};
```

the `DDSG0001` error reports that `Array dimensions` must be a positive
`Int32`, at the array bound in `Shared.idl`. If the same declaration is placed
directly in `Telemetry.idl`, the error points to the root file instead. Start
from the reported path and line, correct the bound, and rebuild; the build
fails while a generation error remains.

## Upgrade or roll back

Change the generator `PackageReference` version to `1.1.0`, restore, and rebuild
the projects that own IDL. Review regenerated contracts and handwritten partial
extensions, then rebuild applications that consume a shared contracts assembly.
Keep the RTI runtime reference unchanged while checking the generator upgrade
so profile changes can be validated separately.

Keep existing mapping, bounds, and unbounded-support settings unless you are
intentionally changing the contract. Review the
[1.1.0 release notes](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
and check your application's peer compatibility before deployment. To roll
back, restore the previous generator pin, IDL, and configuration, then clean
and rebuild the contracts and consuming applications.

## Supported boundary

Version 1.1.0 supports documented modules, structs, nested declarations,
aliases, enums, unions, constants, primitives, strings, sequences, arrays,
includes, conditional preprocessing, keys, annotations, and extensibility.
Support depends on the exact IDL shape and selected RTI profile.

Before adopting a feature, consult the [diagnostics table](#diagnostics),
[feature coverage matrix](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/corpus/FEATURE-COVERAGE.md),
and [1.1.0 release limits](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md).

### Effective limits for unbounded values

With unbounded support disabled (the default), Wireloom assigns finite limits
to unbounded IDL values: **255 UTF-8 bytes** for narrow strings, **255 UTF-16 code
units** for wide strings, and **100 elements** for sequences. Set project properties
`DdsIdlSequenceSize` and `DdsIdlStringSize` to change those defaults.
`SequenceSize` and `StringSize` metadata on a `DdsIdl` or `AdditionalFiles`
item override the project values for declarations in that file. All settings
must be positive Int32 values; explicit IDL bounds take precedence. See the
[configuration reference](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md#fixed-behavior-and-configurable-limits)
for examples and scope details. For example, 128 copies of `é` use 256 UTF-8
bytes, so they exceed the narrow-string limit even though the C# string
contains only 128 characters.

Generated string properties do not enforce their bounds when assigned, and
sequence members expose mutable containers whose `Add` operations do not cap
the element count. RTI uses the bound in type metadata when processing DDS data.
Validate values before DDS processing; do not rely on property assignment or
collection mutation to reject over-limit samples.

To retain RTI's unbounded metadata and native conversion behavior, enable
`DdsIdlUnboundedSupport` for the project or set `UnboundedSupport="true"` on
the relevant `DdsIdl` or `AdditionalFiles` item. The file option controls the
declarations originating in that file, including when it is included by a
root. Explicit IDL bounds remain finite. In this mode the configured sequence
and string sizes remain validated but are ignored for unbounded declarations.
Native sequences and strings start with zero capacity and grow to hold the
sample. Qualification covers the documented payloads, QoS, and resource limits;
it does not establish unrestricted sample-size support. See the
[configuration reference](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/configuration.md#fixed-behavior-and-configurable-limits)
and the [support policy](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/support-and-compatibility.md#rti-version-qualification).

The package owns generation for explicit `DdsIdl` roots; RTI owns the runtime,
transport, and serialization engine. It accepts the documented settings rather
than forwarding arbitrary `rtiddsgen` arguments. Diagnostics report unsupported
declarations and ignored annotations or interfaces. Use exact-version runtime
and wire evidence when evaluating interoperability.

## Troubleshooting

When generation appears not to run, start with `dotnet build` in the consuming
project, then follow the symptom table. IDE behavior also depends on the
selected SDK and Roslyn host.

| Symptom                                               | Check and next step                                                                                                                                                                                                                                                                                                     |
| :---------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Build succeeds but no generated types appear          | Confirm the project references `Wireloom.Dds.Generator` and declares the intended roots as `DdsIdl` items. Other `.idl` files are tracked includes, not roots.                                                                                                                                                          |
| Generator package is present but has no effect        | Restore the project, inspect the resolved package version and analyzer/build assets, then run `dotnet build` from the project directory. If the build reports an analyzer load or Roslyn assembly error, verify the SDK/compiler host; older SDK hosts are not all qualified.                                           |
| `DDSG0002`                                            | Set `<LangVersion>12.0</LangVersion>` or later. This diagnostic is reported when the generator loads and the compiler language version is too old.                                                                                                                                                                      |
| `DDSG0003`                                            | Add an explicit `Rti.ConnextDds` reference at a version accepted by the guard; the generator package does not add the runtime transitively.                                                                                                                                                                             |
| `DDSG0001`                                            | Read the diagnostic text and source location. Check IDL syntax, include resolution, semantic rules, and feature coverage. This is also the generation-failure diagnostic for preprocessing limits.                                                                                                                      |
| Generated documents are absent in the IDE             | First confirm the command-line build succeeds. Roslyn generated documents appear in the IDE; to inspect files on disk, enable `EmitCompilerGeneratedFiles` and set `CompilerGeneratedFilesOutputPath` as described in [Inspect generated source](#inspect-generated-source).                                            |
| Build works but the IDE shows stale generated code    | In VS Code, build with **Ctrl+Shift+B**, then run **CSharp: Rerun Source Generators** if needed. If it stays stale, reload and check the selected SDK and analyzer diagnostics. See the [IDE workflow](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/developer-workflow.md#recover-an-editor-mismatch). |
| Compiler reports duplicate generated type definitions | Remove exported generated `.g.cs` files from explicit `<Compile>` items. The generator already adds its documents to compilation; do not compile the same emitted source twice.                                                                                                                                         |

### An include cannot be resolved

Check whether the include is quoted or angle-bracketed. Quoted includes are
resolved relative to the including file. For angle includes, add the directory
to `DdsIdlIncludeDirectories` or the root's `IncludeDirectories` metadata.

### A shared file is generated twice

Keep the shared file as an included input and declare only the intended root in
`DdsIdl`. A physical IDL file should normally have one generation identity per
consumer project.

### A feature is rejected

Read the source-located diagnostic and the feature coverage matrix.
`DDSG0004` identifies a feature unavailable in the selected RTI profile;
`DDSG0001` includes syntax, semantic, and unsupported-shape failures.

## Learn more

- [Documentation by task](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/README.md)
- [Report a compatibility or integration problem](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/reporting-issues.md)
- [Wireloom repository](https://github.com/eNeRGy164/wireloom)
- [Generated value semantics](https://github.com/eNeRGy164/wireloom/blob/v1.1.0/docs/guides/generated-value-semantics.md)
- [1.1.0 release qualification](https://github.com/eNeRGy164/wireloom/blob/main/docs/releases/1.1.0.md)
