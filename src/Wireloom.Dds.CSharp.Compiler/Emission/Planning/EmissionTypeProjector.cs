using Wireloom.Dds.CSharp.Compiler.Emission.Model;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;
using Wireloom.Dds.CSharp.Compiler.Naming;

namespace Wireloom.Dds.CSharp.Compiler.Emission.Planning;

/// <summary>Projects semantic IDL types into target-specific emission plans.</summary>
internal static class EmissionTypeProjector
{
    /// <summary>Projects a semantic member into an emission field.</summary>
    internal static IdlEmissionField ToEmissionField(IdlMember member, string? currentNamespace, SequenceArrayEmissionPlan? sequenceArrays = null, RtiApiProfile? profile = null)
    {
        var mappedType = sequenceArrays?.GetMemberType(member);
        var declaredType = mappedType ?? ProjectType(member.Type, currentNamespace, profile);
        var type = mappedType ?? ProjectMemberType(member.Type, currentNamespace, profile);
        var cSharpType = type.CSharpType;

        if (member.Metadata.IsOptional && type is StringEmissionType)
        {
            cSharpType = "string?";
        }
        else if (member.Metadata.IsOptional && type is SequenceEmissionType or ArrayEmissionType)
        {
            cSharpType += "?";
        }
        else if (member.Metadata.IsOptional && type is not SequenceEmissionType and not ArrayEmissionType)
        {
            cSharpType += "?";
        }

        if (!string.Equals(cSharpType, type.CSharpType, StringComparison.Ordinal))
        {
            type = new OptionalEmissionType(type, cSharpType);
        }

        return new IdlEmissionField(
            member.Name,
            type,
            new EmissionMetadata(
                member.Metadata.IsKey,
                member.Metadata.MemberId,
                member.Metadata.IsOptional,
                member.Metadata.ValueMetadata,
                member.Metadata.IsExternal,
                member.Metadata.IsMustUnderstand,
                member.Metadata.MemberIdHashSource,
                member.Metadata.UsesAutoIdHash,
                member.Metadata.EffectiveMemberId),
            declaredType);
    }

    /// <summary>Projects a collection element using the RTI C# alias mapping.</summary>
    internal static EmissionTypePlan ToCollectionElementType(IdlType type, string? currentNamespace, RtiApiProfile? profile = null)
        => ProjectMemberType(type, currentNamespace, profile);

    /// <summary>Projects a typedef value while preserving each named alias identity.</summary>
    internal static EmissionTypePlan ToTypedefValueType(IdlType type, string? currentNamespace, RtiApiProfile? profile = null)
    {
        if (type is IdlType.Alias alias)
        {
            var target = ToTypedefValueType(alias.Target, currentNamespace, profile);
            var aliasName = IdlNaming.ResolvedTypeReference(alias.QualifiedName, currentNamespace);
            var supportName = IdlNaming.SupportTypeName(alias.QualifiedName, currentNamespace);
            return new AliasEmissionType(alias.QualifiedName, target, aliasName, supportName);
        }

        return ProjectType(type, currentNamespace, profile);
    }

    /// <summary>Projects a semantic union into an emission union.</summary>
    internal static IdlEmissionUnion ToEmissionUnion(IdlUnion union, RtiApiProfile profile, SequenceArrayEmissionPlan? sequenceArrays = null) =>
        new(
            union.Name,
            union.Namespace,
            ProjectDiscriminatorIdlType(union),
            ProjectDiscriminatorCSharpType(union, profile),
            union.DiscriminatorIsEnum,
            union.DiscriminatorDefaultValue,
            union.DiscriminatorFirstValue,
            [.. union.Branches.Select(branch =>
            {
                var field = ToEmissionField(branch.Field, union.Namespace, sequenceArrays, profile);
                return new UnionBranchEmissionPlan(
                    field,
                    new MemberEmissionPlan(field, union.Namespace, profile: profile),
                    ProjectUnionLabels(union, branch),
                    branch.LabelValues,
                    branch.IsDefault);
            })],
            union.Extensibility,
            union.DiscriminatorEnumQualifiedName,
            profile);

    private static string ProjectDiscriminatorIdlType(IdlUnion union)
    {
        if (union.DiscriminatorIsEnum)
        {
            return union.DiscriminatorIdlType;
        }

        return union.DiscriminatorPrimitiveIdlType!;
    }

    private static string ProjectDiscriminatorCSharpType(IdlUnion union, RtiApiProfile profile)
    {
        if (union.DiscriminatorIsEnum)
        {
            return IdlNaming.EscapeQualifiedIdentifier(union.DiscriminatorEnumQualifiedName!);
        }

        return IdlNaming.MapPrimitive(union.DiscriminatorPrimitiveIdlType!, profile);
    }

    private static IReadOnlyList<string> ProjectUnionLabels(IdlUnion union, IdlUnionBranch branch) =>
        branch.Labels.Select(label => ProjectUnionLabel(union, label)).ToArray();

    private static string ProjectUnionLabel(IdlUnion union, string label)
    {
        if (union.DiscriminatorIsEnum)
        {
            var discriminatorType = IdlNaming.ResolvedTypeReference(union.DiscriminatorEnumQualifiedName!, union.Namespace);
            return $"{discriminatorType}.{IdlNaming.EscapeIdentifier(label)}";
        }

        if (union.DiscriminatorPrimitiveIdlType == "boolean")
        {
            if (label == "TRUE")
            {
                return "true";
            }

            return "false";
        }

        if (union.DiscriminatorPrimitiveIdlType == "wchar"
            && label.StartsWith("L'", StringComparison.Ordinal))
        {
            return label[1..];
        }

        return label;
    }

    private static EmissionTypePlan ProjectType(IdlType type, string? currentNamespace, RtiApiProfile? profile) =>
        type switch
        {
            IdlType.Primitive primitive => new PrimitiveEmissionType(IdlNaming.NormalizeIdlType(primitive.Name), IdlNaming.MapPrimitive(primitive.Name, profile), profile),
            IdlType.StringType stringType => new StringEmissionType(stringType.IsWide, stringType.Bound, stringType.IsBounded, stringType.IsUnbounded),
            IdlType.Enum @enum => new EnumEmissionType(
                IdlNaming.ResolvedTypeReference(@enum.QualifiedName, currentNamespace),
                @enum.DefaultValue,
                @enum.DefaultMemberName,
                IdlNaming.SupportTypeName(@enum.QualifiedName, currentNamespace)),
            IdlType.Struct structure => new StructEmissionType(
                IdlNaming.ResolvedTypeReference(structure.QualifiedName, currentNamespace),
                IdlNaming.SupportTypeName(structure.QualifiedName, currentNamespace)),
            IdlType.Union union => new UnionEmissionType(
                IdlNaming.ResolvedTypeReference(union.QualifiedName, currentNamespace),
                IdlNaming.SupportTypeName(union.QualifiedName, currentNamespace)),
            IdlType.Alias alias => ProjectAlias(alias, currentNamespace, profile),
            IdlType.Sequence sequence => ProjectSequence(sequence, currentNamespace, profile),
            IdlType.Array array => ProjectArray(array, currentNamespace, profile),
            _ => throw new InvalidOperationException($"Unknown semantic IDL type: {type.GetType().Name}")
        };

    private static EmissionTypePlan ProjectMemberType(IdlType type, string? currentNamespace, RtiApiProfile? profile)
    {
        var memberType = ProjectType(type, currentNamespace, profile);
        while (memberType is AliasEmissionType alias && RequiresUnderlyingMemberType(alias.Target))
        {
            memberType = alias.Target;
        }

        return memberType;
    }

    private static EmissionTypePlan ProjectAlias(IdlType.Alias alias, string? currentNamespace, RtiApiProfile? profile)
    {
        var target = ProjectType(alias.Target, currentNamespace, profile);
        var aliasName = IdlNaming.ResolvedTypeReference(alias.QualifiedName, currentNamespace);
        var supportName = IdlNaming.SupportTypeName(alias.QualifiedName, currentNamespace);
        var cSharpType = target is SequenceEmissionType or ArrayEmissionType || IsAggregateEmissionType(target)
            ? aliasName
            : target.CSharpType;

        return new AliasEmissionType(alias.QualifiedName, target, cSharpType, supportName);
    }

    private static SequenceEmissionType ProjectSequence(IdlType.Sequence sequence, string? currentNamespace, RtiApiProfile? profile)
    {
        var element = ProjectMemberType(sequence.Element, currentNamespace, profile);

        return new SequenceEmissionType(element, sequence.EffectiveBound, $"ISequence<{element.CSharpType}>", sequence.Dimensions,
            isBounded: sequence.Bound is not null, isUnbounded: sequence.IsUnbounded);
    }

    private static ArrayEmissionType ProjectArray(IdlType.Array array, string? currentNamespace, RtiApiProfile? profile)
    {
        var element = ProjectMemberType(array.Element, currentNamespace, profile);

        return new ArrayEmissionType(element, array.Dimensions, BuildArrayType(element.CSharpType, array.Dimensions));
    }

    private static bool IsAggregateEmissionType(EmissionTypePlan type) => type switch
    {
        StructEmissionType or UnionEmissionType => true,
        AliasEmissionType alias => alias.Target is SequenceEmissionType or ArrayEmissionType || IsAggregateEmissionType(alias.Target),
        _ => false
    };

    private static bool RequiresUnderlyingMemberType(EmissionTypePlan type) => type switch
    {
        StructEmissionType or UnionEmissionType => true,
        AliasEmissionType alias => IsAggregateEmissionType(alias),
        _ => false
    };

    private static EmissionTypePlan UnwrapOptionalEmissionType(EmissionTypePlan type) =>
        type is OptionalEmissionType optional ? optional.Target : type;

    /// <summary>Unwraps optional and alias plans to their underlying value plan.</summary>
    internal static EmissionTypePlan UnwrapValueEmissionType(EmissionTypePlan type)
    {
        type = UnwrapOptionalEmissionType(type);

        return type is AliasEmissionType alias ? UnwrapValueEmissionType(alias.Target) : type;
    }

    /// <summary>Determines whether a plan contains a sequence type.</summary>
    internal static bool HasSequenceType(EmissionTypePlan type)
    {
        type = UnwrapOptionalEmissionType(type);

        return type switch
        {
            SequenceEmissionType => true,
            AliasEmissionType alias => HasSequenceType(alias.Target),
            _ => false
        };
    }

    private static string BuildArrayType(string elementType, IReadOnlyList<int> dimensions) =>
        $"{elementType}[{new string(',', dimensions.Count - 1)}]";
}
