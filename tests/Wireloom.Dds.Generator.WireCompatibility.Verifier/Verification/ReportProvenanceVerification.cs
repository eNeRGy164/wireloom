using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks merged scenario identity and generation provenance constraints.</summary>
internal sealed class ReportProvenanceVerification
{
    internal void Run(VerificationContext context, ShardScenarioFixture scenarios, MergedReportFixture reports)
    {
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var manifestPath = context.ManifestPath;
        var rows = scenarios.Rows;
        var seed = scenarios.Seed;
        var exclusions = scenarios.Exclusions;
        var merged = reports.MergedDirectory;
        var baseline = reports.BaselineJson;

        void Check(bool condition, string description) => context.Check(condition, description);
        string Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        void ChangeJson(string path, Action<JsonNode> change) => VerificationContext.ChangeJson(path, change);

        // Assert: payload/report evidence survives, including registry entries and failure reasons.
        Check(JsonNode.DeepEquals(ReadJson(baseline), ReadJson(Path.Combine(merged, "results.json"))), "Combined JSON equals the unsharded report.");

        var idlScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == "05-array-of-sequences-idl").ToArray();
        Check(idlScenarios.Length == 16 && idlScenarios.All(item =>
                (string?)item!["generationProvenance"]!["sequenceArrayMapping"] == "Idl"
                && (string?)item["generationProvenance"]!["rtiVersion"] == rtiVersion
                && item["generationProvenance"]!["generatedSources"]!.AsArray().Count > 0),
            "All variant fixtures preserve mode, runtime, and source provenance through merging.");

        var manifestWrapperIds = ReadJson(manifestPath)["positiveCases"]!.AsArray()
            .Select(item => (string)item!["id"]!)
            .Where(caseId => File.Exists(Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers",
                caseId switch
                {
                    "14-autoid-scope" => "14-autoid-scope-topic.idl",
                    "14-enum-constant" => "14-enum-constant-topic.idl",
                    _ => caseId + ".idl"
                })))
            .ToHashSet(StringComparer.Ordinal);

        var wrapperScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => manifestWrapperIds.Contains((string)item!["case"]!)).ToArray();
        Check(wrapperScenarios.Length > 18 && wrapperScenarios.All(item =>
                (string?)item!["generationProvenance"]!["rtiVersion"] == rtiVersion
                && ((string?)item["generationProvenance"]!["oracleIdl"])!.StartsWith(
                    "tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/", StringComparison.Ordinal)
                && item["generationProvenance"]!["oracleDependencies"]!.AsArray().Count > 0
                && item["generationProvenance"]!["generatedSources"]!.AsArray().Count > 0),
            "Manifest wrapper exchanges retain validated wrapper, dependency, runtime, and generated-source provenance.");

        var autoIdProvenance = ReadJson(Path.Combine(merged, "logs", "14-autoid-scope-generation.json"));
        Check(autoIdProvenance["oracleDependencies"]!.AsArray().Any(item =>
                (string?)item!["path"] == "docs/corpus/idl/features/14-autoid-scope.idl")
            && autoIdProvenance["oracleDependencies"]!.AsArray().Any(item =>
                (string?)item!["path"] == "docs/corpus/idl/includes/autoid-hash-include.idl"),
            "C164 wrapper provenance includes the canonical source and its transitive include dependency.");

        var idlMarkdownRows = File.ReadAllLines(Path.Combine(merged, "results.md"))
            .Where(line =>
            {
                var columns = line.Split('|', StringSplitOptions.TrimEntries);
                return columns.Length > 2
                    && columns[1].Contains("`05-array-of-sequences-idl`", StringComparison.Ordinal)
                    && columns[2] is "`sequence-empty`" or "`sequence-unequal`" or "`sequence-mixed-empty`" or "`sequence-boundary`";
            })
            .ToArray();
        Check(idlMarkdownRows.Length == 4 && idlMarkdownRows.All(line =>
                line.Split("N/A").Length == 6
                && !line.Contains("❔", StringComparison.Ordinal)),
            "Each IDL-preserving fixture reports five excluded RTI C# pairings as N/A rather than missing exchanges.");

        // Arrange / Act / Assert: missing or different generation provenance must not qualify.
        var provenanceFile = Path.Combine(merged, "logs", "05-array-of-sequences-idl-generation.json");
        var originalProvenance = File.ReadAllText(provenanceFile);

        foreach (var field in new[] { "sequenceArrayMapping", "rtiVersion", "idlSha256", "idl" })
        {
            ChangeJson(provenanceFile, node => node[field] = "incorrect");
            Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                    Path.Combine(root, "bad-mode.json"), Path.Combine(root, "bad-mode.md"),
                    Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match", StringComparison.Ordinal),
                "Reports reject mismatched generation provenance.");
            File.WriteAllText(provenanceFile, originalProvenance);
        }

        ChangeJson(provenanceFile, node => node["generatedSources"] = new JsonArray());
        Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                Path.Combine(root, "missing-sources.json"), Path.Combine(root, "missing-sources.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match", StringComparison.Ordinal),
            "Reports reject provenance without generated sources.");
        File.WriteAllText(provenanceFile, originalProvenance);

        var wrapperProvenanceFile = Path.Combine(merged, "logs", "14-autoid-scope-generation.json");
        var originalWrapperProvenance = File.ReadAllText(wrapperProvenanceFile);
        File.Delete(wrapperProvenanceFile);
        Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                Path.Combine(root, "missing-wrapper-provenance.json"), Path.Combine(root, "missing-wrapper-provenance.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance is missing for 14-autoid-scope", StringComparison.Ordinal),
            "Reports reject completed wrapper exchanges when generation provenance is missing.");
        File.WriteAllText(wrapperProvenanceFile, originalWrapperProvenance);

        ChangeJson(wrapperProvenanceFile, node =>
            node["oracleDependencies"]!.AsArray()[0]!["sha256"] = "incorrect");
        Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                Path.Combine(root, "bad-wrapper-dependency.json"), Path.Combine(root, "bad-wrapper-dependency.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match 14-autoid-scope", StringComparison.Ordinal),
            "Reports reject wrapper provenance with a mismatched dependency hash.");
        File.WriteAllText(wrapperProvenanceFile, originalWrapperProvenance);

        File.Delete(provenanceFile);
        Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                Path.Combine(root, "missing-mode.json"), Path.Combine(root, "missing-mode.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance is missing", StringComparison.Ordinal),
            "Reports reject completed variant exchanges without generation provenance.");
        File.WriteAllText(provenanceFile, originalProvenance);
    }
}
