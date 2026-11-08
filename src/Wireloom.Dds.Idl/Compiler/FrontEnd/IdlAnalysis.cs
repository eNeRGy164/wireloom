using Wireloom.Compiler.FrontEnd.Parsing;
using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd;

/// <summary>Builds the bound IDL model independently of target emission.</summary>
internal static class IdlAnalysis
{
    /// <summary>Preprocesses, parses, binds, and validates a batch of IDL inputs.</summary>
    /// <param name="inputs">The caller's stable snapshot of roots and supplied include inputs.</param>
    /// <param name="computeMd5">The host-provided member-ID digest implementation.</param>
    /// <param name="cancellationToken">Cancels input traversal and language analysis.</param>
    /// <param name="diagnostics">Collects source-located diagnostics reported during analysis.</param>
    /// <param name="targetChecks">Optional checks interleaved with analysis to preserve target diagnostic order.</param>
    /// <returns>The bound model, in deterministic traversal order.</returns>
    /// <exception cref="IdlException">A fatal IDL diagnostic prevented analysis.</exception>
    internal static BoundIdlCompilation Analyze(IReadOnlyList<IdlInput> inputs, Func<byte[], byte[]> computeMd5,
        CancellationToken cancellationToken, ICollection<IdlDiagnostic>? diagnostics = null, IdlTargetChecks? targetChecks = null)
    {
        var comparer = Path.DirectorySeparatorChar == '\\' ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var files = new Dictionary<string, IdlInput>(comparer);
        var roots = inputs.Where(input => input.Generate).ToArray();

        foreach (var file in inputs)
        {
            targetChecks?.ValidateInput(file);
            files[Path.GetFullPath(file.Path)] = file;
        }

        var escapeSettings = roots.Select(input => input.EnableEscapeChar).Distinct().ToArray();
        if (escapeSettings.Length > 1)
        {
            throw new IdlException(roots[0], 0, "EnableEscapeChar must have the same value for every generation root in a compilation batch.");
        }

        var enableEscapeChar = escapeSettings.FirstOrDefault();
        var symbols = new IdlSymbolTable();
        var graph = new IdlInputGraph(files, roots.SelectMany(input => input.Defines), roots.SelectMany(input => input.Undefines),
            roots.SelectMany(input => input.IncludeDirectories), cancellationToken, enableEscapeChar);
        var parser = new IdlDeclarationParser(symbols, cancellationToken, computeMd5, diagnostics, targetChecks);

        // Root order stays stable when a host supplies inputs in a different order.
        foreach (var input in roots.OrderBy(input => input.Path, StringComparer.Ordinal))
        {
            graph.BeginRoot();
            graph.Visit(input, (text, file, offset, currentNamespace, origins, escapedIdentifierOffsets) =>
                parser.Parse(text, file, offset, currentNamespace, origins, escapedIdentifierOffsets), diagnostics);
        }

        parser.Bind();
        parser.Validate(roots.Any(input => input.Strict));

        return new BoundIdlCompilation(parser.Declarations, symbols);
    }
}

/// <summary>Contains the declarations and lexical symbols resolved by one batch of IDL analysis.</summary>
/// <param name="declarations">The bound declarations in traversal order.</param>
/// <param name="symbols">The lexical symbol table for type and value resolution.</param>
internal sealed class BoundIdlCompilation(IReadOnlyList<IdlDeclaration> declarations, IdlSymbolTable symbols)
{
    /// <summary>Gets the bound declarations.</summary>
    internal IReadOnlyList<IdlDeclaration> Declarations { get; } = Array.AsReadOnly(declarations.ToArray());

    /// <summary>Gets the lexical symbols owned by this analysis.</summary>
    internal IdlSymbolTable Symbols { get; } = symbols;

}
