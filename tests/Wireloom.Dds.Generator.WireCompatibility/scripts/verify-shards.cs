// Black-box partition/merge checks without an RTI toolchain or license.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

if (args.Length != 1)
{
    throw new ArgumentException("Usage: verify-shards.cs <built-matrix-tools.dll>");
}

var tool = Path.GetFullPath(args[0]);
var repositoryRoot = FindRepositoryRoot(tool);
var rtiVersion = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0";
var temporaryRoot = Path.Combine(repositoryRoot, "tmp");

var root = Path.Combine(temporaryRoot, "wireloom-shards-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);

var checks = 0;

try
{
    // A source snapshot intentionally omits .git. Verify the supported container
    // fallback finds its root from GITHUB_WORKSPACE.
    var workspaceSnapshot = Path.Combine(root, "source-without-git");
    Directory.CreateDirectory(workspaceSnapshot);
    var oldWorkspace = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
    try
    {
        Environment.SetEnvironmentVariable("GITHUB_WORKSPACE", workspaceSnapshot);
        Check(Invoke(succeeds: true, "repository-root", tool).Trim() == workspaceSnapshot,
            "Repository-root discovery uses GITHUB_WORKSPACE when the source snapshot has no .git directory.");
    }
    finally
    {
        Environment.SetEnvironmentVariable("GITHUB_WORKSPACE", oldWorkspace);
    }

    // Arrange / Act / Assert: applicability changes selection before type-support discovery.
    var manifestPath = Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json");
    var previousVersion = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION");

    try
    {
        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION", version);

            var registryPath = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", $"wire-case-exclusions-{version}.json");
            var selected = Invoke(succeeds: true, "list-cases", manifestPath, registryPath).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Check(selected.Length == (version == "7.3.1" ? 90 : 91), "Only applicable positive cases are selected.");
            Check(selected.Contains("05-array-of-sequences-idl"), "The IDL mapping is an independently selected corpus case.");
            Check(Invoke(succeeds: true, "case-pairings", manifestPath, "05-array-of-sequences-idl")
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .SequenceEqual(["Wireloom C#|Wireloom C#", "Wireloom C#|RTI C++", "RTI C++|Wireloom C#", "RTI C++|RTI C++"]),
                "The IDL case schedules four pairings and explicitly excludes flattened RTI C#.");
            Check(Invoke(succeeds: true, "case-field", manifestPath, "05-array-of-sequences-idl", "idl").Trim()
                    == "idl/features/05-array-of-sequences-idl.idl"
                && Invoke(succeeds: true, "case-field", manifestPath, "05-array-of-sequences-idl", "sequence-array-mapping").Trim() == "Idl"
                && Invoke(succeeds: true, "case-field", manifestPath, "05-array-of-sequences", "sequence-array-mapping").Trim() == "RtiCSharp",
                "The dedicated IDL case retains C018's declarations without changing the default mode.");
            Check(selected.Contains("09-autoid-hash"), "Hash-ID coverage applies to both LTS versions.");
            Check(selected.Contains("10-resolve-name-wstring-aliases"), "C147 is a positive interoperability case for both LTS versions.");
            Check(Invoke(succeeds: true, "case-pairings", manifestPath, "10-resolve-name-wstring-aliases")
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .SequenceEqual([
                        "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++",
                        "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++",
                        "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++"
                    ]),
                "C147 schedules all directed pairings among the three peers.");
            Check(selected.Contains("09-must-understand") == (version == "7.7.0"), "The must-understand fixture is available only on 7.7.0.");

            var idlGapCases = new[]
            {
                "14-extensibility-full", "14-autoid-scope", "14-annotation-arguments", "14-member-declarators",
                "14-union-label-expressions", "14-union-scoped-enum", "14-union-array-branch",
                "14-string-expression-bound", "14-typed-defaults", "14-enum-constant"
            };
            Check(idlGapCases.All(selected.Contains), "C163–C172 are applicable positive cases on both LTS versions.");

            var wrappers = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers");
            var canonicalIdl = Path.Combine(repositoryRoot, "docs", "corpus", "idl", "features");
            foreach (var (caseId, wrapperName, wrapperRoot) in new[]
            {
                ("14-autoid-scope", "14-autoid-scope-topic.idl", "AutoIdScopeSample"),
                ("14-enum-constant", "14-enum-constant-topic.idl", "EnumConstantSample")
            })
            {
                var wrapperText = File.ReadAllText(Path.Combine(wrappers, wrapperName));
                var canonicalText = File.ReadAllText(Path.Combine(canonicalIdl, caseId + ".idl"));
                Check(wrapperText.Contains($"#include \"features/{caseId}.idl\"", StringComparison.Ordinal)
                    && wrapperText.Contains($"struct {wrapperRoot}", StringComparison.Ordinal),
                    $"{caseId} wrapper includes the canonical input and declares its aggregate topic root.");
                Check(!wrapperText.Contains("module ", StringComparison.Ordinal)
                    && !wrapperText.Contains("enum ", StringComparison.Ordinal)
                    && !wrapperText.Contains("union ", StringComparison.Ordinal),
                    $"{caseId} wrapper does not rewrite canonical declarations.");
                Check(canonicalText.Length > 0, $"{caseId} canonical corpus IDL is present and independently retained.");
            }

            var wrapperDiscoverySources = Path.Combine(root, "wrapper-discovery-sources");
            Directory.CreateDirectory(wrapperDiscoverySources);
            foreach (var (caseId, wrapperIdl, rootType) in new[]
            {
                ("14-autoid-scope", "14-autoid-scope-topic.idl", "AutoIdScopeSample"),
                ("14-enum-constant", "14-enum-constant-topic.idl", "EnumConstantSample")
            })
            {
                File.WriteAllText(Path.Combine(wrapperDiscoverySources, rootType + ".g.cs"),
                    $"// This file was generated by Wireloom from '{wrapperIdl}'.\nclass {rootType}Support : TypeSupport<{rootType}> {{ }}");
                var wrapperRegistryPath = Path.Combine(root, $"{caseId}-wrapper-discovery.json");
                Invoke(succeeds: true, "discover-cases", manifestPath, wrapperDiscoverySources, wrapperRegistryPath, caseId);
                var discoveredRoot = ReadJson(wrapperRegistryPath)["cases"]!.AsArray().Single()!;
                Check((string?)discoveredRoot["csharpType"] == rootType
                    && (string?)discoveredRoot["cppType"] == "::" + rootType,
                    $"{caseId} wrapper discovery selects the aggregate C# and C++ topic root.");
            }

            var missingWrapperSources = Path.Combine(root, "missing-wrapper-sources");
            Directory.CreateDirectory(missingWrapperSources);
            File.WriteAllText(Path.Combine(missingWrapperSources, "InnerType.g.cs"),
                "// This file was generated by Wireloom from '14-autoid-scope.idl'.\nclass InnerTypeSupport : TypeSupport<AutoIds.Hashed> { }");
            var missingWrapperRoot = Invoke(succeeds: false, "discover-cases", manifestPath, missingWrapperSources,
                Path.Combine(root, "missing-wrapper-root.json"), "14-autoid-scope");
            Check(missingWrapperRoot.Contains("requires generated topic root AutoIdScopeSample", StringComparison.Ordinal),
                "C164 discovery fails explicitly when only an included inner type is emitted.");

            var generatedRoot = Path.Combine(repositoryRoot, "docs", "corpus", version, "generator");
            var discovery = Invoke(version == "7.7.0", "discover-cases", manifestPath, generatedRoot, Path.Combine(root, "discovered.json"), "09-must-understand");
            var enumArrayDiscoveryPath = Path.Combine(root, $"discovered-enum-array-{version}.json");
            Invoke(succeeds: true, "discover-cases", manifestPath, generatedRoot, enumArrayDiscoveryPath, "10-default-literal-enum-array");
            var enumArrayCase = ReadJson(enumArrayDiscoveryPath)["cases"]!.AsArray().Single()!;
            Check((string?)enumArrayCase["csharpType"] == "CorpusDefaultLiteralEnumArray.Shape"
                && (string?)enumArrayCase["cppType"] == "::CorpusDefaultLiteralEnumArray::Shape",
                "C148 discovers its aggregate topic type instead of its enum dependency.");

            if (version == "7.3.1")
            {
                Check(discovery.Contains("not a wire-testable", StringComparison.Ordinal), "Inapplicable cases are rejected before type discovery.");
            }
        }
    }
    finally
    {
        Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION", previousVersion);
    }

    // Arrange: unequal case sizes, all peer pairings, and an RTI-only case.
    // TSV columns match ScenarioRow in matrix-tools.cs: case, three types,
    // writer/version, reader/version, status, fixture.
    var peers = new[] { "Wireloom C#", "RTI C#", "RTI C++" };
    var cases = new Dictionary<string, string[]>
    {
        ["04-strings"] = ["non-default"],
        ["04-boundaries"] = ["string-boundary"],
        ["04-unbounded-string-aliases"] = ["string-empty", "string-boundary"],
        ["04-string-alias-chains"] = ["string-empty", "string-boundary"],
        ["07-union-string-sequence-bounded-direct"] = ["union-narrow-multiple", "union-wide-multiple"],
        ["02-multiple"] = ["non-default"],
        ["03-enum-values-prefix"] = ["default"],
        ["03-enum-values-explicit"] = ["default"],
        ["01-boundaries"] = ["non-default"],
        ["01-full-widths"] = ["non-default"],
        ["09-defaults-ranges"] = ["non-default"],
        ["05-array-of-sequences"] = ["default"],
        ["05-array-of-sequences-idl"] = ["sequence-empty", "sequence-unequal", "sequence-mixed-empty", "sequence-boundary"],
        ["05-collections"] = ["sequence-empty", "sequence-single", "sequence-multiple"],
        ["05-unbounded-sequence-aliases"] = ["sequence-empty", "sequence-multiple"],
        ["07-union-string-sequence-aliases"] = [
            "union-direct-narrow", "union-direct-wide", "union-alias-narrow",
            "union-alias-wide", "union-bounded-narrow", "union-bounded-wide"],
        ["07-union-char-numeric"] = ["union-127", "union-default"],
        ["07-union-char-numeric-255"] = ["union-255", "union-default"],
        ["07-union-char-boundaries"] = ["union-minimum", "union-maximum", "union-default"],
        ["07-union-ulong"] = ["union-minimum", "union-maximum", "union-default"],
        ["09-optional-aggregate-member"] = ["optional-absent", "optional-present"],
        ["09-optional-recursive-array"] = ["optional-absent", "optional-present", "optional-nested"],
        ["10-resolve-name-wstring-aliases"] = ["string-empty", "string-boundary"],
        ["14-extensibility-full"] = ["non-default"],
        ["14-autoid-scope"] = ["scope-nested"],
        ["14-annotation-arguments"] = ["default", "non-default"],
        ["14-member-declarators"] = ["non-default"],
        ["14-union-label-expressions"] = ["symbolic", "arithmetic", "hexadecimal"],
        ["14-union-scoped-enum"] = ["enum-selected", "enum-first"],
        ["14-union-array-branch"] = ["array-branch", "scalar-branch"],
        ["14-string-expression-bound"] = ["string-boundary"],
        ["14-typed-defaults"] = ["default", "non-default"],
        ["14-enum-constant"] = ["constant-and-label"]
    };
    var rows = new List<string[]>();

    foreach (var item in cases)
    {
        var pairings = peers.SelectMany(writer => peers.Select(reader => (Writer: writer, Reader: reader))).ToArray();

        foreach (var fixture in item.Value)
        {
            foreach (var (writer, reader) in pairings)
            {
                if (item.Key == "05-array-of-sequences-idl" && (writer == peers[1] || reader == peers[1]))
                {
                    continue;
                }
                if (item.Key == "09-optional-aggregate-member" && (writer == peers[0] || reader == peers[0]))
                {
                    continue;
                }

                var status = "PASS";
                if (item.Key == "05-array-of-sequences"
                    && ((writer == peers[0] && reader == peers[2]) || (writer == peers[2] && reader == peers[0])))
                {
                    status = "FAIL";
                }
                else if (item.Key == "07-union-ulong" && (writer == peers[1] || reader == peers[1]))
                {
                    status = "FAIL";
                }

                rows.Add([item.Key, "Managed.Type", "Oracle.Type", "::Native::Type", writer, rtiVersion, reader, rtiVersion, status, fixture]);
            }
        }
    }

    var seed = Path.Combine(root, "all.tsv");
    File.WriteAllLines(seed, rows.Select(row => string.Join('\t', row)));

    var exclusions = Path.Combine(root, "exclusions.json");
    File.WriteAllText(exclusions,
        """
        {
          "notImplementedCases": [
            {
              "id": "09-optional-aggregate-member",
              "diagnostic": "TEST",
              "reason": "RTI-only control",
              "issue": "https://github.com/eNeRGy164/wireloom/issues/15"
            }
          ],
          "notApplicableCases": [
            {
              "id": "09-must-understand",
              "reason": "RTI feature unavailable in this test profile"
            }
          ]
        }
        """);

    var source = Path.Combine(root, "shards");

    for (var index = 0; index < 4; index++)
    {
        var directory = Path.Combine(source, $"shard-{index}");
        Invoke(succeeds: true, "partition-shard", rtiVersion, index.ToString(), "4", seed, directory);

        var logs = Path.Combine(directory, "logs");
        Directory.CreateDirectory(logs);

        if (File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Any(line => line.StartsWith("05-array-of-sequences-idl\t", StringComparison.Ordinal)))
        {
            var generated = Path.Combine(root, "generated");
            Directory.CreateDirectory(generated);
            File.WriteAllText(Path.Combine(generated, "Sample.g.cs"), "// Synthetic source for black-box provenance validation.");
            Invoke(succeeds: true, "generation-provenance", manifestPath, "05-array-of-sequences-idl", generated,
                rtiVersion, Path.Combine(logs, "05-array-of-sequences-idl-generation.json"));
        }

        foreach (var manifestCase in ReadJson(manifestPath)["positiveCases"]!.AsArray())
        {
            var caseId = (string)manifestCase!["id"]!;
            var wrapperName = caseId switch
            {
                "14-autoid-scope" => "14-autoid-scope-topic.idl",
                "14-enum-constant" => "14-enum-constant-topic.idl",
                _ => caseId + ".idl"
            };
            var wrapperPath = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers", wrapperName);
            if (!File.Exists(wrapperPath)
                || !File.ReadAllLines(Path.Combine(directory, "scenarios.tsv"))
                    .Any(line => line.StartsWith(caseId + "\t", StringComparison.Ordinal)))
            {
                continue;
            }

            var generated = Path.Combine(root, caseId + "-generated");
            Directory.CreateDirectory(generated);
            File.WriteAllText(Path.Combine(generated, "Sample.g.cs"), "// Synthetic source for wrapper provenance validation.");
            Invoke(succeeds: true, "generation-provenance", manifestPath, caseId, generated,
                rtiVersion, Path.Combine(logs, caseId + "-generation.json"));
        }

        foreach (var row in File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Select(line => line.Split('\t')))
        {
            var peerIds = new Dictionary<string, string>
            {
                [peers[0]] = "wireloom-cs",
                [peers[1]] = "rti-cs",
                [peers[2]] = "rti-cpp",
            };

            var logText = "FAIL_DETAIL retained symptom: NullReferenceException\n";

            if (row[8] == "NOT_RUN")
            {
                logText = "NOT_RUN_DETAIL interrupted exchange\n";
            }
            else if (row[0] == "07-union-ulong" && row[8] == "FAIL")
            {
                logText = "FAIL_DETAIL RTI-generated C# peer failed to compile; CS0221\n";
            }

            File.WriteAllText(Path.Combine(logs, $"{row[0]}-{row[9]}-{peerIds[row[4]]}-{peerIds[row[6]]}.log"), logText);
        }

        var count = File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Length.ToString();

        Invoke(succeeds: true, "render-report", rtiVersion, count, Path.Combine(directory, "scenarios.tsv"),
            Path.Combine(directory, "results.json"), Path.Combine(directory, "results.md"), logs, exclusions,
            Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));
    }

    // Act: compare the merged report with an unsharded rendering of the same rows/logs.
    var merged = Path.Combine(root, "merged");
    Invoke(succeeds: true, "merge-shards", rtiVersion, "4", source, merged, exclusions);

    var baseline = Path.Combine(root, "baseline.json");
    Invoke(succeeds: true, "render-report", rtiVersion, rows.Count.ToString(), seed, baseline,
        Path.Combine(root, "baseline.md"), Path.Combine(merged, "logs"), exclusions,
        Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));

    // Assert: payload/report evidence survives, including registry entries and failure reasons.
    Check(JsonNode.DeepEquals(ReadJson(baseline), ReadJson(Path.Combine(merged, "results.json"))), "Combined JSON equals the unsharded report.");
    var idlScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "05-array-of-sequences-idl").ToArray();
    Check(idlScenarios.Length == 16 && idlScenarios.All(item =>
            (string?)item!["generationProvenance"]!["sequenceArrayMapping"] == "Idl"
            && (string?)item["generationProvenance"]!["rtiVersion"] == rtiVersion
            && item["generationProvenance"]!["generatedSources"]!.AsArray().Count > 0),
        "All variant fixtures preserve mode, runtime, and source provenance through merging.");
    var manifestWrapperIds = ReadJson(manifestPath)["positiveCases"]!.AsArray()
        .Select(item => (string)item!["id"]!)
        .Where(caseId => File.Exists(Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers",
            caseId switch
            {
                "14-autoid-scope" => "14-autoid-scope-topic.idl",
                "14-enum-constant" => "14-enum-constant-topic.idl",
                _ => caseId + ".idl"
            })))
        .ToHashSet(StringComparer.Ordinal);
    var wrapperScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => manifestWrapperIds.Contains((string)item!["case"]!)).ToArray();
    Check(wrapperScenarios.Length > 18 && wrapperScenarios.All(item =>
            (string?)item!["generationProvenance"]!["rtiVersion"] == rtiVersion
            && ((string?)item["generationProvenance"]!["oracleIdl"])!.StartsWith(
                "tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/", StringComparison.Ordinal)
            && item["generationProvenance"]!["oracleDependencies"]!.AsArray().Count > 0
            && item["generationProvenance"]!["generatedSources"]!.AsArray().Count > 0),
        "Manifest wrapper exchanges retain validated wrapper, dependency, runtime, and generated-source provenance.");
    var autoIdProvenance = ReadJson(Path.Combine(merged, "logs", "14-autoid-scope-generation.json"));
    Check(autoIdProvenance["oracleDependencies"]!.AsArray().Any(item =>
            (string?)item!["path"] == "docs/corpus/idl/features/14-autoid-scope.idl")
        && autoIdProvenance["oracleDependencies"]!.AsArray().Any(item =>
            (string?)item!["path"] == "docs/corpus/idl/includes/autoid-hash-include.idl"),
        "C164 wrapper provenance includes the canonical source and its transitive include dependency.");
    var idlMarkdown = File.ReadAllLines(Path.Combine(merged, "results.md"))
        .Single(line => line.StartsWith("| `05-array-of-sequences-idl.idl`", StringComparison.Ordinal));
    Check(idlMarkdown.Split("N/A", StringSplitOptions.None).Length == 6
        && !idlMarkdown.Contains("❔", StringComparison.Ordinal),
        "The five excluded RTI C# pairings are N/A rather than missing exchanges.");

    // Arrange / Act / Assert: missing or different generation provenance must not qualify.
    var provenanceFile = Path.Combine(merged, "logs", "05-array-of-sequences-idl-generation.json");
    var originalProvenance = File.ReadAllText(provenanceFile);
    foreach (var field in new[] { "sequenceArrayMapping", "rtiVersion", "idlSha256", "idl" })
    {
        ChangeJson(provenanceFile, node => node[field] = "incorrect");
        Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
                Path.Combine(root, "bad-mode.json"), Path.Combine(root, "bad-mode.md"),
                Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match", StringComparison.Ordinal),
            "Reports reject mismatched generation provenance.");
        File.WriteAllText(provenanceFile, originalProvenance);
    }

    ChangeJson(provenanceFile, node => node["generatedSources"] = new JsonArray());
    Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
            Path.Combine(root, "missing-sources.json"), Path.Combine(root, "missing-sources.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match", StringComparison.Ordinal),
        "Reports reject provenance without generated sources.");
    File.WriteAllText(provenanceFile, originalProvenance);

    var wrapperProvenanceFile = Path.Combine(merged, "logs", "14-autoid-scope-generation.json");
    var originalWrapperProvenance = File.ReadAllText(wrapperProvenanceFile);
    File.Delete(wrapperProvenanceFile);
    Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
            Path.Combine(root, "missing-wrapper-provenance.json"), Path.Combine(root, "missing-wrapper-provenance.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance is missing for 14-autoid-scope", StringComparison.Ordinal),
        "Reports reject completed wrapper exchanges when generation provenance is missing.");
    File.WriteAllText(wrapperProvenanceFile, originalWrapperProvenance);

    ChangeJson(wrapperProvenanceFile, node =>
        node["oracleDependencies"]!.AsArray()[0]!["sha256"] = "incorrect");
    Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
            Path.Combine(root, "bad-wrapper-dependency.json"), Path.Combine(root, "bad-wrapper-dependency.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance does not match 14-autoid-scope", StringComparison.Ordinal),
        "Reports reject wrapper provenance with a mismatched dependency hash.");
    File.WriteAllText(wrapperProvenanceFile, originalWrapperProvenance);

    File.Delete(provenanceFile);
    Check(Invoke(succeeds: false, "render-report", rtiVersion, rows.Count.ToString(), seed,
            Path.Combine(root, "missing-mode.json"), Path.Combine(root, "missing-mode.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("provenance is missing", StringComparison.Ordinal),
        "Reports reject completed variant exchanges without generation provenance.");
    File.WriteAllText(provenanceFile, originalProvenance);

    var excludedPeerRow = rows.Single(row => row[0] == "05-array-of-sequences-idl"
        && row[9] == "sequence-empty" && row[4] == peers[0] && row[6] == peers[0]).ToArray();
    excludedPeerRow[6] = peers[1];
    var excludedPeerSeed = Path.Combine(root, "excluded-peer.tsv");
    File.WriteAllText(excludedPeerSeed, string.Join('\t', excludedPeerRow));
    Check(Invoke(succeeds: false, "render-report", rtiVersion, "1", excludedPeerSeed,
            Path.Combine(root, "excluded-peer.json"), Path.Combine(root, "excluded-peer.md"),
            Path.Combine(merged, "logs"), exclusions, manifestPath).Contains("excluded peer", StringComparison.Ordinal),
        "Reports reject exchanges outside the variant's declared peer scope.");

    // Arrange / Act / Assert: an early setup failure still produces a truthful NOT_RUN report.
    excludedPeerRow[6] = peers[0];
    excludedPeerRow[8] = "NOT_RUN";
    File.WriteAllText(excludedPeerSeed, string.Join('\t', excludedPeerRow));
    var emptyLogs = Path.Combine(root, "empty-logs");
    Directory.CreateDirectory(emptyLogs);
    Invoke(succeeds: true, "render-report", rtiVersion, "1", excludedPeerSeed,
        Path.Combine(root, "early-failure.json"), Path.Combine(root, "early-failure.md"), emptyLogs, exclusions, manifestPath);
    Check((string?)ReadJson(Path.Combine(root, "early-failure.json"))["scenarios"]![0]!["status"] == "NOT_RUN",
        "Missing build provenance never turns an early setup failure into completed qualification.");
    Check((int)ReadJson(baseline)["notApplicableCount"]! == 1, "Unavailable features are recorded separately from failures.");
    Check(ReadJson(baseline)["scenarios"]!.AsArray().All(item => (string?)item!["case"] != "09-must-understand"), "Unavailable features add no exchanges to the denominator.");
    Check(File.ReadAllText(Path.Combine(merged, "results.md")).Contains("Not applicable", StringComparison.Ordinal), "The Markdown report explains unavailable features.");

    var unavailableSeed = Path.Combine(root, "unavailable.tsv");
    var unavailableRow = rows[0].ToArray();
    unavailableRow[0] = "09-must-understand";
    File.WriteAllText(unavailableSeed, string.Join('\t', unavailableRow));

    var unavailableResult = Invoke(succeeds: false, "render-report", rtiVersion, "1", unavailableSeed,
        Path.Combine(root, "unavailable.json"), Path.Combine(root, "unavailable.md"),
        Path.Combine(merged, "logs"), exclusions, manifestPath);
    Check(unavailableResult.Contains("not applicable", StringComparison.Ordinal), "Reports reject an exchange for an unavailable feature instead of counting it.");

    // Assert: endpoint notes describe the selected release, without leaking another profile's defect rationale.
    var otherVersion = rtiVersion == "7.3.1" ? "7.7.0" : "7.3.1";
    foreach (var scenario in ReadJson(baseline)["scenarios"]!.AsArray())
    {
        foreach (var role in new[] { "writer", "reader" })
        {
            var adaptation = (string?)scenario![role]!["adaptation"] ?? "";
            Check(!adaptation.Contains(otherVersion, StringComparison.Ordinal),
                "Endpoint adaptations do not attribute another release's defects to the selected profile.");
        }
    }

    // Assert: typed fixture construction is normal on both profiles.
    foreach (var caseId in new[] { "01-boundaries", "01-full-widths", "09-defaults-ranges" })
    {
        var scalarScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == caseId).ToArray();
        Check(scalarScenarios.Length == 9, "All scalar adaptation pairings remain in the report.");

        foreach (var scenario in scalarScenarios)
        {
            foreach (var role in new[] { "writer", "reader" })
            {
                var peer = scenario![role]!;
                var adaptation = (string?)peer["adaptation"] ?? "";
                Check(adaptation.Length == 0,
                    "Scalar and constructor fixtures are not wire adaptations on either profile.");
            }
        }
    }

    var scalarMarkdown = File.ReadAllText(Path.Combine(merged, "results.md"));

    // Assert: compact presentation keeps the profile and affected peers explicit.
    Check(scalarMarkdown.StartsWith($"# {rtiVersion} Wire compatibility results\n", StringComparison.Ordinal),
        "The report title identifies the tested profile.");

    var header = scalarMarkdown.Split('\n').Single(line => line.StartsWith("| IDL scenario |", StringComparison.Ordinal));
    Check(header.Contains("Wireloom → Wireloom", StringComparison.Ordinal)
        && !header.Contains("Wireloom C#", StringComparison.Ordinal)
        && !header.Contains("🧵", StringComparison.Ordinal),
        "Table headings use the compact Wireloom label without changing RTI peer labels.");

    var boundaryRow = scalarMarkdown.Split('\n').Single(line => line.StartsWith("| `01-boundaries.idl`", StringComparison.Ordinal));
    Check(!boundaryRow.Contains("Typed fixture:", StringComparison.Ordinal),
        "Normal generated sample construction is not described as a wire workaround.");

    Check(scalarMarkdown.Contains("`-funsigned-char`", StringComparison.Ordinal)
        && scalarMarkdown.Contains("`UnionMember` label: `255` → `-1`", StringComparison.Ordinal)
        && scalarMarkdown.Contains("`0xFF`", StringComparison.Ordinal),
        "Compiler flags, API identifiers, and discriminator values use code spans.");

    var recursiveRow = scalarMarkdown.Split('\n').Single(line => line.StartsWith("| `09-optional-recursive-array.idl`", StringComparison.Ordinal));
    Check(recursiveRow.Contains("**Wireloom, RTI C#, RTI C++:**", StringComparison.Ordinal)
        && recursiveRow.Split("pool_buffer_max_size=4096").Length == 2
        && recursiveRow.Contains("**RTI C#:** Bootstrap", StringComparison.Ordinal)
        && recursiveRow.Contains("**RTI C++:** Forward", StringComparison.Ordinal),
        "Common recursive setup is grouped while peer-specific corrections remain distinct.");

    Check(scalarMarkdown.Contains("`NullReferenceException`", StringComparison.Ordinal),
        "Technical exception names in explanations use code spans.");

    Check(scalarMarkdown.Contains("“retained symptom: NullReferenceException”", StringComparison.Ordinal),
        "Observed error bodies are quoted without changing their content.");

    Check(!scalarMarkdown.Contains("``", StringComparison.Ordinal),
        "Existing code spans are not formatted a second time.");

    Check(!scalarMarkdown.Contains("leave `color` untouched", StringComparison.Ordinal),
        "Constructor fixture setup is not a compatibility workaround.");

    Check(!scalarMarkdown.Contains("negative `int8` values", StringComparison.Ordinal),
        "Scalar fixture assignment is not a compatibility workaround.");

    var sequenceScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] is "05-unbounded-sequence-aliases" or "07-union-string-sequence-aliases")
        .ToArray();
    Check(sequenceScenarios.Length == 72 && sequenceScenarios.All(item =>
        new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
        "Normal generated-sample construction is not an adaptation for any peer in either sequence case.");
    var optionalAggregateScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "09-optional-aggregate-member").ToArray();
    Check(optionalAggregateScenarios.Length == 8 && optionalAggregateScenarios.All(item =>
        new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
        "Ordinary optional aggregate fixture construction is not an RTI workaround.");

    foreach (var caseId in new[] { "04-strings", "04-boundaries", "04-unbounded-string-aliases",
        "04-string-alias-chains", "07-union-string-sequence-bounded-direct",
        "02-multiple", "03-enum-values-prefix", "03-enum-values-explicit" })
    {
        var normalScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == caseId).ToArray();
        Check(normalScenarios.Length > 0 && normalScenarios.All(item =>
            new[] { "writer", "reader" }.All(role => item![role]!["adaptation"] is null)),
            "Case-specific typed fixtures and wrappers do not imply an RTI wire workaround.");
    }

    var workaroundDocument = File.ReadAllText(Path.Combine(repositoryRoot, "tests",
        "Wireloom.Dds.Generator.WireCompatibility", "WORKAROUNDS.md"));
    var workaroundAnchors = Regex.Matches(workaroundDocument, @"(?m)^## (.+)$")
        .Select(heading => Regex.Replace(heading.Groups[1].Value.Trim().ToLowerInvariant(), @"[^\w\s-]", "")
            .Replace(' ', '-')).ToHashSet(StringComparer.Ordinal);
    workaroundAnchors.UnionWith(Regex.Matches(workaroundDocument, "<a id=\"(?<anchor>[^\"]+)\"></a>")
        .Select(anchor => anchor.Groups["anchor"].Value));

    var reportGuideLinks = Regex.Matches(scalarMarkdown, @"WORKAROUNDS\.md#(?<anchor>[^)]+)");
    Check(reportGuideLinks.Count > 0 && reportGuideLinks.All(link => workaroundAnchors.Contains(link.Groups["anchor"].Value)),
        "Every case-specific report link resolves to a workaround section.");
    Check(!scalarMarkdown.Contains("Typed C++ fixture", StringComparison.Ordinal),
        "Normal typed C++ fixture paths do not create workaround annotations.");

    var numericScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] is "07-union-char-numeric" or "07-union-char-numeric-255").ToArray();
    Check(numericScenarios.Length == 36 && numericScenarios.All(item =>
        new[] { "writer", "reader" }.Where(role => (string?)item![role]!["peer"] == "RTI C++")
            .All(role => (item![role]!["adaptation"] is not null) == ((string?)item!["case"] == "07-union-char-numeric-255"))),
        "Only the high-label C++ binding needs compiler and label-metadata corrections.");

    Check(scalarMarkdown.Contains("payload `13702` unchanged", StringComparison.Ordinal),
        "Metadata corrections preserve the high-label fixture payload.");

    var recursiveScenarios = ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "09-optional-recursive-array").ToArray();
    Check(recursiveScenarios.Length == 27, "All recursive array exchanges are retained.");

    foreach (var scenario in recursiveScenarios)
    {
        foreach (var role in new[] { "writer", "reader" })
        {
            var peer = scenario![role]!;
            var adaptation = (string?)peer["adaptation"] ?? "";
            Check(adaptation.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
                "Every recursive peer reports the writer fast-pool threshold.");
            Check(adaptation.Contains("Bootstrap", StringComparison.Ordinal) == ((string?)peer["peer"] == "RTI C#"),
                "Only the RTI C# reference reports a recursive support bootstrap.");
        }
    }

    Check(scalarMarkdown.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
        "The Markdown report discloses the recursive writer memory adaptation.");

    Check(ReadJson(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] != "09-optional-recursive-array")
        .All(item => new[] { "writer", "reader" }.All(role =>
            !((string?)item![role]!["adaptation"] ?? "").Contains("pool_buffer_max_size", StringComparison.Ordinal))),
        "The writer memory adaptation is scoped to recursive-array scenarios.");

    Check(File.ReadAllText(Path.Combine(root, "baseline.md")) == File.ReadAllText(Path.Combine(merged, "results.md")), "Combined Markdown equals the unsharded report.");

    var unsignedScenarios = ReadJson(Path.Combine(merged, "results.json"))["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "07-union-ulong").ToArray();
    Check(unsignedScenarios.Count(item => (string?)item!["status"] == "FAIL"
        && ((string?)item["observedProblem"])!.Contains("CS0221", StringComparison.Ordinal)) == 15,
        "All RTI C# build-blocked pairings retain failure status and compiler evidence.");

    Check(unsignedScenarios.Count(item => (string?)item!["status"] == "PASS") == 12,
        "Wireloom and C++ controls remain passing when the RTI C# peer cannot compile.");

    var markdownLines = File.ReadAllLines(Path.Combine(merged, "results.md"));
    var compilerRows = markdownLines.Where(line => line.Contains("CS0221", StringComparison.Ordinal)).ToArray();
    var compilerPairings = new[]
    {
        "Wireloom → RTI C#",
        "RTI C# → Wireloom",
        "RTI C# → RTI C#",
        "RTI C# → RTI C++",
        "RTI C++ → RTI C#",
    };

    Check(compilerRows.Length == 3 && compilerRows.All(line => line.Split("CS0221").Length == 2
        && compilerPairings.All(pairing => line.Contains(pairing, StringComparison.Ordinal))),
        "Each row shows one compiler message and identifies all five affected pairings.");

    var arrayRow = markdownLines.Single(line => line.StartsWith("| `05-array-of-sequences.idl`", StringComparison.Ordinal));
    Check(arrayRow.Split("retained symptom").Length == 2
        && arrayRow.Contains("**Wireloom → RTI C++, RTI C++ → Wireloom:**", StringComparison.Ordinal),
        "Pairings with the same observed failure are grouped together.");
    var report = ReadJson(Path.Combine(merged, "results.json"));
    var aliasScenarios = report["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "10-resolve-name-wstring-aliases").ToArray();
    Check(aliasScenarios.Length == 18 && aliasScenarios.All(item => (string?)item!["status"] == "PASS"),
        "C147 reports all 18 empty and boundary exchanges passing across its three peers.");
    var aliasReport = File.ReadAllText(Path.Combine(merged, "results.md"));
    Check(aliasReport.Contains("wstring-alias-typecode-tags", StringComparison.Ordinal)
        && aliasReport.Contains("Post-generation namespace correction", StringComparison.Ordinal)
        && aliasReport.Contains("Post-generation `TypeCode` correction", StringComparison.Ordinal),
        "C147 report rows link both guarded RTI peer corrections.");
    Check(report!["expectedFailures"] is null && report["expectedFailureCount"] is null
        && report["scenarios"]!.AsArray().All(item => item!["expectedFailure"] is null),
        "JSON reports contain no expected-failure classification.");
    Check(!File.ReadAllText(Path.Combine(root, "baseline.md")).Contains("Expected failure", StringComparison.Ordinal)
        && !File.ReadAllText(Path.Combine(root, "baseline.md")).Contains("Unexpected failure", StringComparison.Ordinal),
        "Markdown reports use one failure status.");

    var shardRows = Enumerable.Range(0, 4).Select(index => File.ReadAllLines(Path.Combine(source, $"shard-{index}", "scenarios.tsv"))).ToArray();
    Check(shardRows.Sum(items => items.Length) == rows.Count, "Every exchange is retained exactly once.");

    var caseSets = shardRows.Select(items => items.Select(line => line.Split('\t')[0]).ToHashSet()).ToArray();
    Check(caseSets.Sum(items => items.Count) == cases.Count && caseSets.SelectMany(items => items).Distinct().Count() == cases.Count, "Whole cases belong to exactly one shard.");

    Check(shardRows.Max(items => items.Length) - shardRows.Min(items => items.Length) <= rows.GroupBy(row => row[0]).Max(group => group.Count()), "Exchange counts are balanced within one indivisible case.");

    var repeat = Path.Combine(root, "repeat");
    Invoke(succeeds: true, "partition-shard", rtiVersion, "0", "4", seed, repeat);
    Check(JsonNode.DeepEquals(ReadJson(Path.Combine(source, "shard-0", "plan.json")), ReadJson(Path.Combine(repeat, "plan.json"))), "Independent plans are deterministic.");

    ExpectMergeFailure("missing-shard", directory => Directory.Move(Path.Combine(directory, "shard-1"), Path.Combine(root, "removed-shard")));
    ExpectMergeFailure("duplicate-index", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["shardIndex"] = 0));
    ExpectMergeFailure("different-plan", directory => ChangeJson(Path.Combine(directory, "shard-1", "plan.json"), node => node["expectedScenarioCount"] = 999));
    ExpectMergeFailure("wrong-version", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["rtiVersion"] = rtiVersion == "7.7.0" ? "7.3.1" : "7.7.0"));
    ExpectMergeFailure("missing-exchange", directory => ChangeRows(directory, lines => lines.Skip(1).ToArray()));
    ExpectMergeFailure("unexecuted-exchange", directory => ChangeRows(directory, lines =>
    {
        var fields = lines[0].Split('\t');
        fields[8] = "NOT_RUN";
        return [string.Join('\t', fields), .. lines.Skip(1)];
    }));
    ExpectMergeFailure("duplicate-exchange", directory => ChangeRows(directory, lines => lines.Skip(1).Append(lines[1]).ToArray()));
    ExpectMergeFailure("unexpected-fixture", directory => ChangeRows(directory, lines => [lines[0] + "-unknown", .. lines.Skip(1)]));
    ExpectMergeFailure("invalid-status", directory => ChangeRows(directory, lines => [lines[0].Replace("\tPASS\t", "\tUNKNOWN\t"), .. lines.Skip(1)]));
    ExpectMergeFailure("wrong-report-count", directory => ChangeJson(Path.Combine(directory, "shard-0", "results.json"), node => node["scenarioCount"] = 999));

    Invoke(succeeds: false, "partition-shard", "7.7.0", "4", "4", seed, Path.Combine(root, "invalid-index"));
    checks++;

    Invoke(succeeds: false, "partition-shard", "7.7.0", "0", "0", seed, Path.Combine(root, "invalid-count"));
    checks++;

    var duplicateSeed = Path.Combine(root, "duplicate.tsv");
    File.WriteAllLines(duplicateSeed, rows.Select(row => string.Join('\t', row)).Append(string.Join('\t', rows[0])));

    Invoke(succeeds: false, "partition-shard", "7.7.0", "0", "4", duplicateSeed, Path.Combine(root, "duplicate-plan"));
    checks++;

    VerifyReferenceBindingCorrections();

    Invoke(succeeds: false, "adapt-rti-characters", "csharp", "unrelated-case", root);
    Invoke(succeeds: false, "adapt-rti-characters", "cpp", "07-union-char-numeric", root);
    Invoke(succeeds: false, "adapt-rti-characters", "unknown-language", "07-union-char-numeric-255", root);
    checks += 3;

    Check(numericScenarios.All(item => new[] { "writer", "reader" }
        .Where(role => (string?)item![role]!["peer"] == "RTI C#")
        .All(role => ((string?)item![role]!["adaptation"] ?? "").Contains("Post-generation RTI C#", StringComparison.Ordinal))),
        "Every RTI C# numeric-character endpoint discloses its corrected generated binding.");

    Check(scalarMarkdown.Contains("`TypeCode` label: `255` → `-1`", StringComparison.Ordinal)
        && scalarMarkdown.Contains("`Setfallback`", StringComparison.Ordinal),
        "Markdown reports disclose the C++ label and C# discriminator-preservation corrections.");

    Console.WriteLine($"Shard verification passed: {checks} checks, {rows.Count} exchanges, no RTI license required.");

    void VerifyReferenceBindingCorrections()
    {
        // Exercise guarded production-style post-generation edits against both retained RTI toolchains.
        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            var referenceExclusions = ReadJson(Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", $"wire-case-exclusions-{version}.json"));
            Check(referenceExclusions["expectedFailures"] is null,
                "Profile exclusions contain no expected-failure registry.");

            foreach (var caseId in new[] { "07-union-char-numeric", "07-union-char-numeric-255" })
            {
                var referenceRoot = Path.Combine(repositoryRoot, "docs", "corpus", version, "rtiddsgen");
                var originalDirectory = Path.Combine(referenceRoot, "csharp", "features", caseId);
                var patchDirectory = Path.Combine(root, $"reference-{version}-{caseId}");
                Directory.CreateDirectory(patchDirectory);

                var originalSample = File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}.cs"));
                var originalPlugin = File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}Plugin.cs"));
                var samplePath = Path.Combine(patchDirectory, $"{caseId}.cs");
                var pluginPath = Path.Combine(patchDirectory, $"{caseId}Plugin.cs");

                File.WriteAllText(samplePath, originalSample);
                File.WriteAllText(pluginPath, originalPlugin);

                Invoke(succeeds: true, "adapt-rti-characters", "csharp", caseId, patchDirectory);

                var adaptedSample = File.ReadAllText(samplePath);
                var adaptedPlugin = File.ReadAllText(pluginPath);
                var label = caseId.EndsWith("-255", StringComparison.Ordinal) ? "255" : "127";
                Check(adaptedSample.Contains($"Discriminator = (char){label};", StringComparison.Ordinal)
                    && adaptedPlugin.Contains("sample.Setfallback(fallback, NativeChar.FromUtf8(_d));", StringComparison.Ordinal),
                    "C# reference corrections compile numeric chars and preserve the incoming fallback discriminator.");

                Check(adaptedPlugin.Contains("new int[] { -1 }", StringComparison.Ordinal) == (label == "255"),
                    "Only high character metadata uses signed-byte normalization.");

                Invoke(succeeds: true, "adapt-rti-characters", "csharp", caseId, patchDirectory);
                Check(adaptedSample == File.ReadAllText(samplePath) && adaptedPlugin == File.ReadAllText(pluginPath),
                    "Reapplying the reference correction is idempotent.");

                Check(originalSample == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}.cs"))
                    && originalPlugin == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}Plugin.cs")),
                    "Retained RTI captures are untouched.");

                File.WriteAllText(samplePath, originalSample);
                File.WriteAllText(pluginPath, originalPlugin.Replace("sample.fallback = fallback;", "/* unexpected generated shape */", StringComparison.Ordinal));

                var invalidPlugin = File.ReadAllText(pluginPath);
                Invoke(succeeds: false, "adapt-rti-characters", "csharp", caseId, patchDirectory);
                Check(originalSample == File.ReadAllText(samplePath) && invalidPlugin == File.ReadAllText(pluginPath), "Unexpected generated code fails before either C# file is changed.");
            }

            var cppCase = "07-union-char-numeric-255";
            var cppDirectory = Path.Combine(root, $"cpp-reference-{version}");
            Directory.CreateDirectory(cppDirectory);

            var cppPath = Path.Combine(cppDirectory, "idl.cxx");
            var originalCpp = File.ReadAllText(Path.Combine(repositoryRoot, "docs", "corpus", version,
                "rtiddsgen", "cpp", "features", cppCase, $"{cppCase}.cxx"));

            File.WriteAllText(cppPath, originalCpp);

            Invoke(succeeds: true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);

            var adaptedCpp = File.ReadAllText(cppPath);
            Check(adaptedCpp == originalCpp.Replace("static_cast<int>(255)", "static_cast<int>(-1)", StringComparison.Ordinal),
                "The C++ adaptation changes only the native TypeCode label.");

            Invoke(succeeds: true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
            Check(adaptedCpp == File.ReadAllText(cppPath), "The C++ metadata correction is idempotent.");

            File.WriteAllText(cppPath, originalCpp.Replace("static_cast<int>(255)", "unexpected-label", StringComparison.Ordinal));

            var invalidCpp = File.ReadAllText(cppPath);
            Invoke(succeeds: false, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
            Check(invalidCpp == File.ReadAllText(cppPath), "An unexpected C++ label fails without changing source.");
        }
    }

    void ExpectMergeFailure(string name, Action<string> mutate)
    {
        var directory = Path.Combine(root, name);
        CopyDirectory(source, directory);

        mutate(directory);

        Invoke(succeeds: false, "merge-shards", rtiVersion, "4", directory, Path.Combine(root, name + "-output"), exclusions);

        checks++;
    }
}
finally
{
    // Only remove the unique temporary directory created by this verification.
    if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(temporaryRoot) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Verification output is outside the temporary directory.");
    }

    Directory.Delete(root, recursive: true);
}

static string FindRepositoryRoot(string path)
{
    // CI builds the utility under RUNNER_TEMP, outside the checkout.
    foreach (var startPath in new[] { Environment.CurrentDirectory, Path.GetDirectoryName(Path.GetFullPath(path))! })
    {
        var directory = new DirectoryInfo(startPath);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "docs", "corpus", "manifest.json")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }
    }

    throw new DirectoryNotFoundException("Could not locate the repository corpus manifest.");
}

string Invoke(bool succeeds, params string[] command)
{
    var start = new ProcessStartInfo("dotnet")
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
    };
    start.ArgumentList.Add(tool);
    foreach (var argument in command)
    {
        start.ArgumentList.Add(argument);
    }

    using var process = Process.Start(start)!;
    var output = process.StandardOutput.ReadToEndAsync();
    var error = process.StandardError.ReadToEndAsync();
    if (!process.WaitForExit(30000))
    {
        process.Kill(entireProcessTree: true);
        throw new TimeoutException("Shard utility exceeded 30 seconds.");
    }

    Task.WaitAll(output, error);

    if (process.ExitCode == 0 != succeeds)
    {
        throw new InvalidOperationException($"{command[0]} exit {process.ExitCode}: {output.Result} {error.Result}");
    }

    return output.Result + error.Result;
}

void Check(bool condition, string description)
{
    if (!condition)
    {
        throw new InvalidOperationException(description);
    }

    checks++;
}

JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

void ChangeJson(string path, Action<JsonNode> change)
{
    var node = ReadJson(path);

    change(node);

    File.WriteAllText(path, node.ToJsonString());
}

void ChangeRows(string directory, Func<string[], string[]> change)
{
    var path = Path.Combine(directory, "shard-0", "scenarios.tsv");

    File.WriteAllLines(path, change(File.ReadAllLines(path)));
}

void CopyDirectory(string source, string destination)
{
    Directory.CreateDirectory(destination);

    foreach (var file in Directory.GetFiles(source))
    {
        File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
    }

    foreach (var directory in Directory.GetDirectories(source))
    {
        CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)));
    }
}
