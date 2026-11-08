using Wireloom.Compiler.FrontEnd.Parsing;

namespace Wireloom.Compiler.FrontEnd;

/// <summary>Normalizes and resolves lexical IDL type names.</summary>
internal static class IdlNames
{
    private static readonly HashSet<string> PrimitiveNames =
    [
        "short", "int16",
        "long", "int32",
        "long long", "int64",
        "unsigned short", "uint16",
        "unsigned long", "uint32",
        "unsigned long long", "uint64",
        "int8", "uint8", "octet",
        "boolean", "char", "wchar",
        "float", "double", "long double"
    ];

    /// <summary>Determines whether an IDL spelling names a supported primitive.</summary>
    internal static bool IsPrimitive(string idlType) => PrimitiveNames.Contains(NormalizeIdlType(idlType));

    /// <summary>Resolves an IDL type spelling against the current module namespace.</summary>
    internal static string ResolveTypeName(string idlType, string? currentNamespace)
    {
        var normalized = NormalizeIdlType(idlType).Replace("::", ".");
        var isAbsolute = normalized.StartsWith(".", StringComparison.Ordinal);
        normalized = normalized.TrimStart('.');

        if (isAbsolute || currentNamespace is null || normalized.StartsWith($"{currentNamespace}.", StringComparison.Ordinal))
        {
            return normalized;
        }

        return $"{currentNamespace}.{normalized}";
    }

    /// <summary>Enumerates relative and enclosing-module candidates for an IDL type name.</summary>
    internal static IEnumerable<string> GetTypeNameCandidates(string idlType, string? currentNamespace)
    {
        var normalized = NormalizeIdlType(idlType).Replace("::", ".");
        var isAbsolute = normalized.StartsWith(".", StringComparison.Ordinal);
        normalized = normalized.TrimStart('.');

        if (isAbsolute || currentNamespace is null)
        {
            yield return normalized;
            yield break;
        }

        if (normalized.StartsWith($"{currentNamespace}.", StringComparison.Ordinal))
        {
            yield return normalized;
            yield break;
        }

        var scope = currentNamespace;
        while (true)
        {
            yield return $"{scope}.{normalized}";

            var separator = scope.LastIndexOf('.');
            if (separator < 0)
            {
                break;
            }

            scope = scope[..separator];
        }

        yield return normalized;
    }

    /// <summary>Normalizes whitespace in an IDL type spelling.</summary>
    internal static string NormalizeIdlType(string idlType) =>
        IdlGrammar.WhitespacePattern.Replace(idlType, " ").Trim();

}
