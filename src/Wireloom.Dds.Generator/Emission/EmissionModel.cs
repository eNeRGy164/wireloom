namespace Wireloom;

using JetBrains.Annotations;
using static IdlCompiler;

/// <summary>
/// Target-specific type projection used by source emitters after semantic
/// resolution has completed.
/// </summary>
internal abstract class EmissionTypePlan(string cSharpType)
{
    public string CSharpType { get; } = cSharpType;
    public virtual bool IsAggregate => false;
    public virtual bool IsUnion => false;
    public virtual bool IsEnum => false;
    public virtual int? Bound => null;
    public virtual string? SupportType => null;
    public virtual EmissionTypePlan? Element => null;
    public virtual IReadOnlyList<int> Dimensions => [];
}

[PublicAPI]
internal sealed class OptionalEmissionType(EmissionTypePlan target, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public EmissionTypePlan Target { get; } = target;
    public override bool IsAggregate => Target.IsAggregate;
    public override bool IsUnion => Target.IsUnion;
    public override bool IsEnum => Target.IsEnum;
    public override int? Bound => Target.Bound;
    public override string? SupportType => Target.SupportType;
    public override EmissionTypePlan? Element => Target.Element;
    public override IReadOnlyList<int> Dimensions => Target.Dimensions;
}

[PublicAPI]
internal sealed class PrimitiveEmissionType(string idlName, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public string IdlName { get; } = idlName;
}

[PublicAPI]
internal sealed class StringEmissionType(bool isWide, int bound)
    : EmissionTypePlan("string")
{
    public bool IsWide { get; } = isWide;
    public override int? Bound { get; } = bound;
}

[PublicAPI]
internal sealed class EnumEmissionType(string cSharpType, int defaultValue)
    : EmissionTypePlan(cSharpType)
{
    public int DefaultValue { get; } = defaultValue;
    public override bool IsEnum => true;
}

[PublicAPI]
internal sealed class StructEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override bool IsAggregate => true;
}

[PublicAPI]
internal sealed class UnionEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override bool IsAggregate => true;
    public override bool IsUnion => true;
}

[PublicAPI]
internal sealed class AliasEmissionType(string qualifiedName, EmissionTypePlan target, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public string QualifiedName { get; } = qualifiedName;
    public EmissionTypePlan Target { get; } = target;
    public override bool IsAggregate => Target is SequenceEmissionType or ArrayEmissionType || Target.IsAggregate;
    public override bool IsUnion => Target.IsUnion;
    public override bool IsEnum => Target.IsEnum;
    public override int? Bound => Target.Bound;
    public override string SupportType => EscapeQualifiedIdentifier(QualifiedName);
    public override EmissionTypePlan? Element => Target.Element;
    public override IReadOnlyList<int> Dimensions => Target.Dimensions;
}

[PublicAPI]
internal sealed class SequenceEmissionType(EmissionTypePlan element, int bound, string cSharpType, IReadOnlyList<int>? dimensions = null)
    : EmissionTypePlan(cSharpType)
{
    public override int? Bound { get; } = bound;
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
}

[PublicAPI]
internal sealed class ArrayEmissionType(EmissionTypePlan element, IReadOnlyList<int> dimensions, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions;
}

/// <summary>
/// Target-specific field projection used only by source emitters. Its type
/// shape is explicit; the forwarding properties are formatting conveniences.
/// </summary>
[PublicAPI]
internal sealed class IdlEmissionField(string name, EmissionTypePlan type, bool isKey, int? memberId, bool isOptional)
{
    public string Name { get; } = name;
    public EmissionTypePlan Type { get; } = type;
    public bool IsKey { get; } = isKey;
    public int? MemberId { get; } = memberId;
    public bool IsOptional { get; } = isOptional;
    public string CSharpType => Type.CSharpType;
    public int? Bound => Type.Bound;
    public string? SupportType => Type.SupportType;
    public EmissionTypePlan? ElementType => Type.Element;
    public string? ElementCSharpType => Type.Element?.CSharpType;
    public string? ElementSupportType => Type.Element?.SupportType;
    public IReadOnlyList<int> Dimensions => Type.Dimensions;
}

/// <summary>Resolved branch decisions shared by all union emitters.</summary>
[PublicAPI]
internal sealed class UnionBranchEmissionPlan(IdlEmissionField field, MemberEmissionPlan plan, IReadOnlyList<string> labels, IReadOnlyList<int> labelValues, bool isDefault)
{
    public IdlEmissionField Field { get; } = field;
    public MemberEmissionPlan Plan { get; } = plan;
    public IReadOnlyList<string> Labels { get; } = labels;
    public IReadOnlyList<int> LabelValues { get; } = labelValues;
    public bool IsDefault { get; } = isDefault;
}

[PublicAPI]
internal sealed class IdlEmissionUnion(string name, string? @namespace, string discriminatorCSharpType, bool discriminatorIsEnum, IReadOnlyList<UnionBranchEmissionPlan> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorCSharpType { get; } = discriminatorCSharpType;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    public IReadOnlyList<UnionBranchEmissionPlan> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
}
