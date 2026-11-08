namespace Wireloom;

/// <summary>Describes the default severity of an IDL compiler diagnostic.</summary>
[PublicAPI]
public enum IdlDiagnosticSeverity
{
    /// <summary>Informational message that does not prevent generation.</summary>
    Info,

    /// <summary>Warning that does not prevent generation.</summary>
    Warning,

    /// <summary>Error that prevents successful generation.</summary>
    Error
}
