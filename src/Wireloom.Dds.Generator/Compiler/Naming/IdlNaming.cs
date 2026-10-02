using Microsoft.CodeAnalysis.CSharp;
using Wireloom.Compiler.FrontEnd.Parsing;

namespace Wireloom.Compiler.Naming;

/// <summary>Provides shared IDL-to-C# naming and type-resolution helpers.</summary>
internal static class IdlNaming
{
    /// <summary>Maps an IDL primitive spelling to its managed C# type.</summary>
    internal static string MapPrimitive(string idlType)
    {
        return PrimitiveTypeMapping.Resolve(idlType).ManagedType;
    }

    /// <summary>Determines whether an IDL type spelling is a supported primitive.</summary>
    internal static bool IsPrimitive(string idlType) => PrimitiveTypeMapping.IsPrimitive(idlType);

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

    /// <summary>Shortens a generated type reference when the file already imports its namespace.</summary>
    internal static string TypeReference(string typeName, string? currentNamespace)
    {
        if (typeName.StartsWith("global::", StringComparison.Ordinal))
        {
            return typeName;
        }

        if (currentNamespace is null || typeName.IndexOf('.') < 0)
        {
            return typeName;
        }

        var originalTypeName = typeName;
        var lastDot = typeName.LastIndexOf('.');
        var declaringNamespace = typeName[..lastDot];
        var simpleName = typeName[(lastDot + 1)..];
        if (string.Equals(simpleName, "System", StringComparison.Ordinal)
            && !string.Equals(declaringNamespace, currentNamespace, StringComparison.Ordinal))
        {
            return $"global::{EscapeQualifiedIdentifier(originalTypeName)}";
        }

        typeName = typeName.Replace(currentNamespace + ".", string.Empty);

        const string implementationSuffix = ".Implementation";

        if (currentNamespace.EndsWith(implementationSuffix, StringComparison.Ordinal))
        {
            var parentNamespace = currentNamespace.Substring(
                0,
                currentNamespace.Length - implementationSuffix.Length);

            if (typeName.StartsWith(parentNamespace + ".", StringComparison.Ordinal))
            {
                return typeName[(parentNamespace.Length + 1)..];
            }

            return typeName.Replace(parentNamespace + ".", string.Empty);
        }

        return typeName;
    }

    /// <summary>Resolves a generated type from its declaring namespace in a generated document.</summary>
    internal static string TypeReference(string typeName, string? declaringNamespace, string? currentNamespace)
    {
        if (declaringNamespace is null)
        {
            return ResolvedTypeReference(typeName, currentNamespace);
        }

        return TypeReference(EscapeQualifiedIdentifier($"{declaringNamespace}.{typeName}"), currentNamespace);
    }

    /// <summary>Creates an unambiguous C# reference for a resolved IDL type name.</summary>
    internal static string ResolvedTypeReference(string qualifiedName, string? currentNamespace)
    {
        if (qualifiedName.StartsWith("global::", StringComparison.Ordinal))
        {
            return qualifiedName;
        }

        var normalized = qualifiedName.Replace("::", ".");
        var escaped = EscapeQualifiedIdentifier(normalized);

        if (currentNamespace is not null && normalized.StartsWith($"{currentNamespace}.", StringComparison.Ordinal))
        {
            return TypeReference(escaped, currentNamespace);
        }

        if (currentNamespace is null)
        {
            return escaped;
        }

        return $"global::{escaped}";
    }

    /// <summary>Preserves the declaring scope needed to resolve a generated support type.</summary>
    internal static string SupportTypeName(string qualifiedName, string? currentNamespace)
    {
        var escaped = EscapeQualifiedIdentifier(qualifiedName);
        if (currentNamespace is not null && qualifiedName.IndexOf('.') < 0)
        {
            return $"global::{escaped}";
        }

        return escaped;
    }

    /// <summary>Creates a shadowing-safe reference to a generated type-support class.</summary>
    internal static string GeneratedSupportTypeReference(string typeName, string? implementationNamespace)
    {
        const string implementationSuffix = ".Implementation";

        if (typeName.StartsWith("global::", StringComparison.Ordinal))
        {
            return typeName;
        }

        if (typeName.IndexOf('.') < 0)
        {
            if (implementationNamespace is null)
            {
                return typeName;
            }

            if (string.Equals(implementationNamespace, "Implementation", StringComparison.Ordinal))
            {
                return $"global::{EscapeIdentifier(typeName)}";
            }

            return typeName;
        }

        var declarationNamespace = implementationNamespace;
        if (declarationNamespace?.EndsWith(implementationSuffix, StringComparison.Ordinal) == true)
        {
            declarationNamespace = declarationNamespace[..^implementationSuffix.Length];
        }

        return ResolvedTypeReference(typeName, declarationNamespace);
    }

    /// <summary>Creates the deterministic generated-document hint name.</summary>
    internal static GeneratedName CreateGeneratedName(string? currentNamespace, string typeName) =>
        new(currentNamespace ?? string.Empty, EscapeIdentifier(typeName));

    /// <summary>Creates the authoritative generated identities for a declaration.</summary>
    internal static GeneratedTypeNames CreateGeneratedTypeNames(string? currentNamespace, string declarationName) =>
        new(currentNamespace, declarationName);

    /// <summary>Escapes each segment of a qualified C# identifier.</summary>
    internal static string EscapeQualifiedIdentifier(string identifier) =>
        string.Join(".", identifier.Split('.').Select(EscapeIdentifier));

    /// <summary>Escapes a C# keyword while preserving ordinary identifiers.</summary>
    internal static string EscapeIdentifier(string identifier) =>
        SyntaxFacts.IsReservedKeyword(SyntaxFacts.GetKeywordKind(identifier))
            ? "@" + identifier
            : identifier;
}
