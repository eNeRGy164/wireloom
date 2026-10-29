# Wireloom

<!-- markdownlint-disable-next-line MD033 -->
<img src="docs/assets/wireloom.svg" alt="Wireloom logo" width="360">

[![Quality](https://github.com/eNeRGy164/wireloom/actions/workflows/quality.yml/badge.svg)](https://github.com/eNeRGy164/wireloom/actions/workflows/quality.yml)
[![Coverage Status](https://coveralls.io/repos/github/eNeRGy164/wireloom/badge.svg?branch=main)](https://coveralls.io/github/eNeRGy164/wireloom?branch=main)
[![NuGet Version](https://img.shields.io/nuget/v/Wireloom.Dds.Generator)](https://www.nuget.org/packages/Wireloom.Dds.Generator)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/eNeRGy164/wireloom/badge)](https://scorecard.dev/viewer/?uri=github.com/eNeRGy164/wireloom)

**Replace `rtiddsgen` in your C# build. Keep RTI Connext DDS.**

Wireloom brings IDL generation into the .NET compiler for **.NET 8 and higher**.
It generates your C# data types and RTI type-specific support from IDL during a normal build, with
rich IntelliSense documentation, modern C#, and a small NuGet package. For
supported IDL contracts, it replaces the complete C# generation step:
preprocessing, parsing, validation, and emission.

The RTI Connext DDS runtime still provides serialization and DDS communication.
Your application continues to use RTI's runtime, APIs, and configuration.

Try the [browser Playground](https://energy164.github.io/wireloom/) to preview
generated C# from a local multi-file IDL workspace. Compilation runs in your browser. See the
[source and limitations](playground/README.md) for its scope; previews do not
establish RTI compilation, runtime behavior, or DDS wire compatibility.

## Get started

Add the generator and RTI runtime to the project that owns your DDS contracts.
The runtime reference is explicit because your application controls its runtime
version and configuration.

For this first example, create a console application:

```powershell
dotnet new console --name WireloomQuickstart --framework net10.0
cd WireloomQuickstart
```

Replace `WireloomQuickstart.csproj` with:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
    <PackageReference Include="Wireloom.Dds.Generator" Version="0.5.0" PrivateAssets="all" />
  </ItemGroup>

  <ItemGroup>
    <DdsIdl Include="Contracts\Telemetry.idl" />
  </ItemGroup>
</Project>
```

Create a `Contracts` directory and save this as `Contracts/Telemetry.idl`:

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

Replace `Program.cs` with code that uses the generated API:

```csharp
var sample = new Telemetry.Sample
{
    id = 42,
    label = "temperature"
};

sample.values.Add(20.5F);
using var serializer = Telemetry.SampleSupport.Instance.CreateSerializer();
```

```powershell
dotnet build
```

A successful build confirms that the generator ran, emitted the expected
`Telemetry.Sample` and `Telemetry.SampleSupport` APIs, and that the compiler
accepted them against the referenced RTI runtime.

Continue with the [Hello World publisher/subscriber tutorial](docs/guides/hello-world.md)
to run two applications and exchange samples. It follows RTI's familiar C#
getting-started flow, with generation supplied by the Wireloom package. For a
ready-to-build repository project, see the [checked-in publisher/subscriber
sample](samples/HelloWorld/README.md).

## Why Wireloom?

- **Keep build images and developer machines lean.** Restore a small NuGet
  generator package and use the .NET build to create C# from IDL. Generation
  needs no separate RTI generator installation, Java toolchain, or external
  preprocessor.
- **Use generated contracts naturally in the editor.** Generated XML
  documentation explains DDS keys, optional presence, string and collection
  bounds, defaults, member IDs, and union branch selection. Remarks, examples,
  exception documentation, and RTI API links put the contract beside the code
  you are writing.
- **Keep generated C# usable under strict checks.** The generated C# in the
  original build added a static-analysis burden alongside the external tooling
  cost. The [quality evidence](#quality-evidence) records what has been checked
  and links to the Qodana method and exclusions; static findings are not an
  interoperability measure.
- **Make generation setup repeatable.** One `PackageReference` and one
  `DdsIdl` item connect IDL to the build. Includes and defines are declarative
  MSBuild settings carried with the project. A clean restore and build use that
  configuration without custom `Exec` targets, generator installation paths,
  or generation-tool environment variables.
- **Work directly from IDL.** Roslyn exposes generated documents in the IDE;
  diagnostics point back to the IDL or included file. Root and include changes
  are tracked as compiler inputs, so regeneration belongs to the normal build.
- **Extend your types without editing generated files.** Generated data classes
  are `partial`, so application helpers can live in separate, maintained C#
  files and survive regeneration.

Wireloom fits C# projects whose IDL shapes are covered and whose RTI runtime
version is qualified, especially when build and IDE integration matters and
best-effort support is acceptable. Keep RTI generation for shapes or options
outside that coverage, or when your requirements call for a generation tool
covered by your RTI support agreement. See the
[support and compatibility policy](docs/guides/support-and-compatibility.md).

RTI's generator serves multiple languages, platforms, and release toolchains.
Wireloom focuses on the C# developer experience within that ecosystem. The
benefit is a smaller generation setup and C# output that fits naturally into
today's .NET tooling.

### See the DDS contract in IntelliSense

Hover over a generated type to see where it was declared and how its
extensibility affects compatible type evolution:

![IntelliSense for Context identifies its IDL source and explains that new members can be appended while preserving member order.](docs/assets/intellisense-type.png)

Hover over a member to see the details that matter when assigning values.
Here, `producer` is optional, its bound is **16 UTF-8 bytes**, and `null` means
the member is absent:

![IntelliSense for the nullable producer property explains optional presence, the 16 UTF-8 byte string bound, and the meaning of null.](docs/assets/intellisense-optional-member.png)

### Less tooling to install

Generate C# from IDL using the SDK already building your application and a
small generator package. The latest release, Wireloom 0.5.0, measures
**402,534 bytes (about 403 kB)** as the published NuGet package. The RTI runtime remains a
NuGet dependency in either workflow; the package measurement covers generation
tooling.

The footprint matters in practice: provisioning RTI generation tooling added
about **2 GB** to the C# Docker build images in the pipeline that motivated
Wireloom. That is an observation from the original setup, whose selected tools
determined its size. See the [project background](docs/architecture/arc42/01-introduction-and-goals.md)
for the other requirements that shaped Wireloom, and the
[package-size evidence](docs/releases/0.5.0.md#package-and-source) for the local
package measurement.

| Generation concern | External `rtiddsgen` workflow                                                                | Wireloom workflow                                          |
| :----------------- | :------------------------------------------------------------------------------------------- | :--------------------------------------------------------- |
| Tool provisioning  | Install or provide the RTI code generator and its Java tooling                               | Restore one small NuGet generator package                  |
| IDL preprocessing  | Configure the external C/C++ preprocessor when needed                                        | Managed preprocessing inside the generator                 |
| Build integration  | Invoke the generator and manage tool paths, arguments, outputs, and generated compile inputs | Declare the package and IDL roots in the project           |
| Generated files    | Generate physical C# files and arrange their inclusion in the build                          | Roslyn generated documents; optional export for inspection |
| Error navigation   | Surface external-tool output through your build integration                                  | Source-located compiler diagnostics for IDL and includes   |

RTI's [C# getting-started guide](https://community.rti.com/static/documentation/connext-dds/current/doc/manuals/connext_dds_professional/getting_started_guide/csharp/intro_pubsub_csharp.html)
describes generator invocation, path setup, and external preprocessing. Existing
projects may already automate these steps; Wireloom makes them part of the
package's compiler integration.

## Check project fit

- **Build baseline:** Consumer projects need .NET 8 or later, C# 12 or later,
  and an explicit `Rti.ConnextDds` reference at version 7.3.1 or later. See
  [consumer requirements](#support-and-compatibility) and the
  [generator diagnostics](src/Wireloom.Dds.Generator/README.md#diagnostics).
- **Qualification:** Only exact RTI 7.3.1 and 7.7.0 releases have qualification
  evidence for documented cases; mapping other versions does not qualify them.
  See the [RTI version policy](docs/guides/support-and-compatibility.md#rti-version-qualification).
- **IDL and options:** Coverage is evidence-backed, not universal. RTI's
  dynamic `-unboundedSupport` behavior is unsupported; defaults are 100 sequence
  elements, 255 UTF-8 bytes for narrow strings, and 255 UTF-16 code units for
  wide strings. Direct arrays of sequences flatten by default. Check the
  [feature coverage](docs/corpus/FEATURE-COVERAGE.md),
  [`rtiddsgen` option map](docs/migration/rtiddsgen-to-wireloom.md#rtiddsgen-option-map),
  [unbounded limits](src/Wireloom.Dds.Generator/README.md#effective-limits-for-unbounded-values),
  and [sequence-array mapping](docs/guides/configuration.md#choose-the-sequence-array-mapping).
- **Runtime and license:** Generated sources do not provide the RTI runtime or
  native assets; applications still need those and the applicable RTI license.
  See the [support boundary](docs/guides/support-and-compatibility.md#support-boundary)
  and [Hello World prerequisites](docs/guides/hello-world.md#before-you-start).
- **Support and stability:** Support is best effort, without response-time or
  resolution commitments. Breaking changes may occur in 0.x; the post-1.0
  policy is undecided. See the
  [support commitment](docs/guides/support-and-compatibility.md#support-boundary)
  and [stability policy](docs/guides/support-and-compatibility.md#compatibility-and-stability).
- **Migration:** Inventory IDL and options, compare APIs, check runtime and
  wire behavior, verify deployment, and retain a rollback baseline. Follow the
  [migration procedure](docs/migration/rtiddsgen-to-wireloom.md#migration-procedure).

## Quality evidence

Use the [record for the exact package version](docs/releases/README.md) before
making an adoption claim. Source checks on a later commit do not retroactively
qualify a release. Each evidence layer answers a different question:

| Evidence                                                                           | What it establishes                                                                                      |
| :--------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------- |
| [Package integration](tests/Wireloom.Dds.Generator.PackageIntegration/README.md)   | The packed analyzer loads and generates usable C# in declared consumers, including the minimum SDK host. |
| [Feature coverage](docs/corpus/FEATURE-COVERAGE.md)                                | Accepted/rejected IDL cases and structural comparison with exact-profile RTI oracles.                    |
| [Runtime contracts](tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)       | Generated APIs, values, type support, and native lifecycle for tested shapes.                            |
| [Directed DDS exchanges](tests/Wireloom.Dds.Generator.WireCompatibility/README.md) | Actual sample exchange for named fixtures, exact RTI releases, peer directions, and adaptations.         |
| [Generated-C# analysis](docs/corpus/csharp-quality.md)                             | A dated independent static scan with explicit exclusions and configuration differences.                  |

The [0.5.0 release record](docs/releases/0.5.0.md) retains passing and failing
wire outcomes and links the original archived reports. A successful source build,
runtime test, browser preview, or complete report does not establish every
interoperability outcome.

## Follow an IDL diagnostic into generated C\#

Diagnostics identify the originating IDL or included file. For example, an
unsupported `@position` use reports `DDSG0102` and a contextual `DDSG0001`
because generation cannot continue. Correct the declaration and rebuild; then
navigate from a handwritten C# use site to its generated type.

See the [diagnostic catalog](src/Wireloom.Dds.Generator/README.md#diagnostics)
and [developer workflow](docs/guides/developer-workflow.md) for editor checks,
optional source inspection, handwritten partial extensions, and analysis.
Generated files remain compiler documents; physical exports under `obj` are
optional and do not need to enter source control.

## Additional advantages over RTI C\#

These less common cases are additional Wireloom advantages over the retained
RTI C# output. They follow the quick start so readers see the everyday
workflow first; each claim links to its evidence and states its limits.

### Wide-character union support

Wireloom accepts the `wchar` discriminator case that the retained RTI
generator rejects before C# emission
([C113](docs/corpus/idl/features/07-union-wchar-label.idl)). This is a
generation extension; consult the separate wire evidence before adopting it.

### Recursive-array support on first use

Wireloom initializes self-referencing optional fixed-array support through
ordinary `Support.Instance` access and type-support discovery, without application
bootstrapping. With RTI 7.7.0, the retained rtiddsgen 4.7.0 C# output instead
requires plugin initialization first. Fresh-process tests check startup and
serialization of absent, present, and nested arrays; see the
[reproducible comparison](tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#recursive-support-startup).
Public recursive DynamicType and default recursive TypeObject discovery remain
RTI limitations.

### Compilable generated C\#

The retained corpus also includes cases where RTI accepts an IDL input but its
generated C# does not compile, while Wireloom generates compilable C# for the
same data-type cases. The [catalog](docs/corpus/RTI-CODEGEN-DEFECTS.md)
lists findings by RTI release and case, with evidence and scope limits.

## Support and compatibility

Support is feature-specific. The [coverage index](docs/corpus/FEATURE-COVERAGE.md)
and [support policy](docs/guides/support-and-compatibility.md) identify exact RTI
qualification, known gaps, and stability expectations. The consumer runtime
reference selects the API profile; mapped versions are not automatically
qualified.

Consumer applications need .NET 8 or later and C# 12 or later. The analyzer
targets `netstandard2.0`; repository development uses .NET 10. RTI supplies the
runtime, native assets, licensing, and DDS configuration.

For an existing application, follow the [migration procedure](docs/migration/rtiddsgen-to-wireloom.md)
to compare API usage, runtime behavior, peer exchanges, and deployment against
your baseline. Direct arrays of sequences flatten by default; general RTI-style
unbounded support remains a documented capability gap.

## Testing and quality

[Quality CI](.github/workflows/quality.yml) checks the maintained compiler and
adapter, corpus profiles, and declared consumer/browser scopes. Bounded
[fuzzing](docs/testing/fuzzing.md) supplements those examples. Licensed wire
qualification and advisory mutation testing have separate workflows.
Maintainers start with [Contributing](CONTRIBUTING.md#choose-validation).

## Documentation

The [documentation hub](docs/README.md) groups tutorials, consumer references,
maintainer runbooks, and exact-version evidence by task.

| Task                                    | Read                                                                                                       |
| :-------------------------------------- | :--------------------------------------------------------------------------------------------------------- |
| Build your first DDS application        | [Hello World](docs/guides/hello-world.md)                                                                  |
| Adopt or upgrade generation             | [Migration](docs/migration/rtiddsgen-to-wireloom.md) · [support](docs/guides/support-and-compatibility.md) |
| Configure, inspect, or extend contracts | [Configuration](docs/guides/configuration.md) · [developer workflow](docs/guides/developer-workflow.md)    |
| Check compatibility or release evidence | [Feature coverage](docs/corpus/FEATURE-COVERAGE.md) · [release records](docs/releases/README.md)           |
| Understand or maintain Wireloom         | [Architecture](docs/architecture/arc42/index.md) · [Contributing](CONTRIBUTING.md)                         |

## Report a problem

If generation, generated APIs, IDE integration, or analysis does not behave as
expected, [report a minimal reproducer](docs/guides/reporting-issues.md) using the
[compatibility template](https://github.com/eNeRGy164/wireloom/issues/new?template=compatibility.md).
Known qualification limits remain visible in the support guide and feature index.
Security issues use the [private reporting policy](SECURITY.md).

## License

Wireloom is licensed under the [MIT License](LICENSE). That license does not
apply to the RTI-generated oracle sources under
[`docs/corpus/<version>/rtiddsgen`](docs/corpus/README.md) or other third-party
materials. Review the [corpus guidance](docs/corpus/README.md) before
redistributing or changing those reference artifacts.
