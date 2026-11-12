using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a supported IDL union value and its discriminator metadata projection.</summary>
internal sealed record UnionPlan(
    string Name,
    string? Namespace,
    IdlExtensibilityKind Extensibility,
    string DiscriminatorType,
    string DiscriminatorMetadataTypeExpression,
    string DiscriminatorDefaultValue,
    string? DiscriminatorEnumQualifiedName,
    IReadOnlyList<UnionBranchPlan> Branches)
{
    internal string QualifiedName => string.IsNullOrEmpty(Namespace)
        ? Name
        : $"{Namespace.Replace(".", "::", StringComparison.Ordinal)}::{Name}";

    internal string CppType => $"::{QualifiedName}";

    internal string ContextIdentifier => $"wireloom_{Name}_union_context";

    internal IReadOnlyList<string> NamespaceSegments => string.IsNullOrEmpty(Namespace)
        ? []
        : Namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);

    internal string ExtensibilityExpression => StructPlan.GetExtensibilityExpression(Extensibility);
}

/// <summary>Represents a union branch with resolved value storage and discriminator labels.</summary>
internal sealed record UnionBranchPlan(
    string Name,
    MemberTypePlan Type,
    IReadOnlyList<long> Labels,
    IReadOnlyList<long> MetadataLabels,
    IReadOnlyList<string> LabelExpressions,
    bool IsDefault,
    string DefaultBranchAccessDiscriminator)
{
    /// <summary>Gets the parameter type shared by the declaration and definition of a branch setter.</summary>
    internal string SetterParameterType(bool useRvalue, bool isRti612 = false)
    {
        if (isRti612 && Type is MemberTypePlan.String)
        {
            return useRvalue ? "::std::string&&" : "const ::std::string&";
        }

        if (Type is MemberTypePlan.Primitive or MemberTypePlan.Enumeration)
        {
            return Type.StorageType;
        }

        var typeName = Type is MemberTypePlan.String
            ? "::omg::types::string_view"
            : Type.StorageType;
        return useRvalue ? $"{typeName}&&" : $"const {typeName}&";
    }
}
