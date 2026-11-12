namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a supported typedef, its declaration target, and its resolved metadata type.</summary>
internal sealed record AliasPlan(
    string Name,
    string? Namespace,
    string TargetType,
    string? StructTargetType,
    MemberTypePlan ResolvedType,
    MemberTypePlan MetadataTargetType)
{
    /// <summary>Gets the canonical IDL name.</summary>
    internal string QualifiedName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

    /// <summary>Gets the C++ namespace segments.</summary>
    internal IReadOnlyList<string> NamespaceSegments => string.IsNullOrEmpty(Namespace)
        ? []
        : Namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);

    /// <summary>Gets the C++ qualified name.</summary>
    internal string CppType => $"::{QualifiedName.Replace(".", "::", StringComparison.Ordinal)}";

    /// <summary>Gets the RTI DynamicType projection that retains this typedef's identity.</summary>
    internal MemberTypePlan MetadataType => new MemberTypePlan.Alias(MetadataTargetType, CppType);
}
