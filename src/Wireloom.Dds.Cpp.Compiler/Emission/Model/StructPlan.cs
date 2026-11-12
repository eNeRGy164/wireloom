using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a validated IDL struct and its C++ namespace projection.</summary>
internal sealed record StructPlan(
    string Name,
    string? Namespace,
    IdlExtensibilityKind Extensibility,
    IReadOnlyList<MemberPlan> Members,
    string? BaseTypeName,
    bool IsXcdr2Only,
    bool IsTopic,
    bool IsNested,
    bool HasPlainLanguageBinding = false,
    bool HasInbandTransferMode = false,
    bool IsFlatData = false,
    bool IsValueType = false)
{
    /// <summary>Gets the canonical IDL type name used in DDS metadata.</summary>
    internal string QualifiedName => string.IsNullOrEmpty(Namespace)
        ? Name
        : $"{Namespace.Replace(".", "::", StringComparison.Ordinal)}::{Name}";

    /// <summary>Gets the absolute C++ type name.</summary>
    internal string CppType => $"::{QualifiedName}";

    /// <summary>Gets the plain helper type used to describe FlatData member layout.</summary>
    internal string FlatDataPlainHelperName => $"{Name}PlainHelper";

    /// <summary>Gets the absolute FlatData plain-helper C++ type.</summary>
    internal string FlatDataPlainHelperType => $"::{QualifiedName}PlainHelper";

    /// <summary>Gets the qualified mutable RTI FlatData offset type.</summary>
    internal string FlatDataOffsetType => $"::{FlatDataNamespace}{Name}Offset";

    /// <summary>Gets the qualified read-only RTI FlatData offset type.</summary>
    internal string FlatDataConstOffsetType => $"::{FlatDataNamespace}{Name}ConstOffset";

    private string FlatDataNamespace => string.IsNullOrEmpty(Namespace)
        ? string.Empty
        : $"{Namespace.Replace(".", "::", StringComparison.Ordinal)}::";

    /// <summary>Gets the qualified IDL base class in C++ spelling, when this declaration inherits.</summary>
    internal string? CppBaseType => BaseTypeName is null ? null : $"::{BaseTypeName}";

    /// <summary>Gets a readable internal C++ identifier scoped to the IDL namespace.</summary>
    internal string ContextIdentifier => $"wireloom_{Name}_type_context";

    /// <summary>Gets whether this struct contains a sequence member of its own type.</summary>
    internal bool HasSelfRecursiveSequence => Members.Any(member =>
        IsSelfRecursiveSequence(member.Type, CppType));

    /// <summary>Gets namespace segments from outermost to innermost.</summary>
    internal IReadOnlyList<string> NamespaceSegments => string.IsNullOrEmpty(Namespace)
        ? []
        : Namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);

    /// <summary>Gets the RTI extensibility expression for this declaration.</summary>
    internal string ExtensibilityExpression => GetExtensibilityExpression(Extensibility);

    /// <summary>Maps bound IDL extensibility to the RTI Modern C++ expression.</summary>
    internal static string GetExtensibilityExpression(IdlExtensibilityKind extensibility) => extensibility switch
    {
        // RTI 7.7.0's Modern C++ API represents IDL appendable types with EXTENSIBLE.
        IdlExtensibilityKind.Appendable => "::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE",
        IdlExtensibilityKind.Final => "::dds::core::xtypes::ExtensibilityKind::FINAL",
        IdlExtensibilityKind.Mutable => "::dds::core::xtypes::ExtensibilityKind::MUTABLE",
        _ => "::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE",
    };

    private static bool IsSelfRecursiveSequence(MemberTypePlan type, string cppType) => type switch
    {
        MemberTypePlan.Sequence sequence => ResolvesToSelf(sequence.ElementType, cppType),
        MemberTypePlan.Alias alias => IsSelfRecursiveSequence(alias.Target, cppType),
        _ => false,
    };

    private static bool ResolvesToSelf(MemberTypePlan type, string cppType)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Struct structure
            && string.Equals($"::{structure.QualifiedName}", cppType, StringComparison.Ordinal);
    }
}
