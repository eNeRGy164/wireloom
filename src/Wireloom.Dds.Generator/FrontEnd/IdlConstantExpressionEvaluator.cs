using System.Globalization;
using System.Numerics;

namespace Wireloom;

/// <summary>Evaluates the integral subset of IDL constant expressions.</summary>
internal sealed class IdlConstantExpressionEvaluator
{
    private readonly string text;
    private readonly IdlSymbolTable symbols;
    private readonly string? currentNamespace;
    private int position;

    private IdlConstantExpressionEvaluator(string text, IdlSymbolTable symbols, string? currentNamespace)
    {
        this.text = text;
        this.symbols = symbols;
        this.currentNamespace = currentNamespace;
    }

    public static BigInteger Evaluate(string text, IdlSymbolTable symbols, string? currentNamespace)
    {
        var evaluator = new IdlConstantExpressionEvaluator(text, symbols, currentNamespace);
        var value = evaluator.ParseBitwiseOr();
        evaluator.SkipWhitespace();

        if (evaluator.position != text.Length)
        {
            throw new FormatException($"Unexpected token in constant expression: {text[evaluator.position..]}");
        }

        return value;
    }

    private BigInteger ParseBitwiseOr()
    {
        var value = ParseBitwiseXor();

        while (TryConsume('|'))
        {
            value |= ParseBitwiseXor();
        }

        return value;
    }

    private BigInteger ParseBitwiseXor()
    {
        var value = ParseBitwiseAnd();

        while (TryConsume('^'))
        {
            value ^= ParseBitwiseAnd();
        }

        return value;
    }

    private BigInteger ParseBitwiseAnd()
    {
        var value = ParseShift();

        while (TryConsume('&'))
        {
            value &= ParseShift();
        }

        return value;
    }

    private BigInteger ParseShift()
    {
        var value = ParseAdditive();

        while (true)
        {
            if (TryConsume("<<"))
            {
                value <<= CheckedShift(ParseAdditive());
            }
            else if (TryConsume(">>"))
            {
                value >>= CheckedShift(ParseAdditive());
            }
            else
            {
                return value;
            }
        }
    }

    private BigInteger ParseAdditive()
    {
        var value = ParseMultiplicative();

        while (true)
        {
            if (TryConsume('+'))
            {
                value += ParseMultiplicative();
            }
            else if (TryConsume('-'))
            {
                value -= ParseMultiplicative();
            }
            else
            {
                return value;
            }
        }
    }

    private BigInteger ParseMultiplicative()
    {
        var value = ParseUnary();

        while (true)
        {
            if (TryConsume('*'))
            {
                value *= ParseUnary();
            }
            else if (TryConsume('/'))
            {
                var divisor = ParseUnary();
                if (divisor.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value /= divisor;
            }
            else if (TryConsume('%'))
            {
                var divisor = ParseUnary();
                if (divisor.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value %= divisor;
            }
            else
            {
                return value;
            }
        }
    }

    private BigInteger ParseUnary()
    {
        if (TryConsume('+'))
        {
            return ParseUnary();
        }

        if (TryConsume('-'))
        {
            return -ParseUnary();
        }

        if (TryConsume('~'))
        {
            return ~ParseUnary();
        }

        return ParsePrimary();
    }

    private BigInteger ParsePrimary()
    {
        if (TryConsume('('))
        {
            var value = ParseBitwiseOr();
            Expect(')');
            return value;
        }

        SkipWhitespace();

        var start = position;
        while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or ':'))
        {
            position++;
        }

        if (start == position)
        {
            throw new FormatException("Expected an integer literal or constant name.");
        }

        var token = text[start..position];
        if (token.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return BigInteger.Parse(token[2..], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        }

        if (BigInteger.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var literal))
        {
            return literal;
        }

        var qualified = ResolveConstantName(token);
        if (symbols.TryGetConstant(qualified, out var constant) && constant.IntegerValue is { } constantValue)
        {
            return constantValue;
        }

        throw new FormatException($"Unknown integral constant: {token}");
    }

    private string ResolveConstantName(string name)
    {
        var normalized = name.TrimStart(':').Replace("::", ".");
        var scope = currentNamespace;

        while (true)
        {
            var candidate = scope is null ? normalized : $"{scope}.{normalized}";

            if (symbols.ContainsConstant(candidate))
            {
                return candidate;
            }

            if (scope is null)
            {
                return normalized;
            }

            var separator = scope.LastIndexOf('.');
            scope = separator < 0 ? null : scope[..separator];
        }
    }

    private int CheckedShift(BigInteger value)
    {
        if (value < 0 || value > int.MaxValue)
        {
            throw new FormatException("Shift count is outside the supported range.");
        }

        return (int)value;
    }

    private bool TryConsume(char character)
    {
        SkipWhitespace();

        if (position < text.Length && text[position] == character)
        {
            position++;
            return true;
        }

        return false;
    }

    private bool TryConsume(string token)
    {
        SkipWhitespace();

        if (text.AsSpan(position).StartsWith(token.AsSpan(), StringComparison.Ordinal))
        {
            position += token.Length;
            return true;
        }

        return false;
    }

    private void Expect(char character)
    {
        if (!TryConsume(character))
        {
            throw new FormatException($"Expected '{character}' in constant expression.");
        }
    }

    private void SkipWhitespace()
    {
        while (position < text.Length && char.IsWhiteSpace(text[position]))
        {
            position++;
        }
    }
}
