namespace Wireloom;

public static partial class IdlCompiler
{
    internal static readonly string[] DataTypeUsings = ["Omg.Types", "Rti.Types", "System"];
    internal static readonly string[] UnmanagedTypeUsings = ["Rti.Dds.NativeInterface.TypePlugin", "Rti.Types", "System"];
    internal static readonly string[] PluginUsings = ["Omg.Types", "Omg.Types.Dynamic", "Rti.Dds.Core", "Rti.Dds.NativeInterface.TypePlugin", "Rti.Types", "Rti.Types.Dynamic"];
    internal static readonly string[] TypeSupportUsings = ["Rti.Dds.Core", "Rti.Dds.Topics", "Rti.Types.Dynamic", "System"];

    /// <summary>Creates a writer configured for one generated source document.</summary>
    internal static GeneratedSourceWriter CreateSource(
        string? currentNamespace,
        IEnumerable<string> usingAliases,
        string sourceIdlFileName) =>
        new(
            currentNamespace is null ? null : EscapeQualifiedIdentifier(currentNamespace),
            usingAliases,
            sourceIdlFileName);

    internal static string GetUnmanagedType(string typeName, string? currentNamespace)
    {
        var lastDot = typeName.LastIndexOf('.');
        if (lastDot < 0)
        {
            return typeName + "Unmanaged";
        }

        return TypeReference($"{typeName[..lastDot]}.Implementation.{typeName[(lastDot + 1)..]}Unmanaged", currentNamespace);
    }

    internal static string GetSupportType(string typeName, string? currentNamespace) =>
        TypeReference(typeName, currentNamespace) + "Support.Instance";
}
