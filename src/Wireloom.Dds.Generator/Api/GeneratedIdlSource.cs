namespace Wireloom;

/// <summary>
/// Represents one generated C# document and its deterministic Roslyn hint name.
/// </summary>
public sealed class GeneratedIdlSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratedIdlSource"/> class.
    /// </summary>
    /// <param name="hintName">The generated document name displayed by Roslyn hosts.</param>
    /// <param name="source">The complete generated C# source.</param>
    public GeneratedIdlSource(string hintName, string source)
    {
        HintName = hintName;
        Source = source;
    }

    /// <summary>
    /// Gets the generated document name displayed by Roslyn hosts.
    /// </summary>
    public string HintName { get; }

    /// <summary>
    /// Gets the complete generated C# source.
    /// </summary>
    public string Source { get; }
}
