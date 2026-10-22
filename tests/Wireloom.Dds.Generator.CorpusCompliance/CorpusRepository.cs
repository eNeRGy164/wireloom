using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wireloom.Dds.Generator.CorpusCompliance;

public sealed record CorpusCase(string Id, string SourceKind, string Idl, List<string> Defines, string Classification, string OracleDirectory, IdlSequenceArrayMapping SequenceArrayMapping = IdlSequenceArrayMapping.RtiCSharp, string Tag = "", bool OracleSourcesRetained = true, string? OracleCompileOutcome = null, bool Strict = false)
{
    // This is an explicit managed-generator acceptance map, not an inference
    // from the RTI result or the positive/negative manifest bucket. It keeps
    // accepted RTI probes that the managed front end actually supports while
    // leaving unrelated positive gaps as expected rejections.
    private static readonly HashSet<string> ManagedAcceptedCaseIds =
    [
        "12-interface-keyword-default",
        "12-interface-case-default",
        "12-interface-body-control",
        "12-keyword-default",
        "12-case-collision-default",
        "12-identifier-control",
        "01-primitives",
        "01-boundaries",
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
        "04-string-alias-chains",
        "04-unbounded-string-aliases",
        "05-unbounded-sequence-aliases",
        "05-collections",
        "05-shapes",
        "05-array-of-sequences",
        "05-array-of-sequences-idl",
        "06-aggregates",
        "06-valuetypes",
        "06-alias-composition",
        "07-unions",
        "07-union-enum",
        "07-union-scoped",
        "07-union-multilabel",
        "07-union-short",
        "07-union-short-boundaries",
        "07-union-char",
        "07-union-char-numeric",
        "07-union-char-numeric-255",
        "07-union-wchar-label",
        "07-union-boolean",
        "07-union-aliases",
        "07-union-octet",
        "07-union-ushort",
        "07-union-ulong",
        "07-union-int8",
        "07-union-uint8",
        "07-union-int16",
        "07-union-uint16",
        "07-union-int32",
        "07-union-uint32",
        "07-union-char-boundaries",
        "07-union-long",
        "07-union-string-sequence-aliases",
        "07-union-string-sequence-bounded-direct",
        "12-union-wchar-rti-limit",
        "08-keys",
        "08-key-nested",
        "08-key-inherited",
        "08-key-boundaries",
        "08-key-union",
        "09-data-representation",
        "09-allowed-data-representation",
        "09-extensibility",
        "09-default",
        "09-id-gaps",
        "09-evolution-optional",
        "09-optional-collections",
        "09-optional-aggregate-member",
        "09-optional-recursive-array",
        "09-optional-string-sequences",
        "09-autoid-hash",
        "09-must-understand",
        "09-defaults-ranges",
        "10-annotations",
        "10-annotations-extended",
        "10-annotations-rti",
        "10-resolve-name-wstring-aliases",
        "10-default-literal-enum-array",
        "10-flat-data-binding",
        "11-preprocessing",
        "11-conditionals",
        "11-macros",
        "11-macro-operators",
        "11-preprocessor-advanced",
        "11-include-search",
        "11-comments",
        "12-legacy-name",
        "12-module-idl",
        "13-integration",
        "13-modules",
        "13-alias-inheritance",
        "12-unknown-annotation",
        "12-interface",
        "12-macro-recursion",
        "12-macro-arity-windows",
        "12-macro-arity-linux",
        "13-compositions",
        "14-extensibility-full",
        "14-autoid-scope",
        "14-annotation-arguments",
        "14-member-declarators",
        "14-union-label-expressions",
        "14-union-scoped-enum",
        "14-union-array-branch",
        "14-string-expression-bound",
        "14-typed-defaults",
        "14-enum-constant"
    ];

    public bool ExpectedToCompile => ExpectedToCompileForProfile(CorpusRepository.RtiProfile);

    public bool ExpectedToCompileForProfile(string? rtiProfile) =>
        ManagedAcceptedCaseIds.Contains(Id) &&
        !(Id == "09-must-understand" && string.Equals(rtiProfile, "7.3.1", StringComparison.Ordinal));

    public bool HasOracleOutput =>
        OracleSourcesRetained &&
        !string.Equals(Classification, "rejected", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(Classification, "pending", StringComparison.OrdinalIgnoreCase);

    public bool OracleCompilationFailed =>
        string.Equals(OracleCompileOutcome, "failed", StringComparison.OrdinalIgnoreCase);

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

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;

    [JsonPropertyName("oracleSourcesRetained")]
    public bool OracleSourcesRetained { get; init; } = true;

    [JsonPropertyName("oracleDirectory")]
    public string? OracleDirectory { get; init; }

    [JsonPropertyName("strict")]
    public bool Strict { get; init; }

    [JsonPropertyName("sequenceArrayMapping")]
    public string? SequenceArrayMapping { get; init; }
}

internal sealed class OracleManifest
{
    [JsonPropertyName("cases")]
    public List<OracleEntry> Cases { get; init; } = [];

    [JsonPropertyName("characterizationProbes")]
    public List<OracleEntry> CharacterizationProbes { get; init; } = [];
}

internal sealed record OracleEntry
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("classification")]
    public string Classification { get; init; } = string.Empty;

    /// <summary>Gets the generation outcome captured for the selected LTS version.</summary>
    [JsonPropertyName("outcome")]
    public string Outcome { get; init; } = string.Empty;

    /// <summary>Gets the captured oracle directory relative to the LTS profile.</summary>
    [JsonPropertyName("sourceDirectory")]
    public string SourceDirectory { get; init; } = string.Empty;

    [JsonPropertyName("oracleSourcesRetained")]
    public bool OracleSourcesRetained { get; init; } = true;

    [JsonPropertyName("compileOutcome")]
    public string? CompileOutcome { get; init; }
}

internal static class CorpusRepository
{
    private static readonly Lazy<CorpusRepositoryState> Current = new(Load);

    public static string RepositoryRoot => Current.Value.RepositoryRoot;

    public static string CorpusRoot => Current.Value.CorpusRoot;

    public static string IdlRoot => Path.Combine(CorpusRoot, "idl");

    /// <summary>Gets the exact LTS oracle selected for the current compliance run.</summary>
    public static string RtiProfile => Environment.GetEnvironmentVariable("CORPUS_RTI_PROFILE") switch
    {
        null or "7.7.0" => "7.7.0",
        "7.3.1" => "7.3.1",
        var profile => throw new InvalidOperationException($"Unsupported corpus RTI profile: {profile}")
    };

    public static string OracleRoot => Path.Combine(CorpusRoot, RtiProfile, "rtiddsgen", "csharp");

    public static IReadOnlyList<CorpusCase> Cases => Current.Value.Cases;

    public static IReadOnlyList<string> AllIdlFiles => Current.Value.AllIdlFiles;

    public static IReadOnlyList<ManifestEntry> ManifestRoots => Current.Value.ManifestRoots;

    public static IReadOnlyList<string> OracleCaseIds => Current.Value.OracleCaseIds;

    public static IReadOnlyList<string> OracleFiles(CorpusCase corpusCase)
    {
        return OracleFiles(corpusCase.OracleDirectory);
    }

    private static IReadOnlyList<string> OracleFiles(string directory)
    {
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
                strict: string.Equals(path, rootPath, PathComparer) && corpusCase.Strict,
                defines: string.Equals(path, rootPath, PathComparer) ? corpusCase.Defines : [],
                includeDirectories: Current.Value.IncludeDirectories,
                sequenceArrayMapping: corpusCase.SequenceArrayMapping))
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

        var profileRoot = Path.Combine(corpusRoot, RtiProfile);
        Dictionary<string, OracleEntry>? capturedCases = null;
        var oracleCompileOutcomes = new Dictionary<string, string>(StringComparer.Ordinal);
        if (RtiProfile == "7.3.1")
        {
            var oracleManifest = Read<OracleManifest>(Path.Combine(profileRoot, "manifest.json"));
            capturedCases = oracleManifest.Cases.Concat(oracleManifest.CharacterizationProbes)
                .ToDictionary(entry => entry.Id, StringComparer.Ordinal);
            oracleCompileOutcomes = capturedCases.Values
                .Where(entry => entry.CompileOutcome is not null)
                .ToDictionary(entry => entry.Id, entry => entry.CompileOutcome!, StringComparer.Ordinal);
            foreach (var id in classifications.Keys.ToArray())
            {
                if (capturedCases.TryGetValue(id, out var capturedCase))
                {
                    classifications[id] = capturedCase.Outcome == "generated" ? "accepted" : "rejected";
                }
            }
        }
        else
        {
            var oracleManifest = Read<OracleManifest>(Path.Combine(profileRoot, "manifest.json"));
            oracleCompileOutcomes = oracleManifest.CharacterizationProbes
                .Where(entry => entry.CompileOutcome is not null)
                .ToDictionary(entry => entry.Id, entry => entry.CompileOutcome!, StringComparer.Ordinal);
        }

        var cases = new List<CorpusCase>();

        foreach (var entry in manifest.PositiveCases)
        {
            cases.Add(CreateCase(entry, "features", classifications, profileRoot, capturedCases, oracleCompileOutcomes));
        }

        foreach (var entry in manifest.NegativeCases)
        {
            cases.Add(CreateCase(entry, "negative", classifications, profileRoot, capturedCases, oracleCompileOutcomes));
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
                Path.Combine(profileRoot, capturedCases is null
                    ? Path.Combine("rtiddsgen", "csharp", "integration", group, id)
                    : capturedCases[id].SourceDirectory),
                IdlSequenceArrayMapping.RtiCSharp));
        }

        var explicitTagNumbers = cases
            .Where(corpusCase => corpusCase.Tag.Length > 0)
            .Select(corpusCase => int.Parse(corpusCase.Tag[1..], System.Globalization.CultureInfo.InvariantCulture))
            .ToHashSet();
        var nextTagNumber = 1;
        cases = [.. cases
            .Select(corpusCase =>
            {
                if (corpusCase.Tag.Length > 0)
                {
                    return corpusCase;
                }

                while (explicitTagNumbers.Contains(nextTagNumber))
                {
                    nextTagNumber++;
                }

                return corpusCase with { Tag = $"C{nextTagNumber++:D3}" };
            })
            .OrderBy(corpusCase => int.Parse(corpusCase.Tag[1..], System.Globalization.CultureInfo.InvariantCulture))];

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
            [.. cases.Where(entry => entry.OracleSourcesRetained).Select(entry => entry.Id).OrderBy(id => id, StringComparer.Ordinal)],
            manifest.IncludeDirectories
                .Select(path => Path.GetFullPath(Path.Combine(corpusRoot, path.Replace('/', Path.DirectorySeparatorChar))))
                .ToList());
    }

    private static CorpusCase CreateCase(ManifestEntry entry, string sourceKind, Dictionary<string, string> classifications, string profileRoot, Dictionary<string, OracleEntry>? capturedCases, Dictionary<string, string> oracleCompileOutcomes)
    {
        OracleEntry? capturedCase = null;
        capturedCases?.TryGetValue(entry.Id, out capturedCase);
        var mapping = entry.SequenceArrayMapping is null
            ? IdlSequenceArrayMapping.RtiCSharp
            : Enum.Parse<IdlSequenceArrayMapping>(entry.SequenceArrayMapping, ignoreCase: true);

        return new CorpusCase(
            entry.Id,
            sourceKind,
            entry.Idl,
            entry.Defines,
            classifications[entry.Id],
            Path.Combine(profileRoot, capturedCase is null
                ? entry.OracleDirectory ?? Path.Combine("rtiddsgen", "csharp", sourceKind, entry.Id)
                : capturedCase.SourceDirectory),
            mapping,
            entry.Tag,
            capturedCase is null
                ? entry.OracleSourcesRetained
                : capturedCase.OracleSourcesRetained,
            oracleCompileOutcomes.GetValueOrDefault(entry.Id),
            entry.Strict);
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
