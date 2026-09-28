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
        TypeResolver = new IdlTypeResolver(symbols);
        Validator = new IdlSemanticValidator(symbols);
        CancellationToken = cancellationToken;
        Diagnostics = diagnostics;
        TypeParser = new IdlTypeParser(this);
    }

    internal IdlSymbolTable Symbols { get; }

    private IdlTypeResolver TypeResolver { get; }

    internal IdlSemanticValidator Validator { get; }

    internal List<IdlDeclaration> Declarations { get; } = [];

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

    internal void EnsureNewName(IdlInput input, int offset, string name) =>
        Validator.EnsureNewName(input, MapOffset(offset), name);

    internal IdlType? ResolveFieldType(string idlType, string? currentNamespace, IdlInput input, int offset) =>
        TypeResolver.Resolve(idlType, currentNamespace, input, MapOffset(offset));

    internal static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };
}
