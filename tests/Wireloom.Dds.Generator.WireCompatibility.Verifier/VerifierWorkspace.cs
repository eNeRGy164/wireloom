namespace Wireloom.Dds.Generator.WireCompatibility.Verifier;

/// <summary>Owns the unique temporary directory used by one verifier run.</summary>
internal sealed class VerifierWorkspace : IDisposable
{
    private readonly string temporaryRoot;

    internal VerifierWorkspace(string temporaryRoot)
    {
        this.temporaryRoot = Path.GetFullPath(temporaryRoot);

        Root = Path.Combine(this.temporaryRoot, "wireloom-shards-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Root);
    }

    internal string Root { get; }

    public void Dispose()
    {
        var safeRoot = Path.GetFullPath(Root);
        if (!safeRoot.StartsWith(temporaryRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Verification output is outside the temporary directory.");
        }

        Directory.Delete(safeRoot, recursive: true);
    }
}
