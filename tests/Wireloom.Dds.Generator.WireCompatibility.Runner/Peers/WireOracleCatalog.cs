using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Selects checked-in oracle sources from one lazily loaded profile manifest per run.</summary>
/// <param name="manifestPath">Profile-specific wire-oracles.json source of truth.</param>
internal sealed class WireOracleCatalog(string manifestPath)
{
    private readonly Lazy<JsonNode> manifest = new(() => JsonNode.Parse(File.ReadAllText(manifestPath))!);
    private readonly string oracleRoot = Path.GetDirectoryName(Path.GetFullPath(manifestPath))!;

    /// <summary>Returns a configured or existing conventional relative source path, or null when unavailable.</summary>
    /// <remarks>Explicit non-capture modes suppress fallback. An explicit empty capture path remains empty.</remarks>
    internal string? FindSource(string caseId, string peer)
    {
        ArgumentNullException.ThrowIfNull(caseId);
        ArgumentNullException.ThrowIfNull(peer);

        var entry = manifest.Value["cases"]?[caseId]?[peer];
        if (entry is not null)
        {
            if ((string?)entry["mode"] is "captured" or "adjusted")
            {
                return (string?)entry["sourceDirectory"] ?? "";
            }

            return null;
        }

        var language = peer switch
        {
            "rtiCSharp" => "csharp",
            "rtiCpp" => "cpp",
            _ => throw new ArgumentException($"Unknown wire oracle peer: {peer}"),
        };
        var conventionalSource = Path.Combine(language, "features", caseId);
        if (Directory.Exists(Path.Combine(oracleRoot, conventionalSource)))
        {
            return conventionalSource.Replace('\\', '/');
        }

        return null;
    }
}
