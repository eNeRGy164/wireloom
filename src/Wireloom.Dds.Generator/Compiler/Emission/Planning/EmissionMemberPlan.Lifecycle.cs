using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Writers;
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
            return BuildSequenceInitializeStatement(namespaceName, nativeFieldPrefix);
        }

        if (IsArray)
        {
            return BuildInitializeStatement(namespaceName, nativeFieldPrefix)!;
        }

        if (IsAggregate)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(allocatePointers, allocateMemory);";
        }

        if (IsString)
        {
            var initialize = $"{nativeFieldPrefix}{EscapedName}.Initialize(size: {Bound}, allocateMemory);";
            return HasExplicitDefault
                ? $"{initialize}\n{nativeFieldPrefix}{EscapedName}.ToNative({ManagedDefaultValue}, {Bound});"
                : initialize;
        }

        if (IsOptionalScalar)
        {
            return $"{nativeFieldPrefix}{EscapedName} = default;";
        }

        return $"{nativeFieldPrefix}{EscapedName} = {NativeDefaultValue()};";
    }

    private string NativeDefaultValue() =>
        new ScalarValueEmissionPlan(ValueType, facts.ValueMetadata).NativeDefaultLiteral;

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
            var initialize = $"{nativeFieldPrefix}{EscapedName}.Initialize(size: {Bound}, allocateMemory);";
            return HasExplicitDefault
                ? $"{initialize}\n{nativeFieldPrefix}{EscapedName}.ToNative({ManagedDefaultValue}, {Bound});"
                : initialize;
        }

        return $"{nativeFieldPrefix}{EscapedName} = {NativeDefaultValue()};";
    }

    /// <summary>Emits native initialization, including enum array defaults on older RTI profiles.</summary>
    public void EmitNativeInitialization(GeneratedSourceWriter writer, string? namespaceOverride = null, string nativeFieldPrefix = "")
    {
        var statement = BuildInitializeStatement(namespaceOverride, nativeFieldPrefix);
        if (statement is null)
        {
            return;
        }

        writer.WriteLines(statement);
        EmitNativeEnumArrayDefaultInitialization(writer, namespaceOverride, nativeFieldPrefix);
    }

    /// <summary>Emits a native union branch's initialization using the same enum-array fallback as aggregate members.</summary>
    public void EmitUnionNativeInitialization(GeneratedSourceWriter writer, string? namespaceOverride, string nativeFieldPrefix = "")
    {
        writer.WriteLines(UnionDefaultInitializationStatement(namespaceOverride, nativeFieldPrefix));
        EmitNativeEnumArrayDefaultInitialization(writer, namespaceOverride, nativeFieldPrefix);
    }

    private void EmitNativeEnumArrayDefaultInitialization(GeneratedSourceWriter writer, string? namespaceOverride, string nativeFieldPrefix)
    {
        var enumType = ArraySourceEmitter.EnumElementType(ElementType);
        if (!IsArray || IsOptional || enumType is not { DefaultValue: not 0 } || Profile.SupportsNativeEnumArrayDefaultValueInitialization)
        {
            return;
        }

        var namespaceName = namespaceOverride ?? currentNamespace;
        var nativeArray = $"{nativeFieldPrefix}{EscapedName}";
        var localName = $"__wireloom_{IdlNaming.EscapeIdentifier(Name).TrimStart('@')}_defaultValues";

        writer.OpenBlock("if (allocateMemory)");
        ArraySourceEmitter.EmitNativeEnumDefaultInitialization(
            writer,
            nativeArray,
            ElementCSharpType!,
            localName,
            enumType,
            Dimensions,
            namespaceName);
        writer.CloseBlock();
    }

    private string BuildSequenceInitializeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        var element = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);

        if (IsRecursiveUnboundedSequence && HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: 0, absoluteMax: SequenceType.Unbounded, allocateMemory);";
        }

        if (IsStringSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize(max: {Bound}, absoluteMax: {Bound}, maxStrLen: {ElementType!.Bound}, allocateMemory);";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}>(max: {Bound}, absoluteMax: {Bound}, allocateMemory);";
    }

    private string BuildArrayInitializeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        var element = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);

        var enumType = ArraySourceEmitter.EnumElementType(ElementType);
        if (enumType is { DefaultValue: not 0 } && Profile.SupportsNativeEnumArrayDefaultValueInitialization)
        {
            return $"{nativeFieldPrefix}{EscapedName}.InitializeEnum(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocateMemory: allocateMemory, defaultValue: {enumType.DefaultValue});";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocatePointers: allocatePointers, allocateMemory: allocateMemory);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Initialize<{element}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, allocateMemory);";
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
            var type = IdlNaming.TypeReference(CSharpType.TrimEnd('?'), namespaceName);
            return $"{nativeFieldPrefix}{EscapedName}.Destroy<{type}, {GetReferencedUnmanagedType(namespaceName)}>(optionalsOnly);";
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

        if (IsOptional)
        {
            return $"{nativeFieldPrefix}{EscapedName}.Destroy<{IdlNaming.TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, optionalsOnly);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.Destroy<{IdlNaming.TypeReference(ElementCSharpType!, currentNamespace)}, {ElementUnmanagedType(namespaceName)}>(dimension: {ArraySourceEmitter.ElementCount(Dimensions)}, optionalsOnly: optionalsOnly);";
    }
}
