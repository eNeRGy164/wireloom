using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission;

/// <summary>Provides shared source-emission helpers for IDL compilation.</summary>
internal static class EmissionSupport
{
    internal static readonly string[] TypeSupportUsings = ["Rti.Dds.Core", "Rti.Dds.Topics", "Rti.Types.Dynamic"];

    /// <summary>Creates a writer configured for one generated source document.</summary>
    internal static GeneratedSourceWriter CreateSource(
        string? currentNamespace,
        IEnumerable<string> usingAliases,
        string sourceIdlFileName,
        bool nullableContext = true) =>
        new(
            currentNamespace is null ? null : IdlNaming.EscapeQualifiedIdentifier(currentNamespace),
            usingAliases,
            sourceIdlFileName,
            nullableContext);

    internal static string GetUnmanagedType(string typeName, string? currentNamespace)
    {
        if (typeName.StartsWith("global::", StringComparison.Ordinal))
        {
            typeName = typeName["global::".Length..];
        }

        var lastDot = typeName.LastIndexOf('.');
        if (lastDot < 0)
        {
            return $"global::Implementation.{typeName}Unmanaged";
        }

        var unmanagedNamespace = $"{typeName[..lastDot]}.Implementation";
        var unmanagedTypeName = $"{typeName[(lastDot + 1)..]}Unmanaged";
        if (string.Equals(unmanagedNamespace, currentNamespace, StringComparison.Ordinal))
        {
            return unmanagedTypeName;
        }

        return IdlNaming.ResolvedTypeReference($"{unmanagedNamespace}.{unmanagedTypeName}", currentNamespace);
    }

    internal static string GetSupportType(string typeName, string? currentNamespace) =>
        IdlNaming.GeneratedSupportTypeReference(typeName, currentNamespace) + "Support.Instance";

    /// <summary>Gets namespaces required by the managed member declarations and attributes.</summary>
    internal static IReadOnlyList<string> GetManagedTypeUsings(IEnumerable<MemberEmissionPlan> fields)
    {
        var plans = fields.ToArray();
        var types = plans.Select(field => field.Type).ToArray();
        var usesSequence = types.Any(UsesOmgSequence);
        var usesOmgTypes = usesSequence || plans.Any(field => field.IsKey || field.IsOptional || field.Bound is not null || field.IsBoundedString);
        return GetTypeUsings(types, usesOmgTypes, usesSequence, includeNativeTopicTypePlugin: false, requireNativeRtiTypes: false);
    }

    /// <summary>Gets namespaces required by managed types and optional sequence or attribute syntax.</summary>
    internal static IReadOnlyList<string> GetManagedTypeUsings(IEnumerable<EmissionTypePlan> types, bool usesOmgTypes, bool usesSequence = false)
    {
        var typePlans = types.ToArray();
        var hasSequence = usesSequence || typePlans.Any(UsesOmgSequence);
        return GetTypeUsings(typePlans, usesOmgTypes || hasSequence, hasSequence, includeNativeTopicTypePlugin: false, requireNativeRtiTypes: false);
    }

    /// <summary>Gets namespaces required by native storage types and their runtime interfaces.</summary>
    internal static IReadOnlyList<string> GetUnmanagedTypeUsings(IEnumerable<MemberEmissionPlan> fields, bool usesNativeChar = false)
    {
        var plans = fields.ToArray();
        var types = plans.Select(field => field.Type).ToArray();
        var usesOmgSequence = plans.Any(field => field.IsSequence && field.IsOptional && (field.IsStringSequence || field.HasAggregateElement));
        var usesSequenceType = plans.Any(field => field is
        {
            IsSequence: true,
            HasAggregateElement: false,
            IsStringSequence: false,
            Type.Shape.Kind: not EmissionShapeKind.Alias
        });
        return GetTypeUsings(types, usesOmgSequence, usesSequenceType, includeNativeTopicTypePlugin: true, requireNativeRtiTypes: true, usesNativeChar);
    }

    /// <summary>Gets namespaces required by native storage for an alias or collection type.</summary>
    internal static IReadOnlyList<string> GetUnmanagedTypeUsings(IEnumerable<EmissionTypePlan> types, bool usesSequence = false, bool usesNativeChar = false)
    {
        var typePlans = types.ToArray();
        return GetTypeUsings(typePlans, usesOmgTypes: false, usesSequence, includeNativeTopicTypePlugin: true, requireNativeRtiTypes: true, usesNativeChar);
    }

    /// <summary>Gets namespaces required by one plugin document's dynamic types and metadata.</summary>
    internal static IReadOnlyList<string> GetPluginUsings(IEnumerable<EmissionTypePlan> types, bool usesExtensibility, bool usesAnnotations)
    {
        var typePlans = types.ToArray();
        var usings = new List<string>
        {
            "Rti.Dds.Core",
            "Rti.Dds.NativeInterface.TypePlugin",
            "Rti.Types.Dynamic"
        };

        if (usesExtensibility)
        {
            usings.Add("Omg.Types");
        }

        if (usesAnnotations)
        {
            usings.Add("Omg.Types.Dynamic");
        }

        if (typePlans.Any(RequiresPluginRtiTypes))
        {
            usings.Add("Rti.Types");
        }

        return usings;
    }

    /// <summary>Builds a namespace list for a generated document from its type and runtime requirements.</summary>
    private static List<string> GetTypeUsings(IEnumerable<EmissionTypePlan> types, bool usesOmgTypes, bool usesSequence, bool includeNativeTopicTypePlugin, bool requireNativeRtiTypes, bool usesNativeChar = false)
    {
        var typePlans = types.ToArray();
        var usings = new List<string>();

        if (usesOmgTypes)
        {
            usings.Add("Omg.Types");
        }

        if (usesNativeChar || typePlans.Any(requireNativeRtiTypes ? RequiresRtiTypes : RequiresManagedRtiTypes) || usesSequence)
        {
            usings.Add("Rti.Types");
        }

        if (includeNativeTopicTypePlugin)
        {
            usings.Add("Rti.Dds.NativeInterface.TypePlugin");
        }

        return usings;
    }

    /// <summary>Determines whether native storage for a type references RTI value wrappers.</summary>
    private static bool RequiresRtiTypes(EmissionTypePlan type) => type switch
    {
        PrimitiveEmissionType { IdlName: "char" or "long double" } => true,
        SequenceEmissionType sequence => RequiresRtiTypes(sequence.Element),
        ArrayEmissionType array => RequiresRtiTypes(array.Element),
        OptionalEmissionType optional => RequiresRtiTypes(optional.Target),
        AliasEmissionType alias when string.Equals(alias.CSharpType, alias.Target.CSharpType, StringComparison.Ordinal) => RequiresRtiTypes(alias.Target),
        _ => false
    };

    /// <summary>Determines whether a managed type references RTI managed collection or numeric types.</summary>
    private static bool RequiresManagedRtiTypes(EmissionTypePlan type) => type switch
    {
        PrimitiveEmissionType { IdlName: "long double" } => true,
        SequenceEmissionType => true,
        OptionalEmissionType optional => RequiresManagedRtiTypes(optional.Target),
        ArrayEmissionType array => RequiresManagedRtiTypes(array.Element),
        AliasEmissionType alias when string.Equals(alias.CSharpType, alias.Target.CSharpType, StringComparison.Ordinal) => RequiresManagedRtiTypes(alias.Target),
        _ => false
    };

    /// <summary>Determines whether a plugin dynamic type references RTI primitive value types.</summary>
    private static bool RequiresPluginRtiTypes(EmissionTypePlan type) => type switch
    {
        PrimitiveEmissionType { IdlName: "long double" or "octet" } => true,
        OptionalEmissionType optional => RequiresPluginRtiTypes(optional.Target),
        SequenceEmissionType sequence => RequiresPluginRtiTypes(sequence.Element),
        ArrayEmissionType array => RequiresPluginRtiTypes(array.Element),
        AliasEmissionType alias => RequiresPluginRtiTypes(alias.Target),
        _ => false
    };

    /// <summary>Determines whether a type plan contains a sequence requiring OMG's sequence interface.</summary>
    private static bool UsesOmgSequence(EmissionTypePlan type) => type switch
    {
        SequenceEmissionType => true,
        OptionalEmissionType optional => UsesOmgSequence(optional.Target),
        _ => false
    };

    /// <summary>Determines whether a member plan emits primitive dynamic-type annotations.</summary>
    internal static bool UsesPrimitiveAnnotation(MemberEmissionPlan field) =>
        field is { IsOptional: false, IsExternal: false } && field.PrimitiveAnnotation() is not null;
}
