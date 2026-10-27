using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Expands fixtures and directed peer rules, then balances whole cases across shards.</summary>
internal static class ScenarioPlanBuilder
{
    /// <summary>Builds a typed plan without creating intermediate or evidence files.</summary>
    internal static ScenarioPlan Create(string manifestPath, string exclusionsPath, string version, string? caseFilter, int shardIndex, int shardCount)
    {
        if (version is not ("7.3.1" or "7.7.0"))
        {
            throw new ArgumentException($"Unsupported RTI profile: {version}.", nameof(version));
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shardCount);
        ArgumentOutOfRangeException.ThrowIfNegative(shardIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(shardIndex, shardCount);

        var discovered = CorpusCaseDiscovery.Read(manifestPath, exclusionsPath);
        var selectedCases = discovered.Select(caseFilter);
        var rtiOnlyCases = discovered.RtiOnlyCases.ToHashSet(StringComparer.Ordinal);

        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        var definitions = manifest["positiveCases"]!.AsArray()
            .ToDictionary(item => (string)item!["id"]!, item => item!, StringComparer.Ordinal);

        var scenarios = ExpandScenarios(selectedCases, rtiOnlyCases, definitions, version);

        var groups = scenarios.GroupBy(item => item.CaseId, StringComparer.Ordinal).ToArray();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(shardCount, groups.Length);

        var assignments = AssignShards(groups, shardCount);
        var cases = groups.Select(group => new PlannedCase(group.Key, assignments[group.Key], group.ToArray())).ToArray();
        var selected = scenarios.Where(item => assignments[item.CaseId] == shardIndex).ToArray();

        return new ScenarioPlan(version, shardIndex, shardCount, cases, selected, rtiOnlyCases);
    }

    private static List<PlannedScenario> ExpandScenarios(IReadOnlyList<string> caseIds, HashSet<string> rtiOnlyCases, Dictionary<string, JsonNode> definitions, string version)
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
                foreach (var (writer, reader) in PeerPairingPolicy.GetPairings(definition, fixture: fixture, version: version))
                {
                    if (rtiOnlyCases.Contains(caseId) && (writer == PeerNames.WireloomCSharp || reader == PeerNames.WireloomCSharp))
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
