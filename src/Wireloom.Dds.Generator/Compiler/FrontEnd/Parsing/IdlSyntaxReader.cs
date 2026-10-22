using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Reads delimited IDL syntax without treating quoted literal contents as syntax.</summary>
internal static class IdlSyntaxReader
{
    /// <summary>Reads a complete annotation name and optional balanced argument list without interpreting its meaning.</summary>
    internal static bool TryReadAnnotationApplication(string text, out string name, out int length)
    {
        name = string.Empty;
        length = 0;
        if (text.Length == 0 || text[0] != '@')
        {
            return false;
        }

        var position = 1;
        while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or ':'))
        {
            position++;
        }

        name = text[1..position];
        if (!IdlScopedName.IsValid(name))
        {
            return false;
        }

        while (position < text.Length && char.IsWhiteSpace(text[position]))
        {
            position++;
        }

        if (position < text.Length && text[position] == '(')
        {
            var closing = FindClosingDelimiter(text, position, '(', ')');
            if (closing < 0)
            {
                return false;
            }

            position = closing + 1;
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }
        }

        length = position;
        return true;
    }

    /// <summary>Finds a matching delimiter while preserving quoted literals and nested delimiters.</summary>
    internal static int FindClosingDelimiter(string text, int openingOffset, char opening, char closing)
    {
        var depth = 0;
        for (var index = openingOffset; index < text.Length; index++)
        {
            if (IsLiteralStart(text, index))
            {
                var quote = text[index++];
                while (index < text.Length)
                {
                    var character = text[index++];
                    if (character == '\\' && index < text.Length)
                    {
                        index++;
                    }
                    else if (character == quote)
                    {
                        break;
                    }
                }

                index--;
            }
            else if (text[index] == opening)
            {
                depth++;
            }
            else if (text[index] == closing && --depth == 0)
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>Reads a positional or named value argument and the complete annotation length.</summary>
    internal static bool TryReadAnnotationValue(string text, string name, out string value, out int length)
    {
        value = string.Empty;
        length = 0;
        var prefix = $"@{name}";
        if (!text.StartsWith(prefix, StringComparison.Ordinal))
        {
            return false;
        }

        var opening = prefix.Length;
        while (opening < text.Length && char.IsWhiteSpace(text[opening]))
        {
            opening++;
        }

        if (opening >= text.Length || text[opening] != '(')
        {
            return false;
        }

        var closing = FindClosingDelimiter(text, opening, '(', ')');
        if (closing < 0)
        {
            return false;
        }

        value = text[(opening + 1)..closing].Trim();
        if (value.StartsWith("value", StringComparison.Ordinal))
        {
            var equals = 5;
            while (equals < value.Length && char.IsWhiteSpace(value[equals]))
            {
                equals++;
            }

            if (equals < value.Length && value[equals] == '=')
            {
                value = value[(equals + 1)..].Trim();
            }
        }

        length = closing + 1;
        while (length < text.Length && char.IsWhiteSpace(text[length]))
        {
            length++;
        }

        return true;
    }

    private static bool IsLiteralStart(string text, int index)
    {
        if (text[index] == '"')
        {
            return true;
        }

        if (text[index] != '\'')
        {
            return false;
        }

        var hasPrefix = index > 0 && (text[index - 1] is 'L' or 'u' or 'U'
            || index > 1 && text[index - 1] == '8' && text[index - 2] == 'u');
        var isIdentifierApostrophe = index > 0 && index + 1 < text.Length
            && char.IsLetterOrDigit(text[index - 1]) && char.IsLetterOrDigit(text[index + 1]);
        return hasPrefix || !isIdentifierApostrophe;
    }
}
