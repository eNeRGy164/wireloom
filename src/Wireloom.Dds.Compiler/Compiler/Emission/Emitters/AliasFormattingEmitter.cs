using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits value formatting for aliases whose root shape RTI cannot format.</summary>
internal static class AliasFormattingEmitter
{
    /// <summary>Reports whether an alias root needs managed formatting instead of RTI DynamicData.</summary>
    internal static bool RequiresFallback(CollectionAliasEmissionPlan plan)
    {
        var target = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan);
        return plan.IsCollection || target is not (StructEmissionType or UnionEmissionType);
    }

    /// <summary>Emits the typed instance formatter that hides RTI's nonvirtual root formatter.</summary>
    internal static void EmitSupportFormatter(GeneratedSourceWriter writer, IdlTypedef declaration, CollectionAliasEmissionPlan plan)
    {
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        writer.WriteXmlSummary("Formats an alias sample without using RTI's unsupported root DynamicData conversion.");
        writer.WriteXmlParam("sample", "The alias sample to format.");
        writer.WriteXmlReturns("Diagnostic text with invariant numbers, quoted collection strings, and RTI-formatted aggregate elements.");
        writer.WriteXmlRemarks("This method hides RTI's nonvirtual default formatter. Calls through the base TypeSupport type and explicit print-format overloads retain RTI behavior. This diagnostic text is not a serialization format.");
        writer.WriteXmlException("global::System.ArgumentNullException", "The sample is null.");
        writer.OpenBlock($"public new string ToString({typeName} sample)");
        writer.WriteLine("global::System.ArgumentNullException.ThrowIfNull(sample);");
        writer.BlankLine();
        if (!plan.IsCollection)
        {
            writer.WriteLine($"return {FormatValue(plan.ElementPlan, "sample.Value", quoteStrings: false)};");
            writer.CloseBlock();
            return;
        }

        writer.WriteLine("var value = sample.Value;");
        writer.OpenBlock("if (value is null)");
        writer.WriteLine("return \"null\";");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine("var text = new global::System.Text.StringBuilder();");
        if (plan.IsSequence)
        {
            writer.WriteLine("text.Append('[');");
            writer.OpenBlock("for (var index = 0; index < value.Count; index++)");
            EmitSeparator(writer, "index != 0");
            writer.WriteLine($"text.Append({FormatValue(plan.ElementPlan, "value[index]", quoteStrings: true)});");
            writer.CloseBlock();
            writer.WriteLine("text.Append(']');");
        }
        else
        {
            EmitArrayDimension(writer, plan.ElementPlan, declaration.Dimensions.Count, dimension: 0);
        }

        writer.BlankLine();
        writer.WriteLine("return text.ToString();");
        writer.CloseBlock();

        if (IsStringAlias(plan.ElementPlan))
        {
            writer.BlankLine();
            AliasStringFormattingEmitter.Emit(writer, "FormatString");
        }
    }

    private static void EmitArrayDimension(GeneratedSourceWriter writer, EmissionTypePlan element, int rank, int dimension)
    {
        writer.WriteLine("text.Append('[');");
        writer.OpenBlock($"for (var index{dimension} = value.GetLowerBound({dimension}); index{dimension} <= value.GetUpperBound({dimension}); index{dimension}++)");
        EmitSeparator(writer, $"index{dimension} != value.GetLowerBound({dimension})");
        if (dimension + 1 < rank)
        {
            EmitArrayDimension(writer, element, rank, dimension + 1);
        }
        else
        {
            var indices = string.Join(", ", Enumerable.Range(0, rank).Select(index => $"index{index}"));
            writer.WriteLine($"text.Append({FormatValue(element, $"value[{indices}]", quoteStrings: true)});");
        }

        writer.CloseBlock();
        writer.WriteLine("text.Append(']');");
    }

    private static void EmitSeparator(GeneratedSourceWriter writer, string condition)
    {
        writer.OpenBlock($"if ({condition})");
        writer.WriteLine("text.Append(\", \");");
        writer.CloseBlock();
        writer.BlankLine();
    }

    private static string FormatValue(EmissionTypePlan type, string value, bool quoteStrings)
    {
        if (type is AliasEmissionType alias)
        {
            if (string.Equals(alias.CSharpType, alias.Target.CSharpType, StringComparison.Ordinal))
            {
                return FormatValue(alias.Target, value, quoteStrings);
            }
        }

        if (type is StringEmissionType)
        {
            if (quoteStrings)
            {
                return $"FormatString({value})";
            }

            return $"{value} ?? \"null\"";
        }

        if (type is PrimitiveEmissionType primitive)
        {
            if (primitive.IdlName == "long double")
            {
                return $"{value}.ToDecimal().ToString(global::System.Globalization.CultureInfo.InvariantCulture)";
            }

            return $"global::System.Convert.ToString({value}, global::System.Globalization.CultureInfo.InvariantCulture)";
        }

        if (type is EnumEmissionType)
        {
            return $"{value}.ToString()";
        }

        return $"{value}?.ToString() ?? \"null\"";
    }

    private static bool IsStringAlias(EmissionTypePlan type) =>
        type switch
        {
            StringEmissionType => true,
            AliasEmissionType alias => IsStringAlias(alias.Target),
            _ => false
        };
}
