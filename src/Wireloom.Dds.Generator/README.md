# Wireloom.Dds.Generator

Wireloom.Dds.Generator is a Roslyn source generator and MSBuild integration for
generating C# types from the RTI Connext DDS IDL subset. Add it to a .NET
project that owns DDS data contracts; the package generates source during the
normal build and does not invoke Java, native tooling, or `rtiddsgen`.

Consumer projects can target .NET 8 and higher using C# 12 or later.
Support is feature-specific and evidence-driven;
successful compilation of one IDL shape is not a blanket claim of RTI or wire
compatibility.

The example below uses a .NET 8 target and C# 12. Current package integration
evidence builds that target using the repository-pinned .NET 10 SDK; the
Roslyn hosts in older SDKs are not all qualified. RTI Connext 7.7.0 is the
tested runtime baseline.

For a project replacing existing RTI-generated C#, follow the
[migration guide](https://github.com/eNeRGy164/wireloom/blob/main/docs/migration/rtiddsgen-to-wireloom.md). It maps
common `rtiddsgen` options, identifies known API differences, and gives a
validation and rollback procedure.

## Find your task

- [Build and run Hello World](https://github.com/eNeRGy164/wireloom/blob/main/docs/guides/hello-world.md) with a publisher and subscriber.
- [Install the package](#install) and [declare an IDL root](#declare-an-idl-generation-root).
- [Share compiled contracts](#share-generated-contracts) between applications.
- [Understand IDL-to-C# mappings](#common-idl-to-c-mappings).
- [Configure includes, defines, and multiple roots](#configure-includes-and-preprocessing).
- [Inspect generated source](#inspect-generated-source).
- Look up [annotations](#annotation-reference), [diagnostics](#diagnostics), and [effective limits](#effective-limits-for-unbounded-values).
- [Troubleshoot generation](#troubleshooting).

## Install

Add the generator and the RTI runtime explicitly. The generator package does
not add `Rti.ConnextDds` transitively because the application owns its runtime
version and configuration.

Running the DDS application also requires the RTI runtime's native assets and
a valid license for the NuGet.org distribution. Follow RTI's
[license setup](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/installation_guide/license_management.html#installing-license-file-for-c-or-python)
and the [Hello World prerequisites](https://github.com/eNeRGy164/wireloom/blob/main/docs/guides/hello-world.md#before-you-start).

```xml
<ItemGroup>
  <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
  <PackageReference Include="Wireloom.Dds.Generator"
                    Version="0.4.0"
                    PrivateAssets="all" />
</ItemGroup>
```

The retained oracle corpus was generated with RTI Connext DDS 7.7.0 /
`rtiddsgen` 4.7.0. Use RTI 7.7.0 as the tested runtime baseline. The package's
minimum-version guard accepts 7.3.1 and later; it does not establish that every
generated API works with older releases. The consumer project must resolve a
compatible `Rti.ConnextDds` reference.

The generator itself targets `netstandard2.0` for Roslyn host compatibility.
The repository's .NET 10 build and test defaults do not impose a .NET 10
requirement on consumer applications.

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
  <PackageReference Include="Wireloom.Dds.Generator"
                    Version="0.4.0"
                    PrivateAssets="all" />
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

These examples show the public C# API generated by Wireloom and compare it with
the retained RTI Connext 7.7.0 C# oracle. The comparison is about generated
source shape. Wireloom emits the C# contracts and adapters; RTI supplies the
sequence types and DDS runtime. See RTI documentation for behavior owned by its
runtime. The [effective-limits section](#effective-limits-for-unbounded-values)
describes how bounds relate to mutation and DDS processing.

| IDL declaration                                                     | Wireloom C# API and use                                                                                                           | RTI C# comparison                                                                                     | Evidence                                                          |
| :------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------- |
| `long id;`                                                          | `int id { get; set; }`<br>Assign with `sample.id = 42;`.                                                                          | Same member type and accessors.                                                                       | [RTI][rti-01] · [generated][wire-01]                              |
| `typedef sequence<long, 4> BoundedLongs;`                           | `ISequence<int> Value` with `[Bound(4)]`.<br>Use `var bounded = new BoundedLongs(); bounded.Value.Add(17);`.                      | Same generated public shape.                                                                          | [RTI][rti-05] · [generated][wire-05]                              |
| `@optional sequence<long, 4> values;`<br>`@optional long items[2];` | `ISequence<int>? values`; `int[]? items`.<br>Set `items = [1, 2]`; set `values = new Sequence<int> { 17 }` or `null` for absence. | Optional metadata matches. Wireloom adds nullable C# annotations absent from the retained RTI source. | [RTI][rti-09] · [generated][wire-09]                              |
| `typedef long Scalar;`<br>`typedef sequence<Scalar> Scalars;`       | `ISequence<int> Value`; the scalar alias is erased for this collection element.                                                   | Same public element type.                                                                             | [RTI][rti-05a] · [generated][wire-05a] · [C136][cov-c136]         |
| `union Choice switch(Kind) { case Number: long number; ... };`      | `int number`; assigning selects the branch. Reading an inactive branch throws `InvalidOperationException`.                        | Same generated branch API and guard.                                                                  | [RTI][rti-07] · [generated][wire-07] · [runtime test][union-test] |

[rti-01]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/oracles/features/01-primitives/
[wire-01]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/generator/features/01-primitives/
[rti-05]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/oracles/features/05-collections/
[wire-05]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/generator/features/05-collections/
[rti-09]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/oracles/features/09-optional-collections/
[wire-09]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/generator/features/09-optional-collections/
[rti-05a]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/oracles/features/05-unbounded-sequence-aliases/
[wire-05a]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/generator/features/05-unbounded-sequence-aliases/
[cov-c136]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/FEATURE-COVERAGE.md#positive-cases
[rti-07]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/oracles/features/07-union-enum/
[wire-07]: https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/generator/features/07-union-enum/
[union-test]: https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.RuntimeContracts/Unions/UnionSpecs.cs

This is a quick reference, not a complete mapping catalog. Consult the
[feature coverage matrix](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/FEATURE-COVERAGE.md)
for case-level status and evidence. A difference in generated C# shape does not
by itself establish a difference in DDS wire behavior. The [runtime contract
coverage matrix](https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
indexes tested copy, ownership, optional-presence, union, and native-round-trip
behavior.

## Configure includes and preprocessing

Use project properties for defaults shared by roots, or root-item metadata for
the project-wide generation batch:

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

| Setting                    | Scope                                      | Meaning                                                                                       |
| :------------------------- | :----------------------------------------- | :-------------------------------------------------------------------------------------------- |
| `DdsIdlIncludeDirectories` | Project                                    | Semicolon-separated directories searched for configured includes and tracked for invalidation |
| `IncludeDirectories`       | Root-item metadata collected for the batch | Additional include directories available to the generation batch                              |
| `Defines`                  | Root-item metadata collected for the batch | Semicolon-separated preprocessor symbols                                                      |
| `Undefines`                | Root-item metadata collected for the batch | Semicolon-separated symbols removed before preprocessing                                      |
| `Strict`                   | Root-item metadata collected for the batch | Enables strict validation for the generation batch when any root sets it                      |

The current compiler combines these settings across all generating roots. It
does not provide isolated preprocessing or validation settings per root.

Quoted relative includes resolve from the including file's directory. Angle
includes use the configured include directories. Keep a shared file as an
included input unless it is intentionally a separate generation root.

Project-level include directories are added before root-item directories.
Include directories are deduplicated by path, keeping the first occurrence.
Quoted includes search the including file's directory first, then the combined
include directories; angle includes search the combined directories only. If
two search directories contain the same include name, the first matching path
wins.

`Defines` and `Undefines` are pooled across the generation roots. All external
defines seed each root's preprocessor, then all external undefines are applied;
an external undefine therefore removes a name even if another root defines it.
Each root starts with that same external macro state, so in-file `#define` and
`#undef` changes do not carry from one root to the next. Avoid assigning
different values to the same define in separate roots; per-root values are not
isolated. Split variants into separate projects or separate builds when they
need different IDL macro or strict-validation settings.

`Strict` defaults to `false`. With strict validation off, this inherited-key
declaration is accepted; setting `Strict="true"` on any generation root makes
the whole batch reject it with a source-located generation error:

```idl
struct Base { @key long tenant; };
struct Derived : Base { @key long localId; };
```

This is the strict check currently added by Wireloom: a derived struct or
valuetype may not declare another `@key` member. Strict mode does not turn on a
general validation suite beyond that check.

Large inputs may hit deterministic preprocessing safety limits. They are not
configurable through MSBuild; see [preprocessor limits](#preprocessor-limits).

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

Wireloom applies finite deterministic limits to each IDL input. Exceeding a
limit stops generation and reports a source-located generation error. These
limits have no MSBuild override:

| Limit                        | Maximum                               |
| :--------------------------- | ------------------------------------: |
| Preprocessed output          | 4,194,304 UTF-16 code units per input |
| Macro expansion work         | 100,000 operations per input          |
| Macro expansion depth        | 64 nested expansions                  |
| Conditional expression depth | 256 nested expressions                |
| Include nesting              | 128 levels                            |
| Source-origin alignment work | 100,000 work units per expanded line  |

To reduce a failure, simplify or split large IDL inputs, trim unused macro
expansions, reduce conditional-expression and macro nesting, and simplify deep
include chains. Splitting helps only when each resulting input and its
preprocessed output stay within the per-input limits. The
[preprocessor feature matrix](https://github.com/eNeRGy164/wireloom/blob/main/docs/PREPROCESSOR-FEATURES.md) describes
which preprocessing forms are implemented or partial.

## Annotation reference

This is a reference to behaviors implemented and exercised by the current
corpus, not a claim that every OMG or RTI annotation is supported. Annotation
effects often appear in RTI DynamicType metadata rather than in the managed
C# member signature.

| Annotation or group                                                                                                                          | Current behavior and scope                                                                                                                                                                                                                                                                                                   | Evidence                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| :------------------------------------------------------------------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `@topic`                                                                                                                                     | Marks a struct as a DDS topic data type in generated documentation. It does not create a topic or publish samples.                                                                                                                                                                                                           | [`10-annotations.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-annotations.idl)                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| `@key`                                                                                                                                       | Marks a member as a DDS instance key and is reflected in type support metadata. Key identity is distinct from equality of complete C# samples.                                                                                                                                                                               | [`08-keys.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/08-keys.idl), [`10-annotations.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-annotations.idl)                                                                                                                                                                                                                                                                                                                             |
| `@optional`                                                                                                                                  | Marks optional members in type support and produces nullable C# members for supported optional shapes. An optional union branch is ignored because a union always selects one branch.                                                                                                                                        | [`09-optional-collections.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-optional-collections.idl), [`09-optional-aggregate-member.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-optional-aggregate-member.idl)                                                                                                                                                                                                                                                                 |
| `@id`, `@hashid`, `@autoid(HASH)`                                                                                                            | Set explicit or hash-derived DDS member IDs in supported aggregate contexts. Other `@autoid` mappings are not separately qualified here.                                                                                                                                                                                     | [`09-id-gaps.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-id-gaps.idl), [`09-autoid-hash.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-autoid-hash.idl)                                                                                                                                                                                                                                                                                                                       |
| `@final`, `@appendable`, `@mutable`                                                                                                          | Set the corresponding extensibility kind in generated DynamicType metadata for covered declarations.                                                                                                                                                                                                                         | [`09-extensibility.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-extensibility.idl)                                                                                                                                                                                                                                                                                                                                                                                                                                |
| `@default`, `@min`, `@max`, `@range`, `@unit`                                                                                                | Supported on primitive/enum members as applicable. Defaults initialize supported values; ranges and units are represented in generated type support annotations. Ranges require primitive members; defaults also support enums.                                                                                              | [`09-defaults-ranges.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-defaults-ranges.idl), [`10-annotations-rti.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-annotations-rti.idl)                                                                                                                                                                                                                                                                                               |
| `@value`, `@default_literal`                                                                                                                 | Set explicit enum numeric values and the default enum literal for covered enum shapes.                                                                                                                                                                                                                                       | [`03-enum-values-prefix.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/03-enum-values-prefix.idl), [`09-defaults-ranges.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-defaults-ranges.idl)                                                                                                                                                                                                                                                                                         |
| `@must_understand`                                                                                                                           | Preserved as a member property in generated DynamicType metadata for the covered RTI annotation case.                                                                                                                                                                                                                        | [`09-autoid-hash.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-autoid-hash.idl)                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| `@external`                                                                                                                                  | Accepted in the tested annotation case but does not create a distinct managed C# member shape. Check runtime/wire evidence before relying on any external-member serialization effect.                                                                                                                                       | [`10-annotations-rti.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-annotations-rti.idl)                                                                                                                                                                                                                                                                                                                                                                                                                            |
| `@language_binding`, `@transfer_mode`, `@data_representation`, `@allowed_data_representation`, `@nested`, `@default_nested`, `@resolve_name` | Recognized/consumed syntax, but not all have an effect in emitted C# or generated DynamicType. The corpus comparisons establish only the listed case outcomes; do not rely on these annotations to configure wire representation or naming without separate runtime/wire evidence. `FLAT_DATA` is ignored for C# generation. | [`10-annotations-rti.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-annotations-rti.idl), [`09-data-representation.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-data-representation.idl), [`09-allowed-data-representation.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/09-allowed-data-representation.idl), [`10-flat-data-binding.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/features/10-flat-data-binding.idl) |
| Unknown annotation                                                                                                                           | Emits warning `DDSG0101` and continues while ignoring the annotation. Verify that ignoring it does not change the contract you need.                                                                                                                                                                                         | [`12-unknown-annotation.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/negative/12-unknown-annotation.idl)                                                                                                                                                                                                                                                                                                                                                                                                                      |
| `@position`, `@bit_bound`; invalid `@service` target                                                                                         | Recognized but unsupported uses emit `DDSG0102` and reject generation. A `@service("DDS")` interface is instead ignored with `DDSG0103`; interfaces do not become C# types.                                                                                                                                                  | [`10-unsupported-annotations.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/negative/10-unsupported-annotations.idl), [`10-service-annotation.idl`](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/idl/negative/10-service-annotation.idl)                                                                                                                                                                                                                                                                         |

An annotation accepted by RTI is not automatically implemented by Wireloom.
For a declaration not listed here, check the exact corpus case and diagnostic;
use an unknown-annotation warning as a prompt to verify the generated
contract, not as evidence that the annotation is harmless.

## Diagnostics

| ID         | Meaning                                                              | Typical action                                                                                             |
| :--------- | :------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------- |
| `DDSG0001` | IDL generation failed                                                | Fix the source-located parse, include, semantic, or unsupported-feature error                              |
| `DDSG0002` | C# 12 or later is required                                           | Set the consumer's language version to C# 12 or later                                                      |
| `DDSG0003` | A compatible RTI runtime was not found                               | Add an explicit `Rti.ConnextDds` reference at the tested compatible version                                |
| `DDSG0101` | An unknown IDL annotation was ignored                                | Verify the annotation is supported by the managed generator if it affects the generated contract           |
| `DDSG0102` | A recognized but unsupported annotation use was reported             | Remove the annotation or use a supported declaration context; this warning accompanies rejected generation |
| `DDSG0103` | An IDL interface was ignored because no C# interface type is emitted | Interfaces are accepted as service declarations, but are not emitted as C# types                           |
| `DDSG0104` | A function-like macro was invoked with the wrong number of arguments | Correct the macro invocation; RTI-compatible expansion continues after the warning                         |
| `DDSG0105` | A direct array of sequences may not preserve IDL semantics           | Use a typedef for the sequence before declaring the array                                                  |
| `DDSG0106` | An active `#warning` directive was encountered                       | Review the warning in the IDL or included file; preprocessing and generation continue                      |
| `DDSG0107` | An active `#pragma message` directive was encountered                | Review the informational message in the IDL or included file; generation continues                         |

Diagnostics are owned by the generator and use the original IDL/include
location where one is available. Review the table's typical actions before
adoption; ignored annotations or interfaces and mappings flagged as potentially
lossy can affect the generated contract. Configure an individual diagnostic's
severity through the consumer's `.editorconfig`, for example:

```ini
[*.idl]
dotnet_diagnostic.DDSG0101.severity = error
```

The default severities are grouped by diagnostic family: `DDSG0001`–`DDSG0003`
are errors, `DDSG0101`–`DDSG0106` are warnings, and `DDSG0107` is informational.
Warnings do not all mean generation continues: `DDSG0102` reports an unsupported
annotation use and is followed by a generation error. `DDSG0101`, `DDSG0103`,
`DDSG0104`, `DDSG0105`, and `DDSG0106` report cases where generation continues.
The diagnostics table describes the action for each one.

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

## Supported boundary

The current corpus exercises modules, structs, nested declarations, aliases,
enums, unions, constants, primitive values, narrow and wide strings, bounded
strings, sequences, arrays, include graphs, conditional preprocessing, keys,
annotations, and extensibility. Several of these are supported only for the
specific shapes covered by the retained RTI oracle evidence.

Before adopting a feature, consult the [diagnostics table](#diagnostics),
[feature coverage matrix](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/FEATURE-COVERAGE.md),
and [corpus rules](https://github.com/eNeRGy164/wireloom/blob/main/docs/corpus/README.md).

### Effective limits for unbounded values

Wireloom currently assigns effective limits to unbounded IDL values: **255
UTF-8 bytes** for narrow strings, **255 UTF-16 code units** for wide strings,
and **100 elements** for sequences. For example, 128 copies of `é` use 256
UTF-8 bytes, so they exceed the narrow-string limit even though the C# string
contains only 128 characters.

Generated string properties do not enforce their bounds when assigned, and
sequence members expose mutable containers whose `Add` operations do not cap
the element count. RTI uses the bound in type metadata when processing DDS data.
The [runtime contract suite](https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
verifies native round trips at the effective limits. It does not establish
exactly where or how an over-limit value fails, so consumers should validate
values before DDS processing rather than rely on assignment-time checks.

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
- It does not silently omit unsupported declarations; accepted exceptions that
  ignore annotations or interfaces are reported with diagnostics.

## Troubleshooting

When generation appears not to run, start with a clean command-line build and
follow the symptom table. IDE behavior can depend on its Roslyn host; current
package integration evidence covers command-line builds, not every IDE/SDK
combination.

| Symptom                                               | Check and next step                                                                                                                                                                                                                                                           |
| :---------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Build succeeds but no generated types appear          | Confirm the project references `Wireloom.Dds.Generator` and declares the intended roots as `DdsIdl` items. Other `.idl` files are tracked includes, not roots.                                                                                                                |
| Generator package is present but has no effect        | Restore the project, inspect the resolved package version and analyzer/build assets, then run `dotnet build` from the project directory. If the build reports an analyzer load or Roslyn assembly error, verify the SDK/compiler host; older SDK hosts are not all qualified. |
| `DDSG0002`                                            | Set `<LangVersion>12.0</LangVersion>` or later. This diagnostic is reported when the generator loads and the compiler language version is too old.                                                                                                                            |
| `DDSG0003`                                            | Add an explicit `Rti.ConnextDds` reference at a version accepted by the guard; the generator package does not add the runtime transitively.                                                                                                                                   |
| `DDSG0001`                                            | Read the diagnostic text and source location. Check IDL syntax, include resolution, semantic rules, and feature coverage. This is also the generation-failure diagnostic for preprocessing limits.                                                                            |
| Generated documents are absent in the IDE             | First confirm the command-line build succeeds. Roslyn generated documents appear in the IDE; to inspect files on disk, enable `EmitCompilerGeneratedFiles` and set `CompilerGeneratedFilesOutputPath` as described in [Inspect generated source](#inspect-generated-source).  |
| Build works but the IDE shows stale generated code    | Rebuild and reload the project, then check the IDE's selected SDK and analyzer diagnostics. Do not infer an IDE-host compatibility result from a command-line build.                                                                                                          |
| Compiler reports duplicate generated type definitions | Remove exported generated `.g.cs` files from explicit `<Compile>` items. The generator already adds its documents to compilation; do not compile the same emitted source twice.                                                                                               |

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
- [Corpus compliance tests](https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.CorpusCompliance/README.md)
- [Package integration test](https://github.com/eNeRGy164/wireloom/blob/main/tests/Wireloom.Dds.Generator.PackageIntegration/README.md)
- [Generated value semantics](https://github.com/eNeRGy164/wireloom/blob/main/docs/guides/generated-value-semantics.md)

RTI-generated reference material in the repository is retained under the
applicable RTI license and review rules. It is not redistributed by this NuGet
package.
