using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Plans opt-in direct sequence arrays without changing the bound semantic model.</summary>
internal sealed class SequenceArrayEmissionPlan
{
    private readonly Dictionary<IdlMember, EmissionTypePlan> memberTypes = [];

    /// <summary>Gets sequence wrappers required by the selected mappings.</summary>
    internal IReadOnlyList<SequenceArrayElementPlan> Elements { get; }

    /// <summary>Plans wrappers, reserves their generated identities, and retains member ownership.</summary>
    internal SequenceArrayEmissionPlan(CompilationContext compilation, IdlSymbolTable symbols,
        IReadOnlyList<IdlDeclaration> declarations, RtiApiProfile profile)
    {
        var elements = new List<SequenceArrayElementPlan>();

        foreach (var declaration in declarations)
        {
            string declarationName;
            string? declarationNamespace;
            IEnumerable<IdlMember> members;
            string sourceIdlFileName;

            switch (declaration)
            {
                case IdlClassDeclaration classDeclaration:
                    declarationName = classDeclaration.Name;
                    declarationNamespace = classDeclaration.Namespace;
                    members = classDeclaration.Fields;
                    sourceIdlFileName = classDeclaration.SourceIdlFileName;
                    break;

                case IdlUnionDeclaration unionDeclaration:
                    declarationName = unionDeclaration.Declaration.Name;
                    declarationNamespace = unionDeclaration.Declaration.Namespace;
                    members = unionDeclaration.Declaration.Branches.Select(branch => branch.Field);
                    sourceIdlFileName = unionDeclaration.SourceIdlFileName;
                    break;

                default:
                    continue;
            }

            foreach (var member in members)
            {
                if (member.SourceInput?.SequenceArrayMapping != IdlSequenceArrayMapping.Idl
                    || member.Type is not IdlType.Sequence { Dimensions.Count: > 0 } sequence)
                {
                    continue;
                }

                var owner = declarationNamespace is null ? declarationName : $"{declarationNamespace}.{declarationName}";
                if (ReferencesOwner(sequence.Element, owner, symbols, new HashSet<string>(StringComparer.Ordinal)))
                {
                    throw new IdlException(member.SourceInput, member.SourceOffset,
                        "The Idl sequence-array mapping does not yet support recursive array elements. Select RtiCSharp for this file to retain the existing mapping.");
                }

                var names = ReserveNames(symbols, declarationNamespace, $"{declarationName}_{member.Name}Sequence");
                var element = EmissionTypeProjector.ToCollectionElementType(sequence.Element, declarationNamespace);
                var sequenceType = new SequenceEmissionType(element, sequence.EffectiveBound, $"ISequence<{element.CSharpType}>",
                    isBounded: sequence.Bound is not null, isUnbounded: sequence.IsUnbounded);
                var wrapper = new AliasEmissionType(names.RuntimeTypeName, sequenceType, names.RuntimeTypeName,
                    names.RuntimeTypeName, isAnonymousSequence: true);
                memberTypes.Add(member, new ArrayEmissionType(wrapper, sequence.Dimensions,
                    $"{names.RuntimeTypeName}[{new string(',', sequence.Dimensions.Count - 1)}]"));

                var nativeElement = sequence.Element;
                while (nativeElement is IdlType.Alias alias
                    && !string.Equals(IdlNaming.ResolvedTypeReference(alias.QualifiedName, declarationNamespace), element.CSharpType, StringComparison.Ordinal))
                {
                    nativeElement = alias.Target;
                }

                string? nativeType = null;
                if (nativeElement is not IdlType.StringType)
                {
                    nativeType = compilation.ResolveNativeType(nativeElement, declarationNamespace);
                }

                var requiresCast = true;
                if (EmissionTypeProjector.UnwrapValueEmissionType(element) is PrimitiveEmissionType primitive)
                {
                    requiresCast = PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeValueRequiresCast;
                }

                var aliasPlan = new CollectionAliasEmissionPlan(element, isSequence: true, isArray: false,
                    nativeValueRequiresCast: requiresCast, elementNativeType: nativeType,
                    sequenceBound: sequence.Bound, profile: profile, isSequenceArrayElement: true,
                    effectiveSequenceBound: sequence.EffectiveBound,
                    isUnboundedSequence: sequence.IsUnbounded);
                var helper = new IdlTypedef(names.ManagedTypeName, declarationNamespace, "sequence", elementType: null,
                    bound: sequence.Bound, effectiveSequenceBound: sequence.EffectiveBound);
                elements.Add(new SequenceArrayElementPlan(helper, aliasPlan, sourceIdlFileName));
            }
        }

        Elements = elements;
    }

    /// <summary>Returns the selected emission override for a member, including inherited references.</summary>
    internal EmissionTypePlan? GetMemberType(IdlMember member)
    {
        memberTypes.TryGetValue(member, out var type);
        return type;
    }

    private static GeneratedTypeNames ReserveNames(IdlSymbolTable symbols, string? currentNamespace, string preferredName)
    {
        var name = preferredName;
        while (true)
        {
            var names = IdlNaming.CreateGeneratedTypeNames(currentNamespace, name);
            if (symbols.AddGeneratedIdentities([names.RuntimeTypeName, names.SupportIdentity, names.PluginIdentity, names.UnmanagedIdentity]))
            {
                return names;
            }

            name += "_";
        }
    }

    private static bool ReferencesOwner(IdlType type, string owner, IdlSymbolTable symbols, HashSet<string> visited)
    {
        switch (type)
        {
            case IdlType.Alias alias:
                return ReferencesOwner(alias.Target, owner, symbols, visited);
            case IdlType.Sequence sequence:
                return ReferencesOwner(sequence.Element, owner, symbols, visited);
            case IdlType.Array array:
                return ReferencesOwner(array.Element, owner, symbols, visited);
            case IdlType.Struct structure:
                if (structure.QualifiedName == owner)
                {
                    return true;
                }

                return visited.Add(structure.QualifiedName)
                    && symbols.TryGetClass(structure.QualifiedName, out var declaration)
                    && declaration.InheritedFields.Concat(declaration.Fields)
                        .Any(member => ReferencesOwner(member.Type, owner, symbols, visited));
            case IdlType.Union union:
                return visited.Add(union.QualifiedName)
                    && symbols.TryGetUnion(union.QualifiedName, out var unionDeclaration)
                    && unionDeclaration.Branches.Any(branch => ReferencesOwner(branch.Field.Type, owner, symbols, visited));
            default:
                return false;
        }
    }
}

/// <summary>Retains the existing sequence-wrapper emission contract for one anonymous element.</summary>
internal sealed class SequenceArrayElementPlan(IdlTypedef declaration, CollectionAliasEmissionPlan plan, string sourceIdlFileName)
{
    /// <summary>Gets the generated wrapper declaration.</summary>
    internal IdlTypedef Declaration { get; } = declaration;

    /// <summary>Gets the shared managed/native sequence plan.</summary>
    internal CollectionAliasEmissionPlan Plan { get; } = plan;

    /// <summary>Gets the IDL file recorded in generated source headers.</summary>
    internal string SourceIdlFileName { get; } = sourceIdlFileName;
}
