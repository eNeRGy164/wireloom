using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Target-specific type projection used by source emitters after semantic resolution.</summary>
internal abstract class EmissionTypePlan(string cSharpType)
{
    public string CSharpType { get; } = cSharpType;
    public abstract EmissionShape Shape { get; }
    public bool IsEnum => Shape.IsEnum;
    public virtual int? Bound => null;
    public virtual string? SupportType => null;
    public virtual EmissionTypePlan? Element => null;
    public virtual IReadOnlyList<int> Dimensions => [];
    public virtual bool IsWideString => false;
}

/// <summary>Preserves the target-independent type wrapped by an optional field.</summary>
internal sealed class OptionalEmissionType(EmissionTypePlan target, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public EmissionTypePlan Target { get; } = target;
    public override EmissionShape Shape => Target.Shape;
    public override int? Bound => Target.Bound;
    public override string? SupportType => Target.SupportType;
    public override EmissionTypePlan? Element => Target.Element;
    public override IReadOnlyList<int> Dimensions => Target.Dimensions;
    public override bool IsWideString => Target.IsWideString;
}

/// <summary>Represents an IDL primitive projected to a C# type.</summary>
internal sealed class PrimitiveEmissionType(string idlName, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public string IdlName { get; } = idlName;
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Primitive);
}

/// <summary>Represents a bounded IDL string projected to C#.</summary>
internal sealed class StringEmissionType(bool isWide, int bound)
    : EmissionTypePlan("string")
{
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.String);
    public bool IsWide { get; } = isWide;
    public override bool IsWideString => IsWide;
    public override int? Bound { get; } = bound;
}

/// <summary>Represents an IDL enum projected to a C# type.</summary>
internal sealed class EnumEmissionType(string cSharpType, int defaultValue)
    : EmissionTypePlan(cSharpType)
{
    public int DefaultValue { get; } = defaultValue;
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Enum);
}

/// <summary>Represents an IDL struct projected to a C# type.</summary>
internal sealed class StructEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Struct);
}

/// <summary>Represents an IDL union projected to a C# type.</summary>
internal sealed class UnionEmissionType(string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Union);
}

/// <summary>Represents an IDL alias and its projected target type.</summary>
internal sealed class AliasEmissionType(string qualifiedName, EmissionTypePlan target, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    private string QualifiedName { get; } = qualifiedName;
    public EmissionTypePlan Target { get; } = target;
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Alias, target.Shape);
    public override int? Bound => Target.Bound;
    public override string SupportType => IdlNaming.EscapeQualifiedIdentifier(QualifiedName);
    public override EmissionTypePlan? Element => Target.Element;
    public override IReadOnlyList<int> Dimensions => Target.Dimensions;
    public override bool IsWideString => Target.IsWideString;
}

/// <summary>Represents a bounded IDL sequence projected to a C# type.</summary>
internal sealed class SequenceEmissionType(EmissionTypePlan element, int bound, string cSharpType, IReadOnlyList<int>? dimensions = null)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Sequence, element.Shape);
    public override int? Bound { get; } = bound;
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
}

/// <summary>Represents a fixed-size IDL array projected to a C# type.</summary>
internal sealed class ArrayEmissionType(EmissionTypePlan element, IReadOnlyList<int> dimensions, string cSharpType)
    : EmissionTypePlan(cSharpType)
{
    public override EmissionShape Shape { get; } = new(EmissionShapeKind.Array, element.Shape);
    public override EmissionTypePlan Element { get; } = element;
    public override IReadOnlyList<int> Dimensions { get; } = dimensions;
}
