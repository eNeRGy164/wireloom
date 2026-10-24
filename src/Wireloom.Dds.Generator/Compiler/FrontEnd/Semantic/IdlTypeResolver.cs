using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves IDL names and aliases into the target-independent semantic type model.</summary>
internal sealed class IdlTypeResolver(IdlSymbolTable symbols)
{
    public IdlType? Resolve(string idlType, string? currentNamespace, IdlInput input, int offset) =>
        Resolve(idlType, currentNamespace, input, offset, wasEscaped: false);

    public IdlType? Resolve(string idlType, string? currentNamespace, IdlInput input, int offset, bool wasEscaped) =>
        Resolve(idlType, currentNamespace, input, offset, new(StringComparer.Ordinal), wasEscaped);

    private IdlType? Resolve(string idlType, string? currentNamespace, IdlInput input, int offset, HashSet<string> activeAliases, bool wasEscaped = false)
    {
        var normalized = IdlNaming.NormalizeIdlType(idlType);

        if (!wasEscaped && IdlBuiltinTypeSyntax.TryParseStringType(normalized, out var isWide, out var boundExpression))
        {
            if (boundExpression is null)
            {
                return new IdlType.StringType(isWide, input.StringSize, isBounded: false);
            }

            var bound = IdlBoundResolver.Resolve(
                input,
                offset,
                boundExpression,
                currentNamespace,
                symbols,
                "String bound");
            return new IdlType.StringType(isWide, bound, isBounded: true);
        }

        if (!wasEscaped && IdlNaming.IsPrimitive(idlType))
        {
            return new IdlType.Primitive(normalized);
        }

        if (!symbols.TryResolveTypeName(idlType, currentNamespace, out var qualified))
        {
            qualified = IdlNaming.ResolveTypeName(idlType, null);
        }

        if (symbols.TryGetEnum(qualified, out var enumDeclaration))
        {
            return new IdlType.Enum(qualified, enumDeclaration.DefaultMember.Value, enumDeclaration.DefaultMember.Name);
        }

        if (symbols.ContainsUnion(qualified))
        {
            return new IdlType.Union(qualified);
        }

        if (symbols.TryGetTypedef(qualified, out var alias))
        {
            if (alias.IsString)
            {
                return new IdlType.Alias(qualified, new IdlType.StringType(alias.IsWideString, alias.StringBound, alias.IsStringBounded));
            }

            if (!activeAliases.Add(qualified))
            {
                throw new IdlException(input, offset, $"Typedef alias cycle detected: {qualified}");
            }

            try
            {
                var target = alias.IsCollection
                    ? Resolve(alias.ElementType!, alias.Namespace, input, offset, activeAliases, alias.ElementTypeWasEscaped)
                    : Resolve(alias.Target, alias.Namespace, input, offset, activeAliases, alias.TargetWasEscaped);
                if (target is null)
                {
                    return null;
                }

                if (alias.IsSequence)
                {
                    var element = Resolve(alias.ElementType!, alias.Namespace, input, offset, activeAliases, alias.ElementTypeWasEscaped);
                    return element is null ? null : new IdlType.Alias(qualified,
                        new IdlType.Sequence(element, alias.Bound, effectiveBound: alias.EffectiveSequenceBound));
                }

                if (alias.IsArray)
                {
                    // Preserve collection typedef identity for nested and composed
                    // collection shapes instead of flattening the alias.
                    var element = Resolve(alias.ElementType!, alias.Namespace, input, offset, activeAliases, alias.ElementTypeWasEscaped);
                    return element is null ? null : new IdlType.Alias(qualified, new IdlType.Array(element, alias.Dimensions));
                }

                return new IdlType.Alias(qualified, target);
            }
            finally
            {
                activeAliases.Remove(qualified);
            }
        }

        return symbols.ContainsTypeName(qualified) ? new IdlType.Struct(qualified) : null;
    }

}
