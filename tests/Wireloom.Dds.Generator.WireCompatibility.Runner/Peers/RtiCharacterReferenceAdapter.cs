namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Applies guarded character-union reference-source workarounds.</summary>
internal static class RtiCharacterReferenceAdapter
{
    /// <summary>Applies the generated-source edits for one supported language and case.</summary>
    /// <param name="language">Generated peer language: <c>cpp</c> or <c>csharp</c>.</param>
    /// <param name="caseId">Corpus case with a known character-label workaround.</param>
    /// <param name="directory">Directory containing the generated source files.</param>
    /// <exception cref="ArgumentException">The language or case has no defined adaptation.</exception>
    /// <exception cref="InvalidDataException">The generated source does not match the expected reference shape.</exception>
    internal static void Apply(string language, string caseId, string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(language);
        ArgumentException.ThrowIfNullOrWhiteSpace(caseId);
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);

        var label = caseId switch
        {
            "07-union-char-numeric" => "127",
            "07-union-char-numeric-255" => "255",
            _ => throw new ArgumentException($"No character reference adaptation is defined for '{caseId}'.", nameof(caseId)),
        };

        if (language == "cpp")
        {
            if (label != "255")
            {
                throw new ArgumentException("Only label 255 needs a C++ TypeCode adaptation.", nameof(caseId));
            }

            AdaptCpp(directory);
        }
        else if (language == "csharp")
        {
            AdaptCSharp(caseId, label, directory);
        }
        else
        {
            throw new ArgumentException($"Unknown reference language '{language}'.", nameof(language));
        }

        Console.WriteLine($"RTI character reference adapted: {language}, {caseId}.");
    }

    private static void AdaptCpp(string directory)
    {
        var path = Path.Combine(directory, "idl.cxx");
        var source = File.ReadAllText(path);
        source = ReplaceReferenceText(source, "static_cast<int>(255)", "static_cast<int>(-1)", count: 1);

        File.WriteAllText(path, source);
    }

    private static void AdaptCSharp(string caseId, string label, string directory)
    {
        var samplePath = Path.Combine(directory, $"{caseId}.cs");
        var pluginPath = Path.Combine(directory, $"{caseId}Plugin.cs");
        var sample = File.ReadAllText(samplePath);
        var plugin = File.ReadAllText(pluginPath);

        // Validate every expected edit before writing either generated file.
        sample = ReplaceReferenceText(sample, $"Discriminator = {label};", $"Discriminator = (char){label};", count: 1);
        sample = ReplaceReferenceText(sample, $"case {label}:", $"case (char){label}:", count: 4);
        plugin = ReplaceReferenceText(plugin, $"case {label}:", $"case (char){label}:", count: 2);
        plugin = ReplaceReferenceText(plugin, "sample.fallback = fallback;", "sample.Setfallback(fallback, NativeChar.FromUtf8(_d));", count: 1);

        if (label == "255")
        {
            plugin = ReplaceReferenceText(plugin, "new int[] {(int) 255}", "new int[] { -1 }", count: 1);
        }

        File.WriteAllText(samplePath, sample);
        File.WriteAllText(pluginPath, plugin);
    }

    private static string ReplaceReferenceText(string source, string original, string replacement, int count)
    {
        var originalCount = source.Split(original).Length - 1;
        var replacementCount = source.Split(replacement).Length - 1;
        if (originalCount == 0 && replacementCount == count)
        {
            return source;
        }

        if (originalCount != count || replacementCount != 0)
        {
            throw new InvalidDataException($"RTI generated reference shape changed: expected {count} occurrences of '{original}', found {originalCount}.");
        }

        return source.Replace(original, replacement, StringComparison.Ordinal);
    }
}
