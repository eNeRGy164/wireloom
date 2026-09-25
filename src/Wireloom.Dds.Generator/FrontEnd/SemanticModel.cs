namespace Wireloom;

internal enum IdlCollectionKind
{
    Sequence,
    Array
}

internal enum IdlExtensibilityKind
{
    Extensible,
    Final,
    Mutable
}

/// <summary>
/// The target-independent semantic type system produced by the IDL front end.
/// It must not contain generated-language or RTI-specific names.
/// </summary>
internal abstract class IdlType
{
    private IdlType()
    {
    }

    public sealed class Primitive(string name) : IdlType
    {
        public string Name { get; } = name;
    }

    public sealed class StringType(bool isWide, int bound) : IdlType
    {
        public bool IsWide { get; } = isWide;
        public int Bound { get; } = bound;
    }

    public sealed class Enum(string qualifiedName) : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
    }

    public sealed class Struct(string qualifiedName) : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
    }

    public sealed class Alias(string qualifiedName, IdlType target) : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
        public IdlType Target { get; } = target;
    }

    public sealed class Sequence(IdlType element, int? bound) : IdlType
    {
        public IdlType Element { get; } = element;
        public int? Bound { get; } = bound;
    }

    public sealed class Array(IdlType element, IReadOnlyList<int> dimensions) : IdlType
    {
        public IdlType Element { get; } = element;
        public IReadOnlyList<int> Dimensions { get; } = dimensions;
    }
}

/// <summary>Metadata attached to an IDL member independently of its type.</summary>
internal sealed class IdlMemberMetadata(bool isKey = false, bool isOptional = false, int? memberId = null)
{
    public bool IsKey { get; } = isKey;
    public bool IsOptional { get; } = isOptional;
    public int? MemberId { get; } = memberId;
}

/// <summary>Represents an IDL member independently of any target language.</summary>
internal sealed class IdlMember(string name, IdlType type, IdlMemberMetadata? metadata = null)
{
    public string Name { get; } = name;
    public IdlType Type { get; } = type;
    public IdlMemberMetadata Metadata { get; } = metadata ?? new();

    public IdlMember WithName(string name, bool? isKey = null, int? memberId = null) =>
        new(
            name,
            Type,
            new IdlMemberMetadata(
                isKey ?? Metadata.IsKey,
                Metadata.IsOptional,
                memberId ?? Metadata.MemberId));
}

internal sealed class IdlEnumMember(string name, int value, bool hasExplicitValue)
{
    public string Name { get; } = name;
    public int Value { get; } = value;
    public bool HasExplicitValue { get; } = hasExplicitValue;
}

internal sealed class IdlEnum(string name, string? @namespace, IReadOnlyList<IdlEnumMember> members, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public IReadOnlyList<IdlEnumMember> Members { get; } = members;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
}

/// <summary>Raw typedef facts retained until semantic type resolution.</summary>
internal sealed class IdlTypedef(string name, string? @namespace, string target, string? elementType, int? bound, IReadOnlyList<int>? dimensions = null, int? stringBound = null, bool isWideString = false)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string Target { get; } = target;
    public bool IsSequence => Target == "sequence";
    public bool IsArray => Target == "array";
    public bool IsCollection => IsSequence || IsArray;
    public string? ElementType { get; } = elementType;
    public int? Bound { get; } = bound;
    public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
    public bool IsString => Target is "string" or "wstring";
    public bool IsWideString { get; } = isWideString;
    public int StringBound { get; } = stringBound ?? 255;
}

internal sealed class IdlUnionBranch(IdlMember field, IReadOnlyList<string>? labels, IReadOnlyList<int>? labelValues, bool isDefault = false)
{
    public IdlMember Field { get; } = field;
    public IReadOnlyList<string> Labels { get; } = labels ?? [];
    public IReadOnlyList<int> LabelValues { get; } = labelValues ?? [];
    public bool IsDefault { get; } = isDefault;
    public string? Label => IsDefault ? null : Labels[0];
}

internal sealed class IdlUnion(string name, string? @namespace, string discriminatorIdlType, bool discriminatorIsEnum, IReadOnlyList<IdlUnionBranch> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorIdlType { get; } = discriminatorIdlType;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    public IReadOnlyList<IdlUnionBranch> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
}
