using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private static readonly Regex StringifyPattern = new(
        @"(?<!#)#(?!#)\s*(?<name>[A-Za-z_]\w*)",
        RegexOptions.Compiled);

    private static readonly Regex CharizePattern = new(
        @"(?<!#)#@\s*(?<name>[A-Za-z_]\w*)",
        RegexOptions.Compiled);

    private static readonly Regex TokenPastePattern = new(
        @"(?<left>[^\s#]+)\s*##\s*(?<right>[^\s#]+)",
        RegexOptions.Compiled);

    private static string ExpandVaOpt(string body, bool hasVariadicTokens)
    {
        const string marker = "__VA_OPT__";
        var output = new StringBuilder(body.Length);
        var index = 0;

        while (index < body.Length)
        {
            if (StartsPrefixedLiteral(body, index) || IsLiteralStart(body, index))
            {
                AppendLiteral(body, ref index, output);
                continue;
            }

            if (!body.AsSpan(index).StartsWith(marker, StringComparison.Ordinal)
                || (index > 0 && IsIdentifierPart(body[index - 1]))
                || (index + marker.Length < body.Length && IsIdentifierPart(body[index + marker.Length])))
            {
                output.Append(body[index++]);
                continue;
            }

            var open = index + marker.Length;
            while (open < body.Length && char.IsWhiteSpace(body[open]))
            {
                open++;
            }

            if (open >= body.Length || body[open] != '(')
            {
                output.Append(body[index++]);
                continue;
            }

            if (!TryReadBalancedText(body, open, out var contents, out var end))
            {
                output.Append(body[index++]);
                continue;
            }

            if (hasVariadicTokens)
            {
                output.Append(contents);
            }
            else
            {
                while (output.Length > 0 && char.IsWhiteSpace(output[^1]))
                {
                    output.Length--;
                }

                if (output.Length > 0
                    && end < body.Length
                    && !char.IsWhiteSpace(body[end]))
                {
                    output.Append(' ');
                }
            }

            index = end;
        }

        return output.ToString();
    }

    private static bool TryReadBalancedText(string text, int open, out string contents, out int end)
    {
        contents = string.Empty;
        end = open;
        var depth = 0;
        var start = open + 1;

        for (var index = open; index < text.Length; index++)
        {
            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                index = SkipLiteral(text, index) - 1;
                continue;
            }

            if (text[index] == '(')
            {
                depth++;
            }
            else if (text[index] == ')')
            {
                depth--;
                if (depth == 0)
                {
                    contents = text.Substring(start, index - start);
                    end = index + 1;
                    return true;
                }
            }
        }

        return false;
    }

    private static string ReplaceMacroParameters(string text, IReadOnlyDictionary<string, (string Raw, string Expanded)> substitutions)
    {
        var output = new StringBuilder(text.Length);
        var index = 0;

        while (index < text.Length)
        {
            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                AppendLiteral(text, ref index, output);

                continue;
            }

            if (!IsIdentifierStart(text[index]))
            {
                output.Append(text[index++]);
                continue;
            }

            var start = index++;
            while (index < text.Length && IsIdentifierPart(text[index]))
            {
                index++;
            }

            var name = text[start..index];
            output.Append(substitutions.TryGetValue(name, out var substitution)
                ? substitution.Expanded
                : name);
        }

        return output.ToString();
    }

    private static string ReplaceOutsideLiterals(string text, Regex pattern, MatchEvaluator evaluator)
    {
        var output = new StringBuilder(text.Length);
        var segmentStart = 0;
        var index = 0;

        while (index < text.Length)
        {
            if (!StartsPrefixedLiteral(text, index) && !IsLiteralStart(text, index))
            {
                index++;
                continue;
            }

            if (index > segmentStart)
            {
                output.Append(pattern.Replace(text[segmentStart..index], evaluator));
            }

            AppendLiteral(text, ref index, output);
            segmentStart = index;
        }

        if (segmentStart < text.Length)
        {
            output.Append(pattern.Replace(text[segmentStart..], evaluator));
        }

        return output.ToString();
    }

    private static void AppendLiteral(string text, ref int index, StringBuilder output)
    {
        var quoteIndex = LiteralQuoteIndex(text, index);
        output.Append(text, index, quoteIndex - index);
        index = quoteIndex;
        var quote = text[index++];
        output.Append(quote);

        while (index < text.Length)
        {
            var character = text[index++];
            output.Append(character);

            if (character == '\\' && index < text.Length)
            {
                output.Append(text[index++]);
            }
            else if (character == quote)
            {
                break;
            }
        }
    }

    private static string Stringify(string text)
    {
        var normalizedBuilder = new StringBuilder(text.Length);
        var pendingWhitespace = false;
        var hasToken = false;
        var index = 0;

        while (index < text.Length)
        {
            if (char.IsWhiteSpace(text[index]))
            {
                pendingWhitespace = hasToken;
                index++;
                continue;
            }

            if (pendingWhitespace)
            {
                normalizedBuilder.Append(' ');
                pendingWhitespace = false;
            }

            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                AppendLiteral(text, ref index, normalizedBuilder);
            }
            else
            {
                normalizedBuilder.Append(text[index++]);
            }

            hasToken = true;
        }

        var normalized = normalizedBuilder.ToString();
        return "\"" + normalized.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }

    private static string Charize(string text)
    {
        var normalized = text.Trim();
        return "'" + normalized.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
    }
}
