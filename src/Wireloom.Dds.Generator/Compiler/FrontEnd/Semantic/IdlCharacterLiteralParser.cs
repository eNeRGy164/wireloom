using System.Globalization;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Parses the character literal forms shared by IDL defaults and union labels.</summary>
internal static class IdlCharacterLiteralParser
{
    /// <summary>Attempts to parse a one-character literal, optionally allowing the IDL wide prefix.</summary>
    internal static bool TryParse(string literal, bool allowWideLiteral, out int value)
    {
        value = 0;
        var isWideLiteral = literal.StartsWith("L'", StringComparison.Ordinal);
        if (isWideLiteral && !allowWideLiteral)
        {
            return false;
        }

        var contentStart = isWideLiteral ? 2 : 1;
        if (literal.Length < contentStart + 2
            || literal[contentStart - 1] != '\''
            || literal[^1] != '\'')
        {
            return false;
        }

        var content = literal[contentStart..^1];
        var character = content switch
        {
            "\\0" => '\0',
            "\\a" => '\a',
            "\\n" => '\n',
            "\\r" => '\r',
            "\\t" => '\t',
            "\\v" => '\v',
            "\\b" => '\b',
            "\\f" => '\f',
            "\\\\" => '\\',
            "\\?" => '?',
            "\\'" => '\'',
            "\\\"" => '"',
            _ when content.Length == 1 => content[0],
            _ => '\0'
        };
        var decodedNumericEscape = false;

        if (content.StartsWith("\\x", StringComparison.Ordinal)
            && content.Length is >= 3 and <= 4
            && content[2..].All(Uri.IsHexDigit)
            && int.TryParse(content[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexadecimalValue)
            && hexadecimalValue <= char.MaxValue)
        {
            character = (char)hexadecimalValue;
            decodedNumericEscape = true;
        }

        if (allowWideLiteral
            && content.StartsWith("\\u", StringComparison.Ordinal)
            && content.Length is >= 3 and <= 6
            && content[2..].All(Uri.IsHexDigit)
            && int.TryParse(content[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var unicodeValue)
            && unicodeValue <= char.MaxValue)
        {
            character = (char)unicodeValue;
            decodedNumericEscape = true;
        }

        if (content.Length is >= 2 and <= 4
            && content[0] == '\\'
            && content[1..].All(digit => digit is >= '0' and <= '7'))
        {
            var octalValue = 0;
            foreach (var digit in content[1..])
            {
                octalValue = (octalValue * 8) + (digit - '0');
            }

            if (octalValue <= char.MaxValue)
            {
                character = (char)octalValue;
                decodedNumericEscape = true;
            }
        }

        if (content.Length != 1 && character == '\0' && content != "\\0" && !decodedNumericEscape)
        {
            return false;
        }

        value = character;

        return true;
    }
}
