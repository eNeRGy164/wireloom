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

        // Act: compare the merged report with an unsharded rendering of the same rows/logs.
        var merged = Path.Combine(root, "merged");
        Invoke(succeeds: true, "merge-shards", rtiVersion, "4", source, merged, exclusions);

        var baseline = Path.Combine(root, "baseline.json");
        Invoke(succeeds: true, "render-report", rtiVersion, rows.Count.ToString(), seed, baseline,
            Path.Combine(root, "baseline.md"), Path.Combine(merged, "logs"), exclusions,
            Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));

        return new MergedReportFixture(merged, baseline);
    }
}
