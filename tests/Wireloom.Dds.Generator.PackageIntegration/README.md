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
