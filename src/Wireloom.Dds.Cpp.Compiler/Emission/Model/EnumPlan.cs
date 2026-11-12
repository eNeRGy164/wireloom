using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a validated IDL enum and its Modern C++ type metadata.</summary>
internal sealed record EnumPlan(
    string Name,
    string? Namespace,
    IdlExtensibilityKind Extensibility,
    IReadOnlyList<EnumMemberPlan> Members)
{
    /// <summary>Gets the canonical IDL type name used in DDS metadata.</summary>
    internal string QualifiedName => string.IsNullOrEmpty(Namespace)
        ? Name
        : $"{Namespace.Replace(".", "::", StringComparison.Ordinal)}::{Name}";

    /// <summary>Gets the canonical name used by the bound IDL symbol model.</summary>
    internal string IdlQualifiedName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

    /// <summary>Gets the absolute C++ type name.</summary>
    internal string CppType => $"::{QualifiedName}";

    /// <summary>Gets a readable internal C++ identifier for the enum type context.</summary>
    internal string ContextIdentifier => $"wireloom_{Name}_enum_type_context";

    /// <summary>Gets namespace segments from outermost to innermost.</summary>
    internal IReadOnlyList<string> NamespaceSegments => string.IsNullOrEmpty(Namespace)
        ? []
        : Namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);

    /// <summary>Gets the default enumerator selected by IDL and RTI.</summary>
    internal EnumMemberPlan DefaultMember => Members.First(member => member.IsDefault);

    /// <summary>Gets the RTI extensibility expression for this declaration.</summary>
    internal string ExtensibilityExpression => StructPlan.GetExtensibilityExpression(Extensibility);
}

/// <summary>Represents one enumerator and its validated ordinal.</summary>
internal sealed record EnumMemberPlan(string Name, int Value, bool HasExplicitValue, bool IsDefault);
