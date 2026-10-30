using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Immutable source identity captured before a wire execution starts.</summary>
internal sealed record SourceSnapshotEvidence(string Revision, string DirtySnapshotSha256, int DirtyFileCount,
    IReadOnlyList<SourceSnapshotFile> Files, bool VerifiedUnchanged = true, string? IntegrityFailure = null)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>Captures the exact dirty working-tree bytes and retains them beside the run artifacts.</summary>
    internal static SourceSnapshotEvidence Capture(string repositoryRoot, string retainedDirectory)
    {
        var identity = CaptureIdentity(repositoryRoot);
        var retainedFiles = new List<SourceSnapshotFile>(identity.Files.Count);
        foreach (var file in identity.Files)
        {
            string? retainedPath = null;
            if (file.Status != "deleted")
            {
                var sourcePath = ResolvePath(repositoryRoot, file.Path);
                retainedPath = Path.Combine("source-files", file.Path.Replace('/', Path.DirectorySeparatorChar));
                var destination = Path.Combine(retainedDirectory, retainedPath);
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.Copy(sourcePath, destination, overwrite: true);
            }

            retainedFiles.Add(file with { RetainedPath = retainedPath });
        }

        return identity with { Files = retainedFiles };
    }

    /// <summary>Captures source identity without retaining a second copy of the source files.</summary>
    internal static SourceSnapshotEvidence CaptureIdentity(string repositoryRoot)
    {
        var revision = RunGit(repositoryRoot, "rev-parse", "HEAD").Trim();
        var changed = ReadPorcelainStatus(RunGit(repositoryRoot, "status", "--porcelain=v1", "-z", "--untracked-files=all", "--no-renames"));

        var files = changed.OrderBy(item => item.Key, StringComparer.Ordinal)
            .Select(item => new SourceSnapshotFile(item.Key.Replace('\\', '/'), item.Value,
                item.Value == "deleted" ? "DELETED" : HashFile(ResolvePath(repositoryRoot, item.Key)), null))
            .ToArray();
        var hash = ComputeSnapshotHash(files);
        return new SourceSnapshotEvidence(revision, hash, files.Length, files);
    }

    /// <summary>Loads and validates retained execution provenance without consulting Git.</summary>
    internal static SourceSnapshotEvidence Read(string path)
    {
        if (!File.Exists(path))
        {
            throw new InvalidDataException($"Execution provenance is missing: {path}");
        }

        SourceSnapshotEvidence evidence;
        try
        {
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object
                || !root.TryGetProperty("revision", out var revision) || revision.ValueKind != JsonValueKind.String
                || !root.TryGetProperty("dirtySnapshotSha256", out var snapshotHash) || snapshotHash.ValueKind != JsonValueKind.String
                || !root.TryGetProperty("dirtyFileCount", out var fileCount) || fileCount.ValueKind != JsonValueKind.Number
                || !root.TryGetProperty("files", out var files) || files.ValueKind != JsonValueKind.Array
                || !root.TryGetProperty("verifiedUnchanged", out var verifiedUnchanged) || verifiedUnchanged.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
            {
                throw new InvalidDataException("Execution provenance is missing required identity fields.");
            }

            if (files.EnumerateArray().Any(file => file.ValueKind != JsonValueKind.Object
                || !file.TryGetProperty("path", out var pathValue) || pathValue.ValueKind != JsonValueKind.String
                || !file.TryGetProperty("status", out var statusValue) || statusValue.ValueKind != JsonValueKind.String
                || !file.TryGetProperty("sha256", out var hashValue) || hashValue.ValueKind != JsonValueKind.String))
            {
                throw new InvalidDataException("Execution provenance contains a malformed source file entry.");
            }

            evidence = root.Deserialize<SourceSnapshotEvidence>(JsonOptions)
                ?? throw new InvalidDataException("Execution provenance is empty or malformed.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("Execution provenance is malformed JSON.", exception);
        }

        if (string.IsNullOrWhiteSpace(evidence.Revision) || evidence.DirtySnapshotSha256.Length != 64
            || evidence.Files.Count != evidence.DirtyFileCount
            || !string.Equals(ComputeSnapshotHash(evidence.Files), evidence.DirtySnapshotSha256, StringComparison.Ordinal))
        {
            throw new InvalidDataException("Execution provenance is incomplete or its source manifest hash is invalid.");
        }

        var uniquePaths = new HashSet<string>(StringComparer.Ordinal);
        foreach (var file in evidence.Files)
        {
            if (!uniquePaths.Add(file.Path) || Path.IsPathRooted(file.Path)
                || file.Path.Split('/').Any(segment => segment is ".." or ""))
            {
                throw new InvalidDataException("Execution provenance contains an unsafe or duplicate source path.");
            }

            if (file.Status != "deleted")
            {
                if (string.IsNullOrWhiteSpace(file.RetainedPath) || Path.IsPathRooted(file.RetainedPath)
                    || file.RetainedPath.Split('/').Any(segment => segment is ".." or ""))
                {
                    throw new InvalidDataException($"Execution provenance is missing retained bytes for {file.Path}.");
                }

                var retainedRoot = Path.GetDirectoryName(Path.GetFullPath(path))!;
                var retainedFile = Path.GetFullPath(Path.Combine(retainedRoot, file.RetainedPath.Replace('/', Path.DirectorySeparatorChar)));
                if (!retainedFile.StartsWith(retainedRoot + Path.DirectorySeparatorChar,
                    OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                    || !File.Exists(retainedFile) || HashFile(retainedFile) != file.Sha256)
                {
                    throw new InvalidDataException($"Retained source bytes are missing or corrupt for {file.Path}.");
                }
            }
        }

        return evidence;
    }

    /// <summary>Writes the retained identity and file manifest as deterministic UTF-8 JSON.</summary>
    internal void Write(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonOptions) + "\n", new UTF8Encoding(false));
    }

    /// <summary>Checks whether the live checkout still matches this execution identity.</summary>
    internal bool MatchesCurrent(string repositoryRoot)
    {
        var current = CaptureIdentity(repositoryRoot);
        return Revision == current.Revision && DirtySnapshotSha256 == current.DirtySnapshotSha256
            && DirtyFileCount == current.DirtyFileCount;
    }

    internal static string ComputeSnapshotHash(IEnumerable<SourceSnapshotFile> files)
    {
        using var snapshot = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (var file in files.OrderBy(file => file.Path, StringComparer.Ordinal))
        {
            var entry = Encoding.UTF8.GetBytes($"{file.Path.Replace('\\', '/') }\0{file.Status}\0{file.Sha256}\n");
            snapshot.AppendData(entry);
        }

        return Convert.ToHexString(snapshot.GetHashAndReset());
    }

    private static Dictionary<string, string> ReadPorcelainStatus(string output)
    {
        var fields = output.Split('\0', StringSplitOptions.RemoveEmptyEntries);
        var paths = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var field in fields)
        {
            if (field.Length < 4)
            {
                throw new InvalidDataException("Git returned an invalid dirty-source status record.");
            }

            var status = field[..2];
            var path = field[3..];
            paths[path] = status.Contains('D', StringComparison.Ordinal)
                ? "deleted"
                : status == "??" || status.Contains('A', StringComparison.Ordinal) ? "added" : "modified";
        }

        return paths;
    }

    private static string ResolvePath(string repositoryRoot, string relativePath)
    {
        var path = Path.GetFullPath(Path.Combine(repositoryRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));
        var root = Path.GetFullPath(repositoryRoot) + Path.DirectorySeparatorChar;
        if (!path.StartsWith(root, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
        {
            throw new InvalidDataException($"Dirty source path escapes the repository: {relativePath}");
        }

        return path;
    }

    private static string RunGit(string repositoryRoot, params string[] arguments)
    {
        var start = new ProcessStartInfo("git")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        start.ArgumentList.Add("-C");
        start.ArgumentList.Add(repositoryRoot);
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not start git to capture source provenance.");
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Could not capture source provenance from '{repositoryRoot}': {error.Trim()}");
        }

        return output;
    }

    private static string HashFile(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream));
    }
}

/// <summary>A dirty source path and the hash of its exact retained bytes.</summary>
/// <param name="Path">Repository-relative UTF-8 path with slash separators.</param>
/// <param name="Status">Git change kind: added, modified, or deleted.</param>
/// <param name="Sha256">Hash of exact file bytes, or DELETED.</param>
/// <param name="RetainedPath">Relative path to the preserved file bytes, when present.</param>
internal sealed record SourceSnapshotFile(string Path, string Status, string Sha256, string? RetainedPath);
