# C# Roslyn generator tests

This project verifies the Roslyn adapter: compiler inputs and options,
applicability diagnostics, generated-document presentation, and incremental
invalidation. Inputs are created in memory; tests do not require the retained
RTI corpus or a live DDS domain.

## Run

From the repository root, using the pinned SDK:

```powershell
dotnet restore tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.CSharp.Generator.Tests/Wireloom.Dds.CSharp.Generator.Tests.csproj --no-restore --configuration Release
```

Use Microsoft.Testing.Platform options after `--`. The
[incremental measurement guide](../../docs/performance/incremental-generator-baseline.md)
provides the focused measurement filter and historical observations.

## Related test layers

- [Compiler unit tests](../Wireloom.Dds.CSharp.Compiler.Tests/README.md) own parsing,
  preprocessing, binding, validation, and emission rules.
- [Fuzzing](../Wireloom.Dds.CSharp.Fuzzing.Tests/README.md) owns bounded
  property checks of the public compiler and adapter.
- [Corpus compliance](../Wireloom.Dds.CSharp.CorpusCompliance/README.md)
  owns manifest loading and exact-profile oracle comparisons.
- [Package integration](../Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md)
  checks real MSBuild/analyzer consumption; in-memory adapter tests do not.

Focused tests may carry `Corpus=C###` traits without reading corpus files.
Test support is shared through `Wireloom.Dds.CSharp.TestSupport` and the
linked helpers under `tests/Shared`.

[Documentation](../../docs/README.md) · [Quality procedure](../../docs/testing/quality.md)
