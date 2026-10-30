using System.Text.Json.Nodes;
using System.Security.Cryptography;
using System.Text;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks shard allocation, malformed plans, and reference-adaptation behavior.</summary>
internal static class ShardIntegrityVerification
{
    internal static void Run(VerificationContext context, ShardScenarioFixture fixture, ShardReportArtifacts report)
    {
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var rows = fixture.Rows;
        var cases = fixture.Cases;
        var source = fixture.Source;
        var seed = fixture.Seed;
        var scalarMarkdown = report.ScalarMarkdown;
        var numericScenarios = report.NumericScenarios;

        void Check(bool condition, string description) => context.Check(condition, description);
        void Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        void ChangeJson(string path, Action<JsonNode> change) => VerificationContext.ChangeJson(path, change);
        void ChangeRows(string directory, Func<string[], string[]> change) => VerificationContext.ChangeRows(directory, change);

        var shardRows = Enumerable.Range(0, 4).Select(index => File.ReadAllLines(Path.Combine(source, $"shard-{index}", "scenarios.tsv"))).ToArray();
        Check(shardRows.Sum(items => items.Length) == rows.Count, "Every exchange is retained exactly once.");

        var caseSets = shardRows.Select(items => items.Select(line => line.Split('\t')[0]).ToHashSet()).ToArray();
        Check(caseSets.Sum(items => items.Count) == cases.Count && caseSets.SelectMany(items => items).Distinct().Count() == cases.Count, "Whole cases belong to exactly one shard.");

        Check(shardRows.Max(items => items.Length) - shardRows.Min(items => items.Length) <= rows.GroupBy(row => row.CaseId).Max(group => group.Count()), "Exchange counts are balanced within one indivisible case.");

        var repeat = Path.Combine(root, "repeat");
        Invoke(succeeds: true, "partition-shard", rtiVersion, "0", "4", seed, repeat);
        Check(JsonNode.DeepEquals(ReadJson(Path.Combine(source, "shard-0", "plan.json")), ReadJson(Path.Combine(repeat, "plan.json"))), "Independent plans are deterministic.");

        ExpectMergeFailure(context, fixture, "missing-shard", directory => Directory.Move(Path.Combine(directory, "shard-1"), Path.Combine(root, "removed-shard")));
        ExpectMergeFailure(context, fixture, "duplicate-index", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["shardIndex"] = 0));
        ExpectMergeFailure(context, fixture, "different-plan", directory => ChangeJson(Path.Combine(directory, "shard-1", "plan.json"), node => node["expectedScenarioCount"] = 999));
        ExpectMergeFailure(context, fixture, "wrong-version", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["rtiVersion"] = rtiVersion == "7.7.0" ? "7.3.1" : "7.7.0"));
        ExpectMergeFailure(context, fixture, "missing-exchange", directory => ChangeRows(directory, lines => lines.Skip(1).ToArray()));
        ExpectMergeFailure(context, fixture, "unexecuted-exchange", directory => ChangeRows(directory, lines =>
        {
            var fields = lines[0].Split('\t');
            fields[8] = "NOT_RUN";
            return [string.Join('\t', fields), .. lines.Skip(1)];
        }));
        ExpectMergeFailure(context, fixture, "duplicate-exchange", directory => ChangeRows(directory, lines => [.. lines.Skip(1), lines[1]]));
        ExpectMergeFailure(context, fixture, "unexpected-fixture", directory => ChangeRows(directory, lines => [lines[0] + "-unknown", .. lines.Skip(1)]));
        ExpectMergeFailure(context, fixture, "invalid-status", directory => ChangeRows(directory, lines => [lines[0].Replace("\tPASS\t", "\tUNKNOWN\t"), .. lines.Skip(1)]));
        ExpectMergeFailure(context, fixture, "wrong-report-count", directory => ChangeJson(Path.Combine(directory, "shard-0", "results.json"), node => node["scenarioCount"] = 999));
        ExpectMergeFailure(context, fixture, "different-source-revision", directory => ChangeSourceRevisionIdentity(
            Path.Combine(directory, "shard-1")));
        ExpectMergeFailure(context, fixture, "different-dirty-source", directory => ChangeDirtySourceIdentity(
            Path.Combine(directory, "shard-1")));
        ExpectMergeFailure(context, fixture, "invalid-lifecycle-qos", directory =>
        {
            var log = Directory.GetFiles(directory, "14-unbounded-receive-lifecycle-fragmented-xcdr1-*.log", SearchOption.AllDirectories).First();
            File.WriteAllText(log, File.ReadAllText(log).Replace("fragmentation_enabled=true", "fragmentation_enabled=false", StringComparison.Ordinal));
        });

        Invoke(succeeds: false, "partition-shard", "7.7.0", "4", "4", seed, Path.Combine(root, "invalid-index"));
        context.RecordChecks(1);

        Invoke(succeeds: false, "partition-shard", "7.7.0", "0", "0", seed, Path.Combine(root, "invalid-count"));
        context.RecordChecks(1);

        var duplicateSeed = Path.Combine(root, "duplicate.tsv");
        File.WriteAllLines(duplicateSeed, rows.Select(row => row.ToTsv()).Append(rows[0].ToTsv()));

        Invoke(succeeds: false, "partition-shard", "7.7.0", "0", "4", duplicateSeed, Path.Combine(root, "duplicate-plan"));
        context.RecordChecks(1);

        RtiCharacterReferenceVerification.Run(context);

        Invoke(succeeds: false, "adapt-rti-characters", "csharp", "unrelated-case", root);
        Invoke(succeeds: false, "adapt-rti-characters", "cpp", "07-union-char-numeric", root);
        Invoke(succeeds: false, "adapt-rti-characters", "unknown-language", "07-union-char-numeric-255", root);
        context.RecordChecks(3);

        Check(numericScenarios.All(item => new[] { "writer", "reader" }
            .Where(role => (string?)item![role]!["peer"] == "RTI C#")
            .All(role => ((string?)item![role]!["adaptation"] ?? "").Contains("Post-generation RTI C#", StringComparison.Ordinal))),
            "Every RTI C# numeric-character endpoint discloses its corrected generated binding.");

        Check(scalarMarkdown.Contains("`TypeCode` label: `255` → `-1`", StringComparison.Ordinal)
            && scalarMarkdown.Contains("`Setfallback`", StringComparison.Ordinal),
            "Markdown reports disclose the C++ label and C# discriminator-preservation corrections.");
    }

    private static void ExpectMergeFailure(VerificationContext context, ShardScenarioFixture fixture, string name, Action<string> mutate)
    {
        var directory = Path.Combine(context.Root, name);

        VerificationContext.CopyDirectory(fixture.Source, directory);

        mutate(directory);

        context.Invoke(succeeds: false, "merge-shards", context.RtiVersion, "4", directory, Path.Combine(context.Root, name + "-output"), fixture.Exclusions);
        context.RecordChecks(1);
    }

    private static void ChangeDirtySourceIdentity(string shardDirectory)
    {
        var path = Path.Combine(shardDirectory, "execution-provenance.json");
        var node = VerificationContext.ReadJson(path);
        var retainedPath = Path.Combine("source-files", "identity-change.txt");
        var bytes = "different retained source\n"u8.ToArray();
        var sourceFileHash = Convert.ToHexString(SHA256.HashData(bytes));
        var manifestEntry = Encoding.UTF8.GetBytes($"identity-change.txt\0added\0{sourceFileHash}\n");
        node["dirtySnapshotSha256"] = Convert.ToHexString(SHA256.HashData(manifestEntry));
        node["dirtyFileCount"] = 1;
        node["files"] = new JsonArray(new JsonObject
        {
            ["path"] = "identity-change.txt",
            ["status"] = "added",
            ["sha256"] = sourceFileHash,
            ["retainedPath"] = retainedPath.Replace('\\', '/'),
        });
        var retainedFile = Path.Combine(shardDirectory, retainedPath);
        Directory.CreateDirectory(Path.GetDirectoryName(retainedFile)!);
        File.WriteAllBytes(retainedFile, bytes);
        File.WriteAllText(path, node.ToJsonString() + "\n");
        AlignReportIdentity(shardDirectory, (string)node["revision"]!, (string)node["dirtySnapshotSha256"]!, (int)node["dirtyFileCount"]!);
    }

    private static void ChangeSourceRevisionIdentity(string shardDirectory)
    {
        var path = Path.Combine(shardDirectory, "execution-provenance.json");
        var node = VerificationContext.ReadJson(path);
        node["revision"] = "another-revision";
        File.WriteAllText(path, node.ToJsonString() + "\n");
        AlignReportIdentity(shardDirectory, "another-revision", (string)node["dirtySnapshotSha256"]!, (int)node["dirtyFileCount"]!);
    }

    private static void AlignReportIdentity(string shardDirectory, string revision, string snapshotHash, int fileCount)
    {
        var reportPath = Path.Combine(shardDirectory, "results.json");
        var report = VerificationContext.ReadJson(reportPath);
        report["executionProvenance"]!["revision"] = revision;
        report["executionProvenance"]!["dirtySourceSnapshotSha256"] = snapshotHash;
        report["executionProvenance"]!["dirtySourceFileCount"] = fileCount;
        foreach (var scenario in report["scenarios"]!.AsArray())
        {
            scenario!["sourceRevision"] = revision;
            scenario["dirtySourceSnapshotSha256"] = snapshotHash;
            scenario["dirtySourceFileCount"] = fileCount;
        }

        File.WriteAllText(reportPath, report.ToJsonString() + "\n");
    }
}
