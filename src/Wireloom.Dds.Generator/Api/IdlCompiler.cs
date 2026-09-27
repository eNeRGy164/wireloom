using JetBrains.Annotations;

namespace Wireloom;

/// <summary>
/// Compiles the currently supported, deterministic subset of Connext IDL into
/// C# source.
/// </summary>
/// <remarks>
/// This core deliberately has no Roslyn dependency. It rejects unsupported
/// syntax rather than silently producing an incomplete data contract.
/// </remarks>
[PublicAPI]
public static partial class IdlCompiler
{
    /// <summary>
    /// Compiles IDL generation roots and returns one C# document for each
    /// generated type or type-support class.
    /// </summary>
    /// <param name="inputs">All IDL inputs available to the compiler.</param>
    /// <param name="cancellationToken">A token that cancels compilation.</param>
    /// <returns>The generated C# documents keyed by deterministic Roslyn hint name.</returns>
    /// <exception cref="IdlException">
    /// Thrown when an include cannot be resolved, an include is cyclic, or the
    /// input uses an unsupported construct.
    /// </exception>
    public static Dictionary<string, GeneratedIdlSource> CompileSources(
        List<IdlInput> inputs,
        CancellationToken cancellationToken = default) =>
        CompileSourcesWithDiagnostics(inputs, null, cancellationToken);

    /// <summary>
    /// Compiles IDL generation roots and collects non-fatal diagnostics raised
    /// while processing the inputs.
    /// </summary>
    /// <param name="inputs">All IDL inputs available to the compiler.</param>
    /// <param name="diagnostics">The collection that receives compiler diagnostics.</param>
    /// <param name="cancellationToken">A token that cancels compilation.</param>
    /// <returns>The generated C# documents keyed by deterministic Roslyn hint name.</returns>
    /// <exception cref="IdlException">
    /// Thrown when an include cannot be resolved, an include is cyclic, or the
    /// input uses an unsupported construct.
    /// </exception>
    internal static Dictionary<string, GeneratedIdlSource> CompileSourcesWithDiagnostics(
        List<IdlInput> inputs,
        ICollection<IdlDiagnostic>? diagnostics,
        CancellationToken cancellationToken)
    {
        return new IdlCompilation(inputs, cancellationToken)
            .Run(diagnostics)
            .ToDictionary(s => s.HintName, StringComparer.Ordinal);
    }
}
