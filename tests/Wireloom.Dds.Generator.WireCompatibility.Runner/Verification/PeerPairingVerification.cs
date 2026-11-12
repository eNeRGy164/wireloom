using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks ordered profile/fixture pairings and peer requirements derived from the typed plan.</summary>
internal static class PeerPairingVerification
{
    /// <summary>Verifies pairing policy and in-memory peer selection without building or starting peers.</summary>
    internal static void Run(MatrixOptions options)
    {
        VerifyDefaultsAndProfileRules();
        VerifyInvalidFixtureRules();
        VerifyPlanRequirements(Path.Combine(options.ScratchDirectory, "pairing-verification"));
        VerifyCorpusRequirements(options);
        Console.WriteLine("Peer pairings passed: ordering, profile exclusions, directed fixture overrides, invalid rules, and in-memory peer requirements.");
    }

    private static void VerifyDefaultsAndProfileRules()
    {
        // Arrange
        var defaults = JsonNode.Parse("{}");
        var configured = JsonNode.Parse(
            """
            {
                "wireMatrix": {
                    "peers": [ "RTI C++", "Wireloom C#", "RTI C#" ],
                    "excludedPeersByRtiVersion": { "7.3.1": { "RTI C#": "Test exclusion" } },
                    "fixturePairings": { "one-way": [ "RTI C++|Wireloom C#" ] }
                }
            }
            """);

        // Act
        var all = PeerPairingPolicy.GetPairings(defaults!);
        var older = PeerPairingPolicy.GetPairings(configured!, version: "7.3.1");
        var directed = PeerPairingPolicy.GetPairings(configured!, fixture: "one-way", version: "7.7.0");
        var cpp = PeerPairingPolicy.GetPairings(configured!, fixture: "one-way", version: "7.7.0", includeWireloomCpp: true);
        var fixtureScopedCpp = JsonNode.Parse(
            """
            {
                "wireMatrix": {
                    "excludedPeersByFixture": { "recursive": { "Wireloom C++": "Runtime bridge does not support this root." } }
                }
            }
            """);
        var recursive = PeerPairingPolicy.GetPairings(fixtureScopedCpp!, fixture: "recursive", version: "7.7.0", includeWireloomCpp: true);
        var profileScopedCpp = JsonNode.Parse(
            """
            {
                "wireMatrix": {
                    "excludedPeersByFixtureByRtiVersion": {
                        "7.7.0": {
                            "recursive": { "Wireloom C++": "Recursive optional-array TypeSupport is not qualified." }
                        }
                    }
                }
            }
            """);
        var recursiveProfile = PeerPairingPolicy.GetPairings(
            profileScopedCpp!, fixture: "recursive", version: "7.7.0", includeWireloomCpp: true);
        var recursiveFallback = PeerPairingPolicy.GetPairings(
            profileScopedCpp!, fixture: "recursive", version: "7.3.1", includeWireloomCpp: true);
        var versionedFixture = JsonNode.Parse(
            """
            {
                "wireMatrix": {
                    "fixturePairings": { "minimum": [ "RTI C++|RTI C++" ] },
                    "fixturePairingsByRtiVersion": {
                        "7.3.1": { "minimum": [ "Wireloom C#|RTI C++", "RTI C++|Wireloom C++" ] }
                    }
                }
            }
            """);
        var versioned = PeerPairingPolicy.GetPairings(versionedFixture!, fixture: "minimum", version: "7.3.1", includeWireloomCpp: true);
        var defaultVersioned = PeerPairingPolicy.GetPairings(versionedFixture!, fixture: "minimum", version: "7.7.0", includeWireloomCpp: true);

        // Assert
        Check(all.Select(Text).SequenceEqual([
            "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++",
            "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++",
            "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++"
        ], StringComparer.Ordinal), "Default pairings must retain writer-major order.");
        Check(older.Select(Text).SequenceEqual([
            "RTI C++|RTI C++", "RTI C++|Wireloom C#", "Wireloom C#|RTI C++", "Wireloom C#|Wireloom C#"
        ], StringComparer.Ordinal), "Profile exclusions must preserve the configured order of remaining peers.");
        Check(directed.SequenceEqual([new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.WireloomCSharp)]), "Fixture overrides must preserve endpoint direction.");
        Check(!directed[0].Involves(PeerNames.RtiCSharp), "An unused peer must not appear in a directed override.");
        Check(cpp.SequenceEqual(directed),
            "An explicit fixture pairing is an exact exception to the default Wireloom C++ full matrix.");
        Check(recursive.SequenceEqual([
            new DirectedPeerPair(PeerNames.WireloomCSharp, PeerNames.WireloomCSharp),
            new DirectedPeerPair(PeerNames.WireloomCSharp, PeerNames.RtiCSharp),
            new DirectedPeerPair(PeerNames.WireloomCSharp, PeerNames.RtiCpp),
            new DirectedPeerPair(PeerNames.RtiCSharp, PeerNames.WireloomCSharp),
            new DirectedPeerPair(PeerNames.RtiCSharp, PeerNames.RtiCSharp),
            new DirectedPeerPair(PeerNames.RtiCSharp, PeerNames.RtiCpp),
            new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.WireloomCSharp),
            new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.RtiCSharp),
            new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.RtiCpp)
        ]), "Fixture-scoped Wireloom C++ exclusions must retain the full matrix among the three remaining peers.");
        Check(recursiveProfile.Length == 9
              && recursiveProfile.All(pair => !pair.Involves(PeerNames.WireloomCpp)),
            "A profile-specific fixture exclusion must remove only the affected peer from that profile's Cartesian matrix.");
        Check(recursiveFallback.Length == 16
              && recursiveFallback.Any(pair => pair.Involves(PeerNames.WireloomCpp)),
            "A profile-specific fixture exclusion must not alter other profiles.");
        Check(versioned.SequenceEqual([
            new DirectedPeerPair(PeerNames.WireloomCSharp, PeerNames.RtiCpp),
            new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.WireloomCpp)
        ]), "An exact profile fixture pairing must override the shared fixture list and remain exact for Wireloom C++.");
        Check(defaultVersioned.SequenceEqual([new DirectedPeerPair(PeerNames.RtiCpp, PeerNames.RtiCpp)]),
            "A profile fixture pairing must not change other RTI profiles.");
        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            var matrix = PeerPairingPolicy.GetPairings(defaults!, version: version, includeWireloomCpp: true);
            Check(matrix.Length == 16 && matrix.Select(Text).Distinct(StringComparer.Ordinal).Count() == 16,
                $"The {version} four-peer plan must include all 16 directed pairings, including Wireloom C++ self-compatibility.");
        }
        Check(PeerPairingPolicy.GetPeers(configured!).SequenceEqual([PeerNames.RtiCpp, PeerNames.WireloomCSharp, PeerNames.RtiCSharp], StringComparer.Ordinal), "Versionless queries must retain default profile behavior and must not observe mutations from earlier queries.");
        Check(PeerPairingPolicy.GetPairings(configured!, fixture: "unconfigured", version: "7.7.0").Length == 9, "An unconfigured fixture must use the Cartesian product.");
    }

    private static void VerifyInvalidFixtureRules()
    {
        // Arrange
        var definitions = new[]
        {
            """{ "wireMatrix": { "fixturePairings": { "default": [] } } }""",
            """{ "wireMatrix": { "fixturePairings": { "default": [ "RTI C++|Wireloom C#", "RTI C++|Wireloom C#" ] } } }""",
            """{ "wireMatrix": { "fixturePairings": { "default": [ "RTI C++|unknown" ] } } }""",
            """{ "wireMatrix": { "fixturePairings": { "default": [ "RTI C++|Wireloom C#|RTI C#" ] } } }""",
            """{ "wireMatrix": { "excludedPeersByRtiVersion": { "7.3.1": { "RTI C#": "Test exclusion" } }, "fixturePairings": { "default": [ "RTI C#|RTI C++" ] } } }""",
        };

        foreach (var definition in definitions)
        {
            // Act
            var rejected = false;
            try
            {
                PeerPairingPolicy.GetPairings(JsonNode.Parse(definition)!, fixture: "default", version: "7.3.1");
            }
            catch (InvalidDataException)
            {
                rejected = true;
            }

            // Assert
            Check(rejected, "Empty, duplicate, malformed, unavailable, or unknown fixture endpoints must be rejected.");
        }
    }

    private static void VerifyPlanRequirements(string directory)
    {
        // Arrange
        Directory.CreateDirectory(directory);

        var manifest = Path.Combine(directory, "manifest.json");
        var exclusions = Path.Combine(directory, "exclusions.json");

        File.WriteAllText(manifest,
            """
            {
                "positiveCases": [
                    { "id": "custom-rti-cpp-pairing", "wireMatrix": { "fixturePairings": { "default": [ "RTI C++|RTI C++" ] } } },
                    { "id": "03-enums-aliases", "wireMatrix": { "fixturePairings": { "enum-alternate": [ "RTI C++|RTI C++" ] } } },
                    { "id": "09-optional-aggregate-member" },
                    { "id": "profile", "wireMatrix": { "excludedPeersByRtiVersion": { "7.3.1": { "RTI C#": "Test exclusion" } } } }
                ]
            }
            """);
        File.WriteAllText(exclusions, """{ "notImplementedCases": [ { "id": "09-optional-aggregate-member" } ] }""");
        // Act
        var plans = new[] { "7.3.1", "7.7.0" }.Select(version =>
            Wrap(ScenarioPlanBuilder.Create(manifest, exclusions, version, caseFilter: null, shardIndex: 0, shardCount: 1))).ToArray();

        // Assert
        foreach (var plan in plans)
        {
            Check(!plan.RequiresPeer("custom-rti-cpp-pairing", PeerNames.RtiCSharp)
                  && plan.RequiresPeer("custom-rti-cpp-pairing", PeerNames.RtiCpp)
                  && !plan.RequiresPeer("custom-rti-cpp-pairing", PeerNames.WireloomCpp),
                "Peer requirements must follow the exact fixture exception, not merely the profile's available peers.");
            Check(plan.RequiresPeer("03-enums-aliases", PeerNames.RtiCSharp), "A peer used by any fixture must remain available even when another fixture omits it.");
            Check(plan.RequiresPeer("09-optional-aggregate-member", PeerNames.RtiCSharp) && !plan.RequiresPeer("09-optional-aggregate-member", PeerNames.WireloomCSharp), "RTI-only requirements must follow the filtered plan.");
            Check(plan.RequiresPeer("profile", PeerNames.RtiCSharp) == (plan.Exchanges!.RtiVersion == "7.7.0"), "Peer requirements must retain profile-specific exclusions.");
            Check(!plan.RequiresPeer("unknown", PeerNames.RtiCSharp), "An unselected case must not require a peer.");
        }

        var cppPairings = plans[1].Exchanges!.Cases.Single(item => item.CaseId == "03-enums-aliases").Scenarios
            .Where(item => item.Fixture == "default")
            .Select(item => $"{item.Writer}|{item.Reader}")
            .ToArray();
        Check(plans.All(plan => plan.RequiresPeer("03-enums-aliases", PeerNames.WireloomCpp)),
            "Normal fixtures must include Wireloom C++ in every profile even when another fixture has an exact override.");
        Check(cppPairings.Length == 16 && cppPairings.Distinct(StringComparer.Ordinal).Count() == 16,
            "An eligible fixture must schedule all 16 unique directed peer combinations.");

        var filteredPlan = Wrap(ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0",
            caseFilter: "custom-rti-cpp-pairing,03-enums-aliases", shardIndex: 0, shardCount: 1));
        Check(!filteredPlan.CaseIds.Contains("09-optional-aggregate-member", StringComparer.Ordinal)
              && !filteredPlan.RequiresPeer("09-optional-aggregate-member", PeerNames.RtiCSharp),
            "Cases outside the explicit filter must not affect peer preparation.");
        File.Delete(manifest);
        Check(!MatrixPlan.Empty.RequiresPeer("custom-rti-cpp-pairing", PeerNames.RtiCpp), "An empty or compile-only plan must not require runtime peers.");
    }

    private static void VerifyCorpusRequirements(MatrixOptions options)
    {
        // Arrange
        var workspace = new MatrixWorkspace(options);
        var expectedWithoutRtiCSharp = new[]
        {
            "Wireloom C#|Wireloom C#",
            "Wireloom C#|RTI C++",
            "Wireloom C#|Wireloom C++",
            "RTI C++|Wireloom C#",
            "RTI C++|RTI C++",
            "RTI C++|Wireloom C++",
            "Wireloom C++|Wireloom C#",
            "Wireloom C++|RTI C++",
            "Wireloom C++|Wireloom C++",
        };
        var expectedWithoutRtiCSharpAndWireloomCpp = new[]
        {
            "Wireloom C#|Wireloom C#",
            "Wireloom C#|RTI C++",
            "RTI C++|Wireloom C#",
            "RTI C++|RTI C++",
        };

        var enumProfilePlan = ScenarioPlanBuilder.Create(workspace.Manifest,
            Path.Combine(workspace.TestRoot, "wire-case-exclusions-6.1.2.json"), "6.1.2",
            "03-enum-values-explicit", shardIndex: 0, shardCount: 1);
        CheckCasePairings(enumProfilePlan, "03-enum-values-explicit", ["default"], expectedWithoutRtiCSharp,
            "The RTI 6.1.2 enum-default C# exclusion must preserve all four directed pairings among remaining peers.");
        var wcharProfilePlan = ScenarioPlanBuilder.Create(workspace.Manifest,
            Path.Combine(workspace.TestRoot, "wire-case-exclusions-6.1.2.json"), "6.1.2",
            "07-union-wchar-label", shardIndex: 0, shardCount: 1);
        CheckCasePairings(wcharProfilePlan, "07-union-wchar-label", ["union-number", "union-text", "union-default"],
            expectedWithoutRtiCSharpAndWireloomCpp, "The 6.1.2 wchar discriminator exclusions must preserve all pairings among supported peers for each fixture.");
        Check(!Wrap(wcharProfilePlan).RequiresPeer("07-union-wchar-label", PeerNames.WireloomCpp),
            "The wchar discriminator fixture must not build the unsupported Wireloom C++ peer.");

        var cppOnlyArrayPairings = new[]
        {
            "RTI C++|RTI C++",
            "RTI C++|Wireloom C++",
            "Wireloom C++|RTI C++",
            "Wireloom C++|Wireloom C++",
        };
        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            var arrayPlan = ScenarioPlanBuilder.Create(workspace.Manifest,
                Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json"), version,
                "05-array-of-sequences", shardIndex: 0, shardCount: 1);
            CheckCasePairings(arrayPlan, "05-array-of-sequences", ["default"], cppOnlyArrayPairings,
                $"The {version} direct arrays-of-sequences exception must retain all four directed C++ pairings.");
            Check(arrayPlan.Scenarios.Count == 4
                  && arrayPlan.WireloomCppCases.Contains("05-array-of-sequences", StringComparer.Ordinal),
                $"The {version} direct arrays-of-sequences fixture total must be exactly four exchanges, including Wireloom C++.");

            var matrixPlan = Wrap(arrayPlan);
            Check(!matrixPlan.RequiresPeer("05-array-of-sequences", PeerNames.WireloomCSharp)
                  && !matrixPlan.RequiresPeer("05-array-of-sequences", PeerNames.RtiCSharp)
                  && matrixPlan.RequiresPeer("05-array-of-sequences", PeerNames.RtiCpp)
                  && matrixPlan.RequiresPeer("05-array-of-sequences", PeerNames.WireloomCpp),
                $"The {version} direct arrays-of-sequences fixture must build only the two C++ peers.");
        }

        var int8MinimumPairs = ScenarioPlanBuilder.Create(workspace.Manifest,
            Path.Combine(workspace.TestRoot, "wire-case-exclusions-7.3.1.json"), "7.3.1",
            "07-union-int8", shardIndex: 0, shardCount: 1);
        var minimumPairings = int8MinimumPairs.Cases.Single(item => item.CaseId == "07-union-int8").Scenarios
            .Where(item => item.Fixture == "union-minimum")
            .Select(item => $"{item.Writer}|{item.Reader}");
        Check(minimumPairings.SequenceEqual(cppOnlyArrayPairings, StringComparer.Ordinal),
            "The 7.3.1 int8 minimum exception must retain all four directions among the C++ peers.");
        var completeInt8Pairings = new[]
        {
            "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++", "Wireloom C#|Wireloom C++",
            "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++", "RTI C#|Wireloom C++",
            "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++", "RTI C++|Wireloom C++",
            "Wireloom C++|Wireloom C#", "Wireloom C++|RTI C#", "Wireloom C++|RTI C++", "Wireloom C++|Wireloom C++",
        };
        foreach (var fixture in new[] { "union-maximum", "union-default" })
        {
            var pairings = int8MinimumPairs.Cases.Single(item => item.CaseId == "07-union-int8").Scenarios
                .Where(item => item.Fixture == fixture)
                .Select(item => $"{item.Writer}|{item.Reader}");
            Check(pairings.SequenceEqual(completeInt8Pairings, StringComparer.Ordinal),
                $"The 7.3.1 int8 {fixture} fixture must retain all 16 peer directions.");
        }

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            var recursiveProfilePlan = ScenarioPlanBuilder.Create(workspace.Manifest,
                Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json"), version,
                "06-aggregates", shardIndex: 0, shardCount: 1);
            var recursivePairings = recursiveProfilePlan.Cases.Single(item => item.CaseId == "06-aggregates").Scenarios
                .Where(item => item.Fixture == "direct-recursive-sequence")
                .Select(item => $"{item.Writer}|{item.Reader}");
            Check(recursivePairings.SequenceEqual(["Wireloom C#|RTI C++"], StringComparer.Ordinal),
                $"The RTI {version} recursive fixture must retain only its verified C# writer to RTI C++ reader direction.");
        }

        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            var flatDataPlan = ScenarioPlanBuilder.Create(workspace.Manifest,
                Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json"), version,
                "10-flat-data-binding", shardIndex: 0, shardCount: 1);
            var flatDataUsesCpp = version == "7.7.0";
            CheckCasePairings(flatDataPlan, "10-flat-data-binding", ["non-default"], flatDataUsesCpp ?
            [
                "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++", "Wireloom C#|Wireloom C++",
                "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++", "RTI C#|Wireloom C++",
                "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++", "RTI C++|Wireloom C++",
                "Wireloom C++|Wireloom C#", "Wireloom C++|RTI C#", "Wireloom C++|RTI C++", "Wireloom C++|Wireloom C++",
            ] :
            [
                "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++",
                "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++",
                "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++",
            ], $"C047 must schedule the complete {(flatDataUsesCpp ? "16-pairing" : "9-pairing")} directed matrix for RTI {version}.");
            var flatDataCase = flatDataPlan.Cases.Single(item => item.CaseId == "10-flat-data-binding");
            Check(flatDataPlan.WireloomCppCases.Contains("10-flat-data-binding", StringComparer.Ordinal) == flatDataUsesCpp
                  && flatDataCase.Scenarios.Any(scenario => scenario.Writer == PeerNames.WireloomCpp || scenario.Reader == PeerNames.WireloomCpp) == flatDataUsesCpp,
                $"C047 Wireloom C++ eligibility must be limited to the RTI 7.7.0 FlatData API profile, not RTI {version}.");
            Check(flatDataUsesCpp || flatDataCase.Scenarios.All(scenario =>
                    scenario.Writer != PeerNames.WireloomCpp && scenario.Reader != PeerNames.WireloomCpp),
                $"C047's legacy RTI {version} matrix must retain all three supported peers without any Wireloom C++ endpoint.");
        }

        var nativeScopeRoot = Path.Combine(workspace.TestRoot, "flat-data-native-scope");
        Directory.CreateDirectory(nativeScopeRoot);
        var nativeScopeManifest = Path.Combine(nativeScopeRoot, "manifest.json");
        var nativeScopeExclusions = Path.Combine(nativeScopeRoot, "exclusions.json");
        File.WriteAllText(nativeScopeManifest,
            """{ "positiveCases": [ { "id": "10-flat-data-binding", "wireMatrix": { "excludedPeersByRtiVersion": { "6.1.2": { "Wireloom C++": "FlatData API is RTI 7.7-only." }, "7.3.1": { "Wireloom C++": "FlatData API is RTI 7.7-only." } } } } ] }""");
        File.WriteAllText(nativeScopeExclusions,
            """{ "notImplementedCases": [], "notApplicableCases": [{ "id": "10-flat-data-binding", "reason": "This case is selected only for native peer scope." }] }""");
        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            var nativeScopePlan = ScenarioPlanBuilder.Create(nativeScopeManifest, nativeScopeExclusions, version,
                caseFilter: "10-flat-data-binding", shardIndex: 0, shardCount: 1,
                nativePeerScopeCaseFilter: "10-flat-data-binding");
            var expectedCount = version == "7.7.0" ? 2 : 9;
            Check(nativeScopePlan.Scenarios.Count == expectedCount
                  && nativeScopePlan.NativePeerScopeCases.Contains("10-flat-data-binding", StringComparer.Ordinal) == (version == "7.7.0")
                  && nativeScopePlan.Scenarios.All(scenario =>
                      (scenario.Writer == PeerNames.WireloomCpp || scenario.Reader == PeerNames.WireloomCpp) == (version == "7.7.0")),
                $"C047's explicit native peer scope must honor RTI {version}: expected {expectedCount} exchanges, actual {nativeScopePlan.Scenarios.Count}; active native scope={nativeScopePlan.NativePeerScopeCases.Contains("10-flat-data-binding", StringComparer.Ordinal)}; pairs={string.Join(",", nativeScopePlan.Scenarios.Select(scenario => $"{scenario.Writer}>{scenario.Reader}"))}.");
        }

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            // Act
            var exclusions = Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json");
            var exchanges = ScenarioPlanBuilder.Create(workspace.Manifest, exclusions, version,
                "01-primitives,05-array-of-sequences-idl,07-union-wchar-label,14-typed-defaults", shardIndex: 0, shardCount: 1);
            var plan = Wrap(exchanges);

            // Assert
            Check(plan.RequiresPeer("01-primitives", PeerNames.RtiCSharp), "The standard three-peer case must build its RTI managed control.");
            Check(plan.RequiresPeer("01-primitives", PeerNames.WireloomCpp),
                $"Wireloom C++ peer builds are included by default for RTI {version}.");
            Check(!plan.RequiresPeer("05-array-of-sequences-idl", PeerNames.RtiCSharp), "The IDL-preserving case must omit its excluded RTI managed peer.");
            Check(!plan.RequiresPeer("07-union-wchar-label", PeerNames.RtiCSharp), "The unsupported wchar control must remain excluded.");
            Check(!plan.RequiresPeer("07-union-wchar-label", PeerNames.WireloomCpp), "The unsupported wchar discriminator must not require a Wireloom C++ build.");
            Check(plan.RequiresPeer("14-typed-defaults", PeerNames.RtiCSharp), "Normal typed-default fixtures still require RTI C# despite a sparse fixture's narrower pairing.");

            CheckCasePairings(exchanges, "07-union-wchar-label", ["union-number", "union-text", "union-default"],
                expectedWithoutRtiCSharpAndWireloomCpp, $"The RTI {version} wchar exclusions must preserve all pairings among supported peers for each fixture.");
        }

        var octetPlan = ScenarioPlanBuilder.Create(workspace.Manifest,
            Path.Combine(workspace.TestRoot, "wire-case-exclusions-7.7.0.json"), "7.7.0",
            "07-union-octet", shardIndex: 0, shardCount: 1);
        CheckCasePairings(octetPlan, "07-union-octet", ["union-minimum", "union-maximum", "union-default"],
            expectedWithoutRtiCSharp, "The RTI 7.7.0 octet C# exclusion must preserve all directed pairings among remaining peers for each fixture.");

        var primitives = ScenarioPlanBuilder.Create(workspace.Manifest,
            Path.Combine(workspace.TestRoot, "wire-case-exclusions-7.7.0.json"), "7.7.0",
            "01-primitives", shardIndex: 0, shardCount: 1);
        CheckCasePairings(primitives, "01-primitives", ["default"],
        [
            "Wireloom C#|Wireloom C#", "Wireloom C#|RTI C#", "Wireloom C#|RTI C++", "Wireloom C#|Wireloom C++",
            "RTI C#|Wireloom C#", "RTI C#|RTI C#", "RTI C#|RTI C++", "RTI C#|Wireloom C++",
            "RTI C++|Wireloom C#", "RTI C++|RTI C#", "RTI C++|RTI C++", "RTI C++|Wireloom C++",
            "Wireloom C++|Wireloom C#", "Wireloom C++|RTI C#", "Wireloom C++|RTI C++", "Wireloom C++|Wireloom C++",
        ], "The 7.7.0 primitives fixture must use the complete directed four-peer matrix.");
    }

    private static void CheckCasePairings(ScenarioPlan plan, string caseId, IReadOnlyList<string> expectedFixtures,
        IReadOnlyList<string> expectedPairings, string message)
    {
        // Arrange
        var scenarios = plan.Cases.Single(item => item.CaseId == caseId).Scenarios;

        // Assert
        Check(scenarios.Select(item => item.Fixture).Distinct(StringComparer.Ordinal)
                .SequenceEqual(expectedFixtures, StringComparer.Ordinal), $"{message} Fixture coverage changed.");
        foreach (var fixture in expectedFixtures)
        {
            var pairings = scenarios.Where(item => item.Fixture == fixture)
                .Select(item => $"{item.Writer}|{item.Reader}");
            Check(pairings.SequenceEqual(expectedPairings, StringComparer.Ordinal), message);
        }
    }

    private static MatrixPlan Wrap(ScenarioPlan exchanges) => new(exchanges.CaseIds, exchanges.RtiOnlyCases, exchanges);

    private static string Text(DirectedPeerPair pair) => $"{pair.Writer}|{pair.Reader}";

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
