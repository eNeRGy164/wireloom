using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Captures immutable resolved facts used by a member emission plan.</summary>
internal sealed class MemberEmissionFacts(IdlEmissionField field, string? currentNamespace)
{
    internal string? CurrentNamespace { get; } = currentNamespace;
    internal EmissionTypePlan Type { get; } = field.Type;
    private MemberEmissionShape Shape { get; } = new(field.Type);
    internal string Name { get; } = field.Name;
    internal string CSharpType { get; } = field.CSharpType;
    internal bool IsKey { get; } = field.IsKey;
    internal int? MemberId { get; } = field.MemberId;
    internal bool IsOptional { get; } = field.IsOptional;
    internal int? Bound { get; } = field.Bound;
    internal string? SupportType { get; } = field.SupportType;
    internal EmissionTypePlan? ElementType { get; } = field.ElementType;
    internal string? ElementCSharpType { get; } = field.ElementCSharpType;
    internal string? ElementSupportType { get; } = field.ElementSupportType;
    internal EmissionTypePlan ValueType => EmissionTypeProjector.UnwrapValueEmissionType(Type);
    internal IReadOnlyList<int> Dimensions { get; } = field.Dimensions;
    internal IdlMemberValueMetadata? ValueMetadata { get; } = field.ValueMetadata;
    internal bool IsExternal { get; } = field.IsExternal;
    internal bool IsMustUnderstand { get; } = field.IsMustUnderstand;
    internal string? MemberIdHashSource { get; } = field.MemberIdHashSource;
    internal bool UsesAutoIdHash { get; } = field.UsesAutoIdHash;

    internal bool IsString => Shape.IsString;
    internal bool IsSequence => Shape.IsSequence;
    internal bool IsArray => Shape.IsArray;
    internal bool IsAggregate => Shape.IsAggregate;
    internal bool IsUnion => Shape.IsUnion;
    internal bool IsStringSequence => Shape.IsStringSequence;
    internal bool HasAggregateElement => Shape.HasAggregateElement;
}
