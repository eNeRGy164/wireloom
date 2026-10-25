using System.Text;
using Wireloom.Compiler.FrontEnd.Symbols;
using BigInt = System.Numerics.BigInteger;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves typed scalar values and member constraints against the complete symbol graph.</summary>
internal sealed class IdlValueResolver(IdlSymbolTable symbols)
{
    /// <summary>Resolves a member default and validates its declared range and unit constraints.</summary>
    internal IdlMemberValueMetadata? ResolveMemberValueMetadata(
        IdlInput input,
        int offset,
        string? currentNamespace,
        IdlType type,
        IdlMemberAnnotationSyntax annotations)
    {
        var inheritedDefault = annotations.DefaultExpression is null ? FindTypedefDefaultMetadata(type) : null;
        var defaultExpression = annotations.DefaultExpression ?? inheritedDefault?.DefaultExpression;
        if (annotations.MinimumExpression is null &&
            annotations.MaximumExpression is null &&
            defaultExpression is null &&
            annotations.UnitExpression is null)
        {
            return null;
        }

        var valueType = UnwrapAliases(type);
        if (valueType is not IdlType.Primitive and not IdlType.Enum and not IdlType.StringType)
        {
            throw new IdlException(input, offset, "@min, @max, @range, and @default are only supported on primitive and enum members.");
        }

        if (valueType is IdlType.Primitive { Name: "long double" }
            && (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null || defaultExpression is not null))
        {
            throw new IdlException(
                input,
                offset,
                "@min, @max, @range, and @default are not supported for long double.");
        }

        BigInt? minimum = null;
        BigInt? maximum = null;
        var defaultValue = inheritedDefault?.DefaultValue;

        if (valueType is IdlType.StringType &&
            (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null || annotations.UnitExpression is not null))
        {
            throw new IdlException(input, offset, "String members support @default but not @min, @max, @range, or @unit.");
        }

        if (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null)
        {
            if (valueType is not IdlType.Primitive primitive)
            {
                throw new IdlException(input, offset, "@min, @max, and @range require a primitive member.");
            }

            if (primitive.Name is "float" or "double")
            {
                throw new IdlException(input, offset, "@min, @max, and @range are not supported for floating-point members.");
            }

            minimum = annotations.MinimumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MinimumExpression, "minimum", primitive.Name);
            maximum = annotations.MaximumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MaximumExpression, "maximum", primitive.Name);

            if (minimum is { } minimumValue)
            {
                IdlScalarTypeRules.ValidateConstantRange(input, offset, primitive.Name, minimumValue);
            }

            if (maximum is { } maximumValue)
            {
                IdlScalarTypeRules.ValidateConstantRange(input, offset, primitive.Name, maximumValue);
            }

            if (minimum is { } lower && maximum is { } upper && lower > upper)
            {
                throw new IdlException(input, offset, "Member minimum value cannot be greater than its maximum value.");
            }
        }

        if (defaultExpression is not null)
        {
            if (annotations.DefaultExpression is null && inheritedDefault is not null)
            {
                // Typedef defaults were already evaluated in the typedef's own IDL scope.
                // Reusing the typed value avoids resolving its expression in the consuming scope.
                if (defaultValue is IdlScalarValue.Text { Value: var inheritedString } && valueType is IdlType.StringType stringType)
                {
                    var inheritedLength = stringType.IsWide ? inheritedString.Length : Encoding.UTF8.GetByteCount(inheritedString);
                    if (inheritedLength > stringType.Bound)
                    {
                        throw new IdlException(input, offset, $"String default exceeds the effective bound of {stringType.Bound}.");
                    }
                }
            }
            else if (valueType is IdlType.StringType stringType)
            {
                var stringDefaultValue = ParseStringDefault(input, offset, currentNamespace, defaultExpression, stringType.IsWide);
                defaultValue = new IdlScalarValue.Text(stringDefaultValue, stringType.IsWide);
                var defaultLength = stringType.IsWide
                    ? stringDefaultValue.Length
                    : Encoding.UTF8.GetByteCount(stringDefaultValue);
                if (defaultLength > stringType.Bound)
                {
                    throw new IdlException(input, offset, $"String default exceeds the effective bound of {stringType.Bound}.");
                }
            }
            else if (valueType is IdlType.Primitive { Name: "float" })
            {
                var floatingDefaultValue = ParseFloatingDefault(input, offset, currentNamespace, defaultExpression, "float");
                if (!double.IsNaN(floatingDefaultValue) && !double.IsInfinity(floatingDefaultValue) && Math.Abs(floatingDefaultValue) > float.MaxValue)
                {
                    throw new IdlException(input, offset, "Default value is outside the range of float.");
                }

                defaultValue = new IdlScalarValue.Floating(floatingDefaultValue);
            }
            else if (valueType is IdlType.Primitive { Name: "double" })
            {
                defaultValue = new IdlScalarValue.Floating(ParseFloatingDefault(input, offset, currentNamespace, defaultExpression, "double"));
            }
            else if (valueType is IdlType.Primitive { Name: "boolean" })
            {
                defaultValue = new IdlScalarValue.Boolean(ParseBooleanDefault(
                    input,
                    offset,
                    currentNamespace,
                    defaultExpression,
                    annotations.DefaultExpressionWasEscaped));
            }
            else if (valueType is IdlType.Primitive { Name: "char" or "wchar" } characterType)
            {
                var character = ParseCharacterDefault(input, offset, currentNamespace, defaultExpression, characterType.Name);
                IdlScalarTypeRules.ValidateConstantRange(input, offset, characterType.Name, character);
                defaultValue = new IdlScalarValue.Character((char)(int)character, characterType.Name == "wchar");
            }
            else if (valueType is IdlType.Primitive primitive)
            {
                var integer = EvaluateMemberInteger(input, offset, currentNamespace, defaultExpression, "default", primitive.Name);
                IdlScalarTypeRules.ValidateConstantRange(input, offset, primitive.Name, integer);
                defaultValue = new IdlScalarValue.Integral(integer);
            }
            else if (valueType is IdlType.Enum @enum)
            {
                if (!symbols.TryGetEnum(@enum.QualifiedName, out var enumDeclaration))
                {
                    throw new IdlException(input, offset, $"Unknown enum type: {@enum.QualifiedName}");
                }

                var enumMember = IdlEnumValueResolver.Resolve(defaultExpression, enumDeclaration, symbols, currentNamespace, input, offset);
                if (enumMember is null)
                {
                    throw new IdlException(input, offset, $"Unknown default enum value: {defaultExpression}");
                }

                defaultValue = new IdlScalarValue.Enumeration(@enum.QualifiedName, enumMember.Name, enumMember.Value);
            }
        }

        var rangeValue = defaultValue switch
        {
            IdlScalarValue.Integral integer => integer.Value,
            IdlScalarValue.Boolean boolean => boolean.Value ? BigInteger.One : BigInteger.Zero,
            IdlScalarValue.Character character => new BigInteger(character.Value),
            _ => (BigInteger?)null
        };
        if (rangeValue is { } value && ((minimum is { } defaultLower && value < defaultLower) || (maximum is { } defaultUpper && value > defaultUpper)))
        {
            throw new IdlException(input, offset, "Member default value is outside its declared range.");
        }

        return new IdlMemberValueMetadata(defaultValue, minimum, maximum, defaultExpression, annotations.UnitExpression);
    }

    /// <summary>Parses the declared default after the typedef target and referenced constants are bound.</summary>
    internal IdlMemberValueMetadata? ResolveTypedefDefaultMetadata(
        IdlInput input,
        int offset,
        string? currentNamespace,
        IdlType type,
        string defaultExpression)
    {
        var annotations = new IdlMemberAnnotationSyntax { DefaultExpression = defaultExpression };
        return ResolveMemberValueMetadata(input, offset, currentNamespace, type, annotations);
    }

    /// <summary>Returns the nearest bound default inherited through a typedef chain.</summary>
    internal IdlMemberValueMetadata? ResolveInheritedTypedefDefaultMetadata(IdlType type) =>
        FindTypedefDefaultMetadata(type);

    private IdlMemberValueMetadata? FindTypedefDefaultMetadata(IdlType type)
    {
        for (var current = type; current is IdlType.Alias alias; current = alias.Target)
        {
            if (symbols.TryGetTypedef(alias.QualifiedName, out var declaration) && declaration.ValueMetadata?.HasDefault == true)
            {
                return declaration.ValueMetadata;
            }
        }

        return null;
    }

    /// <summary>Validates a floating constant even when no member or typedef default references it.</summary>
    internal double ResolveFloatingConstantValue(IdlConstantDeclaration declaration, string typeName)
    {
        var value = ParseFloatingDefault(declaration.SourceInput, declaration.SourceOffset, declaration.Namespace, declaration.Expression, typeName, valueKind: "constant");
        if (typeName == "float" && Math.Abs(value) > float.MaxValue)
        {
            throw new IdlException(declaration.SourceInput, declaration.SourceOffset, "float constant value is outside its representable range.");
        }

        return typeName == "float" ? (float)value : value;
    }

    private double ParseFloatingDefault(IdlInput input, int offset, string? currentNamespace, string expression, string typeName, string valueKind = "default")
    {
        double value;
        try
        {
            value = ((IdlScalarValue.Floating)IdlTypedExpressionEvaluator.Evaluate(expression, typeName, symbols, currentNamespace)).Value;
        }
        catch (FormatException)
        {
            throw new IdlException(input, offset, $"Invalid {typeName} {valueKind} expression: {expression}");
        }

        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new IdlException(input, offset, $"Invalid {typeName} {valueKind} expression: {expression}");
        }

        return value;
    }

    private bool ParseBooleanDefault(IdlInput input, int offset, string? currentNamespace, string expression, bool expressionWasEscaped = false)
    {
        if (!expressionWasEscaped && string.Equals(expression, "TRUE", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!expressionWasEscaped && string.Equals(expression, "FALSE", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (symbols.TryResolveConstant(expression, currentNamespace, out var constant)
            && constant.Value is IdlScalarValue.Boolean { Value: var constantValue })
        {
            return constantValue;
        }

        throw new IdlException(input, offset, $"Invalid boolean default expression: {expression}");
    }

    /// <summary>Resolves a boolean annotation argument after the constant symbol graph is bound.</summary>
    internal bool ResolveBooleanAnnotation(IdlInput input, int offset, string? currentNamespace, string expression, string annotationName, bool expressionWasEscaped = false)
    {
        if (!expressionWasEscaped && string.Equals(expression, "TRUE", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!expressionWasEscaped && string.Equals(expression, "FALSE", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (symbols.TryResolveConstant(expression, currentNamespace, out var constant)
            && constant.Value is IdlScalarValue.Boolean { Value: var value })
        {
            return value;
        }

        throw new IdlException(input, offset, $"Invalid boolean argument for @{annotationName}: {expression}");
    }

    private BigInt ParseCharacterDefault(
        IdlInput input,
        int offset,
        string? currentNamespace,
        string expression,
        string typeName)
    {
        if (expression.StartsWith("L'", StringComparison.Ordinal) == (typeName == "wchar")
            && IdlCharacterLiteralParser.TryParse(expression, allowWideLiteral: typeName == "wchar", out var character))
        {
            return character;
        }

        if (symbols.TryResolveConstant(expression, currentNamespace, out var constant))
        {
            if (constant.Value is not IdlScalarValue.Character characterValue || characterValue.IsWide != (typeName == "wchar"))
            {
                throw new IdlException(input, offset, $"Character default constant '{expression}' has type {constant.Type}, not {typeName}.");
            }

            return characterValue.Value;
        }

        throw new IdlException(input, offset, $"Invalid {typeName} default expression: {expression}");
    }

    /// <summary>Decodes a string constant and validates its declared bound in encoding units.</summary>
    internal string ResolveStringConstantValue(IdlConstantDeclaration declaration, IdlType.StringType type)
    {
        var value = ParseStringDefault(declaration.SourceInput, declaration.SourceOffset, declaration.Namespace, declaration.Expression, type.IsWide);
        var length = type.IsWide ? value.Length : Encoding.UTF8.GetByteCount(value);
        if (type.IsBounded && length > type.Bound)
        {
            throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"String constant exceeds its declared bound of {type.Bound}.");
        }

        return value;
    }

    private string ParseStringDefault(
        IdlInput input,
        int offset,
        string? currentNamespace,
        string expression,
        bool isWide,
        HashSet<string>? activeStringConstants = null)
    {
        expression = expression.Trim();
        if (symbols.TryResolveConstant(expression, currentNamespace, out var constant))
        {
            if (constant.Value is IdlScalarValue.Text text)
            {
                ValidateStringConstantWidth(input, offset, expression, isWide, text.IsWide);
                return text.Value;
            }

            var resolvedConstantType = new IdlTypeResolver(symbols).Resolve(
                constant.Type,
                constant.Namespace,
                constant.SourceInput,
                constant.SourceOffset,
                constant.TypeWasEscaped);
            if (resolvedConstantType is not null
                && UnwrapAliases(resolvedConstantType) is IdlType.StringType stringConstantType)
            {
                ValidateStringConstantWidth(input, offset, expression, isWide, stringConstantType.IsWide);

                activeStringConstants ??= new HashSet<string>(StringComparer.Ordinal);
                var qualifiedName = constant.Namespace is null ? constant.Name : $"{constant.Namespace}.{constant.Name}";
                if (!activeStringConstants.Add(qualifiedName))
                {
                    throw new IdlException(constant.SourceInput, constant.SourceOffset, $"Cyclic string constant reference involving '{constant.Name}'.");
                }

                try
                {
                    return ParseStringDefault(
                        constant.SourceInput,
                        constant.SourceOffset,
                        constant.Namespace,
                        constant.Expression,
                        isWide,
                        activeStringConstants: activeStringConstants);
                }
                finally
                {
                    activeStringConstants.Remove(qualifiedName);
                }
            }
        }

        return IdlStringLiteralDecoder.Decode(expression, isWide, input, offset);
    }

    private static void ValidateStringConstantWidth(IdlInput input, int offset, string expression, bool targetIsWide, bool valueIsWide)
    {
        if (targetIsWide != valueIsWide)
        {
            throw new IdlException(input, offset, $"String default constant '{expression}' has an incompatible narrow or wide string type.");
        }
    }

    private BigInteger EvaluateMemberInteger(
        IdlInput input,
        int offset,
        string? currentNamespace,
        string expression,
        string valueName,
        string? expectedType = null)
    {
        try
        {
            var expressionType = expectedType is "boolean" or "char" or "wchar" ? "long" : expectedType ?? "long long";
            var scalar = IdlTypedExpressionEvaluator.Evaluate(expression, expressionType, symbols, currentNamespace);
            return ((IdlScalarValue.Integral)scalar).Value;
        }
        catch (FormatException exception)
        {
            throw new IdlException(input, offset, $"Invalid {valueName} expression: {exception.Message}");
        }
    }

    private static IdlType UnwrapAliases(IdlType type)
    {
        while (type is IdlType.Alias alias)
        {
            type = alias.Target;
        }

        return type;
    }

}
