namespace Wireloom.Dds.Idl;

/// <summary>Contains generated documents and diagnostics from one IDL compilation.</summary>
/// <typeparam name="TDocument">The backend-specific generated document type.</typeparam>
[PublicAPI]
public sealed class IdlCompilationResult<TDocument>
{
    /// <summary>Creates an immutable compiler result.</summary>
    /// <param name="documents">Generated backend documents.</param>
    /// <param name="diagnostics">Diagnostics produced during compilation.</param>
    public IdlCompilationResult(IEnumerable<TDocument> documents, IEnumerable<IdlDiagnostic> diagnostics)
    {
        ArgumentNullException.ThrowIfNull(documents);
        ArgumentNullException.ThrowIfNull(diagnostics);

        Documents = Array.AsReadOnly(documents.ToArray());
        Diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>Gets generated documents in deterministic compiler order.</summary>
    public IReadOnlyList<TDocument> Documents { get; }

    /// <summary>Gets diagnostics collected during compilation.</summary>
    public IReadOnlyList<IdlDiagnostic> Diagnostics { get; }

    /// <summary>Gets whether compilation completed without error diagnostics.</summary>
    public bool Success => Diagnostics.All(d => d.Severity != IdlDiagnosticSeverity.Error);
}
