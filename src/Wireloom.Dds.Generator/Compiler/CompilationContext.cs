using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler;

/// <summary>Provides shared state and services used while emitting a parsed IDL compilation.</summary>
internal sealed class CompilationContext
{
    private readonly CompilationResolver resolver;
    internal CompilationContext(IdlSymbolTable symbols)
    {
        resolver = new CompilationResolver(symbols);
    }

    public bool TryResolveConstant(string reference, string? currentNamespace, out string qualifiedName) =>
        resolver.TryResolveConstant(reference, currentNamespace, out qualifiedName);

    public bool IsEnum(string typeName, string? currentNamespace) =>
        resolver.IsEnum(typeName, currentNamespace);

    public bool IsUnion(string typeName, string? currentNamespace) =>
        resolver.IsUnion(typeName, currentNamespace);

    /// <summary>Resolves a scalar typedef chain for source emitters.</summary>
    public string ResolveUnderlyingType(string idlType, string? currentNamespace) =>
        resolver.ResolveUnderlyingType(idlType, currentNamespace);

    /// <summary>Maps a scalar alias value type to its native storage type.</summary>
    public string ResolveAliasNativeType(string resolvedType, string? currentNamespace) =>
        resolver.ResolveAliasNativeType(resolvedType, currentNamespace);

}
