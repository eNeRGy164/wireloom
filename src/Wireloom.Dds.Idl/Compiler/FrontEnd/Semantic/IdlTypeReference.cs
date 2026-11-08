namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Raw type syntax retained by parsing until semantic binding.</summary>
internal sealed class IdlTypeReference(string text, string? currentNamespace, IdlInput input, int offset, bool wasEscaped = false)
{
    public string Text { get; } = text;

    public string? CurrentNamespace { get; } = currentNamespace;

    public IdlInput Input { get; } = input;

    public int Offset { get; } = offset;

    /// <summary>Gets whether the normalized spelling came from an escaped IDL identifier.</summary>
    public bool WasEscaped { get; } = wasEscaped;
}
