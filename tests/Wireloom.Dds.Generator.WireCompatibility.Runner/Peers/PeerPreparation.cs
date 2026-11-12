namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Coordinates case preparation using typed peer artifacts and explicit build outcomes.</summary>
internal sealed class PeerPreparation(MatrixOptions options, MatrixWorkspace workspace, WireloomTypeDiscovery discovery,
    ManagedPeerBuilder managed, NativePeerBuilder native, GenerationProvenanceRecorder provenance, WorkflowLog log)
{
    /// <summary>Builds managed peers, stages each case, then compiles the complete native queue.</summary>
    internal async Task<PeerPreparationResult> PrepareAsync(MatrixPlan plan)
    {
        var managedBatch = await managed.BuildWireloomAsync(plan);
        var hasIssues = managedBatch.HasIssues;

        var prepared = new List<PreparedCase>();
        var unconfiguredCases = new List<BuiltCase>();
        var outcomes = new List<BuildOutcome>();

        for (var index = 0; index < plan.CaseIds.Count; index++)
        {
            var caseId = plan.CaseIds[index];
            log.StartGroup($"Case {index + 1}/{plan.CaseIds.Count}: {caseId}");
            Console.WriteLine($"WIRE_MATRIX_CASE {index + 1}/{plan.CaseIds.Count} case={caseId} phase=peer-prepare completed-scenarios=0/{plan.ExpectedScenarioCount} elapsed={log.ElapsedSeconds:F0}s");

            try
            {
                var item = await PrepareCaseAsync(caseId, plan, managedBatch);
                hasIssues |= item.HasManagedIssues;

                if (!item.Configuration.RtiOutcome.IsConfigured)
                {
                    outcomes.Add(item.Configuration.RtiOutcome);
                    if (item.Configuration.WireloomCppOutcome is not null)
                    {
                        outcomes.Add(item.Configuration.WireloomCppOutcome);
                    }
                    var unconfiguredCase = item.Case with
                    {
                        WireloomCppAvailable = false,
                        WireloomCppFailure = item.Configuration.WireloomCppFailure,
                    };
                    unconfiguredCases.Add(new BuiltCase(unconfiguredCase, item.Case.Paths.NativePeer,
                        NativePeerAvailable: false, NativePeerFailure: item.Configuration.RtiOutcome.Detail,
                        WireloomCppPeer: null));
                    hasIssues = true;
                    continue;
                }

                prepared.Add(item.Case);

                var fixturesPassed = await managed.VerifyFixturesAsync(item.Case);

                hasIssues |= !fixturesPassed;
            }
            finally
            {
                log.EndGroup();
            }
        }

        var nativeBatch = await native.BuildAsync(prepared);
        outcomes.AddRange(nativeBatch.Outcomes);

        PeerBuildFiles.WriteCompilation(workspace.CppResults, outcomes);
        log.Phase("peer-preparation", $"wireloom-cases={plan.CaseIds.Count} native-peers={nativeBatch.Cases.Count}");

        return new PeerPreparationResult([.. nativeBatch.Cases, .. unconfiguredCases], outcomes,
            hasIssues || nativeBatch.HasIssues);
    }

    private async Task<PreparedCaseResult> PrepareCaseAsync(string caseId, MatrixPlan plan, ManagedBuildBatch managedBatch)
    {
        var paths = workspace.ForCase(caseId);
        Directory.CreateDirectory(paths.Native);
        Directory.CreateDirectory(paths.WireloomBin);

        var usesNativePeerScope = plan.NativePeerScopeCases.Contains(caseId);
        var wireloomAvailable = managedBatch.Outcomes.TryGetValue(caseId, out var build) && build.IsCompiled;
        var descriptor = usesNativePeerScope
            ? native.DescribeNativePeerScopeCase(caseId)
            : DescribeCase(paths, plan.RtiOnlyCases.Contains(caseId), wireloomAvailable, build?.Detail);

        var wireloomPeer = new ManagedPeer(descriptor.WireloomType, paths.WireloomPeer, wireloomAvailable,
            usesNativePeerScope ? $"Wireloom C# peer is outside the selected native peer scope for {caseId}" : $"Wireloom C# build failed for {caseId}");
        var rtiPeer = new ManagedPeer("-", paths.RtiPeer, false, $"RTI C# peer is unavailable for {caseId}");
        var needsRti = !options.CompileOnly && plan.RequiresPeer(caseId, PeerNames.RtiCSharp);
        if (needsRti)
        {
            rtiPeer = await managed.BuildRtiAsync(descriptor, paths);
        }

        var needsWireloomCpp = plan.RequiresPeer(caseId, PeerNames.WireloomCpp);
        var configuration = await native.ConfigureAsync(descriptor, paths, needsWireloomCpp);
        var prepared = new PreparedCase(descriptor, paths, wireloomPeer, rtiPeer,
            configuration.WireloomCppAvailable, configuration.WireloomCppFailure);

        return new PreparedCaseResult(prepared, configuration, needsRti && !rtiPeer.Available);
    }

    private CaseDescriptor DescribeCase(CaseWorkspace paths, bool rtiOnly, bool wireloomAvailable, string? buildLog)
    {
        if (rtiOnly)
        {
            return CaseDescriptor.RtiOnly(paths.CaseId);
        }

        if (wireloomAvailable)
        {
            provenance.Record(paths);
        }
        else
        {
            WorkflowLog.BuildFailure($"Wireloom C# build failed for {paths.CaseId}", buildLog);
        }

        return discovery.Discover(paths.WireloomGenerated, [paths.CaseId]).Single();
    }

    private sealed record PreparedCaseResult(PreparedCase Case, NativePeerConfiguration Configuration, bool HasManagedIssues);
}
