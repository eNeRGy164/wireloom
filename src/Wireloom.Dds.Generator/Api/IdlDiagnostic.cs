namespace Wireloom;

/// <summary>Represents a non-fatal diagnostic produced while compiling IDL.</summary>
internal sealed class IdlDiagnostic(IdlInput input, int offset, string message)
{
    public IdlInput Input { get; } = input;

    public int Offset { get; } = offset;

    public string Message { get; } = message;
}
