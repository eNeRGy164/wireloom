namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Identifies specification rules independently of diagnostic severity.</summary>
internal enum IdlSpecificationRule
{
    /// <summary>An exact reserved keyword appears as an identifier.</summary>
    ReservedKeyword,
    /// <summary>An identifier collides with a keyword ignoring case.</summary>
    KeywordCaseCollision,
    /// <summary>A declaration collides with another spelling in its scope.</summary>
    IdentifierCaseCollision,
    /// <summary>A derived aggregate declares a new key.</summary>
    DerivedKey
}

/// <summary>Describes a semantic finding without choosing a validation mode.</summary>
/// <param name="rule">The violated specification rule.</param>
/// <param name="input">The original input containing the violation.</param>
/// <param name="offset">The original source offset.</param>
/// <param name="name">The offending identifier.</param>
/// <param name="conflictingName">The keyword or declaration it conflicts with.</param>
/// <param name="scope">The scope displayed for a declaration collision.</param>
internal sealed class IdlSpecificationViolation(IdlSpecificationRule rule, IdlInput input, int offset, string name, string conflictingName = "", string scope = "")
{
    /// <summary>Gets the violated specification rule.</summary>
    internal IdlSpecificationRule Rule { get; } = rule;
    /// <summary>Gets the original input.</summary>
    internal IdlInput Input { get; } = input;
    /// <summary>Gets the original source offset.</summary>
    internal int Offset { get; } = offset;
    /// <summary>Gets the offending identifier.</summary>
    internal string Name { get; } = name;
    /// <summary>Gets the conflicting keyword or identifier.</summary>
    internal string ConflictingName { get; } = conflictingName;
    /// <summary>Gets the lexical scope display name.</summary>
    internal string Scope { get; } = scope;
}
