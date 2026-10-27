using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>The deterministic scenario inputs and partition outputs for shard checks.</summary>
internal sealed record ShardScenarioFixture(Dictionary<string, string[]> Cases, List<ScenarioRow> Rows, string[] Peers, string Seed, string Exclusions, string Source);

/// <summary>The merged report and independently rendered baseline.</summary>
internal sealed record MergedReportFixture(string MergedDirectory, string BaselineJson);

/// <summary>Artifacts emitted by report verification and consumed by integrity checks.</summary>
internal sealed record ShardReportArtifacts(string ScalarMarkdown, JsonNode?[] NumericScenarios);
