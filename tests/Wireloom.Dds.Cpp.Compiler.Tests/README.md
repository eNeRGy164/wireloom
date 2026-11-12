# C++ compiler tests

This project exercises the C++ compiler facade, RTI profile selection, planning,
and source-writing primitives directly. It references the compiler and shared
IDL contracts, without depending on the CLI or native RTI SDK.

```powershell
dotnet restore tests/Wireloom.Dds.Cpp.Compiler.Tests/Wireloom.Dds.Cpp.Compiler.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Cpp.Compiler.Tests/Wireloom.Dds.Cpp.Compiler.Tests.csproj --configuration Release --no-restore
```

The planning tests use an assembly alias for the internal compiler type because
its name overlaps the `Wireloom.Dds.Cpp.Compiler.Tests` namespace.

See the [test layers](../../CONTRIBUTING.md#tests-and-fixtures),
[generator tests](../Wireloom.Dds.Cpp.Generator.Tests/README.md), and
[coverage procedure](../../docs/testing/quality.md#coverage-and-scan-scope).
