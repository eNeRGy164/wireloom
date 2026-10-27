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
                    { "id": "native-only", "wireMatrix": { "fixturePairings": { "default": [ "RTI C++|RTI C++" ] } } },
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
        var firstShard = Wrap(ScenarioPlanBuilder.Create(manifest, exclusions, "7.7.0", caseFilter: null, shardIndex: 0, shardCount: 2));
        File.Delete(manifest);

        // Assert
        foreach (var plan in plans)
        {
            Check(!plan.RequiresPeer("native-only", PeerNames.RtiCSharp) && plan.RequiresPeer("native-only", PeerNames.RtiCpp), "Peer requirements must use actual fixture exchanges rather than default unused endpoints.");
            Check(plan.RequiresPeer("03-enums-aliases", PeerNames.RtiCSharp), "A peer used by any fixture must remain available even when another fixture omits it.");
            Check(plan.RequiresPeer("09-optional-aggregate-member", PeerNames.RtiCSharp) && !plan.RequiresPeer("09-optional-aggregate-member", PeerNames.WireloomCSharp), "RTI-only requirements must follow the filtered plan.");
            Check(plan.RequiresPeer("profile", PeerNames.RtiCSharp) == (plan.Exchanges!.RtiVersion == "7.7.0"), "Peer requirements must retain profile-specific exclusions.");
            Check(!plan.RequiresPeer("unknown", PeerNames.RtiCSharp), "An unselected case must not require a peer.");
        }

        Check(!firstShard.CaseIds.Contains("09-optional-aggregate-member", StringComparer.Ordinal) &&
            !firstShard.RequiresPeer("09-optional-aggregate-member", PeerNames.RtiCSharp), "A different shard's cases must not affect peer preparation.");
        Check(!MatrixPlan.Empty.RequiresPeer("native-only", PeerNames.RtiCpp), "An empty or compile-only plan must not require runtime peers.");
    }

    private static void VerifyCorpusRequirements(MatrixOptions options)
    {
        // Arrange
        var workspace = new MatrixWorkspace(options);
        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            // Act
            var exchanges = ScenarioPlanBuilder.Create(workspace.Manifest,
                Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json"), version,
                "01-primitives,05-array-of-sequences-idl,07-union-wchar-label,14-typed-defaults", shardIndex: 0, shardCount: 1);
            var plan = Wrap(exchanges);

            // Assert
            Check(plan.RequiresPeer("01-primitives", PeerNames.RtiCSharp), "The standard three-peer case must build its RTI managed control.");
            Check(!plan.RequiresPeer("05-array-of-sequences-idl", PeerNames.RtiCSharp), "The IDL-preserving case must omit its excluded RTI managed peer.");
            Check(!plan.RequiresPeer("07-union-wchar-label", PeerNames.RtiCSharp), "The unsupported wchar control must remain excluded.");
            Check(plan.RequiresPeer("14-typed-defaults", PeerNames.RtiCSharp), "Normal typed-default fixtures still require RTI C# despite a sparse fixture's narrower pairing.");
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
