using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Reads positive-case build metadata from one lazily loaded manifest snapshot per run.</summary>
/// <param name="manifestPath">Corpus manifest selected by the workspace.</param>
internal sealed class CorpusCaseCatalog(string manifestPath)
{
    private readonly Lazy<JsonNode> manifest = new(() => JsonNode.Parse(File.ReadAllText(manifestPath))!);

    /// <summary>Looks up an ordinal case identity and decodes its typed build fields.</summary>
    /// <exception cref="InvalidOperationException">The manifest has no matching positive case.</exception>
    internal CaseMetadata Get(string caseId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caseId);

        var definition = manifest.Value["positiveCases"]!.AsArray()
            .FirstOrDefault(item => (string)item!["id"]! == caseId)
            ?? throw new InvalidOperationException($"No positive corpus case found for {caseId}.");

        return CaseMetadata.Read(definition);
    }
}
