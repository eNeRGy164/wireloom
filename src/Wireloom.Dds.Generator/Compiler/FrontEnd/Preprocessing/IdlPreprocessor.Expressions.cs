using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private static readonly Regex UnsignedComparisonPattern = new(
        @"^\s*(?<left>[^<>=!]+?)\s*(?<operator>==|!=|<=|>=|<|>)\s*(?<right>[^<>=!]+?)\s*$",
        RegexOptions.Compiled);

    private bool EvaluateCondition(string text, IdlInput input, int sourceOffset, int diagnosticOffset)
    {
        text = ReplaceDefinedOperators(text, input, diagnosticOffset);

        var expression = Expand(text, sourceOffset, sourceOffsetMap).Trim();
        try
        {
            if (expression.Contains("++", StringComparison.Ordinal)
                || expression.Contains("--", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("increment and decrement operators are not valid in preprocessor conditions");
            }

            if (TryEvaluateUnsignedComparison(expression, out var unsignedResult))
            {
                return unsignedResult;
            }

            return new ConditionalExpression(expression).Evaluate() != 0;
        }
        catch (InvalidOperationException exception)
        {
            throw new IdlException(input, diagnosticOffset, $"Invalid preprocessor condition: {exception.Message}");
        }
        catch (ArithmeticException exception)
        {
            throw new IdlException(input, diagnosticOffset, $"Invalid preprocessor condition: {exception.Message}");
        }
    }

    private string ReplaceDefinedOperators(string text, IdlInput input, int offset)
    {
        var output = new StringBuilder(text.Length);
        var copiedThrough = 0;
        var scanIndex = 0;

        while (scanIndex < text.Length)
        {
            if (StartsPrefixedLiteral(text, scanIndex) || IsLiteralStart(text, scanIndex))
            {
                scanIndex = SkipLiteral(text, scanIndex);
                continue;
            }

            if (!IsDefinedOperatorAt(text, scanIndex))
            {
                scanIndex++;
                continue;
            }

            output.Append(text, copiedThrough, scanIndex - copiedThrough);

            var position = scanIndex + "defined".Length;
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }

            string name;
            if (position < text.Length && text[position] == '(')
            {
                position++;
                while (position < text.Length && char.IsWhiteSpace(text[position]))
                {
                    position++;
                }

                var nameStart = position;
                if (position >= text.Length || !IsIdentifierStart(text[position]))
                {
                    throw InvalidDefinedOperator(input, offset);
                }

                while (position < text.Length && IsIdentifierPart(text[position]))
                {
                    position++;
                }

                name = text[nameStart..position];
                while (position < text.Length && char.IsWhiteSpace(text[position]))
                {
                    position++;
                }

                if (position >= text.Length || text[position] != ')')
                {
                    throw InvalidDefinedOperator(input, offset);
                }

                position++;
            }
            else
            {
                var nameStart = position;
                if (position >= text.Length || !IsIdentifierStart(text[position]))
                {
                    throw InvalidDefinedOperator(input, offset);
                }

                while (position < text.Length && IsIdentifierPart(text[position]))
                {
                    position++;
                }

                name = text[nameStart..position];
            }

            output.Append(macros.ContainsKey(name) ? '1' : '0');
            copiedThrough = position;
            scanIndex = position;
        }

        output.Append(text, copiedThrough, text.Length - copiedThrough);
        return output.ToString();
    }

    private static bool IsDefinedOperatorAt(string text, int index) =>
        text.AsSpan(index).StartsWith("defined", StringComparison.Ordinal)
        && (index == 0 || !IsIdentifierPart(text[index - 1]))
        && (index + "defined".Length == text.Length
            || !IsIdentifierPart(text[index + "defined".Length]));

    private static IdlException InvalidDefinedOperator(IdlInput input, int offset) =>
        new(input, offset, "Invalid preprocessor condition: malformed defined operator.");

    private static bool TryEvaluateUnsignedComparison(string expression, out bool result)
    {
        result = false;

        var match = UnsignedComparisonPattern.Match(expression);
        if (!match.Success)
        {
            return false;
        }

        var leftText = match.Groups["left"].Value.Trim();
        var rightText = match.Groups["right"].Value.Trim();
        if (!leftText.EndsWith("ULL", StringComparison.OrdinalIgnoreCase)
            && !rightText.EndsWith("ULL", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!TryParseUnsignedOperand(leftText, out var left)
            || !TryParseUnsignedOperand(rightText, out var right))
        {
            return false;
        }

        result = match.Groups["operator"].Value switch
        {
            "==" => left == right,
            "!=" => left != right,
            "<" => left < right,
            ">" => left > right,
            "<=" => left <= right,
            ">=" => left >= right,
            _ => false
        };

        return true;
    }

    private static bool TryParseUnsignedOperand(string text, out ulong value)
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
        while (suffixStart > 0 && text[suffixStart - 1] is 'u' or 'U' or 'l' or 'L')
        {
            suffixStart--;
        }

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
            if (separator == 0
                || separator == number.Length - 1
                || !IsDigitForRadix(number[separator - 1], radix)
                || !IsDigitForRadix(number[separator + 1], radix))
            {
                value = 0;

                return false;
            }
        }

        var hasUnsignedSuffix = suffix.Contains('u');
        text = number.Replace("'", string.Empty);

        if (text.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
        {
            return TryParseBase(text[2..], 2, hasUnsignedSuffix, out value);
        }

        if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            if (ulong.TryParse(text[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var unsignedValue))
            {
                value = unsignedValue <= long.MaxValue || hasUnsignedSuffix
                    ? unchecked((long)unsignedValue)
                    : 0;
                return unsignedValue <= long.MaxValue || hasUnsignedSuffix;
            }

            value = 0;

            return false;
        }

        if (text.Length > 1 && text[0] == '0')
        {
            return TryParseBase(text[1..], 8, hasUnsignedSuffix, out value);
        }

        if (hasUnsignedSuffix && ulong.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unsignedDecimal))
        {
            value = unsignedDecimal <= long.MaxValue ? (long)unsignedDecimal : unchecked((long)unsignedDecimal);

            return true;
        }

        return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    private static bool IsDigitForRadix(char character, int radix) =>
        radix switch
        {
            2 => character is '0' or '1',
            8 => character is >= '0' and <= '7',
            10 => character is >= '0' and <= '9',
            16 => character is >= '0' and <= '9'
                or >= 'a' and <= 'f'
                or >= 'A' and <= 'F',
            _ => false
        };

    private static bool TryParseBase(string text, int radix, bool allowUnsigned, out long value)
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

            if (digit < 0
                || digit >= radix
                || parsed > (ulong.MaxValue - (ulong)digit) / (ulong)radix)
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

    private sealed class ConditionalFrame(bool parentActive, bool branchTaken, bool elseSeen)
    {
        public bool ParentActive { get; } = parentActive;
        public bool BranchTaken { get; } = branchTaken;
        public bool ElseSeen { get; } = elseSeen;
    }

    /// <summary>Evaluates the integer expression subset used by #if.</summary>
    private sealed class ConditionalExpression(string text)
    {
        private const int MaximumExpressionNesting = 256;
        private int position;
        private int nestingDepth;

        public long Evaluate()
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
                if (!evaluate)
                {
                    return 0;
                }

                return condition != 0 ? whenTrue : whenFalse;
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
                    value = value != 0 || right != 0 ? 1 : 0;
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
                    value = value != 0 && right != 0 ? 1 : 0;
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
                    value |= right;
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
                    value ^= right;
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
                    value &= right;
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
                        value = value == right ? 1 : 0;
                }
                else if (Take("!="))
                {
                    var right = ParseRelational(evaluate);
                    if (evaluate)
                        value = value != right ? 1 : 0;
                }
                else
                    return value;
            }
        }

        private long ParseRelational(bool evaluate)
        {
            var value = ParseShift(evaluate);
            while (true)
            {
                if (Take("<="))
                { var right = ParseShift(evaluate); if (evaluate) value = value <= right ? 1 : 0; }
                else if (Take(">="))
                { var right = ParseShift(evaluate); if (evaluate) value = value >= right ? 1 : 0; }
                else if (Take("<"))
                { var right = ParseShift(evaluate); if (evaluate) value = value < right ? 1 : 0; }
                else if (Take(">"))
                { var right = ParseShift(evaluate); if (evaluate) value = value > right ? 1 : 0; }
                else
                    return value;
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
                        throw Invalid("shift count must be between 0 and 63");
                    if (evaluate)
                        value <<= (int)right;
                }
                else if (Take(">>"))
                {
                    var right = ParseAdditive(evaluate);
                    if (evaluate && right is < 0 or > 63)
                        throw Invalid("shift count must be between 0 and 63");
                    if (evaluate)
                        value >>= (int)right;
                }
                else
                    return value;
            }
        }

        private long ParseAdditive(bool evaluate)
        {
            var value = ParseMultiplicative(evaluate);
            while (true)
            {
                if (Take("+"))
                { var right = ParseMultiplicative(evaluate); if (evaluate) value += right; }
                else if (Take("-"))
                { var right = ParseMultiplicative(evaluate); if (evaluate) value -= right; }
                else
                    return value;
            }
        }

        private long ParseMultiplicative(bool evaluate)
        {
            var value = ParseUnary(evaluate);
            while (true)
            {
                if (Take("*"))
                { var right = ParseUnary(evaluate); if (evaluate) value *= right; }
                else if (Take("/"))
                {
                    var right = ParseUnary(evaluate);
                    if (evaluate && right == 0)
                        throw Invalid("division by zero");
                    if (evaluate)
                        value /= right;
                }
                else if (Take("%"))
                {
                    var right = ParseUnary(evaluate);
                    if (evaluate && right == 0)
                        throw Invalid("remainder by zero");
                    if (evaluate)
                        value %= right;
                }
                else
                    return value;
            }
        }

        private long ParseUnary(bool evaluate)
        {
            EnterNesting();
            try
            {
                if (Take("!"))
                { var value = ParseUnary(evaluate); return evaluate && value == 0 ? 1 : 0; }
                if (Take("~"))
                { var value = ParseUnary(evaluate); return evaluate ? ~value : 0; }
                if (Take("+"))
                    return ParseUnary(evaluate);
                if (Take("-"))
                { var value = ParseUnary(evaluate); return evaluate ? -value : 0; }
                return ParsePrimary(evaluate);
            }
            finally
            {
                nestingDepth--;
            }
        }

        private void EnterNesting()
        {
            if (nestingDepth >= MaximumExpressionNesting)
            {
                throw Invalid($"expression nesting exceeds the {MaximumExpressionNesting}-level limit");
            }

            nestingDepth++;
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
                var parenthesized = ParseConditional(evaluate);
                if (!Take(")"))
                    throw Invalid("expected ')'");
                return parenthesized;
            }

            var start = position;
            while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or '\''))
                position++;
            if (start == position)
                throw Invalid("expected an integer or identifier");

            var token = text[start..position];
            // Any remaining identifier is undefined and therefore has value zero.
            if (IsIdentifier(token))
                return 0;
            if (!TryParseInteger(token, out var integer))
                throw Invalid($"invalid integer '{token}'");
            return evaluate ? integer : 0;
        }

        private bool TryTakeCharacterEncodingPrefix()
        {
            if (position + 1 < text.Length
                && text[position] is 'L' or 'u' or 'U'
                && text[position + 1] == '\'')
            {
                position++;
                return true;
            }

            if (position + 2 < text.Length
                && text[position] == 'u'
                && text[position + 1] == '8'
                && text[position + 2] == '\'')
            {
                position += 2;
                return true;
            }

            return false;
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

            var value = text[position++] == '\\'
                ? ParseCharacterEscape()
                : text[position - 1];

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
            if (escape is >= '0' and <= '7')
            {
                var value = escape - '0';
                var digits = 1;
                while (digits < 3 && position < text.Length && text[position] is >= '0' and <= '7')
                {
                    value = value * 8 + text[position++] - '0';
                    digits++;
                }

                return value;
            }

            return escape switch
            {
                '\\' => '\\',
                '\'' => '\'',
                '"' => '"',
                '0' => '\0',
                'a' => '\a',
                'b' => '\b',
                'f' => '\f',
                'n' => '\n',
                'r' => '\r',
                't' => '\t',
                'v' => '\v',
                'x' => ParseHexEscape(),
                _ => throw Invalid($"unsupported character escape '\\{escape}'")
            };
        }

        private long ParseHexEscape()
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

        private bool Take(string token)
        {
            SkipWhiteSpace();
            if (!text.AsSpan(position).StartsWith(token, StringComparison.Ordinal))
                return false;
            position += token.Length;
            return true;
        }

        private bool TakeSingle(char token)
        {
            SkipWhiteSpace();
            if (position >= text.Length || text[position] != token
                || (position + 1 < text.Length && text[position + 1] == token))
                return false;
            position++;
            return true;
        }

        private void SkipWhiteSpace()
        {
            while (position < text.Length && char.IsWhiteSpace(text[position]))
                position++;
        }

        private InvalidOperationException Invalid(string message) =>
            new($"{message} at column {position + 1}.");
    }

}
