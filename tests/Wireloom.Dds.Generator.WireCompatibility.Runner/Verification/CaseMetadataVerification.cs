using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks typed case fields, manifest snapshot lifetime, and native adaptation consumption.</summary>
internal static class CaseMetadataVerification
{
    /// <summary>Exercises cached metadata and native IDL staging without utility subprocesses or RTI.</summary>
    internal static async Task RunAsync(MatrixOptions options)
    {
        VerifyFields();

        var repository = Path.Combine(options.ScratchDirectory, "case-metadata-verification");
        var manifest = Path.Combine(repository, "docs", "corpus", "manifest.json");

        // Arrange: composition must not read the manifest before run initialization.
        var catalog = new CorpusCaseCatalog(manifest);

        Directory.CreateDirectory(Path.GetDirectoryName(manifest)!);

        await File.WriteAllTextAsync(manifest,
            """
            { "positiveCases": [
              { "id": "adapted", "idl": "idl/labels.idl", "wireMatrix": {
                "rtiCppIdlAdaptations": [ "7.3.1:numeric-char-labels" ]
              } },
              { "id": "CASE", "idl": "upper.idl" },
              { "id": "case", "idl": "lower.idl" }
            ], "negativeCases": [ { "id": "negative", "idl": "invalid.idl" } ] }
            """);

        // Act
        var selected = catalog.Get("adapted");

        File.Delete(manifest);

        var retained = catalog.Get("adapted");

        // Assert
        Check(retained.Input == selected.Input, "Build fields must remain available from the run's in-memory snapshot.");
        Check(catalog.Get("CASE").Idl == "upper.idl" && catalog.Get("case").Idl == "lower.idl", "Case identity must remain ordinal.");
        CheckRejected<InvalidOperationException>(() => catalog.Get("missing"), "Unknown cases must remain errors.");
        CheckRejected<InvalidOperationException>(() => catalog.Get("negative"), "Negative cases must not become build inputs.");
        CheckRejected<ArgumentException>(() => catalog.Get(" "), "Blank case identity must be rejected.");

        // Arrange / Act / Assert: a new run must load its own snapshot.
        await File.WriteAllTextAsync(manifest,
            """
            { "positiveCases": [ { "id": "adapted", "idl": "changed.idl" } ] }
            """);

        Check(new CorpusCaseCatalog(manifest).Get("adapted").Idl == "changed.idl", "A new run must see changed manifest metadata.");

        File.Delete(manifest);

        await VerifyNativeStagingAsync(options, repository, catalog, selected);

        Console.WriteLine("Case metadata passed: defaults, ordered fields, ordinal positive-case lookup, per-run snapshots, and profile-specific native IDL staging; no case-field subprocess required.");
    }

    private static void VerifyFields()
    {
        // Arrange
        var defaults = JsonNode.Parse("""{ "idl": null, "sequenceArrayMapping": null, "defines": null, "wireMatrix": null }""")!;
        var definition = JsonNode.Parse(
            """
            {
                "idl": "idl/source.idl",
                "sequenceArrayMapping": "Idl",
                "defines": [ "Z=1", "A", "Z=1" ],
                "wireMatrix": {
                    "rtiCppCodegenOptions": [ "-unboundedSupport", "-example", "x64Linux" ],
                    "rtiCppIdlAdaptations": [ "7.3.1:numeric-char-labels", "7.7.0:other" ]
                }
            }
            """)!;

        // Act
        var empty = CaseMetadata.Read(defaults);
        var omitted = CaseMetadata.Read(JsonNode.Parse("{}")!);
        var metadata = CaseMetadata.Read(definition);

        // Assert
        foreach (var value in new[] { empty, omitted })
        {
            Check(value.Input == new IdlInput("", "RtiCSharp"), "Missing/null scalars must retain standalone defaults.");
            Check(value.Defines.Count == 0 && value.RtiCppCodegenOptions.Count == 0 && value.RtiCppIdlAdaptations.Count == 0, "Missing/null lists must remain empty.");
        }

        Check(metadata.Input == new IdlInput("idl/source.idl", "Idl"), "Managed inputs must retain explicit IDL and mapping.");
        Check(metadata.FieldLines("idl").SequenceEqual(["idl/source.idl"], StringComparer.Ordinal), "The IDL field must retain its single-line contract.");
        Check(metadata.FieldLines("sequence-array-mapping").SequenceEqual(["Idl"], StringComparer.Ordinal), "The mapping field must retain its single-line contract.");
        Check(metadata.FieldLines("defines").SequenceEqual(["Z=1", "A", "Z=1"], StringComparer.Ordinal), "Definition order and duplicate values must be preserved.");
        Check(metadata.FieldLines("rti-cpp-codegen-options").SequenceEqual(["-unboundedSupport", "-example", "x64Linux"], StringComparer.Ordinal), "Native options must retain argument order.");
        Check(metadata.FieldLines("rti-cpp-idl-adaptations").SequenceEqual(["7.3.1:numeric-char-labels", "7.7.0:other"], StringComparer.Ordinal), "Adaptation tokens must retain their exact versions and order.");

        CheckRejected<ArgumentException>(() => metadata.FieldLines("unknown"), "Unknown field names must remain errors.");
    }

    private static async Task VerifyNativeStagingAsync(MatrixOptions options, string repository, CorpusCaseCatalog catalog, CaseMetadata metadata)
    {
        // Arrange
        const string idl = "union Labels switch (char) { case '\\0': long zero; case '~': long tilde; };";

        var source = Path.Combine(repository, "docs", "corpus", metadata.Idl);

        Directory.CreateDirectory(Path.GetDirectoryName(source)!);
        await File.WriteAllTextAsync(source, idl);

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            var stagingOptions = options with
            {
                RepositoryRoot = repository,
                RtiVersion = version
            };

            var workspace = new MatrixWorkspace(stagingOptions);

            Directory.CreateDirectory(Path.GetDirectoryName(workspace.OracleManifest)!);
            await File.WriteAllTextAsync(workspace.OracleManifest, """{ "cases": {} }""");

            var paths = workspace.ForCase("adapted");

            Directory.CreateDirectory(paths.Native);

            var oracles = new WireOracleCatalog(workspace.OracleManifest);
            var sources = new NativePeerSources(stagingOptions, workspace, catalog, oracles);
            var descriptor = new CaseDescriptor("adapted", "Example.Type", "Example::Type", metadata.Idl);

            // Act: a missing oracle stops before native builds; IDL staging still consumes the cached metadata.
            var captured = sources.Stage(descriptor, paths);
            var actual = await File.ReadAllTextAsync(Path.Combine(paths.Native, "idl.idl"));

            // Assert
            var expected = idl;
            if (version == "7.3.1")
            {
                expected = idl
                    .Replace("case '\\0':", "case 0:", StringComparison.Ordinal)
                    .Replace("case '~':", "case 126:", StringComparison.Ordinal);
            }

            Check(!captured && actual == expected, "Native staging must apply only the selected profile's numeric-label adaptation without rereading the manifest.");
        }
    }

    private static void CheckRejected<TException>(Action action, string message) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
