namespace Wireloom;

/// <summary>
/// Describes a deterministic IDL compilation failure and its source location.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="IdlException"/> class.
/// </remarks>
/// <param name="input">The input in which the failure occurred.</param>
/// <param name="offset">The zero-based character offset of the failure.</param>
/// <param name="message">The failure description.</param>
public sealed class IdlException(IdlInput input, int offset, string message) : Exception(message)
{
    /// <summary>
    /// Gets the input in which the failure occurred.
    /// </summary>
    public IdlInput Input { get; } = input;

    /// <summary>
    /// Gets the zero-based character offset of the failure.
    /// </summary>
    public int Offset { get; } = offset;
}
