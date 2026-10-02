using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Provides the shared state and services used while parsing one IDL compilation.</summary>
internal sealed class IdlParseContext
{
    internal IdlParseContext(IdlSymbolTable symbols, CancellationToken cancellationToken, ICollection<IdlDiagnostic>? diagnostics)
    {
        Symbols = symbols;
        TypeBinder = new IdlTypeBinder(symbols);
        Validator = new IdlSemanticValidator(symbols);
        CancellationToken = cancellationToken;
        Diagnostics = diagnostics;
        TypeParser = new IdlTypeParser(this);
    }

    internal IdlSymbolTable Symbols { get; }

    private IdlTypeBinder TypeBinder { get; }

    private IdlSemanticValidator Validator { get; }

    internal List<IdlDeclaration> Declarations { get; } = [];

    private Dictionary<string, (IdlInput Input, int Offset)> TypedefLocations { get; } = new(StringComparer.Ordinal);

    private List<IdlDeferredBound> DeferredBounds { get; } = [];

    private List<Action> DeferredValidation { get; } = [];

    internal Dictionary<string, IdlClassDeclaration> Classes { get; } = new(StringComparer.Ordinal);

    internal CancellationToken CancellationToken { get; }

    internal ICollection<IdlDiagnostic>? Diagnostics { get; }

    internal IdlTypeParser TypeParser { get; }

    private IReadOnlyList<SourceOriginSpan>? SourceOrigins { get; set; }

    /// <summary>Sets source-origin mappings for the current parsed input.</summary>
    internal void SetSourceOrigins(IReadOnlyList<SourceOriginSpan>? sourceOrigins) =>
        SourceOrigins = sourceOrigins;

    /// <summary>Maps a preprocessed offset to its original source offset.</summary>
    internal int MapOffset(int offset)
    {
        if (SourceOrigins is null || SourceOrigins.Count == 0 || offset < 0)
        {
            return offset;
        }

        var low = 0;
        var high = SourceOrigins.Count - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var origin = SourceOrigins[middle];
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

        return SourceOrigins[^1].SourceStart;
    }

    /// <summary>Qualifies a declaration name against a namespace.</summary>
    internal string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    /// <summary>Adds a parsed class declaration to the context.</summary>
    internal void AddClass(string qualifiedName, IdlClassDeclaration declaration) =>
        Classes.TryAdd(qualifiedName, declaration);

    /// <summary>Adds a parsed typedef declaration and validates its name.</summary>
    internal void AddTypedef(string qualifiedName, IdlTypedef declaration, IdlInput input, int offset)
    {
        Symbols.AddTypedef(qualifiedName, declaration);
        TypedefLocations.TryAdd(qualifiedName, (input, MapOffset(offset)));
    }

    /// <summary>Resolves constants and bounds after parsing has completed.</summary>
    private void BindDeferredValues()
    {
        foreach (var declaration in Declarations.OfType<IdlConstantDeclaration>())
        {
            declaration.SetIntegerValue(EvaluateConstant(declaration));
        }

        foreach (var bound in DeferredBounds)
        {
            bound.Resolve(Validator);
        }
    }

    /// <summary>Validates all deferred semantic rules against the complete symbol graph.</summary>
    internal void Validate()
    {
        foreach (var typedef in TypedefLocations)
        {
            DeferredValidation.Add(() => Validator.ValidateTypedef(typedef.Value.Input, typedef.Value.Offset, typedef.Key));
        }

        foreach (var validation in DeferredValidation)
        {
            validation();
        }
    }

    /// <summary>Binds all deferred type references after parsing completes.</summary>
    /// <summary>Binds all parsed declarations against the complete symbol graph.</summary>
    internal void BindDeclarations()
    {
        BindDeferredValues();

        foreach (var declaration in Declarations)
        {
            switch (declaration)
            {
                case IdlClassDeclaration @class:
                    foreach (var field in @class.Fields)
                    {
                        var bound = TypeBinder.Bind(field.Type);
                        if (field.Metadata.IsOptional && !IsOptionalScalar(bound))
                        {
                            throw new IdlException(field.SourceInput!, field.SourceOffset, "Optional aggregate members are not supported yet.");
                        }

                        field.Bind(bound);
                    }

                    break;
                case IdlUnionDeclaration union:
                    var discriminatorQualified = IdlNaming.ResolveTypeName(
                        union.Declaration.DiscriminatorIdlType,
                        union.Declaration.Namespace);
                    var discriminatorIsEnum = Symbols.TryGetEnum(discriminatorQualified, out var discriminatorEnum);
                    int? discriminatorDefaultValue = null;
                    if (discriminatorIsEnum)
                    {
                        discriminatorDefaultValue = discriminatorEnum.DefaultMember.Value;
                    }

                    union.Declaration.BindDiscriminator(discriminatorIsEnum, discriminatorDefaultValue);

                    foreach (var branch in union.Declaration.Branches)
                    {
                        branch.BindLabels(
                            union.Declaration.DiscriminatorIdlType,
                            union.Declaration.Namespace,
                            discriminatorEnum);
                        branch.Field.Bind(TypeBinder.Bind(branch.Field.Type));
                    }

                    break;
            }
        }

        TypeParser.ResolveDeferredMemberMetadata();
    }

    /// <summary>Records a declaration name and defers its semantic validation.</summary>
    internal void EnsureNewName(IdlInput input, int offset, string name)
    {
        Validator.RegisterName(name);
        var mappedOffset = MapOffset(offset);
        DeferValidation(() => Validator.ValidateNewName(input, mappedOffset, name));
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
    internal void DeferMemberIdValidation(IdlInput input, int offset, IReadOnlyList<IdlMember> fields) =>
        DeferValidation(() => IdlSemanticValidator.ValidateMemberIds(input, offset, fields));

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
    internal void DeferUnionDefaultDiscriminatorValidation(IdlInput input, int offset, string discriminatorType, IReadOnlyList<IdlUnionBranch> branches) =>
        DeferValidation(() => IdlSemanticValidator.ValidateUnionDefaultDiscriminator(input, offset, discriminatorType, branches));

    /// <summary>Creates a bound type or a deferred reference with source context.</summary>
    /// <summary>Records an IDL type reference for resolution during binding.</summary>
    internal IdlType ReferenceType(string idlType, string? currentNamespace, IdlInput input, int offset, string errorPrefix) =>
        new IdlType.Reference(new IdlTypeReference(idlType, currentNamespace, input, MapOffset(offset)), errorPrefix);

    private BigInteger? EvaluateConstant(IdlConstantDeclaration declaration)
    {
        if (declaration.Type is "string" or "wstring" or "float" or "double" or "long double" or "boolean" or "char" or "wchar")
        {
            return null;
        }

        try
        {
            var value = IdlConstantExpressionEvaluator.Evaluate(declaration.Expression, Symbols, declaration.Namespace);
            IdlConstantParser.ValidateConstantRange(declaration.SourceInput, declaration.SourceOffset, declaration.Type, value);
            return value;
        }
        catch (FormatException exception)
        {
            throw new IdlException(declaration.SourceInput, declaration.SourceOffset, $"Invalid {declaration.Type} constant expression: {exception.Message}");
        }
    }

    private static bool IsOptionalScalar(IdlType type) => type switch
    {
        IdlType.Primitive or IdlType.StringType or IdlType.Enum or IdlType.Sequence or IdlType.Array => true,
        IdlType.Alias alias => IsOptionalScalar(alias.Target),
        _ => false
    };

    /// <summary>Parses an extensibility annotation into its semantic kind.</summary>
    internal static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };
}
