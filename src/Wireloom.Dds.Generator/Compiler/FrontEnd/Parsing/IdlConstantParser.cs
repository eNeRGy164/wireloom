using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

internal sealed partial class IdlDeclarationParser
{
    private bool TryParseConstant(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var constant = ConstantPattern.Match(declarations[position..]);
        if (!constant.Success)
        {
            return false;
        }

        var name = constant.Groups["name"].Value;
        var qualified = Qualify(name, currentNamespace);
        EnsureNewName(input, baseOffset + position, qualified);

        var type = NormalizeIdlType(constant.Groups["type"].Value);
        var expression = constant.Groups["expression"].Value.Trim();
        var integerValue = TryEvaluateIntegerConstant(input, baseOffset + position, type, expression, currentNamespace);
        var declaration = new IdlConstantDeclaration(name, type, expression, currentNamespace, Path.GetFileName(input.Path), integerValue);

        symbols.AddConstant(qualified, declaration);
        declarationQueue.Add(declaration);

        position += constant.Length;

        return true;
    }

    private BigInteger? TryEvaluateIntegerConstant(IdlInput input, int offset, string type, string expression, string? currentNamespace)
    {
        if (type is "string" or "wstring" or "float" or "double" or "long double" or "boolean" or "char" or "wchar")
        {
            return null;
        }

        try
        {
            var value = IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace);

            ValidateConstantRange(input, offset, type, value);

            return value;
        }
        catch (FormatException exception)
        {
            throw new IdlException(input, offset, $"Invalid {type} constant expression: {exception.Message}");
        }
    }

    private static void ValidateConstantRange(IdlInput input, int offset, string type, BigInteger value)
    {
        var (minimum, maximum) = type switch
        {
            "int8" => (BigInteger.Parse("-128"), BigInteger.Parse("127")),
            "uint8" or "octet" => (BigInteger.Zero, BigInteger.Parse("255")),
            "short" or "int16" => (BigInteger.Parse("-32768"), BigInteger.Parse("32767")),
            "unsigned short" or "uint16" => (BigInteger.Zero, BigInteger.Parse("65535")),
            "long" or "int32" => (BigInteger.Parse(int.MinValue.ToString()), BigInteger.Parse(int.MaxValue.ToString())),
            "unsigned long" or "uint32" => (BigInteger.Zero, BigInteger.Parse(uint.MaxValue.ToString())),
            "long long" or "int64" => (BigInteger.Parse(long.MinValue.ToString()), BigInteger.Parse(long.MaxValue.ToString())),
            "unsigned long long" or "uint64" => (BigInteger.Zero, BigInteger.Parse(ulong.MaxValue.ToString())),
            _ => throw new InvalidOperationException($"Unsupported integral constant type: {type}")
        };

        if (value < minimum || value > maximum)
        {
            throw new IdlException(input, offset, $"{type} constant value is outside its representable range.");
        }
    }
}
