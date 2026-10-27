using System.Diagnostics;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Runs child processes with one consistent timeout, output, and failure policy.</summary>
internal sealed class ProcessRunner(string workingDirectory)
{
    /// <summary>Executes a command, optionally retaining its diagnostics or allowing a handled nonzero exit.</summary>
    internal async Task<ProcessResult> RunAsync(string executable, IEnumerable<string> arguments,
        string? logPath = null, TimeSpan? timeout = null, bool allowFailure = false)
    {
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {executable}.");
        using var cancellation = new CancellationTokenSource(timeout ?? TimeSpan.FromMinutes(30));
        var stdout = process.StandardOutput.ReadToEndAsync(cancellation.Token);
        var stderr = process.StandardError.ReadToEndAsync(cancellation.Token);
        try
        {
            await process.WaitForExitAsync(cancellation.Token);
        }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None);
            throw new TimeoutException($"{executable} exceeded its timeout.");
        }

        var result = new ProcessResult(process.ExitCode, await stdout, await stderr);
        if (logPath is not null)
        {
            await File.WriteAllTextAsync(logPath, result.Output + result.Error, cancellation.Token);
        }

        if (result.Output.Length > 0)
        {
            Console.Write(result.Output);
        }

        if (!result.Succeeded && !allowFailure)
        {
            await Console.Error.WriteAsync(result.Error);
            throw new InvalidOperationException($"{executable} failed with exit code {result.ExitCode}.");
        }

        return result;
    }
}

/// <summary>Captured child-process completion.</summary>
/// <param name="ExitCode">Operating-system exit code.</param>
/// <param name="Output">Standard output.</param>
/// <param name="Error">Standard error.</param>
internal sealed record ProcessResult(int ExitCode, string Output, string Error)
{
    /// <summary>Whether the command completed successfully.</summary>
    internal bool Succeeded => ExitCode == 0;
}
