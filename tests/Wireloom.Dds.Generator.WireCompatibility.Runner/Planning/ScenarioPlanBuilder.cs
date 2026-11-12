using System.Text.Json.Nodes;
namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Expands fixtures and directed peer rules, then balances whole cases across shards.</summary>
internal static class ScenarioPlanBuilder
{
    /// <summary>Builds a typed plan without creating intermediate or evidence files.</summary>
    internal static ScenarioPlan Create(string manifestPath, string exclusionsPath, string version, string? caseFilter, int shardIndex, int shardCount,
        string? wireloomCppCaseFilter = null, string? nativePeerScopeCaseFilter = null)
    {
        if (version is not ("6.1.2" or "7.3.1" or "7.7.0"))
        {
            throw new ArgumentException($"Unsupported RTI profile: {version}.", nameof(version));
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shardCount);
        ArgumentOutOfRangeException.ThrowIfNegative(shardIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(shardIndex, shardCount);

        var discovered = CorpusCaseDiscovery.Read(manifestPath, exclusionsPath);
        var nativePeerScopeCases = WireloomCppScope.ReadExplicitCases(nativePeerScopeCaseFilter, CorpusCaseDiscovery.ReadPositiveCaseIds(manifestPath));
        var selectedCases = discovered.Select(caseFilter, nativePeerScopeCases.ToHashSet(StringComparer.Ordinal)).ToList();
        var rtiOnlyCases = discovered.RtiOnlyCases.ToHashSet(StringComparer.Ordinal);
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        var definitions = manifest["positiveCases"]!.AsArray()
            .ToDictionary(item => (string)item!["id"]!, item => item!, StringComparer.Ordinal);
        var overlap = selectedCases.Intersect(nativePeerScopeCases, StringComparer.Ordinal).ToArray();
        if (overlap.Length > 0)
        {
            throw new InvalidDataException($"Cases selected for the native peer scope must be excluded from the managed case filter: {string.Join(", ", overlap)}.");
        }

        selectedCases.AddRange(nativePeerScopeCases);
        var explicitCppCases = WireloomCppScope.ReadExplicitCases(wireloomCppCaseFilter, selectedCases);

        // Live C++ participation follows the same case/profile peer exclusions as
        // the other peers. A successful qualification list is not a prerequisite
        // for scheduling a peer: unqualified exchanges must run and produce
        // evidence before the matrix can claim support.
        var cppSelection = string.IsNullOrWhiteSpace(wireloomCppCaseFilter)
            ? selectedCases.ToHashSet(StringComparer.Ordinal)
            : explicitCppCases;
        cppSelection.UnionWith(nativePeerScopeCases);
        var cppCases = cppSelection.Where(caseId =>
                PeerPairingPolicy.GetPeers(definitions[caseId], version, includeWireloomCpp: true)
                    .Contains(PeerNames.WireloomCpp, StringComparer.Ordinal))
            .ToHashSet(StringComparer.Ordinal);
        var activeNativePeerScopeCases = nativePeerScopeCases.Where(cppCases.Contains).ToHashSet(StringComparer.Ordinal);
        var scenarios = ExpandScenarios(selectedCases, rtiOnlyCases, definitions, version, cppCases, activeNativePeerScopeCases);

        var groups = scenarios.GroupBy(item => item.CaseId, StringComparer.Ordinal).ToArray();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(shardCount, groups.Length);

        var assignments = AssignShards(groups, shardCount);
        var cases = groups.Select(group => new PlannedCase(group.Key, assignments[group.Key], group.ToArray())).ToArray();
        var selected = scenarios.Where(item => assignments[item.CaseId] == shardIndex).ToArray();

        return new ScenarioPlan(version, shardIndex, shardCount, cases, selected, rtiOnlyCases, cppCases, activeNativePeerScopeCases);
    }

    private static List<PlannedScenario> ExpandScenarios(IReadOnlyList<string> caseIds, HashSet<string> rtiOnlyCases,
        Dictionary<string, JsonNode> definitions, string version, HashSet<string> cppCases,
        HashSet<string> nativePeerScopeCases)
    {
        var scenarios = new List<PlannedScenario>();

        foreach (var caseId in caseIds)
        {
            if (!definitions.TryGetValue(caseId, out var definition))
            {
                throw new InvalidDataException($"Selected case {caseId} has no positive corpus definition.");
            }

            foreach (var fixture in WireCompatibilityCaseCatalog.FixturesFor(caseId, version))
            {
                if (nativePeerScopeCases.Contains(caseId) && cppCases.Contains(caseId))
                {
                    scenarios.Add(new PlannedScenario(caseId, fixture, PeerNames.RtiCpp, PeerNames.WireloomCpp, version));
                    scenarios.Add(new PlannedScenario(caseId, fixture, PeerNames.WireloomCpp, PeerNames.RtiCpp, version));
                    continue;
                }

                var includeWireloomCpp = cppCases.Contains(caseId);
                foreach (var (writer, reader) in PeerPairingPolicy.GetPairings(definition, fixture: fixture, version: version,
                    includeWireloomCpp: includeWireloomCpp))
                {
                    if (rtiOnlyCases.Contains(caseId)
                        && (writer is PeerNames.WireloomCSharp or PeerNames.WireloomCpp
                            || reader is PeerNames.WireloomCSharp or PeerNames.WireloomCpp))
                    {
                        continue;
                    }

                    scenarios.Add(new PlannedScenario(caseId, fixture, writer, reader, version));
                }
            }
        }

        if (scenarios.Count == 0 || scenarios.Distinct().Count() != scenarios.Count)
        {
            throw new InvalidDataException("The planned exchange set must be non-empty and unique.");
        }

        return scenarios;
    }

    private static Dictionary<string, int> AssignShards(IGrouping<string, PlannedScenario>[] cases, int shardCount)
    {
        var loads = new int[shardCount];
        var assignments = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var group in cases.OrderByDescending(g => g.Count()).ThenBy(g => g.Key, StringComparer.Ordinal))
        {
            var shard = Enumerable.Range(0, shardCount)
                .OrderBy(index => loads[index])
                .ThenBy(index => index)
                .First();

            assignments.Add(group.Key, shard);
            loads[shard] += group.Count();
        }

        return assignments;
    }
}
