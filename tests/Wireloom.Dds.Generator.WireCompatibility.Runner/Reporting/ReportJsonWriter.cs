using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Preserves the retained wire report JSON schema and property order.</summary>
internal static class ReportJsonWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    };

    /// <summary>Writes validated report data directly to its retained JSON artifact.</summary>
    internal static void Write(ReportData data, string destination)
    {
        var version = data.RtiVersion;
        var expectedCount = data.ExpectedScenarioCount;
        var scenarios = data.Scenarios;
        var notImplemented = data.NotImplemented;
        var notApplicable = data.NotApplicable;
        var knownExceptions = data.KnownExceptions;

        var document = new ReportFields
            {
                ["rtiVersion"] = version,
                ["expectedScenarioCount"] = expectedCount,
                ["scenarioCount"] = scenarios.Count,
                ["fixtureCount"] = scenarios.Select(scenario => (scenario["case"], scenario["fixture"])).Distinct().Count(),
                ["notImplementedCount"] = notImplemented.Count,
                ["adaptedScenarioCount"] = scenarios.Count(scenario => scenario.ContainsKey("adaptations")),
                ["notApplicableCount"] = notApplicable.Count,
                ["knownExceptionCount"] = knownExceptions.Length,
                ["knownExceptions"] = knownExceptions,
                ["notApplicableCases"] = notApplicable,
                ["notImplementedCases"] = notImplemented,
                ["scenarios"] = scenarios,
            };

        File.WriteAllText(destination, JsonSerializer.Serialize(document, JsonOptions) + "\n");
    }
}
