# C# compiler tests

This is the fast in-memory layer for C# compiler behavior, including generated
type mapping, naming, emission, and compiler diagnostics. Shared preprocessing
and direct IDL parser tests live in the separate [IDL front-end suite](../Wireloom.Dds.Idl.Tests/README.md).

From the repository root:

```shell
dotnet restore tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.Compiler.Tests/Wireloom.Dds.CSharp.Compiler.Tests.csproj --no-restore --configuration Release
```

Pass Microsoft.Testing.Platform filters after `--`. Preprocessor traits use
`PP###` identifiers from the [feature matrix](../../docs/PREPROCESSOR-FEATURES.md);
`C###` traits connect focused tests to corpus cases without a file dependency.

For end-to-end C# evidence, use [corpus compliance](../Wireloom.Dds.CSharp.CorpusCompliance/README.md),
[generated runtime contracts](../Wireloom.Dds.CSharp.RuntimeContracts/README.md),
and the [quality procedure](../../docs/testing/quality.md). The
[Roslyn adapter tests](../Wireloom.Dds.CSharp.Generator.Tests/README.md) and
[fuzzing suite](../Wireloom.Dds.CSharp.Fuzzing.Tests/README.md) are separate projects.
