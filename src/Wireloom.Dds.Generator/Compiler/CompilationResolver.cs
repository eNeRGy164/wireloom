using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler;

/// <summary>Resolves IDL symbols and typedef representations for the compiler pipeline.</summary>
internal sealed class CompilationResolver(IdlSymbolTable symbols)
{
    private readonly IdlTypeResolver typeResolver = new(symbols);

    /// <summary>Resolves an IDL type into the target-independent semantic type model.</summary>
    public IdlType ResolveType(string idlType, string? currentNamespace)
    {
        var input = new IdlInput("<generated>", string.Empty, generate: false);
        return typeResolver.Resolve(idlType, currentNamespace, input, 0)
            ?? throw new InvalidOperationException($"Unsupported IDL type: {idlType}");
    }

    /// <summary>Resolves an IDL constant reference using lexical module scope.</summary>
    public bool TryResolveConstant(string reference, string? currentNamespace, out string qualifiedName)
    {
        var normalized = reference.TrimStart(':').Replace("::", ".");
        var isAbsolute = reference.StartsWith("::", StringComparison.Ordinal);
        var scope = isAbsolute ? null : currentNamespace;

        while (true)
        {
            var candidate = scope is null ? normalized : $"{scope}.{normalized}";
            if (symbols.TryGetConstant(candidate, out _))
            {
                qualifiedName = candidate;
                return true;
            }

            if (scope is null)
            {
                break;
            }

            var separator = scope.LastIndexOf('.');
            scope = separator < 0 ? null : scope[..separator];
        }

        qualifiedName = normalized;
        return false;
    }

    /// <summary>Resolves a scalar typedef chain in IDL type space.</summary>
    public string ResolveUnderlyingType(string idlType, string? currentNamespace)
    {
        var type = IdlNaming.NormalizeIdlType(idlType);
        var active = new HashSet<string>(StringComparer.Ordinal);

        while (!IdlNaming.IsPrimitive(type))
        {
            var qualified = IdlNaming.ResolveTypeName(type, currentNamespace);

            if (!symbols.TryGetTypedef(qualified, out var alias) || alias.IsCollection)
            {
                return IdlNaming.EscapeQualifiedIdentifier(qualified);
            }

            if (alias.IsString)
            {
                return "string";
            }

            if (!active.Add(qualified))
            {
                return IdlNaming.EscapeQualifiedIdentifier(qualified);
            }

            type = alias.Target;
            currentNamespace = alias.Namespace;
        }

        // Keep this resolver in IDL space. Emitters decide whether the normalized
        // primitive is represented as C# int, short, etc.
        return IdlNaming.NormalizeIdlType(type);
    }

    /// <summary>Maps a scalar alias value type to its native storage type.</summary>
    public string ResolveAliasNativeType(string resolvedType, string? currentNamespace)
    {
        if (IdlNaming.IsPrimitive(resolvedType))
        {
            return IdlNaming.MapPrimitive(resolvedType);
        }

        if (resolvedType is "bool" or "byte" or "sbyte" or "short" or "ushort" or
            "int" or "uint" or "long" or "ulong" or "char" or "float" or "double")
        {
            return resolvedType;
        }

        var qualified = IdlNaming.ResolveTypeName(resolvedType, currentNamespace);
        if (symbols.ContainsEnum(qualified))
        {
            return IdlNaming.EscapeQualifiedIdentifier(qualified);
        }

        var lastDot = qualified.LastIndexOf('.');
        if (lastDot < 0)
        {
            return $"{IdlNaming.EscapeQualifiedIdentifier(qualified)}Unmanaged";
        }

        return IdlNaming.EscapeQualifiedIdentifier($"{qualified[..lastDot]}.Implementation.{qualified[(lastDot + 1)..]}Unmanaged");
    }
}
