using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Verifies scenario policy, deterministic shard assignments, and retained planning artifacts.</summary>
internal static class ScenarioPlanningVerification
{
    /// <summary>Exercises all complete profiles and focused planning edge cases without RTI processes.</summary>
    internal static void Run(MatrixOptions options)
    {
        var root = Path.Combine(options.ScratchDirectory, "planning-verification");
        VerifyPairingPolicy(root);
        VerifyShardTiesAndInvalidInputs(root);
        VerifyExplicitWireloomCppSelection(options, root);

        var profileCounts = new List<(string Version, int Cases, int Scenarios)>();
        var expectedCounts = new[] { (Version: "6.1.2", Cases: 85, Scenarios: 2521), (Version: "7.3.1", Cases: 93, Scenarios: 2829), (Version: "7.7.0", Cases: 94, Scenarios: 2823) };
        foreach (var (version, expectedCases, _) in expectedCounts)
        {
            var (cases, scenarios) = VerifyCompleteProfile(options, root, version, expectedCases);
            profileCounts.Add((version, cases, scenarios));
            Console.WriteLine($"Scenario planning {version}: {cases} cases, {scenarios} exchanges across 8 shards; complete and deterministic.");
        }

        var expectedByVersion = expectedCounts.ToDictionary(item => item.Version, item => (item.Cases, item.Scenarios), StringComparer.Ordinal);
        var mismatches = profileCounts.Where(actual =>
        {
            var expected = expectedByVersion[actual.Version];
            return actual.Cases != expected.Cases || actual.Scenarios != expected.Scenarios;
        }).Select(actual =>
        {
            var expected = expectedByVersion[actual.Version];
            return $"RTI {actual.Version}: expected {expected.Cases} cases/{expected.Scenarios} exchanges, found {actual.Cases} cases/{actual.Scenarios} exchanges";
        }).ToArray();
        Check(mismatches.Length == 0, string.Join("; ", mismatches));

        VerifyCompileOnly(options, root);
    }

    private static (int Cases, int Scenarios) VerifyCompleteProfile(MatrixOptions options, string root, string version, int expectedCases)
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
                    NativePeerScopeCaseFilter = version == "6.1.2" ? "09-optional-collections" : null,
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
                VerifyFixtureCoverage(exchanges, workspace.Manifest, version);
                Check(exchanges.Cases.Count == expectedCases,
                    $"RTI {version}: expected {expectedCases} cases; found {exchanges.Cases.Count}.");
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

        Check(allCaseIds.Count == expectedCases, "The shard union must cover every profile case exactly once.");
        return (allCaseIds.Count, allScenarios.Count);
    }

    private static void VerifyFixtureCoverage(ScenarioPlan plan, string manifestPath, string version)
    {
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        var definitions = manifest["positiveCases"]!.AsArray()
            .ToDictionary(item => (string)item!["id"]!, item => item!, StringComparer.Ordinal);
        var actualByFixture = plan.Cases.SelectMany(item => item.Scenarios)
            .GroupBy(item => (item.CaseId, item.Fixture))
            .ToDictionary(group => group.Key, group => group.ToHashSet());

        foreach (var plannedCase in plan.Cases)
        {
            foreach (var fixture in WireCompatibilityCaseCatalog.FixturesFor(plannedCase.CaseId, version))
            {
                HashSet<PlannedScenario> expected;
                if (plan.NativePeerScopeCases.Contains(plannedCase.CaseId))
                {
                    expected =
                    [
                        new PlannedScenario(plannedCase.CaseId, fixture, PeerNames.RtiCpp, PeerNames.WireloomCpp, version),
                        new PlannedScenario(plannedCase.CaseId, fixture, PeerNames.WireloomCpp, PeerNames.RtiCpp, version),
                    ];
                }
                else
                {
                    var definition = definitions[plannedCase.CaseId];
                    expected = PeerPairingPolicy.GetPairings(definition, fixture, version, includeWireloomCpp: true)
                        .Where(pairing => !plan.RtiOnlyCases.Contains(plannedCase.CaseId)
                            || (pairing.Writer is not (PeerNames.WireloomCSharp or PeerNames.WireloomCpp)
                                && pairing.Reader is not (PeerNames.WireloomCSharp or PeerNames.WireloomCpp)))
                        .Select(pairing => new PlannedScenario(plannedCase.CaseId, fixture, pairing.Writer, pairing.Reader, version))
                        .ToHashSet();
                }

                var actual = actualByFixture.GetValueOrDefault((plannedCase.CaseId, fixture), []);
                Check(actual.SetEquals(expected),
                    $"RTI {version} {plannedCase.CaseId}/{fixture}: expected {expected.Count} directed exchanges from available peers, found {actual.Count}; missing [{string.Join(", ", expected.Except(actual).Select(item => $"{item.Writer}->{item.Reader}"))}], unexpected [{string.Join(", ", actual.Except(expected).Select(item => $"{item.Writer}->{item.Reader}"))}].");
            }
        }
    }

    private static void VerifyPairingPolicy(string root)
    {
        // Arrange
        Directory.CreateDirectory(root);

        var manifest = Path.Combine(root, "manifest.json");
        var exclusions = Path.Combine(root, "exclusions.json");

        File.WriteAllText(manifest,
            """
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

        File.WriteAllText(exclusions,
            """
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
            var primitive = scenarios.Where(item => item.CaseId == "01-primitives").ToArray();
            var control = scenarios.Where(item => item.CaseId == "09-optional-aggregate-member").ToArray();
            var enumScenarios = scenarios.Where(item => item.CaseId == "03-enums-aliases").ToArray();
            var alternate = scenarios.Where(item => item is { CaseId: "03-enums-aliases", Fixture: "enum-alternate" }).ToArray();

            // Assert
            Check(plan.CaseIds.SequenceEqual(["01-primitives", "09-optional-aggregate-member"], StringComparer.Ordinal), "Shard case order must follow selection order after weighted assignment.");
            Check(primitive.Length == (version == "7.3.1" ? 9 : 16), "The primitive fixture must include every available directed peer combination for its exact profile.");
            Check(control.Length == 8 && control.All(item =>
                    item.Writer is not (PeerNames.WireloomCSharp or PeerNames.WireloomCpp)
                    && item.Reader is not (PeerNames.WireloomCSharp or PeerNames.WireloomCpp)),
                "RTI-only controls must exclude every Wireloom endpoint.");
            Check(enumScenarios.Length == 17 && alternate.Length == 1
                  && alternate.Contains(new PlannedScenario("03-enums-aliases", "enum-alternate", "RTI C++", "Wireloom C#", version))
                  && alternate.Select(item => (item.Writer, item.Reader)).Distinct().Count() == 1,
                "An exact fixture-pairing exception must remain one-way when Wireloom C++ is eligible.");
            Check(scenarios.All(item => item.CaseId != "09-must-understand"), "Inapplicable cases must not enter the plan.");
        }
    }

    private static void VerifyShardTiesAndInvalidInputs(string root)
    {
        // Arrange
        var manifest = Path.Combine(root, "ties.json");
        var exclusions = Path.Combine(root, "no-exclusions.json");

        File.WriteAllText(manifest,
            """
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
        File.WriteAllText(manifest,
            """
            { "positiveCases": [ { "id": "01-primitives", "wireMatrix": {
              "fixturePairings": { "default": [ "RTI C++|unknown" ] }
            } } ] }
            """);

        // Act / Assert
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", null, 0, 1));

        // Assert: a sparse live-C++ scope no longer gates ordinary matrix planning.
        var missingScopeRoot = Path.Combine(root, "missing-cpp-scope");
        Directory.CreateDirectory(missingScopeRoot);
        var missingScopeManifest = Path.Combine(missingScopeRoot, "manifest.json");
        var missingScopeExclusions = Path.Combine(missingScopeRoot, "exclusions.json");
        File.WriteAllText(missingScopeManifest, """{ "positiveCases": [ { "id": "case" } ] }""");
        File.WriteAllText(missingScopeExclusions, """{ "notImplementedCases": [] }""");
        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            var fullMatrix = ScenarioPlanBuilder.Create(missingScopeManifest, missingScopeExclusions,
                version, null, 0, 1);
            Check(fullMatrix.WireloomCppCases.SetEquals(["case"]) && fullMatrix.Scenarios.Count == 16,
                $"The {version} full matrix must not depend on a sparse live-C++ qualification file.");
        }
    }

    private static void VerifyExplicitWireloomCppSelection(MatrixOptions options, string root)
    {
        // Arrange
        var workspaceOptions = options with
        {
            RtiVersion = "6.1.2",
            CompileOnly = false,
            CaseFilter = "01-boundaries",
            WireloomCppCaseFilter = null,
            NativePeerScopeCaseFilter = null,
            ShardIndex = 0,
            ShardCount = 1,
            ScratchDirectory = Path.Combine(root, "explicit-cpp-scratch"),
            OutputDirectory = Path.Combine(root, "explicit-cpp-output"),
        };
        var workspace = new MatrixWorkspace(workspaceOptions);
        workspace.Initialize();

        // Act
        var defaultPlan = new MatrixPlanner(workspaceOptions, workspace, new WorkflowLog()).Create();
        var explicitOptions = workspaceOptions with { WireloomCppCaseFilter = "01-boundaries" };
        var explicitWorkspace = new MatrixWorkspace(explicitOptions);
        explicitWorkspace.Initialize();
        var explicitPlan = new MatrixPlanner(explicitOptions, explicitWorkspace, new WorkflowLog()).Create();

        // Assert
        Check(defaultPlan.Exchanges!.TotalScenarioCount == 16 &&
              defaultPlan.RequiresPeer("01-boundaries", PeerNames.WireloomCpp),
            "The default full matrix must include Wireloom C++ in RTI 6.1.2.");
        Check(explicitPlan.Exchanges!.RtiVersion == "6.1.2" && explicitPlan.Exchanges.WireloomCppCases.SetEquals(["01-boundaries"]) &&
              explicitPlan.RequiresPeer("01-boundaries", PeerNames.WireloomCpp) &&
              explicitPlan.Exchanges.TotalScenarioCount == 16,
            "An explicitly selected older-profile peer scope must add directed pairings under that exact profile.");

        var modernOptions = workspaceOptions with
        {
            RtiVersion = "7.3.1",
            CaseFilter = "09-optional-collections",
            WireloomCppCaseFilter = "09-optional-collections",
            ScratchDirectory = Path.Combine(root, "modern-explicit-cpp-scratch"),
            OutputDirectory = Path.Combine(root, "modern-explicit-cpp-output"),
        };
        var modernWorkspace = new MatrixWorkspace(modernOptions);
        modernWorkspace.Initialize();
        var modernPlan = new MatrixPlanner(modernOptions, modernWorkspace, new WorkflowLog()).Create();
        Check(modernPlan.Exchanges!.CaseIds.SequenceEqual(["09-optional-collections"], StringComparer.Ordinal)
              && modernPlan.Exchanges.NativePeerScopeCases.Count == 0
              && modernPlan.RequiresPeer("09-optional-collections", PeerNames.WireloomCpp),
            "The shared optional-collections corpus case must retain normal profile eligibility while adding Wireloom C++ peer directions.");

        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(workspace.Manifest, workspace.Exclusions,
            "6.1.2", "01-boundaries", 0, 1, wireloomCppCaseFilter: "14-enum-constant"));
    }

    private static void VerifyCompileOnly(MatrixOptions options, string root)
    {
        // Arrange
        var compileOptions = options with
        {
            CompileOnly = true,
            CaseFilter = "14-enum-constant,01-boundaries",
            RtiVersion = "6.1.2",
            WireloomCppCaseFilter = "14-enum-constant",
            NativePeerScopeCaseFilter = null,
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
        Check(plan.CaseIds.SequenceEqual(["14-enum-constant", "01-boundaries"], StringComparer.Ordinal), "Compile-only mode must retain ordered selected cases in memory.");
        Check(plan.RequiresPeer("14-enum-constant", PeerNames.WireloomCpp) &&
              !plan.RequiresPeer("01-boundaries", PeerNames.WireloomCpp),
            "Compile-only mode must compile only explicitly selected Wireloom C++ cases.");
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
