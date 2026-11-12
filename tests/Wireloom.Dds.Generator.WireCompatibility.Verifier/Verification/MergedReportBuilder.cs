namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Creates the merged and unsharded baseline reports used by report assertions.</summary>
internal static class MergedReportBuilder
{
    internal static MergedReportFixture Build(VerificationContext context, ShardScenarioFixture scenarios)
    {
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var rows = scenarios.Rows;
        var seed = scenarios.Seed;
        var exclusions = scenarios.Exclusions;
        var source = scenarios.Source;

        void Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        // Arrange: a reused destination must not carry old logs into this run.
        var merged = Path.Combine(root, "merged");
        var existingLogs = Path.Combine(merged, "logs");
        Directory.CreateDirectory(Path.Combine(existingLogs, "shard-0"));
        File.WriteAllText(Path.Combine(existingLogs, "wireloom-cpp-generator-build.log"), "Existing conflicting merged log.\n");
        File.WriteAllText(Path.Combine(existingLogs, "shard-0", "wireloom-cpp-generator-build.log"), "Existing conflicting shard log.\n");

        // Act: compare the merged report with an unsharded rendering of the same rows/logs.
        Invoke(succeeds: true, "merge-shards", rtiVersion, "4", source, merged, exclusions);

        var retainedBuildLogs = Directory.GetFiles(Path.Combine(merged, "logs"), "wireloom-cpp-generator-build*.log", SearchOption.AllDirectories)
            .Select(File.ReadAllText)
            .ToHashSet(StringComparer.Ordinal);
        var expectedBuildLogs = Enumerable.Range(0, 4)
            .Select(index => $"Synthetic C++ generator build log for shard {index}.\n")
            .ToHashSet(StringComparer.Ordinal);
        context.Check(retainedBuildLogs.SetEquals(expectedBuildLogs),
            "The merged report contains each current shard build log once and excludes stale destination logs.");

        // Act: retrying the same merge into the same destination is idempotent.
        Invoke(succeeds: true, "merge-shards", rtiVersion, "4", source, merged, exclusions);

        // Assert
        var retriedBuildLogs = Directory.GetFiles(Path.Combine(merged, "logs"), "wireloom-cpp-generator-build*.log", SearchOption.AllDirectories)
            .Select(File.ReadAllText)
            .ToHashSet(StringComparer.Ordinal);
        context.Check(retriedBuildLogs.SetEquals(expectedBuildLogs)
            && Directory.GetFiles(Path.Combine(merged, "logs"), "wireloom-cpp-generator-build*.log", SearchOption.AllDirectories).Length == expectedBuildLogs.Count,
            "Repeating a shard merge remains idempotent without stale or duplicate build logs.");

        var baseline = Path.Combine(root, "baseline.json");
        Invoke(succeeds: true, "render-report", rtiVersion, rows.Count.ToString(), seed, baseline,
            Path.Combine(root, "baseline.md"), Path.Combine(merged, "logs"), exclusions,
            Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));

        return new MergedReportFixture(merged, baseline);
    }
}
