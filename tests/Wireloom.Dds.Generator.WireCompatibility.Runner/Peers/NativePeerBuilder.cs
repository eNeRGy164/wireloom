using System.Diagnostics;
using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Stages and configures native cases, then compiles them with bounded concurrency.</summary>
internal sealed class NativePeerBuilder(MatrixOptions options, MatrixWorkspace workspace, NativePeerSources sources, ProcessRunner processes, WorkflowLog log)
{
    /// <summary>Stages sources and configures a CMake build, returning evidence for either outcome.</summary>
    internal async Task<BuildOutcome> ConfigureAsync(CaseDescriptor descriptor, CaseWorkspace paths)
    {
        if (!sources.Stage(descriptor, paths))
        {
            return new BuildOutcome(descriptor.CaseId, "missing-captured-oracle", workspace.OracleManifest);
        }

        var profile = NativePeerProfile.ForCase(descriptor.CaseId, options.RtiVersion, descriptor.CppType);
        sources.WritePeerTemplates(paths, profile.CppType);

        var configureLog = Path.Combine(paths.Directory, "cmake-configure.log");
        var architecture = options.RtiVersion == "7.3.1" ? "x64Linux4gcc7.3.0" : "x64Linux4gcc8.5.0";
        var arguments = new List<string>
        {
            "-S", paths.Native,
            "-B", paths.NativeBuild,
            "-DCMAKE_MODULE_PATH=" + Path.Combine(Environment.GetEnvironmentVariable("NDDSHOME")!, "resource", "cmake"),
            "-DCONNEXTDDS_ARCH=" + architecture,
        };
        arguments.AddRange(profile.Options);

        var result = await processes.RunAsync("cmake", arguments, configureLog, allowFailure: true);
        if (!result.Succeeded)
        {
            return new BuildOutcome(descriptor.CaseId, "cmake-configure-failed", $"see {configureLog}");
        }

        return new BuildOutcome(descriptor.CaseId, "configured", paths.NativeBuild);
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

        var outcomes = await BoundedBuildExecutor.RunAsync(preparedCases, options.NativeBuildConcurrency, BuildNativeAsync);
        var preparedById = preparedCases.ToDictionary(item => item.Descriptor.CaseId, StringComparer.Ordinal);
        var builtCases = new List<BuiltCase>();
        foreach (var outcome in outcomes)
        {
            Console.WriteLine($"WIRE_CPP_BUILD case={outcome.CaseId} status={outcome.Status} elapsed={outcome.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");

            if (outcome.IsCompiled)
            {
                var prepared = preparedById[outcome.CaseId];
                builtCases.Add(new BuiltCase(prepared, prepared.Paths.NativePeer));
            }
        }

        log.EndGroup();

        var evidence = outcomes.Select(outcome => outcome.IsCompiled
            ? outcome with
            {
                Detail = preparedById[outcome.CaseId].Paths.NativePeer
            }
            : outcome with
            {
                Status = "compile-failed",
                Detail = $"see {outcome.Detail}"
            }).ToArray();

        return new NativeBuildBatch(builtCases, evidence, outcomes.Any(outcome => !outcome.IsCompiled));
    }

    private async Task<BuildOutcome> BuildNativeAsync(PreparedCase prepared)
    {
        var stopwatch = Stopwatch.StartNew();
        var paths = prepared.Paths;
        var status = "compile-failed";
        var detail = paths.NativeBuildLog;

        try
        {
            var result = await processes.RunAsync("cmake",
                ["--build", paths.NativeBuild, "--parallel", "2"], paths.NativeBuildLog, allowFailure: true);
            if (result.Succeeded)
            {
                status = "compiled";
                detail = paths.NativePeer;
            }
        }
        catch (Exception exception)
        {
            await File.WriteAllTextAsync(paths.NativeBuildLog, exception.ToString());
        }

        stopwatch.Stop();
        return new BuildOutcome(prepared.Descriptor.CaseId, status, detail, stopwatch.Elapsed.TotalSeconds);
    }
}

/// <summary>Artifacts and evidence from the bounded native build batch.</summary>
/// <param name="Cases">Cases whose native peer compiled.</param>
/// <param name="Outcomes">Ordered compilation results.</param>
/// <param name="HasIssues">Whether a build failed.</param>
internal sealed record NativeBuildBatch(IReadOnlyList<BuiltCase> Cases, IReadOnlyList<BuildOutcome> Outcomes, bool HasIssues);
