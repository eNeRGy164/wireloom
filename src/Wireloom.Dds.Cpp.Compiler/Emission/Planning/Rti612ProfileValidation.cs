using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Validates C++ shapes supported by the retained RTI 6.1.2 plugin adapter.</summary>
internal static class Rti612ProfileValidation
{
    /// <summary>Rejects declarations outside the qualified RTI 6.1.2 aggregate path.</summary>
    internal static void Validate(
        GenerationPlan plan,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        if (plan.Profile.RtiVersion != "6.1.2")
        {
            return;
        }

        if (plan.Unions.Any(union => !IsSupportedUnion(union))
            || plan.Aliases.Any(alias => !IsSupportedAliasType(alias.ResolvedType)))
        {
            ProfileValidation.AddError(diagnostics, input, 0,
                "The Wireloom C++ RTI 6.1.2 adapter cannot represent this union or typedef shape.");
            return;
        }

        foreach (var structure in plan.Structs)
        {
            if (structure.Members.Count == 0
                || structure.Members.Any(member => !IsSupportedMemberType(member.Type)
                    || member.IsMustUnderstand))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The Wireloom C++ RTI 6.1.2 adapter cannot represent one or more members of '{structure.Name}'.");
            }
        }
    }

    private static bool IsSupportedMemberType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Primitive => true,
        MemberTypePlan.String => true,
        MemberTypePlan.Struct => true,
        MemberTypePlan.Union => true,
        MemberTypePlan.Enumeration => true,
        MemberTypePlan.Alias alias => IsSupportedAliasType(alias.Target),
        MemberTypePlan.Sequence sequence => IsSupportedMemberType(sequence.ElementType),
        MemberTypePlan.Array array => IsSupportedMemberType(array.ElementType),
        _ => false,
    };

    private static bool IsSupportedAliasType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Primitive => true,
        MemberTypePlan.Enumeration => true,
        MemberTypePlan.String => true,
        MemberTypePlan.Struct => true,
        MemberTypePlan.Union => true,
        MemberTypePlan.Alias alias => IsSupportedAliasType(alias.Target),
        MemberTypePlan.Sequence sequence => IsSupportedAliasType(sequence.ElementType),
        MemberTypePlan.Array array => IsSupportedAliasType(array.ElementType),
        _ => false,
    };

    private static bool IsSupportedUnion(UnionPlan union) =>
        union.Branches.All(branch => IsSupportedMemberType(branch.Type));
}
