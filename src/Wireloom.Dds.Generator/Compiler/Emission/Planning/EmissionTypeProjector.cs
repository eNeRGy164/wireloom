using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Projects semantic IDL types into target-specific emission plans.</summary>
internal static class EmissionTypeProjector
{
    /// <summary>Projects a semantic member into an emission field.</summary>
    internal static IdlEmissionField ToEmissionField(IdlMember member, string? currentNamespace)
    {
        var type = ProjectType(member.Type, currentNamespace);
        var cSharpType = type.CSharpType;

        if (member.Metadata.IsOptional && type is StringEmissionType)
        {
            cSharpType = "string?";
        }
        else if (member.Metadata.IsOptional && type is SequenceEmissionType or ArrayEmissionType)
        {
            cSharpType += "?";
        }
        else if (member.Metadata.IsOptional && type is not SequenceEmissionType and not ArrayEmissionType && !IsAggregateEmissionType(type))
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
                member.Metadata.UsesAutoIdHash));
    }

    /// <summary>Projects a collection element while preserving a named alias identity.</summary>
    internal static EmissionTypePlan ToCollectionElementType(IdlType type, string? currentNamespace)
    {
        if (type is IdlType.Alias alias)
        {
            var target = ToCollectionElementType(alias.Target, currentNamespace);
            var aliasName = IdlNaming.EscapeQualifiedIdentifier(alias.QualifiedName);
            return new AliasEmissionType(alias.QualifiedName, target, aliasName);
        }

        return ProjectType(type, currentNamespace);
    }

    /// <summary>Projects a semantic union into an emission union.</summary>
    internal static IdlEmissionUnion ToEmissionUnion(IdlUnion union) =>
        new(
            union.Name,
            union.Namespace,
            union.DiscriminatorIsEnum
                ? IdlNaming.EscapeQualifiedIdentifier(IdlNaming.ResolveTypeName(union.DiscriminatorIdlType, union.Namespace))
                : IdlNaming.MapPrimitive(union.DiscriminatorIdlType),
            union.DiscriminatorIsEnum,
            union.DiscriminatorDefaultValue,
            [.. union.Branches.Select(branch =>
            {
                var field = ToEmissionField(branch.Field, union.Namespace);
                return new UnionBranchEmissionPlan(
                    field,
                    new MemberEmissionPlan(field, union.Namespace),
                    branch.Labels,
                    branch.LabelValues,
                    branch.IsDefault);
            })],
            union.Extensibility);

    private static EmissionTypePlan ProjectType(IdlType type, string? currentNamespace) =>
        type switch
        {
            IdlType.Primitive primitive => new PrimitiveEmissionType(IdlNaming.NormalizeIdlType(primitive.Name), IdlNaming.MapPrimitive(primitive.Name)),
            IdlType.StringType stringType => new StringEmissionType(stringType.IsWide, stringType.Bound),
            IdlType.Enum @enum => new EnumEmissionType(IdlNaming.EscapeQualifiedIdentifier(@enum.QualifiedName), @enum.DefaultValue),
            IdlType.Struct structure => new StructEmissionType(IdlNaming.EscapeQualifiedIdentifier(structure.QualifiedName)),
            IdlType.Union union => new UnionEmissionType(IdlNaming.EscapeQualifiedIdentifier(union.QualifiedName)),
            IdlType.Alias alias => ProjectAlias(alias, currentNamespace),
            IdlType.Sequence sequence => ProjectSequence(sequence, currentNamespace),
            IdlType.Array array => ProjectArray(array, currentNamespace),
            _ => throw new InvalidOperationException($"Unknown semantic IDL type: {type.GetType().Name}")
        };

    private static EmissionTypePlan ProjectAlias(IdlType.Alias alias, string? currentNamespace)
    {
        var target = ProjectType(alias.Target, currentNamespace);
        var aliasName = IdlNaming.EscapeQualifiedIdentifier(alias.QualifiedName);
        var cSharpType = target is SequenceEmissionType or ArrayEmissionType ? aliasName : target.CSharpType;

        return new AliasEmissionType(alias.QualifiedName, target, cSharpType);
    }

    private static SequenceEmissionType ProjectSequence(IdlType.Sequence sequence, string? currentNamespace)
    {
        var element = ProjectType(sequence.Element, currentNamespace);

        return new SequenceEmissionType(element, sequence.Bound ?? 100, $"ISequence<{element.CSharpType}>", sequence.Dimensions);
    }

    private static ArrayEmissionType ProjectArray(IdlType.Array array, string? currentNamespace)
    {
        var element = ProjectType(array.Element, currentNamespace);

        return new ArrayEmissionType(element, array.Dimensions, BuildArrayType(element.CSharpType, array.Dimensions));
    }

    private static bool IsAggregateEmissionType(EmissionTypePlan type) => type switch
    {
        StructEmissionType or UnionEmissionType => true,
        AliasEmissionType alias => alias.Target is SequenceEmissionType or ArrayEmissionType || IsAggregateEmissionType(alias.Target),
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
