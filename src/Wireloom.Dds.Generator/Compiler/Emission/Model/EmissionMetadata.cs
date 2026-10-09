using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Describes an IDL field and its projected target types.</summary>
internal sealed class IdlEmissionField(string name, EmissionTypePlan type, EmissionMetadata metadata, EmissionTypePlan? declaredType = null)
{
    public string Name { get; } = name;
    public EmissionTypePlan Type { get; } = type;
    public EmissionTypePlan DeclaredType { get; } = declaredType ?? type;
    public bool IsKey => Metadata.IsKey;
    public int? MemberId => Metadata.MemberId;
    public bool IsOptional => Metadata.IsOptional;
    public IdlMemberValueMetadata? ValueMetadata => Metadata.ValueMetadata;
    public bool IsExternal => Metadata.IsExternal;
    public bool IsMustUnderstand => Metadata.IsMustUnderstand;
    public string? MemberIdHashSource => Metadata.MemberIdHashSource;
    public bool UsesAutoIdHash => Metadata.UsesAutoIdHash;
    private EmissionMetadata Metadata { get; } = metadata;
    public string CSharpType => Type.CSharpType;
    public int? Bound => Type.Bound;
    public string? SupportType => Type.SupportType;
    public EmissionTypePlan? ElementType => Type.Element;
    public string? ElementCSharpType => Type.Element?.CSharpType;
    public string? ElementSupportType => Type.Element?.SupportType;
    public IReadOnlyList<int> Dimensions => Type.Dimensions;
}

/// <summary>Preserves member metadata needed by target-specific emission.</summary>
internal sealed class EmissionMetadata(bool isKey, int? memberId, bool isOptional, IdlMemberValueMetadata? valueMetadata, bool isExternal, bool isMustUnderstand, string? memberIdHashSource, bool usesAutoIdHash)
{
    public bool IsKey { get; } = isKey;
    public int? MemberId { get; } = memberId;
    public bool IsOptional { get; } = isOptional;
    public IdlMemberValueMetadata? ValueMetadata { get; } = valueMetadata;
    public bool IsExternal { get; } = isExternal;
    public bool IsMustUnderstand { get; } = isMustUnderstand;
    public string? MemberIdHashSource { get; } = memberIdHashSource;
    public bool UsesAutoIdHash { get; } = usesAutoIdHash;
}
