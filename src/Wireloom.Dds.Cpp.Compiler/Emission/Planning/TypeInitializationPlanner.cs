using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Orders complete type dependencies and selects the staged RTI initialization path.</summary>
internal static class TypeInitializationPlanner
{
    /// <summary>Plans dependency order and the RTI metadata and plugin initialization stages.</summary>
    internal static TypeInitializationPlan Create(
        IReadOnlyList<StructPlan> structs,
        IReadOnlyList<UnionPlan> unions,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var structInitializationOrder = CreateStructInitializationOrder(structs, input, diagnostics);
        var typeInitializationOrder = CreateTypeInitializationOrder(structs, unions, input, diagnostics);
        var structShellsBeforeMetadata = HasMixedTypeDependencies(structs, unions)
            ? structInitializationOrder.Where(declaration => declaration.CppBaseType is not null).ToArray()
            : [];
        var requiresMetadataOnlyTypeInitialization = HasOptionalAggregateWithVariableLengthStorage(structs)
            || structs.Any(declaration => declaration.IsFlatData)
            || unions.Any(HasNumericCharLabel255);
        var requiresStagedInitialization = structs.Any(declaration =>
            declaration.CppBaseType is not null
            || declaration.Members.Any(member => ContainsSelfReference(member.Type, declaration.QualifiedName)));
        requiresStagedInitialization |= unions.Count > 0;
        requiresStagedInitialization |= structs.Any(declaration => declaration.IsFlatData);

        return new TypeInitializationPlan(typeInitializationOrder, structShellsBeforeMetadata,
            requiresStagedInitialization || requiresMetadataOnlyTypeInitialization,
            requiresMetadataOnlyTypeInitialization);
    }

    private static bool HasMixedTypeDependencies(
        IReadOnlyList<StructPlan> structs,
        IReadOnlyList<UnionPlan> unions) =>
        structs.SelectMany(declaration => declaration.Members).Any(member => ContainsUnion(member.Type))
        || unions.SelectMany(declaration => declaration.Branches).Any(branch => ContainsStruct(branch.Type));

    private static bool ContainsUnion(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Union => true,
        MemberTypePlan.Alias alias => ContainsUnion(alias.Target),
        MemberTypePlan.Sequence sequence => ContainsUnion(sequence.ElementType),
        MemberTypePlan.Array array => ContainsUnion(array.ElementType),
        _ => false,
    };

    private static bool ContainsStruct(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Struct => true,
        MemberTypePlan.Alias alias => ContainsStruct(alias.Target),
        MemberTypePlan.Sequence sequence => ContainsStruct(sequence.ElementType),
        MemberTypePlan.Array array => ContainsStruct(array.ElementType),
        _ => false,
    };

    private static bool HasOptionalAggregateWithVariableLengthStorage(IReadOnlyList<StructPlan> structs)
    {
        var byName = structs.ToDictionary(structure => structure.CppType, StringComparer.Ordinal);
        return structs.SelectMany(structure => structure.Members)
            .Where(member => member.IsOptional)
            .Any(member => IsOptionalAggregateWithVariableLengthStorage(
                member.Type, byName, new HashSet<string>(StringComparer.Ordinal)));
    }

    private static bool IsOptionalAggregateWithVariableLengthStorage(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structs,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias => IsOptionalAggregateWithVariableLengthStorage(alias.Target, structs, active),
        MemberTypePlan.Struct structure when structs.TryGetValue($"::{structure.QualifiedName}", out var declaration)
            && active.Add(declaration.CppType) => declaration.Members.Any(member =>
                ContainsVariableLengthStorage(member.Type, structs, active)),
        _ => false,
    };

    private static bool ContainsVariableLengthStorage(
        MemberTypePlan type,
        IReadOnlyDictionary<string, StructPlan> structs,
        ISet<string> active) => type switch
    {
        MemberTypePlan.Alias alias => ContainsVariableLengthStorage(alias.Target, structs, active),
        MemberTypePlan.Sequence => true,
        MemberTypePlan.String => true,
        MemberTypePlan.Array array => ContainsVariableLengthStorage(array.ElementType, structs, active),
        MemberTypePlan.Struct structure when structs.TryGetValue($"::{structure.QualifiedName}", out var declaration)
            && active.Add(declaration.CppType) => declaration.Members.Any(member =>
                ContainsVariableLengthStorage(member.Type, structs, active)),
        _ => false,
    };

    private static bool HasNumericCharLabel255(UnionPlan union) =>
        union.DiscriminatorType == "char"
        && union.Branches.SelectMany(branch => branch.Labels)
            .Any(label => label == byte.MaxValue);

    private static bool ContainsSelfReference(MemberTypePlan type, string qualifiedName)
    {
        return type switch
        {
            MemberTypePlan.Struct structure =>
                string.Equals(structure.QualifiedName, qualifiedName, StringComparison.Ordinal),
            MemberTypePlan.Sequence sequence => ContainsSelfReference(sequence.ElementType, qualifiedName),
            MemberTypePlan.Array array => ContainsSelfReference(array.ElementType, qualifiedName),
            MemberTypePlan.Alias alias => ContainsSelfReference(alias.Target, qualifiedName),
            _ => false,
        };
    }

    private static IReadOnlyList<TypeInitializationEntry> CreateTypeInitializationOrder(
        IReadOnlyList<StructPlan> structs,
        IReadOnlyList<UnionPlan> unions,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var declarations = new Dictionary<string, TypeInitializationEntry>(StringComparer.Ordinal);
        foreach (var declaration in structs)
        {
            declarations.Add(declaration.CppType, new TypeInitializationEntry(declaration, null));
        }

        foreach (var declaration in unions)
        {
            declarations.Add(declaration.CppType, new TypeInitializationEntry(null, declaration));
        }

        var dependencies = declarations.Keys.ToDictionary(
            name => name,
            _ => new HashSet<string>(StringComparer.Ordinal),
            StringComparer.Ordinal);
        foreach (var declaration in structs)
        {
            var node = declaration.CppType;
            if (declaration.CppBaseType is { } baseType && declarations.ContainsKey(baseType))
            {
                dependencies[node].Add(baseType);
            }

            foreach (var member in declaration.Members)
            {
                AddReferences(member.Type, node,
                    insideSequence: member.IsOptional && RtiSampleAccess.IsSelfRecursiveArrayPlan(member.Type, node.TrimStart(':')));
            }
        }

        foreach (var declaration in unions)
        {
            var node = declaration.CppType;
            foreach (var branch in declaration.Branches)
            {
                AddReferences(branch.Type, node, insideSequence: false);
            }
        }

        var complete = new HashSet<string>(StringComparer.Ordinal);
        var active = new List<string>();
        var ordered = new List<TypeInitializationEntry>(declarations.Count);
        bool Visit(string node)
        {
            if (complete.Contains(node))
            {
                return true;
            }

            var cycleIndex = active.IndexOf(node);
            if (cycleIndex >= 0)
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator does not support mutually recursive aggregate metadata or by-value recursive struct/union metadata involving '{node}'.");
                return false;
            }

            active.Add(node);
            foreach (var dependency in dependencies[node])
            {
                if (!Visit(dependency))
                {
                    return false;
                }
            }

            active.RemoveAt(active.Count - 1);
            complete.Add(node);
            ordered.Add(declarations[node]);
            return true;
        }

        foreach (var node in declarations.Keys)
        {
            if (!Visit(node))
            {
                return [];
            }
        }

        return ordered;

        void AddReferences(MemberTypePlan type, string source, bool insideSequence)
        {
            switch (type)
            {
                case MemberTypePlan.Struct structure:
                    AddReference($"::{structure.QualifiedName}", source, insideSequence);
                    break;
                case MemberTypePlan.Union union:
                    AddReference($"::{union.QualifiedName}", source, insideSequence);
                    break;
                case MemberTypePlan.Alias alias:
                    AddReferences(alias.Target, source, insideSequence);
                    break;
                case MemberTypePlan.Sequence sequence:
                    AddReferences(sequence.ElementType, source, insideSequence: true);
                    break;
                case MemberTypePlan.Array array:
                    AddReferences(array.ElementType, source, insideSequence);
                    break;
            }
        }

        void AddReference(string target, string source, bool insideSequence)
        {
            // Struct and union plans contain only successfully resolved references.
            if (string.Equals(target, source, StringComparison.Ordinal) && insideSequence)
            {
                return;
            }

            // A declaration with unsupported members is omitted from the plans; its own diagnostic rejects generation.
            if (declarations.ContainsKey(target))
            {
                dependencies[source].Add(target);
            }
        }
    }

    private static IReadOnlyList<StructPlan> CreateStructInitializationOrder(
        IReadOnlyList<StructPlan> structs,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var declarations = structs.ToDictionary(declaration => declaration.QualifiedName, StringComparer.Ordinal);
        var dependencies = structs.ToDictionary(
            declaration => declaration.QualifiedName,
            declaration => GetStructDependencies(declaration, declarations, input, diagnostics),
            StringComparer.Ordinal);
        var ordered = new List<StructPlan>(structs.Count);
        var complete = new HashSet<string>(StringComparer.Ordinal);
        var active = new HashSet<string>(StringComparer.Ordinal);

        bool Visit(StructPlan declaration)
        {
            if (complete.Contains(declaration.QualifiedName))
            {
                return true;
            }

            if (!active.Add(declaration.QualifiedName))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator does not support mutually recursive aggregate metadata involving '{declaration.QualifiedName}'.");
                return false;
            }

            foreach (var dependency in dependencies[declaration.QualifiedName])
            {
                if (!Visit(dependency))
                {
                    return false;
                }
            }

            active.Remove(declaration.QualifiedName);
            complete.Add(declaration.QualifiedName);
            ordered.Add(declaration);
            return true;
        }

        foreach (var declaration in structs)
        {
            if (!Visit(declaration))
            {
                break;
            }
        }

        return ordered;
    }

    private static IReadOnlyList<StructPlan> GetStructDependencies(
        StructPlan declaration,
        IReadOnlyDictionary<string, StructPlan> structs,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var dependencies = new HashSet<string>(StringComparer.Ordinal);
        if (declaration.BaseTypeName is { } baseTypeName)
        {
            dependencies.Add(baseTypeName);
        }

        foreach (var member in declaration.Members)
        {
            AddDependencies(member.Type,
                insideCollection: member.IsOptional && RtiSampleAccess.IsSelfRecursiveArrayPlan(member.Type, declaration.QualifiedName));
        }

        return dependencies
            .Where(structs.ContainsKey)
            .Select(dependency => structs[dependency])
            .ToArray();

        void AddDependencies(MemberTypePlan type, bool insideCollection)
        {
            switch (type)
            {
                case MemberTypePlan.Struct structure:
                    if (string.Equals(structure.QualifiedName, declaration.QualifiedName, StringComparison.Ordinal))
                    {
                        if (!insideCollection)
                        {
                            ProfileValidation.AddError(diagnostics, input, 0,
                                $"The C++ generator does not support recursive by-value member '{declaration.QualifiedName}'.");
                        }

                        return;
                    }

                    dependencies.Add(structure.QualifiedName);
                    break;
                case MemberTypePlan.Sequence sequence:
                    AddDependencies(sequence.ElementType, insideCollection: true);
                    break;
                case MemberTypePlan.Array array:
                    // Fixed arrays store each element inline. Only a sequence adds the
                    // indirection needed to represent a recursive value graph.
                    AddDependencies(array.ElementType, insideCollection);
                    break;
                case MemberTypePlan.Alias alias:
                    AddDependencies(alias.Target, insideCollection);
                    break;
            }
        }
    }
}

