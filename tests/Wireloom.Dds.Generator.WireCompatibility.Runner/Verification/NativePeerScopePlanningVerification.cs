namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks that explicit native peer-scope selection keeps managed applicability exclusions scoped.</summary>
internal static class NativePeerScopePlanningVerification
{
    /// <summary>Verifies optional collection exchanges use only the two native C++ peers in RTI 6.1.2.</summary>
    internal static void Run(MatrixOptions options)
    {
        // Arrange
        var manifest = Path.Combine(options.RepositoryRoot, "docs", "corpus", "manifest.json");
        var exclusions = Path.Combine(options.RepositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "wire-case-exclusions-6.1.2.json");
        var discovered = CorpusCaseDiscovery.Read(manifest, exclusions);

        // Act
        var plan = ScenarioPlanBuilder.Create(manifest, exclusions, "6.1.2", "08-keys", 0, 1,
            nativePeerScopeCaseFilter: "09-optional-collections");

        // Assert
        Check(!discovered.WireloomCases.Contains("09-optional-collections", StringComparer.Ordinal),
            "The native peer-scope selection must not broaden managed case discovery.");
        Check(plan.CaseIds.SequenceEqual(["08-keys", "09-optional-collections"], StringComparer.Ordinal),
            "Explicit native peer-scope cases must be added to the selected plan without replacing managed selections.");
        Check(plan.WireloomCppCases.Contains("09-optional-collections"),
            "The explicitly selected case must prepare a Wireloom C++ peer.");

        // Arrange / Act: the full shard plan carries explicit peer-scope selection to the merger.
        var planOutput = Path.Combine(options.ScratchDirectory, "native-peer-scope-plan-metadata");
        PlanArtifactWriter.Write(plan, planOutput);
        var planEvidence = System.Text.Json.Nodes.JsonNode.Parse(File.ReadAllText(Path.Combine(planOutput, "plan.json")))!;

        // Assert
        Check(planEvidence["nativePeerScopeCases"]!.AsArray()
                .Select(item => (string)item!)
                .SequenceEqual(["09-optional-collections"], StringComparer.Ordinal),
            "The shared plan must retain the explicit native peer scope for merged report finalization.");

        // Arrange / Act / Assert: the explicit peer-scope variable takes precedence while the legacy name remains accepted.
        var priorScopeFilter = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_NATIVE_PEER_SCOPE_CASE_FILTER");
        var priorLegacyFilter = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CPP_ONLY_CASE_FILTER");
        try
        {
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_NATIVE_PEER_SCOPE_CASE_FILTER", "09-optional-collections");
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_CPP_ONLY_CASE_FILTER", "08-keys");
            Check(MatrixOptions.FromEnvironment().NativePeerScopeCaseFilter == "09-optional-collections",
                "The explicit native peer-scope filter takes precedence over its legacy alias.");

            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_NATIVE_PEER_SCOPE_CASE_FILTER", null);
            Check(MatrixOptions.FromEnvironment().NativePeerScopeCaseFilter == "08-keys",
                "The legacy peer-scope filter remains accepted when the new setting is absent.");
        }
        finally
        {
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_NATIVE_PEER_SCOPE_CASE_FILTER", priorScopeFilter);
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_CPP_ONLY_CASE_FILTER", priorLegacyFilter);
        }

        var nativeScenarios = plan.Scenarios.Where(item => item.CaseId == "09-optional-collections").ToArray();
        var expected = WireCompatibilityCaseCatalog.FixturesFor("09-optional-collections", "6.1.2")
            .SelectMany(fixture => new[]
            {
                $"{fixture}|{PeerNames.RtiCpp}|{PeerNames.WireloomCpp}",
                $"{fixture}|{PeerNames.WireloomCpp}|{PeerNames.RtiCpp}",
            }).ToHashSet(StringComparer.Ordinal);
        var actual = nativeScenarios
            .Select(item => $"{item.Fixture}|{item.Writer}|{item.Reader}")
            .ToHashSet(StringComparer.Ordinal);
        Check(actual.SetEquals(expected),
            "Native peer-scope fixtures must retain both directed RTI C++ / Wireloom C++ exchanges and no managed peers.");
        Check(nativeScenarios.All(item => item.Version == "6.1.2"),
            "Native peer-scope exchanges must retain the exact selected RTI profile.");
        var managedCaseScenarios = plan.Scenarios.Where(item => item.CaseId == "08-keys").ToArray();
        Check(managedCaseScenarios.Length == WireCompatibilityCaseCatalog.FixturesFor("08-keys", "6.1.2").Count * 16
              && managedCaseScenarios.Any(item => item is { Writer: PeerNames.WireloomCpp, Reader: PeerNames.WireloomCpp }),
            "Every other eligible case in the same plan must also retain its full Wireloom C++ peer matrix.");

        var fullMatrixPlan = ScenarioPlanBuilder.Create(manifest, exclusions, "6.1.2", null, 0, 1,
            nativePeerScopeCaseFilter: "09-optional-collections");
        Check(fullMatrixPlan.CaseIds.Count == discovered.WireloomCases.Count + discovered.RtiOnlyCases.Count + 1
              && fullMatrixPlan.CaseIds.Contains("08-keys", StringComparer.Ordinal)
              && fullMatrixPlan.CaseIds.Contains("09-optional-collections", StringComparer.Ordinal),
            "Adding a native peer scope without a managed filter must retain the complete eligible case matrix.");

        var focusedNativePeerScopePlan = ScenarioPlanBuilder.Create(manifest, exclusions, "6.1.2",
            "09-optional-collections", 0, 1,
            nativePeerScopeCaseFilter: "09-optional-collections");
        var focusedMatrixPlan = new MatrixPlan(focusedNativePeerScopePlan.CaseIds,
            focusedNativePeerScopePlan.RtiOnlyCases, focusedNativePeerScopePlan);
        Check(focusedNativePeerScopePlan.CaseIds.SequenceEqual(["09-optional-collections"], StringComparer.Ordinal)
              && focusedNativePeerScopePlan.Scenarios.Count == WireCompatibilityCaseCatalog.FixturesFor("09-optional-collections", "6.1.2").Count * 2,
            "A matching managed filter may select only the profile-excluded case for the explicit native peer scope.");
        Check(focusedMatrixPlan.ManagedCaseIds.Count == 0,
            "A profile-excluded native peer-scope case must not be queued for Wireloom managed peer generation.");

        var cppOracleManifest = Path.Combine(options.RepositoryRoot, "docs", "corpus", "6.1.2", "rtiddsgen", "cpp", "manifest.json");
        var cppCatalog = new CppOracleCatalog(cppOracleManifest, options.RepositoryRoot);
        var cppType = cppCatalog.FindTopicType("09-optional-collections",
            new CorpusCaseCatalog(manifest).Get("09-optional-collections"), "6.1.2");
        Check(cppType == "::CorpusOptionalCollections::Sample",
            "A native peer-scope descriptor must resolve the exact topic type from the retained RTI C++ oracle.");

        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "6.1.2",
            "09-optional-collections", 0, 1));
        ExpectFailure<InvalidDataException>(() => ScenarioPlanBuilder.Create(manifest, exclusions, "6.1.2",
            "08-keys", 0, 1, nativePeerScopeCaseFilter: "08-keys"));

        Console.WriteLine("Native peer-scope planning passed: managed peer exclusions retained; exact RTI C++ / Wireloom C++ directions verified.");
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

        throw new InvalidOperationException($"Invalid native peer-scope selection must throw {typeof(TException).Name}.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
