using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Immutable settings captured once from the wire runner environment.</summary>
/// <param name="RepositoryRoot">Checkout containing the corpus and peer projects.</param>
/// <param name="RtiVersion">Exact qualified RTI release.</param>
/// <param name="CompileOnly">Whether to stop after peer compilation.</param>
/// <param name="CaseFilter">Optional comma-separated selection of corpus cases.</param>
/// <param name="ShardIndex">Zero-based shard selected by the planner.</param>
/// <param name="ShardCount">Number of whole-case shards.</param>
/// <param name="ManagedBuildConcurrency">Maximum simultaneous managed builds.</param>
/// <param name="NativeBuildConcurrency">Maximum simultaneous native builds.</param>
/// <param name="ExchangeConcurrency">Maximum simultaneous DDS exchanges.</param>
/// <param name="ScratchDirectory">Disposable build and scheduling files.</param>
/// <param name="OutputDirectory">Retained reports and diagnostic logs.</param>
/// <param name="PackagesDirectory">Shared NuGet cache used by peer builds.</param>
internal sealed record MatrixOptions(
    string RepositoryRoot,
    string RtiVersion,
    bool CompileOnly,
    string? CaseFilter,
    int ShardIndex,
    int ShardCount,
    int ManagedBuildConcurrency,
    int NativeBuildConcurrency,
    int ExchangeConcurrency,
    string ScratchDirectory,
    string OutputDirectory,
    string PackagesDirectory)
{
    /// <summary>Maximum execution time for an individual peer exchange.</summary>
    internal static TimeSpan ExchangeTimeout => TimeSpan.FromSeconds(20);

    /// <summary>Reads the existing environment-variable command-line contract.</summary>
    internal static MatrixOptions FromEnvironment()
    {
        var root = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE") ?? FindRepositoryRoot();
        var scratch = Path.Combine(Environment.GetEnvironmentVariable("RUNNER_TEMP") ?? "/tmp", "wireloom-wire-compatibility");

        return new MatrixOptions(
            root,
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0",
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CPP_COMPILE_ONLY") == "1",
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CASE_FILTER"),
            ReadInteger("WIRE_COMPATIBILITY_SHARD_INDEX", 0),
            ReadInteger("WIRE_COMPATIBILITY_SHARD_COUNT", 1),
            ReadInteger("WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY", 4),
            ReadInteger("WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY", 2),
            ReadInteger("WIRE_COMPATIBILITY_MAX_CONCURRENCY", 16),
            scratch,
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_OUTPUT_DIR") ?? Path.Combine(root, "artifacts", "wire-compatibility"),
            Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? Path.Combine(scratch, "nuget-packages"));
    }

    /// <summary>Rejects unsupported profiles and invalid scheduling bounds before preparation.</summary>
    internal void Validate()
    {
        if (RtiVersion is not ("7.3.1" or "7.7.0"))
        {
            throw new ArgumentException("The wire runner supports exact RTI 7.3.1 and 7.7.0 only.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ShardCount);
        ArgumentOutOfRangeException.ThrowIfNegative(ShardIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(ShardIndex, ShardCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ManagedBuildConcurrency);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(NativeBuildConcurrency);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ExchangeConcurrency);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(ExchangeConcurrency, 100);
    }

    private static int ReadInteger(string name, int defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value is null)
        {
            return defaultValue;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
        {
            throw new ArgumentException($"{name} must be an integer.");
        }

        return result;
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(Environment.CurrentDirectory);
        while (directory is not null)
        {
            var gitPath = Path.Combine(directory.FullName, ".git");
            if (File.Exists(gitPath) || Directory.Exists(gitPath))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find the Git repository root.");
    }
}
