using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Performs literal-aware macro argument and replacement-token transformations.</summary>
internal sealed class PreprocessorMacroTokenService
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

    /// <summary>Transforms a macro replacement body using parameter substitutions.</summary>
    internal string TransformReplacement(
        string body,
        bool variadic,
        bool hasVariadicTokens,
        IReadOnlyDictionary<string, (string Raw, string Expanded)> substitutions)
    {
        var replacement = variadic ? ExpandVaOpt(body, hasVariadicTokens) : body;

        replacement = ReplaceOutsideLiterals(replacement, CharizePattern, match =>
            substitutions.TryGetValue(match.Groups["name"].Value, out var charSubstitution)
                ? Charize(charSubstitution.Raw)
                : match.Value);

        replacement = ReplaceOutsideLiterals(replacement, StringifyPattern, match =>
            substitutions.TryGetValue(match.Groups["name"].Value, out var substitution)
                ? Stringify(substitution.Raw)
                : match.Value);

        while (true)
        {
            var pasted = ReplaceOutsideLiterals(replacement, TokenPastePattern, match =>
            {
                var left = substitutions.TryGetValue(match.Groups["left"].Value, out var leftValue)
                    ? leftValue.Raw
                    : match.Groups["left"].Value;
                var right = substitutions.TryGetValue(match.Groups["right"].Value, out var rightValue)
                    ? rightValue.Raw
                    : match.Groups["right"].Value;
                return left + right;
            });

            if (string.Equals(pasted, replacement, StringComparison.Ordinal))
            {
                break;
            }

            replacement = pasted;
        }

        return ReplaceMacroParameters(replacement, substitutions);
    }

    /// <summary>Reads balanced parenthesized text from a macro invocation.</summary>
    internal bool TryReadBalancedText(string text, int open, out string contents, out int end)
    {
        contents = string.Empty;
        end = open;
        var depth = 0;
        var start = open + 1;

        for (var index = open; index < text.Length; index++)
        {
            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                index = PreprocessorLexicalService.SkipLiteral(text, index) - 1;
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

    /// <summary>Reads macro arguments while respecting nested delimiters and literals.</summary>
    internal bool TryReadArguments(string text, int open, out List<string> arguments, out int end)
    {
        arguments = [];
        end = open;
        var depth = 0;
        var start = open + 1;

        for (var index = open; index < text.Length; index++)
        {
            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                index = PreprocessorLexicalService.SkipLiteral(text, index) - 1;
                continue;
            }

            switch (text[index])
            {
                case '(':
                    depth++;
                    break;
                case ')':
                    depth--;
                    if (depth == 0)
                    {
                        var argument = text.Substring(start, index - start).Trim();
                        if (argument.Length != 0 || arguments.Count != 0)
                        {
                            arguments.Add(argument);
                        }

                        end = index + 1;
                        return true;
                    }

                    break;
                case ',' when depth == 1:
                    arguments.Add(text.Substring(start, index - start).Trim());
                    start = index + 1;
                    break;
            }
        }

        return false;
    }

    /// <summary>Converts replacement text into a C preprocessor string literal.</summary>
    internal string Stringify(string text)
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

            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
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

    private string ExpandVaOpt(string body, bool hasVariadicTokens)
    {
        const string marker = "__VA_OPT__";
        var output = new StringBuilder(body.Length);
        var index = 0;
        while (index < body.Length)
        {
            if (PreprocessorLexicalService.StartsPrefixedLiteral(body, index)
                || PreprocessorLexicalService.IsLiteralStart(body, index))
            {
                AppendLiteral(body, ref index, output);
                continue;
            }

            if (!body.AsSpan(index).StartsWith(marker, StringComparison.Ordinal)
                || (index > 0 && PreprocessorLexicalService.IsIdentifierPart(body[index - 1]))
                || (index + marker.Length < body.Length
                    && PreprocessorLexicalService.IsIdentifierPart(body[index + marker.Length])))
            {
                output.Append(body[index++]);
                continue;
            }

            var open = index + marker.Length;
            while (open < body.Length && char.IsWhiteSpace(body[open]))
            {
                open++;
            }

            if (open >= body.Length || body[open] != '('
                || !TryReadBalancedText(body, open, out var contents, out var end))
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

                if (output.Length > 0 && end < body.Length && !char.IsWhiteSpace(body[end]))
                {
                    output.Append(' ');
                }
            }

            index = end;
        }

        return output.ToString();
    }

    private static string ReplaceMacroParameters(
        string text,
        IReadOnlyDictionary<string, (string Raw, string Expanded)> substitutions)
    {
        var output = new StringBuilder(text.Length);
        var index = 0;
        while (index < text.Length)
        {
            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                AppendLiteral(text, ref index, output);
                continue;
            }

            if (!PreprocessorLexicalService.IsIdentifierStart(text[index]))
            {
                output.Append(text[index++]);
                continue;
            }

            var start = index++;
            while (index < text.Length && PreprocessorLexicalService.IsIdentifierPart(text[index]))
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
            if (!PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                && !PreprocessorLexicalService.IsLiteralStart(text, index))
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
        var quoteIndex = PreprocessorLexicalService.LiteralQuoteIndex(text, index);
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

    private static string Charize(string text)
    {
        var normalized = text.Trim();
        return "'" + normalized.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
    }
}
