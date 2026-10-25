namespace Wireloom;

/// <summary>Represents a diagnostic produced while compiling IDL.</summary>
/// <param name="id">The stable diagnostic identifier.</param>
/// <param name="severity">The diagnostic severity.</param>
/// <param name="input">The input that produced the diagnostic.</param>
/// <param name="offset">The zero-based UTF-16 source offset.</param>
/// <param name="message">The diagnostic message.</param>
[PublicAPI]
public sealed class IdlDiagnostic(string id, IdlDiagnosticSeverity severity, IdlInput input, int offset, string message)
{
    internal IdlDiagnostic(string id, IdlInput input, int offset, string message)
        : this(id, GetDefaultSeverity(id), input, offset, message)
    {
    }

    /// <summary>Gets the diagnostic identifier.</summary>
    public string Id { get; } = id;

    /// <summary>Gets the default severity assigned by the compiler.</summary>
    public IdlDiagnosticSeverity Severity { get; } = severity;

    /// <summary>Gets the source input that produced the diagnostic.</summary>
    public IdlInput Input { get; } = input;

    /// <summary>Gets the source path associated with the diagnostic.</summary>
    public string Path => Input.Path;

    /// <summary>Gets the complete source text associated with the diagnostic.</summary>
    public string SourceText => Input.Text;

    /// <summary>Gets the zero-based UTF-16 offset of the diagnostic.</summary>
    public int Offset { get; } = offset;

    /// <summary>Gets the diagnostic message.</summary>
    public string Message { get; } = message;

    private static IdlDiagnosticSeverity GetDefaultSeverity(string id) => id switch
    {
        "DDSG0001" or "DDSG0004" => IdlDiagnosticSeverity.Error,
        "DDSG0107" => IdlDiagnosticSeverity.Info,
        _ => IdlDiagnosticSeverity.Warning
    };
}
