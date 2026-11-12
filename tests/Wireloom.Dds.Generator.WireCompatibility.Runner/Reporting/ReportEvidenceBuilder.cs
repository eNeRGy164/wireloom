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
        var cppCases = request.WireloomCppCases is not null
            ? request.WireloomCppCases.ToHashSet(StringComparer.Ordinal)
            : manifest["positiveCases"]!.AsArray()
                .Where(item => PeerPairingPolicy.GetPeers(item!, version, includeWireloomCpp: true)
                    .Contains(PeerNames.WireloomCpp, StringComparer.Ordinal))
                .Select(item => (string)item!["id"]!)
                .ToHashSet(StringComparer.Ordinal);
        var notImplemented = exclusions["notImplementedCases"]!.AsArray();
        var notApplicable = exclusions["notApplicableCases"]?.AsArray() ?? [];
        var unavailableIds = notApplicable.Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
        var nativePeerScopeCases = request.NativePeerScopeCases?.ToHashSet(StringComparer.Ordinal)
            ?? new HashSet<string>(StringComparer.Ordinal);
        var wireMatrixConfiguration = manifest["positiveCases"]!.AsArray()
            .Where(item => item!["wireMatrix"] is not null)
            .ToDictionary(item => (string)item!["id"]!, item => item!["wireMatrix"]!, StringComparer.Ordinal);
        var provenancePath = request.ExecutionProvenancePath
            ?? Path.Combine(Path.GetDirectoryName(Path.GetFullPath(request.ScenarioPath))!, "execution-provenance.json");
        var sourceSnapshot = SourceSnapshotEvidence.Read(provenancePath);

        var scenarioRows = ReadScenarioRows(request.ScenarioPath);
        ValidateNativePeerScopeRows(scenarioRows, version, unavailableIds, nativePeerScopeCases);
        var reportedNativePeerScopeUnavailableCases = scenarioRows
            .Where(row => unavailableIds.Contains(row.CaseId) && nativePeerScopeCases.Contains(row.CaseId))
            .Select(row => row.CaseId)
            .ToHashSet(StringComparer.Ordinal);
        var reportNotApplicable = new JsonArray(notApplicable.Select(item =>
        {
            var copy = (JsonObject)item!.DeepClone();
            if (reportedNativePeerScopeUnavailableCases.Contains((string)copy["id"]!))
            {
                copy["reason"] = "The managed peer pairing scope excludes this case because it exercises optional sequences and arrays. Explicit native peer-scope RTI C++ / Wireloom C++ exchanges are included; managed peer exchanges remain excluded.";
            }

            return copy;
        }).ToArray());

        var scenarios = scenarioRows
            .Select(row => BuildScenario(row, request, manifest, unavailableIds, nativePeerScopeCases, wireMatrixConfiguration, sourceSnapshot, cppCases)).ToList();

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

        return new ReportData(version, expectedCount, scenarios, notImplemented, reportNotApplicable, knownExceptions, manifest, sourceSnapshot, cppCases,
            nativePeerScopeCases);
    }

    private static void ValidateNativePeerScopeRows(IReadOnlyList<ScenarioRow> rows, string version,
        HashSet<string> unavailableIds, HashSet<string> nativePeerScopeCases)
    {
        foreach (var row in rows.Where(row => unavailableIds.Contains(row.CaseId)))
        {
            if (!nativePeerScopeCases.Contains(row.CaseId))
            {
                throw new InvalidDataException($"Case {row.CaseId} is not applicable to RTI {version} and cannot contribute exchanges.");
            }
        }

        foreach (var caseGroup in rows.Where(row => nativePeerScopeCases.Contains(row.CaseId))
                     .GroupBy(row => row.CaseId, StringComparer.Ordinal))
        {
            var expectedFixtures = WireCompatibilityCaseCatalog.FixturesFor(caseGroup.Key, version)
                .ToHashSet(StringComparer.Ordinal);
            var fixtureGroups = caseGroup.GroupBy(row => row.Fixture, StringComparer.Ordinal).ToArray();
            if (!fixtureGroups.Select(group => group.Key).ToHashSet(StringComparer.Ordinal).SetEquals(expectedFixtures))
            {
                throw new InvalidDataException($"Native peer-scope case {caseGroup.Key} must report every expected {version} fixture.");
            }

            foreach (var fixtureGroup in fixtureGroups)
            {
                var pairings = fixtureGroup.Select(row => (row.Writer, row.Reader)).ToHashSet();
                var expectedPairings = new HashSet<(string Writer, string Reader)>
                {
                    (PeerNames.RtiCpp, PeerNames.WireloomCpp),
                    (PeerNames.WireloomCpp, PeerNames.RtiCpp),
                };
                if (fixtureGroup.Any(row => row.WriterVersion != version || row.ReaderVersion != version)
                    || fixtureGroup.Count() != expectedPairings.Count || !pairings.SetEquals(expectedPairings))
                {
                    throw new InvalidDataException($"Native peer-scope case {caseGroup.Key} [{fixtureGroup.Key}] must contain exactly the directed RTI C++ / Wireloom C++ pairings for RTI {version}.");
                }
            }
        }
    }

    private static ReportFields BuildScenario(ScenarioRow row, ReportRequest request, JsonNode manifest, HashSet<string> unavailableIds,
        HashSet<string> nativePeerScopeCases, Dictionary<string, JsonNode> wireMatrixConfiguration, SourceSnapshotEvidence sourceSnapshot, HashSet<string> cppCases)
    {
        var version = request.RtiVersion;
        var logsPath = request.LogsDirectory;
        var (id, wireType, rtiType, cppType, writer, writerVersion, reader, readerVersion, rowStatus, fixture) = row;
        var provenanceInvalid = !sourceSnapshot.VerifiedUnchanged;
        var status = provenanceInvalid && rowStatus == "PASS" ? "FAIL" : rowStatus;

        if (unavailableIds.Contains(id) && !nativePeerScopeCases.Contains(id))
        {
            throw new InvalidDataException($"Case {id} is not applicable to RTI {version} and cannot contribute exchanges.");
        }

        var writerEndpoint = ReportEndpointPolicy.Create(id, fixture, writer, writerVersion, isWriter: true);
        var readerEndpoint = ReportEndpointPolicy.Create(id, fixture, reader, readerVersion, isWriter: false);
        var scenario = new ReportFields
        {
            ["case"] = id,
            ["writer"] = writerEndpoint,
            ["reader"] = readerEndpoint,
            ["fixture"] = fixture,
            ["status"] = status,
            ["sourceRevision"] = sourceSnapshot.Revision,
            ["dirtySourceSnapshotSha256"] = sourceSnapshot.DirtySnapshotSha256,
            ["dirtySourceFileCount"] = sourceSnapshot.DirtyFileCount,
        };

        if (wireMatrixConfiguration.TryGetValue(id, out var matrixConfiguration))
        {
            scenario["matrixConfiguration"] = EffectiveWireMatrixConfiguration(matrixConfiguration, version);
        }

        var corpusCase = FindCase(manifest, id);
        var includeWireloomCpp = cppCases.Contains(id);
        if (!PeerPairingPolicy.GetPeers(corpusCase, version, includeWireloomCpp).Contains(writer)
            || !PeerPairingPolicy.GetPeers(corpusCase, version, includeWireloomCpp).Contains(reader))
        {
            throw new InvalidDataException($"Exchange uses an excluded peer for {id}.");
        }

        if (!PeerPairingPolicy.GetPairings(corpusCase, fixture, version, includeWireloomCpp).Contains(new DirectedPeerPair(writer, reader)))
        {
            throw new InvalidDataException($"Exchange uses an excluded pairing for {id} [{fixture}].");
        }

        ReportProvenance.Attach(scenario, corpusCase, request, row);

        if (id == "14-unbounded-receive-lifecycle")
        {
            var logName = $"{id}-{fixture}-{PeerLogId(writer)}-{PeerLogId(reader)}.log";
            scenario["lifecycleEvidence"] = LifecycleEvidenceBuilder.Build(fixture, status, Path.Combine(logsPath, logName),
                writer == "RTI C++" || reader == "RTI C++");
        }

        var adaptations = new Dictionary<string, string>();

        // Prefer writer evidence when both endpoints use the same peer.
        foreach (var endpoint in new[] { readerEndpoint, writerEndpoint })
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
                ["Wireloom C++"] = cppType,
            };
        }

        if (status is "FAIL" or "NOT_RUN")
        {
            scenario["observedProblem"] = provenanceInvalid && rowStatus == "PASS"
                ? $"Execution source changed during the run: {sourceSnapshot.IntegrityFailure}"
                : ReportProblemReader.Read(id, fixture, writer, reader, status, logsPath);
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

    private static string PeerLogId(string peer) => peer switch
    {
        "Wireloom C#" => "wireloom-cs",
        "Wireloom C++" => "wireloom-cpp",
        "RTI C#" => "rti-cs",
        "RTI C++" => "rti-cpp",
        _ => throw new InvalidDataException($"Unknown wire peer '{peer}'."),
    };

    private static JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

    private static List<ScenarioRow> ReadScenarioRows(string path) =>
        [.. File.ReadLines(path).Where(line => line.Length > 0).Select(ScenarioRow.Parse)];
}
