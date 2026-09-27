namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents one branch of an IDL union and its discriminator labels.</summary>
internal sealed class IdlUnionBranch(IdlMember field, IReadOnlyList<string>? labels, IReadOnlyList<int>? labelValues, bool isDefault = false)
{
    public IdlMember Field { get; } = field;
    public IReadOnlyList<string> Labels { get; } = labels ?? [];
    public IReadOnlyList<int> LabelValues { get; } = labelValues ?? [];
    public bool IsDefault { get; } = isDefault;
}

/// <summary>Represents the target-independent semantic form of an IDL union.</summary>
internal sealed class IdlUnion(string name, string? @namespace, string discriminatorIdlType, bool discriminatorIsEnum, IReadOnlyList<IdlUnionBranch> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorIdlType { get; } = discriminatorIdlType;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    public IReadOnlyList<IdlUnionBranch> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
}
