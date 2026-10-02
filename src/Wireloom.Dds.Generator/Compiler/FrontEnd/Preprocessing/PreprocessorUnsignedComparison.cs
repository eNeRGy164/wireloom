using System.Globalization;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Handles unsigned literal comparisons not representable by the signed expression subset.</summary>
internal static class PreprocessorUnsignedComparison
{
    /// <summary>Evaluates an unsigned comparison when the expression uses an unsigned literal.</summary>
    /// <summary>Evaluates an unsigned comparison expression when its syntax is supported.</summary>
    internal static (bool Matched, bool Result) Evaluate(string expression)
    {
        var match = System.Text.RegularExpressions.Regex.Match(expression, @"^\s*(?<left>[^<>=!]+?)\s*(?<operator>==|!=|<=|>=|<|>)\s*(?<right>[^<>=!]+?)\s*$");
        if (!match.Success)
        {
            return (false, false);
        }

        var leftText = match.Groups["left"].Value.Trim();
        var rightText = match.Groups["right"].Value.Trim();
        if (!leftText.EndsWith("ULL", StringComparison.OrdinalIgnoreCase) && !rightText.EndsWith("ULL", StringComparison.OrdinalIgnoreCase))
        {
            return (false, false);
        }

        if (!TryParse(leftText, out var left) || !TryParse(rightText, out var right))
        {
            return (false, false);
        }
        var result = match.Groups["operator"].Value switch
        {
            "==" => left == right,
            "!=" => left != right,
            "<" => left < right,
            ">" => left > right,
            "<=" => left <= right,
            ">=" => left >= right,
            _ => false
        };
        return (true, result);
    }

    private static bool TryParse(string text, out ulong value)
    {
        text = text.Trim();
        if (text.StartsWith("-", StringComparison.Ordinal))
        {
            if (!TryParseInteger(text[1..], out var positive))
            {
                value = 0;
                return false;
            }
            value = unchecked((ulong)-positive);
            return true;
        }
        if (text.StartsWith("+", StringComparison.Ordinal))
        {
            text = text[1..].TrimStart();
        }

        if (!TryParseInteger(text, out var integer))
        {
            value = 0;
            return false;
        }
        value = unchecked((ulong)integer);
        return true;
    }

    private static bool TryParseInteger(string text, out long value)
    {
        text = text.Trim();
        var suffixStart = text.Length;
        while (suffixStart > 0 && text[suffixStart - 1] is 'u' or 'U' or 'l' or 'L') suffixStart--;
        var suffix = text[suffixStart..].ToLowerInvariant();
        if (suffix is not ("" or "u" or "l" or "ll" or "ul" or "lu" or "ull" or "llu"))
        {
            value = 0;
            return false;
        }

        var number = text[..suffixStart];
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
            if (separator == 0 || separator == number.Length - 1 || !IsDigit(number[separator - 1], radix) || !IsDigit(number[separator + 1], radix))
            {
                value = 0;
                return false;
            }
        }

        var unsigned = suffix.Contains('u');
        number = number.Replace("'", string.Empty);

        if (number.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
        {
            return TryBase(number[2..], 2, unsigned, out value);
        }

        if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && ulong.TryParse(number[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hex))
        {
            value = hex <= long.MaxValue || unsigned ? unchecked((long)hex) : 0;
            return hex <= long.MaxValue || unsigned;
        }

        if (number.Length > 1 && number[0] == '0')
        {
            return TryBase(number[1..], 8, unsigned, out value);
        }

        if (unsigned && ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unsignedValue))
        {
            value = unchecked((long)unsignedValue);
            return true;
        }

        return long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    private static bool IsDigit(char character, int radix) => radix switch
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
}
