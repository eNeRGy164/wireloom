namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Verifies scenario policy, deterministic shard assignments, and retained planning artifacts.</summary>
internal static class ScenarioPlanningVerification
{
    /// <summary>Exercises both complete profiles and focused planning edge cases without RTI processes.</summary>
    internal static void Run(MatrixOptions options)
    {
        var root = Path.Combine(options.ScratchDirectory, "planning-verification");
        VerifyPairingPolicy(root);
        VerifyShardTiesAndInvalidInputs(root);
        foreach (var (version, expectedCases, expectedScenarios) in new[] { ("7.3.1", 90, 1560), ("7.7.0", 91, 1555) })
        {
            VerifyCompleteProfile(options, root, version, expectedCases, expectedScenarios);
            Console.WriteLine($"Scenario planning {version}: {expectedCases} cases, {expectedScenarios} exchanges across 8 shards; complete and deterministic.");
        }

        VerifyCompileOnly(options, root);
    }

    private static void VerifyCompleteProfile(MatrixOptions options, string root, string version, int expectedCases, int expectedScenarios)
    {
        // Arrange
        var allScenarios = new HashSet<PlannedScenario>();
        var allCaseIds = new HashSet<string>(StringComparer.Ordinal);
        string? commonPlan = null;
        var originalOutput = Console.Out;
        using var capturedOutput = new StringWriter();
        try
        {
            Console.SetOut(capturedOutput);
            for (var index = 0; index < 8; index++)
            {
                var shardOptions = options with
                {
                    RtiVersion = version,
                    CompileOnly = false,
                    CaseFilter = null,
                    ShardIndex = index,
                    ShardCount = 8,
                    ScratchDirectory = Path.Combine(root, version, $"scratch-{index}"),
                    OutputDirectory = Path.Combine(root, version, $"shard-{index}"),
                };
                var workspace = new MatrixWorkspace(shardOptions);
                workspace.Initialize();

                // Act
                var plan = new MatrixPlanner(shardOptions, workspace, new WorkflowLog()).Create();

                // Assert
                Check(plan.Exchanges is not null, "A wire run must return its typed scenario plan.");
                var exchanges = plan.Exchanges!;
                Check(exchanges.Cases.Count == expectedCases && exchanges.TotalScenarioCount == expectedScenarios, "A full profile must retain its complete expected evidence.");
                Check(plan.ExpectedScenarioCount == exchanges.Scenarios.Count, "Expected shard count must come from the in-memory plan.");
                Check(plan.CaseIds.All(allCaseIds.Add), "A whole case must belong to exactly one shard.");
                Check(exchanges.Scenarios.All(allScenarios.Add), "An exchange must belong to exactly one shard.");
                Check(File.ReadAllLines(workspace.Scenarios).Length == plan.ExpectedScenarioCount, "The worker artifact must contain the selected scenario count.");
                Check(workspace.Scenarios == workspace.Output("scenarios.tsv"), "Execution and reporting must share the retained scenario file.");
                Check(!File.Exists(Path.Combine(shardOptions.ScratchDirectory, "scenarios.tsv")), "Planning must not create a duplicate scratch scenario file.");
                Check(!File.Exists(workspace.Output("selected-cases.txt")), "Selected case IDs must stay in memory.");
                var fullPlan = File.ReadAllText(workspace.Output("plan.json"));
                commonPlan ??= fullPlan;
                Check(commonPlan == fullPlan, "Every shard must retain an identical full case assignment.");
                Check(File.Exists(workspace.Output("shard.json")), "Shard identity must remain available to the merger.");
            }
        }
        finally
        {
            Console.SetOut(originalOutput);
        }

        Check(allCaseIds.Count == expectedCases && allScenarios.Count == expectedScenarios, "The shard union must cover the full profile exactly once.");
    }

    private static void VerifyPairingPolicy(string root)
    {
        // Arrange
        Directory.CreateDirectory(root);
        var manifest = Path.Combine(root, "manifest.json");
        var exclusions = Path.Combine(root, "exclusions.json");
        File.WriteAllText(manifest, """
            { "positiveCases": [
              { "id": "01-primitives", "wireMatrix": {
                "excludedPeersByRtiVersion": { "7.3.1": { "RTI C#": "Test profile exclusion" } }
              } },
              { "id": "09-optional-aggregate-member" },
              { "id": "03-enums-aliases", "wireMatrix": {
                "fixturePairings": { "enum-alternate": [ "RTI C++|Wireloom C#" ] }
              } },
              { "id": "09-must-understand" }
            ] }
            """);
        File.WriteAllText(exclusions, """
            {
              "notImplementedCases": [ { "id": "09-optional-aggregate-member" } ],
              "notApplicableCases": [ { "id": "09-must-understand" } ]
            }
            """);
        const string filter = "01-primitives,09-optional-aggregate-member,03-enums-aliases";

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            // Act
            var plan = ScenarioPlanBuilder.Create(manifest, exclusions, version, filter, shardIndex: 1, shardCount: 2);
            var scenarios = plan.Cases.SelectMany(item => item.Scenarios).ToArray();
            var control = scenarios.Where(item => item.CaseId == "09-optional-aggregate-member").ToArray();
            var alternate = scenarios.Single(item => item is { CaseId: "03-enums-aliases", Fixture: "enum-alternate" });

            // Assert
            Check(plan.TotalScenarioCount == (version == "7.3.1" ? 22 : 27), "Profile peer exclusions and fixture-specific pairings must affect the plan weight.");
            Check(plan.CaseIds.SequenceEqual(["01-primitives", "09-optional-aggregate-member"], StringComparer.Ordinal), "Shard case order must follow selection order after weighted assignment.");
            Check(control.Length == 8 && control.All(item => item is not { Writer: "Wireloom C#" } and not { Reader: "Wireloom C#" }), "RTI-only controls must exclude every Wireloom endpoint.");
            Check(alternate is { Writer: "RTI C++", Reader: "Wireloom C#" }, "A fixture pairing override must remain directed.");
            Check(scenarios.All(item => item.CaseId != "09-must-understand"), "Inapplicable cases must not enter the plan.");
        }
    }

    private static void VerifyShardTiesAndInvalidInputs(string root)
    {
        // Arrange
        var manifest = Path.Combine(root, "ties.json");
        var exclusions = Path.Combine(root, "no-exclusions.json");
        File.WriteAllText(manifest, """
            { "positiveCases": [
              { "id": "z", "wireMatrix": { "peers": [ "RTI C++" ] } },
              { "id": "a", "wireMatrix": { "peers": [ "RTI C++" ] } },
              { "id": "m", "wireMatrix": { "peers": [ "RTI C++" ] } }
            ] }
            """);
        File.WriteAllText(exclusions, """{ "notImplementedCases": [] }""");

        // Act
        var plan = ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", caseFilter: null, shardIndex: 0, shardCount: 2);

        // Assert
        Check(plan.CaseIds.SequenceEqual(["z", "a"], StringComparer.Ordinal), "Equal-weight assignment must break ties by ordinal case ID, then shard index, while retaining case order.");
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", "z,z", 0, 1));
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", "unknown", 0, 1));
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", " ", 0, 1));
        ExpectFailure<ArgumentOutOfRangeException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", null, 0, 4));
        ExpectFailure<ArgumentOutOfRangeException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", null, 2, 2));

        // Arrange
        File.WriteAllText(manifest, """
            { "positiveCases": [ { "id": "01-primitives", "wireMatrix": {
              "fixturePairings": { "default": [ "RTI C++|unknown" ] }
            } } ] }
            """);

        // Act / Assert
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", null, 0, 1));
    }

    private static void VerifyCompileOnly(MatrixOptions options, string root)
    {
        // Arrange
        var compileOptions = options with
        {
            CompileOnly = true,
            CaseFilter = "14-enum-constant,01-primitives",
            ShardIndex = 0,
            ShardCount = 1,
            ScratchDirectory = Path.Combine(root, "compile-scratch"),
            OutputDirectory = Path.Combine(root, "compile-output"),
        };
        var workspace = new MatrixWorkspace(compileOptions);
        Directory.CreateDirectory(compileOptions.OutputDirectory);
        foreach (var name in new[] { "plan.json", "shard.json", "selected-cases.txt" })
        {
            File.WriteAllText(workspace.Output(name), "stale");
        }

        workspace.Initialize();

        // Act
        var plan = new MatrixPlanner(compileOptions, workspace, new WorkflowLog()).Create();

        // Assert
        Check(plan.Exchanges is null && plan.ExpectedScenarioCount == 0, "Compile-only mode must not expand exchanges.");
        Check(plan.CaseIds.SequenceEqual(["14-enum-constant", "01-primitives"], StringComparer.Ordinal), "Compile-only mode must retain ordered selected cases in memory.");
        Check(!File.Exists(workspace.Output("plan.json")) && !File.Exists(workspace.Output("shard.json")) &&
            !File.Exists(workspace.Output("selected-cases.txt")), "Initialization must discard stale planning artifacts.");
    }

    private static void ExpectFailure<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException($"Invalid planning input must throw {typeof(TException).Name}.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
