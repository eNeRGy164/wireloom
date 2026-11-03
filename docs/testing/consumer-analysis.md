# Consumer static analysis

Use this procedure to qualify normal application analysis with a packed
Wireloom analyzer. The separate [emitted-C# scan](../corpus/csharp-quality.md)
removes generated classification to inspect intrinsic output quality.

## Qualified scope

Fresh packed consumers pass with RTI 7.3.1 and 7.7.0, each targeting `net8.0`
and `net10.0` with C# 12. Windows x64 builds use SDK 10.0.403; Qodana analyzes
fresh Linux staging roots under its SDK 10.0.400 host.

| Tool                                                                                        | Configuration                         | Result after regeneration               |
| :------------------------------------------------------------------------------------------ | :------------------------------------ | :-------------------------------------- |
| Community Qodana 2026.2.691 / InspectCode 2026.2.1                                          | `qodana.recommended`                  | Zero findings across all four consumers |
| SonarQube Community Build 26.10.0.133078 / Scanner for .NET 12.0.0 / C# plugin 10.35.0.4138 | C# `Sonar way core`, 121 active rules | Zero issues across all four consumers   |

The controls are reported in both ordinary handwritten files and handwritten
partial extensions: Qodana reports eight unused-local warnings, and SonarQube
reports eight `S1764` issues. Additional compiler findings on the controls are
expected. After their removal, root-member and included-bound changes regenerate
correctly; all four consumers build without warnings and run successfully.

This qualifies the named profiles and fixture behavior. Custom rules, future
tool versions, and other consumer configurations require their own checks.
Use the [release record](../releases/README.md) for a published package's scope.

## Configuration

Use Community Qodana's `qodana.recommended` profile and SonarQube Community
Build's C# `Sonar way core` profile. Keep SonarQube's
`sonar.cs.analyzeGeneratedCode=false` default. Do not disable maintained-code
analysis, suppress compiler errors, or copy the independent scan's inspection
exclusions into a consumer configuration.

Wireloom emits generated headers and `.g.cs` document names. Physical exports
are optional; when enabled, keep them under `obj` and out of explicit `Compile`
items. Compiling exported documents again creates duplicate definitions.

## Qualification procedure

1. Pack the current generator with a unique local version using the
   [isolated-feed procedure](../../tests/Wireloom.Dds.Generator.PackageIntegration/README.md).
   Record the source/input hashes and package hash so an older package cannot
   stand in for the current source.
2. Copy the existing [Hello World consumer](../../samples/HelloWorld/README.md)
   into an ignored qualification directory. Use central package versions and
   the repository's pinned SDK. Test RTI 7.3.1 and 7.7.0 with `net8.0` and
   `net10.0`, using C# 12. Do not reference the generator project directly.
3. Include a root IDL file, an included file that supplies a string bound,
   bounded collections, an optional unbounded string, and a handwritten partial
   extension used by the application. Enable unbounded support. Build and run
   each consumer before analysis.
4. Add temporary findings to an ordinary handwritten C# file and a handwritten
   partial extension. An unused local is detected by Qodana; subtraction of an
   expression from itself is detected by Sonar's `S1764` rule. For example:

   ```csharp
   public static int AnalyzeControl(int input)
   {
       int unused = 42;
       return input - input;
   }
   ```

5. Run Qodana against the consumer solution and run SonarScanner for .NET's
   begin/build/end sequence. Use a nonincremental build and wait for successful
   SonarQube server processing. Confirm the findings identify maintained files,
   including the partial extension, while generated documents produce no
   findings. Compiler diagnostics imported by SonarQube alone do not establish
   that its own C# rules ran; require the `S1764` control too.
6. Remove the controls. Add a member to the root IDL and change the included
   bound. Build, confirm the new member compiles at a handwritten use site, and
   inspect the updated bound in emitted C#. Run the consumer again.
7. Repeat both analyses. Require zero remaining findings and successful builds
   for every cell. Preserve the tools' normal generated-code recognition and
   verify any needed configuration separately from intrinsic source quality.

For Docker Qodana, keep `global.json` visible and use isolated Linux `obj`/`bin`
directories or a fresh source staging root. Do not reuse Windows intermediates.
Only one Qodana scan should run at a time. The
[maintainer quality procedure](quality.md#interpreting-local-docker-runs)
explains report freshness and progress checks.

For SonarQube, follow the vendor's
[.NET scanner procedure](https://docs.sonarsource.com/sonarqube-community-build/analyzing-source-code/scanners/dotnet/using).
Use a dedicated local qualification server or an authorized project. If staging
under a Git-ignored directory, disable SCM exclusions for that isolated scan;
otherwise the tool may omit the fixture. Keep authentication tokens out of
retained logs and revoke temporary tokens after analysis.

Retain package/source hashes, project and tool configuration, baseline and
regenerated inputs, build/run logs, raw Qodana SARIF, SonarQube task status and
issue/file responses, and tool/profile versions under `artifacts/` or CI
artifacts. Report controls separately from unexpected findings. Repeat the
qualification when the generator, packaging, or relevant tool configuration
changes.
