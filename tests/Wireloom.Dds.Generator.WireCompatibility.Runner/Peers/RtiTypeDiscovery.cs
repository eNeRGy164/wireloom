using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Finds the RTI managed topic root in captured TypeSupport sources without loading assemblies.</summary>
internal static class RtiTypeDiscovery
{
    /// <summary>Prefers the Wireloom root name, then case-specific/default roots, then ordinal qualified names.</summary>
    /// <exception cref="InvalidOperationException">No matching RTI TypeSupport is present.</exception>
    internal static string Discover(string caseId, string wireloomType, string sourceDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caseId);
        ArgumentException.ThrowIfNullOrWhiteSpace(wireloomType);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceDirectory);

        var types = Directory.EnumerateFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories)
            .SelectMany(path => Regex.Matches(File.ReadAllText(path), @"class\s+\w+Support\s*:\s*Rti\.Dds\.Topics\.TypeSupport<\s*global::([\w.]+)\s*>")
                .Select(match => match.Groups[1].Value)).ToList();
        if (types.Count == 0)
        {
            throw new InvalidOperationException($"No RTI C# TypeSupport was generated for {caseId}.");
        }

        var preferred = new Dictionary<string, int>(StringComparer.Ordinal) { ["Sample"] = 0, ["Composed"] = 1 };
        var selected = caseId switch
        {
            "03-enums-aliases" => "Record",
            "09-optional-recursive-array" => "Node",
            "09-optional-aggregate-member" => "Holder",
            "07-union-aliases" or "06-valuetypes" => "Holder",
            "04-unbounded-string-aliases" => "StringAliasesSample",
            "05-unbounded-sequence-aliases" => "SequenceAliasesSample",
            "04-string-alias-chains" => "StringAliasChainsSample",
            "07-union-string-sequence-aliases" => "StringSequenceUnionsSample",
            "07-union-char-numeric" or "07-union-char-numeric-255" => "Choice",
            "07-union-string-sequence-bounded-direct" => "DirectBoundedStringSequences",
            "08-key-inherited" => "Derived",
            "08-key-nested" => "Outer",
            _ => null,
        };

        if (selected is not null)
        {
            preferred[selected] = -1;
        }

        var preferredName = wireloomType.Split('.').Last();
        return types
            .OrderBy(type => type.Split('.').Last() == preferredName ? 0 : 1)
            .ThenBy(type => preferred.GetValueOrDefault(type.Split('.').Last(), 2))
            .ThenBy(type => type, StringComparer.Ordinal)
            .First();
    }
}
