using System.Globalization;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Parses the integer expression grammar used by conditional directives.</summary>
internal sealed class PreprocessorConditionalExpressionParser(string text)
{
    private int position;
    private int nestingDepth;

    /// <summary>Evaluates the conditional expression supplied to the parser.</summary>
    /// <summary>Evaluates the conditional expression.</summary>
    internal long Evaluate()
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw Invalid("expected an expression");
        }

        var value = ParseConditional(true);
        SkipWhiteSpace();
        if (position != text.Length)
        {
            throw Invalid($"unexpected token '{text[position]}'");
        }

        return value;
    }

    private long ParseConditional(bool evaluate)
    {
        EnterNesting();
        try
        {
            var condition = ParseLogicalOr(evaluate);
            if (!Take("?"))
            {
                return condition;
            }

            var whenTrue = ParseConditional(evaluate && condition != 0);
            if (!Take(":"))
            {
                throw Invalid("expected ':' in conditional expression");
            }

            var whenFalse = ParseConditional(evaluate && condition == 0);
            return evaluate ? condition != 0 ? whenTrue : whenFalse : 0;
        }
        finally
        {
            nestingDepth--;
        }
    }

    private long ParseLogicalOr(bool evaluate)
    {
        var value = ParseLogicalAnd(evaluate);
        while (Take("||"))
        {
            var right = ParseLogicalAnd(evaluate && value == 0);
            if (evaluate)
            {
                value = value != 0 || right != 0 ? 1 : 0;
            }
        }

        return value;
    }

    private long ParseLogicalAnd(bool evaluate)
    {
        var value = ParseBitwiseOr(evaluate);
        while (Take("&&"))
        {
            var right = ParseBitwiseOr(evaluate && value != 0);
            if (evaluate)
            {
                value = value != 0 && right != 0 ? 1 : 0;
            }
        }

        return value;
    }

    private long ParseBitwiseOr(bool evaluate)
    {
        var value = ParseBitwiseXor(evaluate);
        while (TakeSingle('|'))
        {
            var right = ParseBitwiseXor(evaluate);
            if (evaluate)
            {
                value |= right;
            }
        }

        return value;
    }

    private long ParseBitwiseXor(bool evaluate)
    {
        var value = ParseBitwiseAnd(evaluate);
        while (Take("^"))
        {
            var right = ParseBitwiseAnd(evaluate);
            if (evaluate)
            {
                value ^= right;
            }
        }

        return value;
    }

    private long ParseBitwiseAnd(bool evaluate)
    {
        var value = ParseEquality(evaluate);
        while (TakeSingle('&'))
        {
            var right = ParseEquality(evaluate);
            if (evaluate)
            {
                value &= right;
            }
        }

        return value;
    }

    private long ParseEquality(bool evaluate)
    {
        var value = ParseRelational(evaluate);
        while (true)
        {
            if (Take("=="))
            {
                var right = ParseRelational(evaluate);
                if (evaluate)
                {
                    value = value == right ? 1 : 0;
                }
            }
            else if (Take("!="))
            {
                var right = ParseRelational(evaluate);
                if (evaluate)
                {
                    value = value != right ? 1 : 0;
                }
            }
            else
            {
                return value;
            }
        }
    }

    private long ParseRelational(bool evaluate)
    {
        var value = ParseShift(evaluate);
        while (true)
        {
            if (Take("<="))
            {
                var right = ParseShift(evaluate);
                if (evaluate)
                {
                    value = value <= right ? 1 : 0;
                }
            }
            else if (Take(">="))
            {
                var right = ParseShift(evaluate);
                if (evaluate)
                {
                    value = value >= right ? 1 : 0;
                }
            }
            else if (Take("<"))
            {
                var right = ParseShift(evaluate);
                if (evaluate)
                {
                    value = value < right ? 1 : 0;
                }
            }
            else if (Take(">"))
            {
                var right = ParseShift(evaluate);
                if (evaluate)
                {
                    value = value > right ? 1 : 0;
                }
            }
            else
            {
                return value;
            }
        }
    }

    private long ParseShift(bool evaluate)
    {
        var value = ParseAdditive(evaluate);
        while (true)
        {
            if (Take("<<"))
            {
                var right = ParseAdditive(evaluate);
                if (evaluate && right is < 0 or > 63)
                {
                    throw Invalid("shift count must be between 0 and 63");
                }

                if (evaluate)
                {
                    value <<= (int)right;
                }
            }
            else if (Take(">>"))
            {
                var right = ParseAdditive(evaluate);
                if (evaluate && right is < 0 or > 63)
                {
                    throw Invalid("shift count must be between 0 and 63");
                }

                if (evaluate)
                {
                    value >>= (int)right;
                }
            }
            else
            {
                return value;
            }
        }
    }

    private long ParseAdditive(bool evaluate)
    {
        var value = ParseMultiplicative(evaluate);
        while (true)
        {
            if (Take("+"))
            {
                var right = ParseMultiplicative(evaluate);
                if (evaluate)
                {
                    value += right;
                }
            }
            else if (Take("-"))
            {
                var right = ParseMultiplicative(evaluate);
                if (evaluate)
                {
                    value -= right;
                }
            }
            else
            {
                return value;
            }
        }
    }

    private long ParseMultiplicative(bool evaluate)
    {
        var value = ParseUnary(evaluate);
        while (true)
        {
            if (Take("*"))
            {
                var right = ParseUnary(evaluate);
                if (evaluate)
                {
                    value *= right;
                }
            }
            else if (Take("/"))
            {
                var right = ParseUnary(evaluate);
                if (evaluate && right == 0)
                {
                    throw Invalid("division by zero");
                }

                if (evaluate)
                {
                    value /= right;
                }
            }
            else if (Take("%"))
            {
                var right = ParseUnary(evaluate);
                if (evaluate && right == 0)
                {
                    throw Invalid("remainder by zero");
                }

                if (evaluate)
                {
                    value %= right;
                }
            }
            else
            {
                return value;
            }
        }
    }

    private long ParseUnary(bool evaluate)
    {
        EnterNesting();
        try
        {
            if (Take("!"))
            {
                var value = ParseUnary(evaluate);
                return evaluate && value == 0 ? 1 : 0;
            }

            if (Take("~"))
            {
                var value = ParseUnary(evaluate);
                return evaluate ? ~value : 0;
            }

            if (Take("+"))
            {
                return ParseUnary(evaluate);
            }

            if (Take("-"))
            {
                var value = ParseUnary(evaluate);
                return evaluate ? -value : 0;
            }

            return ParsePrimary(evaluate);
        }
        finally
        {
            nestingDepth--;
        }
    }

    private long ParsePrimary(bool evaluate)
    {
        SkipWhiteSpace();
        if (position < text.Length && text[position] is '\'' or '"')
        {
            return ParseCharacter(evaluate);
        }

        if (TryTakeCharacterEncodingPrefix())
        {
            return ParseCharacter(evaluate);
        }

        if (Take("("))
        {
            var value = ParseConditional(evaluate);
            if (!Take(")"))
            {
                throw Invalid("expected ')' ");
            }

            return value;
        }

        var start = position;
        while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or '\''))
        {
            position++;
        }

        if (start == position)
        {
            throw Invalid("expected an integer or identifier");
        }

        var token = text[start..position];
        if (PreprocessorLexicalService.IsIdentifier(token))
        {
            return 0;
        }

        if (!TryParseInteger(token, out var integer))
        {
            throw Invalid($"invalid integer '{token}'");
        }

        return evaluate ? integer : 0;
    }

    private long ParseCharacter(bool evaluate)
    {
        var quote = text[position++];
        if (quote != '\'')
        {
            throw Invalid("expected a character constant");
        }

        if (position >= text.Length)
        {
            throw Invalid("unterminated character constant");
        }

        var value = text[position++] == '\\' ? ParseCharacterEscape() : text[position - 1];
        if (position >= text.Length || text[position++] != '\'')
        {
            throw Invalid("expected closing quote for character constant");
        }

        return evaluate ? value : 0;
    }

    private long ParseCharacterEscape()
    {
        if (position >= text.Length)
        {
            throw Invalid("unterminated character escape");
        }
        var escape = text[position++];
        return escape switch
        {
            '\\' => '\\', '\'' => '\'', '"' => '"', '0' => '\0', 'a' => '\a', 'b' => '\b', 'f' => '\f',
            'n' => '\n', 'r' => '\r', 't' => '\t', 'v' => '\v',
            >= '1' and <= '7' => ParseOctal(escape),
            'x' => ParseHex(),
            _ => throw Invalid($"unsupported character escape '\\{escape}'")
        };
    }

    private int ParseOctal(char first)
    {
        var value = first - '0';
        var digits = 1;
        while (digits < 3 && position < text.Length && text[position] is >= '0' and <= '7')
        {
            value = value * 8 + text[position++] - '0';
            digits++;
        }
        return value;
    }

    private int ParseHex()
    {
        var start = position;
        while (position < text.Length && Uri.IsHexDigit(text[position]))
        {
            position++;
        }

        if (start == position || !int.TryParse(text[start..position], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            throw Invalid("invalid hexadecimal character escape");
        }

        return value;
    }

    private bool TryTakeCharacterEncodingPrefix()
    {
        if (position + 1 < text.Length && text[position] is 'L' or 'u' or 'U' && text[position + 1] == '\'')
        {
            position++;
            return true;
        }
        if (position + 2 < text.Length && text[position] == 'u' && text[position + 1] == '8' && text[position + 2] == '\'')
        {
            position += 2;
            return true;
        }
        return false;
    }

    private void EnterNesting()
    {
        if (nestingDepth >= PreprocessorLimits.MaximumExpressionNesting)
        {
            throw Invalid($"expression nesting exceeds the {PreprocessorLimits.MaximumExpressionNesting}-level limit");
        }
        nestingDepth++;
    }

    private bool Take(string token)
    {
        SkipWhiteSpace();
        if (!text.AsSpan(position).StartsWith(token, StringComparison.Ordinal))
        {
            return false;
        }
        position += token.Length;
        return true;
    }

    private bool TakeSingle(char token)
    {
        SkipWhiteSpace();
        if (position >= text.Length || text[position] != token || position + 1 < text.Length && text[position + 1] == token)
        {
            return false;
        }
        position++;
        return true;
    }

    private void SkipWhiteSpace()
    {
        while (position < text.Length && char.IsWhiteSpace(text[position]))
        {
            position++;
        }
    }

    private static bool TryParseInteger(string value, out long result)
    {
        value = value.Trim();
        var suffixStart = value.Length;
        while (suffixStart > 0 && value[suffixStart - 1] is 'u' or 'U' or 'l' or 'L') suffixStart--;
        var suffix = value[suffixStart..].ToLowerInvariant();
        if (suffix is not ("" or "u" or "l" or "ll" or "ul" or "lu" or "ull" or "llu"))
        {
            result = 0;
            return false;
        }

        var number = value[..suffixStart];
        int radix;
        if (number.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
        {
            radix = 2;
        }
        else if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            radix = 16;
        }
        else if (number.Length > 1 && number[0] == '0')
        {
            radix = 8;
        }
        else
        {
            radix = 10;
        }

        for (var separator = number.IndexOf('\''); separator >= 0; separator = number.IndexOf('\'', separator + 1))
        {
            if (separator == 0 || separator == number.Length - 1 || !IsDigitForRadix(number[separator - 1], radix) || !IsDigitForRadix(number[separator + 1], radix))
            {
                result = 0;
                return false;
            }
        }

        number = number.Replace("'", string.Empty);
        var unsigned = suffix.Contains('u');

        if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && ulong.TryParse(number[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hex))
        {
            result = unchecked((long)hex);
            return unsigned || hex <= long.MaxValue;
        }

        if (number.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
        {
            return TryBase(number[2..], 2, unsigned, out result);
        }

        if (number.Length > 1 && number[0] == '0')
        {
            return TryBase(number[1..], 8, unsigned, out result);
        }

        if (unsigned && ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unsignedValue))
        {
            result = unchecked((long)unsignedValue);
            return true;
        }

        return long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    private static bool IsDigitForRadix(char character, int radix) => radix switch
    {
        2 => character is '0' or '1',
        8 => character is >= '0' and <= '7',
        10 => character is >= '0' and <= '9',
        16 => character is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F',
        _ => false
    };

    private static bool TryBase(string text, int radix, bool allowUnsigned, out long value)
    {
        ulong parsed = 0;
        if (text.Length == 0)
        {
            value = 0;
            return false;
        }

        foreach (var character in text)
        {
            var digit = character switch
            {
                >= '0' and <= '9' => character - '0',
                >= 'a' and <= 'f' => character - 'a' + 10,
                >= 'A' and <= 'F' => character - 'A' + 10,
                _ => -1
            };

            if (digit < 0 || digit >= radix || parsed > (ulong.MaxValue - (ulong)digit) / (ulong)radix)
            {
                value = 0;
                return false;
            }

            parsed = parsed * (ulong)radix + (ulong)digit;
        }

        if (!allowUnsigned && parsed > long.MaxValue)
        {
            value = 0;
            return false;
        }

        value = unchecked((long)parsed);
        return true;
    }

    private InvalidOperationException Invalid(string message) => new($"{message} at column {position + 1}.");
}
