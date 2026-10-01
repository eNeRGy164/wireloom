using Wireloom.Compiler.Emission.Model;

namespace Wireloom.Compiler.Emission.Planning;

internal enum FieldEmissionShape
{
    Primitive,
    String,
    Enum,
    Struct,
    Alias,
    Sequence,
    Array
}

internal enum ManagedInitializationKind
{
    None,
    Sequence,
    Array,
    Aggregate
}

internal enum NativeDestroyKind
{
    None,
    Nested,
    Collection,
    String,
    OptionalPrimitive
}

/// <summary>Classifies the target shape and lifecycle policies of a member.</summary>
internal static class MemberEmissionPolicies
{
    internal static FieldEmissionShape GetShape(EmissionTypePlan type) =>
        EmissionTypeProjector.UnwrapOptionalEmissionType(type) switch
        {
            PrimitiveEmissionType => FieldEmissionShape.Primitive,
            StringEmissionType => FieldEmissionShape.String,
            EnumEmissionType => FieldEmissionShape.Enum,
            StructEmissionType or UnionEmissionType => FieldEmissionShape.Struct,
            AliasEmissionType => FieldEmissionShape.Alias,
            SequenceEmissionType => FieldEmissionShape.Sequence,
            ArrayEmissionType => FieldEmissionShape.Array,
            _ => throw new InvalidOperationException("Unknown emission type plan.")
        };

    internal static ManagedInitializationKind GetManagedInitialization(
        FieldEmissionShape shape,
        bool isOptional,
        bool isSequence,
        bool isArray,
        bool isAggregate)
    {
        if (isOptional && (isSequence || isArray))
        {
            return ManagedInitializationKind.None;
        }

        if (shape == FieldEmissionShape.Sequence)
        {
            return ManagedInitializationKind.Sequence;
        }

        if (shape == FieldEmissionShape.Array)
        {
            return ManagedInitializationKind.Array;
        }

        return isAggregate ? ManagedInitializationKind.Aggregate : ManagedInitializationKind.None;
    }

    internal static NativeDestroyKind GetDestroyKind(
        bool isAggregate,
        bool isSequence,
        bool isArray,
        bool isString,
        bool isOptionalScalar)
    {
        if (isAggregate && !isSequence && !isArray)
        {
            return NativeDestroyKind.Nested;
        }

        if (isSequence || isArray)
        {
            return NativeDestroyKind.Collection;
        }

        if (isString)
        {
            return NativeDestroyKind.String;
        }

        return isOptionalScalar ? NativeDestroyKind.OptionalPrimitive : NativeDestroyKind.None;
    }
}
