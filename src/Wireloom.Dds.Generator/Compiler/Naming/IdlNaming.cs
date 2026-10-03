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

    /// <summary>Normalizes whitespace in an IDL type spelling.</summary>
    internal static string NormalizeIdlType(string idlType) =>
        IdlGrammar.WhitespacePattern.Replace(idlType, " ").Trim();

    /// <summary>Shortens a generated type reference when the file already imports its namespace.</summary>
    internal static string TypeReference(string typeName, string? currentNamespace)
    {
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
    internal static string TypeReference(string typeName, string? declaringNamespace, string? currentNamespace) =>
        TypeReference(
            declaringNamespace is null
                ? EscapeIdentifier(typeName)
                : EscapeQualifiedIdentifier($"{declaringNamespace}.{typeName}"),
            currentNamespace);

    /// <summary>Creates an unambiguous C# reference for a resolved IDL type name.</summary>
    internal static string ResolvedTypeReference(string qualifiedName, string? currentNamespace)
    {
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
