# Incremental generator baseline

This document records the current Roslyn incremental-generator behavior before
any per-root caching redesign. The measurement harness is
[`IncrementalGeneratorMeasurementSpecs`](../../tests/Wireloom.Dds.Generator.Tests/Generation/IncrementalGeneratorMeasurementSpecs.cs).
It uses `GeneratorDriver` tracking and measures both driver and generator wall
time. Timing values are indicative; tracked-step reasons and generated-source
counts are the authoritative invalidation evidence.

## Reproduction

Run the focused measurement with the pinned SDK and existing test dependencies:

```powershell
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj `
  --no-restore --configuration Release `
  --filter-class Wireloom.Generation.Tests.IncrementalGeneratorMeasurementSpecs `
  --show-live-output on --output Detailed
```

The harness covers a cold run, unchanged rerun, root edit, included-file edit,
unrelated non-IDL input edit, and an edit to one of two roots. It keeps the
same generator driver between the initial and changed run in each scenario.

## Captured baseline

Captured on 2026-10-02 with SDK 10.0.401, .NET 10.0.12, and Microsoft.CodeAnalysis
4.14.0 on the repository Windows host. The elapsed values are rounded from the
test output and should not be treated as performance thresholds.

| Scenario | Generator ms | Generated sources | Tracked-step result |
| --- | ---: | ---: | --- |
| Cold run | 319.70 | 8 | 5 New |
| Unchanged rerun | 7.81 | 8 | 5 Cached |
| Root IDL change | 3.12 | 8 | SourceOutput Modified; one AdditionalTexts Modified; remaining steps Cached |
| Included IDL change | 0.34 | 8 | SourceOutput Modified; one AdditionalTexts Modified; remaining steps Cached |
| Unrelated non-IDL input change | 0.13 | 8 | 5 Cached |
| Second root changed in a two-root batch | 0.41 | 12 | SourceOutput Modified; one AdditionalTexts Modified; remaining steps Cached |

The Roslyn pipeline therefore reuses unchanged `AdditionalTexts`, compilation,
and analyzer-options steps. A tracked root or included-file edit reaches the
single `SourceOutput` step, so the current generator recomputes the generation
batch and emits all documents for that batch. A non-IDL additional-file edit is
outside the filtered input provider and remains fully cached. With multiple
roots, changing one root still invalidates the shared source-output batch even
though the other root's input step remains cached.

This confirms the existing architecture risk: the generator has incremental
input-step reuse, but does not yet provide per-root output caching. No caching
redesign or generator semantic change is included in this baseline.
