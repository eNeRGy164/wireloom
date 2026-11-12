using Wireloom.Dds.Idl.Compiler.FrontEnd.Symbols;

namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Owns front-end validation that does not produce target code.</summary>
internal sealed class IdlSemanticValidator(IdlSymbolTable symbols)
{
    private readonly HashSet<string> duplicateNames = new(StringComparer.Ordinal);
    private readonly Dictionary<string, (IdlInput Input, int Offset)> forwardStructs = new(StringComparer.Ordinal);
    private readonly HashSet<string> definedStructs = new(StringComparer.Ordinal);

    /// <summary>Checks whether a resolved type name has an incomplete struct declaration.</summary>
    public bool IsIncompleteForwardStruct(string name) =>
        forwardStructs.ContainsKey(name) && !definedStructs.Contains(name);

    /// <summary>Registers a declaration name while retaining duplicate evidence for validation.</summary>
    public void RegisterName(string name, bool isType)
    {
        if (!symbols.AddName(name))
        {
            duplicateNames.Add(name);
        }

        if (isType)
        {
            symbols.AddTypeName(name);
        }
    }

    /// <summary>Registers an IDL struct forward declaration for later type references.</summary>
    public void RegisterForwardStruct(string name, IdlInput input, int offset)
    {
        var isKnownForward = forwardStructs.ContainsKey(name);
        var isDefinedStruct = definedStructs.Contains(name);
        if (!symbols.AddName(name) && !isKnownForward && !isDefinedStruct)
        {
            duplicateNames.Add(name);
        }

        symbols.AddTypeName(name);

        forwardStructs.TryAdd(name, (input, offset));
    }

    /// <summary>Registers a struct definition, completing a preceding forward declaration when present.</summary>
    public void RegisterStructDefinition(string name, bool isType)
    {
        if (!definedStructs.Add(name))
        {
            duplicateNames.Add(name);
            return;
        }

        if (forwardStructs.ContainsKey(name))
        {
            return;
        }

        RegisterName(name, isType);
    }

    /// <summary>Validates unresolved struct forward declarations after parsing the complete IDL input.</summary>
    public void ValidateForwardStructs()
    {
        foreach (var forwardStruct in forwardStructs)
        {
            var name = forwardStruct.Key;
            var (Input, Offset) = forwardStruct.Value;
            if (!definedStructs.Contains(name))
            {
                throw new IdlException(Input, Offset, $"Forward-declared struct '{name}' has no definition.");
            }
        }
    }

    /// <summary>Validates a declaration name after parsing has registered all symbols.</summary>
    public void ValidateNewName(IdlInput input, int offset, string name)
    {
        if (duplicateNames.Contains(name))
        {
            throw new IdlException(input, offset, $"Duplicate type: {name}");
        }
    }

    /// <summary>Validates a typedef declaration name.</summary>
    public void ValidateTypedef(IdlInput input, int offset, string name) =>
        ValidateTypedef(input, offset, name, new(StringComparer.Ordinal));

    /// <summary>Resolves and validates a collection bound.</summary>
    public int ResolveBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName = "Collection bound")
        => IdlBoundResolver.Resolve(input, offset, text, currentNamespace, symbols, diagnosticName);

    /// <summary>Validates member identifiers within a declaration.</summary>
    public static void ValidateMemberIds(IdlInput input, int offset, IReadOnlyList<IdlMember> fields)
    {
        var duplicateId = fields
            .Where(field => field.Metadata.EffectiveMemberId is not null)
            .GroupBy(field => field.Metadata.EffectiveMemberId!.Value)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateId is not null)
        {
            throw new IdlException(input, offset, $"Duplicate member ID: {duplicateId.Key}");
        }
    }

    /// <summary>Rejects recursive aggregate members that would create a target-language value cycle.</summary>
    public static void ValidateValueTypeCycles(IReadOnlyList<IdlDeclaration> declarations)
    {
        var classes = declarations
            .OfType<IdlClassDeclaration>()
            .ToDictionary(declaration => IdlNames.ResolveTypeName(declaration.Name, declaration.Namespace), StringComparer.Ordinal);
        var unions = declarations
            .OfType<IdlUnionDeclaration>()
            .ToDictionary(declaration => IdlNames.ResolveTypeName(declaration.Declaration.Name, declaration.Declaration.Namespace), StringComparer.Ordinal);
        var visiting = new HashSet<string>(StringComparer.Ordinal);
        var visited = new HashSet<string>(StringComparer.Ordinal);

        foreach (var name in classes.Keys.Concat(unions.Keys))
        {
            Visit(name);
        }

        void Visit(string name)
        {
            if (!visited.Add(name))
            {
                return;
            }

            visiting.Add(name);
            var fields = classes.TryGetValue(name, out var classDeclaration)
                ? classDeclaration.Fields.Concat(classDeclaration.InheritedFields)
                : unions[name].Declaration.Branches.Select(branch => branch.Field);

            foreach (var field in fields)
            {
                var valueType = field.Type;
                while (valueType is IdlType.Alias alias)
                {
                    valueType = alias.Target;
                }

                if (field.Metadata.IsOptional && valueType is IdlType.Array)
                {
                    continue;
                }

                var target = GetValueTypeName(field.Type);
                if (target is null)
                {
                    continue;
                }

                if (visiting.Contains(target))
                {
                    throw new IdlException(
                        field.SourceInput!,
                        field.SourceOffset,
                        $"Recursive value-type member '{field.Name}' creates a type cycle through '{target}'. Use a sequence to represent recursive data.");
                }

                Visit(target);
            }

            visiting.Remove(name);
        }
    }

    private static string? GetValueTypeName(IdlType type) => type switch
    {
        IdlType.Struct structure => structure.QualifiedName,
        IdlType.Union union => union.QualifiedName,
        IdlType.Alias alias => GetValueTypeName(alias.Target),
        IdlType.Array array => GetValueTypeName(array.Element),
        _ => null
    };

    /// <summary>Evaluates specification rules and applies the batch policy before emission.</summary>
    internal void ValidateSpecificationRules(IReadOnlyList<IdlIdentifierOccurrence> identifiers, IReadOnlyList<IdlDeclaration> declarations, bool strict, ICollection<IdlDiagnostic>? diagnostics, CancellationToken cancellationToken)
    {
        foreach (var violation in IdlIdentifierValidator.FindViolations(identifiers, cancellationToken))
        {
            IdlValidationPolicy.Apply(violation, strict, diagnostics);
        }

        foreach (var declaration in declarations.OfType<IdlClassDeclaration>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var violation = FindDerivedKeyViolation(declaration);
            if (violation is not null)
            {
                IdlValidationPolicy.Apply(violation, strict, diagnostics);
            }
        }
    }

    /// <summary>Detects a new key in a derived aggregate without choosing its severity.</summary>
    private static IdlSpecificationViolation? FindDerivedKeyViolation(IdlClassDeclaration declaration)
    {
        if (declaration.BaseType is null)
        {
            return null;
        }

        var keyField = declaration.Fields.FirstOrDefault(field => field.Metadata.IsKey);
        if (keyField is null)
        {
            return null;
        }

        return new IdlSpecificationViolation(
            IdlSpecificationRule.DerivedKey,
            keyField.SourceInput ?? declaration.SourceInput,
            keyField.SourceOffset,
            keyField.Name);
    }

    /// <summary>Rejects duplicate explicit discriminator values across union branches.</summary>
    public static void ValidateUnionDiscriminatorLabels(
        IdlInput input,
        IReadOnlyList<IdlUnionBranch> branches)
    {
        var labels = new HashSet<long>();

        foreach (var branch in branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.LabelValues)
            {
                if (!labels.Add(label))
                {
                    throw new IdlException(
                        input,
                        branch.Field.SourceOffset,
                        $"Duplicate union discriminator label: {label}");
                }
            }
        }
    }

    /// <summary>Validates a union's default discriminator.</summary>
    public static void ValidateUnionDefaultDiscriminator(IdlInput input, int offset, string? discriminatorType, IReadOnlyList<IdlUnionBranch> branches)
    {
        if (!branches.Any(branch => branch.IsDefault)
            || discriminatorType is null
            || !TryGetDiscriminatorRange(discriminatorType, out var minimum, out var maximum))
        {
            return;
        }

        var occupied = new HashSet<long>(branches
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

    private static bool TryGetDiscriminatorRange(string discriminatorType, out long minimum, out long maximum)
    {
        switch (IdlNames.NormalizeIdlType(discriminatorType))
        {
            case "boolean":
                minimum = 0;
                maximum = 1;
                return true;
            case "wchar":
                minimum = char.MinValue;
                maximum = char.MaxValue;
                return true;
            case "int8":
                minimum = sbyte.MinValue;
                maximum = sbyte.MaxValue;
                return true;
            case "char" or "uint8" or "octet":
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
            case "unsigned long" or "uint32":
                minimum = uint.MinValue;
                maximum = uint.MaxValue;
                return true;
            default:
                minimum = int.MinValue;
                maximum = int.MaxValue;
                return false;
        }
    }

    private static bool HasUnoccupiedValue(HashSet<long> occupied, long minimum, long maximum) =>
        occupied.Count < maximum - minimum + 1;

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
        if (!IdlBuiltinTypeSyntax.TryParseStringType(target, out _, out _)
            && !IdlNames.IsPrimitive(target))
        {
            if (!symbols.TryResolveTypeName(target, alias.Namespace, out var qualifiedTarget))
            {
                throw new IdlException(input, offset, $"Unknown typedef target: {target}");
            }

            if (symbols.ContainsTypedef(qualifiedTarget))
            {
                ValidateTypedef(input, offset, qualifiedTarget, activeAliases);
            }
        }

        activeAliases.Remove(name);
    }
}
