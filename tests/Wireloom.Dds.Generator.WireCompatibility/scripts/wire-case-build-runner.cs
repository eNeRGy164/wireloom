// Builds prepared native wire peers concurrently with bounded process and compiler parallelism.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

const string usage = "Usage: wire-case-build-runner.cs [managed] --builds <tsv> --results <tsv> [managed build options]";

try
{
    if (args.Length > 0 && args[0] == "managed")
    {
        return await RunManagedBuildsAsync(args[1..]);
    }

    var options = Options.Parse(args);
    var builds = File.ReadAllLines(options.BuildsPath)
        .Where(line => line.Length > 0)
        .Select(Build.Parse)
        .ToArray();
    var results = new ConcurrentDictionary<string, BuildResult>(StringComparer.Ordinal);
    using var concurrency = new SemaphoreSlim(options.MaxConcurrency, options.MaxConcurrency);
    await Task.WhenAll(builds.Select(async build =>
    {
        await concurrency.WaitAsync();
        try
        {
            results[build.CaseId] = await BuildAsync(build, options.JobsPerBuild);
        }
        finally
        {
            concurrency.Release();
        }
    }));

    var orderedResults = builds.Select(build => results[build.CaseId]).ToArray();
    var temporaryPath = options.ResultsPath + ".tmp";
    File.WriteAllLines(temporaryPath, orderedResults.Select(result => result.ToTsv()));
    File.Move(temporaryPath, options.ResultsPath, overwrite: true);

    foreach (var result in orderedResults)
    {
        Console.WriteLine($"WIRE_CPP_BUILD case={result.CaseId} status={result.Status} elapsed={result.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");
    }

    return orderedResults.All(result => result.Status == "compiled") ? 0 : 1;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception is ArgumentException ? usage : exception.Message);
    return 2;
}

static async Task<BuildResult> BuildAsync(Build build, int jobsPerBuild)
{
    var stopwatch = Stopwatch.StartNew();
    try
    {
        var startInfo = new ProcessStartInfo("cmake")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("--build");
        startInfo.ArgumentList.Add(build.Directory);
        startInfo.ArgumentList.Add("--parallel");
        startInfo.ArgumentList.Add(jobsPerBuild.ToString(CultureInfo.InvariantCulture));

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Could not start CMake for {build.CaseId}.");
        var stdout = process.StandardOutput.ReadToEndAsync();
        var stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        await File.WriteAllTextAsync(build.LogPath, await stdout + await stderr);
        stopwatch.Stop();

        return new BuildResult(build.CaseId, process.ExitCode == 0 ? "compiled" : "compile-failed",
            process.ExitCode == 0 ? build.ArtifactPath : build.LogPath, stopwatch.Elapsed.TotalSeconds);
    }
    catch (Exception exception)
    {
        stopwatch.Stop();
        await File.WriteAllTextAsync(build.LogPath, exception.ToString());
        return new BuildResult(build.CaseId, "compile-failed", build.LogPath, stopwatch.Elapsed.TotalSeconds);
    }
}

static async Task<int> RunManagedBuildsAsync(string[] arguments)
{
    var options = ManagedOptions.Parse(arguments);
    var builds = File.ReadAllLines(options.BuildsPath)
        .Where(line => line.Length > 0)
        .Select(ManagedBuild.Parse)
        .ToArray();
    var results = new ConcurrentDictionary<string, BuildResult>(StringComparer.Ordinal);
    using var concurrency = new SemaphoreSlim(options.MaxConcurrency, options.MaxConcurrency);
    await Task.WhenAll(builds.Select(async build =>
    {
        await concurrency.WaitAsync();
        try
        {
            results[build.CaseId] = await BuildManagedAsync(build, options);
        }
        finally
        {
            concurrency.Release();
        }
    }));

    var orderedResults = builds.Select(build => results[build.CaseId]).ToArray();
    var temporaryPath = options.ResultsPath + ".tmp";
    File.WriteAllLines(temporaryPath, orderedResults.Select(result => result.ToTsv()));
    File.Move(temporaryPath, options.ResultsPath, overwrite: true);

    foreach (var result in orderedResults)
    {
        Console.WriteLine($"WIRE_CSHARP_BUILD case={result.CaseId} status={result.Status} elapsed={result.ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture)}s");
    }

    return orderedResults.All(result => result.Status == "compiled") ? 0 : 1;
}

static async Task<BuildResult> BuildManagedAsync(ManagedBuild build, ManagedOptions options)
{
    var stopwatch = Stopwatch.StartNew();
    try
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        foreach (var argument in new[]
        {
            "build", options.ProjectPath, "--no-restore", "--configuration", "Release",
            $"-p:WireCompatibilityCase={build.CaseId}",
            $"-p:WireCompatibilityRtiVersion={options.RtiVersion}",
            $"-p:RtiConnextDdsVersion={options.RtiVersion}",
            $"-p:RestorePackagesPath={options.RestorePackagesPath}",
            "-p:BuildProjectReferences=false",
            "-p:WireCompatibilityIsolatedIntermediateOutputPath=true",
            $"-p:BaseIntermediateOutputPath={build.IntermediateOutputPath}/",
            $"-p:MSBuildProjectExtensionsPath={options.ProjectExtensionsPath}/",
            $"-p:OutputPath={build.OutputPath}/",
            $"-p:WireCompatibilityGeneratedOutputPath={build.GeneratedOutputPath}",
            $"-p:WireCompatibilityIdl={build.IdlPath}",
            $"-p:DdsIdlSequenceArrayMapping={build.SequenceArrayMapping}",
            "--verbosity", "quiet",
        })
        {
            startInfo.ArgumentList.Add(argument);
        }

        if (build.WarningsNotAsErrors.Length > 0)
        {
            startInfo.ArgumentList.Insert(startInfo.ArgumentList.Count - 2,
                $"-p:WarningsNotAsErrors={build.WarningsNotAsErrors}");
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Could not start dotnet for {build.CaseId}.");
        var stdout = process.StandardOutput.ReadToEndAsync();
        var stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        await File.WriteAllTextAsync(build.LogPath, await stdout + await stderr);
        stopwatch.Stop();

        return new BuildResult(build.CaseId, process.ExitCode == 0 ? "compiled" : "build-failed",
            process.ExitCode == 0 ? build.OutputPath : build.LogPath, stopwatch.Elapsed.TotalSeconds);
    }
    catch (Exception exception)
    {
        stopwatch.Stop();
        await File.WriteAllTextAsync(build.LogPath, exception.ToString());
        return new BuildResult(build.CaseId, "build-failed", build.LogPath, stopwatch.Elapsed.TotalSeconds);
    }
}

sealed record Build(string CaseId, string Directory, string LogPath, string ArtifactPath)
{
    internal static Build Parse(string line)
    {
        var fields = line.Split('\t');
        if (fields.Length != 4 || fields.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidDataException("A native build row must contain a case ID, build directory, log path, and artifact path.");
        }

        return new Build(fields[0], fields[1], fields[2], fields[3]);
    }
}

sealed record BuildResult(string CaseId, string Status, string Detail, double ElapsedSeconds)
{
    internal string ToTsv() => string.Join('\t', CaseId, Status, Detail,
        ElapsedSeconds.ToString("F1", CultureInfo.InvariantCulture));
}

sealed record ManagedBuild(string CaseId, string IdlPath, string SequenceArrayMapping,
    string OutputPath, string GeneratedOutputPath, string IntermediateOutputPath,
    string LogPath, string WarningsNotAsErrors)
{
    internal static ManagedBuild Parse(string line)
    {
        var fields = line.Split('\t');
        if (fields.Length != 8 || fields.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidDataException("A managed build row must contain a case ID, IDL path, mapping, output path, generated path, intermediate path, log path, and warning override.");
        }

        return new ManagedBuild(fields[0], fields[1], fields[2] == "-" ? string.Empty : fields[2],
            fields[3], fields[4], fields[5], fields[6],
            fields[7] == "-" ? string.Empty : fields[7]);
    }
}

sealed record Options(string BuildsPath, string ResultsPath, int MaxConcurrency, int JobsPerBuild)
{
    internal static Options Parse(string[] arguments)
    {
        string? buildsPath = null;
        string? resultsPath = null;
        var maxConcurrency = 2;
        var jobsPerBuild = 2;
        for (var index = 0; index < arguments.Length; index++)
        {
            var value = index + 1 < arguments.Length ? arguments[++index] : throw new ArgumentException("Missing option value.");
            switch (arguments[index - 1])
            {
                case "--builds": buildsPath = value; break;
                case "--results": resultsPath = value; break;
                case "--max-concurrency": maxConcurrency = PositiveInteger(value, "max concurrency"); break;
                case "--jobs-per-build": jobsPerBuild = PositiveInteger(value, "jobs per build"); break;
                default: throw new ArgumentException($"Unknown option {arguments[index - 1]}.");
            }
        }

        if (string.IsNullOrWhiteSpace(buildsPath) || string.IsNullOrWhiteSpace(resultsPath))
        {
            throw new ArgumentException("Both build input and result paths are required.");
        }

        return new Options(buildsPath, resultsPath, maxConcurrency, jobsPerBuild);
    }

    internal static int PositiveInteger(string value, string description) =>
        int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed) && parsed > 0
            ? parsed
            : throw new ArgumentException($"{description} must be a positive integer.");
}

sealed record ManagedOptions(string BuildsPath, string ResultsPath, string ProjectPath, string RtiVersion,
    string RestorePackagesPath, string ProjectExtensionsPath, int MaxConcurrency)
{
    internal static ManagedOptions Parse(string[] arguments)
    {
        string? buildsPath = null;
        string? resultsPath = null;
        string? projectPath = null;
        string? rtiVersion = null;
        string? restorePackagesPath = null;
        string? projectExtensionsPath = null;
        var maxConcurrency = 4;
        for (var index = 0; index < arguments.Length; index++)
        {
            var option = arguments[index];
            var value = index + 1 < arguments.Length ? arguments[++index] : throw new ArgumentException("Missing option value.");
            switch (option)
            {
                case "--builds": buildsPath = value; break;
                case "--results": resultsPath = value; break;
                case "--project": projectPath = value; break;
                case "--rti-version": rtiVersion = value; break;
                case "--restore-packages": restorePackagesPath = value; break;
                case "--project-extensions": projectExtensionsPath = value; break;
                case "--max-concurrency": maxConcurrency = Options.PositiveInteger(value, "max concurrency"); break;
                default: throw new ArgumentException($"Unknown managed-build option {option}.");
            }
        }

        if (new[] { buildsPath, resultsPath, projectPath, rtiVersion, restorePackagesPath, projectExtensionsPath }
            .Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Managed builds require build/result paths, project, RTI version, package path, and project extensions path.");
        }

        return new ManagedOptions(buildsPath!, resultsPath!, projectPath!, rtiVersion!, restorePackagesPath!, projectExtensionsPath!, maxConcurrency);
    }
}
