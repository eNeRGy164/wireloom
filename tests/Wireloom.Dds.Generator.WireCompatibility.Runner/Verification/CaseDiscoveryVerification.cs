namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks profile discovery and synthetic exclusion policy in-process.</summary>
internal static class CaseDiscoveryVerification
{
    /// <summary>Verifies both qualified profiles and edge cases without a utility subprocess.</summary>
    internal static void Run(MatrixWorkspace workspace)
    {
        VerifyExclusionPolicy(workspace.ForCase("case-discovery-verification").Directory);

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            // Arrange
            var exclusions = Path.Combine(workspace.TestRoot, $"wire-case-exclusions-{version}.json");
            var discovered = CorpusCaseDiscovery.Read(workspace.Manifest, exclusions);
            var expectedCaseCount = version == "7.3.1" ? 92 : 93;

            // Assert
            Check(discovered.WireloomCases.Count == expectedCaseCount, $"Case discovery must retain the expected eligible case count for RTI {version}.");
            Check(discovered.WireloomCases.Distinct(StringComparer.Ordinal).Count() == discovered.WireloomCases.Count, $"Case discovery must not duplicate cases for RTI {version}.");
            Check(discovered.RtiOnlyCases.Distinct(StringComparer.Ordinal).Count() == discovered.RtiOnlyCases.Count, $"RTI-only discovery must not duplicate cases for RTI {version}.");
            Check(!discovered.WireloomCases.Intersect(discovered.RtiOnlyCases, StringComparer.Ordinal).Any(), $"Wireloom and RTI-only cases must remain disjoint for RTI {version}.");

            Console.WriteLine($"Case discovery {version}: {discovered.WireloomCases.Count} eligible cases; profile selection is valid.");
        }
    }

    private static void VerifyExclusionPolicy(string directory)
    {
        // Arrange
        Directory.CreateDirectory(directory);

        var manifest = Path.Combine(directory, "manifest.json");
        var exclusions = Path.Combine(directory, "exclusions.json");

        File.WriteAllText(manifest,
            """
            { "positiveCases": [ { "id": "case-z" }, { "id": "case-a" }, { "id": "CASE-A" }, { "id": "case-b" } ] }
            """);

        File.WriteAllText(exclusions,
            """
            {
              "notImplementedCases": [ { "id": "case-a" }, { "id": "control-only" } ],
              "notApplicableCases": [ { "id": "case-z" } ]
            }
            """);

        // Act
        var discovered = CorpusCaseDiscovery.Read(manifest, exclusions);

        // Assert
        Check(discovered.WireloomCases.SequenceEqual(["CASE-A", "case-b"], StringComparer.Ordinal), "Exclusions must use ordinal identity and retain manifest order.");
        Check(discovered.RtiOnlyCases.SequenceEqual(["case-a", "control-only"], StringComparer.Ordinal), "Implementation gaps must remain RTI-only controls in declaration order.");

        // Arrange
        File.WriteAllText(exclusions,
            """
            { "notImplementedCases": [] }
            """);

        // Act
        var noApplicabilityExclusions = CorpusCaseDiscovery.Read(manifest, exclusions);

        // Assert
        Check(noApplicabilityExclusions.WireloomCases.SequenceEqual(["case-z", "case-a", "CASE-A", "case-b"], StringComparer.Ordinal), "An omitted optional applicability list must preserve all positive cases.");

        // Arrange
        File.WriteAllText(manifest,
            """
            { "positiveCases": [ { "id": null } ] }
            """);

        // Act
        var rejectedMalformedId = false;
        try
        {
            CorpusCaseDiscovery.Read(manifest, exclusions);
        }
        catch (InvalidDataException)
        {
            rejectedMalformedId = true;
        }

        // Assert
        Check(rejectedMalformedId, "Malformed case identifiers must be rejected before peer preparation.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
