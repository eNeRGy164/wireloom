using System.Globalization;
using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Builds managed lifecycle operations for a member emission plan.</summary>
internal sealed partial class MemberEmissionPlan
{
    /// <summary>Builds initialization code for a union member's default value.</summary>
    public string UnionDefaultInitializationStatement(string? namespaceOverride, string nativeFieldPrefix = "")
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsSequence)
        {
            var element = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);
            if (HasAggregateElement)
            {
                return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
            }

            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
        }

        if (IsAggregate)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(allocatePointers, allocateMemory);";
        }

        if (IsString)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(size: {Bound}, allocateMemory: allocateMemory);";
        }

        if (IsOptionalScalar)
        {
            return $"{nativeFieldPrefix}{EscapedName} = default;";
        }

        return $"{nativeFieldPrefix}{EscapedName} = {NativeDefaultValue(namespaceName)};";
    }

    private string NativeDefaultValue(string? namespaceName)
    {
        if (HasExplicitDefault)
        {
            var value = ValueType is EnumEmissionType
                ? DefaultValue!.Value.ToString(CultureInfo.InvariantCulture)
                : FormatCSharpValue(CSharpType.TrimEnd('?'), DefaultValue!.Value);

            if (ValueType is EnumEmissionType)
            {
                return $"({IdlNaming.TypeReference(CSharpType, namespaceName)})({value})";
            }

            return value;
        }

        if (ValueType is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeDefaultLiteral;
        }

        return ValueType switch
        {
            EnumEmissionType enumType => $"({IdlNaming.TypeReference(CSharpType, namespaceName)})({enumType.DefaultValue})",
            _ => throw new InvalidOperationException("Expected a scalar native value.")
        };
    }

    /// <summary>Builds native initialization code for this member.</summary>
    public string? BuildInitializeStatement(string? namespaceOverride = null, string nativeFieldPrefix = "")
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
                return $"{nativeFieldPrefix}{EscapedName}.Initialize<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>();";
            }

            return $"{nativeFieldPrefix}{EscapedName}.Initialize();";
        }

        if (IsSequence)
        {
            return BuildSequenceInitializeStatement(namespaceName, nativeFieldPrefix);
        }

        if (IsArray)
        {
            return BuildArrayInitializeStatement(namespaceName, nativeFieldPrefix);
        }

        if (IsAggregate)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(allocatePointers, allocateMemory);";
        }

        if (IsString)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(size: {Bound}, allocateMemory: allocateMemory);";
        }

        return $"{nativeFieldPrefix}{EscapedName} = {NativeDefaultValue(namespaceName)};";
    }

    private string BuildSequenceInitializeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        var element = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);

        if (IsStringSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(max: {Bound}, absoluteMax: {Bound}, maxStrLen: {ElementType!.Bound}, allocateMemory: allocateMemory);";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory: allocateMemory);";
    }

    private string BuildArrayInitializeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        var element = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocatePointers: allocatePointers, allocateMemory: allocateMemory);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocateMemory: allocateMemory);";
    }

    /// <summary>Builds native destruction code for this member.</summary>
    public string? BuildDestroyStatement(string? namespaceOverride = null, string nativeFieldPrefix = "")
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsOptionalScalar)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy(optionalsOnly);";
        }

        if (IsString)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy();";
        }

        if (IsSequence || IsArray)
        {
            return BuildCollectionDestroyStatement(namespaceName, nativeFieldPrefix);
        }

        if (!IsAggregate)
        {
            return null;
        }

        if (IsOptional)
        {
            var type = IdlNaming.TypeReference(CSharpType.TrimEnd('?'), currentNamespace);
            return $"{nativeFieldPrefix}{EscapedName}.Destroy<{type}, {GetReferencedUnmanagedType(currentNamespace)}>(optionalsOnly);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Destroy(optionalsOnly);";
    }

    private string BuildCollectionDestroyStatement(string? namespaceName, string nativeFieldPrefix)
    {
        if (IsStringSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy();";
        }

        if (!HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy(optionalsOnly);";
        }

        if (IsSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy<{IdlNaming.TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(optionalsOnly);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Destroy<{IdlNaming.TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, optionalsOnly: optionalsOnly);";
    }
}
