namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>The selected cases and expected evidence for one whole-case shard.</summary>
/// <param name="CaseIds">Cases in deterministic execution order.</param>
/// <param name="RtiOnlyCases">Cases without a Wireloom managed peer.</param>
/// <param name="Exchanges">Typed scenario plan, or null for a compile-only sweep.</param>
/// <param name="SelectedWireloomCppCases">Explicit native peers to compile in compile-only mode.</param>
/// <param name="SelectedNativePeerScopeCases">Explicit cases selected for the RTI C++ / Wireloom C++ peer scope in compile-only mode.</param>
internal sealed record MatrixPlan(IReadOnlyList<string> CaseIds, IReadOnlySet<string> RtiOnlyCases, ScenarioPlan? Exchanges,
    IReadOnlySet<string>? SelectedWireloomCppCases = null, IReadOnlySet<string>? SelectedNativePeerScopeCases = null)
{
    /// <summary>Number of planned directed exchanges, derived from the typed plan.</summary>
    internal int ExpectedScenarioCount => Exchanges?.Scenarios.Count ?? 0;

    /// <summary>Whether a selected case needs a peer for any of its actual planned fixture exchanges.</summary>
    internal IReadOnlySet<string> WireloomCppCases => SelectedWireloomCppCases ?? Exchanges?.WireloomCppCases
        ?? EmptyWireloomCppCases;

    /// <summary>Cases whose report rows must contain only the two native C++ peers.</summary>
    internal IReadOnlySet<string> NativePeerScopeCases => Exchanges?.NativePeerScopeCases
        ?? SelectedNativePeerScopeCases ?? EmptyWireloomCppCases;

    /// <summary>Cases that require the Wireloom managed peer in this invocation.</summary>
    internal IReadOnlyList<string> ManagedCaseIds => CaseIds.Where(caseId => !RtiOnlyCases.Contains(caseId)
        && !NativePeerScopeCases.Contains(caseId)).ToArray();

    /// <summary>Whether a selected case needs a peer for an exchange or an explicit compile-only request.</summary>
    internal bool RequiresPeer(string caseId, string peer) => Exchanges?.Scenarios.Any(scenario =>
        scenario.CaseId == caseId && (scenario.Writer == peer || scenario.Reader == peer)) == true
        || Exchanges is null && peer == PeerNames.WireloomCpp && WireloomCppCases.Contains(caseId);

    private static HashSet<string> EmptyWireloomCppCases { get; } = new(StringComparer.Ordinal);

    /// <summary>Recovery-report context before planning completes.</summary>
    internal static MatrixPlan Empty { get; } = new([], new HashSet<string>(StringComparer.Ordinal), Exchanges: null);
}
