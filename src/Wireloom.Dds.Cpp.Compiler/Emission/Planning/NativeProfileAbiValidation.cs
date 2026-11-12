using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Rejects output paths whose Wireloom native adapter is qualified only for RTI 7.7.0.</summary>
internal static class NativeProfileAbiValidation
{
    /// <summary>Reports Wireloom adapter gaps without attributing them to RTI runtime capability.</summary>
    internal static void Validate(
        GenerationPlan plan,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        if (!plan.Profile.UsesWireloom770PrivateTypeCodeLayout)
        {
            foreach (var declaration in plan.Structs.Where(declaration =>
                         plan.Profile.RtiVersion is not ("6.1.2" or "7.3.1")
                         && declaration.IsNested))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The Wireloom C++ nested TypeCode adapter is not qualified for RTI {plan.Profile.RtiVersion}; this shape remains unsupported for that profile ('{declaration.Name}').");
            }

            // RTI 6.1.2 and 7.3.1 expose the union TypeCode fields used by
            // their profile-specific adapters (_typeCode, _members, labels,
            // and sample access metadata).
            foreach (var declaration in plan.Unions.Where(_ =>
                         plan.Profile.RtiVersion is not ("6.1.2" or "7.3.1")))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator's union TypeCode adapter uses RTI 7.7.0 private TypeCode layout and is not qualified for RTI {plan.Profile.RtiVersion}; this Wireloom shape remains unsupported for that profile ('{declaration.Name}').");
            }

            foreach (var alias in plan.Aliases.Where(alias =>
                         plan.Profile.RtiVersion is not ("6.1.2" or "7.3.1")
                         && RtiSampleAccess.IsAggregateAliasTarget(alias.ResolvedType)))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator's aggregate-alias TypeCode adapter uses RTI 7.7.0 private TypeCode layout and is not qualified for RTI {plan.Profile.RtiVersion}; this Wireloom shape remains unsupported for that profile ('{alias.Name}').");
            }

            if (RtiSampleAccess.HasCollectionUnionBranch(plan)
                && plan.Profile.RtiVersion is not ("6.1.2" or "7.3.1"))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator's recursive collection TypeCode adapter uses RTI 7.7.0 private TypeCode layout and is not qualified for RTI {plan.Profile.RtiVersion}; this Wireloom shape remains unsupported for that profile.");
            }
        }

        // RTI 6.1.2 uses the dedicated legacy PRESTypePlugin path and its own
        // profile validation; this guard covers the shared STL interpreter bridge.
        if (plan.Profile.RtiVersion != "6.1.2" && !plan.Profile.SupportsStlSampleAccessAdapter)
        {
            foreach (var declaration in plan.Structs.Where(declaration =>
                         RtiSampleAccess.UsesStlInterpreterPluginBridge(plan, declaration)
                         || RtiSampleAccess.HasInterpreterSampleAccess(plan, declaration)
                         || RtiSampleAccess.IsSequenceAggregateElement(plan, declaration)))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator's STL sample-access metadata adapter is qualified only for RTI 7.7.0, not RTI {plan.Profile.RtiVersion}; this Wireloom shape remains unsupported for that profile ('{declaration.Name}').");
            }
        }
    }

}
