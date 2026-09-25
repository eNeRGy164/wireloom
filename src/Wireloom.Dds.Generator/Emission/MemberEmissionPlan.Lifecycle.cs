namespace Wireloom;

using System;
using static IdlCompiler;

internal sealed partial class MemberEmissionPlan
{
    public string UnionDefaultInitializationStatement(string? namespaceOverride)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsSequence)
        {
            var element = TypeReference(ElementCSharpType!, namespaceName);
            if (HasAggregateElement)
            {
                return $"{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
            }

            return $"{EscapedName}.Initialize<{element}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
        }

        if (IsAggregate)
        {
            return $"{EscapedName}.Initialize(allocatePointers, allocateMemory);";
        }

        if (IsString)
        {
            return $"{EscapedName}.Initialize(size: {Bound}, allocateMemory: allocateMemory);";
        }

        if (IsOptionalScalar)
        {
            return $"{EscapedName} = default;";
        }

        return $"{EscapedName} = {NativeDefaultValue(namespaceName)};";
    }

    private string NativeDefaultValue(string? namespaceName) => ValueType switch
    {
        PrimitiveEmissionType primitive => primitive.IdlName switch
        {
            "float" => "0F",
            "double" => "0D",
            "long double" => "(LongDouble)0",
            _ => "0"
        },
        EnumEmissionType enumType => $"({TypeReference(CSharpType, namespaceName)}){enumType.DefaultValue}",
        _ => throw new InvalidOperationException("Expected a scalar native value.")
    };

    public string NativeStorageType => NativeStorageTypeFor(currentNamespace);

    public string? BuildInitializeStatement(string? namespaceOverride = null)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsOptional)
        {
            if (!IsSequence)
            {
                return null;
            }

            if (HasAggregateElement)
            {
                return $"{EscapedName}.Initialize<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>();";
            }

            return $"{EscapedName}.Initialize();";
        }

        if (IsSequence)
        {
            return BuildSequenceInitializeStatement(namespaceName);
        }

        if (IsArray)
        {
            return BuildArrayInitializeStatement(namespaceName);
        }

        if (IsAggregate)
        {
            return $"{EscapedName}.Initialize(allocatePointers, allocateMemory);";
        }

        if (IsString)
        {
            return $"{EscapedName}.Initialize(size: {Bound}, allocateMemory: allocateMemory);";
        }

        return $"{EscapedName} = {NativeDefaultValue(namespaceName)};";
    }

    private string BuildSequenceInitializeStatement(string? namespaceName)
    {
        var element = TypeReference(ElementCSharpType!, namespaceName);

        if (HasAggregateElement)
        {
            return $"{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
        }

        return $"{EscapedName}.Initialize<{element}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
    }

    private string BuildArrayInitializeStatement(string? namespaceName)
    {
        var element = TypeReference(ElementCSharpType!, namespaceName);

        if (HasAggregateElement)
        {
            return $"{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocatePointers: allocatePointers, allocateMemory: allocateMemory);";
        }

        return $"{EscapedName}.Initialize<{element}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocateMemory: allocateMemory);";
    }

    public string? BuildDestroyStatement(string? namespaceOverride = null)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsOptionalScalar)
        {
            return $"{EscapedName}.Destroy(optionalsOnly);";
        }

        if (IsString)
        {
            return $"{EscapedName}.Destroy();";
        }

        if (IsSequence || IsArray)
        {
            return BuildCollectionDestroyStatement(namespaceName);
        }

        return IsAggregate ? $"{EscapedName}.Destroy(optionalsOnly);" : null;
    }

    private string BuildCollectionDestroyStatement(string? namespaceName)
    {
        if (!HasAggregateElement)
        {
            return $"{EscapedName}.Destroy(optionalsOnly);";
        }

        if (IsSequence)
        {
            return $"{EscapedName}.Destroy<{TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(optionalsOnly);";
        }

        return $"{EscapedName}.Destroy<{TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, optionalsOnly: optionalsOnly);";
    }
}
