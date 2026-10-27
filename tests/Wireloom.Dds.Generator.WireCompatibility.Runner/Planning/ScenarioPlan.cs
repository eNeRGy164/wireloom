namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>A complete assignment of whole cases and the exchanges selected for one shard.</summary>
/// <param name="RtiVersion">Exact qualified runtime profile.</param>
/// <param name="ShardIndex">Selected zero-based shard.</param>
/// <param name="ShardCount">Number of independent case partitions.</param>
/// <param name="Cases">Full case assignment in selection order, shared by all shards.</param>
/// <param name="Scenarios">Selected shard exchanges in fixture and pairing order.</param>
/// <param name="RtiOnlyCases">Implementation gaps restricted to RTI control peers.</param>
internal sealed record ScenarioPlan(string RtiVersion, int ShardIndex, int ShardCount,
    IReadOnlyList<PlannedCase> Cases, IReadOnlyList<PlannedScenario> Scenarios, IReadOnlySet<string> RtiOnlyCases)
{
    /// <summary>Case IDs selected for this shard, ready for peer preparation.</summary>
    internal IReadOnlyList<string> CaseIds => [.. Cases.Where(item => item.Shard == ShardIndex).Select(item => item.CaseId)];

    /// <summary>Expected exchange count across every shard.</summary>
    internal int TotalScenarioCount => Cases.Sum(item => item.Scenarios.Count);
}

/// <summary>A whole-case shard assignment and its complete expected evidence.</summary>
/// <param name="CaseId">Corpus identifier.</param>
/// <param name="Shard">Assigned zero-based shard.</param>
/// <param name="Scenarios">All directed exchanges belonging to this case.</param>
internal sealed record PlannedCase(string CaseId, int Shard, IReadOnlyList<PlannedScenario> Scenarios);

/// <summary>One immutable, directed fixture exchange before peer execution.</summary>
/// <param name="CaseId">Corpus identifier.</param>
/// <param name="Fixture">Payload fixture name.</param>
/// <param name="Writer">Writer peer kind.</param>
/// <param name="Reader">Reader peer kind.</param>
/// <param name="Version">Exact runtime version for both endpoints.</param>
internal sealed record PlannedScenario(string CaseId, string Fixture, string Writer, string Reader, string Version);
