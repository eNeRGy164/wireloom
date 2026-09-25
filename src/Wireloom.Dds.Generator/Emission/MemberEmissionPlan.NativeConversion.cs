namespace Wireloom;

using static IdlCompiler;

internal sealed partial class MemberEmissionPlan
{
    public string BuildFromNativeStatement(bool forwardKeysOnly, string? namespaceOverride = null)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsArray)
        {
            return BuildArrayFromNativeStatement(forwardKeysOnly, namespaceName);
        }

        if (IsSequence)
        {
            return BuildSequenceFromNativeStatement(namespaceName);
        }

        if (IsAggregate)
        {
            return BuildAggregateFromNativeStatement(forwardKeysOnly);
        }

        if (IsString)
        {
            return BuildStringFromNativeStatement();
        }

        return BuildPrimitiveFromNativeStatement();
    }

    public string? BuildFromNativeValueExpression()
    {
        if (IsArray || IsSequence || IsAggregate || IsOptional)
        {
            return null;
        }

        if (IsString)
        {
            return $"{EscapedName}.FromNative()";
        }

        return PrimitiveFromNativeExpression();
    }

    private string BuildArrayFromNativeStatement(bool forwardKeysOnly, string? namespaceName)
    {
        if (IsOptional)
        {
            var dimensions = $"new int[] {{ {string.Join(", ", Dimensions)} }}";
            var temporary = $"{EscapedName}Temporary_";
            var arrayType = TypeReference(CSharpType, namespaceName);

            if (HasAggregateElement)
            {
                return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(out {arrayType} {temporary}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")}, dimensions: {dimensions}); sample.{EscapedName} = {temporary};";
            }

            return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}>(out {arrayType} {temporary}, dimensions: {dimensions}); sample.{EscapedName} = {temporary};";
        }

        if (HasAggregateElement)
        {
            return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        return $"{EscapedName}.FromNative(sample.{EscapedName}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
    }

    private string BuildSequenceFromNativeStatement(string? namespaceName)
    {
        if (IsStringSequence && IsSequenceArray)
        {
            return $"{EscapedName}.FromNative(sample.{EscapedName});";
        }

        if (IsOptional)
        {
            var temporary = $"{EscapedName}Temporary_";

            if (HasAggregateElement)
            {
                return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(out ISequence<{TypeReference(ElementCSharpType!, namespaceName)}> {temporary}, keysOnly: false); sample.{EscapedName} = {temporary};";
            }

            return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}>(out Sequence<{TypeReference(ElementCSharpType!, namespaceName)}> {temporary}); sample.{EscapedName} = {temporary};";
        }

        if (HasAggregateElement)
        {
            return $"{EscapedName}.FromNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName});";
        }

        return $"{EscapedName}.FromNative((Sequence<{TypeReference(ElementCSharpType!, namespaceName)}>)sample.{EscapedName});";
    }

    private string BuildAggregateFromNativeStatement(bool forwardKeysOnly) =>
        $"{EscapedName}.FromNative(sample.{EscapedName}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")});";

    private string BuildStringFromNativeStatement() =>
        $"sample.{EscapedName} = {EscapedName}{(IsOptional ? ".FromNativeOptional();" : ".FromNative();")}";

    private string BuildPrimitiveFromNativeStatement() => IsOptional
        ? $"sample.{EscapedName} = {EscapedName}.FromNative<{NullableValueType()}>();"
        : $"sample.{EscapedName} = {PrimitiveFromNativeExpression()};";

    private string PrimitiveFromNativeExpression()
    {
        var name = EscapedName;

        if (ValueType is not PrimitiveEmissionType primitive)
        {
            return name;
        }

        return primitive.IdlName switch
        {
            "boolean" => $"Convert.ToBoolean({name})",
            "char" => $"NativeChar.FromUtf8({name})",
            "wchar" => $"(char){name}",
            _ => name
        };
    }

    private string PrimitiveToNativeExpression()
    {
        var name = $"sample.{EscapedName}";

        if (ValueType is not PrimitiveEmissionType primitive)
        {
            return name;
        }

        return primitive.IdlName switch
        {
            "boolean" => $"Convert.ToByte({name})",
            "char" => $"NativeChar.ToUtf8({name})",
            "wchar" => $"(short){name}",
            _ => name
        };
    }

    public string BuildToNativeStatement(bool forwardKeysOnly, string? namespaceOverride = null)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        if (IsArray)
        {
            return BuildArrayToNativeStatement(forwardKeysOnly, namespaceName);
        }

        if (IsSequence)
        {
            return BuildSequenceToNativeStatement(namespaceName);
        }

        if (IsAggregate)
        {
            return BuildAggregateToNativeStatement(forwardKeysOnly);
        }

        if (IsString)
        {
            return BuildStringToNativeStatement();
        }

        return BuildPrimitiveToNativeStatement();
    }

    private string BuildArrayToNativeStatement(bool forwardKeysOnly, string? namespaceName)
    {
        if (IsOptional && HasAggregateElement)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        if (IsOptional)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}>(sample.{EscapedName}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        if (HasAggregateElement)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
        }

        return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}>(sample.{EscapedName}, dimension: {ArraySourceEmitter.ElementCount(Dimensions)});";
    }

    private string BuildSequenceToNativeStatement(string? namespaceName)
    {
        if (IsStringSequence && IsSequenceArray)
        {
            return $"{EscapedName}.ToNative(sample.{EscapedName}, {ElementType!.Bound});";
        }

        if (IsOptional && HasAggregateElement)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName}, {Bound});";
        }

        if (IsOptional)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}>((Sequence<{TypeReference(ElementCSharpType!, namespaceName) }>)sample.{EscapedName}, {Bound});";
        }

        if (HasAggregateElement)
        {
            return $"{EscapedName}.ToNative<{TypeReference(ElementCSharpType!, namespaceName)}, {ElementUnmanagedType(namespaceName)}>(sample.{EscapedName});";
        }

        return $"{EscapedName}.ToNative((Sequence<{TypeReference(ElementCSharpType!, namespaceName)}>)sample.{EscapedName});";
    }

    private string BuildAggregateToNativeStatement(bool forwardKeysOnly) =>
        $"{EscapedName}.ToNative(sample.{EscapedName}, keysOnly: {(forwardKeysOnly ? "keysOnly" : "false")});";

    private string BuildStringToNativeStatement() =>
        $"{EscapedName}{(IsOptional ? ".ToNativeOptional(sample." : ".ToNative(sample.")}{EscapedName}, {Bound});";

    private string BuildPrimitiveToNativeStatement() => IsOptional
        ? $"{EscapedName}.ToNative<{NullableValueType()}>(sample.{EscapedName});"
        : $"{EscapedName} = {PrimitiveToNativeExpression()};";

}
