namespace Wireloom.Compiler.Emission.Model;

/// <summary>Identifies the resolved target shape of an emitted member.</summary>
internal enum EmissionShapeKind
{
    Primitive,
    String,
    Enum,
    Struct,
    Union,
    Alias,
    Sequence,
    Array
}

/// <summary>Resolved target-shape facts shared by emission planning and emitters.</summary>
internal sealed class EmissionShape(EmissionShapeKind kind, EmissionShape? element = null)
{
    public EmissionShapeKind Kind { get; } = kind;

    public EmissionShape? Element { get; } = element;

    public bool IsPrimitive => Kind == EmissionShapeKind.Primitive;

    public bool IsString => Kind == EmissionShapeKind.String || IsAliasOf(shape => shape.IsString);

    public bool IsEnum => Kind == EmissionShapeKind.Enum || IsAliasOf(shape => shape.IsEnum);

    public bool IsAggregate => Kind is EmissionShapeKind.Struct or EmissionShapeKind.Union
        || IsAliasOf(shape => shape.IsAggregate || shape.IsCollection);

    public bool IsUnion => Kind == EmissionShapeKind.Union || IsAliasOf(shape => shape.IsUnion);

    public bool IsSequence => Kind == EmissionShapeKind.Sequence;

    public bool IsArray => Kind == EmissionShapeKind.Array;

    private bool IsCollection => IsSequence || IsArray;

    public bool HasAggregateElement => IsCollection
        ? Element?.IsAggregate == true
        : IsAliasOf(shape => shape.HasAggregateElement);

    private bool IsAliasOf(Func<EmissionShape, bool> predicate) =>
        Kind == EmissionShapeKind.Alias && Element is not null && predicate(Element);
}
