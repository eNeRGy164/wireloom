using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission;

/// <summary>Provides shared source-emission helpers for IDL compilation.</summary>
internal static class EmissionSupport
{
    internal static readonly string[] DataTypeUsings = ["Omg.Types", "Rti.Types"];
    private static readonly string[] UnmanagedTypeUsingsWithoutOmg = ["Rti.Dds.NativeInterface.TypePlugin", "Rti.Types"];
    internal static readonly string[] PluginUsings = ["Omg.Types", "Omg.Types.Dynamic", "Rti.Dds.Core", "Rti.Dds.NativeInterface.TypePlugin", "Rti.Types", "Rti.Types.Dynamic"];
    internal static readonly string[] TypeSupportUsings = ["Rti.Dds.Core", "Rti.Dds.Topics", "Rti.Types.Dynamic"];

    /// <summary>Creates a writer configured for one generated source document.</summary>
    internal static GeneratedSourceWriter CreateSource(
        string? currentNamespace,
        IEnumerable<string> usingAliases,
        string sourceIdlFileName) =>
        new(
            currentNamespace is null ? null : IdlNaming.EscapeQualifiedIdentifier(currentNamespace),
            usingAliases,
            sourceIdlFileName);

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

        return IdlNaming.ResolvedTypeReference($"{typeName[..lastDot]}.Implementation.{typeName[(lastDot + 1)..]}Unmanaged", currentNamespace);
    }

    internal static string GetSupportType(string typeName, string? currentNamespace) =>
        IdlNaming.GeneratedSupportTypeReference(typeName, currentNamespace) + "Support.Instance";

    internal static IReadOnlyList<string> GetUnmanagedTypeUsings(IEnumerable<EmissionTypePlan> types) =>
        types.Any(RequiresOmgTypes)
            ? ["Omg.Types", .. UnmanagedTypeUsingsWithoutOmg]
            : UnmanagedTypeUsingsWithoutOmg;

    private static bool RequiresOmgTypes(EmissionTypePlan type) => type switch
    {
        PrimitiveEmissionType { IdlName: "long double" } => true,
        OptionalEmissionType optional => RequiresOmgTypes(optional.Target) || UsesOmgSequence(optional.Target),
        AliasEmissionType alias => RequiresOmgTypes(alias.Target),
        SequenceEmissionType sequence => RequiresOmgTypes(sequence.Element),
        ArrayEmissionType array => RequiresOmgTypes(array.Element),
        _ => false
    };

    private static bool UsesOmgSequence(EmissionTypePlan type) => type switch
    {
        SequenceEmissionType => true,
        AliasEmissionType alias => UsesOmgSequence(alias.Target),
        _ => false
    };
}
