namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Validates scalar values against their declared IDL ranges.</summary>
internal static class IdlScalarTypeRules
{
    /// <summary>Rejects a scalar value outside the range of its declared type.</summary>
    internal static void ValidateConstantRange(IdlInput input, int offset, string type, BigInteger value)
    {
        var (minimum, maximum) = type switch
        {
            "boolean" => (BigInteger.Zero, BigInteger.One),
            "char" => (new BigInteger(byte.MinValue), new BigInteger(byte.MaxValue)),
            "wchar" => (new BigInteger(char.MinValue), new BigInteger(char.MaxValue)),
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
