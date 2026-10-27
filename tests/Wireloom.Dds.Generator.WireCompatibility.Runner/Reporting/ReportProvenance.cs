using System.Security.Cryptography;
using System.Text.Json.Nodes;
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Validates retained generation evidence before exchanges qualify in a report.</summary>
internal static class ReportProvenance
{
    /// <summary>Attaches validated configuration and provenance, rejecting missing or mismatched completed evidence.</summary>
    internal static void Attach(ReportFields scenario, JsonNode corpusCase, ReportRequest request, ScenarioRow row)
    {
        var id = row.CaseId;
        var status = row.Status;
        var repository = request.RepositoryRoot;
        var version = request.RtiVersion;
        var logsPath = request.LogsDirectory;
        var hasSequenceArrayMapping = corpusCase["sequenceArrayMapping"] is not null;
        if (hasSequenceArrayMapping)
        {
            scenario["generationConfiguration"] = new
            {
                idl = (string)corpusCase["idl"]!,
                sequenceArrayMapping = (string)corpusCase["sequenceArrayMapping"]!
            };
        }

        var wrapperCandidate = IdlSourceResolver.WrapperPath(repository, id);
        var requiresGenerationProvenance = hasSequenceArrayMapping || File.Exists(wrapperCandidate);
        if (requiresGenerationProvenance)
        {
            var provenancePath = Path.Combine(logsPath, $"{id}-generation.json");
            if (File.Exists(provenancePath))
            {
                var provenance = ReadJson(provenancePath);
                var canonicalIdl = Path.GetFullPath(Path.Combine(repository, "docs", "corpus", (string)corpusCase["idl"]!));
                var oracleIdl = IdlSourceResolver.ResolveOracleIdl(repository, id, canonicalIdl);
                var oracleDependencies = IdlSourceResolver.ResolveIncludedIdlFiles(oracleIdl,
                    Path.Combine(repository, "docs", "corpus", "idl"), Path.Combine(repository, "docs", "corpus", "idl", "includes"));
                var expectedSequenceArrayMapping = (string?)corpusCase["sequenceArrayMapping"] ?? "RtiCSharp";
                var recordedDependencies = provenance["oracleDependencies"] as JsonArray;
                var dependenciesMatch = recordedDependencies is not null
                    && recordedDependencies.Count == oracleDependencies.Length
                    && oracleDependencies.Select((path, index) =>
                        (string?)recordedDependencies[index]?["path"] == Path.GetRelativePath(repository, path).Replace('\\', '/')
                        && (string?)recordedDependencies[index]? ["sha256"] == Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))))
                        .All(matches => matches);

                if ((string?)provenance["sequenceArrayMapping"] != expectedSequenceArrayMapping
                    || (string?)provenance["rtiVersion"] != version
                    || (string?)provenance["idl"] != (string)corpusCase["idl"]!
                    || (string?)provenance["idlSha256"] != Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(
                        canonicalIdl)))
                    || (string?)provenance["oracleIdl"] != Path.GetRelativePath(repository, oracleIdl).Replace('\\', '/')
                    || (string?)provenance["oracleIdlSha256"] != Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(oracleIdl)))
                    || !dependenciesMatch
                    || provenance["generatedSources"] is not JsonArray { Count: > 0 })
                {
                    throw new InvalidDataException($"Generation provenance does not match {id} and RTI {version}.");
                }

                scenario["generationProvenance"] = provenance;
            }
            else if (status != "NOT_RUN")
            {
                throw new InvalidDataException($"Generation provenance is missing for {id}.");
            }
        }
    }

    private static JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;
}
