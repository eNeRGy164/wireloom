using System.Text.Json.Nodes;
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Explicit input and artifact paths for one report-rendering invocation.</summary>
/// <param name="RtiVersion">Exact runtime profile.</param>
/// <param name="ExpectedScenarioCount">Expected directed exchange count.</param>
/// <param name="ScenarioPath">Canonical scheduler evidence.</param>
/// <param name="JsonPath">Retained JSON report destination.</param>
/// <param name="MarkdownPath">Retained Markdown report destination.</param>
/// <param name="LogsDirectory">Sanitized scenario logs and generation evidence.</param>
/// <param name="ExclusionsPath">Exact-profile exclusion registry.</param>
/// <param name="ManifestPath">Corpus source of truth.</param>
/// <param name="RepositoryRoot">Root used to validate canonical IDL and wrapper bytes.</param>
/// <param name="ExecutionProvenancePath">Retained source identity captured before peer builds.</param>
internal sealed record ReportRequest(string RtiVersion, int ExpectedScenarioCount, string ScenarioPath, string JsonPath,
    string MarkdownPath, string LogsDirectory, string ExclusionsPath, string ManifestPath, string RepositoryRoot,
    string? ExecutionProvenancePath = null);

/// <summary>Validated report evidence shared by the JSON and Markdown writers without a file handoff.</summary>
/// <param name="RtiVersion">Exact runtime profile.</param>
/// <param name="ExpectedScenarioCount">Expected directed exchange count.</param>
/// <param name="Scenarios">Enriched scenario evidence in scheduler order.</param>
/// <param name="NotImplemented">Implementation gaps retained in reports.</param>
/// <param name="NotApplicable">Profile-specific applicability exclusions.</param>
/// <param name="KnownExceptions">Reported profile-specific peer exceptions.</param>
/// <param name="Manifest">Corpus metadata used for presentation.</param>
/// <param name="ExecutionProvenance">Immutable source identity captured before the exchanges.</param>
internal sealed record ReportData(string RtiVersion, int ExpectedScenarioCount, List<ReportFields> Scenarios,
    JsonArray NotImplemented, JsonArray NotApplicable, JsonObject[] KnownExceptions, JsonNode Manifest,
    SourceSnapshotEvidence ExecutionProvenance);
