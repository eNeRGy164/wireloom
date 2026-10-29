# Work with generated contracts

Use this guide after the [package quick start](../../src/Wireloom.Dds.Generator/README.md#quick-start).
IDL and handwritten C# are maintained inputs. Wireloom supplies generated
documents to the compiler during the normal build.

## Establish the command-line baseline

From the consumer project directory, run:

```shell
dotnet --version
dotnet restore
dotnet build
```

Resolve generator errors before investigating the editor. Consumers need
.NET 8 or later, C# 12 or later, and a compatible explicit RTI runtime reference.
The repository's pinned producer SDK is a separate requirement for contributing
to Wireloom. See [configuration](configuration.md#select-the-consumer-framework-language-and-rti-runtime).

## Use VS Code or Visual Studio 2026

For VS Code, use Microsoft's C# extension; C# Dev Kit adds solution/project
management on top of its language service. Open the folder containing the
consumer project or solution and wait for project loading and restore.
Microsoft documents [C# setup](https://code.visualstudio.com/docs/languages/csharp)
and [navigation](https://code.visualstudio.com/docs/csharp/navigate-edit).

For Visual Studio 2026, open the consumer project or solution and allow restore
and project loading to finish. Use the normal build and C# navigation tools;
Wireloom needs no external-generator command or IDE-specific generation script.
Microsoft describes [Go To Definition and Peek Definition](https://learn.microsoft.com/en-us/visualstudio/ide/go-to-and-peek-definition).

In either editor, use the same small check:

1. Build the project and reference a generated type from handwritten C#.
2. Use **Go To Definition** on that type to inspect its generated declaration.
   Hover over a member to inspect its XML documentation.
3. Add an IDL member, save, and build. Check that C# use sites and generated
   documents reflect the change.
4. Introduce an invalid bound such as `long values[0];`. Build, select the
   `DDSG0001` diagnostic, and check the original IDL location. Correct it and rebuild.

This is the intended interactive workflow, not a completed qualification claim
for every IDE/extension version. Record the editor, C# extensions, selected SDK,
OS, RTI version, and operations when reporting a failure. Interactive qualification
remains open under [R04](../architecture/arc42/11-risks-and-technical-debt.md#r04).
The Playground's lexical editor does not provide the consumer IDE's semantic C# service.

## Extend a generated type

Keep application helpers in a separate partial declaration. For this IDL:

```idl
module Telemetry {
    struct Sample {
        long id;
        string<64> label;
    };
};
```

Add a handwritten `Sample.Extensions.cs` to the same project:

```csharp
namespace Telemetry;

/// <summary>Adds application display helpers to the generated contract.</summary>
public partial class Sample
{
    /// <summary>Gets a label for application displays.</summary>
    public string DisplayLabel => $"{id}: {label}";
}
```

Application code can use `new Telemetry.Sample { id = 42, label = "sensor" }.DisplayLabel`.
Regenerating the IDL does not replace this handwritten file. Use the generated
type's namespace and accessibility, avoid names already emitted by the generator,
and do not add a second definition of an existing generated constructor or method.
A helper does not add an IDL member or change DDS type metadata. Change the IDL
when the shared data contract must change.

## Inspect files without adding source-control noise

Generated documents are available through Roslyn. Physical exports are optional:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

Keep exports under `obj`, out of source control and explicit `Compile` items.
The generator already adds its documents to compilation. Exporting into a normal
source folder can cause a later build to compile the same definitions twice.

Commit IDL, package/configuration changes, and handwritten partial extensions.
Inspect generated changes when validating an upgrade, without making checked-in
generated files part of the ordinary build.

## Review consumer analysis

Run the application's normal compiler and quality tools after generation and
regeneration. Check that maintained code is still analyzed and that generated
documents retain their generated-code classification. Keep compiler errors and
Wireloom diagnostics visible.

If Qodana, SonarQube, or another tool reports generated-code noise, record the
tool/version, rule IDs, generated paths, whether physical exports were enabled,
and a minimal consumer reproducer. Check first for accidentally compiled exports.
Do not disable application analysis or apply the repository's independent-scan
exclusions as a universal consumer policy.

The [generated-C# report](../corpus/csharp-quality.md) deliberately removes
generated classification to inspect intrinsic output quality. It has a different
purpose from normal consumer analysis. Named consumer Qodana/SonarQube
qualification remains open under [R05](../architecture/arc42/11-risks-and-technical-debt.md#r05).

## Recover an editor mismatch

If the command-line build succeeds but the IDE lacks or retains old generated
symbols, check that the project loaded, compare its selected SDK/compiler host,
inspect analyzer-load diagnostics, then rebuild or reload the project. Use
physical exports under `obj` to inspect build output independently of the IDE.

For a repeatable mismatch, follow [reporting a problem](reporting-issues.md).
Include both command-line and editor results; a passing build alone does not
demonstrate interactive behavior.

[Documentation](../README.md) · [Value semantics](generated-value-semantics.md)
