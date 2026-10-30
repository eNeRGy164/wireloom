using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks profile-qualified corpus selection and generated-type discovery.</summary>
internal static class CaseSelectionVerification
{
    internal static void Run(VerificationContext context)
    {
        var verifierAssembly = context.VerifierAssembly;
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;

        void Check(bool condition, string description) => context.Check(condition, description);
        string Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        // A source snapshot intentionally omits .git. Verify the supported container
        // fallback finds its root from GITHUB_WORKSPACE.
        var workspaceSnapshot = Path.Combine(root, "source-without-git");

        Directory.CreateDirectory(workspaceSnapshot);

        var oldWorkspace = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

        try
        {
            Environment.SetEnvironmentVariable("GITHUB_WORKSPACE", workspaceSnapshot);
            Check(Invoke(succeeds: true, "repository-root", verifierAssembly).Trim() == workspaceSnapshot,
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

                Check(selected.Length == (version == "7.3.1" ? 92 : 93), "Only applicable positive cases are selected.");
                Check(selected.Contains("05-array-of-sequences-idl"), "The IDL mapping is an independently selected corpus case.");
                Check(Invoke(succeeds: true, "case-pairings", manifestPath, "05-array-of-sequences-idl")
                        .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .SequenceEqual(["Wireloom C#|Wireloom C#", "Wireloom C#|RTI C++", "RTI C++|Wireloom C#", "RTI C++|RTI C++"]),
                    "The IDL case schedules four pairings and explicitly excludes flattened RTI C#.");
                Check(Invoke(succeeds: true, "case-pairings", manifestPath, "14-typed-defaults", "sparse-omitted").Trim()
                        == "RTI C++|Wireloom C#",
                    "C171's sparse fixture records only its focused writer-to-reader pairing.");
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
    }
}
