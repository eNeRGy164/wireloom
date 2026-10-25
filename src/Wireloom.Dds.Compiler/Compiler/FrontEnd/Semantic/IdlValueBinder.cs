using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Binds constant values, typedef defaults, and member annotation values in dependency order.</summary>
internal sealed class IdlValueBinder(IdlSymbolTable symbols, CancellationToken cancellationToken)
{
    private readonly IdlValueResolver resolver = new(symbols);
    private readonly List<(IdlMember Member, IdlInput Input, int Offset, string? Namespace, IdlMemberAnnotationSyntax Annotations)> deferredMembers = [];

    /// <summary>Queues member annotation values until types and constants are bound.</summary>
    internal void DeferMemberValues(IdlMember member, IdlInput input, int offset, string? currentNamespace, IdlMemberAnnotationSyntax annotations) =>
        deferredMembers.Add((member, input, offset, currentNamespace, annotations));

    /// <summary>Binds explicit member IDs and defaults against the completed type graph.</summary>
    internal void BindMemberValues()
    {
        foreach (var deferred in deferredMembers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            deferred.Member.SetAnnotationFlags(
                deferred.Annotations.KeyExpression is { } keyExpression
                    ? resolver.ResolveBooleanAnnotation(deferred.Input, deferred.Offset, deferred.Namespace, keyExpression, "key", deferred.Annotations.KeyExpressionWasEscaped)
                    : deferred.Annotations.IsKey,
                deferred.Annotations.OptionalExpression is { } optionalExpression
                    ? resolver.ResolveBooleanAnnotation(deferred.Input, deferred.Offset, deferred.Namespace, optionalExpression, "optional", deferred.Annotations.OptionalExpressionWasEscaped)
                    : deferred.Annotations.IsOptional,
                deferred.Annotations.ExternalExpression is { } externalExpression
                    ? resolver.ResolveBooleanAnnotation(deferred.Input, deferred.Offset, deferred.Namespace, externalExpression, "external", deferred.Annotations.ExternalExpressionWasEscaped)
                    : deferred.Annotations.IsExternal,
                deferred.Annotations.MustUnderstandExpression is { } mustUnderstandExpression
                    ? resolver.ResolveBooleanAnnotation(deferred.Input, deferred.Offset, deferred.Namespace, mustUnderstandExpression, "must_understand", deferred.Annotations.MustUnderstandExpressionWasEscaped)
                    : deferred.Annotations.IsMustUnderstand);

            if (deferred.Annotations.MemberIdExpression is { } expression)
            {
                BigInteger value;
                try
                {
                    value = ((IdlScalarValue.Integral)IdlTypedExpressionEvaluator.Evaluate(expression, "long", symbols, deferred.Namespace)).Value;
                }
                catch (FormatException exception)
                {
                    throw new IdlException(deferred.Input, deferred.Offset, $"Invalid member ID expression: {exception.Message}");
                }

                if (value < int.MinValue || value > int.MaxValue)
                {
                    throw new IdlException(deferred.Input, deferred.Offset, "Duplicate or invalid @id annotation.");
                }

                deferred.Member.Metadata.SetMemberId((int)value);
            }

            deferred.Member.SetValueMetadata(resolver.ResolveMemberValueMetadata(deferred.Input, deferred.Offset, deferred.Namespace, deferred.Member.Type, deferred.Annotations));
        }

        deferredMembers.Clear();
    }

    /// <summary>Binds integer, boolean, character, and enum constants before resolving bounds.</summary>
    internal void BindDiscreteConstants(IEnumerable<IdlDeclaration> declarations)
    {
        foreach (var declaration in declarations.OfType<IdlConstantDeclaration>())
        {
            cancellationToken.ThrowIfCancellationRequested();
            declaration.SetValue(EvaluateConstant(declaration));
        }
    }

    /// <summary>Binds string and floating constants after collection bounds have resolved.</summary>
    internal void BindTextAndFloatingConstants(IEnumerable<IdlDeclaration> declarations)
    {
        var typeResolver = new IdlTypeResolver(symbols);
        foreach (var declaration in declarations.OfType<IdlConstantDeclaration>())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var constantType = typeResolver.Resolve(declaration.Type, declaration.Namespace, declaration.SourceInput, declaration.SourceOffset, declaration.TypeWasEscaped);
            while (constantType is IdlType.Alias alias)
            {
                constantType = alias.Target;
            }

            if (constantType is IdlType.StringType stringType)
            {
                declaration.SetValue(new IdlScalarValue.Text(resolver.ResolveStringConstantValue(declaration, stringType), stringType.IsWide));
            }
            else if (constantType is IdlType.Primitive { Name: "float" or "double" } floatingType)
            {
                declaration.SetValue(new IdlScalarValue.Floating(resolver.ResolveFloatingConstantValue(declaration, floatingType.Name)));
            }
            else if (constantType is IdlType.Primitive { Name: "long double" })
            {
                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, "long double constants are not supported.");
            }
        }
    }

    /// <summary>Binds explicit typedef defaults before inheriting the nearest annotated default.</summary>
    internal void BindTypedefDefaults(IEnumerable<IdlDeclaration> declarations, IReadOnlyDictionary<string, (IdlInput Input, int Offset)> typedefLocations)
    {
        var typeResolver = new IdlTypeResolver(symbols);
        var typedefDeclarations = declarations.OfType<IdlTypedefDeclaration>().ToArray();
        foreach (var typedefDeclaration in typedefDeclarations)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var application = typedefDeclaration.AnnotationApplications
                .SingleOrDefault(candidate => candidate.Name.Equals("default", StringComparison.OrdinalIgnoreCase));
            if (application is null)
            {
                continue;
            }

            var typedef = typedefDeclaration.Declaration;
            var qualifiedName = typedef.Namespace is null ? typedef.Name : $"{typedef.Namespace}.{typedef.Name}";
            if (!typedefLocations.TryGetValue(qualifiedName, out var location))
            {
                continue;
            }

            var resolvedType = typedef.IsString
                ? new IdlType.StringType(typedef.IsWideString, typedef.StringBound, typedef.IsStringBounded)
                : typeResolver.Resolve(typedef.Target, typedef.Namespace, location.Input, application.Offset, typedef.TargetWasEscaped);
            if (resolvedType is null)
            {
                continue;
            }

            typedef.SetValueMetadata(resolver.ResolveTypedefDefaultMetadata(
                location.Input,
                application.Offset,
                typedef.Namespace,
                resolvedType,
                application.ParseArguments().Single().Expression));
        }

        foreach (var typedefDeclaration in typedefDeclarations)
        {
            var typedef = typedefDeclaration.Declaration;
            var hasExplicitDefault = typedefDeclaration.AnnotationApplications
                .Any(application => application.Name.Equals("default", StringComparison.OrdinalIgnoreCase));
            if (typedef.ValueMetadata?.HasDefault == true || hasExplicitDefault || typedef.IsCollection)
            {
                continue;
            }

            var qualifiedName = typedef.Namespace is null ? typedef.Name : $"{typedef.Namespace}.{typedef.Name}";
            if (!typedefLocations.TryGetValue(qualifiedName, out var location))
            {
                continue;
            }

            var resolvedTarget = typeResolver.Resolve(
                typedef.Target,
                typedef.Namespace,
                location.Input,
                location.Offset,
                typedef.TargetWasEscaped);
            var inheritedDefault = resolvedTarget is null
                ? null
                : resolver.ResolveInheritedTypedefDefaultMetadata(resolvedTarget);
            if (inheritedDefault is not null)
            {
                typedef.SetValueMetadata(inheritedDefault);
            }
        }
    }

    private IdlScalarValue? EvaluateConstant(IdlConstantDeclaration declaration) =>
        EvaluateConstant(declaration, new HashSet<string>(StringComparer.Ordinal));

    private IdlScalarValue? EvaluateConstant(IdlConstantDeclaration declaration, HashSet<string> activeConstants)
    {
        var qualifiedName = declaration.Namespace is null ? declaration.Name : $"{declaration.Namespace}.{declaration.Name}";
        if (!activeConstants.Add(qualifiedName))
        {
            throw new IdlException(
                declaration.SourceInput,
                declaration.SourceOffset,
                $"Cyclic constant reference involving '{declaration.Name}'.");
        }

        try
        {
            var resolvedType = new IdlTypeResolver(symbols).Resolve(
                declaration.Type,
                declaration.Namespace,
                declaration.SourceInput,
                declaration.SourceOffset,
                declaration.TypeWasEscaped);
            while (resolvedType is IdlType.Alias alias)
            {
                resolvedType = alias.Target;
            }

            if (resolvedType is IdlType.StringType
                || resolvedType is IdlType.Primitive { Name: "float" or "double" or "long double" })
            {
                return null;
            }

            if (resolvedType is IdlType.Primitive { Name: "boolean" })
            {
                if (!declaration.ExpressionWasEscaped && string.Equals(declaration.Expression, "TRUE", StringComparison.OrdinalIgnoreCase))
                {
                    return new IdlScalarValue.Boolean(true);
                }

                if (!declaration.ExpressionWasEscaped && string.Equals(declaration.Expression, "FALSE", StringComparison.OrdinalIgnoreCase))
                {
                    return new IdlScalarValue.Boolean(false);
                }

                if (symbols.TryResolveConstant(declaration.Expression.Trim(), declaration.Namespace, out var referencedConstant))
                {
                    var referencedType = new IdlTypeResolver(symbols).Resolve(
                        referencedConstant.Type,
                        referencedConstant.Namespace,
                        referencedConstant.SourceInput,
                        referencedConstant.SourceOffset,
                        referencedConstant.TypeWasEscaped);
                    while (referencedType is IdlType.Alias referencedAlias)
                    {
                        referencedType = referencedAlias.Target;
                    }

                    if (referencedType is IdlType.Primitive { Name: "boolean" })
                    {
                        return EvaluateConstant(referencedConstant, activeConstants);
                    }
                }

                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Invalid boolean constant expression: {declaration.Expression}");
            }

            if (resolvedType is IdlType.Primitive { Name: "char" or "wchar" } characterType)
            {
                if (declaration.Expression.StartsWith("L'", StringComparison.Ordinal) != (characterType.Name == "wchar")
                    || !IdlCharacterLiteralParser.TryParse(
                    declaration.Expression,
                    allowWideLiteral: characterType.Name == "wchar",
                    out var character))
                {
                    if (symbols.TryResolveConstant(declaration.Expression.Trim(), declaration.Namespace, out var referencedConstant))
                    {
                        var referencedType = new IdlTypeResolver(symbols).Resolve(
                            referencedConstant.Type,
                            referencedConstant.Namespace,
                            referencedConstant.SourceInput,
                            referencedConstant.SourceOffset,
                            referencedConstant.TypeWasEscaped);
                        while (referencedType is IdlType.Alias referencedAlias)
                        {
                            referencedType = referencedAlias.Target;
                        }

                        if (referencedType is IdlType.Primitive { Name: "char" or "wchar" } referencedCharacterType
                            && string.Equals(referencedCharacterType.Name, characterType.Name, StringComparison.Ordinal))
                        {
                            return EvaluateConstant(referencedConstant, activeConstants);
                        }
                    }

                    throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Invalid {characterType.Name} constant expression: {declaration.Expression}");
                }

                var value = new BigInteger(character);
                IdlScalarTypeRules.ValidateConstantRange(declaration.SourceInput, declaration.SourceOffset, characterType.Name, value);
                return new IdlScalarValue.Character((char)character, characterType.Name == "wchar");
            }

            if (resolvedType is IdlType.Enum enumType)
            {
                return EvaluateEnumConstant(declaration, enumType.QualifiedName, activeConstants);
            }

            if (resolvedType is not IdlType.Primitive primitive || !IsIntegralConstantType(primitive.Name))
            {
                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Unknown constant type: {declaration.Type}");
            }

            try
            {
                var scalar = IdlTypedExpressionEvaluator.Evaluate(declaration.Expression, primitive.Name, symbols, declaration.Namespace);
                var value = ((IdlScalarValue.Integral)scalar).Value;
                IdlScalarTypeRules.ValidateConstantRange(declaration.SourceInput, declaration.SourceOffset, primitive.Name, value);
                return new IdlScalarValue.Integral(value);
            }
            catch (FormatException exception)
            {
                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Invalid {declaration.Type} constant expression: {exception.Message}");
            }
        }
        finally
        {
            activeConstants.Remove(qualifiedName);
        }
    }

    private IdlScalarValue.Enumeration EvaluateEnumConstant(
        IdlConstantDeclaration declaration,
        string enumTypeName,
        HashSet<string> activeConstants)
    {
        if (!symbols.TryGetEnum(enumTypeName, out var enumDeclaration))
        {
            throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Unknown constant type: {declaration.Type}");
        }

        if (symbols.TryResolveConstant(declaration.Expression.Trim(), declaration.Namespace, out var referencedConstant))
        {
            var referencedValue = referencedConstant.Value ?? EvaluateConstant(referencedConstant, activeConstants);
            if (referencedValue is not IdlScalarValue.Enumeration enumValue
                || !string.Equals(enumValue.TypeQualifiedName, enumTypeName, StringComparison.Ordinal))
            {
                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Enum constant value '{declaration.Expression}' does not belong to {enumTypeName}.");
            }

            return enumValue;
        }

        var member = IdlEnumValueResolver.Resolve(declaration.Expression, enumDeclaration, symbols, declaration.Namespace, declaration.SourceInput, declaration.SourceOffset);
        if (member is null)
        {
            if (declaration.Expression.Contains("::"))
            {
                throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Enum constant value '{declaration.Expression}' does not belong to {enumTypeName}.");
            }

            throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Unknown enum constant value: {declaration.Expression}");
        }

        return new IdlScalarValue.Enumeration(enumTypeName, member.Name, member.Value);
    }

    private static bool IsIntegralConstantType(string type) => type is
        "short" or "int16" or "unsigned short" or "uint16" or "long" or "int32" or "unsigned long" or "uint32"
        or "long long" or "int64" or "unsigned long long" or "uint64" or "int8" or "uint8" or "octet";

}
