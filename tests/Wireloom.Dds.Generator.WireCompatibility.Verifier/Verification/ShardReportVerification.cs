namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Coordinates report creation and verifies its provenance, lifecycle, and presentation contracts.</summary>
internal static class ShardReportVerification
{
    internal static ShardReportArtifacts Run(VerificationContext context, ShardScenarioFixture scenarios)
    {
        var reports = MergedReportBuilder.Build(context, scenarios);

        new ReportProvenanceVerification().Run(context, scenarios, reports);
        new ReportLifecycleVerification().Run(context, scenarios, reports);

        return new ReportPresentationVerification().Run(context, reports);
    }
}
