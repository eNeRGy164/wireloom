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

After editing IDL, build the project to refresh its generated contracts. In
VS Code, use **Ctrl+Shift+B**. IntelliSense may show the previous generated
types until you build; if it stays stale, use **CSharp: Rerun Source Generators**
from the Command Palette. Automatic refresh while editing IDL is optional.

In either editor, use the same small check:

1. Build the project and reference a generated type from handwritten C#.
2. Use **Go To Definition** on that type to inspect its generated declaration.
   Hover over a member to inspect its XML documentation.
3. Add an IDL member, save, and build. Check that C# use sites and generated
   documents reflect the change.
4. Introduce an invalid bound such as `long values[0];`. Build, select the
   `DDSG0001` diagnostic, and check the original IDL location. Correct it and rebuild.

The Playground's lexical editor does not provide the consumer IDE's semantic C# service.

Interactive qualification covers VS Code with Microsoft's C# extension and
C# Dev Kit, and Visual Studio 2026 on Windows x64. Each editor passes with
RTI 7.3.1 targeting `net8.0` and RTI 7.7.0 targeting `net10.0`, using C# 12
and SDK 10.0.403. The checked workflow includes generated-definition navigation,
member documentation, root and included-file changes, IDL diagnostic navigation
and correction, and handwritten partial extensions. VS Code's explicit generator
refresh is part of the workflow when its language service retains old output.
Other host configurations need their own checks.

## Route VS Code build diagnostics

Configure a build task in the consumer's `.vscode/tasks.json` so compiler output
reaches Problems with source locations:

```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "Build contracts",
      "type": "process",
      "command": "dotnet",
      "args": ["build", "HelloWorld.csproj", "--configuration", "Debug"],
      "options": { "cwd": "${workspaceFolder}" },
      "problemMatcher": "$msCompile",
      "group": { "kind": "build", "isDefault": true }
    }
  ]
}
```

Replace `HelloWorld.csproj` with the consumer project or solution and merge this
task into existing configuration. Run it with **Ctrl+Shift+B**. Selecting a
`DDSG0001` Problems entry opens the original IDL location.
Microsoft documents [task problem matchers](https://code.visualstudio.com/docs/debugtest/tasks#processing-task-output-with-problem-matchers).

Use the IDE build task after IDL changes. An external `dotnet build` compiles the
updated contracts but may leave VS Code's IntelliSense stale.

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

Under the [1.x compatibility contract](support-and-compatibility.md#public-contract),
generated API evolution follows the selected RTI profile. Wireloom does not
independently add generated members that could collide with partial extensions.
Review extensions when changing IDL or RTI profiles and when adopting a documented
defect correction.

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
purpose from normal consumer analysis. The
[consumer analysis procedure](../testing/consumer-analysis.md) identifies the
qualified Qodana and SonarQube versions/profiles, positive controls, and clean
regeneration results for both RTI profiles. Other rule sets and tool versions
remain subject to [R05](../architecture/arc42/11-risks-and-technical-debt.md#r05).

## Recover an editor mismatch

If the command-line build succeeds but the IDE lacks or retains old generated
symbols, check that the project loaded, compare its selected SDK/compiler host,
and inspect analyzer-load diagnostics. In VS Code, open a handwritten C# call
site and run **CSharp: Rerun Source Generators** from the Command Palette after
the build. If it remains stale, reload the window and recheck the call site.
Use physical exports under `obj` to inspect build output independently of the IDE.

For a repeatable mismatch, follow [reporting a problem](reporting-issues.md).
Include both command-line and editor results; a passing build alone does not
demonstrate interactive behavior.

[Documentation](../README.md) · [Value semantics](generated-value-semantics.md)
