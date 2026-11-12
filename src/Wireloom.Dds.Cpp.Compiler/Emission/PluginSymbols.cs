using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Keeps generated plugin context and callback names consistent across renderers.</summary>
internal static class PluginSymbols
{
    /// <summary>Gets the shared name prefix for the sequence interpreter bridge.</summary>
    internal static string StlSequencePluginPrefix(StructPlan declaration) =>
        $"wireloom_{declaration.Name}_stl_sequence";

    /// <summary>Gets the shared name prefix for the string interpreter bridge.</summary>
    internal static string StlStringPluginPrefix(StructPlan declaration) =>
        $"wireloom_{declaration.Name}_stl_string";

    /// <summary>Gets the shared name prefix for the union interpreter bridge.</summary>
    internal static string StlUnionPluginPrefix(UnionPlan declaration) =>
        $"wireloom_{declaration.Name}_stl_union";

    /// <summary>Gets the qualified accessor name for a union DynamicType.</summary>
    internal static string UnionTypeAccessorName(string qualifiedType) =>
        QualifiedUnionAccessorName(qualifiedType, "union_type");

    /// <summary>Gets the qualified accessor name for a union native TypeCode.</summary>
    internal static string UnionNativeTypeCodeAccessorName(string qualifiedType) =>
        QualifiedUnionAccessorName(qualifiedType, "union_native_type_code");

    /// <summary>Gets the unqualified accessor name for a union DynamicType.</summary>
    internal static string UnionTypeAccessorLocalName(string name) => $"wireloom_{name}_union_type";

    /// <summary>Gets the unqualified accessor name for a union native TypeCode.</summary>
    internal static string UnionNativeTypeCodeAccessorLocalName(string name) =>
        $"wireloom_{name}_union_native_type_code";

    private static string QualifiedUnionAccessorName(string qualifiedType, string suffix)
    {
        var parts = qualifiedType.Split("::", StringSplitOptions.RemoveEmptyEntries);
        var name = parts[^1];
        var @namespace = string.Join("::", parts[..^1]);
        var accessor = $"wireloom_{name}_{suffix}";
        return string.IsNullOrEmpty(@namespace) ? $"::{accessor}" : $"::{@namespace}::{accessor}";
    }

    /// <summary>Creates indentation for generated sample-access metadata.</summary>
    internal static string Spaces(int count) => new(' ', count);

    /// <summary>Writes a call through a generated plugin context and wraps it when needed.</summary>
    internal static void WriteContextMemberCall(
        SourceWriter source,
        string contextFunctionName,
        string memberExpression,
        int indentation)
    {
        var prefix = new string(' ', indentation);
        var singleLine = $"{prefix}{contextFunctionName}().{memberExpression}";
        if (singleLine.Length <= 120)
        {
            source.WriteLine(singleLine);
            return;
        }

        source.WriteLine($"{prefix}{contextFunctionName}()");
        source.WriteLine($"{prefix}    .{memberExpression}");
    }

    /// <summary>Gets the qualified function name that returns a native plugin context.</summary>
    internal static string ContextFunctionName(StructPlan declaration) =>
        string.IsNullOrEmpty(declaration.Namespace)
            ? $"::{declaration.ContextIdentifier}_instance"
            : $"::{declaration.Namespace.Replace(".", "::", StringComparison.Ordinal)}::{declaration.ContextIdentifier}_instance";

    /// <summary>Gets the qualified function name that returns a native plugin context.</summary>
    internal static string ContextFunctionName(EnumPlan declaration) =>
        string.IsNullOrEmpty(declaration.Namespace)
            ? $"::{declaration.ContextIdentifier}_instance"
            : $"::{declaration.Namespace.Replace(".", "::", StringComparison.Ordinal)}::{declaration.ContextIdentifier}_instance";

    /// <summary>Gets the qualified function name that returns a native plugin context.</summary>
    internal static string ContextFunctionName(UnionPlan declaration) =>
        string.IsNullOrEmpty(declaration.Namespace)
            ? $"::{declaration.ContextIdentifier}_instance"
            : $"::{declaration.Namespace.Replace(".", "::", StringComparison.Ordinal)}::{declaration.ContextIdentifier}_instance";

    /// <summary>Gets the native TypeCode helper name for an aggregate typedef.</summary>
    internal static string AggregateAliasTypeCodeFunctionName(string cppType) =>
        "wireloom_alias_type_code_"
        + new string(cppType.Select(character => char.IsLetterOrDigit(character) ? character : '_').ToArray());

    /// <summary>Formats a generated type-context pointer for a planned named type.</summary>
    internal static string ContextTypePointer(
        string qualifiedType,
        string? ownerQualifiedName,
        bool optionalSelfRecursiveArray)
    {
        var separator = qualifiedType.LastIndexOf("::", StringComparison.Ordinal);
        var @namespace = separator < 0 ? string.Empty : qualifiedType[..separator];
        var name = separator < 0 ? qualifiedType : qualifiedType[(separator + 2)..];
        if (optionalSelfRecursiveArray
            || ownerQualifiedName is not null
            && string.Equals(qualifiedType, ownerQualifiedName, StringComparison.Ordinal))
        {
            return "type";
        }

        return string.IsNullOrEmpty(@namespace)
            ? $"::wireloom_{name}_type_context_instance().type"
            : $"::{@namespace}::wireloom_{name}_type_context_instance().type";
    }

    /// <summary>Writes the typed CDR buffer signature, wrapping long type names.</summary>
    internal static void WriteCdrBufferSignature(SourceWriter source, string typeName)
    {
        var signature = $"std::vector<char>& topic_type_support<{typeName}>::to_cdr_buffer(";
        if (signature.Length <= 120)
        {
            source.WriteLine(signature);
            return;
        }

        source.WriteLine("std::vector<char>&");
        source.WriteLine($"topic_type_support<{typeName}>::to_cdr_buffer(");
    }

    /// <summary>Writes the registration call for an interpreter-backed plugin.</summary>
    internal static void WriteInterpreterRegistration(SourceWriter source, string? @namespace, string pluginPrefix)
    {
        var namespacePrefix = string.IsNullOrEmpty(@namespace)
            ? "::"
            : "::" + @namespace.Replace(".", "::", StringComparison.Ordinal) + "::";
        source.WriteLine("    ::rti::domain::register_type_plugin(");
        source.WriteLine("        participant, name,");
        source.WriteLine($"        &{namespacePrefix}{pluginPrefix}_create_plugin,");
        source.WriteLine($"        &{namespacePrefix}{pluginPrefix}_delete_plugin);");
    }
}
