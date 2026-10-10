using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Builds native conversion operations for a member emission plan.</summary>
internal sealed partial class MemberEmissionPlan
{
    /// <summary>Builds a statement that copies this member from native storage.</summary>
    public string BuildFromNativeStatement(bool forwardKeysOnly, string? namespaceOverride = null, string nativeFieldPrefix = "")
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsArray)
        {
            return BuildArrayFromNativeStatement(forwardKeysOnly, namespaceName, nativeFieldPrefix);
        }

        if (IsSequence)
        {
            return BuildSequenceFromNativeStatement(namespaceName, nativeFieldPrefix);
        }

        if (IsAggregate)
        {
            return BuildAggregateFromNativeStatement(forwardKeysOnly, namespaceName, nativeFieldPrefix);
        }

        if (IsString)
        {
            return BuildStringFromNativeStatement(nativeFieldPrefix);
        }

        return BuildPrimitiveFromNativeStatement(nativeFieldPrefix);
    }

    /// <summary>Builds the expression that reads this member from native storage.</summary>
    public string? BuildFromNativeValueExpression(string nativeFieldPrefix = "")
    {
        if (IsArray || IsSequence || IsAggregate || IsOptional)
        {
            return null;
        }

        if (IsString)
        {
            return $"{nativeFieldPrefix}{EscapedName}.FromNative()";
        }

        return PrimitiveFromNativeExpression(nativeFieldPrefix);
    }

    private string BuildArrayFromNativeStatement(bool forwardKeysOnly, string? namespaceName, string nativeFieldPrefix)
    {
        if (IsOptional)
        {
            var dimensions = $"new int[] {{ {string.Join(", ", Dimensions)} }}";
            var temporary = $"{EscapedName}Temporary_";
            var arrayType = IdlNaming.TypeReference(CSharpType.TrimEnd('?'), namespaceName);

            if (HasAggregateElement)
            {
                return $"{nativeFieldPrefix}{EscapedName}.FromNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(out {arrayType} {temporary}, {KeysOnlyArgument(forwardKeysOnly)}, dimensions: {dimensions});\nsample.{EscapedName} = {temporary};";
            }

            return $"{nativeFieldPrefix}{EscapedName}.FromNative(out {arrayType} {temporary}, dimensions: {dimensions});\nsample.{EscapedName} = {temporary};";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.FromNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, {KeysOnlyArgument(forwardKeysOnly)}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        return $"{nativeFieldPrefix}{EscapedName}.FromNative(sample.{EscapedName}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
    }

    private string BuildSequenceFromNativeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        if (IsStringSequence && IsOptional)
        {
            var temporary = $"{EscapedName}Temporary_";
            var elementType = IdlNaming.TypeReference(ElementCSharpType!, namespaceName);

            return $"{nativeFieldPrefix}{EscapedName}.FromNative(out ISequence<{elementType}> {temporary});\nsample.{EscapedName} = {temporary};";
        }

        if (IsStringSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.FromNative(sample.{EscapedName});";
        }

        if (IsOptional)
        {
            var temporary = $"{EscapedName}Temporary_";

            if (HasAggregateElement)
            {
                return $"{nativeFieldPrefix}{EscapedName}.FromNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(out ISequence<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}> {temporary}, keysOnly: false);\nsample.{EscapedName} = {temporary};";
            }

            return $"{nativeFieldPrefix}{EscapedName}.FromNative(out Sequence<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}> {temporary});\nsample.{EscapedName} = {temporary};";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.FromNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName});";
        }

        return $"{nativeFieldPrefix}{EscapedName}.FromNative((Sequence<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}>)sample.{EscapedName});";
    }

    private string BuildAggregateFromNativeStatement(bool forwardKeysOnly, string? namespaceName, string nativeFieldPrefix)
    {
        if (IsOptional)
        {
            var temporary = $"{EscapedName}Temporary_";
            var type = IdlNaming.TypeReference(CSharpType.TrimEnd('?'), namespaceName);

            return $"{nativeFieldPrefix}{EscapedName}.FromNative<{type}, {GetReferencedUnmanagedType(namespaceName)}>(out var {temporary});\nsample.{EscapedName} = {temporary};";
        }

        return $"{nativeFieldPrefix}{EscapedName}.FromNative(sample.{EscapedName}, {KeysOnlyArgument(forwardKeysOnly)});";
    }

    private string BuildStringFromNativeStatement(string nativeFieldPrefix) =>
        $"sample.{EscapedName} = {nativeFieldPrefix}{EscapedName}{(IsOptional ? ".FromNativeOptional();" : ".FromNative();")}";

    private string BuildPrimitiveFromNativeStatement(string nativeFieldPrefix) => IsOptional
        ? $"sample.{EscapedName} = {nativeFieldPrefix}{EscapedName}.FromNative<{NullableValueType()}>();"
        : $"sample.{EscapedName} = {PrimitiveFromNativeExpression(nativeFieldPrefix)};";

    private string PrimitiveFromNativeExpression(string nativeFieldPrefix)
    {
        var name = $"{nativeFieldPrefix}{EscapedName}";

        if (ValueType is not PrimitiveEmissionType primitive)
        {
            return name;
        }

        return PrimitiveTypeMapping.Resolve(primitive.IdlName).FromNativeExpression(name);
    }

    private string PrimitiveToNativeExpression()
    {
        var name = $"sample.{EscapedName}";

        if (ValueType is not PrimitiveEmissionType primitive)
        {
            return name;
        }

        return PrimitiveTypeMapping.Resolve(primitive.IdlName).ToNativeExpression(name);
    }

    /// <summary>Builds a statement that copies this member to native storage.</summary>
    public string BuildToNativeStatement(bool forwardKeysOnly, string? namespaceOverride = null, string nativeFieldPrefix = "")
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsArray)
        {
            return BuildArrayToNativeStatement(forwardKeysOnly, namespaceName, nativeFieldPrefix);
        }

        if (IsSequence)
        {
            return BuildSequenceToNativeStatement(namespaceName, nativeFieldPrefix);
        }

        if (IsAggregate)
        {
            return BuildAggregateToNativeStatement(forwardKeysOnly, namespaceName, nativeFieldPrefix);
        }

        if (IsString)
        {
            return BuildStringToNativeStatement(nativeFieldPrefix);
        }

        return BuildPrimitiveToNativeStatement(nativeFieldPrefix);
    }

    private string BuildArrayToNativeStatement(bool forwardKeysOnly, string? namespaceName, string nativeFieldPrefix)
    {
        if (IsOptional && HasAggregateElement)
        {
            var dimensionArgument = forwardKeysOnly
                ? ArraySourceEmitter.ElementCount(Dimensions)
                : $"dimension: {ArraySourceEmitter.ElementCount(Dimensions)}";

            return $"{nativeFieldPrefix}{EscapedName}.ToNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, {KeysOnlyArgument(forwardKeysOnly)}, {dimensionArgument});";
        }

        if (IsOptional)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName}, {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, {KeysOnlyArgument(forwardKeysOnly)}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        return $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
    }

    private string BuildSequenceToNativeStatement(string? namespaceName, string nativeFieldPrefix)
    {
        if (IsStringSequence && IsOptional)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName}, {Bound}, {ElementType!.Bound});";
        }

        if (IsStringSequence)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName}, {ElementType!.Bound});";
        }

        if (IsOptional && HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, {Bound});";
        }

        if (IsOptional)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative((Sequence<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}>)sample.{EscapedName}!, {Bound});";
        }

        if (HasAggregateElement)
        {
            return $"{nativeFieldPrefix}{EscapedName}.ToNative<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName});";
        }

        return $"{nativeFieldPrefix}{EscapedName}.ToNative((Sequence<{IdlNaming.TypeReference(ElementCSharpType!, namespaceName)}>)sample.{EscapedName});";
    }

    private string BuildAggregateToNativeStatement(bool forwardKeysOnly, string? namespaceName, string nativeFieldPrefix)
    {
        if (IsOptional)
        {
            var type = IdlNaming.TypeReference(CSharpType.TrimEnd('?'), namespaceName);
            return $"{nativeFieldPrefix}{EscapedName}.ToNative<{type}, {GetReferencedUnmanagedType(namespaceName)}>(sample.{EscapedName}!);";
        }

        return $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName}, {KeysOnlyArgument(forwardKeysOnly)});";
    }

    private static string KeysOnlyArgument(bool forwardKeysOnly) =>
        forwardKeysOnly ? "keysOnly" : "keysOnly: false";

    private string BuildStringToNativeStatement(string nativeFieldPrefix) =>
        $"{nativeFieldPrefix}{EscapedName}{(IsOptional ? ".ToNativeOptional(sample." : ".ToNative(sample.")}{EscapedName}, {Bound});";

    private string BuildPrimitiveToNativeStatement(string nativeFieldPrefix) => IsOptional
        ? $"{nativeFieldPrefix}{EscapedName}.ToNative(sample.{EscapedName});"
        : $"{nativeFieldPrefix}{EscapedName} = {PrimitiveToNativeExpression()};";

}
