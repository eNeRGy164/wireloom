using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler;

/// <summary>Provides shared state and services used while emitting a parsed IDL compilation.</summary>
internal sealed class CompilationContext
{
    private readonly CompilationResolver resolver;
    internal CompilationContext(IdlSymbolTable symbols)
    {
        resolver = new CompilationResolver(symbols);
    }

    /// <summary>Resolves an IDL type into the target-independent semantic type model.</summary>
    internal IdlType ResolveType(string idlType, string? currentNamespace) =>
        resolver.ResolveType(idlType, currentNamespace);

    public bool TryResolveConstant(string reference, string? currentNamespace, out string qualifiedName) =>
        resolver.TryResolveConstant(reference, currentNamespace, out qualifiedName);

    /// <summary>Resolves a scalar typedef chain for source emitters.</summary>
    public string ResolveUnderlyingType(string idlType, string? currentNamespace) =>
        resolver.ResolveUnderlyingType(idlType, currentNamespace);

    /// <summary>Maps a scalar alias value type to its native storage type.</summary>
    public string ResolveAliasNativeType(string resolvedType, string? currentNamespace) =>
        resolver.ResolveAliasNativeType(resolvedType, currentNamespace);

}
