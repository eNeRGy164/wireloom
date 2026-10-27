using System.Diagnostics;
using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns managed restore, bootstrap, bounded builds, and RTI control-peer publication.</summary>
internal sealed class ManagedPeerBuilder(MatrixOptions options, MatrixWorkspace workspace, CorpusCaseCatalog catalog, WireOracleCatalog oracles, ProcessRunner processes, WorkflowLog log)
{
    /// <summary>Restores once, builds shared references once, then queues the remaining Wireloom peers.</summary>
    internal async Task<ManagedBuildBatch> BuildWireloomAsync(MatrixPlan plan)
    {
        await RestoreAsync(workspace.WireloomProject);

        if (!options.CompileOnly)
        {
            await RestoreAsync(workspace.RtiProject);
        }

        var outcomes = new Dictionary<string, BuildOutcome>(StringComparer.Ordinal);
        var requests = new List<ManagedBuildRequest>();

        var cases = plan.CaseIds.Where(caseId => !plan.RtiOnlyCases.Contains(caseId)).ToArray();
        if (cases.Length > 0)
        {
            var bootstrap = workspace.ForCase(cases[0]);
            outcomes[bootstrap.CaseId] = await BuildBootstrapAsync(bootstrap);

            foreach (var caseId in cases.Skip(1))
            {
                var paths = workspace.ForCase(caseId);
                Directory.CreateDirectory(paths.WireloomBin);
                Directory.CreateDirectory(paths.WireloomObj);

                var input = catalog.Get(caseId).Input;
                requests.Add(new ManagedBuildRequest(paths, input, workspace.CorpusFile(input.Idl)));
            }
        }

        var workerFailed = false;
        if (requests.Count > 0)
        {
            workerFailed = await BuildQueuedAsync(requests, outcomes);
        }

        log.Phase("managed-peer-publish", $"queued={requests.Count} concurrency={options.ManagedBuildConcurrency}");

        return new ManagedBuildBatch(outcomes, workerFailed || outcomes.Values.Any(outcome => !outcome.IsCompiled));
    }

    /// <summary>Stages a captured RTI C# oracle and publishes its isolated control peer.</summary>
    internal async Task<ManagedPeer> BuildRtiAsync(CaseDescriptor descriptor, CaseWorkspace paths)
    {
        var unavailable = new ManagedPeer("-", paths.RtiPeer, false, $"RTI C# peer is unavailable for {descriptor.CaseId}");

        var oracle = oracles.FindSource(descriptor.CaseId, "rtiCSharp");
        if (string.IsNullOrEmpty(oracle))
        {
            await Console.Error.WriteLineAsync($"No checked-in RTI C# wire oracle is available for {descriptor.CaseId}; refusing to generate during wire execution.");
            return unavailable;
        }

        var generated = Path.Combine(paths.Directory, "rti-csharp-generated");
        CopyOracle(workspace.OracleDirectory(oracle), generated);

        await File.WriteAllTextAsync(Path.Combine(workspace.Logs, descriptor.CaseId + "-rti-csharp-oracle.log"), $"Using checked-in RTI C# oracle ({oracle}; selection and adjustments are recorded in {Path.GetRelativePath(options.RepositoryRoot, workspace.OracleManifest).Replace('\\', '/')}).{Environment.NewLine}");

        string typeName;
        try
        {
            typeName = RtiTypeDiscovery.Discover(descriptor.CaseId, descriptor.WireloomType, generated);
        }
        catch (Exception exception) when (exception is InvalidOperationException or IOException or UnauthorizedAccessException)
        {
            await Console.Error.WriteLineAsync($"RTI C# type discovery failed for {descriptor.CaseId}: {exception.Message}");
            return unavailable;
        }

        var rtiBin = Path.GetDirectoryName(paths.RtiPeer)!;
        Directory.CreateDirectory(rtiBin);

        var buildLog = Path.Combine(paths.Directory, "rti-csharp-build.log");

        var result = await processes.RunAsync("dotnet",
            [
                "publish", workspace.RtiProject,
                "--no-restore",
                "--configuration", "Release",
                "--verbosity", "quiet",
                $"-p:RtiOracleSourceDir={generated}",
                $"-p:WireCompatibilityCase={descriptor.CaseId}",
                $"-p:BaseIntermediateOutputPath={Path.Combine(paths.Directory, "rti-obj")}/",
                $"-p:MSBuildProjectExtensionsPath={Path.Combine(workspace.TestRoot, "RtiOracleProbe", "obj")}/",
                $"-p:RestorePackagesPath={options.PackagesDirectory}",
                $"-p:OutputPath={rtiBin}/",
                $"-p:PublishDir={rtiBin}/",
                "-p:UseAppHost=false"
            ], buildLog, allowFailure: true);

        if (!result.Succeeded)
        {
            WorkflowLog.BuildFailure($"RTI-generated C# peer failed to compile for {descriptor.CaseId}", buildLog);
            return unavailable with
            {
                TypeName = typeName
            };
        }

        Console.WriteLine($"RTI-generated C# peer published: {typeName}");

        return unavailable with
        {
            TypeName = typeName,
            Available = true
        };
    }

    /// <summary>Verifies the annotation/value-binding fixtures before scheduling exchanges.</summary>
    internal async Task<bool> VerifyFixturesAsync(PreparedCase prepared)
    {
        if (!prepared.Descriptor.CaseId.StartsWith("14-", StringComparison.Ordinal))
        {
            return true;
        }

        var passed = true;

        foreach (var fixture in WireCompatibilityCaseCatalog.FixturesFor(prepared.Descriptor.CaseId, options.RtiVersion))
        {
            passed &= await VerifyFixtureAsync("wireloom-cs", prepared.Wireloom, prepared.Paths, fixture);

            if (prepared.Rti.Available)
            {
                passed &= await VerifyFixtureAsync("rti-cs", prepared.Rti, prepared.Paths, fixture);
            }
        }

        return passed;
    }

    private async Task RestoreAsync(string project)
    {
        await processes.RunAsync("dotnet",
            [
                "restore", project,
                $"-p:WireCompatibilityRtiVersion={options.RtiVersion}",
                $"-p:RtiConnextDdsVersion={options.RtiVersion}",
                $"-p:RestorePackagesPath={options.PackagesDirectory}",
                "--locked-mode",
                "--verbosity", "quiet"
            ]);
    }

    private async Task<BuildOutcome> BuildBootstrapAsync(CaseWorkspace paths)
    {
        Directory.CreateDirectory(paths.Directory);

        var input = catalog.Get(paths.CaseId).Input;
        var arguments = new List<string>
            {
                "publish", workspace.WireloomProject,
                "--no-restore",
                "--configuration", "Release",
                $"-p:WireCompatibilityCase={paths.CaseId}",
                $"-p:WireCompatibilityRtiVersion={options.RtiVersion}",
                $"-p:RtiConnextDdsVersion={options.RtiVersion}",
                $"-p:RestorePackagesPath={options.PackagesDirectory}",
                $"-p:OutputPath={paths.WireloomBin}/",
                $"-p:PublishDir={paths.WireloomBin}/",
                "-p:UseAppHost=false",
                $"-p:WireCompatibilityIdl={workspace.CorpusFile(input.Idl)}",
                $"-p:DdsIdlSequenceArrayMapping={input.Mapping}",
                $"-p:WireCompatibilityGeneratedOutputPath={paths.WireloomGenerated}",
            };

        if (paths.CaseId == "05-array-of-sequences")
        {
            arguments.Add("-p:WarningsNotAsErrors=DDSG0105");
        }

        arguments.AddRange(["--verbosity", "quiet"]);

        var result = await processes.RunAsync("dotnet", arguments, paths.WireloomBuildLog, allowFailure: true);
        if (!result.Succeeded)
        {
            return new BuildOutcome(paths.CaseId, "build-failed", paths.WireloomBuildLog);
        }

        return new BuildOutcome(paths.CaseId, "compiled", paths.WireloomBin);
    }

    private async Task<bool> BuildQueuedAsync(IReadOnlyList<ManagedBuildRequest> requests, Dictionary<string, BuildOutcome> outcomes)
    {
        var results = await BoundedBuildExecutor.RunAsync(requests, options.ManagedBuildConcurrency, BuildManagedAsync);
        foreach (var outcome in results)
        {
            outcomes[outcome.CaseId] = outcome;
            Console.WriteLine($"WIRE_CSHARP_BUILD case={outcome.CaseId} status={outcome.Status} elapsed={outcome.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");
        }

        return results.Any(outcome => !outcome.IsCompiled);
    }

    private async Task<BuildOutcome> BuildManagedAsync(ManagedBuildRequest request)
    {
        var stopwatch = Stopwatch.StartNew();
        var paths = request.Paths;
        var arguments = new List<string>
        {
            "build", workspace.WireloomProject, "--no-restore", "--configuration", "Release",
            $"-p:WireCompatibilityCase={paths.CaseId}",
            $"-p:WireCompatibilityRtiVersion={options.RtiVersion}",
            $"-p:RtiConnextDdsVersion={options.RtiVersion}",
            $"-p:RestorePackagesPath={options.PackagesDirectory}",
            "-p:BuildProjectReferences=false",
            "-p:WireCompatibilityIsolatedIntermediateOutputPath=true",
            $"-p:BaseIntermediateOutputPath={paths.WireloomObj}/",
            $"-p:MSBuildProjectExtensionsPath={Path.Combine(workspace.TestRoot, "obj")}/",
            $"-p:OutputPath={paths.WireloomBin}/",
            $"-p:WireCompatibilityGeneratedOutputPath={paths.WireloomGenerated}",
            $"-p:WireCompatibilityIdl={request.IdlPath}",
            $"-p:DdsIdlSequenceArrayMapping={request.Input.Mapping}",
        };

        if (paths.CaseId == "05-array-of-sequences")
        {
            arguments.Add("-p:WarningsNotAsErrors=DDSG0105");
        }

        arguments.AddRange(["--verbosity", "quiet"]);
        var status = "build-failed";
        var detail = paths.WireloomBuildLog;
        try
        {
            var result = await processes.RunAsync("dotnet", arguments, paths.WireloomBuildLog, allowFailure: true);
            if (result.Succeeded)
            {
                status = "compiled";
                detail = paths.WireloomBin;
            }
        }
        catch (Exception exception)
        {
            await File.WriteAllTextAsync(paths.WireloomBuildLog, exception.ToString());
        }

        stopwatch.Stop();
        return new BuildOutcome(paths.CaseId, status, detail, stopwatch.Elapsed.TotalSeconds);
    }

    private async Task<bool> VerifyFixtureAsync(string label, ManagedPeer peer, CaseWorkspace paths, string fixture)
    {
        if (peer.TypeName == "-" || !File.Exists(peer.Path))
        {
            return true;
        }

        var result = await processes.RunAsync("dotnet",
            [
                peer.Path,
                "--case", paths.CaseId,
                "--type", peer.TypeName,
                "--topic", "fixture-validation-" + paths.CaseId,
                "--runtime-version", options.RtiVersion,
                "--role", "writer",
                "--fixture", fixture,
                "--fixture-only", "true"
            ],
            Path.Combine(paths.Directory, $"fixture-verify-{label}-{fixture}.log"),
            timeout: MatrixOptions.ExchangeTimeout + TimeSpan.FromSeconds(5),
            allowFailure: true);

        if (result.Succeeded)
        {
            Console.WriteLine($"{label} fixture verified: {fixture}");
        }

        return result.Succeeded;
    }

    private static void CopyOracle(string source, string destination)
    {
        Directory.CreateDirectory(destination);

        foreach (var directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, directory)));
        }

        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);

            File.Copy(file, target, overwrite: true);
        }
    }
}
