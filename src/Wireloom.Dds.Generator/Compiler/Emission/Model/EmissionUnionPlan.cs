using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

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
internal sealed class IdlEmissionUnion(string name, string? @namespace, string discriminatorCSharpType, bool discriminatorIsEnum, int? discriminatorDefaultValue, IReadOnlyList<UnionBranchEmissionPlan> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorCSharpType { get; } = discriminatorCSharpType;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    private int? DiscriminatorDefaultValue { get; } = discriminatorDefaultValue;
    public IReadOnlyList<UnionBranchEmissionPlan> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    public IReadOnlyList<UnionBranchEmissionPlan> ExplicitBranches =>
        Branches.Where(branch => !branch.IsDefault).ToArray();

    public UnionBranchEmissionPlan? DefaultBranch =>
        Branches.SingleOrDefault(branch => branch.IsDefault);

    public bool IsExhaustiveBoolean =>
        DiscriminatorCSharpType == "bool" && DefaultBranch is null;

    /// <summary>Gets the managed discriminator value selected for the default branch.</summary>
    public string ManagedDefaultDiscriminator => ManagedDiscriminatorValue(FindDefaultDiscriminatorValue());

    /// <summary>Builds the managed discriminator expression for a union branch.</summary>
    public string BranchDiscriminator(UnionBranchEmissionPlan branch)
    {
        if (branch.IsDefault)
        {
            return ManagedDiscriminatorValue(FindUnoccupiedDiscriminatorValue());
        }

        return ManagedDiscriminatorLabel(branch.Labels[0]);
    }

    /// <summary>Builds the branch-selection condition for the supplied discriminator.</summary>
    public string SelectionCondition(UnionBranchEmissionPlan branch, bool negated, string discriminatorName = "Discriminator")
    {
        if (!branch.IsDefault)
        {
            var comparison = negated ? "!=" : "==";
            var join = negated ? " && " : " || ";
            return string.Join(join, branch.Labels.Select(label => $"{discriminatorName} {comparison} {ManagedDiscriminatorLabel(label)}"));
        }

        var operators = ExplicitBranches
            .SelectMany(candidate => candidate.Labels)
            .Select(label => $"{discriminatorName} {(negated ? "==" : "!=")} {ManagedDiscriminatorLabel(label)}");
        return string.Join(negated ? " || " : " && ", operators);
    }

    /// <summary>Builds a managed discriminator expression for an IDL label.</summary>
    public string ManagedDiscriminatorLabel(string label) => IdlNaming.TypeReference(label, Namespace);

    public string NativeDiscriminatorType => DiscriminatorCSharpType is "char" or "bool" ? "byte" : DiscriminatorCSharpType;

    /// <summary>Builds the native-to-managed discriminator conversion expression.</summary>
    public string NativeDiscriminatorReadExpression(string source) => DiscriminatorCSharpType switch
    {
        "char" => $"NativeChar.FromUtf8({source})",
        "bool" => $"global::System.Convert.ToBoolean({source})",
        _ => source
    };

    /// <summary>Builds the managed-to-native discriminator conversion expression.</summary>
    public string NativeDiscriminatorWriteExpression(string source) => DiscriminatorCSharpType switch
    {
        "char" => $"NativeChar.ToUtf8({source})",
        "bool" => $"global::System.Convert.ToByte({source})",
        _ => source
    };

    private int FindDefaultDiscriminatorValue()
    {
        if (DiscriminatorIsEnum)
        {
            return DiscriminatorDefaultValue ?? 0;
        }

        if (DefaultBranch is null)
        {
            return 0;
        }

        return FindUnoccupiedDiscriminatorValue();
    }

    private int FindUnoccupiedDiscriminatorValue()
    {
        var occupied = ExplicitBranches.SelectMany(branch => branch.LabelValues).Distinct().OrderBy(value => value).ToArray();
        var (minimum, maximum) = DiscriminatorRange();
        var candidate = FindFirstUnoccupiedValue(occupied, 0, maximum) ?? FindFirstUnoccupiedValue(occupied, minimum, -1);
        return candidate ?? throw new InvalidOperationException($"Union '{Name}' has no representable default discriminator.");
    }

    private static int? FindFirstUnoccupiedValue(IReadOnlyList<int> occupied, int minimum, int maximum)
    {
        if (minimum > maximum)
        {
            return null;
        }

        long candidate = minimum;

        foreach (var occupiedValue in occupied)
        {
            if (occupiedValue < minimum)
            {
                continue;
            }

            if (occupiedValue > maximum)
            {
                break;
            }

            if (occupiedValue > candidate)
            {
                return (int)candidate;
            }

            if (occupiedValue == candidate)
            {
                candidate++;
            }
        }

        return candidate <= maximum ? (int)candidate : null;
    }

    private (int Minimum, int Maximum) DiscriminatorRange() => DiscriminatorCSharpType switch
    {
        "bool" => (0, 1),
        "char" => (char.MinValue, char.MaxValue),
        "sbyte" => (sbyte.MinValue, sbyte.MaxValue),
        "byte" => (byte.MinValue, byte.MaxValue),
        "short" => (short.MinValue, short.MaxValue),
        "ushort" => (ushort.MinValue, ushort.MaxValue),
        _ => (int.MinValue, int.MaxValue)
    };

    private string ManagedDiscriminatorValue(int value) => DiscriminatorCSharpType switch
    {
        "bool" => value == 0 ? "false" : "true",
        "char" when value == 0 => "'\\0'",
        "char" => $"(char){value}",
        _ when DiscriminatorIsEnum => $"({IdlNaming.TypeReference(DiscriminatorCSharpType, Namespace)}){value}",
        _ => value.ToString()
    };
}
