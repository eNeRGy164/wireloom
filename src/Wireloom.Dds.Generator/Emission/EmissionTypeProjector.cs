using System;
using System.Collections.Generic;
using System.Linq;

namespace Wireloom;

using static Wireloom.IdlCompiler;

internal static class EmissionTypeProjector
{
    internal static IdlEmissionField ToEmissionField(IdlMember member, string? currentNamespace)
    {
        var type = ProjectType(member.Type, currentNamespace);
        var cSharpType = type.CSharpType;

        if (member.Metadata.IsOptional && type is StringEmissionType)
        {
            cSharpType = "string?";
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
            member.Metadata.IsKey,
            member.Metadata.MemberId,
            member.Metadata.IsOptional);
    }

    internal static IdlEmissionUnion ToEmissionUnion(IdlUnion union) =>
        new(
            union.Name,
            union.Namespace,
            union.DiscriminatorIdlType,
            union.DiscriminatorIsEnum
                ? EscapeQualifiedIdentifier(ResolveTypeName(union.DiscriminatorIdlType, union.Namespace))
                : MapPrimitive(union.DiscriminatorIdlType),
            union.DiscriminatorIsEnum,
            [.. union.Branches.Select(branch =>
            {
                var field = ToEmissionField(branch.Field, union.Namespace);
                return new UnionBranchEmissionPlan(
                    field,
                    new MemberEmissionPlan(field, union.Namespace),
                    branch.Labels,
                    branch.LabelValues,
                    branch.IsDefault);
            })]);

    private static EmissionTypePlan ProjectType(IdlType type, string? currentNamespace) =>
        type switch
        {
            IdlType.Primitive primitive => new PrimitiveEmissionType(NormalizeIdlType(primitive.Name), MapPrimitive(primitive.Name)),
            IdlType.StringType stringType => new StringEmissionType(stringType.IsWide, stringType.Bound),
            IdlType.Enum @enum => new EnumEmissionType(@enum.QualifiedName, EscapeQualifiedIdentifier(@enum.QualifiedName)),
            IdlType.Struct structure => new StructEmissionType(structure.QualifiedName, EscapeQualifiedIdentifier(structure.QualifiedName)),
            IdlType.Alias alias => ProjectAlias(alias, currentNamespace),
            IdlType.Sequence sequence => ProjectSequence(sequence, currentNamespace),
            IdlType.Array array => ProjectArray(array, currentNamespace),
            _ => throw new InvalidOperationException($"Unknown semantic IDL type: {type.GetType().Name}")
        };

    private static EmissionTypePlan ProjectAlias(IdlType.Alias alias, string? currentNamespace)
    {
        var target = ProjectType(alias.Target, currentNamespace);
        var aliasName = EscapeQualifiedIdentifier(alias.QualifiedName);
        var cSharpType = target is SequenceEmissionType or ArrayEmissionType ? aliasName : target.CSharpType;

        return new AliasEmissionType(alias.QualifiedName, target, cSharpType);
    }

    private static SequenceEmissionType ProjectSequence(IdlType.Sequence sequence, string? currentNamespace)
    {
        var element = ProjectType(sequence.Element, currentNamespace);

        return new SequenceEmissionType(element, sequence.Bound ?? 100, $"ISequence<{element.CSharpType}>");
    }

    private static ArrayEmissionType ProjectArray(IdlType.Array array, string? currentNamespace)
    {
        var element = ProjectType(array.Element, currentNamespace);

        return new ArrayEmissionType(element, array.Dimensions, BuildArrayType(element.CSharpType, array.Dimensions));
    }

    internal static bool IsAggregateEmissionType(EmissionTypePlan type) => type switch
    {
        StructEmissionType => true,
        AliasEmissionType alias => alias.Target is SequenceEmissionType or ArrayEmissionType || IsAggregateEmissionType(alias.Target),
        _ => false
    };

    internal static EmissionTypePlan UnwrapOptionalEmissionType(EmissionTypePlan type) =>
        type is OptionalEmissionType optional ? optional.Target : type;

    internal static EmissionTypePlan UnwrapValueEmissionType(EmissionTypePlan type)
    {
        type = UnwrapOptionalEmissionType(type);

        return type is AliasEmissionType alias ? UnwrapValueEmissionType(alias.Target) : type;
    }

    private static bool IsStringEmissionType(IdlEmissionField field) =>
        UnwrapOptionalEmissionType(field.Type) is StringEmissionType;

    private static bool IsSequenceEmissionType(IdlEmissionField field) =>
        UnwrapOptionalEmissionType(field.Type) is SequenceEmissionType;

    private static bool IsArrayEmissionType(IdlEmissionField field) =>
        UnwrapOptionalEmissionType(field.Type) is ArrayEmissionType;

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
