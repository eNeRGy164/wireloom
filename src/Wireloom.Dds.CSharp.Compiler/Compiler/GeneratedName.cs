namespace Wireloom.Compiler;

/// <summary>Authoritative identity of a generated source document.</summary>
internal sealed class GeneratedName(string @namespace, string typeName)
{
    private string Namespace { get; } = @namespace;

    private string TypeName { get; } = typeName;

    public string HintName => string.IsNullOrEmpty(Namespace)
        ? $"{TypeName}.g.cs"
        : $"{Namespace}.{TypeName}.g.cs";
}
