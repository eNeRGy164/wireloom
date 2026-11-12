using System.Globalization;
using System.Text;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Formats bound IDL literals and defaults for the generated C++ and RTI metadata.</summary>
internal static class LiteralFormatter
{
    /// <summary>Formats a bound default for the resolved C++ member type.</summary>
    internal static bool TryFormatMemberDefault(
        IdlScalarValue value,
        MemberTypePlan type,
        out string formatted)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        if (type is MemberTypePlan.Primitive primitive)
        {
            return TryFormatDefault(value, primitive.Mapping, out formatted);
        }

        if (type is MemberTypePlan.Enumeration enumeration
            && value is IdlScalarValue.Enumeration enumValue
            && enumValue.TypeQualifiedName == enumeration.QualifiedName.Replace("::", ".", StringComparison.Ordinal))
        {
            formatted = $"::{enumeration.QualifiedName}::{enumValue.MemberName}";
            return true;
        }

        if (type is MemberTypePlan.String { IsWide: false }
            && value is IdlScalarValue.Text { IsWide: false } text)
        {
            formatted = FormatStringLiteral(text.Value);
            return true;
        }

        if (type is MemberTypePlan.String { IsWide: true }
            && value is IdlScalarValue.Text { IsWide: true } wideText)
        {
            formatted = FormatWideStringLiteral(wideText.Value);
            return true;
        }

        formatted = string.Empty;
        return false;
    }

    /// <summary>Gets the native TypeCode default for supported integer and enum members.</summary>
    internal static string? GetTypeCodeDefaultValue(
        IdlType idlType,
        MemberTypePlan memberType,
        MemberTypePlan plannedMemberType,
        IdlScalarValue? defaultValue,
        IReadOnlyDictionary<string, EnumPlan> enums)
    {
        if (idlType is IdlType.Primitive { Name: "long" }
            && defaultValue is IdlScalarValue.Integral)
        {
            return plannedMemberType.DefaultValue;
        }

        while (memberType is MemberTypePlan.Alias alias)
        {
            memberType = alias.Target;
        }

        if (memberType is MemberTypePlan.Enumeration enumeration
            && defaultValue is IdlScalarValue.Enumeration enumValue
            && enumValue.TypeQualifiedName == enumeration.QualifiedName.Replace("::", ".", StringComparison.Ordinal)
            && enums.TryGetValue(enumValue.TypeQualifiedName, out var enumPlan))
        {
            var value = enumPlan.Members.FirstOrDefault(item => item.Name == enumValue.MemberName)?.Value;
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        return null;
    }

    /// <summary>Formats a union discriminator label as a C++ expression.</summary>
    internal static string FormatUnionLabelExpression(
        long label,
        EnumPlan? discriminatorEnum,
        string discriminatorPrimitive)
    {
        if (discriminatorEnum is not null)
        {
            return $"::{discriminatorEnum.QualifiedName}::{discriminatorEnum.Members.First(member => member.Value == label).Name}";
        }

        if (discriminatorPrimitive == "boolean")
        {
            return label == 0 ? "false" : "true";
        }

        if (discriminatorPrimitive == "char" && label is > sbyte.MaxValue and <= byte.MaxValue)
        {
            return $"static_cast<char>({label - (byte.MaxValue + 1L)})";
        }

        return label.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>Formats a bound primitive default as a valid C++ literal or value expression.</summary>
    internal static string FormatDefault(IdlScalarValue value, PrimitiveType primitive)
    {
        if (TryFormatDefault(value, primitive, out var formatted))
        {
            return formatted;
        }

        throw new InvalidOperationException("The validated C++ default could not be formatted.");
    }

    private static bool TryFormatDefault(IdlScalarValue value, PrimitiveType primitive, out string formatted)
    {
        switch (value)
        {
            case IdlScalarValue.Integral integral:
                if (primitive.StorageType == "::std::uint64_t")
                {
                    formatted = integral.Value.ToString(CultureInfo.InvariantCulture) + "ULL";
                }
                else if (primitive.StorageType == "::std::int64_t" && integral.Value == long.MinValue)
                {
                    formatted = "(-9223372036854775807LL - 1LL)";
                }
                else if (primitive.StorageType == "::std::int64_t")
                {
                    formatted = integral.Value.ToString(CultureInfo.InvariantCulture) + "LL";
                }
                else
                {
                    formatted = integral.Value.ToString(CultureInfo.InvariantCulture);
                }

                return true;
            case IdlScalarValue.Boolean boolean:
                formatted = boolean.Value ? "true" : "false";
                return true;
            case IdlScalarValue.Floating floating:
                if (primitive.StorageType == "::rti::core::LongDouble")
                {
                    formatted = "::rti::core::LongDouble{}";
                    return floating.Value == 0.0;
                }

                formatted = floating.Value.ToString("R", CultureInfo.InvariantCulture);
                if (!formatted.Contains('.', StringComparison.Ordinal)
                    && !formatted.Contains('E', StringComparison.OrdinalIgnoreCase))
                {
                    formatted += ".0";
                }

                if (primitive.StorageType == "float")
                {
                    formatted += "F";
                }

                return true;
            case IdlScalarValue.Character { IsWide: false } character:
                formatted = FormatCharacter(character.Value, wide: false);
                return true;
            case IdlScalarValue.Character character:
                formatted = FormatCharacter(character.Value, wide: true);
                return true;
            default:
                formatted = string.Empty;
                return false;
        }
    }

    private static string FormatCharacter(char value, bool wide)
    {
        var literal = value switch
        {
            '\\' => "\\\\",
            '\'' => "\\'",
            '\n' => "\\n",
            '\r' => "\\r",
            '\t' => "\\t",
            '\0' => "\\0",
            _ when char.IsControl(value) => $"\\x{(int)value:X4}",
            _ => value.ToString(),
        };

        return $"{(wide ? "L" : string.Empty)}'{literal}'";
    }

    /// <summary>Formats a narrow string as an escaped C++ literal.</summary>
    internal static string FormatStringLiteral(string value)
    {
        var literal = new StringBuilder(value.Length + 2).Append('"');
        foreach (var character in value)
        {
            literal.Append(character switch
            {
                '\\' => "\\\\",
                '"' => "\\\"",
                '\n' => "\\n",
                '\r' => "\\r",
                '\t' => "\\t",
                '\0' => "\\000",
                _ when char.IsControl(character) => "\\" + Convert.ToString(character, 8).PadLeft(3, '0'),
                _ => character.ToString(),
            });
        }

        return literal.Append('"').ToString();
    }

    private static string FormatWideStringLiteral(string value)
    {
        var literal = new StringBuilder(value.Length + 3).Append("L\"");
        foreach (var rune in value.EnumerateRunes())
        {
            switch (rune.Value)
            {
                case '\\':
                    literal.Append("\\\\");
                    break;
                case '"':
                    literal.Append("\\\"");
                    break;
                case '\n':
                    literal.Append("\\n");
                    break;
                case '\r':
                    literal.Append("\\r");
                    break;
                case '\t':
                    literal.Append("\\t");
                    break;
                case 0:
                    literal.Append("\\0");
                    break;
                default:
                    if (Rune.IsControl(rune) || rune.Value > 0x7E)
                    {
                        literal.Append(rune.Value <= char.MaxValue
                            ? $"\\u{rune.Value:X4}"
                            : $"\\U{rune.Value:X8}");
                    }
                    else
                    {
                        literal.Append(rune.ToString());
                    }

                    break;
            }
        }

        return literal.Append('"').ToString();
    }
}
