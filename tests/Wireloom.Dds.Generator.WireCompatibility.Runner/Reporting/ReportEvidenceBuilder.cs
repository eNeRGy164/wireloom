using System.Text.Json.Nodes;
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Loads scenario evidence and applies profile, pairing, configuration, and provenance rules.</summary>
internal static class ReportEvidenceBuilder
{
    /// <summary>Loads and validates report evidence without serializing intermediate data.</summary>
    internal static ReportData Build(ReportRequest request)
    {
        var version = request.RtiVersion;
        var expectedCount = request.ExpectedScenarioCount;
        var exclusions = ReadJson(request.ExclusionsPath);
        var manifest = ReadJson(request.ManifestPath);
        var notImplemented = exclusions["notImplementedCases"]!.AsArray();
        var notApplicable = exclusions["notApplicableCases"]?.AsArray() ?? [];
        var unavailableIds = notApplicable.Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
        var wireMatrixConfiguration = manifest["positiveCases"]!.AsArray()
            .Where(item => item!["wireMatrix"] is not null)
            .ToDictionary(item => (string)item!["id"]!, item => item!["wireMatrix"]!, StringComparer.Ordinal);

        var scenarios = ReadScenarioRows(request.ScenarioPath)
            .Select(row => BuildScenario(row, request, manifest, unavailableIds, wireMatrixConfiguration)).ToList();

        var reportedCaseIds = scenarios.Select(scenario => (string)scenario["case"]!).ToHashSet(StringComparer.Ordinal);
        var knownExceptions = wireMatrixConfiguration
            .Where(entry => reportedCaseIds.Contains(entry.Key))
            .SelectMany(entry => (entry.Value["excludedPeersByRtiVersion"]?[version]?.AsObject() ?? [])
                .Select(exclusion => new JsonObject
                {
                    ["case"] = entry.Key,
                    ["peer"] = exclusion.Key,
                    ["reason"] = exclusion.Value?.DeepClone(),
                }))
            .ToArray();

        return new ReportData(version, expectedCount, scenarios, notImplemented, notApplicable, knownExceptions, manifest);
    }

    private static ReportFields BuildScenario(ScenarioRow row, ReportRequest request, JsonNode manifest, HashSet<string> unavailableIds, Dictionary<string, JsonNode> wireMatrixConfiguration)
    {
        var version = request.RtiVersion;
        var logsPath = request.LogsDirectory;
        var (id, wireType, rtiType, cppType, writer, writerVersion, reader, readerVersion, status, fixture) = row;
        if (unavailableIds.Contains(id))
        {
            throw new InvalidDataException($"Case {id} is not applicable to RTI {version} and cannot contribute exchanges.");
        }

        var writerEndpoint = ReportEndpointPolicy.Create(id, fixture, writer, writerVersion);
        var readerEndpoint = ReportEndpointPolicy.Create(id, fixture, reader, readerVersion);
        var scenario = new ReportFields
        {
            ["case"] = id,
            ["writer"] = writerEndpoint,
            ["reader"] = readerEndpoint,
            ["fixture"] = fixture,
            ["status"] = status,
        };

        if (wireMatrixConfiguration.TryGetValue(id, out var matrixConfiguration))
        {
            scenario["matrixConfiguration"] = EffectiveWireMatrixConfiguration(matrixConfiguration, version);
        }

        var corpusCase = FindCase(manifest, id);
        if (!PeerPairingPolicy.GetPeers(corpusCase, version).Contains(writer) || !PeerPairingPolicy.GetPeers(corpusCase, version).Contains(reader))
        {
            throw new InvalidDataException($"Exchange uses an excluded peer for {id}.");
        }

        if (!PeerPairingPolicy.GetPairings(corpusCase, fixture, version).Contains(new DirectedPeerPair(writer, reader)))
        {
            throw new InvalidDataException($"Exchange uses an excluded pairing for {id} [{fixture}].");
        }

        ReportProvenance.Attach(scenario, corpusCase, request, row);

        var adaptations = new Dictionary<string, string>();

        foreach (var endpoint in new[] { writerEndpoint, readerEndpoint })
        {
            if (endpoint.TryGetValue("adaptation", out var value))
            {
                adaptations[(string)endpoint["peer"]!] = (string)value!;
            }
        }

        if (adaptations.Count > 0)
        {
            scenario["adaptations"] = adaptations;
        }

        if (wireType.Length + rtiType.Length + cppType.Length > 0)
        {
            scenario["types"] = new Dictionary<string, string>
            {
                ["Wireloom C#"] = wireType,
                ["RTI C#"] = rtiType,
                ["RTI C++"] = cppType,
            };
        }

        if (status is "FAIL" or "NOT_RUN")
        {
            scenario["observedProblem"] = ReportProblemReader.Read(id, fixture, writer, reader, status, logsPath);
        }

        return scenario;
    }

    private static JsonObject EffectiveWireMatrixConfiguration(JsonNode configuration, string version)
    {
        var effective = (JsonObject)configuration.DeepClone();
        var excludedPeers = effective["excludedPeers"] as JsonObject ?? new JsonObject();
        var profileExclusions = effective["excludedPeersByRtiVersion"]?[version]?.AsObject();
        if (profileExclusions is not null)
        {
            foreach (var (peer, reason) in profileExclusions)
            {
                excludedPeers[peer] = reason?.DeepClone();
            }
        }

        effective["excludedPeers"] = excludedPeers;

        return effective;
    }

    private static JsonNode FindCase(JsonNode manifest, string id) => manifest["positiveCases"]!.AsArray()
        .FirstOrDefault(item => (string)item!["id"]! == id)
        ?? throw new InvalidOperationException($"No positive corpus case found for {id}.");

    private static JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

    private static List<ScenarioRow> ReadScenarioRows(string path) =>
        [.. File.ReadLines(path).Where(line => line.Length > 0).Select(ScenarioRow.Parse)];
}
