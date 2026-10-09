# Wireloom package integration tests

This project consumes the packed `Wireloom.Dds.Generator` NuGet package. It
does not reference the generator project, so compilation proves that the
package carries the analyzer and its build targets correctly.

Run it locally by packing the generator into the isolated test feed, then
restoring and testing this project:

```shell
dotnet restore src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj --locked-mode
dotnet pack src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj `
  --no-restore --configuration Release --output package-test-feed `
$package = Get-ChildItem package-test-feed -Filter 'Wireloom.Dds.Generator.*.nupkg' |
  Sort-Object LastWriteTime -Descending | Select-Object -First 1
$packageVersion = $package.BaseName -replace '^Wireloom\.Dds\.Generator\.', ''
dotnet restore tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --configfile tests/Wireloom.Dds.Generator.PackageIntegration/NuGet.Test.Config `
  -p:WireloomDdsGeneratorPackageVersion=$packageVersion
dotnet test tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj --no-restore
```

The integration project intentionally does not use central package management
or a lock file: its generator package reference must be resolved from the
freshly created local feed. The other package versions remain explicit and
pinned in the project file. The build writes generated C# files under the
project's `obj` directory, and the tests exercise those generated types at
compile time and at runtime.

## .NET 8 consumer check

Consumer support starts at .NET 8 with C# 12. Repository projects default to
.NET 10; override the existing integration project's target to check the
minimum consumer framework without adding a project:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --configfile tests/Wireloom.Dds.Generator.PackageIntegration/NuGet.Test.Config `
  -p:WireloomDdsGeneratorPackageVersion=0.3.0 -p:TargetFramework=net8.0
dotnet run --project tests/Wireloom.Dds.Generator.PackageIntegration/Wireloom.Dds.Generator.PackageIntegration.csproj `
  --no-restore --configuration Release `
  -p:WireloomDdsGeneratorPackageVersion=0.3.0 -p:TargetFramework=net8.0 -p:LangVersion=12.0
```

On October 9, 2026, all **7 tests passed** against the local 0.3.0 package,
targeting `net8.0` with C# 12 and running on .NET 8.0.31. The build used .NET SDK
10.0.401 under the repository's pinned SDK policy. This verifies the consumer
framework; it does not establish compatibility with every older SDK's Roslyn
host. Use the package version from your isolated feed when checking a newer
package.
