# C++ generator tests

This project exercises command parsing, diagnostics, filesystem-backed IDL
generation, emitted C++ surfaces, and the executable entry point. It retains
the existing CLI and emission assertions. Lower-level compiler tests, corpus
generation, fuzzing, and native package consumption have their own test layers:

| Layer | Entry point |
| :---- | :---------- |
| Compiler | [Compiler tests](../Wireloom.Dds.Cpp.Compiler.Tests/README.md) |
| Corpus | [Corpus compliance](../Wireloom.Dds.Cpp.CorpusCompliance/README.md) |
| Fuzzing | [FsCheck tests](../Wireloom.Dds.Cpp.Fuzzing.Tests/README.md) |
| Generator package | [Native package checks](../Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md) |
| Generator | This project |

Run the ordinary generator suite independently:

```powershell
dotnet restore tests/Wireloom.Dds.Cpp.Generator.Tests/Wireloom.Dds.Cpp.Generator.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Cpp.Generator.Tests/Wireloom.Dds.Cpp.Generator.Tests.csproj --configuration Release --no-restore
```

For the combined compiler/CLI coverage gate, use the coverage-only build:

```powershell
dotnet run --project tests/Wireloom.Dds.Cpp.Generator.Tests/Wireloom.Dds.Cpp.Generator.Tests.csproj `
  --configuration Release --no-restore -p:WireloomCppCombinedCoverage=true `
  -- --results-directory coverage --coverage `
  --coverage-settings tests/Wireloom.Dds.Cpp.Generator.Tests/coverage.settings.xml `
  --coverage-output cpp.cobertura.xml --coverage-output-format cobertura
```

This mode links the compiler and corpus test sources into the existing generator
test host only for coverage collection. It uses separate `bin/Release/combined`
and `obj/Release/combined` outputs, so ordinary test binaries stay independent.
One collector records exact branch coverage across all three suites without
merging reports. Fuzzing runs separately and does not contribute coverage.

Keep the existing 90% branch and 95% line thresholds. See the
[quality procedure](../../docs/testing/quality.md#coverage-and-scan-scope) for
CI, Qodana, and Coveralls evidence.
