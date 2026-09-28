namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>
/// Owns canonical IDL inputs and deterministic include traversal.
/// Parsing and semantic state are deliberately supplied as a callback.
/// </summary>
internal sealed class IdlInputGraph
{
    private const int MaximumIncludeDepth = 128;

    private readonly Dictionary<string, HashSet<string>> parsedOutputs;
    private readonly HashSet<string> onceIncluded;
    private readonly HashSet<string> active;
    private readonly IReadOnlyList<string> batchDefines;
    private readonly IReadOnlyList<string> batchUndefines;
    private readonly IReadOnlyList<string> batchIncludeDirectories;
    private IdlPreprocessor preprocessor;
    private readonly IdlIncludeResolver includeResolver;
    private readonly CancellationToken cancellationToken;

    public IdlInputGraph(
        IReadOnlyDictionary<string, IdlInput> files,
        IEnumerable<string> defines,
        IEnumerable<string> undefines,
        IEnumerable<string> includeDirectories,
        CancellationToken cancellationToken)
    {
        var comparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;

        batchDefines = [.. defines];
        batchUndefines = [.. undefines];
        batchIncludeDirectories = [.. includeDirectories];
        preprocessor = new IdlPreprocessor(batchDefines, batchUndefines, cancellationToken);
        includeResolver = new IdlIncludeResolver(files, batchIncludeDirectories);
        parsedOutputs = new Dictionary<string, HashSet<string>>(comparer);
        onceIncluded = new HashSet<string>(comparer);
        active = new HashSet<string>(comparer);
        this.cancellationToken = cancellationToken;
    }

    /// <summary>Starts a root with an isolated macro environment and search path.</summary>
    public void BeginRoot()
    {
        onceIncluded.Clear();
        active.Clear();
        preprocessor = new IdlPreprocessor(batchDefines, batchUndefines, cancellationToken);
        includeResolver.SetIncludeDirectories(batchIncludeDirectories);
    }

    public void Visit(IdlInput input, Action<string, IdlInput, int, string?, IReadOnlyList<SourceOriginSpan>> parse, ICollection<IdlDiagnostic>? diagnostics = null) =>
        Visit(input, parse, diagnostics, 0);

    private void Visit(IdlInput input, Action<string, IdlInput, int, string?, IReadOnlyList<SourceOriginSpan>> parse, ICollection<IdlDiagnostic>? diagnostics, int includeDepth)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = Path.GetFullPath(input.Path);
        if (includeDepth > MaximumIncludeDepth)
        {
            var message = active.Contains(path)
                ? "Cyclic include detected."
                : "Include nesting exceeds the maximum depth.";
            throw new IdlException(input, 0, message);
        }

        if (onceIncluded.Contains(path))
        {
            return;
        }

        var wasActive = !active.Add(path);

        try
        {
            var result = preprocessor.ProcessWithMetadata(input, (includeName, angle, offset) =>
            {
                if (!includeResolver.TryResolve(input, includeName, angle, out var child))
                {
                    throw new IdlException(input, offset, $"Could not resolve included IDL: {includeName}");
                }

                Visit(child, parse, diagnostics, includeDepth + 1);
            },
            diagnostics,
            () => onceIncluded.Add(path),
            (includeName, angle) => includeResolver.TryResolve(input, includeName, angle, out _));

            if (result.HasPragmaOnce)
            {
                onceIncluded.Add(path);
            }

            // A non-once include may produce different declarations under
            // different macro states. Parse each distinct preprocessed output
            // once, while still replaying the file for macro side effects.
            if (!parsedOutputs.TryGetValue(path, out var outputs))
            {
                outputs = new HashSet<string>(StringComparer.Ordinal);
                parsedOutputs[path] = outputs;
            }

            if (outputs.Add(result.Text))
            {
                parse(result.Text, input, 0, null, result.SourceOrigins);
            }
        }
        finally
        {
            if (!wasActive)
            {
                active.Remove(path);
            }
        }
    }
}
