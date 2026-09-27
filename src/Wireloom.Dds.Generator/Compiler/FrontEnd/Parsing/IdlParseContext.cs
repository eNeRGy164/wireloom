using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

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

    internal string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    internal void AddClass(string qualifiedName, IdlClassDeclaration declaration) =>
        Classes.Add(qualifiedName, declaration);

    internal void EnsureNewName(IdlInput input, int offset, string name) =>
        Validator.EnsureNewName(input, offset, name);

    internal IdlType? ResolveFieldType(string idlType, string? currentNamespace, IdlInput input, int offset) =>
        TypeResolver.Resolve(idlType, currentNamespace, input, offset);

    internal static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };
}
