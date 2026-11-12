using Wireloom.Dds.Idl.Compiler.FrontEnd.Parsing;

namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Metadata attached to an IDL member independently of its type.</summary>
public sealed class IdlMemberValueMetadata(IdlScalarValue? defaultValue = null, BigInteger? minimum = null, BigInteger? maximum = null, string? defaultExpression = null, string? unit = null)
{
    /// <summary>Gets the effective typed default or null when no default applies.</summary>
    public IdlScalarValue? DefaultValue { get; } = defaultValue;
    /// <summary>Gets the inclusive minimum value annotation, if present.</summary>
    public BigInteger? Minimum { get; } = minimum;
    /// <summary>Gets the inclusive maximum value annotation, if present.</summary>
    public BigInteger? Maximum { get; } = maximum;
    /// <summary>Gets the source expression for the declared default value, if present.</summary>
    public string? DefaultExpression { get; } = defaultExpression;
    /// <summary>Gets the unit annotation, if present.</summary>
    public string? Unit { get; } = unit;

    /// <summary>Gets whether a typed default applies.</summary>
    public bool HasDefault => DefaultValue is not null;
}

/// <summary>Stores annotations and identity metadata associated with an IDL member.</summary>
public sealed class IdlMemberMetadata(bool isKey = false, bool isOptional = false, int? memberId = null, IdlMemberValueMetadata? valueMetadata = null, bool isExternal = false, bool isMustUnderstand = false, string? memberIdHashSource = null, bool usesAutoIdHash = false)
{
    /// <summary>Gets whether the member is part of the DDS key.</summary>
    public bool IsKey { get; private set; } = isKey;
    /// <summary>Gets whether the member may be absent from a value.</summary>
    public bool IsOptional { get; private set; } = isOptional;
    /// <summary>Gets the explicitly declared member identifier, if any.</summary>
    public int? MemberId { get; private set; } = memberId;

    /// <summary>Gets the effective identifier assigned during binding, including sequential IDs.</summary>
    public int? EffectiveMemberId { get; private set; } = memberId;

    /// <summary>Gets default, range, and unit metadata associated with the member.</summary>
    public IdlMemberValueMetadata? ValueMetadata { get; private set; } = valueMetadata;
    /// <summary>Gets whether the member is external to the enclosing value.</summary>
    public bool IsExternal { get; private set; } = isExternal;
    /// <summary>Gets whether readers must understand this member.</summary>
    public bool IsMustUnderstand { get; private set; } = isMustUnderstand;
    /// <summary>Gets the source text used to hash an automatically assigned member identifier.</summary>
    public string? MemberIdHashSource { get; } = memberIdHashSource;
    /// <summary>Gets whether the member identifier was assigned using the hash policy.</summary>
    public bool UsesAutoIdHash { get; } = usesAutoIdHash;

    /// <summary>Applies value metadata resolved after type binding.</summary>
    internal void SetValueMetadata(IdlMemberValueMetadata? valueMetadata) => ValueMetadata = valueMetadata;

    /// <summary>Applies member flags whose boolean annotation expressions bind after constants.</summary>
    internal void SetAnnotationFlags(bool isKey, bool isOptional, bool isExternal, bool isMustUnderstand)
    {
        IsKey = isKey;
        IsOptional = isOptional;
        IsExternal = isExternal;
        IsMustUnderstand = isMustUnderstand;
    }

    /// <summary>Applies an explicit member identifier evaluated after constant binding.</summary>
    internal void SetMemberId(int memberId) => MemberId = memberId;

    /// <summary>Applies the effective member identifier resolved for its aggregate.</summary>
    internal void SetEffectiveMemberId(int memberId) => EffectiveMemberId = memberId;
}

/// <summary>Represents an IDL member independently of any target language.</summary>
public sealed class IdlMember(string name, IdlType type, IdlMemberMetadata? metadata = null, IdlInput? sourceInput = null, int sourceOffset = 0)
{
    /// <summary>Gets the member's declared IDL name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the bound type of the member.</summary>
    public IdlType Type { get; private set; } = type;
    /// <summary>Gets annotations and effective identity/value metadata for the member.</summary>
    public IdlMemberMetadata Metadata { get; } = metadata ?? new();
    /// <summary>Gets the input containing this member, if it has a direct source location.</summary>
    public IdlInput? SourceInput { get; } = sourceInput;
    /// <summary>Gets the zero-based source offset of this member declaration.</summary>
    public int SourceOffset { get; } = sourceOffset;

    /// <summary>Gets the explicit annotation applications retained at their original source locations.</summary>
    internal IReadOnlyList<IdlAnnotationApplicationSyntax> AnnotationApplications { get; private set; } = [];

    /// <summary>Gets the parsed annotations attached to this member in source order.</summary>
    public IReadOnlyList<IdlAnnotation> Annotations { get; private set; } = [];

    /// <summary>Retains the annotations applied to this member before their arguments are bound.</summary>
    internal void SetAnnotationApplications(IReadOnlyList<IdlAnnotationApplicationSyntax> applications)
    {
        AnnotationApplications = Array.AsReadOnly(applications.ToArray());
        Annotations = Array.AsReadOnly(applications.Select(IdlAnnotation.FromSyntax).ToArray());
    }

    /// <summary>Replaces this member's deferred type with its bound type.</summary>
    internal void Bind(IdlType type) => Type = type;

    /// <summary>Applies value metadata resolved after type binding.</summary>
    internal void SetValueMetadata(IdlMemberValueMetadata? valueMetadata) => Metadata.SetValueMetadata(valueMetadata);

    /// <summary>Applies member flags whose boolean annotation expressions bind after constants.</summary>
    internal void SetAnnotationFlags(bool isKey, bool isOptional, bool isExternal, bool isMustUnderstand) =>
        Metadata.SetAnnotationFlags(isKey, isOptional, isExternal, isMustUnderstand);

}
