namespace Wireloom;

/// <summary>
/// Compiles the currently supported, deterministic subset of Connext IDL into
/// C# source.
/// </summary>
/// <remarks>
/// This core deliberately has no Roslyn dependency. It rejects unsupported
/// syntax rather than silently producing an incomplete data contract.
/// </remarks>
public static partial class IdlCompiler
{
    /// <summary>
    /// Compiles IDL generation roots and their quoted include closures.
    /// </summary>
    /// <param name="inputs">
    /// All source inputs available to the compiler. Only inputs with
    /// <see cref="IdlInput.Generate"/> set to <see langword="true"/> initiate
    /// generation.
    /// </param>
    /// <param name="cancellationToken">A token that cancels compilation.</param>
    /// <returns>The generated C# source.</returns>
    /// <exception cref="IdlException">
    /// Thrown when an include cannot be resolved, an include is cyclic, or the
    /// input uses an unsupported construct.
    /// </exception>
    public static string Compile(List<IdlInput> inputs, CancellationToken cancellationToken = default)
    {
        return string.Concat(CompileSources(inputs, cancellationToken)
            .Values
            .Select(s => s.Source));
    }

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
    public static Dictionary<string, GeneratedIdlSource> CompileSources(List<IdlInput> inputs, CancellationToken cancellationToken = default)
    {
        return new IdlCompilation(inputs, cancellationToken)
            .Run()
            .ToDictionary(s => s.HintName, StringComparer.Ordinal);
    }
}
