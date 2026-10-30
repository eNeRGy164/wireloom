using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns report recovery, completion summaries, and final workflow status.</summary>
internal sealed class MatrixReport(MatrixOptions options, MatrixWorkspace workspace, WorkflowLog log)
{
    /// <summary>Retains available evidence exactly once, even when preparation or execution failed.</summary>
    internal int Complete(MatrixRunOutcome run)
    {
        var hasIssues = run.HasIssues;
        try
        {
            if (!options.CompileOnly && VerifyExecutionSourceIntegrity())
            {
                hasIssues = true;
            }

            var summary = ScenarioSummary.Empty;

            if (run.ExecutionExitCode == 0 && !options.CompileOnly)
            {
                summary = ReadScenarioSummary();
                hasIssues |= summary.Recorded != run.Plan.ExpectedScenarioCount;
            }

            WriteArtifacts(run.Plan.ExpectedScenarioCount);

            var outcome = FinalizationOutcome.Resolve(run.ExecutionExitCode, 0, hasIssues ? 1 : 0, options.CompileOnly);

            if (run.ExecutionExitCode == 0)
            {
                if (options.CompileOnly)
                {
                    PrintCompilationSummary(run.Peers);
                }
                else
                {
                    PrintExchangeSummary(run.Plan, summary, hasIssues);
                    AnnotateCompletion(run.Plan, summary, outcome);
                }
            }

            return outcome.ExitCode;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Failed to generate the wire-compatibility report: {exception.Message}");
            log.Annotate("error", "Wire compatibility report generation failed", exception.Message);

            return FinalizationOutcome.Resolve(run.ExecutionExitCode, 1, hasIssues ? 1 : 0, options.CompileOnly).ExitCode;
        }
    }

    private void WriteArtifacts(int expectedCount)
    {
        if (!workspace.HasRunArtifacts || !Directory.Exists(options.OutputDirectory) || !File.Exists(workspace.Scenarios))
        {
            return;
        }

        if (options.CompileOnly)
        {
            if (File.Exists(workspace.CppResults))
            {
                File.Copy(workspace.CppResults, workspace.Output("cpp-compilation.tsv"), overwrite: true);
            }

            return;
        }

        WireReportRenderer.Render(new ReportRequest(
            RtiVersion: options.RtiVersion,
            ExpectedScenarioCount: expectedCount,
            ScenarioPath: workspace.Scenarios,
            JsonPath: workspace.Output("results.json"),
            MarkdownPath: workspace.Output("results.md"),
            LogsDirectory: workspace.Logs,
            ExclusionsPath: workspace.Exclusions,
            ManifestPath: workspace.Manifest,
            RepositoryRoot: options.RepositoryRoot,
            ExecutionProvenancePath: workspace.ExecutionProvenance));
    }

    private bool VerifyExecutionSourceIntegrity()
    {
        if (!workspace.HasRunArtifacts || !File.Exists(workspace.ExecutionProvenance))
        {
            return false;
        }

        var provenance = SourceSnapshotEvidence.Read(workspace.ExecutionProvenance);
        if (provenance.MatchesCurrent(options.RepositoryRoot))
        {
            return !provenance.VerifiedUnchanged;
        }

        provenance = provenance with
        {
            VerifiedUnchanged = false,
            IntegrityFailure = "The dirty source manifest differs from the checkout captured before peer builds.",
        };
        provenance.Write(workspace.ExecutionProvenance);

        var rows = File.ReadLines(workspace.Scenarios).Where(line => line.Length > 0)
            .Select(ScenarioRow.Parse)
            .Select(row => row.Status == "PASS" ? row with { Status = "FAIL" } : row)
            .ToArray();
        File.WriteAllLines(workspace.Scenarios, rows.Select(row => row.ToTsv()));
        log.Annotate("error", "Wire source changed during execution", provenance.IntegrityFailure);
        return true;
    }

    private ScenarioSummary ReadScenarioSummary()
    {
        var rows = File.ReadLines(workspace.Scenarios).Where(line => line.Length > 0).ToArray();

        return new ScenarioSummary(rows.Length,
            rows.Count(line => line.Split('\t').ElementAtOrDefault(8) == "PASS"),
            rows.Count(line => line.Split('\t').ElementAtOrDefault(8) == "FAIL"));
    }

    private void PrintCompilationSummary(PeerPreparationResult peers)
    {
        var compiled = peers.NativeOutcomes.Count(outcome => outcome.IsCompiled);

        Console.WriteLine($"RTI C++ compile sweep: {compiled} compiled, {peers.NativeOutcomes.Count - compiled} failed; see {options.OutputDirectory}/cpp-compilation.tsv.");
    }

    private void PrintExchangeSummary(MatrixPlan plan, ScenarioSummary summary, bool hasIssues)
    {
        if (summary.Recorded != plan.ExpectedScenarioCount)
        {
            Console.Error.WriteLine($"Expected {plan.ExpectedScenarioCount} scenarios but recorded {summary.Recorded}.");
        }

        if (hasIssues)
        {
            Console.WriteLine($"Matrix completed with scenario or peer-build issues in {log.ElapsedSeconds:F0}s; see the generated report.");
        }
        else
        {
            Console.WriteLine($"Matrix completed without scenario or peer-build issues in {log.ElapsedSeconds:F0}s.");
        }
    }

    private void AnnotateCompletion(MatrixPlan plan, ScenarioSummary summary, FinalizationOutcome outcome)
    {
        if (outcome.Annotation == "warning")
        {
            log.Annotate("warning", "Wire compatibility matrix completed with issues", $"{summary.Passed} of {plan.ExpectedScenarioCount} scenario exchanges passed; {summary.Failed} failed; see the generated report.");
        }
        else
        {
            log.Annotate("notice", "Wire compatibility matrix passed", $"{summary.Passed} of {plan.ExpectedScenarioCount} scenario exchanges passed in {log.ElapsedSeconds:F0}s.");
        }
    }

    private sealed record ScenarioSummary(int Recorded, int Passed, int Failed)
    {
        internal static ScenarioSummary Empty { get; } = new(0, 0, 0);
    }
}
