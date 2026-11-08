using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves aggregate inheritance and records inherited member sequences during binding.</summary>
internal sealed class IdlInheritanceBinder(IdlSymbolTable symbols)
{
    private readonly Dictionary<IdlClassDeclaration, IdlClassDeclaration> baseDeclarations = [];

    /// <summary>Binds all parsed aggregate inheritance relationships.</summary>
    internal void Bind(IReadOnlyList<IdlClassDeclaration> declarations)
    {
        baseDeclarations.Clear();

        foreach (var declaration in declarations)
        {
            BindBaseType(declaration);
        }

        foreach (var declaration in declarations)
        {
            declaration.SetInheritedFields(ResolveInheritedFields(declaration, []));
        }
    }

    private void BindBaseType(IdlClassDeclaration declaration)
    {
        if (declaration.BaseType is null)
        {
            return;
        }

        if (!symbols.TryResolveTypeName(declaration.BaseType, declaration.Namespace, out var qualifiedBaseType)
            || !symbols.TryGetClass(qualifiedBaseType, out var baseDeclaration))
        {
            throw new IdlException(
                declaration.SourceInput,
                declaration.BaseTypeOffset,
                $"Unknown struct base type: {declaration.BaseType}");
        }

        declaration.SetBaseType(qualifiedBaseType);
        baseDeclarations.Add(declaration, baseDeclaration);
    }

    private IReadOnlyList<IdlMember> ResolveInheritedFields(
        IdlClassDeclaration declaration,
        HashSet<IdlClassDeclaration> activeDeclarations)
    {
        if (declaration.BaseType is null)
        {
            return [];
        }

        if (!activeDeclarations.Add(declaration))
        {
            throw new IdlException(
                declaration.SourceInput,
                declaration.BaseTypeOffset,
                $"Cyclic struct inheritance detected: {declaration.Name}");
        }

        try
        {
            var baseDeclaration = baseDeclarations[declaration];
            return [.. ResolveInheritedFields(baseDeclaration, activeDeclarations), .. baseDeclaration.Fields];
        }
        finally
        {
            activeDeclarations.Remove(declaration);
        }
    }
}
