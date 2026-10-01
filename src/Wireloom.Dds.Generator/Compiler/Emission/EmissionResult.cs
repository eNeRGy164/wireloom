namespace Wireloom.Compiler.Emission;

/// <summary>Collects generated documents without coupling emitters to orchestration state.</summary>
internal sealed class EmissionResult
{
    private readonly Dictionary<string, GeneratedIdlSource> sources = new(StringComparer.Ordinal);
    private readonly List<GeneratedIdlSource> orderedSources = [];

    public IReadOnlyList<GeneratedIdlSource> Sources => orderedSources;

    public void Add(GeneratedName name, string source)
    {
        var generated = new GeneratedIdlSource(name.HintName, source);
        if (sources.ContainsKey(generated.HintName))
        {
            throw new InvalidOperationException($"Generated hint name '{generated.HintName}' was produced more than once.");
        }

        sources.Add(generated.HintName, generated);
        orderedSources.Add(generated);
    }
}
