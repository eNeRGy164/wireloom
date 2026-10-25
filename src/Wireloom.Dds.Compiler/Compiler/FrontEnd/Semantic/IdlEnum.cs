namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents one member of an IDL enum.</summary>
internal sealed class IdlEnumMember(string name, int value, bool hasExplicitValue, bool isDefaultLiteral)
{
    public string Name { get; } = name;
    public int Value { get; } = value;
    public bool HasExplicitValue { get; } = hasExplicitValue;
    public bool IsDefaultLiteral { get; } = isDefaultLiteral;
}

/// <summary>Represents the target-independent semantic form of an IDL enum.</summary>
internal sealed class IdlEnum(string name, string? @namespace, IReadOnlyList<IdlEnumMember> members, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public IReadOnlyList<IdlEnumMember> Members { get; } = members;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    public IdlEnumMember DefaultMember => Members.SingleOrDefault(member => member.IsDefaultLiteral) ?? Members[0];
}
