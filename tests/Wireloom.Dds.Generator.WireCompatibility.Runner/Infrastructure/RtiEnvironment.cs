using System.Diagnostics;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Imports and validates the selected RTI container environment.</summary>
internal sealed class RtiEnvironment(string repositoryRoot)
{
    /// <summary>Initializes RTI when necessary and verifies license and development-library paths.</summary>
    internal void Initialize()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("NDDSHOME")))
        {
            ImportEnvironment();
        }

        var license = Environment.GetEnvironmentVariable("RTI_LICENSE_FILE");
        ArgumentException.ThrowIfNullOrWhiteSpace(license);
        if (!File.Exists(license))
        {
            throw new ArgumentException("RTI_LICENSE_FILE must name a readable RTI license file.");
        }

        var nddsHome = Environment.GetEnvironmentVariable("NDDSHOME");
        ArgumentException.ThrowIfNullOrWhiteSpace(nddsHome);
        if (!Directory.Exists(Path.Combine(nddsHome, "include")) ||
            !Directory.Exists(Path.Combine(nddsHome, "resource", "cmake")))
        {
            throw new ArgumentException("NDDSHOME must point to the selected RTI development libraries and CMake modules.");
        }
    }

    private void ImportEnvironment()
    {
        // Parse environment output locally; it can contain values that must not enter process logs.
        var start = new ProcessStartInfo("bash")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        start.ArgumentList.Add("-lc");
        start.ArgumentList.Add("eval \"$(rtienv -l \"$RTI_LICENSE_FILE\")\" && env -0");

        using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not initialize the RTI environment.");
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"RTI environment setup failed: {stderr.Trim()}");
        }

        foreach (var entry in stdout.Split('\0', StringSplitOptions.RemoveEmptyEntries))
        {
            var separator = entry.IndexOf('=');
            if (separator > 0)
            {
                Environment.SetEnvironmentVariable(entry[..separator], entry[(separator + 1)..]);
            }
        }
    }
}
