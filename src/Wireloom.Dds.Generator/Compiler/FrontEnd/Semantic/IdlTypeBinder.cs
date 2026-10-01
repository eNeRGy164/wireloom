using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Binds parsed type references to the target-independent semantic type model.</summary>
internal sealed class IdlTypeBinder(IdlSymbolTable symbols)
{
    private readonly IdlTypeResolver resolver = new(symbols);

    public IdlType? Bind(IdlTypeReference reference) =>
        resolver.Resolve(reference.Text, reference.CurrentNamespace, reference.Input, reference.Offset);
}
