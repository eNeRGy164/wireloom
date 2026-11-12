namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Represents one member of an IDL enum.</summary>
public sealed class IdlEnumMember(string name, int value, bool hasExplicitValue, bool isDefaultLiteral)
{
    /// <summary>Gets the enumerator's declared IDL name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the enumerator's resolved integer value.</summary>
    public int Value { get; } = value;
    /// <summary>Gets whether the source explicitly assigned the value.</summary>
    public bool HasExplicitValue { get; } = hasExplicitValue;
    /// <summary>Gets whether this enumerator is marked as the default literal.</summary>
    public bool IsDefaultLiteral { get; } = isDefaultLiteral;
}

/// <summary>Represents the target-independent semantic form of an IDL enum.</summary>
public sealed class IdlEnum(string name, string? @namespace, IReadOnlyList<IdlEnumMember> members, IdlExtensibilityKind extensibility)
{
    /// <summary>Gets the enum's declared IDL name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the enclosing namespace, or null for the global namespace.</summary>
    public string? Namespace { get; } = @namespace;
    /// <summary>Gets the enum members in declaration order.</summary>
    public IReadOnlyList<IdlEnumMember> Members { get; } = members;
    /// <summary>Gets the XTypes extensibility policy.</summary>
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    /// <summary>Gets the default literal, or the first member when none is marked.</summary>
    public IdlEnumMember DefaultMember => Members.SingleOrDefault(member => member.IsDefaultLiteral) ?? Members[0];
}
