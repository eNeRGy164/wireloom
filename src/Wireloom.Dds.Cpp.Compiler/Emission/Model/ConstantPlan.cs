namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a validated scalar or enum IDL constant in its C++ namespace.</summary>
internal sealed record ConstantPlan(
    string Name,
    string? Namespace,
    string StorageType,
    string ValueExpression,
    bool IsEnum = false)
{
    /// <summary>Gets namespace segments from outermost to innermost.</summary>
    internal IReadOnlyList<string> NamespaceSegments => string.IsNullOrEmpty(Namespace)
        ? []
        : Namespace.Replace(".", "::", StringComparison.Ordinal).Split(["::"], StringSplitOptions.None);
}
