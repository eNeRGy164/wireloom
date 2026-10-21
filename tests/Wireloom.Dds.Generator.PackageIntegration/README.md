# Wireloom package integration tests

This project consumes the packed `Wireloom.Dds.Generator` NuGet package. It
does not reference the generator project, so compilation proves that the
package carries the analyzer and its build targets correctly.

Run it locally by packing the generator into the isolated test feed, then
restoring and testing this project:

```powershell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet pack src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj `
  --no-restore --configuration Release --output package-test-feed
$package = Get-ChildItem package-test-feed -Filter 'Wireloom.Dds.Generator.*.nupkg' |
  Sort-Object LastWriteTime -Descending | Select-Object -First 1
$packageVersion = $package.BaseName -replace '^Wireloom\.Dds\.Generator\.', ''
dotnet restore tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --configfile tests/Wireloom.Dds.Generator.PackageIntegration/NuGet.Test.Config `
  -p:WireloomDdsGeneratorPackageVersion=$packageVersion
dotnet test tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --no-restore --configuration Release
```

The Quality workflow tests a fresh package across RTI 7.3.1/7.7.0 and
`net8.0`/`net10.0` on Windows and Ubuntu. This project also links the complete
runtime-contract suite, so package consumption exercises managed values,
aliases, collections, unions, native conversion, type support, and recursive
startup as well as package-specific MSBuild input handling. To reproduce a
combination, pass `-p:WireloomConsumerTargetFramework=net8.0` (or `net10.0`)
and the selected `RtiConnextDdsVersion` on both restore and run, along with
`WireloomDdsGeneratorPackageVersion`. CI uses `-p:LangVersion=12.0` on run to
check the minimum emitted C# language baseline.

The integration project intentionally does not use central package management
or a lock file: its generator package reference must be resolved from the
freshly created local feed. The other package versions remain explicit and
pinned in the project file. Its RTI package cache is isolated by
`RtiConnextDdsVersion`. Select either exact LTS package with
`-p:RtiConnextDdsVersion=7.3.1` or `-p:RtiConnextDdsVersion=7.7.0` on restore
and build; 7.7.0 is the default. The build writes generated C# files under the
project's `obj` directory, and the tests exercise those generated types at
compile time and at runtime.

The unbounded string and sequence fixtures ensure the packed analyzer compiles
those declarations. Runtime behavior, including native conversions at the
existing effective limits of 255 string bytes and 100 sequence elements, is
tested by the [runtime contract suite](../Wireloom.Dds.Generator.RuntimeContracts/README.md),
specifically [`UnboundedAliasNativeSpecs`](../Wireloom.Dds.Generator.RuntimeContracts/Native/UnboundedAliasNativeSpecs.cs).

Union construction regressions also link the runtime suite's
`union-construction.idl`. The packed analyzer must compile negative enum
defaults, use discriminator type defaults, and initialize independent selected
reference values when a copy source is null.

The packed analyzer also compiles `union-string-sequences.idl`, covering direct
narrow/wide sequence branches with nested bounded string syntax and
typedef-backed string-sequence branches.

## Generation settings

The consumer sets `DdsIdlSequenceArrayMapping=Idl` and compiles the runtime
suite's `sequence-arrays.idl` using that project default. Its separate legacy
fixture overrides the mode with `SequenceArrayMapping=RtiCSharp`, intentionally
retaining `DDSG0105` as a non-fatal warning. Typed checks exercise both APIs and
reuse the sequence-array runtime serialization contracts.

The ordinary packed consumer uses `PackageIntegrationStrict=false` and
explicitly exempts `DDSG0108` from warnings-as-errors. Its retained `Context`
type and focused `strict-identifiers.idl` fixture deliberately trigger that
warning; typed tests verify their original names remain usable. The fixture
also exercises a DDS-service operation named `Context`: its identifiers are
checked while its bindings remain omitted with `DDSG0103`. The consumer
exempts that deliberate omission warning and verifies that no service class
is generated.

`PackageIntegrationStrict` is a test-project switch, not a Wireloom package
setting. It supplies the existing `Strict` metadata on a generating root.
Quality CI also checks the negative build across its existing RTI/framework
matrix after the ordinary consumer test:

```powershell
./tests/Wireloom.Dds.Generator.PackageIntegration/scripts/Verify-StrictConsumer.ps1 `
  -PackageVersion $packageVersion -RtiVersion 7.7.0 -Framework net10.0
```

Use the package version, RTI version, and framework from the preceding restore.
The helper requires a nonzero build result containing `DDSG0001` for a naming
violation; an unrelated build error does not pass the check. Roslyn regression tests separately verify
that strict rejection adds no generated documents.

## .NET 8 consumer check

Consumer support starts at .NET 8 with C# 12. Repository projects default to
.NET 10; override the existing integration project's target to check the
minimum consumer framework without adding a project:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --configfile tests/Wireloom.Dds.Generator.PackageIntegration/NuGet.Test.Config `
  -p:WireloomDdsGeneratorPackageVersion=0.4.0 -p:WireloomConsumerTargetFramework=net8.0
dotnet run --project tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --no-restore --configuration Release `
  -p:WireloomDdsGeneratorPackageVersion=0.4.0 -p:WireloomConsumerTargetFramework=net8.0 -p:LangVersion=12.0
```

The Quality workflow also copies the package and the consumer fixture into an
`mcr.microsoft.com/dotnet/sdk:8.0.100` container. The container has only the .NET
8 SDK and runtime, then restores, builds, and runs this complete suite against
the freshly packed analyzer for both RTI LTS packages. This verifies the
supported .NET 8 compiler host rather than only a `net8.0` application running
alongside the repository's .NET 10 SDK. Use the package version from your
isolated feed when checking a newer package.
