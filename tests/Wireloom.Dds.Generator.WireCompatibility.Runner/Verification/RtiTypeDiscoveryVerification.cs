namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks RTI root selection and handled discovery failure without compiling or launching RTI peers.</summary>
internal static class RtiTypeDiscoveryVerification
{
    /// <summary>Exercises root precedence, recursive source scanning, and the managed builder's unavailable-peer path.</summary>
    internal static async Task RunAsync(MatrixOptions options, ProcessRunner processes)
    {
        var root = Path.Combine(options.ScratchDirectory, "rti-discovery-verification");

        VerifySelection(root);

        await VerifyHandledFailureAsync(options, processes, root);

        Console.WriteLine("RTI type discovery passed: Wireloom/case/default precedence, ordinal ties, nested sources, duplicates, missing support, and handled peer failure without a registry handoff.");
    }

    private static void VerifySelection(string root)
    {
        // Arrange
        var directory = Path.Combine(root, "selection");

        WriteTypes(directory, ["Zeta.Sample", "Alpha.Sample", "Oracle.Composed", "Oracle.Zebra", "Oracle.Record", "Alpha.Sample"]);

        File.WriteAllText(Path.Combine(directory, "ignored.txt"), "class IgnoredSupport : Rti.Dds.Topics.TypeSupport<global::A.Sample> {}");
        File.WriteAllText(Path.Combine(directory, "unrelated.cs"),
            """
            class LocalSupport : TypeSupport<global::A.Sample> {}
            class UnqualifiedSupport : Rti.Dds.Topics.TypeSupport<A.Sample> {}
            """);

        // Act / Assert
        Check(RtiTypeDiscovery.Discover("example", "-", directory) == "Alpha.Sample", "Default Sample roots must use ordinal qualified-name tie breaking.");
        Check(RtiTypeDiscovery.Discover("03-enums-aliases", "-", directory) == "Oracle.Record", "The case-specific root must precede defaults.");
        Check(RtiTypeDiscovery.Discover("03-enums-aliases", "Wireloom.Sample", directory) == "Alpha.Sample", "The Wireloom root name must precede case-specific preferences.");
        Check(RtiTypeDiscovery.Discover("example", "Wireloom.Zebra", directory) == "Oracle.Zebra", "Wireloom namespace differences must not affect basename matching.");

        // Arrange
        var preferredRoots = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["03-enums-aliases"] = "Record",
            ["09-optional-recursive-array"] = "Node",
            ["09-optional-aggregate-member"] = "Holder",
            ["07-union-aliases"] = "Holder",
            ["06-valuetypes"] = "Holder",
            ["04-unbounded-string-aliases"] = "StringAliasesSample",
            ["05-unbounded-sequence-aliases"] = "SequenceAliasesSample",
            ["04-string-alias-chains"] = "StringAliasChainsSample",
            ["07-union-string-sequence-aliases"] = "StringSequenceUnionsSample",
            ["07-union-char-numeric"] = "Choice",
            ["07-union-char-numeric-255"] = "Choice",
            ["07-union-string-sequence-bounded-direct"] = "DirectBoundedStringSequences",
            ["08-key-inherited"] = "Derived",
            ["08-key-nested"] = "Outer",
        };

        var special = Path.Combine(root, "special");

        WriteTypes(special, preferredRoots.Values.Distinct(StringComparer.Ordinal).Select(name => "Oracle." + name)
            .Concat(["Oracle.Sample", "Oracle.Composed"]).ToArray());

        foreach (var (caseId, name) in preferredRoots)
        {
            // Act / Assert
            Check(RtiTypeDiscovery.Discover(caseId, "-", special) == "Oracle." + name, $"The RTI-only root preference changed for {caseId}.");
        }

        // Arrange / Act / Assert
        var composed = Path.Combine(root, "composed");
        WriteTypes(composed, ["Oracle.Zebra", "Oracle.Composed"]);

        Check(RtiTypeDiscovery.Discover("example", "missing", composed) == "Oracle.Composed", "Composed must precede the ordinal fallback when Sample is absent.");

        var fallback = Path.Combine(root, "fallback");
        WriteTypes(fallback, ["Zeta.Beta", "Alpha.Zeta"]);

        Check(RtiTypeDiscovery.Discover("03-enums-aliases", "missing", fallback) == "Alpha.Zeta", "An absent preferred root must retain the ordinal fallback.");

        var empty = Path.Combine(root, "empty");
        Directory.CreateDirectory(empty);

        CheckRejected<InvalidOperationException>(() => RtiTypeDiscovery.Discover("example", "-", empty), "An empty capture must fail discovery.");
        CheckRejected<DirectoryNotFoundException>(() => RtiTypeDiscovery.Discover("example", "-", Path.Combine(root, "missing")), "A missing capture directory must fail discovery.");
    }

    private static async Task VerifyHandledFailureAsync(MatrixOptions options, ProcessRunner processes, string root)
    {
        // Arrange
        var repository = Path.Combine(root, "repository");
        var discoveryOptions = options with { RepositoryRoot = repository, RtiVersion = "7.7.0" };
        var workspace = new MatrixWorkspace(discoveryOptions);
        var oracle = workspace.OracleDirectory("csharp/features/empty-capture");

        Directory.CreateDirectory(oracle);
        Directory.CreateDirectory(workspace.Logs);

        await File.WriteAllTextAsync(Path.Combine(oracle, "empty.cs"), "// Capture has no TypeSupport.");
        await File.WriteAllTextAsync(workspace.OracleManifest,
            """
            { "cases": { "empty-capture": { "rtiCSharp": {
              "mode": "captured", "sourceDirectory": "csharp/features/empty-capture"
            } } } }
            """);

        var paths = workspace.ForCase("empty-capture");
        var oracles = new WireOracleCatalog(workspace.OracleManifest);
        var builder = new ManagedPeerBuilder(discoveryOptions, workspace, new CorpusCaseCatalog(workspace.Manifest), oracles, processes, new WorkflowLog());
        var descriptor = new CaseDescriptor("empty-capture", "Wireloom.Sample", "::Oracle::Sample", "unused.idl");
        var originalError = Console.Error;

        await using var diagnostics = new StringWriter();

        try
        {
            Console.SetError(diagnostics);

            // Act
            var peer = await builder.BuildRtiAsync(descriptor, paths);

            // Assert
            Check(peer is { Available: false, TypeName: "-" }, "Discovery failure must remain a handled unavailable RTI peer.");
            Check(diagnostics.ToString().Contains("No RTI C# TypeSupport was generated for empty-capture.", StringComparison.Ordinal), "The failed discovery reason must remain visible.");
            Check(!File.Exists(Path.Combine(paths.Directory, "rti-oracle.json")), "In-process discovery must not create an intermediate registry.");
            Check(!File.Exists(Path.Combine(paths.Directory, "rti-csharp-build.log")), "A failed discovery must stop before publication.");
        }
        finally
        {
            Console.SetError(originalError);
        }
    }

    private static void WriteTypes(string directory, IReadOnlyList<string> names)
    {
        var nested = Path.Combine(directory, "nested");

        Directory.CreateDirectory(nested);

        File.WriteAllText(Path.Combine(nested, "types.cs"), string.Join('\n', names.Select((name, index) =>
            $"class Topic{index}Support\n : Rti.Dds.Topics.TypeSupport<\n global::{name} > {{}}")));
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
