namespace Wireloom.Dds.Idl;

/// <summary>Immutable host inputs and services for one IDL compilation.</summary>
[PublicAPI]
public sealed class IdlCompilationRequest
{
    /// <summary>Creates a compilation request and snapshots its input collection.</summary>
    /// <param name="inputs">All generation roots and supplied include files.</param>
    /// <param name="computeMd5">The host-provided synchronous MD5 digest function.</param>
    /// <param name="cancellationToken">The token used to cancel the compilation.</param>
    public IdlCompilationRequest(
        IReadOnlyList<IdlInput> inputs,
        Func<byte[], byte[]> computeMd5,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(computeMd5);
        Inputs = Array.AsReadOnly(inputs.ToArray());
        ComputeMd5 = computeMd5;
        CancellationToken = cancellationToken;
    }

    /// <summary>Gets the immutable IDL input snapshot.</summary>
    public IReadOnlyList<IdlInput> Inputs { get; }

    /// <summary>Gets the required per-compilation host MD5 provider.</summary>
    public Func<byte[], byte[]> ComputeMd5 { get; }

    /// <summary>Gets the compilation cancellation token.</summary>
    public CancellationToken CancellationToken { get; }
}
