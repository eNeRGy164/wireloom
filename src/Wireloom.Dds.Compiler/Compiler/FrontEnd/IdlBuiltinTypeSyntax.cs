using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd;

/// <summary>Classifies built-in generic and string IDL type spellings.</summary>
internal static class IdlBuiltinTypeSyntax
{
    /// <summary>Determines whether a type spelling is an IDL sequence declaration.</summary>
    internal static bool IsSequenceType(string idlType)
    {
        var normalized = IdlNaming.NormalizeIdlType(idlType);
        return HasParameterizedKeyword(normalized, "sequence");
    }

    /// <summary>Parses an IDL string or wide-string spelling and its optional bound expression.</summary>
    internal static bool TryParseStringType(string idlType, out bool isWide, out string? boundExpression)
    {
        var normalized = IdlNaming.NormalizeIdlType(idlType);
        if (TryParseStringType(normalized, "wstring", out boundExpression))
        {
            isWide = true;
            return true;
        }

        if (TryParseStringType(normalized, "string", out boundExpression))
        {
            isWide = false;
            return true;
        }

        isWide = false;
        boundExpression = null;
        return false;
    }

    private static bool TryParseStringType(string normalized, string keyword, out string? boundExpression)
    {
        if (!normalized.StartsWith(keyword, StringComparison.Ordinal))
        {
            boundExpression = null;
            return false;
        }

        var remainder = normalized[keyword.Length..].TrimStart();
        if (remainder.Length == 0)
        {
            boundExpression = null;
            return true;
        }

        if (!remainder.StartsWith("<", StringComparison.Ordinal)
            || !remainder.EndsWith(">", StringComparison.Ordinal))
        {
            boundExpression = null;
            return false;
        }

        boundExpression = remainder[1..^1].Trim();
        return boundExpression.Length > 0;
    }

    private static bool HasParameterizedKeyword(string normalized, string keyword)
    {
        if (!normalized.StartsWith(keyword, StringComparison.Ordinal))
        {
            return false;
        }

        var remainder = normalized[keyword.Length..].TrimStart();
        return remainder.StartsWith("<", StringComparison.Ordinal)
            && remainder.EndsWith(">", StringComparison.Ordinal);
    }
}
