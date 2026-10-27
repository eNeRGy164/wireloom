namespace Wireloom.Dds.Generator.WireCompatibility.Shared;

/// <summary>One directed writer-to-reader peer pairing.</summary>
/// <param name="Writer">Writer peer name.</param>
/// <param name="Reader">Reader peer name.</param>
public sealed record DirectedPeerPair(string Writer, string Reader)
{
    /// <summary>Whether either endpoint uses the requested peer.</summary>
    /// <param name="peer">Peer label to search for.</param>
    /// <returns><see langword="true"/> when the writer or reader matches the peer.</returns>
    public bool Involves(string peer) => Writer == peer || Reader == peer;
}
