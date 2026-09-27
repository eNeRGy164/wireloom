using static Wireloom.IdlCompiler;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Target-specific type projection used by source emitters after semantic resolution.</summary>
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

/// <summary>Preserves the target-independent type wrapped by an optional field.</summary>
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

/// <summary>Represents an IDL primitive projected to a C# type.</summary>
internal sealed class PrimitiveEmissionType(string idlName, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public string IdlName { get; } = idlName;
}

/// <summary>Represents a bounded IDL string projected to C#.</summary>
internal sealed class StringEmissionType(bool isWide, int bound)
    : EmissionTypePlan("string")
{
    public bool IsWide { get; } = isWide;
    public override int? Bound { get; } = bound;
}

/// <summary>Represents an IDL enum projected to a C# type.</summary>
internal sealed class EnumEmissionType(string cSharpType, int defaultValue)
    : EmissionTypePlan(cSharpType)
{
    public int DefaultValue { get; } = defaultValue;
    public override bool IsEnum => true;
}

/// <summary>Represents an IDL struct projected to a C# type.</summary>
internal sealed class StructEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override bool IsAggregate => true;
}

/// <summary>Represents an IDL union projected to a C# type.</summary>
internal sealed class UnionEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override bool IsAggregate => true;
    public override bool IsUnion => true;
}

/// <summary>Represents an IDL alias and its projected target type.</summary>
internal sealed class AliasEmissionType(string qualifiedName, EmissionTypePlan target, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    private string QualifiedName { get; } = qualifiedName;
    public EmissionTypePlan Target { get; } = target;
    public override bool IsAggregate => Target is SequenceEmissionType or ArrayEmissionType || Target.IsAggregate;
    public override bool IsUnion => Target.IsUnion;
    public override bool IsEnum => Target.IsEnum;
    public override int? Bound => Target.Bound;
    public override string SupportType => EscapeQualifiedIdentifier(QualifiedName);
    public override EmissionTypePlan? Element => Target.Element;
    public override IReadOnlyList<int> Dimensions => Target.Dimensions;
}

/// <summary>Represents a bounded IDL sequence projected to a C# type.</summary>
internal sealed class SequenceEmissionType(EmissionTypePlan element, int bound, string cSharpType, IReadOnlyList<int>? dimensions = null)
    : EmissionTypePlan(cSharpType)
{
    public override int? Bound { get; } = bound;
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
}

/// <summary>Represents a fixed-size IDL array projected to a C# type.</summary>
internal sealed class ArrayEmissionType(EmissionTypePlan element, IReadOnlyList<int> dimensions, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions;
}
