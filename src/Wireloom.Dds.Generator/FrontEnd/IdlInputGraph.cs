namespace Wireloom;

/// <summary>
/// Owns canonical IDL inputs and deterministic include traversal.
/// Parsing and semantic state are deliberately supplied as a callback.
/// </summary>
internal sealed class IdlInputGraph
{
    private readonly IReadOnlyDictionary<string, IdlInput> files;
    private readonly HashSet<string> visited;
    private readonly HashSet<string> active;
    private readonly IdlPreprocessor preprocessor;
    private readonly IReadOnlyList<string> includeDirectories;
    private readonly CancellationToken cancellationToken;

    public IdlInputGraph(
        IReadOnlyDictionary<string, IdlInput> files,
        IEnumerable<string> defines,
        IEnumerable<string> undefines,
        IEnumerable<string> includeDirectories,
        CancellationToken cancellationToken)
    {
        this.files = files;
        var comparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        preprocessor = new IdlPreprocessor(defines, undefines);
        this.includeDirectories = includeDirectories
            .Select(Path.GetFullPath)
            .Distinct(comparer)
            .ToArray();
        this.cancellationToken = cancellationToken;
        visited = new HashSet<string>(comparer);
        active = new HashSet<string>(comparer);
    }

    public void Visit(IdlInput input, Action<string, IdlInput, int, string?> parse)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = Path.GetFullPath(input.Path);
        if (active.Contains(path))
        {
            throw new IdlException(input, 0, "Cyclic include detected.");
        }

        // A shared include may be reached from several roots. Emit it once so
        // the same IDL type cannot become duplicate generated C#.
        if (!visited.Add(path))
        {
            return;
        }

        active.Add(path);

        try
        {
            var declarations = preprocessor.Process(input, (includeName, angle, offset) =>
            {
                var searchDirectories = angle
                    ? includeDirectories
                    : new[] { Path.GetDirectoryName(path)! }.Concat(includeDirectories);
                var candidates = searchDirectories
                    .Select(directory => Path.GetFullPath(Path.Combine(directory, includeName)))
                    .Distinct(Path.DirectorySeparatorChar == '\\'
                        ? StringComparer.OrdinalIgnoreCase
                        : StringComparer.Ordinal)
                    .ToArray();
                var include = candidates.FirstOrDefault(files.ContainsKey);

                if (include is null || !files.TryGetValue(include, out var child))
                {
                    throw new IdlException(input, offset, $"Could not resolve included IDL: {includeName}");
                }

                Visit(child, parse);
            });

            parse(declarations, input, 0, null);
        }
        finally
        {
            active.Remove(path);
        }
    }
}
