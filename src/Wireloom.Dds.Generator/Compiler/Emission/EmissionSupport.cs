using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission;

/// <summary>Provides shared source-emission helpers for IDL compilation.</summary>
internal static class EmissionSupport
{
    internal static readonly string[] DataTypeUsings = ["Omg.Types", "Rti.Types"];
    internal static readonly string[] UnmanagedTypeUsings = ["Rti.Dds.NativeInterface.TypePlugin", "Rti.Types"];
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
        var lastDot = typeName.LastIndexOf('.');
        if (lastDot < 0)
        {
            return $"global::Implementation.{typeName}Unmanaged";
        }

        return IdlNaming.ResolvedTypeReference($"{typeName[..lastDot]}.Implementation.{typeName[(lastDot + 1)..]}Unmanaged", currentNamespace);
    }

    internal static string GetSupportType(string typeName, string? currentNamespace) =>
        IdlNaming.GeneratedSupportTypeReference(typeName, currentNamespace) + "Support.Instance";
}
