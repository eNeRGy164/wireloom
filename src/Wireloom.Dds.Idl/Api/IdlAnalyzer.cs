using Wireloom.Dds.Idl.Compiler.FrontEnd;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Idl;

/// <summary>Analyzes IDL inputs into the shared target-independent semantic model.</summary>
/// <remarks>This operation performs no target mapping or source emission.</remarks>
[PublicAPI]
public static class IdlAnalyzer
{
    /// <summary>Builds a bound declaration model for the supplied IDL inputs.</summary>
    /// <param name="inputs">The generation roots and supplied include inputs.</param>
    /// <param name="computeMd5">The host-provided member-ID digest implementation.</param>
    /// <param name="cancellationToken">Cancels input traversal and language analysis.</param>
    /// <param name="diagnostics">Collects source-located diagnostics reported during analysis.</param>
    /// <returns>The declarations in deterministic traversal order.</returns>
    /// <remarks>
    /// Supply included files in <paramref name="inputs"/>; analysis does not read from disk.
    /// The digest provider must return the complete 16-byte MD5 digest used for hashed member IDs.
    /// </remarks>
    /// <exception cref="IdlException">A fatal IDL diagnostic prevented analysis.</exception>
    public static IdlAnalysisResult Analyze(
        IReadOnlyList<IdlInput> inputs,
        Func<byte[], byte[]> computeMd5,
        CancellationToken cancellationToken,
        ICollection<IdlDiagnostic>? diagnostics = null)
    {
        var analysis = IdlAnalysis.Analyze(inputs, computeMd5, cancellationToken, diagnostics);
        return new IdlAnalysisResult(analysis.Declarations);
    }
}

/// <summary>Contains the declarations resolved by one IDL analysis operation.</summary>
[PublicAPI]
public sealed class IdlAnalysisResult
{
    internal IdlAnalysisResult(IReadOnlyList<IdlDeclaration> declarations) =>
        Declarations = Array.AsReadOnly(declarations.ToArray());

    /// <summary>Gets the bound declarations in deterministic traversal order.</summary>
    public IReadOnlyList<IdlDeclaration> Declarations { get; }
}
