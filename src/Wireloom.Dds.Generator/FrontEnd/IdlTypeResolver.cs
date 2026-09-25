namespace Wireloom;

using static IdlCompiler;

/// <summary>Resolves IDL names and aliases into the target-independent semantic type model.</summary>
internal sealed class IdlTypeResolver(IdlSymbolTable symbols)
{
    public IdlType? Resolve(string idlType, string? currentNamespace)
    {
        var normalized = NormalizeIdlType(idlType);

        if (normalized.StartsWith("string", StringComparison.Ordinal)
            || normalized.StartsWith("wstring", StringComparison.Ordinal))
        {
            var isWide = normalized.StartsWith("wstring", StringComparison.Ordinal);
            var bound = 255;
            var open = normalized.IndexOf('<');
            if (open >= 0 && normalized.EndsWith(">", StringComparison.Ordinal) &&
                int.TryParse(normalized[(open + 1)..^1].Trim(), out var parsedBound))
            {
                bound = parsedBound;
            }

            return new IdlType.StringType(isWide, bound);
        }

        if (IsPrimitive(idlType))
        {
            return new IdlType.Primitive(normalized);
        }

        var qualified = ResolveTypeName(idlType, currentNamespace);
        if (currentNamespace is not null
            && !idlType.StartsWith("::", StringComparison.Ordinal)
            && !idlType.Contains(".", StringComparison.Ordinal)
            && !symbols.ContainsName(qualified)
            && !symbols.ContainsEnum(qualified)
            && !symbols.ContainsTypedef(qualified))
        {
            qualified = ResolveTypeName(idlType, null);
        }

        if (symbols.TryGetEnum(qualified, out var enumDeclaration))
        {
            return new IdlType.Enum(qualified, enumDeclaration.DefaultMember.Value);
        }

        if (symbols.ContainsUnion(qualified))
        {
            return new IdlType.Union(qualified);
        }

        if (symbols.TryGetTypedef(qualified, out var alias))
        {
            if (alias.IsString)
            {
                return new IdlType.Alias(qualified, new IdlType.StringType(alias.IsWideString, alias.StringBound));
            }

            var target = Resolve(alias.IsCollection ? alias.ElementType! : alias.Target, alias.Namespace);
            if (target is null)
            {
                return null;
            }

            if (alias.IsSequence)
            {
                var element = Resolve(alias.ElementType!, alias.Namespace);
                return element is null ? null : new IdlType.Alias(qualified, new IdlType.Sequence(element, alias.Bound));
            }

            if (alias.IsArray)
            {
                // Preserve collection typedef identity for nested and composed
                // collection shapes instead of flattening the alias.
                var element = Resolve(alias.ElementType!, alias.Namespace);
                return element is null ? null : new IdlType.Alias(qualified, new IdlType.Array(element, alias.Dimensions));
            }

            return new IdlType.Alias(qualified, target);
        }

        return symbols.ContainsName(qualified) ? new IdlType.Struct(qualified) : null;
    }
}
