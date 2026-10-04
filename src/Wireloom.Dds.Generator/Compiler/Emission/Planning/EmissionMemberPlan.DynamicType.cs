using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Builds dynamic-type expressions for a member emission plan.</summary>
internal sealed partial class MemberEmissionPlan
{
    /// <summary>Builds the dynamic-type expression for this member.</summary>
    public string BuildDynamicTypeExpression(string implementationNamespace, string? recursiveTypeName = null, bool isRecursive = false) => shape.Kind switch
    {
        EmissionShapeKind.Sequence or EmissionShapeKind.Array => BuildCollectionDynamicType(implementationNamespace, recursiveTypeName, isRecursive),
        EmissionShapeKind.Struct or EmissionShapeKind.Union or EmissionShapeKind.Enum or EmissionShapeKind.Alias => ReferencedSupportType(implementationNamespace) + ".GetDynamicTypeInternal(isPublic)",
        EmissionShapeKind.String => BuildStringDynamicType(),
        _ => $"dtf.GetPrimitiveType<{PrimitiveDynamicType()}>()",
    };

    private string BuildCollectionDynamicType(string implementationNamespace, string? recursiveTypeName, bool isRecursive)
    {
        if (isRecursive && IsRecursive(recursiveTypeName!))
        {
            var supportType = $"{IdlNaming.GeneratedSupportTypeReference(recursiveTypeName!, implementationNamespace)}Support.GetOrCreateInstanceImpl()";

            return $"tsf.CreateSequenceWithAccessInfo(dtf, {supportType}.GetDynamicTypeInternal(isPublic), {Bound})";
        }

        return BuildCollectionDynamicType(implementationNamespace);
    }

    private string BuildStringDynamicType() => ValueType switch
    {
        StringEmissionType { IsWide: true } stringType => $"dtf.CreateWideString({stringType.Bound})",
        StringEmissionType stringType => $"dtf.CreateString({stringType.Bound})",
        _ => throw new InvalidOperationException("Expected a string emission type.")
    };

    private string PrimitiveDynamicType()
    {
        if (ValueType is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).DynamicType;
        }

        return NullableValueType();
    }

    private string BuildCollectionDynamicType(string implementationNamespace)
    {
        var element = BuildCollectionElementDynamicType(implementationNamespace);

        if (IsSequence)
        {
            return $"tsf.CreateSequenceWithAccessInfo(dtf, {element}, {Bound})";
        }

        var unmanagedElementType = HasAggregateElement ? ElementUnmanagedType(implementationNamespace) : ElementCSharpType;

        return $"tsf.CreateArrayWithAccessInfo<{unmanagedElementType}>(dtf, {element}, new uint[] {{ {string.Join(", ", Dimensions)} }})";
    }

    private string BuildCollectionElementDynamicType(string implementationNamespace)
    {
        if (ElementType is StringEmissionType stringType)
        {
            return stringType.IsWide
                ? $"dtf.CreateWideString({stringType.Bound})"
                : $"dtf.CreateString({stringType.Bound})";
        }

        if (HasAggregateElement || ElementType?.IsEnum == true || ElementSupportType is not null)
        {
            return $"{IdlNaming.GeneratedSupportTypeReference(ElementSupportType ?? ElementCSharpType!, implementationNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
        }

        return $"dtf.GetPrimitiveType<{IdlNaming.TypeReference(ElementCSharpType!, implementationNamespace)}>()";
    }

    private string ReferencedSupportType(string implementationNamespace) =>
        $"{IdlNaming.GeneratedSupportTypeReference(SupportType ?? CSharpType, implementationNamespace)}Support.Instance";
}
