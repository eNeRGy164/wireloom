# Shared compiler unit tests

This is the fast in-memory layer for preprocessing, parsing, binding,
validation, naming, and emission. Use focused IDL to assert the changed rule,
diagnostic location, and generated behavior without requiring the retained
corpus or licensed DDS peers.

From the repository root:

```powershell
dotnet restore tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Compiler.Tests/Wireloom.Dds.Compiler.Tests.csproj --no-restore --configuration Release
```

Pass Microsoft.Testing.Platform filters after `--`. Preprocessor traits use
`PP###` identifiers from the [feature matrix](../../docs/PREPROCESSOR-FEATURES.md);
`C###` traits connect focused tests to corpus cases without a file dependency.

For wider evidence, use [corpus compliance](../Wireloom.Dds.Generator.CorpusCompliance/README.md),
[generated runtime contracts](../Wireloom.Dds.Generator.RuntimeContracts/README.md),
and the [quality procedure](../../docs/testing/quality.md). The
[Roslyn adapter tests](../Wireloom.Dds.Generator.Tests/README.md) and
[fuzzing suite](../Wireloom.Dds.Generator.Fuzzing.Tests/README.md) are separate projects.
