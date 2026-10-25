namespace Wireloom;

/// <summary>Contains generated documents and diagnostics from one IDL compilation.</summary>
[PublicAPI]
public sealed class IdlCompilationResult
{
    internal IdlCompilationResult(IEnumerable<GeneratedIdlSource> documents, IEnumerable<IdlDiagnostic> diagnostics)
    {
        Documents = Array.AsReadOnly(documents.ToArray());
        Diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>Gets generated C# documents in deterministic compiler order.</summary>
    public IReadOnlyList<GeneratedIdlSource> Documents { get; }

    /// <summary>Gets diagnostics collected during compilation.</summary>
    public IReadOnlyList<IdlDiagnostic> Diagnostics { get; }

    /// <summary>Gets whether compilation completed without error diagnostics.</summary>
    public bool Success => Diagnostics.All(diagnostic => diagnostic.Severity != IdlDiagnosticSeverity.Error);
}
