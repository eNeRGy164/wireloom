using System.Diagnostics;
namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Verifies deterministic exact-byte provenance capture and retained evidence reads.</summary>
internal static class SourceSnapshotVerification
{
    internal static void Run(string directory)
    {
        // Arrange: create a tiny repository with one modified, one deleted, and one Unicode untracked path.
        var repository = Path.Combine(directory, "source-snapshot-repository");
        Directory.CreateDirectory(repository);
        RunGit(repository, "init", "--quiet");
        RunGit(repository, "config", "user.email", "verification@wireloom.invalid");
        RunGit(repository, "config", "user.name", "Wireloom Verification");
        File.WriteAllText(Path.Combine(repository, "modified.txt"), "baseline\n");
        File.WriteAllText(Path.Combine(repository, "deleted.txt"), "baseline\n");
        RunGit(repository, "add", "modified.txt", "deleted.txt");
        RunGit(repository, "commit", "--quiet", "-m", "baseline");
        File.WriteAllText(Path.Combine(repository, "modified.txt"), "changed bytes\r\n");
        File.Delete(Path.Combine(repository, "deleted.txt"));
        File.WriteAllText(Path.Combine(repository, "untracked-Ω.txt"), "new bytes\n");

        var retained = Path.Combine(directory, "retained-execution");

        // Act: capture, serialize, reload, and repeat the deterministic digest calculation.
        var captured = SourceSnapshotEvidence.Capture(repository, retained);
        captured.Write(Path.Combine(retained, "execution-provenance.json"));
        var loaded = SourceSnapshotEvidence.Read(Path.Combine(retained, "execution-provenance.json"));
        var repeated = SourceSnapshotEvidence.CaptureIdentity(repository);

        // Assert: paths, status, exact bytes, and content hashes survive independently of Git.
        Check(captured.DirtySnapshotSha256 == repeated.DirtySnapshotSha256, "The same dirty source bytes must produce a deterministic snapshot hash.");
        Check(captured.Files.Count == 3 && loaded.Files.Count == 3, "The manifest must include modified, deleted, and untracked paths.");
        Check(loaded.Files.Single(file => file.Path == "deleted.txt").Sha256 == "DELETED", "Deleted paths must remain explicit in the manifest.");
        Check(loaded.Files.Single(file => file.Path == "modified.txt").Status == "modified", "Modified source paths must be classified.");
        Check(loaded.Files.Single(file => file.Path == "untracked-Ω.txt").Status == "added", "UTF-8 untracked paths must be retained safely.");
        Check(loaded.MatchesCurrent(repository), "The captured manifest must match the unchanged execution checkout.");

        var priorPath = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        try
        {
            Environment.SetEnvironmentVariable("PATH", string.Empty);
            var withoutGit = SourceSnapshotEvidence.Read(Path.Combine(retained, "execution-provenance.json"));
            Check(withoutGit.DirtySnapshotSha256 == captured.DirtySnapshotSha256, "Reading retained evidence must not require a Git executable.");
        }
        finally
        {
            Environment.SetEnvironmentVariable("PATH", priorPath);
        }

        File.WriteAllText(Path.Combine(repository, "modified.txt"), "changed again\n");
        Check(!loaded.MatchesCurrent(repository), "A source change during execution must invalidate the successful run identity.");

        Console.WriteLine("Source snapshot provenance passed: deterministic exact-byte hash, modified/deleted/untracked paths, Unicode filenames, retained source bytes, checkout drift, and Git-free reads.");
    }

    private static void RunGit(string repository, params string[] arguments)
    {
        var start = new ProcessStartInfo("git")
        {
            WorkingDirectory = repository,
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not start Git for source snapshot verification.");
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Git source snapshot verification failed: {error.Trim()}");
        }
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
