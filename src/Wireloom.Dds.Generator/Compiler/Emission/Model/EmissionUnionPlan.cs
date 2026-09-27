using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Semantics;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Describes one resolved union branch for the source emitters.</summary>
internal sealed class UnionBranchEmissionPlan(IdlEmissionField field, MemberEmissionPlan plan, IReadOnlyList<string> labels, IReadOnlyList<int> labelValues, bool isDefault)
{
    public IdlEmissionField Field { get; } = field;
    public MemberEmissionPlan Plan { get; } = plan;
    public IReadOnlyList<string> Labels { get; } = labels;
    public IReadOnlyList<int> LabelValues { get; } = labelValues;
    public bool IsDefault { get; } = isDefault;
}

/// <summary>Describes an IDL union after projection to the generated target model.</summary>
internal sealed class IdlEmissionUnion(string name, string? @namespace, string discriminatorCSharpType, bool discriminatorIsEnum, IReadOnlyList<UnionBranchEmissionPlan> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorCSharpType { get; } = discriminatorCSharpType;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    public IReadOnlyList<UnionBranchEmissionPlan> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
}
