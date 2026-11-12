using System.Diagnostics;
using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Stages and configures native cases, then compiles them with bounded concurrency.</summary>
internal sealed class NativePeerBuilder(MatrixOptions options, MatrixWorkspace workspace, NativePeerSources sources,
    CorpusCaseCatalog catalog, ProcessRunner processes, WorkflowLog log)
{
    private Task<bool>? generatorBuild;

    /// <summary>Builds a native peer descriptor from the retained RTI C++ topic metadata.</summary>
    internal CaseDescriptor DescribeNativePeerScopeCase(string caseId)
    {
        var metadata = catalog.Get(caseId);
        return new CaseDescriptor(caseId, "-", sources.DiscoverNativePeerScopeType(caseId), metadata.Idl);
    }

    /// <summary>Stages sources and configures a CMake build, returning evidence for either outcome.</summary>
    internal async Task<NativePeerConfiguration> ConfigureAsync(CaseDescriptor descriptor, CaseWorkspace paths, bool includeWireloomCpp)
    {
        var rtiOracleAvailable = sources.Stage(descriptor, paths);
        if (!rtiOracleAvailable && !includeWireloomCpp)
        {
            return new NativePeerConfiguration(new BuildOutcome(descriptor.CaseId, "missing-captured-oracle", workspace.OracleManifest),
                null, false, "RTI C++ oracle is unavailable.");
        }

        var profile = NativePeerProfile.ForCase(descriptor.CaseId, options.RtiVersion, descriptor.CppType);
        sources.WritePeerTemplates(paths, profile.CppType);

        BuildOutcome? wireloomCppOutcome = null;
        var wireloomCppAvailable = false;
        var wireloomCppFailure = "Wireloom C++ is not selected for this case.";
        if (includeWireloomCpp)
        {
            wireloomCppFailure = await GenerateWireloomCppAsync(descriptor, paths);
            wireloomCppAvailable = wireloomCppFailure.Length == 0;
            if (!wireloomCppAvailable)
            {
                wireloomCppOutcome = new BuildOutcome("wireloom-cpp:" + descriptor.CaseId,
                    "generation-failed", wireloomCppFailure);
            }
        }

        var configureLog = Path.Combine(paths.Directory, "cmake-configure.log");
        var architecture = options.RtiVersion is "6.1.2" or "7.3.1" ? "x64Linux4gcc7.3.0" : "x64Linux4gcc8.5.0";
        var arguments = new List<string>
        {
            "-S", paths.Native,
            "-B", paths.NativeBuild,
            "-DCMAKE_MODULE_PATH=" + Path.Combine(Environment.GetEnvironmentVariable("NDDSHOME")!, "resource", "cmake"),
            "-DCONNEXTDDS_ARCH=" + architecture,
        };
        if (wireloomCppAvailable)
        {
            sources.WriteWireloomCppTemplate(paths, descriptor.CppType,
                Path.GetFileNameWithoutExtension(IdlSourceResolver.ResolveOracleIdl(options.RepositoryRoot, descriptor.CaseId,
                    workspace.CorpusFile(catalog.Get(descriptor.CaseId).Idl))));
            arguments.Add("-DWIRELOOM_CPP_GENERATED_DIR=" + paths.WireloomCppGenerated);
        }
        arguments.AddRange(profile.Options);

        var result = await processes.RunAsync("cmake", arguments, configureLog, allowFailure: true);
        if (!result.Succeeded)
        {
            var detail = $"see {RetainLog(configureLog, $"{descriptor.CaseId}-cmake-configure.log")}";
            return new NativePeerConfiguration(new BuildOutcome(descriptor.CaseId, "cmake-configure-failed", detail),
                includeWireloomCpp && wireloomCppAvailable
                    ? new BuildOutcome("wireloom-cpp:" + descriptor.CaseId, "cmake-configure-failed", detail)
                    : wireloomCppOutcome,
                false, wireloomCppAvailable ? "CMake configuration failed." : wireloomCppFailure);
        }

        return new NativePeerConfiguration(new BuildOutcome(descriptor.CaseId, "configured", paths.NativeBuild),
            wireloomCppOutcome, wireloomCppAvailable, wireloomCppFailure);
    }

    /// <summary>Compiles all prepared native cases before exchanges can start.</summary>
    internal async Task<NativeBuildBatch> BuildAsync(IReadOnlyList<PreparedCase> preparedCases)
    {
        if (preparedCases.Count == 0)
        {
            return new NativeBuildBatch([], [], false);
        }

        log.StartGroup("Build native peers concurrently");
        log.Phase("native-peer-build", $"queued={preparedCases.Count} concurrency={options.NativeBuildConcurrency}");

        var results = await BoundedBuildExecutor.RunAsync(preparedCases, options.NativeBuildConcurrency, BuildNativeAsync);
        var preparedById = preparedCases.ToDictionary(item => item.Descriptor.CaseId, StringComparer.Ordinal);
        var builtCases = new List<BuiltCase>();
        var outcomes = new List<BuildOutcome>();
        foreach (var result in results)
        {
            var outcome = result.RtiOutcome;
            outcomes.Add(outcome);
            if (result.WireloomCppOutcome is not null)
            {
                outcomes.Add(result.WireloomCppOutcome);
            }
            Console.WriteLine($"RTI_CPP_BUILD case={outcome.CaseId} status={outcome.Status} elapsed={outcome.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");

            if (result.WireloomCppOutcome is { } wireloomCppOutcome)
            {
                Console.WriteLine($"WIRELOOM_CPP_BUILD case={outcome.CaseId} status={wireloomCppOutcome.Status} elapsed={wireloomCppOutcome.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");
            }

            var prepared = preparedById[outcome.CaseId];
            var cppPeer = result.WireloomCppPeer;
            var wireloomCppFailure = prepared.WireloomCppFailure;
            if (prepared.WireloomCppAvailable && cppPeer is null)
            {
                wireloomCppFailure = result.WireloomCppOutcome is { } failedCppOutcome
                    ? $"Wireloom C++ peer {failedCppOutcome.Status}; {failedCppOutcome.Detail}."
                    : "Wireloom C++ peer build did not produce an executable.";
            }

            // Keep the case in the exchange scheduler even when both native
            // peers failed to compile. Managed-only pairings are still runnable;
            // the scheduler will fail only rows that use a missing native peer.
            var caseConfiguration = prepared with { WireloomCppFailure = wireloomCppFailure };
            builtCases.Add(new BuiltCase(caseConfiguration, prepared.Paths.NativePeer, outcome.IsCompiled,
                outcome.IsCompiled ? string.Empty : outcome.Detail, cppPeer));
        }

        log.EndGroup();

        var evidence = outcomes.Select(outcome => outcome.IsCompiled
            ? outcome with
            {
                Detail = outcome.CaseId.StartsWith("wireloom-cpp:", StringComparison.Ordinal)
                    ? preparedById[outcome.CaseId["wireloom-cpp:".Length..]].Paths.WireloomCppPeer
                    : preparedById[outcome.CaseId].Paths.NativePeer
            }
            : outcome with
            {
                Detail = outcome.Detail.StartsWith("see ", StringComparison.Ordinal) ? outcome.Detail : $"see {outcome.Detail}"
            }).ToArray();

        return new NativeBuildBatch(builtCases, evidence, outcomes.Any(outcome => !outcome.IsCompiled));
    }

    private async Task<NativePeerBuildResult> BuildNativeAsync(PreparedCase prepared)
    {
        var stopwatch = Stopwatch.StartNew();
        var paths = prepared.Paths;
        var status = "compile-failed";
        string detail;
        BuildOutcome? wireloomCppOutcome = null;
        string? wireloomCppPeer = null;

        try
        {
            var result = await processes.RunAsync("cmake",
                ["--build", paths.NativeBuild, "--target", "wire-peer", "--parallel", "2"], paths.NativeBuildLog, allowFailure: true);
            if (result.Succeeded)
            {
                status = "compiled";
                detail = paths.NativePeer;
            }
            else
            {
                detail = $"see {RetainLog(paths.NativeBuildLog, $"{prepared.Descriptor.CaseId}-rti-cpp-build.log")}";
            }

            var recursiveSequenceSource = Path.Combine(paths.Native, "recursive-sequence-peer.cpp");
            if (result.Succeeded && File.Exists(recursiveSequenceSource))
            {
                var recursiveSequenceLog = Path.Combine(paths.Directory, "native-recursive-sequence-build.log");
                var recursiveSequenceResult = await processes.RunAsync("cmake",
                    ["--build", paths.NativeBuild, "--target", "wire-peer-recursive-sequence", "--parallel", "2"],
                    recursiveSequenceLog, allowFailure: true);
                if (!recursiveSequenceResult.Succeeded)
                {
                    status = "compile-failed";
                    detail = $"see {RetainLog(recursiveSequenceLog, $"{prepared.Descriptor.CaseId}-rti-cpp-recursive-build.log")}";
                }
            }

            if (prepared.WireloomCppAvailable)
            {
                var retainedBuildLog = Path.Combine(workspace.Logs, $"{prepared.Descriptor.CaseId}-wireloom-cpp-build.log");
                Directory.CreateDirectory(workspace.Logs);
                var cppResult = await processes.RunAsync("cmake",
                    ["--build", paths.NativeBuild, "--target", "wire-peer-wireloom-cpp", "--parallel", "2"],
                    retainedBuildLog, allowFailure: true);

                var buildDetail = cppResult.Succeeded
                    ? paths.WireloomCppPeer
                    : $"see {Path.GetRelativePath(options.RepositoryRoot, retainedBuildLog).Replace('\\', '/')}";
                wireloomCppOutcome = new BuildOutcome("wireloom-cpp:" + prepared.Descriptor.CaseId,
                    cppResult.Succeeded ? "compiled" : "compile-failed",
                    buildDetail,
                    stopwatch.Elapsed.TotalSeconds);
                if (cppResult.Succeeded)
                {
                    wireloomCppPeer = paths.WireloomCppPeer;
                }
            }
            else if (prepared.WireloomCppFailure != "Wireloom C++ is not selected for this case.")
            {
                wireloomCppOutcome = new BuildOutcome("wireloom-cpp:" + prepared.Descriptor.CaseId,
                    "generation-failed", prepared.WireloomCppFailure);
            }
        }
        catch (Exception exception)
        {
            await File.WriteAllTextAsync(paths.NativeBuildLog, exception.ToString());
            detail = $"see {RetainLog(paths.NativeBuildLog, $"{prepared.Descriptor.CaseId}-rti-cpp-build.log")}";
        }

        stopwatch.Stop();
        var rtiOutcome = new BuildOutcome(prepared.Descriptor.CaseId, status, detail, stopwatch.Elapsed.TotalSeconds);
        return new NativePeerBuildResult(rtiOutcome, wireloomCppOutcome, wireloomCppPeer);
    }

    private async Task<string> GenerateWireloomCppAsync(CaseDescriptor descriptor, CaseWorkspace paths)
    {
        if (!await (generatorBuild ??= BuildCppGeneratorAsync()))
        {
            return "Wireloom C++ generator build failed; see the runner log.";
        }

        var metadata = catalog.Get(descriptor.CaseId);
        var idl = IdlSourceResolver.ResolveOracleIdl(options.RepositoryRoot, descriptor.CaseId, workspace.CorpusFile(metadata.Idl));
        var arguments = CreateCppGeneratorArguments(
            Path.Combine(options.RepositoryRoot, "src", "Wireloom.Dds.Cpp.Generator", "bin", "Release", "net10.0", "Wireloom.Dds.Cpp.dll"),
            options.RtiVersion,
            idl,
            paths.WireloomCppGenerated,
            metadata,
            [
                workspace.CorpusFile("idl/features"),
                workspace.CorpusFile("idl/includes"),
                workspace.CorpusFile("idl"),
                Path.Combine(workspace.TestRoot, "IdlWrappers"),
            ]);

        Directory.CreateDirectory(paths.WireloomCppGenerated);
        Directory.CreateDirectory(workspace.Logs);
        var generationLog = Path.Combine(workspace.Logs, $"{descriptor.CaseId}-wireloom-cpp-generation.log");
        var result = await processes.RunAsync("dotnet", arguments, generationLog, allowFailure: true);
        return result.Succeeded ? string.Empty : $"Wireloom C++ generation failed; see {Path.GetRelativePath(options.RepositoryRoot, generationLog).Replace('\\', '/')}.";
    }

    /// <summary>Builds the standalone generator command with the selected exact RTI API profile.</summary>
    internal static IReadOnlyList<string> CreateCppGeneratorArguments(string assemblyPath, string rtiVersion,
        string idlPath, string outputDirectory, CaseMetadata metadata, IReadOnlyList<string> includeDirectories)
    {
        var arguments = new List<string>
        {
            assemblyPath,
            "generate",
            idlPath,
            outputDirectory,
            "--rti-version",
            rtiVersion,
        };

        foreach (var define in metadata.Defines)
        {
            arguments.Add("--define");
            arguments.Add(define);
        }

        foreach (var includeDirectory in includeDirectories)
        {
            arguments.Add("--include-directory");
            arguments.Add(includeDirectory);
        }

        if (metadata.RtiCppCodegenOptions.Contains("-unboundedSupport", StringComparer.Ordinal))
        {
            arguments.Add("--unbounded-support");
        }

        return arguments;
    }

    private async Task<bool> BuildCppGeneratorAsync()
    {
        var logPath = Path.Combine(options.OutputDirectory, "logs", "wireloom-cpp-generator-build.log");
        var result = await processes.RunAsync("dotnet",
            ["build", workspace.CppGeneratorProject, "--configuration", "Release", "--verbosity", "quiet"],
            logPath, allowFailure: true);
        return result.Succeeded;
    }

    private string RetainLog(string source, string fileName)
    {
        Directory.CreateDirectory(workspace.Logs);
        var destination = Path.Combine(workspace.Logs, fileName);
        if (File.Exists(source))
        {
            File.Copy(source, destination, overwrite: true);
        }

        return Path.GetRelativePath(options.RepositoryRoot, destination).Replace('\\', '/');
    }
}

/// <summary>Artifacts and evidence from the bounded native build batch.</summary>
/// <param name="Cases">Cases whose native peer compiled.</param>
/// <param name="Outcomes">Ordered compilation results.</param>
/// <param name="HasIssues">Whether a build failed.</param>
internal sealed record NativeBuildBatch(IReadOnlyList<BuiltCase> Cases, IReadOnlyList<BuildOutcome> Outcomes, bool HasIssues);
