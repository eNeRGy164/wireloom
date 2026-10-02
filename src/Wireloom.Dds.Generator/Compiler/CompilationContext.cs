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

    /// <summary>Maps a resolved semantic type to its native C# reference.</summary>
    internal string ResolveNativeType(IdlType type, string? currentNamespace) =>
        resolver.ResolveNativeType(type, currentNamespace);

}
