# Testing

## Purpose

The repository separates fast generator tests, manifest-driven corpus
compliance, and packed-package integration tests. Together they cover parser
and emitter invariants, the checked-in IDL/reference corpus, and the package
consumption path.

## Test layers

- `tests/Wireloom.Dds.Generator.Tests` contains fast, self-contained unit tests
  with in-memory IDL inputs. It does not load the corpus or require RTI
  runtime files.
- `tests/Wireloom.Dds.Generator.CorpusCompliance` loads
  `docs/corpus/manifest.json`, validates corpus inventory and acceptance
  behavior, and compares accepted generated C# shapes with the checked-in RTI
  oracle sources.
- `tests/Wireloom.Dds.Generator.PackageIntegration` consumes the packed
  generator package against the corpus integration entry points.

## Local commands

Restore each project with its lock file before using `--no-restore`:

```text
dotnet restore tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj --no-restore --configuration Release

dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
dotnet test tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
```

The corpus suite currently contains one intentional skip for
`02-alias-cycle`; the in-process alias resolver overflows instead of returning
an `IdlException`, so that probe needs an isolated compiler-process harness.

See [the corpus guide](../corpus/README.md), [the feature coverage index](../corpus/FEATURE-COVERAGE.md), and the README in each test project for the scope and maintenance rules of each layer.
