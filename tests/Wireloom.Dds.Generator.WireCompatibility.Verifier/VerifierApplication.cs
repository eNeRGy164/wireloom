using Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier;

/// <summary>Composes and runs the black-box wire-matrix verification stages.</summary>
internal static class VerifierApplication
{
    internal static int Run(string[] args)
    {
        if (args.Length > 0)
        {
            return VerifierCommandApplication.Run(args);
        }

        var verifierAssembly = typeof(VerifierApplication).Assembly.Location;
        var repositoryRoot = FindRepositoryRoot(verifierAssembly);

        using var workspace = new VerifierWorkspace(Path.Combine(repositoryRoot, "tmp"));

        var context = new VerificationContext(verifierAssembly, repositoryRoot, workspace.Root, Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0");

        try
        {
            CaseSelectionVerification.Run(context);

            var scenarios = ShardScenarioBuilder.Build(context);
            var report = ShardReportVerification.Run(context, scenarios);

        ShardIntegrityVerification.Run(context, scenarios, report);

            Console.WriteLine($"Shard verification passed: {context.CheckCount} checks, {scenarios.Rows.Count} exchanges, no RTI license required.");

            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
    }

    private static string FindRepositoryRoot(string path)
    {
        // CI builds the verifier under RUNNER_TEMP, outside the checkout.
        foreach (var startPath in new[] { Environment.CurrentDirectory, Path.GetDirectoryName(Path.GetFullPath(path))! })
        {
            var directory = new DirectoryInfo(startPath);
            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "docs", "corpus", "manifest.json")))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the repository corpus manifest.");
    }
}
