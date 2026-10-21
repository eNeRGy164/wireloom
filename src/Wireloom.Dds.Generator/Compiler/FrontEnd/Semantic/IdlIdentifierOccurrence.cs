namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Identifies the lexical scopes used for name validation.</summary>
internal enum IdlIdentifierScopeKind
{
    /// <summary>Names in a module or the global IDL namespace.</summary>
    Namespace,
    /// <summary>Names declared in one struct, valuetype, or union.</summary>
    Aggregate,
    /// <summary>Names declared inside a non-emitted interface or its nested types.</summary>
    Interface,
    /// <summary>Parameter names in one interface operation.</summary>
    Operation,
    /// <summary>Names inside an ordinary interface that RTI omits before specification checks.</summary>
    NonServiceInterface
}

/// <summary>Captures one declared identifier without applying validation policy.</summary>
/// <param name="name">The original, unnormalized spelling.</param>
/// <param name="scope">The lexical scope identity, before C# escaping.</param>
/// <param name="scopeKind">The kind of lexical scope.</param>
/// <param name="input">The original IDL input.</param>
/// <param name="offset">The mapped source offset of the identifier.</param>
internal sealed class IdlIdentifierOccurrence(string name, string scope, IdlIdentifierScopeKind scopeKind, IdlInput input, int offset)
{
    /// <summary>Gets the original identifier spelling.</summary>
    internal string Name { get; } = name;
    /// <summary>Gets the lexical scope identity.</summary>
    internal string Scope { get; } = scope;
    /// <summary>Gets the scope kind.</summary>
    internal IdlIdentifierScopeKind ScopeKind { get; } = scopeKind;
    /// <summary>Gets the original IDL input.</summary>
    internal IdlInput Input { get; } = input;
    /// <summary>Gets the original identifier offset.</summary>
    internal int Offset { get; } = offset;

}
