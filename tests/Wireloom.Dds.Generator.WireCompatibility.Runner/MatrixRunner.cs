namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Application lifecycle: prepare the environment, plan, build peers, execute, and retain evidence.</summary>
internal sealed class MatrixRunner(
    MatrixOptions options,
    MatrixWorkspace workspace,
    RtiEnvironment environment,
    MatrixPlanner planner,
    PeerPreparation preparation,
    ExchangeRunner exchanges,
    MatrixReport report,
    WorkflowLog log)
{
    /// <summary>Runs the application and finalizes available evidence after success or failure.</summary>
    internal async Task<int> RunAsync()
    {
        var plan = MatrixPlan.Empty;
        var peers = PeerPreparationResult.Empty;
        var executionExitCode = 0;
        var exchangeIssues = false;

        try
        {
            options.Validate();
            environment.Initialize();
            workspace.Initialize();

            log.StartGroup("Prepare wire compatibility matrix");
            plan = planner.Create();
            peers = await preparation.PrepareAsync(plan);
            exchangeIssues = await exchanges.RunAsync(plan, peers);
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception is ArgumentException ? exception.Message : $"Wire matrix failed: {exception.Message}");

            log.Annotate("error", "Wire compatibility matrix failed", exception.Message);

            executionExitCode = 2;
        }
        finally
        {
            log.EndGroup();
        }

        return report.Complete(new MatrixRunOutcome(plan, peers, executionExitCode, exchangeIssues));
    }
}
