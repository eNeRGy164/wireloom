namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>
/// Resolves quoted and angle-bracket include operands against the canonical IDL inputs.
/// </summary>
internal sealed class IdlIncludeResolver
{
    private readonly IReadOnlyDictionary<string, IdlInput> files;
    private IReadOnlyList<string> includeDirectories = [];
    private readonly StringComparer comparer;

    public IdlIncludeResolver(IReadOnlyDictionary<string, IdlInput> files, IEnumerable<string> includeDirectories)
    {
        this.files = files;
        comparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        SetIncludeDirectories(includeDirectories);
    }

    public void SetIncludeDirectories(IEnumerable<string> directories)
    {
        includeDirectories = [.. directories
            .Select(Path.GetFullPath)
            .Distinct(comparer)];
    }

    public bool TryResolve(IdlInput including, string includeName, bool angle, out IdlInput input)
    {
        input = null!;

        if (string.IsNullOrWhiteSpace(includeName)
            || includeName.IndexOf('\0') >= 0
            || Path.IsPathRooted(includeName))
        {
            return false;
        }

        try
        {
            var includingDirectory = Path.GetDirectoryName(Path.GetFullPath(including.Path));
            var searchDirectories = angle
                ? includeDirectories
                : new[] { includingDirectory }.Concat(includeDirectories);

            var path = searchDirectories
                .Where(directory => directory is not null)
                .Select(directory => Path.GetFullPath(Path.Combine(directory!, includeName)))
                .Distinct(comparer)
                .FirstOrDefault(files.ContainsKey);

            return path is not null && files.TryGetValue(path, out input);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return false;
        }
    }
}
