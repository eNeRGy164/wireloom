namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>The last completed stages, including context needed to report a partial run.</summary>
/// <param name="Plan">The selected plan, or the empty plan if planning failed.</param>
/// <param name="Peers">Completed peer preparation, or the empty result if preparation failed.</param>
/// <param name="ExecutionExitCode">Fatal application failure; handled peer/scenario failures remain evidence.</param>
/// <param name="ExchangeIssues">Whether the exchange scheduler returned handled failures.</param>
internal sealed record MatrixRunOutcome(MatrixPlan Plan, PeerPreparationResult Peers, int ExecutionExitCode, bool ExchangeIssues)
{
    /// <summary>Whether the completed stages contain peer, fixture, or exchange failures.</summary>
    internal bool HasIssues => Peers.HasIssues || ExchangeIssues;
}

/// <summary>Workflow exit status and annotation severity, independently of scenario evidence.</summary>
/// <param name="ExitCode">Application exit code.</param>
/// <param name="Annotation">GitHub Actions severity.</param>
internal sealed record FinalizationOutcome(int ExitCode, string Annotation)
{
    /// <summary>Preserves fatal setup/report errors and the existing advisory wire-evidence policy.</summary>
    internal static FinalizationOutcome Resolve(int runStatus, int reportStatus, int overallStatus, bool compileOnly)
    {
        if (reportStatus != 0)
        {
            return new FinalizationOutcome(1, "error");
        }

        if (runStatus != 0)
        {
            return new FinalizationOutcome(runStatus, "error");
        }

        if (compileOnly && overallStatus != 0)
        {
            return new FinalizationOutcome(1, "error");
        }

        if (overallStatus != 0)
        {
            return new FinalizationOutcome(0, "warning");
        }

        return new FinalizationOutcome(0, "notice");
    }
}
