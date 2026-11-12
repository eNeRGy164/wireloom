using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Cpp.Naming;

/// <summary>Validates IDL names before they are emitted into C++ declarations.</summary>
internal static class IdentifierRules
{
    private static readonly HashSet<string> Keywords = new(StringComparer.Ordinal)
    {
        "alignas", "alignof", "and", "and_eq", "asm", "auto", "bitand", "bitor", "bool", "break",
        "case", "catch", "char", "char16_t", "char32_t", "class", "compl", "const", "constexpr",
        "const_cast", "continue", "decltype", "default", "delete", "do", "double", "dynamic_cast",
        "else", "enum", "explicit", "export", "extern", "false", "float", "for", "friend", "goto",
        "if", "inline", "int", "long", "mutable", "namespace", "new", "noexcept", "not", "not_eq",
        "nullptr", "operator", "or", "or_eq", "private", "protected", "public", "register",
        "reinterpret_cast", "return", "short", "signed", "sizeof", "static", "static_assert", "static_cast",
        "struct", "switch", "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid",
        "typename", "union", "unsigned", "using", "virtual", "void", "volatile", "wchar_t", "while", "xor",
        "xor_eq",
    };

    /// <summary>Adds a source-located diagnostic when an IDL name is reserved in C++.</summary>
    internal static void Validate(string name, IdlInput input, int offset, ICollection<IdlDiagnostic> diagnostics, string kind)
    {
        var isReserved = Keywords.Contains(name)
            || name.Contains("__", StringComparison.Ordinal)
            || (name.Length > 1 && name[0] == '_' && char.IsUpper(name[1]))
            || (kind != "member" && name.StartsWith('_'))
            || (kind != "member" && name.StartsWith("wireloom_", StringComparison.Ordinal))
            || (kind != "member" && name is "std" or "rti" or "dds");
        if (isReserved)
        {
            diagnostics.Add(new IdlDiagnostic("DDSGCPP001", IdlDiagnosticSeverity.Error, input, offset, $"The IDL {kind} name '{name}' conflicts with a reserved C++ or type support name."));
        }
    }

    /// <summary>Gets the IDL-qualified name of a bound struct.</summary>
    internal static string ToIdlQualifiedName(IdlClassDeclaration declaration) =>
        declaration.Namespace is null ? declaration.Name : $"{declaration.Namespace}.{declaration.Name}";

    /// <summary>Joins a declaration's IDL namespace and local name.</summary>
    internal static string QualifiedIdlName(string? @namespace, string name) =>
        string.IsNullOrEmpty(@namespace) ? name : $"{@namespace}.{name}";

    /// <summary>Splits an IDL namespace into its C++ namespace segments.</summary>
    internal static string[] NamespaceSegments(string? @namespace) =>
        string.IsNullOrEmpty(@namespace)
            ? []
            : @namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);

    /// <summary>Converts an IDL-qualified name to C++ scope notation.</summary>
    internal static string ToCppQualifiedName(string idlQualifiedName) =>
        idlQualifiedName.Replace(".", "::", StringComparison.Ordinal);
}
