using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks that the post-upload workflow gate fails on the first incomplete or failed row.</summary>
internal static class WireMatrixGateVerification
{
    internal static void Run(VerificationContext context)
    {
        var root = Path.Combine(context.Root, "wire-matrix-gate");
        Directory.CreateDirectory(root);
        var reportPath = Path.Combine(root, "results.json");

        // Arrange / Act: a complete passing report clears the final workflow gate.
        File.WriteAllText(reportPath, Report([Row("01-pass", "PASS"), Row("02-pass", "PASS")]));
        var success = context.Invoke(succeeds: true, "check-results", reportPath);
        context.Check(success.Contains("All 2 combined wire rows passed.", StringComparison.Ordinal),
            "The final wire-result gate accepts a complete report with only PASS rows.");

        // Arrange / Act: retain subsequent rows after an issue to prove the gate names the first one.
        File.WriteAllText(reportPath, Report([
            Row("01-pass", "PASS"),
            Row("02-fail", "FAIL"),
            Row("03-not-run", "NOT_RUN"),
        ]));
        var failure = context.Invoke(succeeds: false, "check-results", reportPath);
        context.Check(failure.Contains("First non-passing wire row: FAIL 02-fail [fixture].", StringComparison.Ordinal),
            "The final wire-result gate fails immediately on the first non-passing row and identifies it.");

        // Assert: missing report rows fail even if every present row passed.
        File.WriteAllText(reportPath, Report([Row("01-pass", "PASS")], expectedCount: 2));
        var incomplete = context.Invoke(succeeds: false, "check-results", reportPath);
        context.Check(incomplete.Contains("contains 1 rows; expected 2", StringComparison.Ordinal),
            "The final wire-result gate rejects reports that omit planned rows.");

        // Assert: CI retains evidence before applying the gate, and the gate runs after the upload.
        var workflow = File.ReadAllText(Path.Combine(context.RepositoryRoot, ".github", "workflows", "wire-compatibility.yml"));
        var upload = workflow.IndexOf("- name: Upload combined wire evidence", StringComparison.Ordinal);
        var gate = workflow.IndexOf("-- check-results", StringComparison.Ordinal);
        context.Check(upload >= 0 && gate > upload
            && workflow.IndexOf("if: success()", upload, gate - upload, StringComparison.Ordinal) >= 0,
            "The workflow uploads combined evidence before failing on a non-passing row.");
    }

    private static JsonObject Row(string caseId, string status) => new()
    {
        ["case"] = caseId,
        ["fixture"] = "fixture",
        ["status"] = status,
    };

    private static string Report(JsonObject[] rows, int? expectedCount = null) => new JsonObject
    {
        ["expectedScenarioCount"] = expectedCount ?? rows.Length,
        ["scenarioCount"] = rows.Length,
        ["scenarios"] = new JsonArray(rows.Select(row => (JsonNode)row).ToArray()),
    }.ToJsonString();
}
