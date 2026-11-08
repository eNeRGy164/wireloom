using Wireloom.Compiler.Emission.Model;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Identifies the managed initialization required by a member shape.</summary>
internal enum ManagedInitializationKind
{
    None,
    Sequence,
    Array,
    Aggregate
}

/// <summary>Identifies the native cleanup required by a member shape.</summary>
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
    /// <summary>Determines the managed initialization policy for a member.</summary>
    internal static ManagedInitializationKind GetManagedInitialization(
        EmissionShapeKind shape,
        bool isOptional,
        bool isSequence,
        bool isArray,
        bool isAggregate)
    {
        if (isOptional && (isSequence || isArray || isAggregate))
        {
            return ManagedInitializationKind.None;
        }

        if (shape == EmissionShapeKind.Sequence)
        {
            return ManagedInitializationKind.Sequence;
        }

        if (shape == EmissionShapeKind.Array)
        {
            return ManagedInitializationKind.Array;
        }

        return isAggregate ? ManagedInitializationKind.Aggregate : ManagedInitializationKind.None;
    }

    /// <summary>Determines the native destruction policy for a member.</summary>
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
