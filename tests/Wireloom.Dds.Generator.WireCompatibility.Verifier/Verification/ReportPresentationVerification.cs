using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks profile-specific report wording, adaptations, and compact markdown rendering.</summary>
internal sealed class ReportPresentationVerification
{
    internal ShardReportArtifacts Run(VerificationContext context, MergedReportFixture reports)
    {
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var merged = reports.MergedDirectory;
        var baseline = reports.BaselineJson;

        void Check(bool condition, string description) => context.Check(condition, description);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        string FindReportRow(string markdown, string caseId, string fixture) => VerificationContext.FindReportRow(markdown, caseId, fixture);

        // Assert: endpoint notes describe the selected release, without leaking another profile's defect rationale.
        var otherVersion = rtiVersion == "7.3.1" ? "7.7.0" : "7.3.1";

        foreach (var scenario in ReadJson(baseline)["scenarios"]!.AsArray())
        {
            foreach (var role in new[] { "writer", "reader" })
            {
                var adaptation = (string?)scenario![role]!["adaptation"] ?? "";
                Check(!adaptation.Contains(otherVersion, StringComparison.Ordinal),
                    "Endpoint adaptations do not attribute another release's defects to the selected profile.");
            }
        }

        // Assert: typed fixture construction is normal on both profiles.
        foreach (var caseId in new[] { "01-boundaries", "01-full-widths", "09-defaults-ranges" })
        {
            var scalarScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
                .Where(item => (string?)item!["case"] == caseId).ToArray();
            Check(scalarScenarios.Length == 9, "All scalar adaptation pairings remain in the report.");

            foreach (var scenario in scalarScenarios)
            {
                foreach (var role in new[] { "writer", "reader" })
                {
                    var peer = scenario![role]!;
                    var adaptation = (string?)peer["adaptation"] ?? "";
                    Check(adaptation.Length == 0, "Scalar and constructor fixtures are not wire adaptations on either profile.");
                }
            }
        }

        var scalarMarkdown = File.ReadAllText(Path.Combine(merged, "results.md"));

        // Assert: compact presentation keeps the profile and affected peers explicit.
        Check(scalarMarkdown.StartsWith($"# {rtiVersion} Wire compatibility results\n", StringComparison.Ordinal),
            "The report title identifies the tested profile.");

        var header = scalarMarkdown.Split('\n').Single(line => line.StartsWith("| Corpus feature | Variation |", StringComparison.Ordinal));
        Check(header.Contains("🧵 → 🧵", StringComparison.Ordinal)
            && !header.Contains("Wireloom C#", StringComparison.Ordinal),
            "Table headings use the compact Wireloom symbol without changing RTI peer labels.");

        var boundaryRow = FindReportRow(scalarMarkdown, "01-boundaries", "non-default");
        Check(!boundaryRow.Contains("Typed fixture:", StringComparison.Ordinal),
            "Normal generated sample construction is not described as a wire workaround.");

        Check(scalarMarkdown.Contains("`-funsigned-char`", StringComparison.Ordinal)
            && scalarMarkdown.Contains("`UnionMember` label: `255` → `-1`", StringComparison.Ordinal)
            && scalarMarkdown.Contains("`0xFF`", StringComparison.Ordinal),
            "Compiler flags, API identifiers, and discriminator values use code spans.");

        var recursiveRow = FindReportRow(scalarMarkdown, "09-optional-recursive-array", "optional-absent");
        Check(recursiveRow.Contains("**Wireloom, RTI C#, RTI C++:**", StringComparison.Ordinal)
            && recursiveRow.Split("pool_buffer_max_size=4096").Length == 2
            && recursiveRow.Contains("**RTI C#:** Bootstrap", StringComparison.Ordinal)
            && recursiveRow.Contains("**RTI C++:** Forward", StringComparison.Ordinal),
            "Common recursive setup is grouped while peer-specific corrections remain distinct.");

        Check(scalarMarkdown.Contains("`NullReferenceException`", StringComparison.Ordinal),
            "Technical exception names in explanations use code spans.");

        Check(scalarMarkdown.Contains("“retained symptom: NullReferenceException”", StringComparison.Ordinal),
            "Observed error bodies are quoted without changing their content.");

        Check(!scalarMarkdown.Contains("``", StringComparison.Ordinal),
            "Existing code spans are not formatted a second time.");

        Check(!scalarMarkdown.Contains("leave `color` untouched", StringComparison.Ordinal),
            "Constructor fixture setup is not a compatibility workaround.");

        Check(!scalarMarkdown.Contains("negative `int8` values", StringComparison.Ordinal),
            "Scalar fixture assignment is not a compatibility workaround.");

        var sequenceScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] is "05-unbounded-sequence-aliases" or "07-union-string-sequence-aliases")
            .ToArray();
        Check(sequenceScenarios.Length == 72 && sequenceScenarios.All(item =>
            new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
            "Normal generated-sample construction is not an adaptation for any peer in either sequence case.");

        var optionalAggregateScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == "09-optional-aggregate-member").ToArray();
        Check(optionalAggregateScenarios.Length == 8 && optionalAggregateScenarios.All(item =>
            new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
            "Ordinary optional aggregate fixture construction is not an RTI workaround.");

        foreach (var caseId in new[] { "04-strings", "04-boundaries", "04-unbounded-string-aliases",
            "04-string-alias-chains", "07-union-string-sequence-bounded-direct",
            "02-multiple", "03-enum-values-prefix", "03-enum-values-explicit" })
        {
            var normalScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
                .Where(item => (string?)item!["case"] == caseId).ToArray();
            Check(normalScenarios.Length > 0 && normalScenarios.All(item =>
                new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
                "Case-specific typed fixtures and wrappers do not imply an RTI wire workaround.");
        }

        var workaroundDocument = File.ReadAllText(Path.Combine(repositoryRoot, "tests",
            "Wireloom.Dds.Generator.WireCompatibility", "WORKAROUNDS.md"));
        var workaroundAnchors = Regex.Matches(workaroundDocument, "(?m)^## (.+)$")
            .Select(heading => Regex.Replace(heading.Groups[1].Value.Trim().ToLowerInvariant(), @"[^\w\s-]", "")
                .Replace(' ', '-')).ToHashSet(StringComparer.Ordinal);
        workaroundAnchors.UnionWith(Regex.Matches(workaroundDocument, "<a id=\"(?<anchor>[^\"]+)\"></a>")
            .Select(anchor => anchor.Groups["anchor"].Value));

        var reportGuideLinks = Regex.Matches(scalarMarkdown, @"WORKAROUNDS\.md#(?<anchor>[^)]+)");
        Check(reportGuideLinks.Count > 0 && reportGuideLinks.All(link => workaroundAnchors.Contains(link.Groups["anchor"].Value)),
            "Every case-specific report link resolves to a workaround section.");
        Check(!scalarMarkdown.Contains("Typed C++ fixture", StringComparison.Ordinal),
            "Normal typed C++ fixture paths do not create workaround annotations.");

        var numericScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] is "07-union-char-numeric" or "07-union-char-numeric-255").ToArray();
        Check(numericScenarios.Length == 36 && numericScenarios.All(item => item is { } scenario
            && new[] { "writer", "reader" }.Where(role => (string?)scenario[role]?["peer"] == "RTI C++")
                .All(role => (scenario[role]?["adaptation"] is not null) == ((string?)scenario["case"] == "07-union-char-numeric-255"))),
            "Only the high-label C++ binding needs compiler and label-metadata corrections.");

        Check(scalarMarkdown.Contains("payload `13702` unchanged", StringComparison.Ordinal),
            "Metadata corrections preserve the high-label fixture payload.");

        var recursiveScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == "09-optional-recursive-array").ToArray();
        Check(recursiveScenarios.Length == 27, "All recursive array exchanges are retained.");

        foreach (var scenario in recursiveScenarios)
        {
            foreach (var role in new[] { "writer", "reader" })
            {
                var peer = scenario![role]!;
                var adaptation = (string?)peer["adaptation"] ?? "";
                Check(adaptation.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
                    "Every recursive peer reports the writer fast-pool threshold.");
                Check(adaptation.Contains("Bootstrap", StringComparison.Ordinal) == ((string?)peer["peer"] == "RTI C#"),
                    "Only the RTI C# reference reports a recursive support bootstrap.");
            }
        }

        Check(scalarMarkdown.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
            "The Markdown report discloses the recursive writer memory adaptation.");

        Check(ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] != "09-optional-recursive-array")
            .All(item => new[] { "writer", "reader" }.All(role =>
                !((string?)item![role]!["adaptation"] ?? "").Contains("pool_buffer_max_size", StringComparison.Ordinal))),
            "The writer memory adaptation is scoped to recursive-array scenarios.");

        Check(File.ReadAllText(Path.Combine(root, "baseline.md")) == File.ReadAllText(Path.Combine(merged, "results.md")), "Combined Markdown equals the unsharded report.");

        var unsignedScenarios = ReadJson(Path.Combine(merged, "results.json"))["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == "07-union-ulong").ToArray();
        Check(unsignedScenarios.Count(item => (string?)item!["status"] == "FAIL"
            && ((string?)item["observedProblem"])!.Contains("CS0221", StringComparison.Ordinal)) == 15,
            "All RTI C# build-blocked pairings retain failure status and compiler evidence.");

        Check(unsignedScenarios.Count(item => (string?)item!["status"] == "PASS") == 12,
            "Wireloom and C++ controls remain passing when the RTI C# peer cannot compile.");

        var markdownLines = File.ReadAllLines(Path.Combine(merged, "results.md"));
        var compilerRows = markdownLines.Where(line => line.Contains("CS0221", StringComparison.Ordinal)).ToArray();
        var compilerPairings = new[]
        {
            "Wireloom → RTI C#",
            "RTI C# → Wireloom",
            "RTI C# → RTI C#",
            "RTI C# → RTI C++",
            "RTI C++ → RTI C#",
        };

        Check(compilerRows.Length == 3 && compilerRows.All(line => line.Split("CS0221").Length == 2
            && compilerPairings.All(pairing => line.Contains(pairing, StringComparison.Ordinal))),
            "Each row shows one compiler message and identifies all five affected pairings.");

        var arrayRow = FindReportRow(string.Join('\n', markdownLines), "05-array-of-sequences", "default");
        Check(arrayRow.Split("retained symptom").Length == 2
            && arrayRow.Contains("**Wireloom → RTI C++, RTI C++ → Wireloom:**", StringComparison.Ordinal),
            "Pairings with the same observed failure are grouped together.");

        var report = ReadJson(Path.Combine(merged, "results.json"));
        var aliasScenarios = report["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == "10-resolve-name-wstring-aliases").ToArray();
        Check(aliasScenarios.Length == 18 && aliasScenarios.All(item => (string?)item!["status"] == "PASS"),
            "C147 reports all 18 empty and boundary exchanges passing across its three peers.");

        var aliasReport = File.ReadAllText(Path.Combine(merged, "results.md"));
        Check(aliasReport.Contains("Workaround W015", StringComparison.Ordinal)
            && aliasReport.Contains("Post-generation namespace correction", StringComparison.Ordinal)
            && aliasReport.Contains("Post-generation `TypeCode` correction", StringComparison.Ordinal),
            "C147 report rows link both guarded RTI peer corrections.");
        Check(report is { } reportNode
            && reportNode["expectedFailures"] is null && reportNode["expectedFailureCount"] is null
            && reportNode["scenarios"]!.AsArray().All(item => item!["expectedFailure"] is null),
            "JSON reports contain no expected-failure classification.");
        Check(!File.ReadAllText(Path.Combine(root, "baseline.md")).Contains("Expected failure", StringComparison.Ordinal)
            && !File.ReadAllText(Path.Combine(root, "baseline.md")).Contains("Unexpected failure", StringComparison.Ordinal),
            "Markdown reports use one failure status.");

        return new ShardReportArtifacts(scalarMarkdown, numericScenarios);
    }
}
