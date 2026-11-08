using Wireloom.Compiler.FrontEnd;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Applies C# compatibility diagnostics while the front end retains source order.</summary>
/// <param name="identities">The generated name reservations for this compilation.</param>
internal sealed class CSharpAnalysisChecks(GeneratedIdentitySet identities) : IdlTargetChecks
{
    private static readonly string[] GeneratedTypedefMemberNames = ["Value", "Equals", "GetHashCode", "ToString"];

    /// <inheritdoc />
    public void ValidateInput(IdlInput input)
    {
        if (input.SequenceArrayMapping is not IdlSequenceArrayMapping.RtiCSharp and not IdlSequenceArrayMapping.Idl)
        {
            throw new IdlException(input, 0, "SequenceArrayMapping must be 'RtiCSharp' or 'Idl'.");
        }
    }

    /// <inheritdoc />
    public void ObserveMemberType(IdlInput input, int offset, string name, IdlType type, ICollection<IdlDiagnostic>? diagnostics)
    {
        if (type is IdlType.Sequence { Dimensions.Count: > 0 } && input.SequenceArrayMapping == IdlSequenceArrayMapping.RtiCSharp)
        {
            diagnostics?.Add(new IdlDiagnostic("DDSG0105", input, offset,
                $"The C# binding does not support arrays of sequences without using a typedef; generated code for member '{name}' may not match IDL semantics."));
        }
    }

    /// <inheritdoc />
    public void ReportInterface(IdlInput input, int offset, string name, bool isDdsService, ICollection<IdlDiagnostic>? diagnostics)
    {
        var message = isDdsService
            ? $"The DDS service interface '{name}' is ignored because service interfaces are not emitted for C#."
            : $"The interface '{name}' is ignored because it is not a DDS service.";
        diagnostics?.Add(new IdlDiagnostic("DDSG0103", input, offset, message));
    }

    /// <inheritdoc />
    public void ValidateIdentity(IdlInput input, int offset, string name)
    {
        var generatedIdentity = IdlNaming.EscapeQualifiedIdentifier(name);
        if (!identities.AddGeneratedIdentity(generatedIdentity))
        {
            throw new IdlException(input, offset, $"IDL declaration '{name}' collides with a generated type '{generatedIdentity}'.");
        }
    }

    /// <inheritdoc />
    public void ValidateDeclarationName(IdlInput input, int offset, string name, IdlDeclarationKind kind)
    {
        var generatedNames = kind == IdlDeclarationKind.Typedef ? GeneratedTypedefMemberNames : ["Value"];
        ValidateGeneratedDeclarationName(input, offset, name, generatedNames);
    }

    /// <summary>Ensures generated companion names do not collide.</summary>
    public void ValidateCompanions(
        IdlInput input,
        int offset,
        string declarationName,
        string? currentNamespace,
        IdlDeclarationKind kind)
    {
        var names = IdlNaming.CreateGeneratedTypeNames(currentNamespace, declarationName);
        var companions = new List<string>
        {
            names.SupportIdentity,
            names.PluginIdentity
        };

        if (kind != IdlDeclarationKind.Enum)
        {
            companions.Add(names.UnmanagedIdentity);
        }

        if (!identities.AddGeneratedIdentities(companions))
        {
            throw new IdlException(input, offset, $"IDL declaration '{declarationName}' collides with a generated companion type.");
        }
    }

    /// <summary>Rejects recursive sequence typedef members that RTI C++ cannot generate as compilable TypeSupport.</summary>
    public void ValidateDeclarations(IReadOnlyList<IdlDeclaration> declarations)
    {
        var classes = declarations
            .OfType<IdlClassDeclaration>()
            .ToDictionary(declaration => IdlNaming.ResolveTypeName(declaration.Name, declaration.Namespace), StringComparer.Ordinal);
        var sequenceEdges = classes.Keys.ToDictionary(
            name => name,
            _ => new HashSet<string>(StringComparer.Ordinal),
            StringComparer.Ordinal);

        foreach (var declaration in classes.Values)
        {
            var owner = IdlNaming.ResolveTypeName(declaration.Name, declaration.Namespace);
            foreach (var field in declaration.Fields)
            {
                AddSequenceTargets(field.Type, throughSequence: false, sequenceEdges[owner], classes.Keys);
            }
        }

        foreach (var declaration in classes.Values)
        {
            var owner = IdlNaming.ResolveTypeName(declaration.Name, declaration.Namespace);
            foreach (var field in declaration.Fields)
            {
                if (field.Type is not IdlType.Alias)
                {
                    continue;
                }

                var sequenceTargets = new HashSet<string>(StringComparer.Ordinal);
                AddSequenceTargets(field.Type, throughSequence: false, sequenceTargets, classes.Keys);
                foreach (var target in sequenceTargets)
                {
                    if (CanReach(target, owner, sequenceEdges))
                    {
                        throw new IdlException(
                            field.SourceInput!,
                            field.SourceOffset,
                            $"Recursive collection typedef member '{field.Name}' is unsupported for wire compatibility with RTI C++ peers because rtiddsgen emits uncompilable TypeCode code for this shape on versions 7.3.1 and 7.7.0. Use a direct recursive sequence member until a working RTI C++ path is available.");
                    }
                }
            }
        }

        static void AddSequenceTargets(
            IdlType type,
            bool throughSequence,
            ISet<string> targets,
            IEnumerable<string> knownTypes)
        {
            switch (type)
            {
                case IdlType.Alias alias:
                    AddSequenceTargets(alias.Target, throughSequence, targets, knownTypes);
                    break;
                case IdlType.Sequence sequence:
                    AddSequenceTargets(sequence.Element, throughSequence: true, targets, knownTypes);
                    break;
                case IdlType.Array array:
                    AddSequenceTargets(array.Element, throughSequence, targets, knownTypes);
                    break;
                case IdlType.Struct structure when throughSequence && knownTypes.Contains(structure.QualifiedName):
                    targets.Add(structure.QualifiedName);
                    break;
            }
        }

        static bool CanReach(string start, string expected, IReadOnlyDictionary<string, HashSet<string>> edges)
        {
            var pending = new Stack<string>();
            var visited = new HashSet<string>(StringComparer.Ordinal);
            pending.Push(start);

            while (pending.Count > 0)
            {
                var current = pending.Pop();
                if (string.Equals(current, expected, StringComparison.Ordinal))
                {
                    return true;
                }

                if (!visited.Add(current))
                {
                    continue;
                }

                foreach (var target in edges[current])
                {
                    pending.Push(target);
                }
            }

            return false;
        }
    }

    /// <summary>Validates the generated declaration name.</summary>
    private static void ValidateGeneratedDeclarationName(
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
    public void ValidateStructNames(IdlInput input, int offset, string declarationName, IReadOnlyList<IdlMember> fields, string? baseType)
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

        foreach (var field in fields.Where(field => field.Metadata.IsOptional && HasFromNativeTemporary(field.Type)))
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

    private static bool HasFromNativeTemporary(IdlType type) => type switch
    {
        IdlType.Sequence or IdlType.Array or IdlType.Struct or IdlType.Union => true,
        IdlType.Alias alias => HasFromNativeTemporary(alias.Target),
        _ => false
    };

    /// <summary>Validates generated union-branch name collisions.</summary>
    public void ValidateUnionNames(IdlInput input, int offset, string declarationName, IReadOnlyList<IdlUnionBranch> branches)
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
}
