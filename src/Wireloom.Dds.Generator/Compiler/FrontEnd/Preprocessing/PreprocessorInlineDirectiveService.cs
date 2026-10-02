using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Consumes inline include probes and pragma forms embedded in expanded text.</summary>
internal sealed class PreprocessorInlineDirectiveService(
    PreprocessorMacroTokenService macroTokens,
    Func<string, string> expand,
    Func<string, bool, bool>? includeProbe,
    Action? pragmaOnce)
{
    private static readonly Regex IncludePattern = new(
        @"^(?:""(?<quoted>[^""]+)""|<(?<angle>[^>]+)>)\s*$",
        RegexOptions.Compiled);

    /// <summary>Attempts to expand a supported <c>__has_include</c> expression.</summary>
    /// <summary>Expands a supported has-include directive when present.</summary>
    internal bool TryExpandHasInclude(string text, ref int index, out bool result)
    {
        result = false;
        var open = index;
        while (open < text.Length && char.IsWhiteSpace(text[open]))
        {
            open++;
        }

        if (open >= text.Length || text[open] != '(' || !macroTokens.TryReadArguments(text, open, out var arguments, out var end))
        {
            return false;
        }

        if (arguments.Count == 1)
        {
            var match = IncludePattern.Match(expand(arguments[0]).Trim());
            if (match.Success)
            {
                var name = match.Groups["quoted"].Success
                    ? match.Groups["quoted"].Value
                    : match.Groups["angle"].Value;
                result = includeProbe?.Invoke(name, match.Groups["angle"].Success) == true;
            }
        }

        index = end;
        return true;
    }

    /// <summary>Attempts to consume a supported pragma expression.</summary>
    /// <summary>Consumes a supported pragma directive when present.</summary>
    internal bool TryConsumePragma(string text, ref int index)
    {
        var name = text.AsSpan(index).StartsWith("_Pragma", StringComparison.Ordinal)
            ? "_Pragma"
            : text.AsSpan(index).StartsWith("__pragma", StringComparison.Ordinal)
                ? "__pragma"
                : null;

        if (name is null || (index > 0 && PreprocessorLexicalService.IsIdentifierPart(text[index - 1])))
        {
            return false;
        }

        var open = index + name.Length;
        while (open < text.Length && char.IsWhiteSpace(text[open]))
        {
            open++;
        }

        if (open >= text.Length || text[open] != '(' || !macroTokens.TryReadArguments(text, open, out var arguments, out var end))
        {
            return false;
        }

        if (arguments.Count == 1)
        {
            var argument = arguments[0].Trim();
            var isOnce = name == "_Pragma"
                ? argument is ['"', .., '"'] && string.Equals(argument[1..^1], "once", StringComparison.Ordinal)
                : string.Equals(argument, "once", StringComparison.Ordinal);
            if (isOnce)
            {
                pragmaOnce?.Invoke();
            }
        }

        index = end;
        return true;
    }
}
