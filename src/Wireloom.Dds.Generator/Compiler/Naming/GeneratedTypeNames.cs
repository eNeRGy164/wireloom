namespace Wireloom.Compiler.Naming;

/// <summary>Authoritative generated identities for one IDL declaration.</summary>
internal sealed class GeneratedTypeNames
{
    /// <summary>Initializes generated identities for one declaration.</summary>
    internal GeneratedTypeNames(string? currentNamespace, string declarationName)
    {
        Namespace = currentNamespace;
        ManagedTypeName = IdlNaming.EscapeIdentifier(declarationName);
        SupportTypeName = IdlNaming.EscapeIdentifier($"{declarationName}Support");
        PluginTypeName = IdlNaming.EscapeIdentifier($"{declarationName}Plugin");
        UnmanagedTypeName = IdlNaming.EscapeIdentifier($"{declarationName}Unmanaged");
        ImplementationNamespace = currentNamespace is null
            ? "Implementation"
            : $"{IdlNaming.EscapeQualifiedIdentifier(currentNamespace)}.Implementation";
        Managed = IdlNaming.CreateGeneratedName(currentNamespace, declarationName);
        Support = IdlNaming.CreateGeneratedName(currentNamespace, $"{declarationName}Support");
        Plugin = IdlNaming.CreateGeneratedName(ImplementationNamespace, $"{declarationName}Plugin");
        Unmanaged = IdlNaming.CreateGeneratedName(ImplementationNamespace, $"{declarationName}Unmanaged");
    }

    public string? Namespace { get; }

    public string ManagedTypeName { get; }

    public string SupportTypeName { get; }

    public string PluginTypeName { get; }

    public string UnmanagedTypeName { get; }

    public string ImplementationNamespace { get; }

    public GeneratedName Managed { get; }

    public GeneratedName Support { get; }

    public GeneratedName Plugin { get; }

    public GeneratedName Unmanaged { get; }

    public string SupportIdentity => Qualify(EscapeNamespace(Namespace), SupportTypeName);

    public string PluginIdentity => Qualify(ImplementationNamespace, PluginTypeName);

    public string UnmanagedIdentity => Qualify(ImplementationNamespace, UnmanagedTypeName);

    public string RuntimeTypeName => Qualify(EscapeNamespace(Namespace), ManagedTypeName);

    public string IdlTypeName => Namespace is null
        ? ManagedTypeName.TrimStart('@')
        : $"{Namespace.Replace(".", "::")}::{ManagedTypeName.TrimStart('@')}";

    private static string Qualify(string? @namespace, string typeName) =>
        string.IsNullOrEmpty(@namespace) ? typeName : $"{@namespace}.{typeName}";

    private static string? EscapeNamespace(string? @namespace)
    {
        if (@namespace is null)
        {
            return null;
        }

        return IdlNaming.EscapeQualifiedIdentifier(@namespace);
    }
}
