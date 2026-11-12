# IDL front-end unit tests

This project owns direct tests of shared IDL preprocessing and parsing behavior.
It references `Wireloom.Dds.Idl` directly and does not depend on either language
backend. C# and C++ compiler suites retain their backend integration assertions.

From the repository root:

```shell
dotnet restore tests/Wireloom.Dds.Idl.Tests/Wireloom.Dds.Idl.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Idl.Tests/Wireloom.Dds.Idl.Tests.csproj --no-restore --configuration Release
```

For an independent source-coverage report, use `coverage.idl.settings.xml` and
write `idl.cobertura.xml`; the Quality workflow retains this report with the
compiler and generator reports.
