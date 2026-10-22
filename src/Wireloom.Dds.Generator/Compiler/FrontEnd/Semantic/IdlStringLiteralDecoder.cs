using System.Globalization;
using System.Text;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Decodes adjacent IDL string tokens before concatenating their values.</summary>
internal static class IdlStringLiteralDecoder
{
    /// <summary>Decodes literal tokens without allowing escapes to cross token boundaries.</summary>
    internal static string Decode(string expression, bool isWide, IdlInput input, int offset)
    {
        var result = new StringBuilder();
        var position = 0;
        while (position < expression.Length)
        {
            while (position < expression.Length && char.IsWhiteSpace(expression[position]))
            {
                position++;
            }

            if (position == expression.Length)
            {
                break;
            }

            var start = position;
            if (expression[position] == 'L')
            {
                position++;
            }

            if (position == expression.Length || expression[position] != '"')
            {
                throw new IdlException(input, offset, "String default must be a quoted string literal.");
            }

            position++;
            var closed = false;
            while (position < expression.Length)
            {
                var character = expression[position++];
                if (character == '\\' && position < expression.Length)
                {
                    position++;
                }
                else if (character == '"')
                {
                    closed = true;
                    break;
                }
            }

            if (!closed)
            {
                throw new IdlException(input, offset, "String default must be a quoted string literal.");
            }

            result.Append(DecodeToken(expression[start..position], isWide, input, offset));
        }

        if (position == 0)
        {
            throw new IdlException(input, offset, "String default must be a quoted string literal.");
        }

        return result.ToString();
    }

    private static string DecodeToken(string expression, bool isWide, IdlInput input, int offset)
    {
        var literalOffset = expression.StartsWith("L\"", StringComparison.Ordinal) ? 1 : 0;
        if (((literalOffset == 1) != isWide)
            || expression.Length < literalOffset + 2
            || expression[literalOffset] != '"'
            || expression[^1] != '"')
        {
            throw new IdlException(input, offset, "String default must be a quoted string literal.");
        }

        var value = new StringBuilder(expression.Length - literalOffset - 2);
        for (var index = literalOffset + 1; index < expression.Length - 1; index++)
        {
            var character = expression[index];
            if (character != '\\')
            {
                if (character == '\0')
                {
                    throw new IdlException(input, offset, "String defaults cannot contain the NUL character.");
                }

                value.Append(character);
                continue;
            }

            var escape = expression[++index];
            switch (escape)
            {
                case 'a': value.Append('\a'); break;
                case 'b': value.Append('\b'); break;
                case 'f': value.Append('\f'); break;
                case 'n': value.Append('\n'); break;
                case 'r': value.Append('\r'); break;
                case 't': value.Append('\t'); break;
                case 'v': value.Append('\v'); break;
                case '\\': value.Append('\\'); break;
                case '\'': value.Append('\''); break;
                case '"': value.Append('"'); break;
                case '?': value.Append('?'); break;
                case 'x':
                case 'u':
                    if (escape == 'u' && !isWide)
                    {
                        throw new IdlException(input, offset, "Unicode escape \\u is only valid in a wide string default.");
                    }

                    var maximumDigits = escape == 'x' ? 2 : 4;
                    var firstDigit = index + 1;
                    var escapeDigits = 0;
                    while (firstDigit + escapeDigits < expression.Length - 1
                        && escapeDigits < maximumDigits
                        && Uri.IsHexDigit(expression[firstDigit + escapeDigits]))
                    {
                        escapeDigits++;
                    }

                    if (escapeDigits == 0
                        || !int.TryParse(expression.Substring(firstDigit, escapeDigits), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var escapeValue))
                    {
                        throw new IdlException(input, offset, $"Invalid hexadecimal escape in string default: \\{escape}");
                    }

                    if (escapeValue == 0)
                    {
                        throw new IdlException(input, offset, "String defaults cannot contain the NUL character.");
                    }

                    index += escapeDigits;
                    value.Append((char)escapeValue);
                    break;
                default:
                    if (escape is >= '0' and <= '7')
                    {
                        var octalStart = index;
                        while (index + 1 < expression.Length - 1 && index - octalStart < 2 && expression[index + 1] is >= '0' and <= '7')
                        {
                            index++;
                        }

                        var octalValue = Convert.ToInt32(expression.Substring(octalStart, index - octalStart + 1), 8);
                        if (octalValue == 0)
                        {
                            throw new IdlException(input, offset, "String defaults cannot contain the NUL character.");
                        }

                        value.Append((char)octalValue);
                        break;
                    }

                    throw new IdlException(input, offset, $"Unsupported escape sequence in string default: \\{escape}");
            }
        }

        return value.ToString();
    }

}
