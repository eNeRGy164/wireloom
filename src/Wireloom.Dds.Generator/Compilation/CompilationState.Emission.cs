namespace Wireloom;

using static Wireloom.IdlCompiler;

internal sealed partial class CompilationState
{
    /// <summary>Resolves a scalar typedef chain for the legacy alias emitter.</summary>
    public string ResolveUnderlyingType(string idlType, string? currentNamespace)
    {
        var type = NormalizeIdlType(idlType);
        var active = new HashSet<string>(StringComparer.Ordinal);

        while (!IsPrimitive(type))
        {
            var qualified = ResolveTypeName(type, currentNamespace);

            if (!symbols.TryGetTypedef(qualified, out var alias) || alias.IsCollection)
            {
                return EscapeQualifiedIdentifier(qualified);
            }

            if (alias.IsString)
            {
                return "string";
            }

            if (!active.Add(qualified))
            {
                return EscapeQualifiedIdentifier(qualified);
            }

            type = alias.Target;
            currentNamespace = alias.Namespace;
        }

        // Keep this resolver in IDL space. Emitters decide whether the
        // normalized primitive is represented as C# int, short, etc.
        return NormalizeIdlType(type);
    }

    /// <summary>Maps a scalar alias value type to its native storage type.</summary>
    public string ResolveAliasNativeType(string resolvedType, string? currentNamespace)
    {
        if (IsPrimitive(resolvedType))
        {
            return MapPrimitive(resolvedType);
        }

        if (resolvedType is "bool" or "byte" or "sbyte" or "short" or "ushort" or
            "int" or "uint" or "long" or "ulong" or "char" or "float" or "double")
        {
            return resolvedType;
        }

        var qualified = ResolveTypeName(resolvedType, currentNamespace);
        if (symbols.ContainsEnum(qualified))
        {
            return EscapeQualifiedIdentifier(qualified);
        }

        var lastDot = qualified.LastIndexOf('.');
        if (lastDot < 0)
        {
            return $"{EscapeQualifiedIdentifier(qualified)}Unmanaged";
        }

        return EscapeQualifiedIdentifier($"{qualified[..lastDot]}.Implementation.{qualified[(lastDot + 1)..]}Unmanaged");
    }
}
