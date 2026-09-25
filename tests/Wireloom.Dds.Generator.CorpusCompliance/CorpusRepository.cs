using System.Text.Json;
using System.Text.Json.Serialization;
using Wireloom;

namespace Wireloom.Dds.Generator.CorpusCompliance;

public sealed record CorpusCase(string Id, string SourceKind, string Idl, List<string> Defines, string Classification, string OracleDirectory, string Tag = "")
{
    // This is an explicit managed-generator acceptance map, not an inference
    // from the RTI result or the positive/negative manifest bucket. It keeps
    // accepted RTI probes that the managed front end actually supports while
    // leaving unrelated positive gaps as expected rejections.
    private static readonly HashSet<string> ManagedAcceptedCaseIds =
    [
        "01-primitives",
        "01-full-widths",
        "02-names-constants",
        "02-scopes",
        "02-constant-expressions",
        "02-formatting",
        "02-multiple",
        "03-enums-aliases",
        "03-enum-values-prefix",
        "03-enum-values-explicit",
        "03-alias-aggregate",
        "03-alias-collections",
        "04-strings",
        "04-boundaries",
        "05-collections",
        "05-shapes",
        "05-array-of-sequences",
        "06-aggregates",
        "06-alias-composition",
        "07-unions",
        "07-union-enum",
        "07-union-scoped",
        "07-union-multilabel",
        "07-union-short",
        "07-union-aliases",
        "08-keys",
        "08-key-nested",
        "08-key-inherited",
        "08-key-boundaries",
        "08-key-union",
        "09-extensibility",
        "09-default",
        "09-id-gaps",
        "09-evolution-optional",
        "09-optional-collections",
        "11-preprocessing",
        "11-conditionals",
        "11-macros",
        "11-macro-operators",
        "11-preprocessor-advanced",
        "11-include-search",
        "11-comments",
        "12-legacy-name",
        "07-duplicate-label",
        "12-module-idl",
        "13-integration",
        "13-modules",
        "13-alias-inheritance",
        "13-compositions"
    ];

    public bool ExpectedToCompile => ManagedAcceptedCaseIds.Contains(Id);

    public bool HasOracleOutput => !string.Equals(Classification, "rejected", StringComparison.OrdinalIgnoreCase);

    public bool IsUnsafeInProcess => string.Equals(Id, "02-alias-cycle", StringComparison.Ordinal);
}

internal sealed class CorpusManifest
{
    [JsonPropertyName("positiveCases")]
    public List<ManifestEntry> PositiveCases { get; init; } = [];

    [JsonPropertyName("negativeCases")]
    public List<ManifestEntry> NegativeCases { get; init; } = [];

    [JsonPropertyName("integrationEntryPoints")]
    public List<string> IntegrationEntryPoints { get; init; } = [];

    [JsonPropertyName("includeDirectories")]
    public List<string> IncludeDirectories { get; init; } = [];
}

internal sealed class ManifestEntry
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("idl")]
    public string Idl { get; init; } = string.Empty;

    [JsonPropertyName("defines")]
    public List<string> Defines { get; init; } = [];

    [JsonPropertyName("observedOutcome")]
    public string? ObservedOutcome { get; init; }
}

internal sealed class OracleManifest
{
    [JsonPropertyName("cases")]
    public List<OracleEntry> Cases { get; init; } = [];
}

internal sealed class OracleEntry
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("classification")]
    public string Classification { get; init; } = string.Empty;
}

internal static class CorpusRepository
{
    private static readonly Lazy<CorpusRepositoryState> Current = new(Load);

    public static string RepositoryRoot => Current.Value.RepositoryRoot;

    public static string CorpusRoot => Current.Value.CorpusRoot;

    public static string IdlRoot => Path.Combine(CorpusRoot, "idl");

    public static string OracleRoot => Path.Combine(CorpusRoot, "oracles");

    public static IReadOnlyList<CorpusCase> Cases => Current.Value.Cases;

    public static IReadOnlyList<string> AllIdlFiles => Current.Value.AllIdlFiles;

    public static IReadOnlyList<ManifestEntry> ManifestRoots => Current.Value.ManifestRoots;

    public static IReadOnlyList<string> OracleCaseIds => Current.Value.OracleCaseIds;

    public static IReadOnlyList<string> OracleFiles(CorpusCase corpusCase)
    {
        var directory = corpusCase.OracleDirectory;
        if (Directory.Exists(directory))
        {
            return [.. Directory.EnumerateFiles(directory, "*.cs", SearchOption.TopDirectoryOnly).OrderBy(p => p, StringComparer.Ordinal)];
        }

        return [];
    }

    public static List<IdlInput> BuildInputs(CorpusCase corpusCase)
    {
        var rootPath = GetIdlPath(corpusCase.Idl);
        var files = AllIdlFiles
            .Select(path => new IdlInput(
                path,
                File.ReadAllText(path),
                generate: string.Equals(path, rootPath, PathComparer),
                strict: false,
                defines: string.Equals(path, rootPath, PathComparer) ? corpusCase.Defines : [],
                includeDirectories: Current.Value.IncludeDirectories))
            .ToList();

        return files;
    }

    public static string GetIdlPath(string relativePath) =>
        Path.GetFullPath(Path.Combine(CorpusRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

    private static CorpusRepositoryState Load()
    {
        var repositoryRoot = FindRepositoryRoot();
        var corpusRoot = Path.Combine(repositoryRoot, "docs", "corpus");
        var manifest = Read<CorpusManifest>(Path.Combine(corpusRoot, "manifest.json"));
        var classifications = manifest.PositiveCases
            .Select(entry => (entry, Classification: entry.ObservedOutcome ?? "accepted"))
            .Concat(manifest.NegativeCases.Select(entry => (entry, Classification: entry.ObservedOutcome ?? "rejected")))
            .ToDictionary(entry => entry.entry.Id, entry => entry.Classification, StringComparer.Ordinal);

        var cases = new List<CorpusCase>();

        foreach (var entry in manifest.PositiveCases)
        {
            cases.Add(CreateCase(entry, "features", classifications, corpusRoot));
        }

        foreach (var entry in manifest.NegativeCases)
        {
            cases.Add(CreateCase(entry, "negative", classifications, corpusRoot));
        }

        foreach (var idl in manifest.IntegrationEntryPoints)
        {
            var id = Path.GetFileNameWithoutExtension(idl);
            var group = Path.GetFileName(Path.GetDirectoryName(idl.Replace('/', Path.DirectorySeparatorChar)))
                ?? throw new InvalidDataException($"Integration entry point has no group directory: {idl}");
            cases.Add(new CorpusCase(
                id,
                "integration",
                idl,
                [],
                "accepted",
                Path.Combine(corpusRoot, "oracles", "integration", group, id)));
        }

        cases = [.. cases.Select((corpusCase, index) => corpusCase with { Tag = $"C{index + 1:D3}" })];

        var manifestRoots = manifest.PositiveCases
            .Concat(manifest.NegativeCases)
            .Concat(manifest.IntegrationEntryPoints.Select(idl => new ManifestEntry
            {
                Id = Path.GetFileNameWithoutExtension(idl),
                Idl = idl
            }))
            .ToList();

        return new CorpusRepositoryState(
            repositoryRoot,
            corpusRoot,
            cases,
            manifestRoots,
            [.. Directory.EnumerateFiles(Path.Combine(corpusRoot, "idl"), "*.idl", SearchOption.AllDirectories).OrderBy(p => p, StringComparer.Ordinal)],
            [.. cases.Select(entry => entry.Id).OrderBy(id => id, StringComparer.Ordinal)],
            manifest.IncludeDirectories
                .Select(path => Path.GetFullPath(Path.Combine(corpusRoot, path.Replace('/', Path.DirectorySeparatorChar))))
                .ToList());
    }

    private static CorpusCase CreateCase(ManifestEntry entry, string sourceKind, Dictionary<string, string> classifications, string corpusRoot)
    {
        return new CorpusCase(
            entry.Id,
            sourceKind,
            entry.Idl,
            entry.Defines,
            classifications[entry.Id],
            Path.Combine(corpusRoot, "oracles", sourceKind, entry.Id));
    }

    private static T Read<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidDataException($"Could not deserialize corpus metadata: {path}");

    private static string FindRepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Directory.Packages.props")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("Could not find the repository root from the test output directory.");
    }

    private static string FindCorpusRoot() =>
        Path.Combine(FindRepositoryRoot(), "docs", "corpus");

    private static StringComparison PathComparer =>
        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    private sealed record CorpusRepositoryState(
        string RepositoryRoot,
        string CorpusRoot,
        List<CorpusCase> Cases,
        List<ManifestEntry> ManifestRoots,
        List<string> AllIdlFiles,
        List<string> OracleCaseIds,
        List<string> IncludeDirectories);
}
