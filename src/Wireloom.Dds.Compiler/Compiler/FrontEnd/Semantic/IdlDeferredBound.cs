namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Retains a bound expression until the complete constant graph is available.</summary>
internal sealed class IdlDeferredBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName)
{
    private readonly List<Action<int>> consumers = [];

    /// <summary>Gets the placeholder value used until binding resolves the expression.</summary>
    public int Value { get; private set; } = 1;

    /// <summary>Adds a consumer that receives the resolved bound during binding.</summary>
    public void AddConsumer(Action<int> consumer) => consumers.Add(consumer);

    /// <summary>Resolves the retained expression through the semantic validator.</summary>
    public void Resolve(IdlSemanticValidator validator)
    {
        Value = validator.ResolveBound(input, offset, text, currentNamespace, diagnosticName);

        foreach (var consumer in consumers)
        {
            consumer(Value);
        }
    }
}
