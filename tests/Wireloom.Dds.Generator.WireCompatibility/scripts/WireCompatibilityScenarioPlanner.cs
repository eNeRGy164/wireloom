using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Expands the catalog into a deterministic, complete directed exchange plan.</summary>
internal static class WireCompatibilityScenarioPlanner
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    };
    private static readonly JsonSerializerOptions KeyJsonOptions = new(JsonOptions) { WriteIndented = false };

    /// <summary>Creates a full plan and writes the selected whole-case shard artifacts.</summary>
    internal static void CreatePlan(string manifestPath, string exclusionsPath, string version, string caseFilter,
        int shardIndex, int shardCount, string destination)
    {
        if (version is not ("7.3.1" or "7.7.0"))
        {
            throw new ArgumentException($"Unsupported RTI profile: {version}.");
        }

        var manifest = ReadJson(manifestPath);
        var exclusions = ReadJson(exclusionsPath);
        var positiveCases = manifest["positiveCases"]!.AsArray()
            .Select(item => (Id: (string)item!["id"]!, Item: item!))
            .ToDictionary(item => item.Id, item => item.Item, StringComparer.Ordinal);
        var rtiOnlyCases = exclusions["notImplementedCases"]!.AsArray()
            .Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
        var wireloomExclusions = exclusions["notImplementedCases"]!.AsArray()
            .Concat(exclusions["notApplicableCases"]?.AsArray() ?? [])
            .Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
        var availableCases = positiveCases.Keys.Where(id => !wireloomExclusions.Contains(id)).Concat(rtiOnlyCases).ToArray();
        var selectedCases = caseFilter == "-"
            ? availableCases
            : caseFilter.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (selectedCases.Length == 0 || selectedCases.Distinct(StringComparer.Ordinal).Count() != selectedCases.Length
            || selectedCases.Any(id => !availableCases.Contains(id, StringComparer.Ordinal)))
        {
            throw new InvalidDataException("The selected case filter is empty, duplicated, unknown, or inapplicable for this RTI profile.");
        }

        var scenarios = new List<PlannedScenario>();
        foreach (var caseId in selectedCases)
        {
            if (!positiveCases.TryGetValue(caseId, out var caseData))
            {
                throw new InvalidDataException($"Selected case {caseId} has no positive corpus definition.");
            }

            foreach (var fixture in WireCompatibilityCaseCatalog.FixturesFor(caseId, version))
            {
                foreach (var (writer, reader) in Pairings(caseData, fixture, version))
                {
                    if (rtiOnlyCases.Contains(caseId) && (writer == "Wireloom C#" || reader == "Wireloom C#"))
                    {
                        continue;
                    }

                    scenarios.Add(new PlannedScenario(caseId, fixture, writer, reader, version));
                }
            }
        }

        if (scenarios.Count == 0 || scenarios.Select(ScenarioKey).Distinct(StringComparer.Ordinal).Count() != scenarios.Count)
        {
            throw new InvalidDataException("The planned exchange set must be non-empty and unique.");
        }

        var cases = scenarios.GroupBy(scenario => scenario.CaseId, StringComparer.Ordinal).ToArray();
        if (shardCount < 1 || shardCount > cases.Length || shardIndex < 0 || shardIndex >= shardCount)
        {
            throw new ArgumentException("Shard count must be between one and the number of selected cases; index must be within the shard count.");
        }

        var loads = new int[shardCount];
        var assignments = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var group in cases.OrderByDescending(group => group.Count()).ThenBy(group => group.Key, StringComparer.Ordinal))
        {
            var shard = Enumerable.Range(0, shardCount).OrderBy(index => loads[index]).ThenBy(index => index).First();
            assignments.Add(group.Key, shard);
            loads[shard] += group.Count();
        }

        Directory.CreateDirectory(destination);
        var plan = new
        {
            rtiVersion = version,
            shardCount,
            expectedScenarioCount = scenarios.Count,
            cases = cases.Select(group => new
            {
                id = group.Key,
                shard = assignments[group.Key],
                scenarioKeys = group.Select(ScenarioKey).ToArray(),
            }).ToArray(),
        };
        var selected = scenarios.Where(scenario => assignments[scenario.CaseId] == shardIndex).ToArray();
        File.WriteAllText(Path.Combine(destination, "plan.json"), JsonSerializer.Serialize(plan, JsonOptions) + "\n");
        File.WriteAllLines(Path.Combine(destination, "scenarios.tsv"), selected.Select(scenario => scenario.ToTsv()));
        File.WriteAllLines(Path.Combine(destination, "selected-cases.txt"), cases.Where(group => assignments[group.Key] == shardIndex).Select(group => group.Key));
        File.WriteAllText(Path.Combine(destination, "shard.json"), JsonSerializer.Serialize(new
        {
            rtiVersion = version,
            shardCount,
            shardIndex,
            expectedScenarioCount = selected.Length,
        }, JsonOptions) + "\n");
    }

    private static (string Writer, string Reader)[] Pairings(JsonNode caseData, string fixture, string version)
    {
        var peers = caseData["wireMatrix"]?["peers"] is JsonArray configuredPeers
            ? configuredPeers.Select(peer => (string)peer!).ToList()
            : ["Wireloom C#", "RTI C#", "RTI C++"];
        foreach (var excluded in caseData["wireMatrix"]?["excludedPeersByRtiVersion"]?[version]?.AsObject() ?? [])
        {
            peers.Remove(excluded.Key);
        }

        if (caseData["wireMatrix"]?["fixturePairings"]?[fixture] is JsonArray configuredPairings)
        {
            var pairings = configuredPairings.Select(item =>
            {
                var endpoints = ((string)item!).Split('|');
                if (endpoints.Length != 2 || !peers.Contains(endpoints[0]) || !peers.Contains(endpoints[1]))
                {
                    throw new InvalidDataException($"Invalid peer pairing for {fixture}.");
                }

                return (Writer: endpoints[0], Reader: endpoints[1]);
            }).ToArray();
            if (pairings.Length == 0 || pairings.Distinct().Count() != pairings.Length)
            {
                throw new InvalidDataException($"Peer pairings for {fixture} must be non-empty and unique.");
            }

            return pairings;
        }

        return peers.SelectMany(writer => peers.Select(reader => (Writer: writer, Reader: reader))).ToArray();
    }

    private static JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

    private static string ScenarioKey(PlannedScenario scenario) =>
        JsonSerializer.Serialize(new[] { scenario.CaseId, scenario.Fixture, scenario.Writer, scenario.Reader }, KeyJsonOptions);

    private sealed record PlannedScenario(string CaseId, string Fixture, string Writer, string Reader, string Version)
    {
        internal string ToTsv() => string.Join('\t', CaseId, "", "", "", Writer, Version, Reader, Version, "NOT_RUN", Fixture);
    }
}
