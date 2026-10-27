namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>The selected cases and expected evidence for one whole-case shard.</summary>
/// <param name="CaseIds">Cases in deterministic execution order.</param>
/// <param name="RtiOnlyCases">Cases without a Wireloom managed peer.</param>
/// <param name="Exchanges">Typed scenario plan, or null for a compile-only sweep.</param>
internal sealed record MatrixPlan(IReadOnlyList<string> CaseIds, IReadOnlySet<string> RtiOnlyCases, ScenarioPlan? Exchanges)
{
    /// <summary>Number of planned directed exchanges, derived from the typed plan.</summary>
    internal int ExpectedScenarioCount => Exchanges?.Scenarios.Count ?? 0;

    /// <summary>Whether a selected case needs a peer for any of its actual planned fixture exchanges.</summary>
    internal bool RequiresPeer(string caseId, string peer) => Exchanges?.Scenarios.Any(scenario =>
        scenario.CaseId == caseId && (scenario.Writer == peer || scenario.Reader == peer)) == true;

    /// <summary>Recovery-report context before planning completes.</summary>
    internal static MatrixPlan Empty { get; } = new([], new HashSet<string>(StringComparer.Ordinal), Exchanges: null);
}
