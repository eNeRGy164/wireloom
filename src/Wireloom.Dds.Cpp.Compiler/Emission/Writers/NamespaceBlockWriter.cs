namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Keeps adjacent declarations inside shared C++ namespace scopes.</summary>
internal sealed class NamespaceBlockWriter(SourceWriter source)
{
    private IReadOnlyList<string> openNamespaces = Array.Empty<string>();

    /// <summary>Switches to a namespace, retaining any shared outer scopes.</summary>
    internal void SwitchTo(IReadOnlyList<string> requestedNamespaces)
    {
        var commonLength = 0;
        while (commonLength < openNamespaces.Count
            && commonLength < requestedNamespaces.Count
            && openNamespaces[commonLength] == requestedNamespaces[commonLength])
        {
            commonLength++;
        }

        source.CloseNamespaces(openNamespaces.Skip(commonLength).ToArray());
        source.OpenNamespaces(requestedNamespaces.Skip(commonLength).ToArray());
        openNamespaces = requestedNamespaces.ToArray();
    }

    /// <summary>Closes all namespace scopes currently open.</summary>
    internal void Close()
    {
        source.CloseNamespaces(openNamespaces);
        openNamespaces = Array.Empty<string>();
    }
}
