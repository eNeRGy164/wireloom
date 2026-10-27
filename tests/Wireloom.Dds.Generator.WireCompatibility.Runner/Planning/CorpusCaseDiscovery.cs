using System.Text.Json;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Discovers eligible corpus cases directly from the manifest and exact-profile exclusions.</summary>
internal static class CorpusCaseDiscovery
{
    /// <summary>Reads case selection in manifest order, retaining RTI-only controls in exclusion order.</summary>
    internal static DiscoveredCases Read(string manifestPath, string exclusionsPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(manifestPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(exclusionsPath);

        using var manifest = JsonDocument.Parse(File.ReadAllText(manifestPath));
        using var exclusions = JsonDocument.Parse(File.ReadAllText(exclusionsPath));

        var rtiOnlyCases = ReadIds(exclusions.RootElement, "notImplementedCases", requiredCollection: true);
        var excludedWireloomCases = rtiOnlyCases.ToHashSet(StringComparer.Ordinal);
        excludedWireloomCases.UnionWith(ReadIds(exclusions.RootElement, "notApplicableCases", requiredCollection: false));

        var wireloomCases = ReadIds(manifest.RootElement, "positiveCases", requiredCollection: true)
            .Where(caseId => !excludedWireloomCases.Contains(caseId))
            .ToArray();

        return new DiscoveredCases(wireloomCases, rtiOnlyCases);
    }

    private static string[] ReadIds(JsonElement document, string collectionName, bool requiredCollection)
    {
        if (!document.TryGetProperty(collectionName, out var collection) || collection.ValueKind == JsonValueKind.Null)
        {
            if (requiredCollection)
            {
                throw new InvalidDataException($"Case discovery requires the '{collectionName}' array.");
            }

            return [];
        }

        if (collection.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException($"Case discovery expects '{collectionName}' to be an array.");
        }

        return [.. collection.EnumerateArray().Select(item => ReadId(item, collectionName))];
    }

    private static string ReadId(JsonElement item, string collectionName)
    {
        if (item.ValueKind != JsonValueKind.Object || !item.TryGetProperty("id", out var id) ||
            id.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(id.GetString()))
        {
            throw new InvalidDataException($"Each '{collectionName}' entry must have a nonempty string 'id'.");
        }

        return id.GetString()!;
    }
}

/// <summary>Eligible managed cases and RTI-only controls before filtering and shard selection.</summary>
/// <param name="WireloomCases">Positive cases without implementation or applicability exclusions.</param>
/// <param name="RtiOnlyCases">Implementation gaps still eligible for RTI control-peer execution.</param>
[PublicAPI]
internal sealed record DiscoveredCases(IReadOnlyList<string> WireloomCases, IReadOnlyList<string> RtiOnlyCases)
{
    /// <summary>Applies an optional ordered case filter and rejects empty, duplicate, or ineligible selections.</summary>
    internal IReadOnlyList<string> Select(string? caseFilter)
    {
        var available = WireloomCases.Concat(RtiOnlyCases).ToArray();
        var selected = string.IsNullOrEmpty(caseFilter)
            ? available
            : caseFilter.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (selected.Length == 0 || selected.Distinct(StringComparer.Ordinal).Count() != selected.Length ||
            selected.Any(caseId => !available.Contains(caseId, StringComparer.Ordinal)))
        {
            throw new InvalidDataException("The selected case filter is empty, duplicated, unknown, or inapplicable for this RTI profile.");
        }

        return selected;
    }
}
