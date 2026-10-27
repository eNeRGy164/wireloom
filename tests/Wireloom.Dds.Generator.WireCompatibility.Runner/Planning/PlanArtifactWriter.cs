using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Serializes plan evidence and the exchange worker contract from the in-memory plan.</summary>
internal static class PlanArtifactWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
        };
    private static readonly JsonSerializerOptions KeyJsonOptions = new(JsonOptions) { WriteIndented = false };

    /// <summary>Writes only artifacts consumed by execution, reporting, or shard verification.</summary>
    internal static void Write(ScenarioPlan plan, string destination)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);

        Directory.CreateDirectory(destination);

        var evidence = new
        {
            rtiVersion = plan.RtiVersion,
            shardCount = plan.ShardCount,
            expectedScenarioCount = plan.TotalScenarioCount,
            cases = plan.Cases.Select(item => new
            {
                id = item.CaseId,
                shard = item.Shard,
                scenarioKeys = item.Scenarios.Select(ScenarioKey).ToArray(),
            }).ToArray(),
        };

        File.WriteAllText(Path.Combine(destination, "plan.json"), JsonSerializer.Serialize(evidence, JsonOptions) + "\n");
        File.WriteAllLines(Path.Combine(destination, "scenarios.tsv"), plan.Scenarios.Select(ScenarioRow));
        File.WriteAllText(Path.Combine(destination, "shard.json"), JsonSerializer.Serialize(new
            {
                rtiVersion = plan.RtiVersion,
                shardCount = plan.ShardCount,
                shardIndex = plan.ShardIndex,
                expectedScenarioCount = plan.Scenarios.Count,
            }, JsonOptions) + "\n");
    }

    private static string ScenarioKey(PlannedScenario scenario) => JsonSerializer.Serialize(
        new[] { scenario.CaseId, scenario.Fixture, scenario.Writer, scenario.Reader }, KeyJsonOptions);

    private static string ScenarioRow(PlannedScenario scenario) => string.Join('\t', scenario.CaseId, "", "", "",
        scenario.Writer, scenario.Version, scenario.Reader, scenario.Version, "NOT_RUN", scenario.Fixture);
}
