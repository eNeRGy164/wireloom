namespace Wireloom.Dds.CSharp.Compiler;

/// <summary>
/// Represents one generated C# document and its deterministic Roslyn hint name.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeneratedIdlSource"/> class.
/// </remarks>
/// <param name="hintName">The generated document name displayed by Roslyn hosts.</param>
/// <param name="source">The complete generated C# source.</param>
[PublicAPI]
public sealed class GeneratedIdlSource(string hintName, string source)
{
    /// <summary>
    /// Gets the generated document name displayed by Roslyn hosts.
    /// </summary>
    public string HintName { get; } = hintName;

    /// <summary>
    /// Gets the complete generated C# source.
    /// </summary>
    public string Source { get; } = source;
}
