using Microsoft.CodeAnalysis.CSharp;
using static Wireloom.IdlGrammar;

namespace Wireloom;

public static partial class IdlCompiler
{
    /// <summary>Maps an IDL primitive spelling to its managed C# type.</summary>
    internal static string MapPrimitive(string idlType)
    {
        var normalized = NormalizeIdlType(idlType);

        // Managed primitive types are deliberately separate from the native and
        // dynamic RTI mappings used by the type-support emitter.
        return normalized switch
        {
            "short" or "int16" => "short",
            "long" or "int32" => "int",
            "long long" or "int64" => "long",
            "unsigned short" or "uint16" => "ushort",
            "unsigned long" or "uint32" => "uint",
            "unsigned long long" or "uint64" => "ulong",
            "int8" => "sbyte",
            "uint8" or "octet" => "byte",
            "boolean" => "bool",
            "char" or "wchar" => "char",
            "float" => "float",
            "double" => "double",
            "long double" => "LongDouble",
            _ => throw new InvalidOperationException($"Unsupported primitive type: {idlType}")
        };
    }

    /// <summary>Determines whether an IDL type spelling is a supported primitive.</summary>
    internal static bool IsPrimitive(string idlType) => NormalizeIdlType(idlType) switch
    {
        "short" or "int16" or "long" or "int32" or "long long" or "int64" or
        "unsigned short" or "uint16" or "unsigned long" or "uint32" or
        "unsigned long long" or "uint64" or "int8" or "uint8" or "octet" or
        "boolean" or "char" or "wchar" or "float" or "double" or "long double" => true,
        _ => false
    };

    /// <summary>Resolves an IDL type spelling against the current module namespace.</summary>
    internal static string ResolveTypeName(string idlType, string? currentNamespace)
    {
        var normalized = NormalizeIdlType(idlType).Replace("::", ".");
        var isAbsolute = normalized.StartsWith(".", StringComparison.Ordinal);
        normalized = normalized.TrimStart('.');

        if (isAbsolute || currentNamespace is null || normalized.StartsWith(currentNamespace + ".", StringComparison.Ordinal))
        {
            return normalized;
        }

        return currentNamespace + "." + normalized;
    }

    /// <summary>Normalizes whitespace in an IDL type spelling.</summary>
    internal static string NormalizeIdlType(string idlType) =>
        WhitespacePattern.Replace(idlType, " ").Trim();

    /// <summary>Shortens a generated type reference when the file already imports its namespace.</summary>
    internal static string TypeReference(string typeName, string? currentNamespace)
    {
        if (currentNamespace is null || typeName.IndexOf('.') < 0)
        {
            return typeName;
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
                return typeName.Substring(parentNamespace.Length + 1);
            }

            return typeName.Replace(parentNamespace + ".", string.Empty);
        }

        return typeName;
    }

    /// <summary>Creates the deterministic generated-document hint name.</summary>
    internal static string CreateHintName(string? currentNamespace, string typeName) =>
        (currentNamespace is null ? string.Empty : currentNamespace + ".") + typeName + ".g.cs";

    /// <summary>Escapes each segment of a qualified C# identifier.</summary>
    internal static string EscapeQualifiedIdentifier(string identifier) =>
        string.Join(".", identifier.Split('.').Select(EscapeIdentifier));

    /// <summary>Escapes a C# keyword while preserving ordinary identifiers.</summary>
    internal static string EscapeIdentifier(string identifier) =>
        SyntaxFacts.IsReservedKeyword(SyntaxFacts.GetKeywordKind(identifier))
            ? "@" + identifier
            : identifier;
}
