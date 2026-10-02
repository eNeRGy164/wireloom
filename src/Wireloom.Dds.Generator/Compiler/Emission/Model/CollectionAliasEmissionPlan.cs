namespace Wireloom.Compiler.Emission.Model;

/// <summary>Resolved facts shared by every emitter for one IDL typedef.</summary>
internal sealed class CollectionAliasEmissionPlan(
    EmissionTypePlan elementPlan,
    string elementIdlType,
    bool isSequence,
    bool isArray,
    bool nativeValueRequiresCast)
{
    public EmissionTypePlan ElementPlan { get; } = elementPlan;

    public string ElementType => ElementPlan.CSharpType;

    public string ElementIdlType { get; } = elementIdlType;

    public bool IsSequence { get; } = isSequence;

    public bool IsArray { get; } = isArray;

    public bool IsCollection => IsSequence || IsArray;

    public bool IsString => ElementPlan.Shape.IsString;

    public bool IsPrimitive => ElementPlan.Shape.IsPrimitive;

    public bool IsEnum => ElementPlan.Shape.IsEnum;

    public bool IsAggregate => ElementPlan.Shape.IsAggregate;

    public bool IsUnion => ElementPlan.Shape.IsUnion;

    public bool NativeValueRequiresCast { get; } = nativeValueRequiresCast;

    public bool CollectionElementIsAggregate => ElementPlan.Shape is { IsAggregate: true, IsString: false };

    public bool IsWideString => ElementPlan.IsWideString;

    public int StringBound => ElementPlan.Bound ?? 255;
}
