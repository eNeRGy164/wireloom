using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Coordinates the value implementation file and emits struct value operations.</summary>
internal static class ValueEmitter
{
    /// <summary>Renders the C++ value implementation file.</summary>
    internal static string Emit(GenerationPlan plan)
    {
        var source = new SourceWriter();
        EmitBanner(source, plan);
        source.WriteInclude($"{plan.FileStem}.hpp", quoted: true);
        source.BlankLine();
        source.WriteComment("Standard library");
        source.WriteInclude("cmath");
        source.WriteInclude("iomanip");
        source.WriteInclude("limits");
        source.WriteInclude("ostream");
        source.WriteInclude("type_traits");
        source.WriteInclude("utility");
        source.BlankLine();
        source.WriteComment("RTI Connext DDS Modern C++ API");
        source.WriteInclude("rti/util/StreamFlagSaver.hpp");
        source.WriteInclude("rti/util/ostream_operators.hpp");
        source.BlankLine();
        EmitFloatingPointComparison(source);
        if (plan.Structs.SelectMany(declaration => declaration.Members)
            .Any(member => member.Type.ContainsWideString
                || member.Type is MemberTypePlan.Primitive { Mapping.StorageType: "wchar_t" }))
        {
            source.BlankLine();
            StreamEmitter.EmitWideStringHelpers(source);
        }

        var typeNamespaces = new NamespaceBlockWriter(source);
        foreach (var declaration in plan.Enums)
        {
            source.BlankLine();
            typeNamespaces.SwitchTo(declaration.NamespaceSegments);
            StreamEmitter.EmitEnumStreamInsertion(source, declaration);
        }

        foreach (var declaration in plan.Structs)
        {
            source.BlankLine();
            typeNamespaces.SwitchTo(declaration.NamespaceSegments);

            if (declaration.IsFlatData)
            {
                source.WriteComment("The RTI FlatData sample is a serialized buffer accessed through offsets.");
                EmitFlatDataOffsetFunctions(source, declaration);
                EmitStruct(source, declaration with
                {
                    Name = declaration.FlatDataPlainHelperName,
                    IsFlatData = false,
                });

                continue;
            }

            if (declaration.CppBaseType is null
                && !RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
            {
                source.WriteComment("Keep generated IDL value types compatible with RTI's sample access API.");
                source.WriteLine("static_assert(");
                source.WriteLine($"    std::is_standard_layout<{declaration.CppType}>::value,");
                source.WriteLine("    \"IDL structs must use standard layout\");");
                source.BlankLine();
            }
            else
            {
                source.WriteComment("Inherited aggregate storage is not eligible for C-layout sample access.");
            }

            EmitStruct(source, declaration);
        }

        foreach (var declaration in plan.Unions)
        {
            source.BlankLine();
            typeNamespaces.SwitchTo(declaration.NamespaceSegments);
            UnionEmitter.EmitUnion(source, declaration, plan.Profile.RtiVersion == "6.1.2");
        }

        typeNamespaces.Close();

        if (plan.Enums.Count > 0)
        {
            source.BlankLine();
            source.WriteComment("RTI default enumerator values");
            source.OpenNamespaces("rti");
            source.OpenNamespaces("topic");
            foreach (var declaration in plan.Enums)
            {
                var definition = $"const {declaration.CppType} default_enumerator<{declaration.CppType}>::value = " +
                    $"{declaration.CppType}::{declaration.DefaultMember.Name};";
                if (definition.Length <= 120)
                {
                    source.WriteLine(definition);
                    continue;
                }

                source.WriteLine($"const {declaration.CppType}");
                source.WriteLine($"default_enumerator<{declaration.CppType}>::value =");
                source.WriteLine($"    {declaration.CppType}::{declaration.DefaultMember.Name};");
            }

            source.CloseNamespaces("topic");
            source.CloseNamespaces("rti");
        }

        return source.ToString();
    }

    private static void EmitFlatDataOffsetFunctions(SourceWriter source, StructPlan declaration)
    {
        foreach (var (member, index) in declaration.Members.Select((member, index) => (member, index)))
        {
            var offset = index * 4;
            var offsets = string.Join(", ", Enumerable.Repeat(
                offset.ToString(System.Globalization.CultureInfo.InvariantCulture), 4));

            source.OpenBlock($"::std::int32_t {declaration.Name}ConstOffset::{member.Name}() const");
            source.WriteLine($"static const ::rti::flat::offset_t offsets[] = {{ {offsets} }};");
            source.WriteLine("return deserialize<::std::int32_t>(offsets);");
            source.CloseBlock();
            source.BlankLine();

            source.OpenBlock($"::std::int32_t {declaration.Name}Offset::{member.Name}() const");
            source.WriteLine($"static const ::rti::flat::offset_t offsets[] = {{ {offsets} }};");
            source.WriteLine("return deserialize<::std::int32_t>(offsets);");
            source.CloseBlock();
            source.BlankLine();

            source.OpenBlock($"bool {declaration.Name}Offset::{member.Name}(::std::int32_t value)");
            source.WriteLine($"static const ::rti::flat::offset_t offsets[] = {{ {offsets} }};");
            source.WriteLine("return serialize(offsets, value);");
            source.CloseBlock();
            source.BlankLine();
        }
    }

    private static void EmitBanner(SourceWriter source, GenerationPlan plan)
    {
        source.WriteComment("<auto-generated/>");
        source.WriteComment($"Generated by Wireloom from '{plan.FileStem}.idl'.");
        source.WriteComment("Do not edit this file directly. Update the IDL source and rebuild.");
        source.WriteComment("Requires the RTI Connext DDS Modern C++ runtime.");
        source.BlankLine();
    }

    private static void EmitFloatingPointComparison(SourceWriter source)
    {
        source.WriteLine("namespace");
        source.WriteLine("{");
        source.WriteComment("Preserve the epsilon comparison used by RTI-generated value operators.");
        source.WriteLine("template<class T>");
        source.OpenBlock("bool wireloom_float_equal(T left, T right)");
        source.WriteLines("""
            const auto difference = std::fabs(left - right);
            const auto epsilon = std::numeric_limits<T>::epsilon();
            const auto minimum = (std::numeric_limits<T>::min)();
            """);
        source.BlankLine();
        source.WriteLine("return !(difference > epsilon && !(difference < minimum));");
        source.CloseBlock();
        source.WriteLine("} // namespace");
    }

    private static void EmitStruct(SourceWriter source, StructPlan declaration)
    {
        source.WriteComment("Value construction");
        source.WriteLine($"{declaration.Name}::{declaration.Name}() = default;");
        if (declaration.Members.Count > 0)
        {
            EmitValueConstructor(source, declaration);
        }

        source.BlankLine();
        source.WriteComment("Value comparison");
        EmitEquality(source, declaration);
        source.BlankLine();
        EmitInequality(source, declaration);

        source.BlankLine();
        source.WriteComment("Value swapping");
        EmitSwap(source, declaration);

        source.BlankLine();
        source.WriteComment("Stream representation");
        StreamEmitter.EmitStreamInsertion(source, declaration);
    }

    private static void EmitValueConstructor(SourceWriter source, StructPlan declaration)
    {
        string Relative(string spelling) => CppTypeNameFormatter.MakeRelative(spelling, declaration.NamespaceSegments);

        source.WriteLine($"{declaration.Name}::{declaration.Name}(");
        var parameters = new List<string>();
        if (declaration.CppBaseType is { } baseType)
        {
            parameters.Add($"const {Relative(baseType)}& base");
        }

        parameters.AddRange(declaration.Members.Select(member =>
            $"{member.ConstructorParameterType} {member.Name}"));
        for (var index = 0; index < parameters.Count; index++)
        {
            var suffix = index == parameters.Count - 1 ? string.Empty : ",";
            source.WriteLine($"    {parameters[index]}{suffix}");
        }

        source.WriteLine(")");
        var initializers = new List<string>();
        if (declaration.CppBaseType is { } initializerBaseType)
        {
            initializers.Add($"{Relative(initializerBaseType)}(base)");
        }

        initializers.AddRange(declaration.Members.Select(member => $"{member.Name}({member.Name})"));
        if (initializers.Count > 0)
        {
            source.WriteLine($"    : {initializers[0]}");
            for (var index = 1; index < initializers.Count; index++)
            {
                source.WriteLine($"    , {initializers[index]}");
            }
        }

        source.WriteLine("{");
        source.WriteLine("}");
    }

    private static void EmitEquality(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("bool operator==(");
        source.WriteLine($"    const {declaration.Name}& left,");
        source.WriteLine($"    const {declaration.Name}& right)");
        source.WriteLine("{");
        var comparisons = new List<string>();
        if (declaration.CppBaseType is { } baseType)
        {
            var relativeBaseType = CppTypeNameFormatter.MakeRelative(baseType, declaration.NamespaceSegments);
            comparisons.Add($"static_cast<const {relativeBaseType}&>(left) == static_cast<const {relativeBaseType}&>(right)");
        }

        comparisons.AddRange(declaration.Members.Select(GetEqualityExpression));
        if (comparisons.Count == 0)
        {
            source.WriteLine("    return true;");
        }
        else if (comparisons.Any(comparison => comparison.Length > 100
            && (comparison.Contains(" == ", StringComparison.Ordinal)
                || comparison.StartsWith("wireloom_float_equal(", StringComparison.Ordinal))))
        {
            source.WriteLine("    return");
            for (var index = 0; index < comparisons.Count; index++)
            {
                var comparison = comparisons[index];
                var equalityOperatorIndex = comparison.IndexOf(" == ", StringComparison.Ordinal);
                var prefix = index == 0 ? "        " : "        && ";
                var suffix = index == comparisons.Count - 1 ? ";" : string.Empty;
                if (equalityOperatorIndex > 0)
                {
                    source.WriteLine($"{prefix}{comparison[..equalityOperatorIndex]}");
                    source.WriteLine($"            == {comparison[(equalityOperatorIndex + 4)..]}{suffix}");
                }
                else
                {
                    source.WriteLine($"{prefix}wireloom_float_equal(");
                    var argumentsStart = comparison.IndexOf('(') + 1;
                    var argumentSeparator = comparison.IndexOf(", ", argumentsStart, StringComparison.Ordinal);
                    source.WriteLine($"            {comparison[argumentsStart..argumentSeparator]},");
                    source.WriteLine($"            {comparison[(argumentSeparator + 2)..]}{suffix}");
                }
            }
        }
        else
        {
            for (var index = 0; index < comparisons.Count; index++)
            {
                var prefix = index == 0 ? "    return " : "        && ";
                var suffix = index == comparisons.Count - 1 ? ";" : string.Empty;
                source.WriteLine($"{prefix}{comparisons[index]}{suffix}");
            }
        }

        source.WriteLine("}");
    }

    private static string GetEqualityExpression(MemberPlan member) =>
        member is { IsOptional: false, Type: MemberTypePlan.Primitive primitive }
        ? primitive.Mapping.StorageType switch
        {
        "float" => $"wireloom_float_equal(left.{member.Name}, right.{member.Name})",
        "double" => $"wireloom_float_equal(left.{member.Name}, right.{member.Name})",
        _ => $"left.{member.Name} == right.{member.Name}",
        }
        : $"left.{member.Name} == right.{member.Name}";

    private static void EmitInequality(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("bool operator!=(");
        source.WriteLine($"    const {declaration.Name}& left,");
        source.WriteLine($"    const {declaration.Name}& right)");
        source.OpenBrace();
        source.WriteLine("return !(left == right);");
        source.CloseBlock();
    }

    private static void EmitSwap(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("void swap(");
        source.WriteLine($"    {declaration.Name}& left,");
        source.WriteLine($"    {declaration.Name}& right) noexcept");
        source.OpenBrace();
        source.WriteLine("using std::swap;");
        if (declaration.CppBaseType is { } baseType)
        {
            var relativeBaseType = CppTypeNameFormatter.MakeRelative(baseType, declaration.NamespaceSegments);
            source.WriteLine($"swap(static_cast<{relativeBaseType}&>(left), static_cast<{relativeBaseType}&>(right));");
        }

        foreach (var member in declaration.Members)
        {
            source.WriteLine($"swap(left.{member.Name}, right.{member.Name});");
        }

        source.CloseBlock();
    }
}
