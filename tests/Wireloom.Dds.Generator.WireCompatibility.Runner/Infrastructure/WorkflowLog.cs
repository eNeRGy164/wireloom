using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns elapsed progress messages and GitHub Actions presentation state.</summary>
internal sealed class WorkflowLog
{
    private readonly Stopwatch elapsed = Stopwatch.StartNew();
    private readonly bool isGitHubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
    private bool groupOpen;

    /// <summary>Seconds since the application started.</summary>
    internal double ElapsedSeconds => elapsed.Elapsed.TotalSeconds;

    /// <summary>Opens a workflow log group when GitHub Actions supports it.</summary>
    internal void StartGroup(string name)
    {
        if (isGitHubActions && !groupOpen)
        {
            Console.WriteLine($"::group::{Escape(name)}");
            groupOpen = true;
        }
    }

    /// <summary>Closes the current group, including after an exception.</summary>
    internal void EndGroup()
    {
        if (groupOpen)
        {
            Console.WriteLine("::endgroup::");
            groupOpen = false;
        }
    }

    /// <summary>Emits a timed phase marker understood by matrix performance analysis.</summary>
    internal void Phase(string name, string details) => Console.WriteLine($"WIRE_MATRIX_PHASE {name} {details} elapsed={ElapsedSeconds:F0}s");

    /// <summary>Emits a workflow annotation without changing exit policy.</summary>
    internal void Annotate(string level, string title, string message)
    {
        if (isGitHubActions)
        {
            Console.WriteLine($"::{level} title={Escape(title)}::{Escape(message)}");
        }
    }

    /// <summary>Surfaces the first C# error while retaining the complete build log.</summary>
    internal static void BuildFailure(string label, string? path)
    {
        Console.Error.WriteLine($"{label}; see {path}");
        if (path is null || !File.Exists(path))
        {
            return;
        }

        var match = Regex.Match(File.ReadAllText(path), @": error (CS[0-9]+: .*) \[.*$", RegexOptions.Multiline);
        if (match.Success)
        {
            Console.Error.WriteLine(match.Groups[1].Value);
        }
    }

    private static string Escape(string value) => value.Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A");
}
