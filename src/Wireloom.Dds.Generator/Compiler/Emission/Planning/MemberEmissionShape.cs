using Wireloom.Compiler.Emission.Model;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Provides immutable shape facts for one resolved emission member.</summary>
internal sealed class MemberEmissionShape(EmissionTypePlan type)
{
    private readonly EmissionShape shape = type.Shape;

    /// <summary>Gets the resolved emission shape kind.</summary>
    public EmissionShapeKind Kind => shape.Kind;

    /// <summary>Gets whether the member is a string.</summary>
    public bool IsString => shape.IsString;

    /// <summary>Gets whether the member is a sequence.</summary>
    public bool IsSequence => shape.IsSequence;

    /// <summary>Gets whether the member is an array.</summary>
    public bool IsArray => shape.IsArray;

    /// <summary>Gets whether the member is an aggregate.</summary>
    public bool IsAggregate => shape.IsAggregate;

    /// <summary>Gets whether the member is a union.</summary>
    public bool IsUnion => shape.IsUnion;

    /// <summary>Gets whether the member has an aggregate element.</summary>
    public bool HasAggregateElement => shape.HasAggregateElement;

    /// <summary>Gets whether the collection element is a string.</summary>
    public bool IsStringSequence => IsSequence && shape.Element?.IsString == true;
}
