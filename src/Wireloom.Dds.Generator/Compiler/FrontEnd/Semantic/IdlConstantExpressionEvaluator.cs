using System.Globalization;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Evaluates the integral subset of IDL constant expressions.</summary>
internal sealed class IdlConstantExpressionEvaluator
{
    private readonly string text;
    private readonly IdlSymbolTable symbols;
    private readonly string? currentNamespace;
    private int expressionWidth;
    private bool expressionIsUnsigned;
    private int expressionRank;
    private int position;

    private IdlConstantExpressionEvaluator(string text, IdlSymbolTable symbols, string? currentNamespace, string? expectedType)
    {
        this.text = text;
        this.symbols = symbols;
        this.currentNamespace = currentNamespace;
        expressionWidth = expectedType is null or "long long" or "int64" or "unsigned long long" or "uint64" ? 64 : 32;
        expressionIsUnsigned = expectedType is "unsigned short" or "uint16" or "unsigned long" or "uint32" or "unsigned long long" or "uint64" or "uint8" or "octet";
        expressionRank = expectedType is "unsigned long long" or "uint64" ? 4
            : expectedType is "long long" or "int64" ? 3
            : expectedType is "unsigned long" or "uint32" ? 2
            : 1;
    }

    /// <summary>Evaluates an integral expression against the known constant declarations.</summary>
    public static BigInteger Evaluate(string text, IdlSymbolTable symbols, string? currentNamespace, string? expectedType = null)
    {
        var evaluator = new IdlConstantExpressionEvaluator(text, symbols, currentNamespace, expectedType);
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
            value = ApplyBitwise(value, ParseBitwiseXor(), '|');
        }

        return value;
    }

    private BigInteger ParseBitwiseXor()
    {
        var value = ParseBitwiseAnd();

        while (TryConsume('^'))
        {
            value = ApplyBitwise(value, ParseBitwiseAnd(), '^');
        }

        return value;
    }

    private BigInteger ParseBitwiseAnd()
    {
        var value = ParseShift();

        while (TryConsume('&'))
        {
            value = ApplyBitwise(value, ParseShift(), '&');
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
                value = Shift(value, CheckedShift(ParseAdditive()), isLeft: true);
            }
            else if (TryConsume(">>"))
            {
                value = Shift(value, CheckedShift(ParseAdditive()), isLeft: false);
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
                value = CheckIntermediate(value + ParseMultiplicative());
            }
            else if (TryConsume('-'))
            {
                value = CheckIntermediate(value - ParseMultiplicative());
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
                value = CheckIntermediate(value * ParseUnary());
            }
            else if (TryConsume('/'))
            {
                var divisor = ParseUnary();
                if (divisor.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value = CheckIntermediate(value / divisor);
            }
            else if (TryConsume('%'))
            {
                var divisor = ParseUnary();
                if (divisor.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value = CheckIntermediate(value % divisor);
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
            return CheckIntermediate(-ParseUnary());
        }

        if (TryConsume('~'))
        {
            var operand = ParseUnary();
            var modulus = BigInteger.One << expressionWidth;
            var mask = modulus - BigInteger.One;
            var complemented = (~ToBits(operand, mask, modulus)) & mask;
            return expressionIsUnsigned ? complemented : ToSigned(complemented, expressionWidth);
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
            if (token.Length == 2)
            {
                throw new FormatException("Hexadecimal literal must contain at least one digit.");
            }

            // BigInteger interprets a high leading bit as a sign when parsing
            // hexadecimal digits. Prefix a zero nibble so IDL hex literals stay
            // non-negative (unary minus is handled by ParseUnary).
            return BigInteger.Parse($"0{token[2..]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        }

        if (token.Length > 1 && token[0] == '0' && token.All(char.IsDigit))
        {
            var octal = BigInteger.Zero;
            foreach (var digit in token)
            {
                if (digit is < '0' or > '7')
                {
                    throw new FormatException("Octal integer literals may contain only digits 0 through 7.");
                }

                octal = (octal * 8) + (digit - '0');
            }

            return octal;
        }

        if (BigInteger.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var literal))
        {
            return literal;
        }

        if (!IdlScopedName.IsValid(token))
        {
            throw new FormatException($"Invalid constant name: {token}");
        }

        var qualified = ResolveConstantName(token);
        if (symbols.TryGetConstant(qualified, out var constant)
            && constant.Value is IdlScalarValue.Integral { Value: var constantValue })
        {
            PromoteForConstant(constant);
            return constantValue;
        }

        throw new FormatException($"Unknown integral constant: {token}");
    }

    // IDL 4.2 §7.4.1.4.3 promotes an integer binary expression according to
    // its operand types. A referenced constant therefore cannot be truncated
    // to the destination constant's width before the expression is evaluated.
    private void PromoteForConstant(IdlConstantDeclaration constant)
    {
        var type = new IdlTypeResolver(symbols).Resolve(
            constant.Type,
            constant.Namespace,
            constant.SourceInput,
            constant.SourceOffset);
        while (type is IdlType.Alias alias)
        {
            type = alias.Target;
        }

        var typeName = type is IdlType.Primitive primitive ? primitive.Name : constant.Type;
        var rank = typeName is "unsigned long long" or "uint64" ? 4
            : typeName is "long long" or "int64" ? 3
            : typeName is "unsigned long" or "uint32" ? 2
            : 1;
        var width = rank > 2 ? 64 : 32;

        if (rank > expressionRank)
        {
            expressionRank = rank;
            expressionWidth = width;
            expressionIsUnsigned = rank is 2 or 4;
        }
    }

    private string ResolveConstantName(string name)
    {
        var normalized = name.TrimStart(':').Replace("::", ".");
        if (name.StartsWith("::", StringComparison.Ordinal))
        {
            return normalized;
        }

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
        if (value < 0 || value >= 64)
        {
            throw new FormatException("Shift count is outside the supported range.");
        }

        return (int)value;
    }

    private BigInteger Shift(BigInteger value, int count, bool isLeft)
    {
        if (isLeft)
        {
            return CheckIntermediate(value << count);
        }

        var modulus = BigInteger.One << expressionWidth;
        var bits = ToBits(value, modulus - BigInteger.One, modulus);
        var shifted = bits >> count;

        return expressionIsUnsigned ? shifted : ToSigned(shifted, expressionWidth);
    }

    private BigInteger CheckIntermediate(BigInteger value)
    {
        var minimum = -(BigInteger.One << (expressionWidth - 1));
        var maximum = (BigInteger.One << expressionWidth) - BigInteger.One;
        if (value < minimum || value > maximum)
        {
            throw new FormatException("Constant expression sub-expression exceeds the supported integer precision.");
        }

        return value;
    }

    private BigInteger ApplyBitwise(BigInteger left, BigInteger right, char operation)
    {
        var modulus = BigInteger.One << expressionWidth;
        var mask = modulus - BigInteger.One;
        var leftBits = ToBits(left, mask, modulus);
        var rightBits = ToBits(right, mask, modulus);
        var result = operation switch
        {
            '&' => leftBits & rightBits,
            '^' => leftBits ^ rightBits,
            '|' => leftBits | rightBits,
            _ => throw new InvalidOperationException("Unsupported bitwise operator.")
        };

        return expressionIsUnsigned ? result : ToSigned(result, expressionWidth);
    }

    private static BigInteger ToBits(BigInteger value, BigInteger mask, BigInteger modulus)
    {
        var bits = value % modulus;

        return bits.Sign < 0 ? (bits + modulus) & mask : bits & mask;
    }

    private static BigInteger ToSigned(BigInteger value, int width)
    {
        var signBit = BigInteger.One << (width - 1);

        return value >= signBit ? value - (BigInteger.One << width) : value;
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
