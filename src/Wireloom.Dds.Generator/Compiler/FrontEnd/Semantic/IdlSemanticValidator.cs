using Wireloom.Compiler.FrontEnd.Parsing;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Owns front-end validation that does not produce target code.</summary>
internal sealed class IdlSemanticValidator(IdlSymbolTable symbols)
{
    private readonly HashSet<string> duplicateNames = new(StringComparer.Ordinal);

    /// <summary>Registers a declaration name while retaining duplicate evidence for validation.</summary>
    public void RegisterName(string name)
    {
        if (!symbols.AddName(name))
        {
            duplicateNames.Add(name);
        }
    }

    /// <summary>Validates a declaration name after parsing has registered all symbols.</summary>
    public void ValidateNewName(IdlInput input, int offset, string name)
    {
        if (duplicateNames.Contains(name))
        {
            throw new IdlException(input, offset, $"Duplicate type: {name}");
        }

        var generatedIdentity = IdlNaming.EscapeQualifiedIdentifier(name);
        if (!symbols.AddGeneratedIdentity(generatedIdentity))
        {
            throw new IdlException(input, offset, $"IDL declaration '{name}' collides with a generated type '{generatedIdentity}'.");
        }
    }

    /// <summary>Ensures generated companion names do not collide.</summary>
    public void EnsureGeneratedCompanionNames(
        IdlInput input,
        int offset,
        string declarationName,
        string? currentNamespace,
        bool includeUnmanaged)
    {
        var names = IdlNaming.CreateGeneratedTypeNames(currentNamespace, declarationName);
        var companions = new List<string>
        {
            names.SupportIdentity,
            names.PluginIdentity
        };

        if (includeUnmanaged)
        {
            companions.Add(names.UnmanagedIdentity);
        }

        if (!symbols.AddGeneratedIdentities(companions))
        {
            throw new IdlException(input, offset, $"IDL declaration '{declarationName}' collides with a generated companion type.");
        }
    }

    /// <summary>Validates a typedef declaration name.</summary>
    public void ValidateTypedef(IdlInput input, int offset, string name) =>
        ValidateTypedef(input, offset, name, new(StringComparer.Ordinal));

    /// <summary>Resolves and validates a collection bound.</summary>
    public int ResolveBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName = "Collection bound")
    {
        BigInteger value;

        try
        {
            value = IdlConstantExpressionEvaluator.Evaluate(text, symbols, currentNamespace);
        }
        catch (FormatException)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must resolve to a positive constant: {text.Trim()}");
        }

        if (value <= 0 || value > int.MaxValue)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must be a positive Int32: {text.Trim()}");
        }

        return (int)value;
    }

    /// <summary>Validates member identifiers within a declaration.</summary>
    public static void ValidateMemberIds(IdlInput input, int offset, IReadOnlyList<IdlMember> fields)
    {
        var duplicateId = fields
            .Where(field => field.Metadata.MemberId is not null)
            .GroupBy(field => field.Metadata.MemberId!.Value)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateId is not null)
        {
            throw new IdlException(input, offset, $"Duplicate member ID: {duplicateId.Key}");
        }
    }

    /// <summary>Validates the generated declaration name.</summary>
    public static void ValidateGeneratedDeclarationName(
        IdlInput input,
        int offset,
        string declarationName,
        IReadOnlyCollection<string> generatedMemberNames)
    {
        var escapedDeclarationName = IdlNaming.EscapeIdentifier(declarationName);
        if (generatedMemberNames.Contains(escapedDeclarationName, StringComparer.Ordinal))
        {
            throw new IdlException(input, offset, $"IDL declaration '{declarationName}' collides with a generated member.");
        }
    }

    /// <summary>Validates generated member-name collisions.</summary>
    public static void ValidateGeneratedNameCollisions(IdlInput input, int offset, string declarationName, IReadOnlyList<IdlMember> fields, string? baseType)
    {
        var escapedDeclarationName = IdlNaming.EscapeIdentifier(declarationName);
        if (escapedDeclarationName is "Equals" or "GetHashCode" or "ToString")
        {
            throw new IdlException(input, offset, $"IDL declaration '{declarationName}' collides with a generated member.");
        }

        var reservedNames = new HashSet<string>(StringComparer.Ordinal)
        {
            "Destroy",
            "Equals",
            "FromNative",
            "GetHashCode",
            "Initialize",
            "ToNative",
            "ToString"
        };

        if (baseType is not null)
        {
            reservedNames.Add("parent");
        }

        foreach (var field in fields.Where(field => field.Metadata.IsOptional && HasCollectionTemporary(field.Type)))
        {
            reservedNames.Add(IdlNaming.EscapeIdentifier($"{field.Name}Temporary_"));
        }

        foreach (var field in fields)
        {
            if (reservedNames.Contains(field.Name)
                || string.Equals(IdlNaming.EscapeIdentifier(field.Name), IdlNaming.EscapeIdentifier(declarationName), StringComparison.Ordinal))
            {
                throw new IdlException(input, offset, $"IDL member '{field.Name}' in '{declarationName}' collides with a generated member, parameter, or local variable.");
            }
        }
    }

    private static bool HasCollectionTemporary(IdlType type) => type switch
    {
        IdlType.Sequence or IdlType.Array => true,
        IdlType.Alias alias => HasCollectionTemporary(alias.Target),
        _ => false
    };

    /// <summary>Validates generated union-branch name collisions.</summary>
    public static void ValidateUnionGeneratedNameCollisions(IdlInput input, int offset, string declarationName, IReadOnlyList<IdlUnionBranch> branches)
    {
        var escapedDeclarationName = IdlNaming.EscapeIdentifier(declarationName);
        var reservedNames = new HashSet<string>(StringComparer.Ordinal)
        {
            "DefaultDiscriminator",
            "Destroy",
            "Discriminator",
            "Equals",
            "FromNative",
            "Get",
            "GetHashCode",
            "Initialize",
            "ToNative",
            "ToString",
            "_discriminator"
        };

        foreach (var branch in branches.Where(branch => branch.IsDefault || branch.Labels.Count > 1))
        {
            reservedNames.Add(IdlNaming.EscapeIdentifier($"Set{branch.Field.Name}"));
        }

        if (reservedNames.Contains(escapedDeclarationName))
        {
            throw new IdlException(input, offset, $"IDL union declaration '{declarationName}' collides with a generated member.");
        }

        foreach (var branch in branches)
        {
            if (reservedNames.Contains(branch.Field.Name)
                || string.Equals(IdlNaming.EscapeIdentifier(branch.Field.Name), IdlNaming.EscapeIdentifier(declarationName), StringComparison.Ordinal))
            {
                throw new IdlException(input, offset, $"IDL union branch '{branch.Field.Name}' in '{declarationName}' collides with a generated member, parameter, or native storage field.");
            }
        }

        foreach (var branch in branches.Where(branch => branch.IsDefault || branch.Labels.Count > 1))
        {
            var setterName = IdlNaming.EscapeIdentifier($"Set{branch.Field.Name}");
            if (branches.Any(candidate => string.Equals(IdlNaming.EscapeIdentifier(candidate.Field.Name), setterName, StringComparison.Ordinal)))
            {
                throw new IdlException(input, offset, $"IDL union branch '{setterName}' in '{declarationName}' collides with a generated branch setter.");
            }
        }
    }

    /// <summary>Validates a union's default discriminator.</summary>
    public static void ValidateUnionDefaultDiscriminator(IdlInput input, int offset, string discriminatorType, IReadOnlyList<IdlUnionBranch> branches)
    {
        if (!branches.Any(branch => branch.IsDefault)
            || !TryGetDiscriminatorRange(discriminatorType, out var minimum, out var maximum))
        {
            return;
        }

        var occupied = new HashSet<int>(branches
            .Where(branch => !branch.IsDefault)
            .SelectMany(branch => branch.LabelValues));

        if (HasUnoccupiedValue(occupied, minimum, maximum))
        {
            return;
        }

        if (discriminatorType == "boolean")
        {
            throw new IdlException(input, offset, "A boolean union with a default branch must leave either TRUE or FALSE unoccupied.");
        }

        throw new IdlException(input, offset, $"A {discriminatorType} union with a default branch has no representable discriminator value left for the default branch.");
    }

    private static bool TryGetDiscriminatorRange(string discriminatorType, out int minimum, out int maximum)
    {
        switch (IdlNaming.NormalizeIdlType(discriminatorType))
        {
            case "boolean":
                minimum = 0;
                maximum = 1;
                return true;
            case "char" or "wchar":
                minimum = char.MinValue;
                maximum = char.MaxValue;
                return true;
            case "int8":
                minimum = sbyte.MinValue;
                maximum = sbyte.MaxValue;
                return true;
            case "uint8" or "octet":
                minimum = byte.MinValue;
                maximum = byte.MaxValue;
                return true;
            case "short" or "int16":
                minimum = short.MinValue;
                maximum = short.MaxValue;
                return true;
            case "unsigned short" or "uint16":
                minimum = ushort.MinValue;
                maximum = ushort.MaxValue;
                return true;
            default:
                minimum = int.MinValue;
                maximum = int.MaxValue;
                return false;
        }
    }

    private static bool HasUnoccupiedValue(HashSet<int> occupied, int minimum, int maximum)
    {
        for (var candidate = 0; candidate <= maximum; candidate++)
        {
            if (!occupied.Contains(candidate))
            {
                return true;
            }
        }

        for (var candidate = minimum; candidate < 0; candidate++)
        {
            if (!occupied.Contains(candidate))
            {
                return true;
            }
        }

        return false;
    }

    private void ValidateTypedef(IdlInput input, int offset, string name, HashSet<string> activeAliases)
    {
        if (!symbols.TryGetTypedef(name, out var alias))
        {
            return;
        }

        if (!activeAliases.Add(name))
        {
            throw new IdlException(input, offset, $"Typedef alias cycle detected: {name}");
        }

        if (alias.IsString)
        {
            activeAliases.Remove(name);
            return;
        }

        var target = alias.IsCollection ? alias.ElementType! : alias.Target;
        if (!target.StartsWith("string", StringComparison.Ordinal)
            && !target.StartsWith("wstring", StringComparison.Ordinal)
            && !IdlNaming.IsPrimitive(target))
        {
            var qualifiedTarget = IdlNaming.ResolveTypeName(target, alias.Namespace);
            if (!HasKnownType(qualifiedTarget)
                && alias.Namespace is not null
                && !target.StartsWith("::", StringComparison.Ordinal)
                && !target.Contains('.'))
            {
                qualifiedTarget = IdlNaming.ResolveTypeName(target, null);
            }

            if (symbols.ContainsTypedef(qualifiedTarget))
            {
                ValidateTypedef(input, offset, qualifiedTarget, activeAliases);
            }
            else if (!HasKnownType(qualifiedTarget))
            {
                throw new IdlException(input, offset, $"Unknown typedef target: {target}");
            }
        }

        activeAliases.Remove(name);
    }

    private bool HasKnownType(string qualifiedName) =>
        symbols.ContainsTypedef(qualifiedName)
        || symbols.ContainsEnum(qualifiedName)
        || symbols.ContainsName(qualifiedName);
}
