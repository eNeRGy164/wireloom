using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves enum values while preserving alias identity and absolute IDL scopes.</summary>
internal static class IdlEnumValueResolver
{
    /// <summary>Finds a literal or bound constant belonging to the expected enum.</summary>
    internal static IdlEnumMember? Resolve(string expression, IdlEnum expectedEnum, IdlSymbolTable symbols, string? currentNamespace, IdlInput input, int offset)
    {
        var expectedName = $"{expectedEnum.Namespace}.{expectedEnum.Name}".TrimStart('.');
        expression = expression.Trim();
        if (!IdlScopedName.IsValid(expression))
        {
            return null;
        }

        if (symbols.TryResolveConstant(expression, currentNamespace, out var constant))
        {
            return constant.Value is IdlScalarValue.Enumeration value
                && string.Equals(value.TypeQualifiedName, expectedName, StringComparison.Ordinal)
                ? expectedEnum.Members.SingleOrDefault(member => member.Name == value.MemberName)
                : null;
        }

        var separator = expression.LastIndexOf("::", StringComparison.Ordinal);
        var memberName = separator < 0 ? expression : expression[(separator + 2)..];
        if (separator > 0)
        {
            var resolvedType = new IdlTypeResolver(symbols).Resolve(expression[..separator], currentNamespace, input, offset);
            while (resolvedType is IdlType.Alias alias)
            {
                resolvedType = alias.Target;
            }

            if (resolvedType is not IdlType.Enum enumType
                || !string.Equals(enumType.QualifiedName, expectedName, StringComparison.Ordinal))
            {
                return null;
            }
        }
        else if (separator == 0 && expectedEnum.Namespace is not null)
        {
            return null;
        }

        return expectedEnum.Members.SingleOrDefault(member => member.Name == memberName);
    }
}
