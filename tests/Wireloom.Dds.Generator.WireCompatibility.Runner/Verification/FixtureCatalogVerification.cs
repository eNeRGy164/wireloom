namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks the ordered fixture contract used directly by planning and managed validation.</summary>
internal static class FixtureCatalogVerification
{
    /// <summary>Verifies default, named, and profile-specific fixtures without a utility subprocess.</summary>
    internal static void Run()
    {
        // Arrange
        var shared = new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["unknown-case"] = ["default"],
            ["01-primitives"] = ["default"],
            ["01-boundaries"] = ["non-default"],
            ["05-array-of-sequences-idl"] = ["sequence-empty", "sequence-unequal", "sequence-mixed-empty", "sequence-boundary"],
            ["06-aggregates"] = ["non-default", "direct-recursive-sequence"],
            ["09-optional-aggregate-member"] = ["optional-absent", "optional-present"],
            ["14-extensibility-full"] = ["non-default"],
            ["14-autoid-scope"] = ["scope-nested"],
            ["14-annotation-arguments"] = ["default", "non-default"],
            ["14-member-declarators"] = ["non-default"],
            ["14-union-label-expressions"] = ["symbolic", "arithmetic", "hexadecimal"],
            ["14-union-scoped-enum"] = ["enum-selected", "enum-first"],
            ["14-union-array-branch"] = ["array-branch", "scalar-branch"],
            ["14-string-expression-bound"] = ["string-boundary"],
            ["14-enum-constant"] = ["constant-and-label"],
            ["14-unbounded-receive-lifecycle"] = ["lifecycle-xcdr1", "lifecycle-xcdr2", "fragmented-xcdr1", "fragmented-xcdr2"],
        };

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            foreach (var (caseId, expected) in shared)
            {
                // Act
                var actual = WireCompatibilityCaseCatalog.FixturesFor(caseId, version);

                // Assert
                Check(actual.SequenceEqual(expected, StringComparer.Ordinal), $"Fixture order changed for {caseId} on {version}.");
            }
        }

        // Act: alternate profile queries so a previous lookup cannot leak its sparse fixture.
        var newer = WireCompatibilityCaseCatalog.FixturesFor("14-typed-defaults", "7.7.0");
        var older = WireCompatibilityCaseCatalog.FixturesFor("14-typed-defaults", "7.3.1");
        var newerAgain = WireCompatibilityCaseCatalog.FixturesFor("14-typed-defaults", "7.7.0");

        // Assert
        Check(newer.SequenceEqual(["default", "non-default", "sparse-omitted"], StringComparer.Ordinal), "Sparse typed-default validation must remain enabled on 7.7.0.");
        Check(older.SequenceEqual(["default", "non-default"], StringComparer.Ordinal), "Sparse typed-default validation must remain excluded on 7.3.1.");
        Check(newerAgain.SequenceEqual(newer, StringComparer.Ordinal), "Fixture lookups must remain independent across profiles.");

        Console.WriteLine("Fixture catalog passed: defaults, named fixture order, RTI-only controls, and sparse typed-default profile isolation; no utility subprocess required.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
