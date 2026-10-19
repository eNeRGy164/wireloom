// File-based utility for the wire-compatibility matrix. Requires the .NET 10 SDK.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using ReportFields = System.Collections.Generic.Dictionary<string, object?>;
using PeerPair = (string Writer, string Reader);
using FixtureKey = (string CaseId, string Fixture);
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;

const string usage = "Usage: matrix-tools.cs <list-cases|list-rti-only-cases|case-field|case-pairings|case-info|json-field|discover-cases|discover-rti|includes|includes-recursive|partition-shard|merge-shards|render-report|adapt-rti-characters> ...";
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
};
var keyJsonOptions = new JsonSerializerOptions(jsonOptions) { WriteIndented = false };

try
{
    if (args.Length == 0)
    {
        throw new ArgumentException(usage);
    }

    switch (args[0])
    {
        case "list-cases":
        {
            RequireArgs(3);
            var manifest = ReadJson(args[1]);
            var exclusions = ReadJson(args[2]);
            var unsupported = ExcludedWireloomCaseIds(exclusions);
            foreach (var item in manifest["positiveCases"]!.AsArray())
            {
                var id = (string)item!["id"]!;
                if (!unsupported.Contains(id))
                {
                    Console.WriteLine(id);
                }
            }
            break;
        }
        case "list-rti-only-cases":
        {
            RequireArgs(2);
            foreach (var item in ReadJson(args[1])["notImplementedCases"]!.AsArray())
            {
                Console.WriteLine((string)item!["id"]!);
            }

            break;
        }
        case "case-field":
        {
            RequireArgs(4);
            var item = FindCase(ReadJson(args[1]), args[2]);
            switch (args[3])
            {
                case "idl":
                    Console.WriteLine((string)item["idl"]!);
                    break;
                case "defines":
                    foreach (var define in item["defines"]?.AsArray() ?? [])
                    {
                        Console.WriteLine((string)define!);
                    }

                    break;
                case "rti-cpp-codegen-options":
                    foreach (var option in item["wireMatrix"]?["rtiCppCodegenOptions"]?.AsArray() ?? [])
                    {
                        Console.WriteLine((string)option!);
                    }

                    break;
                case "rti-cpp-idl-adaptations":
                    foreach (var adaptation in item["wireMatrix"]?["rtiCppIdlAdaptations"]?.AsArray() ?? [])
                    {
                        Console.WriteLine((string)adaptation!);
                    }

                    break;
                default:
                    throw new ArgumentException($"Unknown case field: {args[3]}");
            }
            break;
        }
        case "case-pairings":
        {
            RequireArgs(3);
            _ = FindCase(ReadJson(args[1]), args[2]);
            var peers = new[] { "Wireloom C#", "RTI C#", "RTI C++" };

            foreach (var writer in peers)
            {
                foreach (var reader in peers)
                {
                    Console.WriteLine($"{writer}|{reader}");
                }
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
            foreach (var define in corpusCase["defines"]?.AsArray() ?? [])
            {
                Console.WriteLine((string)define!);
            }
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
        case "adapt-rti-characters":
            AdaptRtiCharacters(args);
            break;
        case "render-report":
            RenderReport(args);
            break;
        case "partition-shard":
            PartitionShard(args);
            break;
        case "merge-shards":
            MergeShards(args);
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
    if (args.Length != count)
    {
        throw new ArgumentException(usage);
    }
}

// JSON and case selection helpers.
JsonNode ReadJson(string path) => JsonNode.Parse(File.ReadAllText(path))!;

/// <summary>Excludes implementation gaps and unavailable RTI features before planning Wireloom exchanges.</summary>
HashSet<string> ExcludedWireloomCaseIds(JsonNode exclusions) =>
    exclusions["notImplementedCases"]!.AsArray()
        .Concat(exclusions["notApplicableCases"]?.AsArray() ?? [])
        .Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);

// A shard owns whole cases. Greedy assignment by exchange count keeps all
// fixtures/pairings together and is deterministic across independent runners.
void PartitionShard(string[] commandArgs)
{
    RequireArgs(6);
    var version = commandArgs[1];
    var index = int.Parse(commandArgs[2]);
    var count = int.Parse(commandArgs[3]);
    var rows = ReadScenarioRows(commandArgs[4]);
    var destination = commandArgs[5];
    var cases = rows.GroupBy(row => row.CaseId, StringComparer.Ordinal).ToArray();
    if (count < 1 || count > cases.Length || index < 0 || index >= count)
    {
        throw new ArgumentException("Shard count must be positive and no greater than the case count; index must be zero-based and within the shard count.");
    }

    var loads = new int[count];
    var assignments = new Dictionary<string, int>(StringComparer.Ordinal);
    foreach (var item in cases.OrderByDescending(item => item.Count()).ThenBy(item => item.Key, StringComparer.Ordinal))
    {
        var shard = Enumerable.Range(0, count).OrderBy(shard => loads[shard]).ThenBy(shard => shard).First();
        assignments.Add(item.Key, shard);
        loads[shard] += item.Count();
    }

    var keys = rows.Select(ScenarioKey).ToArray();
    if (keys.Distinct(StringComparer.Ordinal).Count() != rows.Count)
    {
        throw new InvalidDataException("The scenario plan contains duplicate exchanges.");
    }

    Directory.CreateDirectory(destination);
    var plan = new
    {
        rtiVersion = version,
        shardCount = count,
        expectedScenarioCount = rows.Count,
        cases = cases.Select(item => new
        {
            id = item.Key,
            shard = assignments[item.Key],
            scenarioKeys = item.Select(ScenarioKey).ToArray()
        }).ToArray()
    };
    File.WriteAllText(Path.Combine(destination, "plan.json"), JsonSerializer.Serialize(plan, jsonOptions) + "\n");
    var selected = rows.Where(row => assignments[row.CaseId] == index).ToArray();
    File.WriteAllLines(Path.Combine(destination, "scenarios.tsv"), selected.Select(row => row.ToTsv()));
    File.WriteAllLines(Path.Combine(destination, "selected-cases.txt"), cases.Where(item => assignments[item.Key] == index).Select(item => item.Key));
    File.WriteAllText(Path.Combine(destination, "shard.json"), JsonSerializer.Serialize(new
    {
        rtiVersion = version,
        shardCount = count,
        shardIndex = index,
        expectedScenarioCount = selected.Length
    }, jsonOptions) + "\n");
}

// Regenerate the combined report from validated rows and sanitized logs. Do
// not concatenate JSON counters or repeat the global exclusion registry.
void MergeShards(string[] commandArgs)
{
    RequireArgs(6);
    var version = commandArgs[1];
    var count = int.Parse(commandArgs[2]);
    var source = commandArgs[3];
    var destination = commandArgs[4];
    var metadataFiles = Directory.GetFiles(source, "shard.json", SearchOption.AllDirectories);
    if (count < 1 || metadataFiles.Length != count)
    {
        throw new InvalidDataException($"Expected {count} shard artifacts; found {metadataFiles.Length}.");
    }

    JsonNode? sharedPlan = null;
    var indices = new HashSet<int>();
    var exchanges = new Dictionary<string, ScenarioRow>(StringComparer.Ordinal);
    var logFiles = new List<string>();
    foreach (var metadataFile in metadataFiles.Order(StringComparer.Ordinal))
    {
        var directory = Path.GetDirectoryName(metadataFile)!;
        var metadata = ReadJson(metadataFile);
        var index = (int)metadata["shardIndex"]!;
        var plan = ReadJson(Path.Combine(directory, "plan.json"));
        if ((string)metadata["rtiVersion"]! != version || (string)plan["rtiVersion"]! != version
            || (int)metadata["shardCount"]! != count || (int)plan["shardCount"]! != count
            || index < 0 || index >= count || !indices.Add(index))
        {
            throw new InvalidDataException("Shard identity or RTI version does not match, or a shard index is duplicated.");
        }

        if (sharedPlan is not null && !JsonNode.DeepEquals(sharedPlan, plan))
        {
            throw new InvalidDataException("Shard case plans differ.");
        }
        sharedPlan = plan;
        var expectedKeys = plan["cases"]!.AsArray().Where(item => (int)item!["shard"]! == index)
            .SelectMany(item => item!["scenarioKeys"]!.AsArray()).Select(item => (string)item!).ToHashSet(StringComparer.Ordinal);
        var rows = ReadScenarioRows(Path.Combine(directory, "scenarios.tsv"));
        var report = ReadJson(Path.Combine(directory, "results.json"));
        if ((int)metadata["expectedScenarioCount"]! != expectedKeys.Count || rows.Count != expectedKeys.Count
            || (int)report["expectedScenarioCount"]! != expectedKeys.Count || (int)report["scenarioCount"]! != rows.Count
            || (string)report["rtiVersion"]! != version)
        {
            throw new InvalidDataException($"Shard {index} has missing exchanges or inconsistent report counts.");
        }

        foreach (var row in rows)
        {
            var key = ScenarioKey(row);
            if (row.Status == "NOT_RUN")
            {
                throw new InvalidDataException($"Shard {index} contains an unexecuted exchange; incomplete evidence cannot be combined.");
            }
            if (!expectedKeys.Contains(key) || row.WriterVersion != version || row.ReaderVersion != version
                || row.Status is not ("PASS" or "FAIL") || !exchanges.TryAdd(key, row))
            {
                throw new InvalidDataException($"Shard {index} contains an unexpected, duplicate, or invalid exchange.");
            }
        }

        var logs = Path.Combine(directory, "logs");
        if (Directory.Exists(logs))
        {
            logFiles.AddRange(Directory.GetFiles(logs, "*.log"));
        }
    }

    var orderedKeys = sharedPlan!["cases"]!.AsArray().SelectMany(item => item!["scenarioKeys"]!.AsArray())
        .Select(item => (string)item!).ToArray();
    var expectedCount = (int)sharedPlan["expectedScenarioCount"]!;
    if (orderedKeys.Length != expectedCount || exchanges.Count != expectedCount
        || orderedKeys.Distinct(StringComparer.Ordinal).Count() != expectedCount)
    {
        throw new InvalidDataException("Combined shards do not cover the complete scenario plan exactly once.");
    }

    Directory.CreateDirectory(destination);
    var logsDestination = Path.Combine(destination, "logs");
    Directory.CreateDirectory(logsDestination);
    foreach (var file in logFiles)
    {
        File.Copy(file, Path.Combine(logsDestination, Path.GetFileName(file)), overwrite: false);
    }
    var rowsDestination = Path.Combine(destination, "scenarios.tsv");
    File.WriteAllLines(rowsDestination, orderedKeys.Select(key => exchanges[key].ToTsv()));
    File.WriteAllText(Path.Combine(destination, "plan.json"), sharedPlan.ToJsonString(jsonOptions) + "\n");
    RenderReport(["render-report", version, expectedCount.ToString(), rowsDestination,
        Path.Combine(destination, "results.json"), Path.Combine(destination, "results.md"), logsDestination, commandArgs[5],
        Path.Combine(FindRepositoryRoot(commandArgs[5]), "docs", "corpus", "manifest.json")]);
    Console.WriteLine($"Merged {count} shards: {expectedCount} exchanges verified exactly once.");
}

List<ScenarioRow> ReadScenarioRows(string path)
{
    return File.ReadLines(path)
        .Where(line => line.Length > 0)
        .Select(ScenarioRow.Parse)
        .ToList();
}

string ScenarioKey(ScenarioRow row) => JsonSerializer.Serialize(new[] { row.CaseId, row.Fixture, row.Writer, row.Reader }, keyJsonOptions);

JsonNode FindCase(JsonNode manifest, string id) => manifest["positiveCases"]!.AsArray()
    .FirstOrDefault(item => (string)item!["id"]! == id)
    ?? throw new InvalidOperationException($"No positive corpus case found for {id}.");

// Discover generated topic types and their IDL include closure.
void DiscoverCases(string[] commandArgs)
{
    if (commandArgs.Length is not (4 or 5))
    {
        throw new ArgumentException(usage);
    }

    var manifestPath = Path.GetFullPath(commandArgs[1]);
    var generatedRoot = Path.GetFullPath(commandArgs[2]);
    var outputPath = Path.GetFullPath(commandArgs[3]);
    var requestedCase = commandArgs.Length == 5 ? commandArgs[4] : null;
    var rtiVersion = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0";
    var exclusionsName = $"wire-case-exclusions-{rtiVersion}.json";
    var exclusionsPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, exclusionsName);

    if (!File.Exists(exclusionsPath))
    {
        // The script is cached in the SDK temp directory, so locate exclusions relative to the manifest/repository.
        var repoRoot = FindRepositoryRoot(manifestPath);
        exclusionsPath = Path.Combine(repoRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", exclusionsName);
    }

    var manifest = ReadJson(manifestPath);
    var unsupported = ExcludedWireloomCaseIds(ReadJson(exclusionsPath));
    var selected = manifest["positiveCases"]!.AsArray()
        .Where(item => !unsupported.Contains((string)item!["id"]!))
        .Where(item => requestedCase is null || (string)item!["id"]! == requestedCase)
        .ToArray();

    if (requestedCase is not null && selected.Length == 0)
    {
        throw new InvalidOperationException($"Case {requestedCase} is not a wire-testable Wireloom positive case.");
    }

    var byIdl = selected.ToDictionary(item => Path.GetFileName((string)item!["idl"]!), item => (string)item!["id"]!, StringComparer.Ordinal);
    var candidates = selected.ToDictionary(item => (string)item!["id"]!, _ => new List<(string Cs, string Cpp)>(), StringComparer.Ordinal);

    foreach (var source in Directory.Exists(generatedRoot) ? Directory.EnumerateFiles(generatedRoot, "*.g.cs", SearchOption.AllDirectories) : [])
    {
        var text = File.ReadAllText(source);
        var header = Regex.Match(text, "generated by Wireloom from '([^']+)'");
        var support = Regex.Match(text, @"class\s+\w+\s*:\s*TypeSupport<\s*([\w.]+)\s*>");
        if (!header.Success || !support.Success || !byIdl.TryGetValue(header.Groups[1].Value, out var id))
        {
            continue;
        }

        var namespaceMatch = Regex.Match(text, @"^namespace\s+([\w.]+)\s*;", RegexOptions.Multiline);
        var type = support.Groups[1].Value;
        if (namespaceMatch.Success && !type.Contains('.'))
        {
            type = $"{namespaceMatch.Groups[1].Value}.{type}";
        }

        candidates[id].Add((type, "::" + type.Replace(".", "::", StringComparison.Ordinal)));
    }

    var results = new List<object>();
    foreach (var item in selected)
    {
        var id = (string)item!["id"]!;
        if (candidates[id].Count == 0)
        {
            throw new InvalidOperationException($"No generated type support found for positive case {id}.");
        }

        var preferred = new Dictionary<string, int>(StringComparer.Ordinal) { ["Sample"] = 0, ["Composed"] = 1 };
        var selectedType = id switch
        {
            "03-enums-aliases" => "Record",
            "02-multiple" => "WireloomWireCompatibilityMultipleTypes",
            "03-enum-values-prefix" or "03-enum-values-explicit" => "EnumTopic",
            "09-optional-recursive-array" => "Node",
            "09-optional-aggregate-member" => "Holder",
            "07-union-aliases" or "06-valuetypes" => "Holder",
            "04-unbounded-string-aliases" => "StringAliasesSample",
            "05-unbounded-sequence-aliases" => "SequenceAliasesSample",
            "04-string-alias-chains" => "StringAliasChainsSample",
            "07-union-string-sequence-aliases" => "StringSequenceUnionsSample",
            "07-union-char-numeric" => "Choice",
            "07-union-char-numeric-255" => "Choice",
            "07-union-string-sequence-bounded-direct" => "DirectBoundedStringSequences",
            "08-key-inherited" => "Derived",
            "08-key-nested" => "Outer",
            _ => null,
        };

        if (selectedType is not null)
        {
            preferred[selectedType] = -1;
        }

        var (Cs, Cpp) = candidates[id]
            .OrderBy(candidate => preferred.GetValueOrDefault(candidate.Cs.Split('.').Last(), 2))
            .ThenBy(candidate => candidate.Cs, StringComparer.Ordinal)
            .First();

        results.Add(new { id, csharpType = Cs, cppType = Cpp });
    }

    Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new { cases = results }, jsonOptions) + "\n");

    Console.WriteLine($"Discovered {results.Count} Wireloom positive case type(s).");
}

void DiscoverRti(string[] commandArgs)
{
    if (commandArgs.Length != 6)
    {
        throw new ArgumentException(usage);
    }

    var (manifestPath, caseId, wireloomType, sourceDir, outputPath) = (commandArgs[1], commandArgs[2], commandArgs[3], Path.GetFullPath(commandArgs[4]), commandArgs[5]);

    _ = ReadJson(manifestPath);

    var types = Directory.EnumerateFiles(sourceDir, "*.cs", SearchOption.AllDirectories)
        .SelectMany(path => Regex.Matches(File.ReadAllText(path), @"class\s+\w+Support\s*:\s*Rti\.Dds\.Topics\.TypeSupport<\s*global::([\w.]+)\s*>")
            .Select(match => match.Groups[1].Value)).ToList();

    if (types.Count == 0)
    {
        throw new InvalidOperationException($"No RTI C# TypeSupport was generated for {caseId}.");
    }

    var preferred = new Dictionary<string, int>(StringComparer.Ordinal) { ["Sample"] = 0, ["Composed"] = 1 };
    var selected = caseId switch
    {
        "03-enums-aliases" => "Record",
        "09-optional-recursive-array" => "Node",
        "09-optional-aggregate-member" => "Holder",
        "07-union-aliases" or "06-valuetypes" => "Holder",
        "04-unbounded-string-aliases" => "StringAliasesSample",
        "05-unbounded-sequence-aliases" => "SequenceAliasesSample",
        "04-string-alias-chains" => "StringAliasChainsSample",
        "07-union-string-sequence-aliases" => "StringSequenceUnionsSample",
        "07-union-char-numeric" => "Choice",
        "07-union-char-numeric-255" => "Choice",
        "07-union-string-sequence-bounded-direct" => "DirectBoundedStringSequences",
        "08-key-inherited" => "Derived",
        "08-key-nested" => "Outer",
        _ => null,
    };

    if (selected is not null)
    {
        preferred[selected] = -1;
    }

    var preferredName = wireloomType.Split('.').Last();
    var selectedType = types
        .OrderBy(type => type.Split('.').Last() == preferredName ? 0 : 1)
        .ThenBy(type => preferred.GetValueOrDefault(type.Split('.').Last(), 2))
        .ThenBy(type => type, StringComparer.Ordinal)
        .First();

    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);

    File.WriteAllText(outputPath, JsonSerializer.Serialize(new { id = caseId, sourceDir, rtiCSharpType = selectedType }, jsonOptions) + "\n");
}

void DiscoverIncludes(string[] commandArgs)
{
    if (commandArgs.Length != 4)
    {
        throw new ArgumentException(usage);
    }

    var rootIdl = Path.GetFullPath(commandArgs[1]);
    var roots = commandArgs.Skip(2).Select(Path.GetFullPath).ToArray();
    var includeRegex = new Regex("^\\s*#\\s*include\\s*[<\\\"]([^>\\\"]+)[>\\\"]", RegexOptions.Multiline);

    foreach (Match match in includeRegex.Matches(File.ReadAllText(rootIdl)))
    {
        var include = match.Groups[1].Value;
        var candidates = new[] { Path.Combine(Path.GetDirectoryName(rootIdl)!, include) }
            .Concat(roots.Select(root => Path.Combine(root, include)));

        var source = candidates.FirstOrDefault(File.Exists);
        if (source is not null)
        {
            Console.WriteLine($"{include}\t{Path.GetFullPath(source)}");
        }
    }
}

void DiscoverIncludesRecursive(string[] commandArgs)
{
    if (commandArgs.Length != 4)
    {
        throw new ArgumentException(usage);
    }

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
            if (source is null)
            {
                continue;
            }

            source = Path.GetFullPath(source);

            if (!discovered.Add(source))
            {
                continue;
            }

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
        if (Directory.Exists(Path.Combine(directory.FullName, ".git")) || File.Exists(Path.Combine(directory.FullName, ".git")))
        {
            return directory.FullName;
        }

        directory = directory.Parent;
    }

    throw new DirectoryNotFoundException("Could not locate repository root.");
}

// Build structured evidence first, then render the same scenarios as Markdown.
void RenderReport(string[] commandArgs)
{
    if (commandArgs.Length != 9)
    {
        throw new ArgumentException(usage);
    }
    var version = commandArgs[1];
    var expectedCount = int.Parse(commandArgs[2]);
    var sourcePath = commandArgs[3];
    var outputPath = commandArgs[4];
    var markdownPath = commandArgs[5];
    var logsPath = commandArgs[6];
    var exclusions = ReadJson(commandArgs[7]);
    var manifest = ReadJson(commandArgs[8]);
    var notImplemented = exclusions["notImplementedCases"]!.AsArray();
    var notApplicable = exclusions["notApplicableCases"]?.AsArray() ?? [];
    var unavailableIds = notApplicable.Select(item => (string)item!["id"]!).ToHashSet(StringComparer.Ordinal);
    var wireMatrixConfiguration = manifest["positiveCases"]!.AsArray()
        .Where(item => item!["wireMatrix"] is not null)
        .ToDictionary(item => (string)item!["id"]!, item => item!["wireMatrix"]!, StringComparer.Ordinal);
    var scenarios = new List<ReportFields>();
    var peerData = new Dictionary<string, (string Implementation, string Language)>
    {
        ["Wireloom C#"] = ("Wireloom", "C#"),
        ["RTI C#"] = ("RTI", "C#"),
        ["RTI C++"] = ("RTI", "C++"),
    };
    foreach (var row in ReadScenarioRows(sourcePath))
    {
        var (id, wireType, rtiType, cppType, writer, writerVersion, reader, readerVersion, status, fixture) = row;
        if (unavailableIds.Contains(id))
        {
            throw new InvalidDataException($"Case {id} is not applicable to RTI {version} and cannot contribute exchanges.");
        }

        ReportFields Endpoint(string peer, string peerVersion)
        {
            var adaptation = Adaptation(id, fixture, peer, peerVersion);
            var data = peerData[peer];
            var endpoint = new ReportFields
            {
                ["peer"] = peer,
                ["implementation"] = data.Implementation,
                ["language"] = data.Language,
                ["rtiVersion"] = peerVersion,
                ["mode"] = EndpointMode(peer, adaptation),
            };
            if (adaptation is not null)
            {
                endpoint["adaptation"] = adaptation;
            }

            return endpoint;
        }
        var writerEndpoint = Endpoint(writer, writerVersion);
        var readerEndpoint = Endpoint(reader, readerVersion);
        var scenario = new ReportFields
        {
            ["case"] = id,
            ["writer"] = writerEndpoint,
            ["reader"] = readerEndpoint,
            ["fixture"] = fixture,
            ["status"] = status,
        };
        if (wireMatrixConfiguration.TryGetValue(id, out var matrixConfiguration))
        {
            scenario["matrixConfiguration"] = matrixConfiguration;
        }
        var adaptations = new Dictionary<string, string>();
        foreach (var endpoint in new[] { writerEndpoint, readerEndpoint })
        {
            if (endpoint.TryGetValue("adaptation", out var value))
            {
                adaptations[(string)endpoint["peer"]!] = (string)value!;
            }
        }

        if (adaptations.Count > 0)
        {
            scenario["adaptations"] = adaptations;
        }

        if (wireType.Length + rtiType.Length + cppType.Length > 0)
        {
            scenario["types"] = new Dictionary<string, string>
            {
                ["Wireloom C#"] = wireType,
                ["RTI C#"] = rtiType,
                ["RTI C++"] = cppType,
            };
        }

        if (status is "FAIL" or "NOT_RUN")
        {
            scenario["observedProblem"] = ObservedProblem(id, fixture, writer, reader, status, logsPath);
        }

        scenarios.Add(scenario);
    }

    var report = new ReportFields
    {
        ["rtiVersion"] = version,
        ["expectedScenarioCount"] = expectedCount,
        ["scenarioCount"] = scenarios.Count,
        ["fixtureCount"] = scenarios.Select(scenario => (scenario["case"], scenario["fixture"])).Distinct().Count(),
        ["notImplementedCount"] = notImplemented.Count,
        ["adaptedScenarioCount"] = scenarios.Count(scenario => scenario.ContainsKey("adaptations")),
        ["notApplicableCount"] = notApplicable.Count,
        ["notApplicableCases"] = notApplicable,
        ["notImplementedCases"] = notImplemented,
        ["scenarios"] = scenarios,
    };
    File.WriteAllText(outputPath, JsonSerializer.Serialize(report, jsonOptions) + "\n");
    WriteMarkdown(version, scenarios, notImplemented, notApplicable, markdownPath);
}

// Report modes describe the actual endpoint setup, independently of PASS/FAIL.
string EndpointMode(string peer, string? adaptation)
{
    if (adaptation is not null)
    {
        return "custom-test-adaptation";
    }

    if (peer == "Wireloom C#")
    {
        return "wireloom-generated";
    }

    return "rti-default";
}

string? Adaptation(string id, string fixture, string peer, string peerVersion)
{
    if (id == "10-resolve-name-wstring-aliases" && peer == "RTI C#")
    {
        return "Post-generation namespace correction: qualify both unmanaged alias types with the IDL module.";
    }

    if (id == "10-resolve-name-wstring-aliases" && peer == "RTI C++")
    {
        return "Post-generation TypeCode correction: use the generated, module-qualified alias-tag types for both wstring members.";
    }

    if (id == "09-optional-recursive-array")
    {
        var adaptation = "Disable TypeObject/TypeCode propagation.";
        adaptation += " Writer dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size=4096 bytes; larger samples allocate separately.";
        if (peerVersion == "7.3.1")
        {
            adaptation += " Avoids RTI 7.3.1 preallocating about 64 GiB.";
        }
        if (peer == "RTI C#")
        {
            adaptation += "\nBootstrap plugin before Support.Instance to avoid recursive initialization NullReferenceException.";
        }
        else if (peer == "RTI C++")
        {
            adaptation += "\nForward the visited-node chain during TypeCode initialization.";
        }

        return adaptation;
    }

    if (id == "06-alias-composition" && peer == "RTI C#")
    {
        return "Qualify the conversion receiver to fix shadowed member access.";
    }

    if (id is "07-union-char-numeric" or "07-union-char-numeric-255")
    {
        if (peer == "RTI C#")
        {
            var adaptation = "Post-generation RTI C# correction: add char casts; preserve fallback discriminator with Setfallback.";
            if (id == "07-union-char-numeric-255")
            {
                adaptation += " UnionMember label: 255 → -1; public 255 and wire 0xFF unchanged.";
            }

            return adaptation;
        }

        if (peer == "RTI C++" && id == "07-union-char-numeric-255")
        {
            return "Compile with -funsigned-char for label 255. TypeCode label: 255 → -1; public 255, wire 0xFF, and payload 13702 unchanged. Fixes zero-payload serialization and type matching.";
        }
    }

    if (peer != "RTI C++")
    {
        return null;
    }

    if (id == "09-optional-string-sequences" && fixture is ("optional-wide-only" or "optional-multiple"))
    {
        return "DynamicData C API element setters bypass the typed optional wide-string sequence serializer.";
    }

    if (id == "06-aggregates" && peerVersion == "7.3.1")
    {
        return "Qualify the generated Recursive TypeCode reference with its IDL module.";
    }

    if (id == "07-union-wchar-label")
    {
        return "Manual DynamicType reference; rtiddsgen rejects the wchar discriminator.";
    }

    if (id == "10-flat-data-binding")
    {
        return "Plain C++ reference: remove @language_binding(FLAT_DATA); canonical extensibility is incompatible with FlatData. FlatData layout is unqualified.";
    }
    return null;
}

string ObservedProblem(string id, string fixture, string writer, string reader, string status, string directory)
{
    var peerIds = new Dictionary<string, string>
    {
        ["Wireloom C#"] = "wireloom-cs",
        ["RTI C#"] = "rti-cs",
        ["RTI C++"] = "rti-cpp",
    };
    var log = Path.Combine(directory, $"{id}-{fixture}-{peerIds[writer]}-{peerIds[reader]}.log");
    if (!File.Exists(log))
    {
        return "scenario log is missing";
    }

    var lines = File.ReadAllLines(log);
    var notRun = lines.FirstOrDefault(l => l.StartsWith("NOT_RUN_DETAIL ", StringComparison.Ordinal));
    if (notRun is not null)
    {
        return notRun["NOT_RUN_DETAIL ".Length..];
    }

    if (status == "NOT_RUN")
    {
        return "scenario did not run";
    }

    var observations = new List<string>();
    foreach (var line in lines)
    {
        if (line.StartsWith("FAIL_DETAIL ", StringComparison.Ordinal))
        {
            observations.Add(line["FAIL_DETAIL ".Length..].TrimEnd('.'));
        }
        else if (line.StartsWith("FAIL ", StringComparison.Ordinal))
        {
            observations.Add(line["FAIL ".Length..].TrimEnd('.'));
        }
    }

    var endpointFields = new Dictionary<string, string>();
    var endpointLine = lines.FirstOrDefault(line => line.StartsWith("ENDPOINT_STATUS ", StringComparison.Ordinal));
    if (endpointLine is not null)
    {
        foreach (var field in endpointLine.Split(' ').Skip(1))
        {
            var parts = field.Split('=', 2);
            if (parts.Length == 2)
            {
                endpointFields.Add(parts[0], parts[1]);
            }
        }
    }

    foreach (var endpoint in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
    {
        if (endpointFields.GetValueOrDefault($"{endpoint.Name}_exit_code") == "139")
        {
            observations.Add($"{endpoint.Peer} {endpoint.Name} terminated with exit 139 (SIGSEGV)");
        }
    }

    if (observations.Count > 0)
    {
        return string.Join("; ", observations.Distinct());
    }

    if (lines.Any(l => l.StartsWith("RTI_DIAGNOSTIC ", StringComparison.Ordinal) && l.Contains("different type kinds", StringComparison.Ordinal)))
    {
        return "RTI discovery rejected the type: different member type kinds";
    }

    foreach (var endpoint in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
    {
        if (endpointFields.TryGetValue($"{endpoint.Name}_exit_code", out var code) && code != "0")
        {
            return $"{endpoint.Peer} {endpoint.Name} exited with code {code}";
        }
    }

    return "endpoint failed; see sanitized scenario log";
}

bool UsesWireloom(PeerPair pairing)
{
    return pairing.Writer == "Wireloom C#" || pairing.Reader == "Wireloom C#";
}

// Peer IDs remain stable in JSON and exclusion registries; labels can be shorter.
string DisplayPeer(string peer)
{
    if (peer == "Wireloom C#")
    {
        return "Wireloom";
    }

    return peer;
}

string WorkaroundLink(string? caseId = null)
{
    var anchor = caseId switch
    {
        "09-optional-string-sequences" => "optional-wide-string-sequences",
        "06-alias-composition" => "shadowed-c-member",
        "07-union-char-numeric" or "07-union-char-numeric-255" => "numeric-character-unions",
        "09-optional-recursive-array" => "recursive-metadata-propagation",
        "06-aggregates" => "recursive-type-generation",
        "07-union-wchar-label" => "wide-character-union-reference",
        "10-flat-data-binding" => "flatdata-reference-input",
        "10-resolve-name-wstring-aliases" => "wstring-alias-typecode-tags",
        _ => null,
    };
    var server = Environment.GetEnvironmentVariable("GITHUB_SERVER_URL") ?? "https://github.com";
    var repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? "eNeRGy164/wireloom";
    var revision = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "main";
    var url = $"{server}/{repository}/blob/{revision}/tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md";
    if (anchor is not null)
    {
        url += $"#{anchor}";
    }

    return $"[Workaround guide]({url})";
}

string FormatExplanation(string text)
{
    text = text.Replace("Wireloom C#", "Wireloom", StringComparison.Ordinal);
    text = text.Replace("value/ranged", "`value`/`ranged`", StringComparison.Ordinal);
    // Preserve existing Markdown and URLs. Format identifiers and values outside
    // those spans; version numbers and issue IDs are not numeric code values.
    const string tokens = """
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

void WriteMarkdown(string rtiVersion, List<ReportFields> scenarios, JsonArray unsupported, JsonArray notApplicable, string destination)
{
    PeerPair[] pairings =
    {
        ("Wireloom C#", "Wireloom C#"), ("Wireloom C#", "RTI C#"), ("Wireloom C#", "RTI C++"),
        ("RTI C#", "Wireloom C#"), ("RTI C#", "RTI C#"), ("RTI C#", "RTI C++"),
        ("RTI C++", "Wireloom C#"), ("RTI C++", "RTI C#"), ("RTI C++", "RTI C++"),
    };

    string FormatScenarioCell(string id, string fixture, PeerPair pairing, ReportFields? item)
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

    var failed = scenarios.Where(scenario => (string)scenario["status"]! == "FAIL").ToArray();
    var notRunCount = scenarios.Count(scenario => (string)scenario["status"]! == "NOT_RUN");
    var passed = scenarios.Count - failed.Length - notRunCount;
    var configuredCases = scenarios.Where(item => item.ContainsKey("matrixConfiguration"))
        .Select(item => (string)item["case"]!).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    var lines = new List<string>
    {
        $"# {rtiVersion} Wire compatibility results", "",
        $"**{scenarios.Count}** DDS exchanges · **{passed}** passed · **{failed.Length}** failed · **{notRunCount}** not run · **{unsupported.Count}** case(s) not implemented · **{notApplicable.Count}** case(s) not applicable", "",
        "Each cell is a writer → reader exchange. A `\"` repeats the IDL from the previous row. Observed errors are quoted from sanitized logs.", "",
        "| Legend | Meaning |",
        "| :-- | :-- |",
        "| ✅ | Exchange passed |",
        "| 🟧<sup>†</sup> | Passed with an adaptation |",
        "| ⛔ | Failed |",
        "| ❔ | Not run |",
        "| ⚫ | Not implemented |",
        "",
        "Failure details show the observed symptom.", "",
        $"{WorkaroundLink()} explains the default paths and applied code changes.", "",
    };
    if (notApplicable.Count > 0)
    {
        lines.AddRange([
            "## Not applicable",
            "",
            "Unavailable features are N/A and excluded from exchange counts.",
            "",
            "| Case | Reason |",
            "| :-- | :-- |",
        ]);
        foreach (var item in notApplicable)
        {
            var reason = FormatExplanation((string)item!["reason"]!);
            lines.Add($"| `{item["id"]}` | {reason} |");
        }

        lines.Add("");
    }

    if (configuredCases.Length > 0)
    {
        lines.AddRange(["## Case-specific RTI C++ generation", ""]);
        foreach (var id in configuredCases)
        {
            var configuration = (JsonObject)scenarios.First(item => (string)item["case"]! == id)["matrixConfiguration"]!;
            var options = (configuration["rtiCppCodegenOptions"]?.AsArray() ?? [])
                .Select(option => $"`{(string)option!}`");
            var idlAdaptations = (configuration["rtiCppIdlAdaptations"]?.AsArray() ?? [])
                .Select(adaptation => $"`{(string)adaptation!}`");
            var qualification = (string?)configuration["qualification"] ?? "";
            var generation = options.Any() ? $"generation adds {string.Join(", ", options)}" : "uses the default generation flags";
            var adaptation = idlAdaptations.Any() ? $" IDL adaptations: {string.Join(", ", idlAdaptations)}." : "";
            lines.Add($"- **{id}**: RTI C++ {generation}.{adaptation} {qualification} {WorkaroundLink(id)}".TrimEnd());
        }
        lines.Add("");
    }
    lines.AddRange([
        "| IDL scenario | Variation | " + string.Join(" | ", pairings.Select(p => $"{DisplayPeer(p.Writer)} → {DisplayPeer(p.Reader)}")) + " | Adaptations | Problem / defect |",
        "| :-- | :-- | " + string.Join(" | ", pairings.Select(_ => ":--:")) + " | :-- | :-- |",
    ]);
    var unsupportedById = unsupported.ToDictionary(item => (string)item!["id"]!, StringComparer.Ordinal);
    var reportRows = new List<(string CaseId, string Fixture, Dictionary<PeerPair, ReportFields> Results, JsonNode? Unsupported)>();
    foreach (var (fixtureKey, results) in groups)
    {
        reportRows.Add((fixtureKey.CaseId, fixtureKey.Fixture, results, unsupportedById.GetValueOrDefault(fixtureKey.CaseId)));
    }

    // Implementation gaps still need a row when no peer exchanges were planned.
    foreach (var unsupportedCase in unsupported)
    {
        var caseId = (string)unsupportedCase!["id"]!;
        if (!groups.Keys.Any(key => key.CaseId == caseId))
        {
            reportRows.Add((caseId, "not implemented", new Dictionary<PeerPair, ReportFields>(), unsupportedCase));
        }
    }

    var orderedRows = reportRows
        .OrderBy(row => row.CaseId, StringComparer.Ordinal)
        .ThenBy(row => row.Fixture, StringComparer.Ordinal);
    string? previous = null;
    foreach (var (id, fixture, row, unsupportedCase) in orderedRows)
    {
        var cells = new List<string>();
        foreach (var pairing in pairings)
        {
            if (unsupportedCase is not null && UsesWireloom(pairing))
            {
                cells.Add("⚫ —");
            }
            else
            {
                cells.Add(FormatScenarioCell(id, fixture, pairing, row.GetValueOrDefault(pairing)));
            }
        }

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
        var problems = new List<(string Pairing, string Detail)>();
        foreach (var pair in pairings)
        {
            if (unsupportedCase is not null && UsesWireloom(pair))
            {
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
            problemParts.Add($"Not implemented by Wireloom: `{unsupportedCase["diagnostic"]}`. {unsupportedCase["reason"]} [#{unsupportedCase["issue"]!.ToString()!.TrimEnd('/').Split('/').Last()}]({unsupportedCase["issue"]})".Replace("|", "\\|"));
        }

        foreach (var problem in problems.GroupBy(item => item.Detail))
        {
            var names = string.Join(", ", problem.Select(item => item.Pairing));
            var detail = problem.Key.Replace("|", "\\|");
            problemParts.Add($"**{names}:** “{detail}”");
        }
        var problemText = problemParts.Count == 0 ? "—" : string.Join("<br><br>", problemParts);
        var caseCell = id == previous ? "\"" : $"`{id}.idl`";
        lines.Add($"| {caseCell} | `{fixture}` | {string.Join(" | ", cells)} | {(adaptationText.Length == 0 ? "—" : adaptationText)} | {problemText} |");
        previous = id;
    }

    File.WriteAllText(destination, string.Join("\n", lines) + "\n");
}

// Apply only known reference-binding edits; reject unexpected generated source.
void AdaptRtiCharacters(string[] commandArgs)
{
    RequireArgs(4);
    var language = commandArgs[1];
    var caseId = commandArgs[2];
    var directory = commandArgs[3];
    var label = caseId switch
    {
        "07-union-char-numeric" => "127",
        "07-union-char-numeric-255" => "255",
        _ => throw new ArgumentException($"No character reference adaptation is defined for '{caseId}'.")
    };

    if (language == "cpp")
    {
        if (label != "255")
        {
            throw new ArgumentException("Only label 255 needs a C++ TypeCode adaptation.");
        }

        var path = Path.Combine(directory, "idl.cxx");
        var source = File.ReadAllText(path);
        source = ReplaceReferenceText(source, "static_cast<int>(255)", "static_cast<int>(-1)", count: 1);
        File.WriteAllText(path, source);
    }
    else if (language == "csharp")
    {
        var samplePath = Path.Combine(directory, $"{caseId}.cs");
        var pluginPath = Path.Combine(directory, $"{caseId}Plugin.cs");
        var sample = File.ReadAllText(samplePath);
        var plugin = File.ReadAllText(pluginPath);

        // Validate every expected edit before writing either generated file.
        sample = ReplaceReferenceText(sample, $"Discriminator = {label};", $"Discriminator = (char){label};", count: 1);
        sample = ReplaceReferenceText(sample, $"case {label}:", $"case (char){label}:", count: 4);
        plugin = ReplaceReferenceText(plugin, $"case {label}:", $"case (char){label}:", count: 2);
        plugin = ReplaceReferenceText(plugin, "sample.fallback = fallback;",
            "sample.Setfallback(fallback, NativeChar.FromUtf8(_d));", count: 1);
        if (label == "255")
        {
            plugin = ReplaceReferenceText(plugin, "new int[] {(int) 255}", "new int[] { -1 }", count: 1);
        }

        File.WriteAllText(samplePath, sample);
        File.WriteAllText(pluginPath, plugin);
    }
    else
    {
        throw new ArgumentException($"Unknown reference language '{language}'.");
    }

    Console.WriteLine($"RTI character reference adapted: {language}, {caseId}.");
}

string ReplaceReferenceText(string source, string original, string replacement, int count)
{
    var originalCount = source.Split(original, StringSplitOptions.None).Length - 1;
    var replacementCount = source.Split(replacement, StringSplitOptions.None).Length - 1;
    if (originalCount == 0 && replacementCount == count)
    {
        return source;
    }

    if (originalCount != count || replacementCount != 0)
    {
        throw new InvalidDataException(
            $"RTI generated reference shape changed: expected {count} occurrences of '{original}', found {originalCount}.");
    }

    return source.Replace(original, replacement, StringComparison.Ordinal);
}

/// <summary>Names the ten fields shared by the shell runner, shard planner, and reports.</summary>
internal sealed record ScenarioRow(
    string CaseId,
    string WireloomType,
    string RtiCSharpType,
    string RtiCppType,
    string Writer,
    string WriterVersion,
    string Reader,
    string ReaderVersion,
    string Status,
    string Fixture)
{
    /// <summary>Reads the stable tab-separated interchange format used by run-matrix.sh.</summary>
    internal static ScenarioRow Parse(string line)
    {
        var fields = line.Split('\t');
        if (fields.Length != 10)
        {
            throw new InvalidDataException("Scenario TSV row must contain 10 fields.");
        }

        return new ScenarioRow(fields[0], fields[1], fields[2], fields[3], fields[4],
            fields[5], fields[6], fields[7], fields[8], fields[9]);
    }

    /// <summary>Writes fields in the same order as the shell runner.</summary>
    internal string ToTsv() => string.Join('\t',
        CaseId, WireloomType, RtiCSharpType, RtiCppType, Writer, WriterVersion,
        Reader, ReaderVersion, Status, Fixture);
}
