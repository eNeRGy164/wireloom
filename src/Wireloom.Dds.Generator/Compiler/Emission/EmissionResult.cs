namespace Wireloom.Compiler.Emission;

/// <summary>Collects generated documents without coupling emitters to orchestration state.</summary>
internal sealed class EmissionResult
{
    private readonly Dictionary<string, GeneratedIdlSource> sources = new(StringComparer.Ordinal);
    private readonly List<GeneratedIdlSource> orderedSources = [];

    public IReadOnlyList<GeneratedIdlSource> Sources => orderedSources;

    /// <summary>Adds one generated document after validating its hint name.</summary>
    private void Add(GeneratedIdlSource generated)
    {
        if (sources.ContainsKey(generated.HintName))
        {
            throw new InvalidOperationException($"Generated hint name '{generated.HintName}' was produced more than once.");
        }

        sources.Add(generated.HintName, generated);
        orderedSources.Add(generated);
    }

    /// <summary>Adds generated documents in their emission order.</summary>
    public void AddRange(IEnumerable<GeneratedIdlSource> generated)
    {
        foreach (var document in generated)
        {
            Add(document);
        }
    }
}
