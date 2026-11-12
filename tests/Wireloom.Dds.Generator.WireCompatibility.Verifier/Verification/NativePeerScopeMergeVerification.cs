using System.Text.Json;
using System.Text.Json.Nodes;
using Wireloom.Dds.Generator.WireCompatibility.Runner;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Verifies explicit native peer scope survives shard report regeneration.</summary>
internal static class NativePeerScopeMergeVerification
{
    internal static void Run(VerificationContext context)
    {
        const string version = "6.1.2";
        const string caseId = "09-optional-collections";
        var root = Path.Combine(context.Root, "native-peer-scope-merge");
        Directory.CreateDirectory(root);
        var fixtures = new[] { "optional-absent", "optional-empty", "optional-single", "optional-multiple" };
        var rows = fixtures.SelectMany(fixture => new[]
        {
            new ScenarioRow(caseId, "CorpusOptionalCollections.Sample", "", "::CorpusOptionalCollections::Sample",
                PeerNames.RtiCpp, version, PeerNames.WireloomCpp, version, "PASS", fixture),
            new ScenarioRow(caseId, "CorpusOptionalCollections.Sample", "", "::CorpusOptionalCollections::Sample",
                PeerNames.WireloomCpp, version, PeerNames.RtiCpp, version, "PASS", fixture),
        }).ToArray();

        var seed = Path.Combine(root, "scenarios.tsv");
        File.WriteAllLines(seed, rows.Select(row => row.ToTsv()));
        new SourceSnapshotEvidence("native-peer-scope-merge-test", SourceSnapshotEvidence.ComputeSnapshotHash([]), 0, [])
            .Write(Path.Combine(root, "execution-provenance.json"));

        var exclusions = Path.Combine(root, "exclusions.json");
        File.WriteAllText(exclusions,
            """
            {
              "notImplementedCases": [],
              "notApplicableCases": [
                { "id": "09-optional-collections", "reason": "Managed peer scope excludes optional collections." }
              ]
            }
            """);

        var source = Path.Combine(root, "shards");
        context.Invoke(succeeds: true, "partition-shard", version, "0", "1", seed, Path.Combine(source, "shard-0"));
        var shard = Path.Combine(source, "shard-0");
        var planPath = Path.Combine(shard, "plan.json");
        var plan = VerificationContext.ReadJson(planPath);
        plan["wireloomCppCases"] = new JsonArray(JsonValue.Create(caseId));
        plan["nativePeerScopeCases"] = new JsonArray(JsonValue.Create(caseId));
        File.WriteAllText(planPath, plan.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");

        var logs = Path.Combine(shard, "logs");
        Directory.CreateDirectory(logs);
        var scenarioPath = Path.Combine(shard, "scenarios.tsv");
        var resultsJson = Path.Combine(shard, "results.json");
        var resultsMarkdown = Path.Combine(shard, "results.md");
        var manifest = Path.Combine(context.RepositoryRoot, "docs", "corpus", "manifest.json");
        context.Invoke(succeeds: true, "render-report", version, rows.Length.ToString(), scenarioPath, resultsJson,
            resultsMarkdown, logs, exclusions, manifest, caseId);

        var mergedOutput = Path.Combine(root, "merged");
        context.Invoke(succeeds: true, "merge-shards", version, "1", source, mergedOutput, exclusions);
        var mergedReport = VerificationContext.ReadJson(Path.Combine(mergedOutput, "results.json"));
        context.Check((int)mergedReport["scenarioCount"]! == rows.Length
            && mergedReport["scenarios"]!.AsArray().All(item => (string?)item!["status"] == "PASS")
            && (string?)mergedReport["notApplicableCases"]![0]!["id"] == caseId,
            "The explicit native peer scope and managed exclusion must survive shard report regeneration.");

        var unselectedSource = Path.Combine(root, "unselected-shards");
        VerificationContext.CopyDirectory(source, unselectedSource);
        var unselectedPlanPath = Path.Combine(unselectedSource, "shard-0", "plan.json");
        var unselectedPlan = VerificationContext.ReadJson(unselectedPlanPath);
        unselectedPlan["nativePeerScopeCases"] = new JsonArray();
        File.WriteAllText(unselectedPlanPath, unselectedPlan.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");

        context.Invoke(succeeds: false, "merge-shards", version, "1", unselectedSource,
            Path.Combine(root, "unselected-output"), exclusions);
        context.RecordChecks(2);
    }
}
