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
            var nativePeerScopeCases = WireloomCppScope.ReadExplicitCases(
                options.NativePeerScopeCaseFilter, CorpusCaseDiscovery.ReadPositiveCaseIds(workspace.Manifest));
            var cases = discovered.Select(options.CaseFilter, nativePeerScopeCases.ToHashSet(StringComparer.Ordinal)).ToList();
            RejectPeerScopeOverlap(cases, nativePeerScopeCases);
            cases.AddRange(nativePeerScopeCases);
            var cppCases = WireloomCppScope.ReadExplicitCases(options.WireloomCppCaseFilter, cases);
            cppCases.UnionWith(nativePeerScopeCases);

            Console.WriteLine($"RTI Connext {options.RtiVersion}: compiling C++ peers for {cases.Count} cases and Wireloom C++ peers for {cppCases.Count} explicitly selected cases (wire exchanges disabled).");
            log.EndGroup();

            return new MatrixPlan(cases, discovered.RtiOnlyCases.ToHashSet(StringComparer.Ordinal), Exchanges: null,
                SelectedWireloomCppCases: cppCases,
                SelectedNativePeerScopeCases: nativePeerScopeCases.ToHashSet(StringComparer.Ordinal));
        }

        var exchanges = ScenarioPlanBuilder.Create(workspace.Manifest, workspace.Exclusions, options.RtiVersion, options.CaseFilter,
            options.ShardIndex, options.ShardCount, options.WireloomCppCaseFilter,
            options.NativePeerScopeCaseFilter);
        PlanArtifactWriter.Write(exchanges, options.OutputDirectory);

        Console.WriteLine($"Shard {options.ShardIndex}/{options.ShardCount}: {exchanges.CaseIds.Count} cases, {exchanges.Scenarios.Count} scenario exchanges planned.");
        log.EndGroup();

        return new MatrixPlan(exchanges.CaseIds, exchanges.RtiOnlyCases, exchanges);
    }

    private static void RejectPeerScopeOverlap(IReadOnlyCollection<string> selectedCases, IReadOnlyCollection<string> nativePeerScopeCases)
    {
        var overlap = selectedCases.Intersect(nativePeerScopeCases, StringComparer.Ordinal).ToArray();
        if (overlap.Length > 0)
        {
            throw new InvalidDataException($"Cases selected for the native peer scope must be excluded from the managed case filter: {string.Join(", ", overlap)}.");
        }
    }
}
