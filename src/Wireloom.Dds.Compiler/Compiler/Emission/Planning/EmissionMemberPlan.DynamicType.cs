using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Builds dynamic-type expressions for a member emission plan.</summary>
internal sealed partial class MemberEmissionPlan
{
    /// <summary>Builds the dynamic-type expression for this member.</summary>
    public string BuildDynamicTypeExpression(
        string implementationNamespace,
        string? recursiveTypeName = null,
        bool isRecursive = false,
        IReadOnlyCollection<string>? recursiveComponent = null)
    {
        var dynamicType = IsOptional && IsAggregate ? Field.DeclaredType : Type;

        if (recursiveComponent is { Count: > 0 }
            && Type is SequenceEmissionType sequence
            && TryBuildRecursiveSequenceExpression(sequence, implementationNamespace, recursiveComponent, out var sequenceExpression))
        {
            return sequenceExpression;
        }

        return dynamicType.Shape.Kind switch
        {
            EmissionShapeKind.Sequence or EmissionShapeKind.Array => BuildCollectionDynamicType(implementationNamespace, recursiveTypeName, isRecursive),
            EmissionShapeKind.Struct or EmissionShapeKind.Union or EmissionShapeKind.Enum or EmissionShapeKind.Alias => ReferencedSupportType(implementationNamespace, dynamicType) + ".GetDynamicTypeInternal(isPublic)",
            EmissionShapeKind.String => BuildStringDynamicType(),
            _ => $"dtf.GetPrimitiveType<{PrimitiveDynamicType()}>()",
        };
    }

    private static bool TryBuildRecursiveSequenceExpression(SequenceEmissionType sequence, string implementationNamespace, IReadOnlyCollection<string> recursiveComponent, out string expression)
    {
        var element = EmissionTypeProjector.UnwrapValueEmissionType(sequence.Element);
        if (element is not StructEmissionType structure
            || structure.SupportType is null
            || !Contains(recursiveComponent, structure.SupportType))
        {
            expression = string.Empty;
            return false;
        }

        var recursiveSupportType = IdlNaming.GeneratedSupportTypeReference(structure.SupportType, implementationNamespace);

        var bound = sequence.IsUnbounded ? "SequenceType.Unbounded" : sequence.Bound.ToString();

        expression = $"tsf.CreateSequenceWithAccessInfo(dtf, {recursiveSupportType}Support.GetOrCreateInstanceImpl().GetDynamicTypeInternal(isPublic), {bound})";
        return true;
    }

    private static bool Contains(IReadOnlyCollection<string> values, string expected)
    {
        foreach (var value in values)
        {
            if (string.Equals(value, expected, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private string BuildCollectionDynamicType(string implementationNamespace, string? recursiveTypeName, bool isRecursive)
    {
        if (isRecursive && IsRecursive(recursiveTypeName!))
        {
            if (IsSequence)
            {
                var recursiveSupportType = $"{IdlNaming.GeneratedSupportTypeReference(recursiveTypeName!, implementationNamespace)}Support.GetOrCreateInstanceImpl()";
                return $"tsf.CreateSequenceWithAccessInfo(dtf, {recursiveSupportType}.GetDynamicTypeInternal(isPublic), {SequenceBoundArgument})";
            }

            var supportType = $"{IdlNaming.GeneratedSupportTypeReference(recursiveTypeName!, implementationNamespace)}Support.GetOrCreateInstanceImpl()";
            return $"tsf.CreateArrayWithAccessInfo<{ElementUnmanagedType(implementationNamespace)}>(dtf, {supportType}.GetDynamicTypeInternal(isPublic), [{string.Join(", ", Dimensions)}])";
        }

        return BuildCollectionDynamicType(implementationNamespace);
    }

    private string BuildStringDynamicType() => ValueType switch
    {
        StringEmissionType { IsUnbounded: true, IsWide: true } => "dtf.CreateWideString()",
        StringEmissionType { IsUnbounded: true } => "dtf.CreateString()",
        StringEmissionType { IsWide: true } stringType => $"dtf.CreateWideString({stringType.Bound})",
        StringEmissionType stringType => $"dtf.CreateString({stringType.Bound})",
        _ => throw new InvalidOperationException("Expected a string emission type.")
    };

    private string PrimitiveDynamicType()
    {
        if (ValueType is PrimitiveEmissionType primitive)
        {
            var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
            return primitive.IdlName == "octet" ? Profile.OctetDynamicType : mapping.DynamicType;
        }

        return NullableValueType();
    }

    private string BuildCollectionDynamicType(string implementationNamespace)
    {
        var element = BuildCollectionElementDynamicType(implementationNamespace);

        if (IsSequence)
        {
            return $"tsf.CreateSequenceWithAccessInfo(dtf, {element}, {SequenceBoundArgument})";
        }

        var unmanagedElementType = HasAggregateElement ? ElementUnmanagedType(implementationNamespace) : ElementCSharpType;

        return $"tsf.CreateArrayWithAccessInfo<{unmanagedElementType}>(dtf, {element}, [{string.Join(", ", Dimensions)}])";
    }

    private string BuildCollectionElementDynamicType(string implementationNamespace)
    {
        if (ElementType is AliasEmissionType { IsAnonymousSequence: true } anonymous)
        {
            var sequenceField = new IdlEmissionField("Value", anonymous.Target,
                new EmissionMetadata(isKey: false, memberId: null, isOptional: false, valueMetadata: null,
                    isExternal: false, isMustUnderstand: false, memberIdHashSource: null, usesAutoIdHash: false));
            return new MemberEmissionPlan(sequenceField, currentNamespace, profile: Profile)
                .BuildDynamicTypeExpression(implementationNamespace);
        }

        if (ElementType is StringEmissionType stringType)
        {
            return stringType.IsUnbounded
                ? stringType.IsWide ? "dtf.CreateWideString()" : "dtf.CreateString()"
                : stringType.IsWide ? $"dtf.CreateWideString({stringType.Bound})" : $"dtf.CreateString({stringType.Bound})";
        }

        if (HasAggregateElement || ElementType?.IsEnum == true || ElementSupportType is not null)
        {
            return $"{IdlNaming.GeneratedSupportTypeReference(ElementSupportType ?? ElementCSharpType!, implementationNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
        }

        return $"dtf.GetPrimitiveType<{IdlNaming.TypeReference(ElementCSharpType!, implementationNamespace)}>()";
    }

    private static string ReferencedSupportType(string implementationNamespace, EmissionTypePlan type) =>
        $"{IdlNaming.GeneratedSupportTypeReference(type.SupportType ?? type.CSharpType, implementationNamespace)}Support.Instance";
}
