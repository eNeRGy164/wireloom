using System.Globalization;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Evaluates the integral subset of IDL constant expressions.</summary>
internal sealed class IdlConstantExpressionEvaluator
{
    private readonly string text;
    private readonly IdlSymbolTable symbols;
    private readonly string? currentNamespace;
    private readonly IntegerType defaultType;
    private int position;

    private IdlConstantExpressionEvaluator(string text, IdlSymbolTable symbols, string? currentNamespace, string? expectedType)
    {
        this.text = text;
        this.symbols = symbols;
        this.currentNamespace = currentNamespace;
        defaultType = TypeFromName(expectedType) ?? new IntegerType(3, isUnsigned: false);
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

        return value.Value;
    }

    private IntegerValue ParseBitwiseOr()
    {
        var value = ParseBitwiseXor();

        while (TryConsume('|'))
        {
            value = ApplyBitwise(value, ParseBitwiseXor(), '|');
        }

        return value;
    }

    private IntegerValue ParseBitwiseXor()
    {
        var value = ParseBitwiseAnd();

        while (TryConsume('^'))
        {
            value = ApplyBitwise(value, ParseBitwiseAnd(), '^');
        }

        return value;
    }

    private IntegerValue ParseBitwiseAnd()
    {
        var value = ParseShift();

        while (TryConsume('&'))
        {
            value = ApplyBitwise(value, ParseShift(), '&');
        }

        return value;
    }

    private IntegerValue ParseShift()
    {
        var value = ParseAdditive();

        while (true)
        {
            if (TryConsume("<<"))
            {
                value = Shift(value, CheckedShift(ParseAdditive().Value), isLeft: true);
            }
            else if (TryConsume(">>"))
            {
                value = Shift(value, CheckedShift(ParseAdditive().Value), isLeft: false);
            }
            else
            {
                return value;
            }
        }
    }

    private IntegerValue ParseAdditive()
    {
        var value = ParseMultiplicative();

        while (true)
        {
            if (TryConsume('+'))
            {
                value = ApplyArithmetic(value, ParseMultiplicative(), static (left, right) => left + right);
            }
            else if (TryConsume('-'))
            {
                value = ApplyArithmetic(value, ParseMultiplicative(), static (left, right) => left - right);
            }
            else
            {
                return value;
            }
        }
    }

    private IntegerValue ParseMultiplicative()
    {
        var value = ParseUnary();

        while (true)
        {
            if (TryConsume('*'))
            {
                value = ApplyArithmetic(value, ParseUnary(), static (left, right) => left * right);
            }
            else if (TryConsume('/'))
            {
                var divisor = ParseUnary();
                if (divisor.Value.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value = ApplyArithmetic(value, divisor, static (left, right) => left / right);
            }
            else if (TryConsume('%'))
            {
                var divisor = ParseUnary();
                if (divisor.Value.IsZero)
                {
                    throw new FormatException("Division by zero in constant expression.");
                }

                value = ApplyArithmetic(value, divisor, static (left, right) => left % right);
            }
            else
            {
                return value;
            }
        }
    }

    private IntegerValue ParseUnary()
    {
        if (TryConsume('+'))
        {
            return ParseUnary();
        }

        if (TryConsume('-'))
        {
            var operand = ParseUnary();
            var type = operand.Type.IsUnsigned ? SignedType(operand.Type) : operand.Type;
            return CheckIntermediate(new IntegerValue(-operand.Value, type));
        }

        if (TryConsume('~'))
        {
            var operand = ParseUnary();
            var modulus = BigInteger.One << operand.Type.Width;
            var mask = modulus - BigInteger.One;
            var complement = (~ToBits(operand.Value, mask, modulus)) & mask;
            var result = operand.Type.IsUnsigned ? complement : ToSigned(complement, operand.Type.Width);
            return new IntegerValue(result, operand.Type);
        }

        return ParsePrimary();
    }

    private IntegerValue ParsePrimary()
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
            var hexadecimal = BigInteger.Parse($"0{token[2..]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            return Literal(hexadecimal);
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

            return Literal(octal);
        }

        if (BigInteger.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var literal))
        {
            return Literal(literal);
        }

        if (!IdlScopedName.IsValid(token))
        {
            throw new FormatException($"Invalid constant name: {token}");
        }

        var qualified = ResolveConstantName(token);
        if (symbols.TryGetConstant(qualified, out var constant)
            && constant.Value is IdlScalarValue.Integral { Value: var constantValue })
        {
            return new IntegerValue(constantValue, ConstantType(constant));
        }

        throw new FormatException($"Unknown integral constant: {token}");
    }

    private IntegerValue Literal(BigInteger value)
    {
        var type = defaultType;
        if (type.IsUnsigned)
        {
            if (value > Maximum(type.Width))
            {
                type = new IntegerType(4, isUnsigned: true);
            }
        }
        else if (value > Maximum(type.Width - 1))
        {
            type = type.Width == 64 || value > Maximum(63)
                ? new IntegerType(4, isUnsigned: true)
                : new IntegerType(3, isUnsigned: false);
        }

        if (value > Maximum(type.Width))
        {
            throw new FormatException("Constant expression sub-expression exceeds the supported integer precision.");
        }

        return new IntegerValue(value, type);
    }

    private static IntegerValue ApplyArithmetic(IntegerValue left, IntegerValue right, Func<BigInteger, BigInteger, BigInteger> operation)
    {
        var type = Promote(left.Type, right.Type);
        return CheckIntermediate(new IntegerValue(operation(left.Value, right.Value), type));
    }

    private static IntegerValue ApplyBitwise(IntegerValue left, IntegerValue right, char operation)
    {
        var type = Promote(left.Type, right.Type);
        var modulus = BigInteger.One << type.Width;
        var mask = modulus - BigInteger.One;
        var leftBits = ToBits(left.Value, mask, modulus);
        var rightBits = ToBits(right.Value, mask, modulus);

        BigInteger result;
        if (operation == '&')
        {
            result = leftBits & rightBits;
        }
        else if (operation == '^')
        {
            result = leftBits ^ rightBits;
        }
        else
        {
            result = leftBits | rightBits;
        }

        var value = type.IsUnsigned ? result : ToSigned(result, type.Width);
        return new IntegerValue(value, type);
    }

    private static IntegerValue Shift(IntegerValue value, int count, bool isLeft)
    {
        var modulus = BigInteger.One << value.Type.Width;
        var mask = modulus - BigInteger.One;
        BigInteger result;
        if (isLeft)
        {
            result = CheckIntermediate(new IntegerValue(value.Value << count, value.Type)).Value;
        }
        else
        {
            result = ToBits(value.Value, mask, modulus) >> count;
            if (!value.Type.IsUnsigned)
            {
                result = ToSigned(result, value.Type.Width);
            }
        }

        return new IntegerValue(result, value.Type);
    }

    private static IntegerValue CheckIntermediate(IntegerValue value)
    {
        var minimum = value.Type.IsUnsigned ? BigInteger.Zero : -(BigInteger.One << (value.Type.Width - 1));
        var maximum = value.Type.IsUnsigned ? Maximum(value.Type.Width) : Maximum(value.Type.Width - 1);
        if (value.Value < minimum || value.Value > maximum)
        {
            throw new FormatException("Constant expression sub-expression exceeds the supported integer precision.");
        }

        return value;
    }

    private static IntegerType Promote(IntegerType left, IntegerType right)
    {
        if (left.Rank > right.Rank)
        {
            return left;
        }

        if (right.Rank > left.Rank)
        {
            return right;
        }

        return new IntegerType(left.Rank, left.IsUnsigned || right.IsUnsigned);
    }

    private IntegerType ConstantType(IdlConstantDeclaration constant)
    {
        var type = new IdlTypeResolver(symbols).Resolve(constant.Type, constant.Namespace, constant.SourceInput, constant.SourceOffset);
        while (type is IdlType.Alias alias)
        {
            type = alias.Target;
        }

        return TypeFromName(type is IdlType.Primitive primitive ? primitive.Name : constant.Type)
            ?? throw new FormatException($"Constant '{constant.Name}' is not an integer constant.");
    }

    private static IntegerType? TypeFromName(string? typeName) => typeName switch
    {
        "unsigned long long" or "uint64" => new IntegerType(4, true),
        "long long" or "int64" => new IntegerType(3, false),
        "unsigned long" or "uint32" => new IntegerType(2, true),
        "long" or "int32" => new IntegerType(1, false),
        "unsigned short" or "uint16" or "uint8" or "octet" => new IntegerType(1, true),
        "short" or "int16" or "int8" => new IntegerType(1, false),
        _ => null
    };

    private static IntegerType SignedType(IntegerType type) => type.Rank switch
    {
        4 => new IntegerType(3, false),
        2 => new IntegerType(1, false),
        _ => type
    };

    private static int CheckedShift(BigInteger value)
    {
        if (value < 0 || value >= 64)
        {
            throw new FormatException("Shift count is outside the supported range.");
        }

        return (int)value;
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

    private static BigInteger Maximum(int bits) => (BigInteger.One << bits) - BigInteger.One;
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

    private readonly struct IntegerValue(BigInteger value, IntegerType type)
    {
        internal BigInteger Value { get; } = value;
        internal IntegerType Type { get; } = type;
    }

    private readonly struct IntegerType(int rank, bool isUnsigned)
    {
        internal int Rank { get; } = rank;
        internal bool IsUnsigned { get; } = isUnsigned;
        internal int Width => Rank >= 3 ? 64 : 32;
    }
}
