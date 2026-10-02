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

            if (currentNamespace is not null
                && !type.StartsWith("::", StringComparison.Ordinal)
                && !type.Contains(".", StringComparison.Ordinal)
                && !IsKnownType(qualified))
            {
                qualified = IdlNaming.ResolveTypeName(type, null);
            }

            if (!symbols.TryGetTypedef(qualified, out var alias) || alias.IsCollection)
            {
                return qualified;
            }

            if (alias.IsString)
            {
                return "string";
            }

            if (!active.Add(qualified))
            {
                return qualified;
            }

            type = alias.Target;
            currentNamespace = alias.Namespace;
        }

        // Keep this resolver in IDL space. Emitters decide whether the normalized
        // primitive is represented as C# int, short, etc.
        return IdlNaming.NormalizeIdlType(type);
    }

    private bool IsKnownType(string qualifiedName) =>
        symbols.ContainsName(qualifiedName)
        || symbols.ContainsEnum(qualifiedName)
        || symbols.ContainsTypedef(qualifiedName);

    /// <summary>Maps a resolved semantic type to its native C# reference.</summary>
    public string ResolveNativeType(IdlType type, string? currentNamespace)
    {
        if (type is IdlType.Primitive primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.Name).NativeStorageType;
        }

        if (type is IdlType.Enum enumType)
        {
            string? implementationNamespace;
            if (currentNamespace is null)
            {
                implementationNamespace = null;
            }
            else
            {
                implementationNamespace = $"{currentNamespace}.Implementation";
            }

            return IdlNaming.ResolvedTypeReference(enumType.QualifiedName, implementationNamespace);
        }

        var qualifiedName = type switch
        {
            IdlType.Alias alias => alias.QualifiedName,
            IdlType.Struct structure => structure.QualifiedName,
            IdlType.Union union => union.QualifiedName,
            _ => throw new InvalidOperationException($"Cannot resolve native type for {type.GetType().Name}.")
        };

        var normalized = qualifiedName.Replace("::", ".");
        var lastDot = normalized.LastIndexOf('.');
        string nativeName;
        if (lastDot < 0)
        {
            nativeName = $"Implementation.{normalized}Unmanaged";
        }
        else
        {
            nativeName = $"{normalized[..lastDot]}.Implementation.{normalized[(lastDot + 1)..]}Unmanaged";
        }

        string currentImplementationNamespace;
        if (currentNamespace is null)
        {
            currentImplementationNamespace = "Implementation";
        }
        else
        {
            currentImplementationNamespace = $"{currentNamespace}.Implementation";
        }

        return IdlNaming.ResolvedTypeReference(nativeName, currentImplementationNamespace);
    }
}
