using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Resolves oracle wrappers and their transitive IDL inputs for peers and provenance validation.</summary>
internal static class IdlSourceResolver
{
    private static readonly Regex IncludePattern = new("""^\s*#\s*include\s*[<"]([^>"]+)[>"]""", RegexOptions.Multiline);

    /// <summary>Locates the optional authored topic wrapper for a corpus case.</summary>
    internal static string WrapperPath(string repositoryRoot, string caseId)
    {
        var name = caseId switch
        {
            "14-autoid-scope" => "14-autoid-scope-topic.idl",
            "14-enum-constant" => "14-enum-constant-topic.idl",
            _ => caseId + ".idl",
        };
        return Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers", name);
    }

    /// <summary>Selects an existing topic wrapper or the canonical corpus IDL.</summary>
    internal static string ResolveOracleIdl(string repositoryRoot, string caseId, string canonicalIdl)
    {
        var wrapper = WrapperPath(repositoryRoot, caseId);
        if (File.Exists(wrapper))
        {
            return Path.GetFullPath(wrapper);
        }

        return Path.GetFullPath(canonicalIdl);
    }

    /// <summary>Returns the unique resolved include closure, excluding the root and sorted by ordinal path.</summary>
    internal static string[] ResolveIncludedIdlFiles(string rootIdl, params IReadOnlyList<string> roots)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootIdl);
        ArgumentNullException.ThrowIfNull(roots);

        var rootPath = Path.GetFullPath(rootIdl);
        var discovered = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { rootPath };
        var pending = new Queue<string>();
        pending.Enqueue(rootPath);
        while (pending.TryDequeue(out var currentIdl))
        {
            foreach (Match match in IncludePattern.Matches(File.ReadAllText(currentIdl)))
            {
                var include = match.Groups[1].Value;
                var candidates = new[] { Path.Combine(Path.GetDirectoryName(currentIdl)!, include) }
                    .Concat(roots.Select(root => Path.Combine(root, include)));
                var source = candidates.FirstOrDefault(File.Exists);
                if (source is not null && discovered.Add(Path.GetFullPath(source)))
                {
                    pending.Enqueue(Path.GetFullPath(source));
                }
            }
        }

        return [.. discovered.Where(path => !string.Equals(path, rootPath, StringComparison.OrdinalIgnoreCase)).Order(StringComparer.Ordinal)];
    }
}
