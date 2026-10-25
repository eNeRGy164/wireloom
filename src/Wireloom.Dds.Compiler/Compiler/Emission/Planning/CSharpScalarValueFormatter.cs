using System.Globalization;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Formats bound scalar IDL values as valid typed C# literals.</summary>
internal static class CSharpScalarValueFormatter
{
    /// <summary>Formats a floating value without losing the IEEE-754 sign of zero.</summary>
    internal static string FormatFloating(string typeName, double value)
    {
        var literal = value.ToString("R", CultureInfo.InvariantCulture);
        if (typeName == "float")
        {
            return $"{literal}F";
        }

        return value == 0 && BitConverter.DoubleToInt64Bits(value) < 0 ? "-0.0D" : literal;
    }

    /// <summary>Formats one bound scalar value for the corresponding C# type.</summary>
    internal static string Format(string typeName, BigInteger value) => typeName switch
    {
        "bool" => value.IsZero ? "false" : "true",
        "char" => CSharpLiteralFormatter.FormatCharacter((char)(int)value),
        "long" when value == long.MinValue => "long.MinValue",
        "long" => $"{value.ToString(CultureInfo.InvariantCulture)}L",
        "ulong" when value == ulong.MaxValue => "ulong.MaxValue",
        "ulong" => $"{value.ToString(CultureInfo.InvariantCulture)}UL",
        "uint" => $"{value.ToString(CultureInfo.InvariantCulture)}U",
        _ => value.ToString(CultureInfo.InvariantCulture)
    };
}
