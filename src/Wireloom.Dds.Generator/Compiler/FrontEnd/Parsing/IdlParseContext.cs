using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.FrontEnd.Preprocessing;

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

    internal IdlSemanticValidator Validator { get; }

    internal List<IdlDeclaration> Declarations { get; } = [];

    private Dictionary<string, (IdlInput Input, int Offset)> TypedefLocations { get; } = new(StringComparer.Ordinal);

    internal Dictionary<string, IdlClassDeclaration> Classes { get; } = new(StringComparer.Ordinal);

    internal CancellationToken CancellationToken { get; }

    internal ICollection<IdlDiagnostic>? Diagnostics { get; }

    internal IdlTypeParser TypeParser { get; }

    private IReadOnlyList<SourceOriginSpan>? SourceOrigins { get; set; }

    internal void SetSourceOrigins(IReadOnlyList<SourceOriginSpan>? sourceOrigins) =>
        SourceOrigins = sourceOrigins;

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

    internal string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    internal void AddClass(string qualifiedName, IdlClassDeclaration declaration) =>
        Classes.Add(qualifiedName, declaration);

    internal void AddTypedef(string qualifiedName, IdlTypedef declaration, IdlInput input, int offset)
    {
        Symbols.AddTypedef(qualifiedName, declaration);
        TypedefLocations.Add(qualifiedName, (input, MapOffset(offset)));
    }

    internal void ValidateTypedefs()
    {
        foreach (var typedef in TypedefLocations)
        {
            Validator.ValidateTypedef(typedef.Value.Input, typedef.Value.Offset, typedef.Key);
        }
    }

    internal void EnsureNewName(IdlInput input, int offset, string name) =>
        Validator.EnsureNewName(input, MapOffset(offset), name);

    internal void EnsureGeneratedCompanionNames(IdlInput input, int offset, string name, string? currentNamespace, bool includeUnmanaged) =>
        Validator.EnsureGeneratedCompanionNames(input, MapOffset(offset), name, currentNamespace, includeUnmanaged);

    internal IdlType? BindFieldType(string idlType, string? currentNamespace, IdlInput input, int offset)
    {
        var reference = new IdlTypeReference(idlType, currentNamespace, input, MapOffset(offset));
        return TypeBinder.Bind(reference);
    }

    internal static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };
}
