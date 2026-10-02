namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Metadata attached to an IDL member independently of its type.</summary>
internal sealed class IdlMemberValueMetadata(BigInteger? defaultValue = null, BigInteger? minimum = null, BigInteger? maximum = null, string? defaultExpression = null, string? unit = null)
{
    public BigInteger? DefaultValue { get; } = defaultValue;
    public BigInteger? Minimum { get; } = minimum;
    public BigInteger? Maximum { get; } = maximum;
    public string? DefaultExpression { get; } = defaultExpression;
    public string? Unit { get; } = unit;
}

/// <summary>Stores annotations and identity metadata associated with an IDL member.</summary>
internal sealed class IdlMemberMetadata(bool isKey = false, bool isOptional = false, int? memberId = null, IdlMemberValueMetadata? valueMetadata = null, bool isExternal = false, bool isMustUnderstand = false, string? memberIdHashSource = null, bool usesAutoIdHash = false)
{
    public bool IsKey { get; } = isKey;
    public bool IsOptional { get; } = isOptional;
    public int? MemberId { get; } = memberId;
    public IdlMemberValueMetadata? ValueMetadata { get; private set; } = valueMetadata;
    public bool IsExternal { get; } = isExternal;
    public bool IsMustUnderstand { get; } = isMustUnderstand;
    public string? MemberIdHashSource { get; } = memberIdHashSource;
    public bool UsesAutoIdHash { get; } = usesAutoIdHash;

    /// <summary>Applies value metadata resolved after type binding.</summary>
    internal void SetValueMetadata(IdlMemberValueMetadata? valueMetadata) => ValueMetadata = valueMetadata;
}

/// <summary>Represents an IDL member independently of any target language.</summary>
internal sealed class IdlMember(string name, IdlType type, IdlMemberMetadata? metadata = null, IdlInput? sourceInput = null, int sourceOffset = 0)
{
    public string Name { get; } = name;
    public IdlType Type { get; private set; } = type;
    public IdlMemberMetadata Metadata { get; } = metadata ?? new();
    internal IdlInput? SourceInput { get; } = sourceInput;
    internal int SourceOffset { get; } = sourceOffset;

    /// <summary>Replaces this member's deferred type with its bound type.</summary>
    internal void Bind(IdlType type) => Type = type;

    /// <summary>Applies value metadata resolved after type binding.</summary>
    internal void SetValueMetadata(IdlMemberValueMetadata? valueMetadata) => Metadata.SetValueMetadata(valueMetadata);
}
