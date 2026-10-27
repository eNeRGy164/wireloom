namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Creates the in-memory case/scenario plan and retains its execution and verification artifacts.</summary>
internal sealed class MatrixPlanner(MatrixOptions options, MatrixWorkspace workspace, WorkflowLog log)
{
    /// <summary>Creates the plan once, before any case-specific peer work begins.</summary>
    internal MatrixPlan Create()
    {
        options.Validate();

        if (options.CompileOnly)
        {
            var discovered = CorpusCaseDiscovery.Read(workspace.Manifest, workspace.Exclusions);
            var cases = discovered.Select(options.CaseFilter);

            Console.WriteLine($"RTI Connext {options.RtiVersion}: compiling C++ peers for {cases.Count} cases (wire exchanges disabled).");
            log.EndGroup();

            return new MatrixPlan(cases, discovered.RtiOnlyCases.ToHashSet(StringComparer.Ordinal), Exchanges: null);
        }

        var exchanges = ScenarioPlanBuilder.Create(workspace.Manifest, workspace.Exclusions, options.RtiVersion, options.CaseFilter, options.ShardIndex, options.ShardCount);
        PlanArtifactWriter.Write(exchanges, options.OutputDirectory);

        Console.WriteLine($"Shard {options.ShardIndex}/{options.ShardCount}: {exchanges.CaseIds.Count} cases, {exchanges.Scenarios.Count} scenario exchanges planned.");
        log.EndGroup();

        return new MatrixPlan(exchanges.CaseIds, exchanges.RtiOnlyCases, exchanges);
    }
}
