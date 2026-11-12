using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits readable stream output for generated values and their member storage.</summary>
internal static class StreamEmitter
{
    /// <summary>Writes the stream helpers that preserve wide string code units.</summary>
    internal static void EmitWideStringHelpers(SourceWriter source)
    {
        source.WriteLine("namespace");
        source.WriteLine("{");
        source.OpenBlock("void wireloom_write_wide_escape(std::ostream& output, char prefix, unsigned long value, int digits)");
        source.WriteLines("""
            static const char hexadecimal_digits[] = "0123456789ABCDEF";
            output.put('\\');
            output.put(prefix);
            for (int shift = (digits - 1) * 4; shift >= 0; shift -= 4)
            {
                output.put(hexadecimal_digits[(value >> shift) & 0x0F]);
            }
            """);
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock("void wireloom_write_wide_string(std::ostream& output, const std::wstring& value)");
        source.WriteLines("""
            using UnsignedWideCharacter = typename std::make_unsigned<wchar_t>::type;
            output << "L\"";
            for (const wchar_t character : value)
            {
                const auto code_unit = static_cast<unsigned long>(
                    static_cast<UnsignedWideCharacter>(character));
                switch (code_unit)
                {
                    case '\\': output << "\\\\"; break;
                    case '"': output << "\\\""; break;
                    case '\n': output << "\\n"; break;
                    case '\r': output << "\\r"; break;
                    case '\t': output << "\\t"; break;
                    default:
                        if (code_unit >= 0x20 && code_unit <= 0x7E)
                        {
                            output.put(static_cast<char>(code_unit));
                        }
                        else if (code_unit <= 0xFFFF)
                        {
                            wireloom_write_wide_escape(output, 'u', code_unit, 4);
                        }
                        else
                        {
                            wireloom_write_wide_escape(output, 'U', code_unit, 8);
                        }
                        break;
                }
            }
            output << '"';
            """);
        source.CloseBlock();
        source.WriteLine("} // namespace");
    }

    /// <summary>Writes the struct stream insertion operator.</summary>
    internal static void EmitStreamInsertion(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("std::ostream& operator<<(");
        source.WriteLine("    std::ostream& output,");
        source.WriteLine($"    const {declaration.Name}& value)");
        source.WriteLines("""
            {
                ::rti::util::StreamFlagSaver stream_flags(output);
                output << "[";
            """);
        var hasPreviousValue = false;
        if (declaration.CppBaseType is { } baseType)
        {
            var relativeBaseType = CppTypeNameFormatter.MakeRelative(baseType, declaration.NamespaceSegments);
            source.WriteLine($"    output << \"base: \" << static_cast<const {relativeBaseType}&>(value);");
            hasPreviousValue = true;
        }

        foreach (var member in declaration.Members)
        {
            if (hasPreviousValue)
            {
                source.WriteLine("    output << \", \";");
            }

            if (member.IsOptional && (member.Type.ContainsCollection || IsAggregateType(member.Type)))
            {
                EmitOptionalMemberStreamValue(source, member);
            }
            else
            {
                EmitMemberStreamValue(source, member);
            }

            hasPreviousValue = true;
        }

        source.WriteLines("""
                output << "]";
                return output;
            }
            """);
    }

    private static void EmitOptionalMemberStreamValue(SourceWriter source, MemberPlan member)
    {
        source.WriteLine($"    if (value.{member.Name}.has_value()) {{");
        if (TryGetArrayType(member.Type, out var array))
        {
            source.WriteLine($"        output << \"{member.Name}: \";");
            EmitArrayStreamValue(
                source,
                $"value.{member.Name}.value()",
                GetArrayDimensions(array),
                0,
                8,
                IsWideStringArray(array));
        }
        else
        {
            source.WriteLine($"        output << \"{member.Name}: \" << value.{member.Name}.value();");
        }

        source.WriteLine("    } else {");
        source.WriteLine($"        output << \"{member.Name}: <unset>\";");
        source.WriteLine("    }");
    }

    private static void EmitMemberStreamValue(SourceWriter source, MemberPlan member)
    {
        var memberLabel = $"    output << \"{member.Name}: \"";
        if (TryGetArrayType(member.Type, out var array))
        {
            var dimensions = GetArrayDimensions(array);
            source.WriteLine($"{memberLabel};");
            EmitArrayStreamValue(
                source,
                $"value.{member.Name}",
                dimensions,
                0,
                4,
                IsWideStringArray(array));
            return;
        }

        if (member.Type is MemberTypePlan.String { IsWide: true })
        {
            source.WriteLine($"{memberLabel};");
            source.WriteLine($"    wireloom_write_wide_string(output, value.{member.Name});");
            return;
        }

        if (member.Type is not MemberTypePlan.Primitive primitive)
        {
            source.WriteLine($"{memberLabel} << value.{member.Name};");
            return;
        }

        switch (primitive.Mapping.StorageType)
        {
            case "wchar_t":
                source.WriteLine($"{memberLabel};");
                source.WriteLine($"    wireloom_write_wide_string(output, ::std::wstring(1, value.{member.Name}));");
                break;
            case "float":
                source.WriteLine($"{memberLabel} << std::setprecision(9) << value.{member.Name};");
                break;
            case "double":
                source.WriteLine($"{memberLabel} << std::setprecision(15) << value.{member.Name};");
                break;
            case "::std::uint8_t" when primitive.Mapping.IsOctet:
                source.WriteLine($"{memberLabel} << std::hex << static_cast<int>(value.{member.Name});");
                break;
            case "::std::int8_t":
            case "::std::uint8_t":
                source.WriteLine($"{memberLabel} << static_cast<int>(value.{member.Name});");
                break;
            default:
                source.WriteLine($"{memberLabel} << value.{member.Name};");
                break;
        }
    }

    private static bool TryGetArrayType(MemberTypePlan type, out MemberTypePlan.Array array)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        if (type is MemberTypePlan.Array arrayType)
        {
            array = arrayType;
            return true;
        }

        array = null!;
        return false;
    }

    private static bool IsAggregateType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Struct or MemberTypePlan.Union => true,
        MemberTypePlan.Alias alias => IsAggregateType(alias.Target),
        _ => false,
    };

    private static IReadOnlyList<int> GetArrayDimensions(MemberTypePlan.Array array)
    {
        var dimensions = new List<int>(array.Dimensions);
        var elementType = array.ElementType;
        while (true)
        {
            while (elementType is MemberTypePlan.Alias alias)
            {
                elementType = alias.Target;
            }

            if (elementType is not MemberTypePlan.Array nestedArray)
            {
                return dimensions;
            }

            dimensions.AddRange(nestedArray.Dimensions);
            elementType = nestedArray.ElementType;
        }
    }

    private static void EmitArrayStreamValue(
        SourceWriter source,
        string expression,
        IReadOnlyList<int> dimensions,
        int dimensionIndex,
        int indentation,
        bool wideStringElements)
    {
        var spaces = new string(' ', indentation);
        source.WriteLine($"{spaces}output << \"[\";");
        for (var index = 0; index < dimensions[dimensionIndex]; index++)
        {
            if (index > 0)
            {
                source.WriteLine($"{spaces}output << \", \";");
            }

            var element = $"{expression}[{index}]";
            if (dimensionIndex + 1 == dimensions.Count)
            {
                source.WriteLine(wideStringElements
                    ? $"{spaces}wireloom_write_wide_string(output, {element});"
                    : $"{spaces}output << {element};");
            }
            else
            {
                EmitArrayStreamValue(
                    source,
                    element,
                    dimensions,
                    dimensionIndex + 1,
                    indentation,
                    wideStringElements);
            }
        }

        source.WriteLine($"{spaces}output << \"]\";");
    }

    private static bool IsWideStringArray(MemberTypePlan.Array array)
    {
        var elementType = array.ElementType;
        while (true)
        {
            if (elementType is MemberTypePlan.Alias alias)
            {
                elementType = alias.Target;
            }
            else if (elementType is MemberTypePlan.Array nestedArray)
            {
                elementType = nestedArray.ElementType;
            }
            else
            {
                break;
            }
        }

        return elementType is MemberTypePlan.String { IsWide: true };
    }

    /// <summary>Writes the enum stream insertion operator.</summary>
    internal static void EmitEnumStreamInsertion(SourceWriter source, EnumPlan declaration)
    {
        source.WriteLine("std::ostream& operator<<(std::ostream& output, const " + declaration.Name + "& value)");
        source.OpenBrace();
        source.WriteLines("""
            ::rti::util::StreamFlagSaver stream_flags(output);
            switch (value)
            {
            """);
        foreach (var member in declaration.Members)
        {
            source.WriteLine($"    case {declaration.Name}::{member.Name}:");
            source.WriteLine($"        output << \"{declaration.Name}::{member.Name}\";");
            source.WriteLine("        break;");
        }

        source.WriteLines("""
                default:
                    break;
            }
            return output;
            """);
        source.CloseBlock();
    }
}
