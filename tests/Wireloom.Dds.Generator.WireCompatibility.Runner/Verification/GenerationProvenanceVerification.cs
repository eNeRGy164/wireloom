using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks source selection, exact byte hashes, include closure, and retained provenance artifacts.</summary>
internal static class GenerationProvenanceVerification
{
    /// <summary>Exercises evidence capture without generating code or starting RTI processes.</summary>
    internal static void Run(MatrixOptions options)
    {
        var root = Path.Combine(options.ScratchDirectory, "provenance-verification");
        var repository = Path.Combine(root, "repository");
        var generated = Path.Combine(root, "generated");

        CreateInputs(repository, generated);

        VerifySourceSelection(repository, generated);
        VerifyContentChanges(repository, generated);
        VerifyRecording(options, root, repository);

        Console.WriteLine("Generation provenance passed: wrappers, transitive/cyclic includes, search precedence, byte hashes, source changes, and retained evidence.");
    }

    private static void CreateInputs(string repository, string generated)
    {
        Write(repository, "docs/corpus/manifest.json",
            """
            {
                "positiveCases": [
                    { "id": "plain", "idl": "idl/features/root.idl" },
                    { "id": "custom", "idl": "idl/features/root.idl", "sequenceArrayMapping": "Idl" },
                    { "id": "14-autoid-scope", "idl": "idl/features/root.idl" },
                    { "id": "14-enum-constant", "idl": "idl/features/root.idl" }
                ]
            }
            """);

        Write(repository, "docs/corpus/idl/features/root.idl",
            """
            #include "local.idl"
            #include <dependency.idl>
            #include "missing.idl"
            """);

        Write(repository, "docs/corpus/idl/features/local.idl",
            """
            #include "root.idl"
            #include <shared.idl>
            """);

        Write(repository, "docs/corpus/idl/local.idl", "// Shadowed by the including file's local directory.");
        Write(repository, "docs/corpus/idl/includes/dependency.idl", "#include <shared.idl>");
        Write(repository, "docs/corpus/idl/shared.idl", "// Shared transitive dependency.");

        foreach (var name in new[] { "custom.idl", "14-autoid-scope-topic.idl", "14-enum-constant-topic.idl" })
        {
            Write(repository, $"tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/{name}",
                """
                #include "features/root.idl"
                #include <shared.idl>
                """);
        }

        Write(generated, "nested/a.g.cs", "// Generated source: café\n");

        File.WriteAllText(Path.Combine(generated, "z.g.cs"), "// Generated source Z\r\n", new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        Write(generated, "ignored.cs", "// Excluded from generated evidence.");
    }

    private static void VerifySourceSelection(string repository, string generated)
    {
        // Arrange
        var manifest = Path.Combine(repository, "docs", "corpus", "manifest.json");
        var canonical = Path.Combine(repository, "docs", "corpus", "idl", "features", "root.idl");
        var expectedDependencies = new[]
        {
            "docs/corpus/idl/features/local.idl",
            "docs/corpus/idl/features/root.idl",
            "docs/corpus/idl/includes/dependency.idl",
            "docs/corpus/idl/shared.idl",
        };

        // Act
        var plain = GenerationProvenanceBuilder.Create(repository, manifest, "plain", generated, "7.3.1");
        var custom = GenerationProvenanceBuilder.Create(repository, manifest, "custom", generated, "7.7.0");

        // Assert
        Check(plain.SequenceArrayMapping == "RtiCSharp" && custom.SequenceArrayMapping == "Idl", "Provenance must retain explicit and default mappings.");
        Check(plain.RtiVersion == "7.3.1" && custom.RtiVersion == "7.7.0", "Provenance must retain the exact runtime.");
        Check(plain.OracleIdl == "docs/corpus/idl/features/root.idl" && plain.IdlSha256 == plain.OracleIdlSha256, "An unwrapped case must use its canonical input.");
        Check(custom.Idl == "idl/features/root.idl" && custom.IdlSha256 == Hash(canonical), "Canonical evidence must hash the exact input bytes.");
        Check(custom.OracleDependencies.Select(item => item.Path).SequenceEqual(expectedDependencies, StringComparer.Ordinal), "Include closure must preserve ordering, local search precedence, and cycle deduplication while omitting unresolved includes.");
        Check(custom.OracleDependencies.All(item => item.Sha256 == Hash(Path.Combine(repository, item.Path))), "Every transitive input must retain its own content hash.");
        Check(custom.GeneratedSources.Select(item => item.Path).SequenceEqual(["nested/a.g.cs", "z.g.cs"], StringComparer.Ordinal), "Only emitted .g.cs files must be included, using normalized ordered paths.");
        Check(custom.GeneratedSources.All(item => item.Sha256 == Hash(Path.Combine(generated, item.Path))), "Generated evidence must include original encoding and line-ending bytes.");

        foreach (var caseId in new[] { "14-autoid-scope", "14-enum-constant" })
        {
            // Act
            var wrapped = GenerationProvenanceBuilder.Create(repository, manifest, caseId, generated, "7.7.0");

            // Assert
            Check(wrapped.OracleIdl.EndsWith(caseId + "-topic.idl", StringComparison.Ordinal), "Special topic wrappers must be selected consistently with native preparation.");
            Check(wrapped.OracleIdlSha256 == Hash(Path.Combine(repository, wrapped.OracleIdl)), "Wrapper evidence must hash the actual wrapper.");
            Check(wrapped.OracleDependencies.Select(item => item.Path).SequenceEqual(expectedDependencies, StringComparer.Ordinal), "Wrapper provenance must include its canonical and transitive inputs exactly once.");
        }
    }

    private static void VerifyContentChanges(string repository, string generated)
    {
        // Arrange
        var manifest = Path.Combine(repository, "docs", "corpus", "manifest.json");
        var original = GenerationProvenanceBuilder.Create(repository, manifest, "custom", generated, "7.7.0");
        File.AppendAllText(Path.Combine(generated, "z.g.cs"), "// Changed emitted bytes.");
        File.AppendAllText(Path.Combine(repository, "docs", "corpus", "idl", "features", "root.idl"), "\n// Changed canonical bytes.");

        // Act
        var updated = GenerationProvenanceBuilder.Create(repository, manifest, "custom", generated, "7.7.0");

        // Assert
        Check(updated.IdlSha256 != original.IdlSha256, "Canonical input changes must invalidate its recorded hash.");
        Check(updated.OracleIdlSha256 == original.OracleIdlSha256, "A canonical-input edit must not alter the separate wrapper hash.");
        Check(updated.OracleDependencies.Single(item => item.Path.EndsWith("features/root.idl", StringComparison.Ordinal)).Sha256 == updated.IdlSha256, "Wrapper dependency hashes must reflect canonical-input changes.");
        Check(updated.GeneratedSources[0] == original.GeneratedSources[0] && updated.GeneratedSources[1].Sha256 != original.GeneratedSources[1].Sha256, "An emitted-source edit must change the affected file's evidence.");
    }

    private static void VerifyRecording(MatrixOptions options, string root, string repository)
    {
        // Arrange
        var captureOptions = options with
        {
            RepositoryRoot = repository,
            ScratchDirectory = Path.Combine(root, "run-scratch"),
            OutputDirectory = Path.Combine(root, "run-output"),
        };
        var workspace = new MatrixWorkspace(captureOptions);
        Directory.CreateDirectory(workspace.Logs);
        File.WriteAllText(workspace.GenerationProvenancePath("stale"), "stale");

        // Act
        workspace.Initialize(captureSourceProvenance: false);

        var syntheticSource = new SourceSnapshotEvidence("synthetic-generation-verification",
            SourceSnapshotEvidence.ComputeSnapshotHash([]), 0, []);
        syntheticSource.Write(workspace.ExecutionProvenance);

        // Assert
        Check(!File.Exists(workspace.GenerationProvenancePath("stale")), "Initialization must discard provenance from a previous invocation.");

        // Arrange
        var paths = workspace.ForCase("14-enum-constant");
        Write(paths.WireloomGenerated, "Actual.g.cs", "// Actual emitted source.");
        var recorder = new GenerationProvenanceRecorder(captureOptions, workspace);

        // Act
        recorder.Record(paths);

        // Assert
        var destination = workspace.GenerationProvenancePath(paths.CaseId);
        using var json = JsonDocument.Parse(File.ReadAllText(destination));
        Check(json.RootElement.EnumerateObject().Count() == 8 && json.RootElement.GetProperty("generatedSources")[0].GetProperty("path").GetString() == "Actual.g.cs", "Retained evidence must preserve the existing JSON contract.");
        Check(Directory.GetFiles(workspace.Logs, "*-generation.json").Length == 1, "Recording must retain one case artifact directly in the report logs.");
        Check(Directory.GetFiles(captureOptions.ScratchDirectory, "*-generation.json", SearchOption.AllDirectories).Length == 0, "Recording must not create an intermediate evidence file.");

        // Arrange
        var empty = workspace.ForCase("plain");
        Directory.CreateDirectory(empty.WireloomGenerated);
        var rejectedEmptySources = false;

        // Act
        try
        {
            recorder.Record(empty);
        }
        catch (InvalidDataException)
        {
            rejectedEmptySources = true;
        }

        // Assert
        Check(rejectedEmptySources && !File.Exists(workspace.GenerationProvenancePath("plain")), "An empty generation directory must fail before publishing evidence.");
    }

    private static void Write(string root, string relativePath, string content)
    {
        var path = Path.Combine(root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private static string Hash(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)));

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
