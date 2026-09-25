namespace Wireloom;

using static IdlCompiler;

internal sealed partial class MemberEmissionPlan
{
    public string BuildDynamicTypeExpression(string implementationNamespace, string? recursiveTypeName = null, bool isRecursive = false) => shape switch
    {
        FieldEmissionShape.Sequence or FieldEmissionShape.Array => BuildCollectionDynamicType(implementationNamespace, recursiveTypeName, isRecursive),
        FieldEmissionShape.Struct or FieldEmissionShape.Enum or FieldEmissionShape.Alias => ReferencedSupportType(implementationNamespace) + ".GetDynamicTypeInternal(isPublic)",
        FieldEmissionShape.String => BuildStringDynamicType(),
        _ => $"dtf.GetPrimitiveType<{PrimitiveDynamicType()}>()",
    };

    private string BuildCollectionDynamicType(string implementationNamespace, string? recursiveTypeName, bool isRecursive)
    {
        if (isRecursive && IsRecursive(recursiveTypeName!))
        {
            var supportType = $"{TypeReference(recursiveTypeName!, implementationNamespace)}Support.GetOrCreateInstanceImpl()";

            return $"tsf.CreateSequenceWithAccessInfo(dtf, {supportType}.GetDynamicTypeInternal(isPublic), {Bound})";
        }

        return BuildCollectionDynamicType(implementationNamespace);
    }

    private string BuildStringDynamicType() => ValueType switch
    {
        StringEmissionType stringType when stringType.IsWide => $"dtf.CreateWideString({stringType.Bound})",
        StringEmissionType stringType => $"dtf.CreateString({stringType.Bound})",
        _ => throw new InvalidOperationException("Expected a string emission type.")
    };

    private string PrimitiveDynamicType() => ValueType switch
    {
        EnumEmissionType => NullableValueType(),
        PrimitiveEmissionType primitive when primitive.IdlName == "octet" => "Octet",
        PrimitiveEmissionType primitive when primitive.IdlName == "wchar" => "DynamicTypeFactory.WideCharType",
        _ => NullableValueType()
    };

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
        if (HasAggregateElement || ElementType?.IsEnum == true || ElementSupportType is not null)
        {
            return $"{TypeReference(ElementSupportType ?? ElementCSharpType!, implementationNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
        }

        return $"dtf.GetPrimitiveType<{TypeReference(ElementCSharpType!, implementationNamespace)}>()";
    }

    private string ReferencedSupportType(string implementationNamespace) =>
        $"{TypeReference(SupportType ?? CSharpType, implementationNamespace)}Support.Instance";
}
