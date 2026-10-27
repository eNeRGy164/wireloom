using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Provides shared paths, command access, and assertion accounting for a verifier run.</summary>
internal sealed class VerificationContext(string verifierAssembly, string repositoryRoot, string root, string rtiVersion)
{
    internal string VerifierAssembly { get; } = verifierAssembly;
    internal string RepositoryRoot { get; } = repositoryRoot;
    internal string Root { get; } = root;
    internal string RtiVersion { get; } = rtiVersion;
    internal string ManifestPath => Path.Combine(RepositoryRoot, "docs", "corpus", "manifest.json");
    internal int CheckCount { get; private set; }

    internal string Invoke(bool succeeds, params string[] command)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        start.ArgumentList.Add(VerifierAssembly);

        foreach (var argument in command)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not start a verifier command.");
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(30000))
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException("Verifier command exceeded 30 seconds.");
        }

        Task.WaitAll(output, error);

        if (process.ExitCode == 0 != succeeds)
        {
            throw new InvalidOperationException($"{command[0]} exit {process.ExitCode}: {output.Result} {error.Result}");
        }

        return output.Result + error.Result;
    }

    internal void Check(bool condition, string description)
    {
        if (condition)
        {
            CheckCount++;
            return;
        }

        throw new InvalidOperationException(description);
    }

    internal void RecordChecks(int count) => CheckCount += count;

    internal static JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

    internal static string FindReportRow(string markdown, string caseId, string fixture) =>
        markdown.Split('\n').Single(line =>
        {
            var columns = line.Split('|', StringSplitOptions.TrimEntries);
            return columns.Length > 2
                && columns[1].Contains($"`{caseId}`", StringComparison.Ordinal)
                && columns[2] == $"`{fixture}`";
        });

    internal static void ChangeJson(string path, Action<JsonNode> change)
    {
        var node = ReadJson(path);
        change(node);
        File.WriteAllText(path, node.ToJsonString());
    }

    internal static void ChangeRows(string directory, Func<string[], string[]> change)
    {
        var path = Path.Combine(directory, "shard-0", "scenarios.tsv");
        File.WriteAllLines(path, change(File.ReadAllLines(path)));
    }

    internal static void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory.GetFiles(source))
        {
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
        }

        foreach (var directory in Directory.GetDirectories(source))
        {
            CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)));
        }
    }
}
