using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using PeerPair = (string Writer, string Reader);
using FixtureKey = (string CaseId, string Fixture);
using System.Text.Json.Nodes;
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Formats validated wire evidence into the existing fixture/pairing Markdown report.</summary>
internal static class MarkdownReportWriter
{
    private static readonly JsonSerializerOptions KeyJsonOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    };

    /// <summary>Writes retained Markdown with the established cells, scope, adaptations, and problems.</summary>
    internal static void Write(ReportData data, string destination)
    {
        var groups = GroupScenarios(data.Scenarios);
        var lines = CreateHeader(data);
        AppendApplicability(lines, data.NotApplicable);
        AppendConfiguration(lines, data);
        AppendFixtureRows(lines, data, groups);
        File.WriteAllText(destination, string.Join("\n", lines) + "\n");
    }

    private static readonly PeerPair[] Pairings =
    [
        ("Wireloom C#", "Wireloom C#"),
        ("Wireloom C#", "RTI C#"),
        ("Wireloom C#", "RTI C++"),

        ("RTI C#", "Wireloom C#"),
        ("RTI C#", "RTI C#"),
        ("RTI C#", "RTI C++"),

        ("RTI C++", "Wireloom C#"),
        ("RTI C++", "RTI C#"),
        ("RTI C++", "RTI C++"),
    ];

    private static string FormatScenarioCell(ReportFields? item)
    {
        if (item is null)
        {
            return "❔";
        }

        var adaptationMarker = item.ContainsKey("adaptations") ? "<sup>†</sup>" : "";

        var state = (string)item["status"]!;
        if (state == "PASS")
        {
            if (adaptationMarker.Length > 0)
            {
                return $"🟧{adaptationMarker}";
            }

            return "✅";
        }

        if (state == "FAIL")
        {
            return $"⛔{adaptationMarker}";
        }

        return $"❔{adaptationMarker}";
    }

    private static Dictionary<FixtureKey, Dictionary<PeerPair, ReportFields>> GroupScenarios(List<ReportFields> scenarios)
    {
        // Keep one row per fixture and one cell per directed writer/reader pairing.
        var groups = new Dictionary<FixtureKey, Dictionary<PeerPair, ReportFields>>();
        foreach (var scenario in scenarios)
        {
            var fixtureKey = ((string)scenario["case"]!, (string)scenario["fixture"]!);
            if (!groups.TryGetValue(fixtureKey, out var fixtureResults))
            {
                fixtureResults = new Dictionary<PeerPair, ReportFields>();
                groups.Add(fixtureKey, fixtureResults);
            }

            var writer = ((ReportFields)scenario["writer"]!)["peer"]!.ToString()!;
            var reader = ((ReportFields)scenario["reader"]!)["peer"]!.ToString()!;
            fixtureResults.Add((writer, reader), scenario);
        }

        return groups;
    }

    private static List<string> CreateHeader(ReportData data)
    {
        var rtiVersion = data.RtiVersion;
        var scenarios = data.Scenarios;
        var unsupported = data.NotImplemented;
        var notApplicable = data.NotApplicable;
        var knownExceptionCount = data.KnownExceptions.Length;

        var failed = scenarios
            .Where(scenario => (string)scenario["status"]! == "FAIL")
            .ToArray();
        var notRunCount = scenarios
            .Count(scenario => (string)scenario["status"]! == "NOT_RUN");
        var passed = scenarios.Count - failed.Length - notRunCount;

        var lines = new List<string>
        {
            $"# {rtiVersion} Wire compatibility results", "",
            $"**{scenarios.Count}** DDS exchanges · **{passed}** passed · **{failed.Length}** failed · **{notRunCount}** not run · **{knownExceptionCount}** known peer exception(s) · **{unsupported.Count}** case(s) not implemented · **{notApplicable.Count}** case(s) not applicable", "",
            "Each cell is a writer → reader exchange. A `\"` repeats the IDL from the previous row. Observed errors are quoted from sanitized logs.", "",
            "| Legend | Meaning |",
            "| :----: | :------ |",
            "|   ✅   | Exchange passed |",
            "| 🟧<sup>†</sup> | Passed with an adaptation |",
            "|   ⛔   | Failed |",
            "|   ❔   | Not run |",
            "|   ⚠️ Exception   | Known peer generation or compile issue; see the message in Problem / defect |",
            "| N/A | Peer or pairing is outside this case's configured scope |",
            "|   ⚫   | Not implemented |",
            "",
            "Failure details show the observed symptom. Adaptation notes link to the applicable workaround IDs (`W001`–`W018`).", "",
            $"{WorkaroundLink()} explains the default paths and applied code changes.", "",
        };

        return lines;
    }

    private static void AppendApplicability(List<string> lines, JsonArray notApplicable)
    {
        if (notApplicable.Count > 0)
        {
            lines.AddRange([
                "## Not applicable",
                "",
                "Unavailable features are N/A and excluded from exchange counts.",
                "",
                "| Case | Reason |",
                "| :--- | :----- |",
            ]);

            foreach (var item in notApplicable)
            {
                var reason = FormatExplanation((string)item!["reason"]!);
                lines.Add($"| `{item["id"]}` | {reason} |");
            }

            lines.Add("");
        }
    }

    private static void AppendConfiguration(List<string> lines, ReportData data)
    {
        var scenarios = data.Scenarios;
        var rtiVersion = data.RtiVersion;
        var configuredCases = scenarios
            .Where(i => i.ContainsKey("matrixConfiguration"))
            .Select(i => (string)i["case"]!)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();

        if (configuredCases.Length > 0)
        {
            lines.AddRange(["## Case-specific generation and peer scope", ""]);
            foreach (var id in configuredCases)
            {
                var configuration = (JsonObject)scenarios.First(i => (string)i["case"]! == id)["matrixConfiguration"]!;
                var options = (configuration["rtiCppCodegenOptions"]?.AsArray() ?? [])
                    .Select(o => $"`{(string)o!}`").ToArray();
                var idlAdaptations = (configuration["rtiCppIdlAdaptations"]?.AsArray() ?? [])
                    .Select(a => $"`{(string)a!}`").ToArray();
                var qualification = (string?)configuration["qualification"] ?? "";
                var generation = options.Any() ? $"generation adds {string.Join(", ", options)}" : "uses the default generation flags";
                var adaptation = idlAdaptations.Any() ? $" IDL adaptations: {string.Join(", ", idlAdaptations)}." : "";
                lines.Add($"- **{id}**: RTI C++ {generation}.{adaptation} {qualification} {WorkaroundLink(id)}".TrimEnd());

                if (scenarios.First(i => (string)i["case"]! == id).TryGetValue("generationConfiguration", out var generationConfiguration))
                {
                    lines.Add($"  Wireloom configuration: `{JsonSerializer.Serialize(generationConfiguration, KeyJsonOptions)}`. Input and generated-source SHA-256 hashes are in the JSON evidence.");
                }

                foreach (var exclusion in configuration["excludedPeers"]?.AsObject() ?? [])
                {
                    lines.Add($"  Known exception for **{exclusion.Key}**: {(string)exclusion.Value!}");
                }

                foreach (var adjustment in configuration["adjustments"]?.AsObject() ?? [])
                {
                    lines.Add($"  Adjustment for **{adjustment.Key}**: {(string)adjustment.Value!}");
                }

                var adjustedCSharpOracle = (string?)configuration["adjustedRtiCSharpOracleByRtiVersion"]?[rtiVersion];
                if (adjustedCSharpOracle is not null)
                {
                    lines.Add($"  RTI C# source: checked-in adjusted oracle at `{adjustedCSharpOracle}`.");
                }
            }

            lines.Add("");
        }
    }

    private static void AppendFixtureRows(List<string> lines, ReportData data, Dictionary<FixtureKey, Dictionary<PeerPair, ReportFields>> groups)
    {
        var pairings = Pairings;
        var unsupported = data.NotImplemented;
        var manifest = data.Manifest;
        lines.AddRange([
            "| Corpus feature | Variation | " + string.Join(" | ", pairings.Select(p => $"{DisplayPeerHeader(p.Writer)} → {DisplayPeerHeader(p.Reader)}")) + " | Adaptations | Problem / defect |",
            "| :------------- | :-------- | " + string.Join(" | ", pairings.Select(_ => ":--:")) + " | :-- | :-- |",
        ]);

        var unsupportedById = unsupported.ToDictionary(item => (string)item!["id"]!, StringComparer.Ordinal);
        var reportRows = new List<FixtureRow>();

        foreach (var (fixtureKey, results) in groups)
        {
            reportRows.Add(new FixtureRow(fixtureKey.CaseId, fixtureKey.Fixture, results, unsupportedById.GetValueOrDefault(fixtureKey.CaseId)));
        }

        // Implementation gaps still need a row when no peer exchanges were planned.
        foreach (var unsupportedCase in unsupported)
        {
            var caseId = (string)unsupportedCase!["id"]!;
            if (groups.Keys.All(key => key.CaseId != caseId))
            {
                reportRows.Add(new FixtureRow(caseId, "not implemented", new Dictionary<PeerPair, ReportFields>(), unsupportedCase));
            }
        }

        var orderedRows = reportRows
            .OrderBy(row => row.CaseId, StringComparer.Ordinal)
            .ThenBy(row => row.Fixture, StringComparer.Ordinal);
        var featureLabels = FeatureLabels(manifest);

        foreach (var fixtureRow in orderedRows)
        {
            lines.Add(FormatFixtureRow(fixtureRow, data, featureLabels));
        }
    }

    private static string FormatFixtureRow(FixtureRow fixtureRow, ReportData data, Dictionary<string, (string Tag, string Theme)> featureLabels)
    {
        var (id, fixture, row, unsupportedCase) = fixtureRow;
        var manifest = data.Manifest;
        var rtiVersion = data.RtiVersion;
        var pairings = Pairings;
        var caseItem = FindCase(manifest, id);
        var plannedPairings = unsupportedCase is null
            ? PeerPairingPolicy.GetPairings(caseItem, fixture).Select(pair => (pair.Writer, pair.Reader)).ToHashSet()
            : [.. pairings];
        var baseExcludedPeers = caseItem["wireMatrix"]?["excludedPeers"]?.AsObject() ?? [];
        var profileExceptions = caseItem["wireMatrix"]?["excludedPeersByRtiVersion"]?[rtiVersion]?.AsObject() ?? [];
        var cells = new List<string>();

        foreach (var pairing in pairings)
        {
            if (!plannedPairings.Contains(pairing) || baseExcludedPeers.ContainsKey(pairing.Writer) || baseExcludedPeers.ContainsKey(pairing.Reader))
            {
                cells.Add("N/A");
            }
            else if (profileExceptions.ContainsKey(pairing.Writer) || profileExceptions.ContainsKey(pairing.Reader))
            {
                cells.Add("⚠️ Exception");
            }
            else if (unsupportedCase is not null && UsesWireloom(pairing))
            {
                cells.Add("⚫");
            }
            else
            {
                cells.Add(FormatScenarioCell(row.GetValueOrDefault(pairing)));
            }
        }

        var adaptationText = FormatAdaptations(row, id);
        var problemText = FormatProblemDetails(fixtureRow, plannedPairings, baseExcludedPeers, profileExceptions);
        var (Tag, Theme) = featureLabels[id];
        var caseCell = $"**{Tag}** · `{id}`<br>{FormatExplanation(Theme)}";
        return $"| {caseCell} | `{fixture}` | {string.Join(" | ", cells)} | {(adaptationText.Length == 0 ? "—" : adaptationText)} | {problemText} |";
    }

    private static string FormatAdaptations(Dictionary<PeerPair, ReportFields> row, string id)
    {
        // Render a shared adjustment once, with all affected peers beside it.
        var adaptationPeers = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        foreach (var scenario in row.Values)
        {
            if (!scenario.TryGetValue("adaptations", out var value))
            {
                continue;
            }

            foreach (var (peer, description) in (Dictionary<string, string>)value!)
            {
                // A newline separates a peer-specific correction from shared setup.
                foreach (var adjustment in description.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (!adaptationPeers.TryGetValue(adjustment, out var peers))
                    {
                        peers = new List<string>();
                        adaptationPeers.Add(adjustment, peers);
                    }

                    var label = DisplayPeer(peer);
                    if (!peers.Contains(label))
                    {
                        peers.Add(label);
                    }
                }
            }
        }

        var adaptationNotes = adaptationPeers.Select(entry =>
            $"<sup>†</sup> **{string.Join(", ", entry.Value)}:** {FormatExplanation(entry.Key)}");

        var adaptationText = string.Join("<br><br>", adaptationNotes);
        if (adaptationText.Length > 0)
        {
            adaptationText += $"<br><br>{WorkaroundLink(id)}";
        }

        return adaptationText;
    }

    private static string FormatProblemDetails(FixtureRow fixtureRow, HashSet<PeerPair> plannedPairings, JsonObject baseExcludedPeers, JsonObject profileExceptions)
    {
        var (_, _, row, unsupportedCase) = fixtureRow;
        var pairings = Pairings;
        var problems = new List<(string Pairing, string Detail)>();

        foreach (var pair in pairings)
        {
            if (!plannedPairings.Contains(pair)
                || (unsupportedCase is not null && UsesWireloom(pair))
                || baseExcludedPeers.ContainsKey(pair.Writer) || baseExcludedPeers.ContainsKey(pair.Reader))
            {
                continue;
            }

            var excludedPeer = new[] { pair.Writer, pair.Reader }
                .Distinct(StringComparer.Ordinal)
                .FirstOrDefault(profileExceptions.ContainsKey);
            if (excludedPeer is not null)
            {
                problems.Add(($"{DisplayPeer(pair.Writer)} → {DisplayPeer(pair.Reader)}", profileExceptions[excludedPeer]!.GetValue<string>()));
                continue;
            }

            var item = row.GetValueOrDefault(pair);
            if (item is null || (string)item["status"]! is "FAIL" or "NOT_RUN")
            {
                var detail = "scenario did not run";
                if (item is not null)
                {
                    detail = (string?)item.GetValueOrDefault("observedProblem") ?? detail;
                }

                var name = $"{DisplayPeer(pair.Writer)} → {DisplayPeer(pair.Reader)}";
                problems.Add((name, detail));
            }
        }

        var problemParts = new List<string>();
        if (unsupportedCase is not null)
        {
            var issue = unsupportedCase["issue"]?.ToString()
                ?? throw new InvalidDataException("A not-implemented case must include its issue URL.");
            var issueId = issue.TrimEnd('/').Split('/').Last();
            problemParts.Add($"Not implemented by Wireloom: `{unsupportedCase["diagnostic"]}`. {unsupportedCase["reason"]} [#{issueId}]({issue})".Replace("|", "\\|"));
        }

        foreach (var problem in problems.GroupBy(item => item.Detail))
        {
            var names = string.Join(", ", problem.Select(item => item.Pairing));
            var detail = problem.Key.Replace("|", "\\|");
            problemParts.Add($"**{names}:** “{detail}”");
        }

        return problemParts.Count == 0 ? "—" : string.Join("<br><br>", problemParts);
    }

    private static bool UsesWireloom(PeerPair pairing) => pairing.Writer == "Wireloom C#" || pairing.Reader == "Wireloom C#";

    // Peer IDs remain stable in JSON and exclusion registries; labels can be shorter.
    private static string DisplayPeer(string peer)
    {
        if (peer == "Wireloom C#")
        {
            return "Wireloom";
        }

        return peer;
    }

    private static string DisplayPeerHeader(string peer) => peer == "Wireloom C#" ? "🧵" : peer;

    private static string WorkaroundLink(string? caseId = null)
    {
        var workaroundIds = caseId switch
        {
            "09-optional-string-sequences" => new[] { "W001" },
            "06-alias-composition" => ["W002"],
            "07-union-char-numeric" => ["W003", "W004"],
            "07-union-char-numeric-255" => ["W003", "W004", "W005", "W006"],
            "09-optional-recursive-array" => ["W007", "W008", "W009", "W010"],
            "06-aggregates" => ["W011", "W012"],
            "07-union-wchar-label" => ["W013"],
            "10-flat-data-binding" => ["W014"],
            "10-resolve-name-wstring-aliases" => ["W015"],
            "07-union-ulong" or "07-union-uint32" => ["W016"],
            "07-union-char-boundaries" => ["W017"],
            "14-autoid-scope" or "14-enum-constant" => ["W018"],
            _ => [],
        };

        var server = Environment.GetEnvironmentVariable("GITHUB_SERVER_URL") ?? "https://github.com";
        var repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? "eNeRGy164/wireloom";
        var revision = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "main";
        var url = $"{server}/{repository}/blob/{revision}/tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md";

        if (workaroundIds.Length == 0)
        {
            return $"[Workaround guide]({url})";
        }

        return string.Join(", ", workaroundIds.Select(id => $"[Workaround {id}]({url}#{id.ToLowerInvariant()})"));
    }

    private static string FormatExplanation(string text)
    {
        text = text
                .Replace("Wireloom C#", "Wireloom", StringComparison.Ordinal)
                .Replace("value/ranged", "`value`/`ranged`", StringComparison.Ordinal);

        // Preserve existing Markdown and URLs. Format identifiers and values outside
        // those spans; version numbers and issue IDs are not numeric code values.
        const string tokens =
            """
            `[^`]*`
            | https?://[^\s)]+
            | (?<![\w])(?:
                dds\.data_writer\.history\.memory_manager\.fast_pool\.pool_buffer_max_size=\d+
                | Support\.Instance | FromDynamicData\(null\) | @\w+(?:\([^)]*\))? | -funsigned-char
                | \w*Exception | CS\d{4} | DDSG\d{4}
                | DynamicData | DynamicType | TypeObject | TypeCode | UnionMember
                | Setfallback | FromDynamicData | Recursive | Holder | color | Uint8 | sbyte | int8 | char | wchar
              )(?![\w])
            | (?<![\w.#-])(?:0x[0-9A-Fa-f]+|-?\d+)(?!\w|\.\d)
            """;

        var formatted = Regex.Replace(text, tokens, match =>
        {
            if (match.Value.StartsWith('`') || match.Value.StartsWith("http", StringComparison.Ordinal))
            {
                return match.Value;
            }

            return $"`{match.Value}`";
        }, RegexOptions.IgnorePatternWhitespace);

        return formatted.Replace("|", "\\|", StringComparison.Ordinal);
    }

    private static Dictionary<string, (string Tag, string Theme)> FeatureLabels(JsonNode manifest)
    {
        var entries = manifest["positiveCases"]!.AsArray()
            .Concat(manifest["negativeCases"]!.AsArray())
            .ToList();

        foreach (var integrationIdl in manifest["integrationEntryPoints"]!.AsArray())
        {
            var idl = (string)integrationIdl!;
            entries.Add(new JsonObject
            {
                ["id"] = Path.GetFileNameWithoutExtension(idl),
                ["theme"] = Path.GetFileNameWithoutExtension(idl),
            });
        }

        var explicitTags = entries
            .Where(entry => !string.IsNullOrEmpty((string?)entry!["tag"]))
            .Select(entry => int.Parse(((string)entry!["tag"]!)[1..], System.Globalization.CultureInfo.InvariantCulture))
            .ToHashSet();
        var labels = new Dictionary<string, (string Tag, string Theme)>(StringComparer.Ordinal);
        var nextTag = 1;

        foreach (var entry in entries)
        {
            var tag = (string?)entry!["tag"];
            if (tag is null)
            {
                while (explicitTags.Contains(nextTag))
                {
                    nextTag++;
                }

                tag = $"C{nextTag++:D3}";
            }

            var id = (string)entry["id"]!;
            labels.Add(id, (tag, (string?)entry["theme"] ?? id));
        }

        return labels;
    }

    private static JsonNode FindCase(JsonNode manifest, string id) => manifest["positiveCases"]!.AsArray()
        .FirstOrDefault(item => (string)item!["id"]! == id)
        ?? throw new InvalidOperationException($"No positive corpus case found for {id}.");
    private sealed record FixtureRow(string CaseId, string Fixture, Dictionary<PeerPair, ReportFields> Results, JsonNode? Unsupported);
}
