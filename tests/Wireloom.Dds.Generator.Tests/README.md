# Roslyn generator tests

This project verifies the Roslyn adapter: compiler inputs and options,
applicability diagnostics, generated-document presentation, and incremental
invalidation. Inputs are created in memory; tests do not require the retained
RTI corpus or a live DDS domain.

## Run

From the repository root, using the pinned SDK:

```powershell
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release
```

Use Microsoft.Testing.Platform options after `--`. The
[incremental measurement guide](../../docs/performance/incremental-generator-baseline.md)
provides the focused measurement filter and historical observations.

## Related test layers

- [Compiler unit tests](../Wireloom.Dds.Compiler.Tests/README.md) own parsing,
  preprocessing, binding, validation, and emission rules.
- [Fuzzing](../Wireloom.Dds.Generator.Fuzzing.Tests/README.md) owns bounded
  property checks of the public compiler and adapter.
- [Corpus compliance](../Wireloom.Dds.Generator.CorpusCompliance/README.md)
  owns manifest loading and exact-profile oracle comparisons.
- [Package integration](../Wireloom.Dds.Generator.PackageIntegration/README.md)
  checks real MSBuild/analyzer consumption; in-memory adapter tests do not.

Focused tests may carry `Corpus=C###` traits without reading corpus files.
Test support is shared through `Wireloom.Dds.Generator.TestSupport` and the
linked helpers under `tests/Shared`.

[Documentation](../../docs/README.md) · [Quality procedure](../../docs/testing/quality.md)
