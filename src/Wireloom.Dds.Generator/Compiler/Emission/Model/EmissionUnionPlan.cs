using System.Globalization;
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
internal sealed class IdlEmissionUnion(string name, string? @namespace, string discriminatorIdlType, string discriminatorCSharpType, bool discriminatorIsEnum, int? discriminatorDefaultValue, IReadOnlyList<UnionBranchEmissionPlan> branches, IdlExtensibilityKind extensibility, string? discriminatorEnumQualifiedName = null)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    private string DiscriminatorIdlType { get; } = discriminatorIdlType;
    public string DiscriminatorCSharpType { get; } = discriminatorCSharpType;
    internal string? DiscriminatorEnumQualifiedName { get; } = discriminatorEnumQualifiedName;
    public bool DiscriminatorIsEnum { get; } = discriminatorIsEnum;
    /// <summary>Gets whether native discriminator conversion uses the RTI character helpers.</summary>
    public bool UsesNativeCharDiscriminator => !DiscriminatorIsEnum && DiscriminatorIdlType == "char";
    private int? DiscriminatorDefaultValue { get; } = discriminatorDefaultValue;
    public IReadOnlyList<UnionBranchEmissionPlan> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    public IReadOnlyList<UnionBranchEmissionPlan> ExplicitBranches =>
        Branches.Where(branch => !branch.IsDefault).ToArray();

    public UnionBranchEmissionPlan? DefaultBranch =>
        Branches.SingleOrDefault(branch => branch.IsDefault);

    /// <summary>Gets whether the generated <c>Get</c> method can return null.</summary>
    public bool GetCanReturnNull =>
        (!IsExhaustiveBoolean && DefaultBranch is null) ||
        Branches.Any(branch => branch.Plan.CSharpType.EndsWith("?", StringComparison.Ordinal));

    /// <summary>Gets whether both boolean discriminator values are covered without a default branch.</summary>
    public bool IsExhaustiveBoolean
    {
        get
        {
            if (DiscriminatorCSharpType != "bool" || DefaultBranch is not null)
            {
                return false;
            }

            var labelValues = ExplicitBranches
                .SelectMany(branch => branch.LabelValues)
                .ToHashSet();

            return labelValues.Contains(0) && labelValues.Contains(1);
        }
    }

    /// <summary>Gets the managed discriminator value selected for the default branch.</summary>
    public string ManagedDefaultDiscriminator => ManagedDiscriminatorValue(FindDefaultDiscriminatorValue());

    /// <summary>Gets the branch selected by the managed default discriminator, if any.</summary>
    public UnionBranchEmissionPlan? DefaultDiscriminatorBranch
    {
        get
        {
            var discriminatorValue = FindDefaultDiscriminatorValue();
            return ExplicitBranches.FirstOrDefault(branch => branch.LabelValues.Contains(discriminatorValue)) ?? DefaultBranch;
        }
    }

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
        if (DiscriminatorCSharpType == "bool")
        {
            return BooleanSelectionCondition(branch, negated, discriminatorName);
        }

        if (!branch.IsDefault)
        {
            var comparison = negated ? "!=" : "==";
            var join = negated ? " && " : " || ";
            return string.Join(join, branch.Labels.Select(label => $"{discriminatorName} {comparison} {ManagedDiscriminatorLabel(label)}"));
        }

        var operators = ExplicitBranches
            .SelectMany(candidate => candidate.Labels)
            .Select(label => $"{discriminatorName} {(negated ? "==" : "!=")} {ManagedDiscriminatorLabel(label)}")
            .ToArray();

        if (operators.Length == 0)
        {
            if (negated)
            {
                return "false";
            }

            return "true";
        }

        if (negated)
        {
            return string.Join(" || ", operators);
        }

        return string.Join(" && ", operators);
    }

    /// <summary>Builds a Boolean branch condition using the discriminator directly.</summary>
    private string BooleanSelectionCondition(UnionBranchEmissionPlan branch, bool negated, string discriminatorName)
    {
        if (!branch.IsDefault)
        {
            var conditions = branch.LabelValues.Select(value =>
            {
                var isTrue = value != 0;
                return isTrue == negated ? $"!{discriminatorName}" : discriminatorName;
            });

            return string.Join(negated ? " && " : " || ", conditions);
        }

        var explicitValues = ExplicitBranches.SelectMany(candidate => candidate.LabelValues).Distinct();
        var defaultConditions = explicitValues.Select(value =>
        {
            var isTrue = value != 0;
            return isTrue == negated ? $"!{discriminatorName}" : discriminatorName;
        }).ToArray();

        if (defaultConditions.Length == 0)
        {
            return negated ? "false" : "true";
        }

        return string.Join(negated ? " || " : " && ", defaultConditions);
    }

    /// <summary>Builds a managed discriminator expression for an IDL label.</summary>
    public string ManagedDiscriminatorLabel(string label) => IdlNaming.TypeReference(label, Namespace);

    public string NativeDiscriminatorType
    {
        get
        {
            if (DiscriminatorIsEnum)
            {
                return DiscriminatorCSharpType;
            }

            return PrimitiveTypeMapping.Resolve(DiscriminatorIdlType).NativeStorageType;
        }
    }

    /// <summary>Gets the RTI dynamic-type primitive name for a primitive discriminator.</summary>
    public string PrimitiveDiscriminatorDynamicType => PrimitiveTypeMapping.Resolve(DiscriminatorIdlType).DynamicType;

    /// <summary>Builds the native-to-managed discriminator conversion expression.</summary>
    public string NativeDiscriminatorReadExpression(string source)
    {
        if (DiscriminatorIsEnum)
        {
            return source;
        }

        return PrimitiveTypeMapping.Resolve(DiscriminatorIdlType).FromNativeExpression(source);
    }

    /// <summary>Builds the managed-to-native discriminator conversion expression.</summary>
    public string NativeDiscriminatorWriteExpression(string source)
    {
        if (DiscriminatorIsEnum)
        {
            return source;
        }

        return PrimitiveTypeMapping.Resolve(DiscriminatorIdlType).ToNativeExpression(source);
    }

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
        _ when DiscriminatorIsEnum && value == 0 => "0",
        _ when DiscriminatorIsEnum => $"({ManagedEnumTypeReference()}){value}",
        _ => value.ToString(CultureInfo.InvariantCulture)
    };

    private string ManagedEnumTypeReference()
    {
        if (DiscriminatorEnumQualifiedName is null)
        {
            return DiscriminatorCSharpType;
        }

        return IdlNaming.ResolvedTypeReference(DiscriminatorEnumQualifiedName, Namespace);
    }
}
