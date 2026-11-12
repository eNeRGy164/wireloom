namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Parses explicit case selections for Wireloom C++ or native-only peer runs.</summary>
internal static class WireloomCppScope
{
    /// <summary>Parses an explicit native-peer selection and verifies it belongs to the selected cases.</summary>
    [PublicAPI]
    internal static HashSet<string> ReadExplicitCases(string? filter, IReadOnlyCollection<string> selectedCases)
    {
        ArgumentNullException.ThrowIfNull(selectedCases);
        if (string.IsNullOrWhiteSpace(filter))
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        var requested = filter.Split(',', StringSplitOptions.TrimEntries);
        if (requested.Any(string.IsNullOrWhiteSpace) || requested.Distinct(StringComparer.Ordinal).Count() != requested.Length)
        {
            throw new InvalidDataException("The explicit Wireloom C++ case filter must contain unique, non-empty case IDs.");
        }

        var selected = selectedCases.ToHashSet(StringComparer.Ordinal);
        var missing = requested.Where(caseId => !selected.Contains(caseId)).ToArray();
        if (missing.Length > 0)
        {
            throw new InvalidDataException($"Wireloom C++ cases must also be selected by the case filter: {string.Join(", ", missing)}.");
        }

        return requested.ToHashSet(StringComparer.Ordinal);
    }
}
