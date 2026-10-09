// File-based utility for the wire-compatibility matrix. Requires the .NET 10 SDK.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;

const string usage = "Usage: matrix-tools.cs <list-cases|list-rti-only-cases|case-field|case-info|json-field|discover-cases|discover-rti|includes|includes-recursive|render-report> ...";
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
};

try
{
    if (args.Length == 0) throw new ArgumentException(usage);
    switch (args[0])
    {
        case "list-cases":
        {
            RequireArgs(3);
            var manifest = ReadJson(args[1]);
            var exclusions = ReadJson(args[2]);
            var unsupported = exclusions["notImplementedCases"]!.AsArray()
                .Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
            foreach (var item in manifest["positiveCases"]!.AsArray())
            {
                var id = (string)item!["id"]!;
                if (!unsupported.Contains(id)) Console.WriteLine(id);
            }
            break;
        }
        case "list-rti-only-cases":
        {
            RequireArgs(2);
            foreach (var item in ReadJson(args[1])["notImplementedCases"]!.AsArray())
                Console.WriteLine((string)item!["id"]!);
            break;
        }
        case "case-field":
        {
            RequireArgs(4);
            var item = FindCase(ReadJson(args[1]), args[2]);
            switch (args[3])
            {
                case "idl": Console.WriteLine((string)item["idl"]!); break;
                case "defines":
                    foreach (var define in item["defines"]?.AsArray() ?? []) Console.WriteLine((string)define!);
                    break;
                default: throw new ArgumentException($"Unknown case field: {args[3]}");
            }
            break;
        }
        case "case-info":
        {
            RequireArgs(4);
            var registry = ReadJson(args[1])["cases"]!.AsArray().First(item => (string)item!["id"]! == args[3])!;
            var corpusCase = FindCase(ReadJson(args[2]), args[3]);
            Console.WriteLine((string)registry["csharpType"]!);
            Console.WriteLine((string)registry["cppType"]!);
            Console.WriteLine((string)corpusCase["idl"]!);
            foreach (var define in corpusCase["defines"]?.AsArray() ?? []) Console.WriteLine((string)define!);
            break;
        }
        case "json-field":
        {
            RequireArgs(3);
            Console.WriteLine((string)ReadJson(args[1])[args[2]]!);
            break;
        }
        case "discover-cases":
            DiscoverCases(args);
            break;
        case "discover-rti":
            DiscoverRti(args);
            break;
        case "includes":
            DiscoverIncludes(args);
            break;
        case "includes-recursive":
            DiscoverIncludesRecursive(args);
            break;
        case "render-report":
            RenderReport(args);
            break;
        default:
            throw new ArgumentException(usage);
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    return 2;
}

return 0;

void RequireArgs(int count)
{
    if (args.Length != count) throw new ArgumentException(usage);
}

JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

JsonNode FindCase(JsonNode manifest, string id) => manifest["positiveCases"]!.AsArray()
    .FirstOrDefault(item => (string)item!["id"]! == id)
    ?? throw new InvalidOperationException($"No positive corpus case found for {id}.");

void DiscoverCases(string[] commandArgs)
{
    if (commandArgs.Length is not (4 or 5)) throw new ArgumentException(usage);
    var manifestPath = Path.GetFullPath(commandArgs[1]);
    var generatedRoot = Path.GetFullPath(commandArgs[2]);
    var outputPath = Path.GetFullPath(commandArgs[3]);
    var requestedCase = commandArgs.Length == 5 ? commandArgs[4] : null;
    var exclusionsPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "wire-case-exclusions.json");
    if (!File.Exists(exclusionsPath))
    {
        // The script is cached in the SDK temp directory, so locate exclusions relative to the manifest/repository.
        var repoRoot = FindRepositoryRoot(manifestPath);
        exclusionsPath = Path.Combine(repoRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "wire-case-exclusions.json");
    }
    var manifest = ReadJson(manifestPath);
    var unsupported = ReadJson(exclusionsPath)["notImplementedCases"]!.AsArray()
        .Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
    var selected = manifest["positiveCases"]!.AsArray()
        .Where(item => !unsupported.Contains((string)item!["id"]!))
        .Where(item => requestedCase is null || (string)item!["id"]! == requestedCase)
        .ToArray();
    if (requestedCase is not null && selected.Length == 0)
        throw new InvalidOperationException($"Case {requestedCase} is not a wire-testable Wireloom positive case.");

    var byIdl = selected.ToDictionary(item => Path.GetFileName((string)item!["idl"]!), item => (string)item!["id"]!, StringComparer.Ordinal);
    var candidates = selected.ToDictionary(item => (string)item!["id"]!, _ => new List<(string Cs, string Cpp)>(), StringComparer.Ordinal);
    foreach (var source in Directory.Exists(generatedRoot) ? Directory.EnumerateFiles(generatedRoot, "*.g.cs", SearchOption.AllDirectories) : [])
    {
        var text = File.ReadAllText(source);
        var header = Regex.Match(text, "generated by Wireloom from '([^']+)'");
        var support = Regex.Match(text, @"class\s+\w+\s*:\s*TypeSupport<\s*([\w.]+)\s*>");
        var ns = Regex.Match(text, @"^namespace\s+([\w.]+)\s*;", RegexOptions.Multiline);
        if (!header.Success || !support.Success || !byIdl.TryGetValue(header.Groups[1].Value, out var id)) continue;
        var type = support.Groups[1].Value;
        if (ns.Success && !type.Contains('.')) type = $"{ns.Groups[1].Value}.{type}";
        candidates[id].Add((type, "::" + type.Replace(".", "::", StringComparison.Ordinal)));
    }

    var results = new List<object>();
    foreach (var item in selected)
    {
        var id = (string)item!["id"]!;
        if (candidates[id].Count == 0) throw new InvalidOperationException($"No generated type support found for positive case {id}.");
        var preferred = new Dictionary<string, int>(StringComparer.Ordinal) { ["Sample"] = 0, ["Composed"] = 1 };
        var selectedType = id switch
        {
            "03-enums-aliases" => "Record",
            "02-multiple" => "WireloomWireCompatibilityMultipleTypes",
            "03-enum-values-prefix" or "03-enum-values-explicit" => "EnumTopic",
            "09-optional-aggregate-member" => "Holder",
            "07-union-aliases" or "06-valuetypes" => "Holder",
            "08-key-inherited" => "Derived",
            "08-key-nested" => "Outer",
            _ => null,
        };
        if (selectedType is not null) preferred[selectedType] = -1;
        var found = candidates[id].OrderBy(candidate => preferred.GetValueOrDefault(candidate.Cs.Split('.').Last(), 2))
            .ThenBy(candidate => candidate.Cs, StringComparer.Ordinal).First();
        results.Add(new { id, csharpType = found.Cs, cppType = found.Cpp });
    }
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new { cases = results }, jsonOptions) + "\n");
    Console.WriteLine($"Discovered {results.Count} Wireloom positive case type(s).");
}

void DiscoverRti(string[] commandArgs)
{
    if (commandArgs.Length != 6) throw new ArgumentException(usage);
    var (manifestPath, caseId, wireloomType, sourceDir, outputPath) = (commandArgs[1], commandArgs[2], commandArgs[3], Path.GetFullPath(commandArgs[4]), commandArgs[5]);
    _ = ReadJson(manifestPath);
    var types = Directory.EnumerateFiles(sourceDir, "*.cs", SearchOption.AllDirectories)
        .SelectMany(path => Regex.Matches(File.ReadAllText(path), @"class\s+\w+Support\s*:\s*Rti\.Dds\.Topics\.TypeSupport<\s*global::([\w.]+)\s*>")
            .Select(match => match.Groups[1].Value)).ToList();
    if (types.Count == 0) throw new InvalidOperationException($"No RTI C# TypeSupport was generated for {caseId}.");
    var preferred = new Dictionary<string, int>(StringComparer.Ordinal) { ["Sample"] = 0, ["Composed"] = 1 };
    var selected = caseId switch
    {
        "03-enums-aliases" => "Record",
        "09-optional-aggregate-member" => "Holder",
        "07-union-aliases" or "06-valuetypes" => "Holder",
        "08-key-inherited" => "Derived",
        "08-key-nested" => "Outer",
        _ => null,
    };
    if (selected is not null) preferred[selected] = -1;
    var preferredName = wireloomType.Split('.').Last();
    var selectedType = types.OrderBy(type => type.Split('.').Last() == preferredName ? 0 : 1)
        .ThenBy(type => preferred.GetValueOrDefault(type.Split('.').Last(), 2))
        .ThenBy(type => type, StringComparer.Ordinal).First();
    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new { id = caseId, sourceDir, rtiCSharpType = selectedType }, jsonOptions) + "\n");
}

void DiscoverIncludes(string[] commandArgs)
{
    if (commandArgs.Length != 4) throw new ArgumentException(usage);
    var rootIdl = Path.GetFullPath(commandArgs[1]);
    var roots = commandArgs.Skip(2).Select(Path.GetFullPath).ToArray();
    var includeRegex = new Regex("^\\s*#\\s*include\\s*[<\\\"]([^>\\\"]+)[>\\\"]", RegexOptions.Multiline);
    foreach (Match match in includeRegex.Matches(File.ReadAllText(rootIdl)))
    {
        var include = match.Groups[1].Value;
        var candidates = new[] { Path.Combine(Path.GetDirectoryName(rootIdl)!, include) }
            .Concat(roots.Select(root => Path.Combine(root, include)));
        var source = candidates.FirstOrDefault(File.Exists);
        if (source is not null) Console.WriteLine($"{include}\t{Path.GetFullPath(source)}");
    }
}

void DiscoverIncludesRecursive(string[] commandArgs)
{
    if (commandArgs.Length != 4) throw new ArgumentException(usage);
    var rootIdl = Path.GetFullPath(commandArgs[1]);
    var roots = commandArgs.Skip(2).Select(Path.GetFullPath).ToArray();
    var discovered = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { rootIdl };
    var pending = new Queue<string>();
    pending.Enqueue(rootIdl);
    var includeRegex = new Regex("^\\s*#\\s*include\\s*[<\\\"]([^>\\\"]+)[>\\\"]", RegexOptions.Multiline);

    while (pending.TryDequeue(out var currentIdl))
    {
        foreach (Match match in includeRegex.Matches(File.ReadAllText(currentIdl)))
        {
            var include = match.Groups[1].Value;
            var candidates = new[] { Path.Combine(Path.GetDirectoryName(currentIdl)!, include) }
                .Concat(roots.Select(root => Path.Combine(root, include)));
            var source = candidates.FirstOrDefault(File.Exists);
            if (source is null) continue;

            source = Path.GetFullPath(source);
            if (!discovered.Add(source)) continue;
            Console.WriteLine($"{include}\t{source}");
            pending.Enqueue(source);
        }
    }
}

string FindRepositoryRoot(string path)
{
    var directory = new DirectoryInfo(Path.GetDirectoryName(Path.GetFullPath(path))!);
    while (directory is not null)
    {
        if (Directory.Exists(Path.Combine(directory.FullName, ".git")) || File.Exists(Path.Combine(directory.FullName, ".git"))) return directory.FullName;
        directory = directory.Parent;
    }
    throw new DirectoryNotFoundException("Could not locate repository root.");
}

void RenderReport(string[] commandArgs)
{
    if (commandArgs.Length != 8) throw new ArgumentException(usage);
    var version = commandArgs[1];
    var expectedCount = int.Parse(commandArgs[2]);
    var sourcePath = commandArgs[3];
    var outputPath = commandArgs[4];
    var markdownPath = commandArgs[5];
    var logsPath = commandArgs[6];
    var exclusions = ReadJson(commandArgs[7]);
    var notImplemented = exclusions["notImplementedCases"]!.AsArray();
    var expectedFailures = exclusions["expectedFailures"]?.AsArray() ?? [];
    var scenarios = new List<Dictionary<string, object?>>();
    var peerData = new Dictionary<string, (string Implementation, string Language)>
    {
        ["Wireloom C#"] = ("Wireloom", "C#"), ["RTI C#"] = ("RTI", "C#"), ["RTI C++"] = ("RTI", "C++"),
    };
    string? ExpectedFailure(string id, string writer, string reader, string fixture)
    {
        var item = expectedFailures.FirstOrDefault(f => (string)f!["id"]! == id && (string)f["writer"]! == writer
            && (string)f["reader"]! == reader && (f["fixture"] is null || (string)f["fixture"]! == fixture));
        return item is null ? null : (string)item["reason"]!;
    }
    string? Adaptation(string id, string fixture, string peer)
    {
        if (id == "09-optional-aggregate-member") return "Typed optional aggregate fixture: construct Holder optionals and select its union branch through generated members; nested DynamicData paths cannot loan unset optional parents.";
        if (id is "02-multiple" or "03-enum-values-prefix" or "03-enum-values-explicit") return "Harness wrapper IDL composes the declarations into a topic type.";
        if (id == "06-alias-composition" && peer == "RTI C#") return "Custom generated-code workaround: qualifies the member conversion receiver because RTI 7.7.0 emits a shadowed field access.";
        if (peer != "RTI C++") return null;
        if (id == "09-optional-string-sequences" && fixture is ("optional-wide-only" or "optional-multiple")) return "Custom RTI C++ path: DynamicData C API per-element setters bypass the RTI 7.7.0 typed optional wide-string sequence serializer.";
        if (id == "07-union-wchar-label") return "Custom RTI DynamicType: manually models the wchar discriminator because rtiddsgen cannot parse wchar union labels.";
        if (id == "10-flat-data-binding") return "Custom IDL adaptation: removes @language_binding(FLAT_DATA) before RTI C++ generation.";
        return null;
    }

    foreach (var line in File.ReadLines(sourcePath).Where(line => line.Length > 0))
    {
        var fields = line.Split('\t');
        if (fields.Length != 10) throw new InvalidDataException("Scenario TSV row must contain 10 fields.");
        var (id, wireType, rtiType, cppType, writer, writerVersion, reader, readerVersion, status, fixture) =
            (fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9]);
        Dictionary<string, object?> Endpoint(string peer, string peerVersion)
        {
            var adaptation = Adaptation(id, fixture, peer);
            var data = peerData[peer];
            var endpoint = new Dictionary<string, object?>
            {
                ["peer"] = peer, ["implementation"] = data.Implementation, ["language"] = data.Language,
                ["rtiVersion"] = peerVersion, ["mode"] = adaptation is not null ? "custom-test-adaptation" : peer == "Wireloom C#" ? "wireloom-generated" : "rti-default",
            };
            if (adaptation is not null) endpoint["adaptation"] = adaptation;
            return endpoint;
        }
        var writerEndpoint = Endpoint(writer, writerVersion);
        var readerEndpoint = Endpoint(reader, readerVersion);
        var scenario = new Dictionary<string, object?>
        {
            ["case"] = id, ["writer"] = writerEndpoint, ["reader"] = readerEndpoint, ["fixture"] = fixture, ["status"] = status,
        };
        var adaptations = new Dictionary<string, string>();
        foreach (var endpoint in new[] { writerEndpoint, readerEndpoint })
            if (endpoint.TryGetValue("adaptation", out var value)) adaptations[(string)endpoint["peer"]!] = (string)value!;
        if (adaptations.Count > 0) scenario["adaptations"] = adaptations;
        var expected = ExpectedFailure(id, writer, reader, fixture);
        if (expected is not null) scenario["expectedFailure"] = expected;
        if (wireType.Length + rtiType.Length + cppType.Length > 0)
            scenario["types"] = new Dictionary<string, string> { ["Wireloom C#"] = wireType, ["RTI C#"] = rtiType, ["RTI C++"] = cppType };
        if (status == "FAIL") scenario["observedProblem"] = ObservedProblem(id, fixture, writer, reader, status, logsPath);
        scenarios.Add(scenario);
    }

    var report = new Dictionary<string, object?>
    {
        ["rtiVersion"] = version, ["expectedScenarioCount"] = expectedCount, ["scenarioCount"] = scenarios.Count,
        ["fixtureCount"] = scenarios.Select(s => (s["case"], s["fixture"])).Distinct().Count(),
        ["expectedFailureCount"] = scenarios.Count(s => (string)s["status"]! == "FAIL" && s.ContainsKey("expectedFailure")),
        ["notImplementedCount"] = notImplemented.Count, ["adaptedScenarioCount"] = scenarios.Count(s => s.ContainsKey("adaptations")),
        ["notImplementedCases"] = notImplemented, ["expectedFailures"] = expectedFailures, ["scenarios"] = scenarios,
    };
    File.WriteAllText(outputPath, JsonSerializer.Serialize(report, jsonOptions) + "\n");
    WriteMarkdown(version, scenarios, notImplemented, expectedFailures, markdownPath);

    string ObservedProblem(string id, string fixture, string writer, string reader, string status, string directory)
    {
        var peerIds = new Dictionary<string, string> { ["Wireloom C#"] = "wireloom-cs", ["RTI C#"] = "rti-cs", ["RTI C++"] = "rti-cpp" };
        var log = Path.Combine(directory, $"{id}-{fixture}-{peerIds[writer]}-{peerIds[reader]}.log");
        if (!File.Exists(log)) return "scenario log is missing";
        var lines = File.ReadAllLines(log);
        var notRun = lines.FirstOrDefault(l => l.StartsWith("NOT_RUN_DETAIL ", StringComparison.Ordinal));
        if (notRun is not null) return notRun["NOT_RUN_DETAIL ".Length..];
        if (status == "NOT_RUN") return "scenario did not run";
        var observations = lines.Where(l => l.StartsWith("FAIL_DETAIL ", StringComparison.Ordinal) || l.StartsWith("FAIL ", StringComparison.Ordinal))
            .Select(l => l.StartsWith("FAIL_DETAIL ", StringComparison.Ordinal) ? l[12..].TrimEnd('.') : l[5..].TrimEnd('.')).ToList();
        var endpointLine = lines.FirstOrDefault(l => l.StartsWith("ENDPOINT_STATUS ", StringComparison.Ordinal));
        var endpointFields = endpointLine?.Split(' ').Skip(1).Select(p => p.Split('=', 2)).Where(p => p.Length == 2).ToDictionary(p => p[0], p => p[1]) ?? [];
        foreach (var endpoint in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
            if (endpointFields.GetValueOrDefault($"{endpoint.Name}_exit_code") == "139") observations.Add($"{endpoint.Peer} {endpoint.Name} terminated with exit 139 (SIGSEGV)");
        if (observations.Count > 0) return string.Join("; ", observations.Distinct());
        if (lines.Any(l => l.StartsWith("RTI_DIAGNOSTIC ", StringComparison.Ordinal) && l.Contains("different type kinds", StringComparison.Ordinal)))
            return "RTI discovery rejected the type: different member type kinds";
        foreach (var endpoint in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
            if (endpointFields.TryGetValue($"{endpoint.Name}_exit_code", out var code) && code != "0") return $"{endpoint.Peer} {endpoint.Name} exited with code {code}";
        return "endpoint failed; see sanitized scenario log";
    }

    void WriteMarkdown(string rtiVersion, List<Dictionary<string, object?>> all, JsonArray unsupported, JsonArray failureRegistry, string destination)
    {
        var pairings = new[]
        {
            ("Wireloom C#", "Wireloom C#"), ("Wireloom C#", "RTI C#"), ("Wireloom C#", "RTI C++"),
            ("RTI C#", "Wireloom C#"), ("RTI C#", "RTI C#"), ("RTI C#", "RTI C++"),
            ("RTI C++", "Wireloom C#"), ("RTI C++", "RTI C#"), ("RTI C++", "RTI C++"),
        };
        string HeaderPeer(string peer) => peer.Replace("Wireloom", "🧵", StringComparison.Ordinal);
        string Cell(string id, string fixture, (string Writer, string Reader) pairing, Dictionary<string, object?>? item)
        {
            if (item is null) return "❔";
            var mark = item.ContainsKey("adaptations") ? "<sup>†</sup>" : "";
            var state = (string)item["status"]!;
            if (state == "PASS") return $"{(mark.Length > 0 ? "🟧" : "✅")}{mark}";
            if (state == "FAIL") return $"{(ExpectedFailure(id, pairing.Writer, pairing.Reader, fixture) is not null ? "⚠️" : "⛔")}{mark}";
            return $"❔{mark}";
        }
        var groups = all.GroupBy(item => ((string)item["case"]!, (string)item["fixture"]!)).ToDictionary(group => group.Key, group => group.ToDictionary(item => (((Dictionary<string, object?>)item["writer"]!)["peer"]!.ToString()!, ((Dictionary<string, object?>)item["reader"]!)["peer"]!.ToString()!)));
        var failed = all.Where(s => (string)s["status"]! == "FAIL").ToArray();
        var notRunCount = all.Count(s => (string)s["status"]! == "NOT_RUN");
        var passed = all.Count - failed.Length - notRunCount;
        var lines = new List<string>
        {
            "# Wire compatibility results", "",
            $"RTI Connext **{rtiVersion}** · **{all.Count}** DDS exchanges · **{passed}** passed · **{failed.Length}** failed (**{failed.Count(s => s.ContainsKey("expectedFailure"))}** expected) · **{notRunCount}** not run · **{unsupported.Count}** case(s) not implemented", "",
            "Each cell is an independent producer/consumer exchange. A `\"` in the IDL scenario column repeats the case from the row above. Failure details are taken from sanitized endpoint logs.", "",
            "| Legend | Meaning |",
            "| :-- | :-- |",
            "| ✅ | Exchange passed |",
            "| 🟧<sup>†</sup> | Exchange passed using a test-harness workaround or custom IDL/type model |",
            "| ⚠️ | Expected failure |",
            "| ⛔ | Unexpected failure |",
            "| ❔ | Not run |",
            "| ⚫ — | Not implemented |",
            "| <sup>†</sup> | At least one endpoint used an adaptation; details appear in Adaptations. |",
            "",
            "The problem column separates observed symptoms from registered expected-failure explanations. Symbols remain distinct without relying on color alone.", "",
            "| IDL scenario | Variation | " + string.Join(" | ", pairings.Select(p => $"{HeaderPeer(p.Item1)} → {HeaderPeer(p.Item2)}")) + " | Adaptations | Problem / defect |",
            "| :-- | :-- | " + string.Join(" | ", pairings.Select(_ => ":--:")) + " | :-- | :-- |",
        };
        var unsupportedById = unsupported.ToDictionary(item => (string)item!["id"]!, StringComparer.Ordinal);
        var reportRows = groups.Select(g => (g.Key.Item1, g.Key.Item2, g.Value, unsupportedById.GetValueOrDefault(g.Key.Item1)))
            .Concat(unsupported.Where(item => !groups.Keys.Any(key => key.Item1 == (string)item!["id"]!))
                .Select(item => ((string)item!["id"]!, "not implemented", new Dictionary<(string, string), Dictionary<string, object?>>(), (JsonNode?)item)))
            .OrderBy(row => row.Item1, StringComparer.Ordinal).ThenBy(row => row.Item2, StringComparer.Ordinal).ToArray();
        string? previous = null;
        foreach (var (id, fixture, row, unsupportedCase) in reportRows)
        {
            var cells = pairings.Select(pair => unsupportedCase is not null && (pair.Item1 == "Wireloom C#" || pair.Item2 == "Wireloom C#")
                ? "⚫ —" : Cell(id, fixture, pair, row.GetValueOrDefault(pair))).ToArray();
            var adaptationsInRow = row.Values.SelectMany(s => s.TryGetValue("adaptations", out var value) ? ((Dictionary<string, string>)value!).Select(kv => $"<sup>†</sup> **{kv.Key}:** {kv.Value}") : []).Distinct();
            var adaptationText = string.Join("<br><br>", adaptationsInRow).Replace("|", "\\|");
            var problems = new List<string>();
            foreach (var pair in pairings)
            {
                if (unsupportedCase is not null && (pair.Item1 == "Wireloom C#" || pair.Item2 == "Wireloom C#")) continue;
                var item = row.GetValueOrDefault(pair);
                if (item is null || (string)item["status"]! is "FAIL" or "NOT_RUN")
                {
                    var detail = item is null ? "scenario did not run" : (string?)item.GetValueOrDefault("observedProblem") ?? "scenario did not run";
                    var expected = ExpectedFailure(id, pair.Item1, pair.Item2, fixture);
                    var name = $"{pair.Item1} → {pair.Item2}";
                    problems.Add(expected is not null && item is not null && (string)item["status"]! == "FAIL"
                        ? $"**{name} observed:** {detail.Replace("|", "\\|")}<br>**Expected:** {expected.Replace("Expected: ", "").Replace("|", "\\|")}"
                        : $"**{name}:** {detail.Replace("|", "\\|")}");
                }
            }
            var problemParts = new List<string>();
            if (unsupportedCase is not null)
                problemParts.Add($"Not implemented by Wireloom: `{unsupportedCase["diagnostic"]}`. {unsupportedCase["reason"]} [#{unsupportedCase["issue"]!.ToString()!.TrimEnd('/').Split('/').Last()}]({unsupportedCase["issue"]})".Replace("|", "\\|"));
            problemParts.AddRange(problems);
            var problemText = problemParts.Count == 0 ? "—" : string.Join("<br><br>", problemParts);
            var caseCell = id == previous ? "\"" : $"`{id}.idl`";
            lines.Add($"| {caseCell} | `{fixture}` | {string.Join(" | ", cells)} | {(adaptationText.Length == 0 ? "—" : adaptationText)} | {problemText} |");
            previous = id;
        }
        File.WriteAllText(destination, string.Join("\n", lines) + "\n");
    }
}
