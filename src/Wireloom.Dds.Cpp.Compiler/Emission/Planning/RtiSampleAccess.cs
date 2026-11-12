using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Classifies value graphs for the RTI sample-access and interpreter paths.</summary>
internal static class RtiSampleAccess
{
    /// <summary>Checks whether the file contains an optional self-recursive fixed array.</summary>
    internal static bool HasOptionalSelfRecursiveArray(GenerationPlan plan) =>
        plan.Structs.Any(declaration => declaration.Members.Any(member =>
            member.IsOptional && IsSelfRecursiveArrayPlan(member.Type, declaration.QualifiedName)));

    /// <summary>Checks whether a struct owns an optional fixed array that recursively references itself.</summary>
    internal static bool HasOptionalSelfRecursiveArray(StructPlan declaration) =>
        declaration.Members.Any(member =>
            member.IsOptional && IsSelfRecursiveArrayPlan(member.Type, declaration.QualifiedName));

    /// <summary>Checks whether any union branch uses collection storage.</summary>
    internal static bool HasCollectionUnionBranch(GenerationPlan plan) =>
        plan.Unions.Any(declaration => declaration.Branches.Any(branch => branch.Type.ContainsCollection));

    private static IReadOnlyList<MemberTypePlan.Sequence> GetSequencePlans(MemberTypePlan type)
    {
        if (type is MemberTypePlan.Alias alias)
        {
            return GetSequencePlans(alias.Target);
        }

        if (type is MemberTypePlan.Array array)
        {
            return GetSequencePlans(array.ElementType);
        }

        if (type is not MemberTypePlan.Sequence sequence)
        {
            return Array.Empty<MemberTypePlan.Sequence>();
        }

        return new[] { sequence }.Concat(GetSequencePlans(sequence.ElementType)).ToArray();
    }

    /// <summary>Checks whether any member stores a sequence through a typedef.</summary>
    internal static bool HasAliasedSequenceMember(GenerationPlan plan) =>
        plan.Structs.SelectMany(declaration => declaration.Members)
            .Any(member => member.Type is MemberTypePlan.Alias
                && GetSequencePlans(member.Type).Count > 0);

    /// <summary>Checks for sequence storage in a struct or any reachable member type.</summary>
    internal static bool HasSequenceInValueGraph(GenerationPlan plan, StructPlan declaration)
    {
        var structures = plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal);
        var unions = plan.Unions.ToDictionary(item => $"::{item.QualifiedName}", StringComparer.Ordinal);
        return HasSequenceInValueGraph(declaration, structures, unions, new HashSet<string>(StringComparer.Ordinal));
    }

    /// <summary>Checks for string storage in a struct or any reachable member type.</summary>
    internal static bool HasStringInValueGraph(GenerationPlan plan, StructPlan declaration)
    {
        var structures = plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal);
        var unions = plan.Unions.ToDictionary(item => $"::{item.QualifiedName}", StringComparer.Ordinal);
        return declaration.Members.Any(member => HasStringInValueGraph(member.Type, structures, unions,
            new HashSet<string>(StringComparer.Ordinal)));
    }

    private static bool HasNestedStringAliasInValueGraph(GenerationPlan plan, StructPlan declaration)
    {
        var structures = plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal);
        var unions = plan.Unions.ToDictionary(item => $"::{item.QualifiedName}", StringComparer.Ordinal);
        return declaration.Members.Any(member =>
            HasNestedStringAlias(member.Type, structures, unions, new HashSet<string>(StringComparer.Ordinal)));
    }

    private static bool HasNestedStringAlias(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias when UnwrapAlias(alias) is MemberTypePlan.String => false,
        MemberTypePlan.Alias alias => HasNestedStringAlias(alias.Target, structures, unions, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                HasNestedStringAlias(nested, structures, unions, active),
        MemberTypePlan.Union union
            when unions.TryGetValue($"::{union.QualifiedName}", out var nestedUnion) =>
                HasNestedStringAlias(nestedUnion, structures, unions, active),
        _ => false,
    };

    private static bool HasNestedStringAlias(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.Members.Any(member =>
            member.Type is MemberTypePlan.Alias alias
                && UnwrapAlias(alias) is MemberTypePlan.String
            || HasNestedStringAlias(member.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return found;
    }

    private static bool HasNestedStringAlias(
        UnionPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.Branches.Any(branch =>
            branch.Type is MemberTypePlan.Alias alias
                && UnwrapAlias(alias) is MemberTypePlan.String
            || HasNestedStringAlias(branch.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return found;
    }

    /// <summary>Checks for string storage in a struct or any reachable member type.</summary>
    private static bool HasStringInValueGraph(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active) => type switch
    {
        MemberTypePlan.String => true,
        MemberTypePlan.Alias alias => HasStringInValueGraph(alias.Target, structures, unions, active),
        MemberTypePlan.Sequence sequence => HasStringInValueGraph(sequence.ElementType, structures, unions, active),
        MemberTypePlan.Array array => HasStringInValueGraph(array.ElementType, structures, unions, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested)
                && active.Add(nested.CppType) =>
                nested.Members.Any(member => HasStringInValueGraph(member.Type, structures, unions, active)),
        MemberTypePlan.Union union
            when unions.TryGetValue($"::{union.QualifiedName}", out var nestedUnion) =>
                HasStringInValueGraph(nestedUnion, structures, unions, active),
        _ => false,
    };

    /// <summary>Checks for string storage in a struct or any reachable member type.</summary>
    private static bool HasStringInValueGraph(
        UnionPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.Branches.Any(branch =>
            HasStringInValueGraph(branch.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return found;
    }

    private static bool SupportsDirectStringValueGraph(
        GenerationPlan plan,
        StructPlan declaration) =>
        !declaration.IsFlatData
        && (declaration.CppBaseType is not null
            || declaration.Members.Any(member =>
                member.Type is MemberTypePlan.String or MemberTypePlan.Struct or MemberTypePlan.Union
                    || member.Type is MemberTypePlan.Alias
                        && (UnwrapAlias(member.Type) is MemberTypePlan.String or MemberTypePlan.Union)))
        && HasStringInValueGraph(plan, declaration)
        && !HasNestedStringAliasInValueGraph(plan, declaration)
        && SupportsStringAggregateValueGraph(
            declaration,
            plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal),
            plan.Unions.ToDictionary(item => $"::{item.QualifiedName}", StringComparer.Ordinal),
            new HashSet<string>(StringComparer.Ordinal));

    private static bool SupportsStringAggregateValueGraph(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (declaration.IsValueType || !active.Add(declaration.CppType))
        {
            return false;
        }

        var baseSupported = declaration.CppBaseType is null
            || structures.TryGetValue(declaration.CppBaseType, out var baseDeclaration)
                && SupportsStringAggregateValueGraph(baseDeclaration, structures, unions, active);
        var membersSupported = declaration.Members.All(member =>
            member is { IsOptional: false, IsExternal: false }
            && SupportsStringAggregateValueGraph(member.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return baseSupported && membersSupported;
    }

    private static bool SupportsStringAggregateValueGraph(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Primitive or MemberTypePlan.Enumeration or MemberTypePlan.String => true,
        MemberTypePlan.Alias alias => SupportsStringAggregateValueGraph(alias.Target, structures, unions, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                SupportsStringAggregateValueGraph(nested, structures, unions, active),
        MemberTypePlan.Union union
            when unions.TryGetValue($"::{union.QualifiedName}", out var nestedUnion) =>
                SupportsStringAggregateValueGraph(nestedUnion, structures, unions, active),
        _ => false,
    };

    private static bool SupportsStringAggregateValueGraph(
        UnionPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var supported = declaration.Branches.All(branch =>
            SupportsStringAggregateValueGraph(branch.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return supported;
    }

    /// <summary>Identifies structs that use the direct-string interpreter bridge.</summary>
    internal static bool UsesDirectStringPluginBridge(GenerationPlan plan, StructPlan declaration) =>
        declaration is { IsValueType: false, IsTopic: true }
        && !HasSequenceInValueGraph(plan, declaration)
        && !IsSequenceAggregateElement(plan, declaration)
        && HasStringInValueGraph(plan, declaration)
        && SupportsDirectStringValueGraph(plan, declaration);

    /// <summary>Identifies structs that use an STL-aware interpreter bridge.</summary>
    internal static bool UsesStlInterpreterPluginBridge(GenerationPlan plan, StructPlan declaration) =>
        (HasSequenceInValueGraph(plan, declaration) && SupportsSequenceInValueGraph(plan, declaration))
        || HasOptionalSelfRecursiveArray(declaration)
        || UsesDirectStringPluginBridge(plan, declaration)
        || IsReferencedByStlInterpreterGraph(plan, declaration);

    /// <summary>Checks whether a struct is referenced from a value using the STL interpreter.</summary>
    internal static bool IsReferencedByStlInterpreterGraph(
        GenerationPlan plan,
        StructPlan target)
    {
        var structures = plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal);
        return plan.Structs.Any(topic => topic.CppType != target.CppType
            && UsesDirectStringPluginBridge(plan, topic)
            && ValueGraphContains(topic, target.CppType, structures, new HashSet<string>(StringComparer.Ordinal)));
    }

    /// <summary>Checks whether a struct is nested in a topic using the STL sequence interpreter.</summary>
    internal static bool IsReferencedBySequenceInterpreterGraph(
        GenerationPlan plan,
        StructPlan target)
    {
        var structures = plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal);
        return plan.Structs.Any(topic => topic.CppType != target.CppType
            && topic.IsTopic
            && HasSequenceInValueGraph(plan, topic)
            && SupportsSequenceInValueGraph(plan, topic)
            && ValueGraphContains(topic, target.CppType, structures, new HashSet<string>(StringComparer.Ordinal)));
    }

    private static bool ValueGraphContains(
        StructPlan declaration,
        string targetType,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active)
    {
        if (declaration.CppType == targetType)
        {
            return true;
        }

        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.CppBaseType is { } baseType
                && structures.TryGetValue(baseType, out var baseDeclaration)
                && ValueGraphContains(baseDeclaration, targetType, structures, active)
            || declaration.Members.Any(member =>
                ValueGraphContains(member.Type, targetType, structures, active));
        active.Remove(declaration.CppType);
        return found;
    }

    private static bool ValueGraphContains(
        MemberTypePlan type,
        string targetType,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias => ValueGraphContains(alias.Target, targetType, structures, active),
        MemberTypePlan.Sequence sequence => ValueGraphContains(sequence.ElementType, targetType, structures, active),
        MemberTypePlan.Array array => ValueGraphContains(array.ElementType, targetType, structures, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                ValueGraphContains(nested, targetType, structures, active),
        _ => false,
    };

    /// <summary>Checks whether a struct needs interpreter sample-access metadata.</summary>
    internal static bool HasInterpreterSampleAccess(GenerationPlan plan, StructPlan declaration) =>
        (HasSequenceInValueGraph(plan, declaration) && SupportsSequenceInValueGraph(plan, declaration))
        || HasOptionalSelfRecursiveArray(declaration)
        || UsesDirectStringPluginBridge(plan, declaration)
        || IsReferencedByStlInterpreterGraph(plan, declaration);

    /// <summary>Checks for sequence storage in a struct or any reachable member type.</summary>
    private static bool HasSequenceInValueGraph(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        return declaration.Members.Any(member => HasSequenceInValueGraph(member.Type, structures, unions, active));
    }

    /// <summary>Checks for sequence storage in a struct or any reachable member type.</summary>
    private static bool HasSequenceInValueGraph(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Sequence => true,
        MemberTypePlan.Array array => HasSequenceInValueGraph(array.ElementType, structures, unions, active),
        MemberTypePlan.Alias alias => HasSequenceInValueGraph(alias.Target, structures, unions, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                HasSequenceInValueGraph(nested, structures, unions, active),
        MemberTypePlan.Union union
            when unions.TryGetValue($"::{union.QualifiedName}", out var nestedUnion) =>
                HasSequenceInValueGraph(nestedUnion, structures, unions, active),
        _ => false,
    };

    private static bool HasSequenceInValueGraph(
        UnionPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.Branches.Any(branch =>
            HasSequenceInValueGraph(branch.Type, structures, unions, active));
        active.Remove(declaration.CppType);
        return found;
    }

    /// <summary>Checks whether the RTI interpreter path supports the sequence-bearing shape.</summary>
    internal static bool SupportsSequenceInValueGraph(GenerationPlan plan, StructPlan declaration) =>
        SupportsSequenceInValueGraph(
            declaration,
            plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal),
            plan.Unions.ToDictionary(item => $"::{item.QualifiedName}", StringComparer.Ordinal),
            new HashSet<string>(StringComparer.Ordinal));

    /// <summary>Checks whether inherited member storage prevents C-layout sample access.</summary>
    internal static bool HasInheritedAggregateStorage(GenerationPlan plan, StructPlan declaration) =>
        HasInheritedAggregateStorage(
            declaration,
            plan.Structs.ToDictionary(item => item.CppType, StringComparer.Ordinal),
            new HashSet<string>(StringComparer.Ordinal));

    private static bool HasKeyInValueGraph(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        var found = declaration.Members.Any(member =>
            member.IsKey || HasKeyInValueGraph(member.Type, structures, active));
        if (declaration.CppBaseType is { } baseType
            && structures.TryGetValue(baseType, out var baseDeclaration))
        {
            found |= HasKeyInValueGraph(baseDeclaration, structures, active);
        }

        active.Remove(declaration.CppType);
        return found;
    }

    private static bool HasKeyInValueGraph(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias => HasKeyInValueGraph(alias.Target, structures, active),
        MemberTypePlan.Sequence sequence => HasKeyInValueGraph(sequence.ElementType, structures, active),
        MemberTypePlan.Array array => HasKeyInValueGraph(array.ElementType, structures, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                HasKeyInValueGraph(nested, structures, active),
        _ => false,
    };

    /// <summary>Checks whether inherited member storage prevents C-layout sample access.</summary>
    private static bool HasInheritedAggregateStorage(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return false;
        }

        return declaration.CppBaseType is not null
            || declaration.Members.Any(member => HasInheritedAggregateStorage(member.Type, structures, active));
    }

    /// <summary>Checks whether inherited member storage prevents C-layout sample access.</summary>
    private static bool HasInheritedAggregateStorage(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias => HasInheritedAggregateStorage(alias.Target, structures, active),
        MemberTypePlan.Sequence sequence => HasInheritedAggregateStorage(sequence.ElementType, structures, active),
        MemberTypePlan.Array array => HasInheritedAggregateStorage(array.ElementType, structures, active),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                HasInheritedAggregateStorage(nested, structures, active),
        _ => false,
    };

    /// <summary>Checks whether the RTI interpreter path supports the sequence-bearing shape.</summary>
    private static bool SupportsSequenceInValueGraph(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active,
        bool allowBoundedStringSequenceElement = false)
    {
        if (declaration.HasSelfRecursiveSequence)
        {
            return IsSupportedSelfRecursiveSequenceAggregate(declaration);
        }

        if (!active.Add(declaration.CppType))
        {
            return true;
        }

        return declaration.Members.All(member =>
            SupportsSequenceInValueGraph(
                member.Type, structures, unions, active, allowBoundedStringSequenceElement)
            || (member.IsOptional && IsSupportedOptionalBoundedStringSequence(member.Type)));
    }

    /// <summary>Checks whether the RTI interpreter path supports the sequence-bearing shape.</summary>
    private static bool SupportsSequenceInValueGraph(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active,
        bool allowBoundedStringSequenceElement = false) => type switch
    {
        MemberTypePlan.Sequence sequence => IsSupportedSequenceElement(
                sequence.ElementType, structures, allowBoundedStringSequenceElement),
        MemberTypePlan.Array array => array.ContainsSequence
            ? IsSupportedArraySequence(array.ElementType)
            : !HasSequenceInValueGraph(array.ElementType, structures, unions, new HashSet<string>(active, StringComparer.Ordinal))
                && SupportsSequenceInValueGraph(array.ElementType, structures, unions, active, allowBoundedStringSequenceElement),
        MemberTypePlan.Alias alias => SupportsSequenceInValueGraph(
            alias.Target, structures, unions, active, allowBoundedStringSequenceElement: true),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var nested) =>
                SupportsSequenceInValueGraph(nested, structures, unions, active, allowBoundedStringSequenceElement),
        MemberTypePlan.Union union
            when unions.TryGetValue($"::{union.QualifiedName}", out var nestedUnion) =>
                SupportsSequenceInValueGraph(nestedUnion, structures, unions, active),
        _ => true,
    };

    private static bool SupportsSequenceInValueGraph(
        UnionPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structures,
        IReadOnlyDictionary<string, UnionPlan> unions,
        ISet<string> active)
    {
        if (!active.Add(declaration.CppType))
        {
            return true;
        }

        var supported = declaration.Branches.All(branch =>
            SupportsSequenceInValueGraph(
                branch.Type, structures, unions, active, allowBoundedStringSequenceElement: true));
        active.Remove(declaration.CppType);
        return supported;
    }

    private static bool IsSupportedArraySequence(MemberTypePlan type)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Sequence
            {
                IsUnbounded: false,
                ElementType: MemberTypePlan.Primitive
                    or MemberTypePlan.Enumeration
                    or MemberTypePlan.String { MetadataBound: > 0 }
            };
    }

    private static bool IsSupportedSequenceElement(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structures,
        bool allowBoundedStringElement = false) => type switch
    {
        MemberTypePlan.Primitive or MemberTypePlan.Enumeration
            => true,
        // RTI's interpreted CDR bridge supports unbounded narrow strings in a
        // sequence. Keep the bounded-string alias path for the other string
        // shapes, which have separate profile qualification.
        MemberTypePlan.String { MetadataBound: 0, IsWide: false } => true,
        MemberTypePlan.String => allowBoundedStringElement,
        MemberTypePlan.Alias alias => IsSupportedSequenceElement(
            alias.Target, structures, allowBoundedStringElement: true),
        MemberTypePlan.Sequence sequence => IsSupportedSequenceElement(
            sequence.ElementType, structures, allowBoundedStringElement: true),
        MemberTypePlan.Array array => IsSupportedFixedArrayElement(array.ElementType),
        MemberTypePlan.Struct structure
            when structures.TryGetValue($"::{structure.QualifiedName}", out var declaration) =>
                IsSupportedSequenceAggregateElement(declaration)
                || IsSupportedSelfRecursiveSequenceAggregate(declaration),
        _ => false,
    };

    private static bool IsSupportedSelfRecursiveSequenceAggregate(StructPlan declaration) =>
        declaration is
        {
            IsTopic: true,
            IsValueType: false,
            IsFlatData: false,
            CppBaseType: null,
            Members.Count: > 1,
        }
        && declaration.Members.All(member =>
            member is { IsOptional: false, IsExternal: false, IsKey: false }
            && (member.Type is MemberTypePlan.Primitive or MemberTypePlan.Enumeration
                || declaration.HasSelfRecursiveSequence
                && IsSelfRecursiveSequence(member.Type, declaration.CppType)));

    /// <summary>Identifies a sequence that directly contains its declaring struct.</summary>
    internal static bool IsSelfRecursiveSequence(MemberTypePlan type, string cppType) => type switch
    {
        MemberTypePlan.Sequence sequence => ResolvesToStruct(sequence.ElementType, cppType.TrimStart(':')),
        MemberTypePlan.Alias alias => IsSelfRecursiveSequence(alias.Target, cppType),
        _ => false,
    };

    private static bool IsSupportedFixedArrayElement(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Primitive or MemberTypePlan.Enumeration or MemberTypePlan.String => true,
        MemberTypePlan.Alias alias => IsSupportedFixedArrayElement(alias.Target),
        MemberTypePlan.Array array => IsSupportedFixedArrayElement(array.ElementType),
        _ => false,
    };

    private static bool IsSupportedSequenceAggregateElement(StructPlan declaration) =>
        declaration is
        {
            IsValueType: false,
            CppBaseType: null,
            IsFlatData: false,
            Members.Count: > 0,
        }
        && declaration.Members.All(member =>
            member is
            {
                IsOptional: false,
                IsExternal: false,
                Type: MemberTypePlan.Primitive
                    or MemberTypePlan.Enumeration
                    or MemberTypePlan.String { MetadataBound: > 0 },
            });

    /// <summary>Checks whether a struct is used as a supported sequence element.</summary>
    internal static bool IsSequenceAggregateElement(GenerationPlan plan, StructPlan declaration) =>
        plan.Structs.Any(topic => topic.IsTopic
            && HasSequenceInValueGraph(plan, topic)
            && SupportsSequenceInValueGraph(plan, topic))
        && plan.Structs.SelectMany(item => item.Members)
            .Any(member => ContainsSequenceElementStruct(member.Type, declaration.QualifiedName));

    private static bool ContainsSequenceElementStruct(MemberTypePlan type, string qualifiedTypeName) => type switch
    {
        MemberTypePlan.Alias alias => ContainsSequenceElementStruct(alias.Target, qualifiedTypeName),
        MemberTypePlan.Sequence sequence => ResolvesToStruct(sequence.ElementType, qualifiedTypeName)
            || ContainsSequenceElementStruct(sequence.ElementType, qualifiedTypeName),
        MemberTypePlan.Array array => ContainsSequenceElementStruct(array.ElementType, qualifiedTypeName),
        _ => false,
    };

    private static bool ResolvesToStruct(MemberTypePlan type, string qualifiedTypeName)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Struct structure
            && string.Equals(structure.QualifiedName, qualifiedTypeName, StringComparison.Ordinal);
    }

    private static bool IsSupportedOptionalBoundedStringSequence(MemberTypePlan type) =>
        type is MemberTypePlan.Sequence
        {
            IsUnbounded: false,
            Bound: 4,
            ElementType: MemberTypePlan.String { MetadataBound: 16 },
        };

    /// <summary>Identifies a fixed array that directly contains its declaring struct.</summary>
    internal static bool IsSelfRecursiveArrayPlan(MemberTypePlan type, string qualifiedTypeName) =>
        type is MemberTypePlan.Array
        {
            Dimensions: [> 0],
            ElementType: MemberTypePlan.Struct structure,
        }
        && string.Equals(structure.QualifiedName, qualifiedTypeName, StringComparison.Ordinal);

    /// <summary>Checks whether aggregate metadata must expose type-plugin sample access.</summary>
    internal static bool NeedsAggregateTypePluginMetadata(
        GenerationPlan plan,
        StructPlan declaration) => HasInterpreterSampleAccess(plan, declaration);

    /// <summary>Checks whether a member type resolves to a struct or union through its typedef chain.</summary>
    internal static bool IsAggregateAliasTarget(MemberTypePlan type)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Struct or MemberTypePlan.Union;
    }

    /// <summary>Resolves a type through all declared typedef aliases.</summary>
    internal static MemberTypePlan UnwrapAlias(MemberTypePlan type) => type is MemberTypePlan.Alias alias
        ? UnwrapAlias(alias.Target)
        : type;
}
