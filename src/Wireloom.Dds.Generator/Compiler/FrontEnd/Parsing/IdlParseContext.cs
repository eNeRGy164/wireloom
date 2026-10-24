using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Provides the shared state and services used while parsing one IDL compilation.</summary>
internal sealed class IdlParseContext
{
    internal IdlParseContext(IdlSymbolTable symbols, CancellationToken cancellationToken, ICollection<IdlDiagnostic>? diagnostics)
    {
        Symbols = symbols;
        TypeBinder = new IdlTypeBinder(symbols);
        InheritanceBinder = new IdlInheritanceBinder(symbols);
        Validator = new IdlSemanticValidator(symbols);
        CancellationToken = cancellationToken;
        Diagnostics = diagnostics;
        TypeParser = new IdlTypeParser(this);
        ValueBinder = new IdlValueBinder(symbols, cancellationToken);
    }

    internal IdlSymbolTable Symbols { get; }

    private IdlTypeBinder TypeBinder { get; }

    private IdlInheritanceBinder InheritanceBinder { get; }

    private IdlSemanticValidator Validator { get; }

    internal List<IdlDeclaration> Declarations { get; } = [];

    private Dictionary<string, (IdlInput Input, int Offset)> TypedefLocations { get; } = new(StringComparer.Ordinal);

    private List<IdlDeferredBound> DeferredBounds { get; } = [];


    private List<Action> DeferredValidation { get; } = [];

    private List<IdlIdentifierOccurrence> Identifiers { get; } = [];

    internal CancellationToken CancellationToken { get; }

    internal ICollection<IdlDiagnostic>? Diagnostics { get; }

    internal IdlTypeParser TypeParser { get; }

    /// <summary>Gets the semantic value binder for this compilation.</summary>
    internal IdlValueBinder ValueBinder { get; }

    private IReadOnlyList<SourceOriginSpan>? SourceOrigins { get; set; }

    /// <summary>Sets source-origin mappings for the current parsed input.</summary>
    internal void SetSourceOrigins(IReadOnlyList<SourceOriginSpan>? sourceOrigins) =>
        SourceOrigins = sourceOrigins;

    /// <summary>Maps a preprocessed offset to its original source offset.</summary>
    internal int MapOffset(int offset)
        => MapOffset(offset, SourceOrigins);

    /// <summary>Reports unknown applications captured between declaration-prefix annotations.</summary>
    internal void ReportUnknownTypePrefixApplications(IdlInput input, IEnumerable<IdlAnnotationApplicationSyntax> applications)
    {
        foreach (var application in applications)
        {
            Diagnostics?.Add(new IdlDiagnostic(
                "DDSG0101",
                input,
                application.Offset,
                $"Annotation '@{application.Name}' is not recognized and will be ignored."));
        }
    }

    /// <summary>Maps a preprocessed offset using an explicitly captured origin table.</summary>
    private static int MapOffset(int offset, IReadOnlyList<SourceOriginSpan>? sourceOrigins)
    {
        if (sourceOrigins is null || sourceOrigins.Count == 0 || offset < 0)
        {
            return offset;
        }

        var low = 0;
        var high = sourceOrigins.Count - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var origin = sourceOrigins[middle];
            if (offset < origin.OutputStart)
            {
                high = middle - 1;
            }
            else if (offset >= origin.OutputStart + origin.OutputLength)
            {
                low = middle + 1;
            }
            else
            {
                return origin.Map(offset);
            }
        }

        return sourceOrigins[^1].SourceStart;
    }

    /// <summary>Qualifies a declaration name against a namespace.</summary>
    internal string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    /// <summary>Adds a parsed class declaration to the compiler symbol table.</summary>
    internal void AddClass(string qualifiedName, IdlClassDeclaration declaration) =>
        Symbols.AddClass(qualifiedName, declaration);

    /// <summary>Adds a parsed typedef declaration and validates its name.</summary>
    internal void AddTypedef(string qualifiedName, IdlTypedef declaration, IdlInput input, int offset)
    {
        Symbols.AddTypedef(qualifiedName, declaration);
        TypedefLocations.TryAdd(qualifiedName, (input, MapOffset(offset)));
    }

    /// <summary>Validates all deferred semantic rules against the complete symbol graph with the requested strictness.</summary>
    internal void Validate(bool strict)
    {
        foreach (var typedef in TypedefLocations)
        {
            DeferredValidation.Add(() => Validator.ValidateTypedef(typedef.Value.Input, typedef.Value.Offset, typedef.Key));
        }

        foreach (var validation in DeferredValidation)
        {
            validation();
        }

        Validator.ValidateSpecificationRules(Identifiers, Declarations, strict, Diagnostics, CancellationToken);

        IdlSemanticValidator.ValidateValueTypeCycles(Declarations);
        IdlSemanticValidator.ValidateRecursiveSequenceTypedefs(Declarations);
    }

    /// <summary>Binds all parsed declarations against the complete symbol graph.</summary>
    internal void BindDeclarations()
    {
        Validator.ValidateForwardStructs();
        ValueBinder.BindDiscreteConstants(Declarations);
        foreach (var bound in DeferredBounds)
        {
            bound.Resolve(Validator);
        }

        ValueBinder.BindTextAndFloatingConstants(Declarations);
        ValueBinder.BindTypedefDefaults(Declarations, TypedefLocations);
        InheritanceBinder.Bind(Declarations.OfType<IdlClassDeclaration>().ToArray());

        foreach (var declaration in Declarations)
        {
            switch (declaration)
            {
                case IdlClassDeclaration @class:
                    foreach (var field in @class.Fields)
                    {
                        var bound = TypeBinder.Bind(field.Type);
                        field.Bind(bound);
                    }

                    break;
                case IdlUnionDeclaration union:
                    var discriminatorReference = new IdlType.Reference(
                        new IdlTypeReference(
                            union.Declaration.DiscriminatorIdlType,
                            union.Declaration.Namespace,
                            union.Declaration.DiscriminatorInput,
                            union.Declaration.DiscriminatorOffset),
                        "Unknown union discriminator type");
                    var resolvedDiscriminator = TypeBinder.Bind(discriminatorReference);
                    var discriminatorEnum = FindEnum(resolvedDiscriminator);
                    union.Declaration.BindDiscriminator(
                        resolvedDiscriminator,
                        discriminatorEnum);

                    foreach (var branch in union.Declaration.Branches)
                    {
                        branch.BindLabels(
                            union.Declaration.DiscriminatorPrimitiveIdlType,
                            discriminatorEnum,
                            Symbols,
                            union.Declaration.Namespace);
                        branch.Field.Bind(TypeBinder.Bind(branch.Field.Type));
                    }

                    DeferUnionDiscriminatorLabelValidation(
                        union.Declaration.DiscriminatorInput,
                        union.Declaration.Branches);

                    DeferUnionDefaultDiscriminatorValidation(
                        union.Declaration.DiscriminatorInput,
                        union.Declaration.DiscriminatorOffset,
                        union.Declaration.DiscriminatorPrimitiveIdlType,
                        union.Declaration.Branches);

                    break;
            }
        }

        ValueBinder.BindMemberValues();
        IdlMemberIdBinder.Bind(Declarations.OfType<IdlClassDeclaration>());
    }

    private IdlEnum? FindEnum(IdlType type)
    {
        if (type is IdlType.Alias alias)
        {
            return FindEnum(alias.Target);
        }

        if (type is IdlType.Enum enumType && Symbols.TryGetEnum(enumType.QualifiedName, out var enumDeclaration))
        {
            return enumDeclaration;
        }

        return null;
    }

    /// <summary>Captures an identifier and its original location without applying validation policy.</summary>
    internal void RecordIdentifier(IdlInput input, int offset, string name, string? scope, IdlIdentifierScopeKind scopeKind = IdlIdentifierScopeKind.Namespace)
    {
        Identifiers.Add(new IdlIdentifierOccurrence(name, scope ?? string.Empty, scopeKind, input, MapOffset(offset)));
    }

    /// <summary>Records a declaration name and defers its semantic validation.</summary>
    internal void EnsureNewName(IdlInput input, int offset, string name, bool isType = true)
    {
        Validator.RegisterName(name, isType);
        var mappedOffset = MapOffset(offset);
        DeferValidation(() => Validator.ValidateNewName(input, mappedOffset, name));
    }

    /// <summary>Registers a struct forward declaration and makes its type name available for later binding.</summary>
    internal void EnsureForwardStruct(IdlInput input, int offset, string name)
    {
        var mappedOffset = MapOffset(offset);
        Validator.RegisterForwardStruct(name, input, mappedOffset);
    }

    /// <summary>Registers a struct definition and validates completion of a preceding forward declaration.</summary>
    internal void EnsureStructDefinitionName(IdlInput input, int offset, string name)
    {
        Validator.RegisterStructDefinition(name, isType: true);

        if (Validator.HasForwardStruct(name))
        {
            var mappedOffset = MapOffset(offset);
            DeferValidation(() => Validator.ValidateForwardStructDefinition(input, mappedOffset, name));
            return;
        }

        var definitionOffset = MapOffset(offset);
        DeferValidation(() => Validator.ValidateNewName(input, definitionOffset, name));
    }

    /// <summary>Ensures generated companion names do not collide with declarations.</summary>
    internal void EnsureGeneratedCompanionNames(IdlInput input, int offset, string name, string? currentNamespace, bool includeUnmanaged)
    {
        var mappedOffset = MapOffset(offset);
        DeferValidation(() => Validator.EnsureGeneratedCompanionNames(input, mappedOffset, name, currentNamespace, includeUnmanaged));
    }

    /// <summary>Records a bound expression for resolution during binding.</summary>
    internal IdlDeferredBound DeferBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName)
    {
        var bound = new IdlDeferredBound(input, MapOffset(offset), text, currentNamespace, diagnosticName);
        DeferredBounds.Add(bound);
        return bound;
    }

    /// <summary>Queues one semantic validation action for the validation phase.</summary>
    private void DeferValidation(Action validation) => DeferredValidation.Add(validation);

    /// <summary>Queues member-identifier validation for the validation phase.</summary>
    internal void DeferMemberIdValidation(IdlInput input, int offset, IdlClassDeclaration declaration) =>
        DeferValidation(() => IdlSemanticValidator.ValidateMemberIds(input, offset,
            [.. declaration.InheritedFields, .. declaration.Fields]));

    /// <summary>Queues member-ID collision checks for union branches.</summary>
    internal void DeferUnionMemberIdValidation(IdlInput input, int offset, IReadOnlyList<IdlUnionBranch> branches) =>
        DeferValidation(() => IdlSemanticValidator.ValidateMemberIds(input, offset, branches.Select(branch => branch.Field).ToArray()));

    /// <summary>Queues generated declaration-name validation for the validation phase.</summary>
    internal void DeferGeneratedDeclarationNameValidation(IdlInput input, int offset, string name, IReadOnlyCollection<string> generatedNames) =>
        DeferValidation(() => IdlSemanticValidator.ValidateGeneratedDeclarationName(input, offset, name, generatedNames));

    /// <summary>Queues generated member-name validation for the validation phase.</summary>
    internal void DeferGeneratedNameCollisionValidation(IdlInput input, int offset, string name, IReadOnlyList<IdlMember> fields, string? baseType) =>
        DeferValidation(() => IdlSemanticValidator.ValidateGeneratedNameCollisions(input, offset, name, fields, baseType));

    /// <summary>Queues union branch-name validation for the validation phase.</summary>
    internal void DeferUnionGeneratedNameCollisionValidation(IdlInput input, int offset, string name, IReadOnlyList<IdlUnionBranch> branches) =>
        DeferValidation(() => IdlSemanticValidator.ValidateUnionGeneratedNameCollisions(input, offset, name, branches));

    /// <summary>Queues union default-discriminator validation for the validation phase.</summary>
    private void DeferUnionDefaultDiscriminatorValidation(IdlInput input, int offset, string? discriminatorType, IReadOnlyList<IdlUnionBranch> branches) =>
        DeferValidation(() => IdlSemanticValidator.ValidateUnionDefaultDiscriminator(input, offset, discriminatorType, branches));

    /// <summary>Queues duplicate union-discriminator-label validation for the validation phase.</summary>
    private void DeferUnionDiscriminatorLabelValidation(IdlInput input, IReadOnlyList<IdlUnionBranch> branches) =>
        DeferValidation(() => IdlSemanticValidator.ValidateUnionDiscriminatorLabels(input, branches));

    /// <summary>Records an IDL type reference for resolution during binding.</summary>
    internal IdlType ReferenceType(string idlType, string? currentNamespace, IdlInput input, int offset, string errorPrefix) =>
        new IdlType.Reference(new IdlTypeReference(idlType, currentNamespace, input, MapOffset(offset)), errorPrefix);

    /// <summary>Checks whether a field refers by value to a still-incomplete forward-declared struct.</summary>
    internal bool IsIncompleteForwardValueType(IdlType type)
    {
        while (type is IdlType.Array array)
        {
            type = array.Element;
        }

        return type is IdlType.Reference reference
            && Symbols.TryResolveTypeName(reference.TypeReference.Text, reference.TypeReference.CurrentNamespace, out var qualifiedName)
            && Validator.IsIncompleteForwardStruct(qualifiedName);
    }

    /// <summary>Parses an extensibility annotation into its semantic kind.</summary>
    internal static IdlExtensibilityKind ParseExtensibility(string annotation)
    {
        var normalized = string.Concat(annotation.Where(character => !char.IsWhiteSpace(character)));
        if (normalized.Equals("FINAL", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("@final", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("@extensibility(FINAL)", StringComparison.OrdinalIgnoreCase))
        {
            return IdlExtensibilityKind.Final;
        }

        if (normalized.Equals("MUTABLE", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("@mutable", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("@extensibility(MUTABLE)", StringComparison.OrdinalIgnoreCase))
        {
            return IdlExtensibilityKind.Mutable;
        }

        return IdlExtensibilityKind.Extensible;
    }
}
