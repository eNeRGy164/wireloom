using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks honest report behavior for early failures, sparse plans, and unavailable cases.</summary>
internal sealed class ReportLifecycleVerification
{
    internal void Run(VerificationContext context, ShardScenarioFixture scenarios, MergedReportFixture reports)
    {
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var manifestPath = context.ManifestPath;
        var rows = scenarios.Rows;
        var peers = scenarios.Peers;
        var exclusions = scenarios.Exclusions;
        var merged = reports.MergedDirectory;
        var baseline = reports.BaselineJson;

        void Check(bool condition, string description) => context.Check(condition, description);
        string Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        var excludedPeerRow = rows.Single(row => row is { CaseId: "05-array-of-sequences-idl", Fixture: "sequence-empty" }
            && row.Writer == peers[0] && row.Reader == peers[0]) with { Reader = peers[1] };
        var excludedPeerSeed = Path.Combine(root, "excluded-peer.tsv");

        File.WriteAllText(excludedPeerSeed, excludedPeerRow.ToTsv());

        var excludedPeerResult = Invoke(succeeds: false, "render-report", rtiVersion, "1", excludedPeerSeed,
                Path.Combine(root, "excluded-peer.json"), Path.Combine(root, "excluded-peer.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath);
        Check(excludedPeerResult.Contains("excluded peer", StringComparison.Ordinal),
            "Reports reject exchanges outside the variant's declared peer scope.");

        // Arrange / Act / Assert: an early setup failure still produces a truthful NOT_RUN report.
        excludedPeerRow = excludedPeerRow with { Reader = peers[0], Status = "NOT_RUN" };

        File.WriteAllText(excludedPeerSeed, excludedPeerRow.ToTsv());

        var emptyLogs = Path.Combine(root, "empty-logs");

        Directory.CreateDirectory(emptyLogs);

        Invoke(succeeds: true, "render-report", rtiVersion, "1", excludedPeerSeed,
            Path.Combine(root, "early-failure.json"), Path.Combine(root, "early-failure.md"), emptyLogs, exclusions, manifestPath);
        Check((string?)ReadJson(Path.Combine(root, "early-failure.json"))["scenarios"]![0]!["status"] == "NOT_RUN",
            "Missing build provenance never turns an early setup failure into completed qualification.");

        if (rtiVersion == "7.7.0")
        {
            // Arrange: one planned sparse pairing with eight deliberately unplanned cells.
            var sparseRow = rows.Single(row => row is { CaseId: "14-typed-defaults", Fixture: "default" }
                && row.Writer == peers[2] && row.Reader == peers[0])
                with { Status = "NOT_RUN", Fixture = "sparse-omitted" };
            var sparseSeed = Path.Combine(root, "sparse-omitted.tsv");
            File.WriteAllText(sparseSeed, sparseRow.ToTsv());

            var sparseJson = Path.Combine(root, "sparse-omitted.json");
            var sparseMarkdown = Path.Combine(root, "sparse-omitted.md");

            // Act: report the sparse scenario before it has run.
            Invoke(succeeds: true, "render-report", rtiVersion, "1", sparseSeed, sparseJson,
                sparseMarkdown, emptyLogs, exclusions, manifestPath);

            // Assert: only the planned exchange is counted; other cells are N/A, not false missing runs.
            var sparseReport = ReadJson(sparseJson);
            var sparseReportRow = File.ReadAllLines(sparseMarkdown)
                .Single(line => line.Contains("`sparse-omitted`", StringComparison.Ordinal));
            Check((int)sparseReport["scenarioCount"]! == 1
                && (string?)sparseReport["scenarios"]![0]!["status"] == "NOT_RUN"
                && sparseReportRow.Split('|').Count(cell => cell.Trim() == "N/A") == 8
                && sparseReportRow.Contains("**RTI C++ → Wireloom:**", StringComparison.Ordinal)
                && !sparseReportRow.Contains("Wireloom → Wireloom", StringComparison.Ordinal),
                $"Sparse report rows show only one planned NOT_RUN exchange; row: {sparseReportRow}");
        }

        Check((int)ReadJson(baseline)["notApplicableCount"]! == 1, "Unavailable features are recorded separately from failures.");
        Check(ReadJson(baseline)["scenarios"]!.AsArray().All(item => (string?)item!["case"] != "09-must-understand"), "Unavailable features add no exchanges to the denominator.");
        Check(File.ReadAllText(Path.Combine(merged, "results.md")).Contains("Not applicable", StringComparison.Ordinal), "The Markdown report explains unavailable features.");

        var unavailableSeed = Path.Combine(root, "unavailable.tsv");
        var unavailableRow = rows[0] with { CaseId = "09-must-understand" };
        File.WriteAllText(unavailableSeed, unavailableRow.ToTsv());

        var unavailableResult = Invoke(succeeds: false, "render-report", rtiVersion, "1", unavailableSeed,
            Path.Combine(root, "unavailable.json"), Path.Combine(root, "unavailable.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath);

        Check(unavailableResult.Contains("not applicable", StringComparison.Ordinal), "Reports reject an exchange for an unavailable feature instead of counting it.");
    }
}
