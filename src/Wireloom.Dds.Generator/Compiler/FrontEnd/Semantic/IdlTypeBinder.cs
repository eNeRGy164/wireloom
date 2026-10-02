using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Binds parsed type references to the target-independent semantic type model.</summary>
internal sealed class IdlTypeBinder(IdlSymbolTable symbols)
{
    private readonly IdlTypeResolver resolver = new(symbols);

    /// <summary>Binds one named type reference against the symbol table.</summary>
    /// <summary>Binds a type reference to its semantic declaration.</summary>
    private IdlType? Bind(IdlTypeReference reference) =>
        resolver.Resolve(reference.Text, reference.CurrentNamespace, reference.Input, reference.Offset);

    /// <summary>Recursively binds references within a resolved type graph.</summary>
    /// <summary>Binds nested references within a semantic type.</summary>
    public IdlType Bind(IdlType type) => type switch
    {
        IdlType.Reference reference => Bind(reference),
        IdlType.Sequence sequence => new IdlType.Sequence(Bind(sequence.Element), sequence.Bound, sequence.Dimensions),
        IdlType.Array array => new IdlType.Array(Bind(array.Element), array.Dimensions),
        _ => type
    };

    private IdlType Bind(IdlType.Reference reference)
    {
        var bound = Bind(reference.TypeReference);
        if (bound is null)
        {
            throw new IdlException(reference.TypeReference.Input, reference.TypeReference.Offset, $"{reference.ErrorPrefix}: {reference.TypeReference.Text}");
        }

        return bound;
    }
}
