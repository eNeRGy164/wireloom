using System.Globalization;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Validates and projects IDL unions, discriminators, and branches into C++ plans.</summary>
internal static class UnionPlanner
{
    /// <summary>Validates and projects supported union declarations in source order.</summary>
    internal static IReadOnlyList<UnionPlan> PlanUnions(
        IReadOnlyList<IdlUnionDeclaration> unionDeclarations,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics,
        MemberTypeResolver typeResolver)
    {
        var unionPlans = new List<UnionPlan>(unionDeclarations.Count);
        foreach (var unionDeclaration in unionDeclarations)
        {
            var union = unionDeclaration.Declaration;
            IdentifierRules.Validate(union.Name, input, union.DiscriminatorOffset, diagnostics, "union");
            foreach (var segment in IdentifierRules.NamespaceSegments(union.Namespace))
            {
                IdentifierRules.Validate(segment, input, union.DiscriminatorOffset, diagnostics, "namespace");
            }

            ProfileValidation.AddUnsupportedAnnotations(unionDeclaration.Annotations,
                ["final", "appendable", "mutable", "extensibility"], input, diagnostics, union.Name);
            var branches = new List<UnionBranchPlan>(union.Branches.Count);
            var explicitLabels = union.Branches.SelectMany(branch => branch.LabelValues).ToHashSet();
            var discriminatorEnum = union.DiscriminatorEnumQualifiedName is { } enumName
                ? typeResolver.Enums.GetValueOrDefault(enumName)
                : null;
            var discriminatorPrimitive = NormalizeIdlType(union.DiscriminatorPrimitiveIdlType ?? string.Empty);
            if (discriminatorPrimitive == "wchar")
            {
                ProfileValidation.AddError(diagnostics, input, union.DiscriminatorOffset,
                    "The C++ generator does not support 'wchar' union discriminators, matching RTI Connext DDS 7.7.0 rtiddsgen's parser rejection.");
                continue;
            }

            var discriminatorType = discriminatorEnum?.CppType ?? discriminatorPrimitive switch
            {
                "boolean" => "bool",
                "long" or "int32" => "::std::int32_t",
                "short" or "int16" => "::std::int16_t",
                "unsigned long" or "uint32" => "::std::uint32_t",
                "unsigned short" or "uint16" => "::std::uint16_t",
                "octet" or "uint8" => "::std::uint8_t",
                "int8" => "::std::int8_t",
                "char" => "char",
                _ => string.Empty,
            };
            var (minimumDiscriminator, maximumDiscriminator) = discriminatorPrimitive switch
            {
                "boolean" => (0L, 1L),
                "short" or "int16" => (short.MinValue, short.MaxValue),
                "unsigned short" or "uint16" => (0L, ushort.MaxValue),
                "octet" or "uint8" => (0L, byte.MaxValue),
                "int8" => (sbyte.MinValue, sbyte.MaxValue),
                "long" or "int32" => (int.MinValue, int.MaxValue),
                "unsigned long" or "uint32" => (0L, uint.MaxValue),
                // The C++ basic execution character set is representable in plain char.
                // Keep default selection in ASCII so char signedness cannot narrow labels.
                "char" => (0L, 127L),
                _ => (0L, int.MaxValue),
            };
            var defaultDiscriminator = FindDefaultDiscriminator(
                explicitLabels, minimumDiscriminator, maximumDiscriminator);
            if (defaultDiscriminator is null && union.Branches.Any(branch => branch.IsDefault))
            {
                ProfileValidation.AddError(diagnostics, input, union.DiscriminatorOffset,
                    $"The C++ union slice cannot select an unused non-negative discriminator for default branch on '{union.Name}'.");
                continue;
            }

            foreach (var branch in union.Branches)
            {
                var field = branch.Field;
                IdentifierRules.Validate(field.Name, field.SourceInput ?? input, field.SourceOffset, diagnostics, "union branch");
                ProfileValidation.AddUnsupportedAnnotations(field.Annotations, [], field.SourceInput ?? input, diagnostics, field.Name);
                if (!typeResolver.TryResolveMemberType(field.Type, out var branchType)
                    || branchType is MemberTypePlan.Union)
                {
                    ProfileValidation.AddError(diagnostics, field.SourceInput ?? input, field.SourceOffset,
                        $"The C++ generator does not support union branch '{field.Name}' with this type.");
                    continue;
                }

                var labelExpressions = branch.LabelValues
                    .Select(label => LiteralFormatter.FormatUnionLabelExpression(label, discriminatorEnum, discriminatorPrimitive))
                    .ToArray();
                var metadataLabels = branch.LabelValues
                    .Select(label => NormalizeMetadataLabel(label, discriminatorPrimitive))
                    .ToArray();
                var defaultBranchAccessDiscriminator = branch.IsDefault
                    ? (defaultDiscriminator ?? 0).ToString(CultureInfo.InvariantCulture)
                    : string.Empty;
                if (branch.IsDefault && discriminatorEnum is not null)
                {
                    defaultBranchAccessDiscriminator = $"static_cast<{discriminatorEnum.CppType}>({defaultBranchAccessDiscriminator})";
                }

                branches.Add(new UnionBranchPlan(field.Name, branchType, branch.LabelValues,
                    metadataLabels, labelExpressions, branch.IsDefault, defaultBranchAccessDiscriminator));
            }

            if (discriminatorType.Length == 0)
            {
                ProfileValidation.AddError(diagnostics, input, union.DiscriminatorOffset,
                    $"The C++ generator does not support union discriminator '{union.DiscriminatorIdlType}' in this slice.");
            }

            if (union.Branches.Count(branch => branch.IsDefault) > 1)
            {
                ProfileValidation.AddError(diagnostics, input, union.DiscriminatorOffset,
                    $"The C++ union slice supports at most one default branch on '{union.Name}'.");
            }

            if (branches.Count == union.Branches.Count && discriminatorType.Length > 0)
            {
                var discriminatorMetadataType = discriminatorEnum is not null
                    ? discriminatorEnum.CppType
                    : PrimitiveType.TryResolve(new IdlType.Primitive(discriminatorPrimitive), out var primitiveMapping)
                        ? primitiveMapping.MetadataExpression
                        : discriminatorType;
                var discriminatorDefault = discriminatorEnum is null
                    ? discriminatorPrimitive == "boolean" ? "false" : "0"
                    : $"::{discriminatorEnum.QualifiedName}::{discriminatorEnum.DefaultMember.Name}";
                unionPlans.Add(new UnionPlan(union.Name, union.Namespace, union.Extensibility,
                    discriminatorType, discriminatorMetadataType, discriminatorDefault, discriminatorEnum?.QualifiedName, branches));
            }
        }

        return unionPlans;
    }

    private static long? FindDefaultDiscriminator(
        IReadOnlySet<long> explicitLabels,
        long minimum,
        long maximum)
    {
        // At most one more candidate than there are labels is needed to find a gap.
        // Keeping the search bounded also avoids walking large uint32 domains.
        var attempts = explicitLabels.Count + 1;
        var firstCandidate = Math.Max(0, minimum);
        for (long offset = 0; offset < attempts; offset++)
        {
            var candidate = firstCandidate + offset;
            if (candidate > maximum)
            {
                break;
            }

            if (!explicitLabels.Contains(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static long NormalizeMetadataLabel(long label, string discriminatorPrimitive)
    {
        // RTI's internal TypeCode label is signed for IDL char values above 127,
        // while the generated discriminator and serialized byte retain the IDL value.
        return discriminatorPrimitive == "char" && label is >= 128 and <= byte.MaxValue
            ? label - 256
            : label;
    }

    private static string NormalizeIdlType(string idlType) =>
        string.Join(" ", idlType.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}
