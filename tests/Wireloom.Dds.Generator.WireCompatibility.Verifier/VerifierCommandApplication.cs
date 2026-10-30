using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Wireloom.Dds.Generator.WireCompatibility.Runner;
using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;
using PeerPair = (string Writer, string Reader);

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier;

/// <summary>Provides the verifier's line-oriented compatibility commands.</summary>
internal static class VerifierCommandApplication
{
    internal static int Run(string[] args)
    {
        const string usage = "Usage: Wireloom.Dds.Generator.WireCompatibility.Verifier <repository-root|list-cases|case-field|case-pairings|discover-cases|generation-provenance|partition-shard|merge-shards|render-report|adapt-rti-characters> ...";
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
                case "repository-root":
                    RequireArgs(2);
                    Console.WriteLine(FindRepositoryRoot(args[1]));
                    break;
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
                case "case-field":
                {
                    RequireArgs(4);
                    var metadata = CaseMetadata.Read(FindCase(ReadJson(args[1]), args[2]));
                    foreach (var value in metadata.FieldLines(args[3]))
                    {
                        Console.WriteLine(value);
                    }
                    break;
                }
                case "case-pairings":
                {
                    if (args.Length is not (3 or 4))
                    {
                        throw new ArgumentException(usage);
                    }

                    var item = FindCase(ReadJson(args[1]), args[2]);
                    foreach (var (writer, reader) in CasePairings(item, args.Length == 4 ? args[3] : null))
                    {
                        Console.WriteLine($"{writer}|{reader}");
                    }
                    break;
                }
                case "discover-cases":
                    DiscoverCases(args);
                    break;
                case "generation-provenance":
                    WriteGenerationProvenance(args);
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
        JsonNode ReadJson(string path)
        {
            return JsonNode.Parse(File.ReadAllText(path))!;
        }

        // Preserves the standalone tuple contract through the shared pairing policy.
        PeerPair[] CasePairings(JsonNode item, string? fixture = null) =>
            PeerPairingPolicy.GetPairings(item, fixture: fixture).Select(pair => (pair.Writer, pair.Reader)).ToArray();

        // Captures generation evidence using the runner's shared in-process implementation.
        void WriteGenerationProvenance(string[] commandArgs)
        {
            RequireArgs(6);
            var provenance = GenerationProvenanceBuilder.Create(FindRepositoryRoot(commandArgs[1]), commandArgs[1],
                commandArgs[2], commandArgs[3], commandArgs[4]);
            GenerationProvenanceWriter.Write(provenance, commandArgs[5]);
        }

        // Excludes implementation gaps and unavailable RTI features before planning Wireloom exchanges.
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

            var seedProvenancePath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(commandArgs[4]))!, "execution-provenance.json");
            SourceSnapshotEvidence.Read(seedProvenancePath);

            Directory.CreateDirectory(destination);
            CopyExecutionProvenance(seedProvenancePath, destination);
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
            SourceSnapshotEvidence? sharedProvenance = null;
            var indices = new HashSet<int>();
            var exchanges = new Dictionary<string, ScenarioRow>(StringComparer.Ordinal);
            var logFiles = new List<string>();
            foreach (var metadataFile in metadataFiles.Order(StringComparer.Ordinal))
            {
                var directory = Path.GetDirectoryName(metadataFile)!;
                var metadata = ReadJson(metadataFile);
                var index = (int)metadata["shardIndex"]!;
                var plan = ReadJson(Path.Combine(directory, "plan.json"));
                var provenancePath = Path.Combine(directory, "execution-provenance.json");
                var provenance = SourceSnapshotEvidence.Read(provenancePath);
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
                if (sharedProvenance is not null
                    && (sharedProvenance.Revision != provenance.Revision
                        || sharedProvenance.DirtySnapshotSha256 != provenance.DirtySnapshotSha256
                        || sharedProvenance.DirtyFileCount != provenance.DirtyFileCount))
                {
                    throw new InvalidDataException($"Shard {index} execution provenance differs from the other shard source snapshots.");
                }

                sharedProvenance ??= provenance;
                sharedPlan = plan;
                var expectedKeys = plan["cases"]!.AsArray().Where(item => (int)item!["shard"]! == index)
                    .SelectMany(item => item!["scenarioKeys"]!.AsArray()).Select(item => (string)item!).ToHashSet(StringComparer.Ordinal);
                var rows = ReadScenarioRows(Path.Combine(directory, "scenarios.tsv"));
                var report = ReadJson(Path.Combine(directory, "results.json"));
                var reportProvenance = report["executionProvenance"];
                if ((int)metadata["expectedScenarioCount"]! != expectedKeys.Count || rows.Count != expectedKeys.Count
                    || (int)report["expectedScenarioCount"]! != expectedKeys.Count || (int)report["scenarioCount"]! != rows.Count
                    || (string)report["rtiVersion"]! != version
                    || reportProvenance is null
                    || (string?)reportProvenance["revision"] != provenance.Revision
                    || (string?)reportProvenance["dirtySourceSnapshotSha256"] != provenance.DirtySnapshotSha256
                    || (int?)reportProvenance["dirtySourceFileCount"] != provenance.DirtyFileCount)
                {
                    throw new InvalidDataException($"Shard {index} has missing exchanges, inconsistent report counts, or execution provenance that does not match its captured source manifest.");
                }

                var reportScenarios = report["scenarios"]!.AsArray();
                if (reportScenarios.Count != rows.Count || reportScenarios.Any(scenario =>
                    (string?)scenario!["sourceRevision"] != provenance.Revision
                    || (string?)scenario["dirtySourceSnapshotSha256"] != provenance.DirtySnapshotSha256
                    || (int?)scenario["dirtySourceFileCount"] != provenance.DirtyFileCount))
                {
                    throw new InvalidDataException($"Shard {index} scenario provenance differs from its captured execution identity.");
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
                    logFiles.AddRange(Directory.GetFiles(logs, "*-generation.json"));
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
            CopyExecutionProvenance(Path.Combine(Path.GetDirectoryName(metadataFiles[0])!, "execution-provenance.json"), destination);
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

            var eligible = CorpusCaseDiscovery.Read(manifestPath, exclusionsPath).WireloomCases;
            var selected = eligible.Where(id => requestedCase is null || id == requestedCase).ToArray();
            if (requestedCase is not null && selected.Length == 0)
            {
                throw new InvalidOperationException($"Case {requestedCase} is not a wire-testable Wireloom positive case.");
            }

            var discovery = new WireloomTypeDiscovery(new CorpusCaseCatalog(manifestPath));
            var descriptors = discovery.Discover(generatedRoot, selected);
            CaseRegistry.Write(descriptors, outputPath);
            Console.WriteLine($"Discovered {descriptors.Count} Wireloom positive case type(s).");
        }

        string FindRepositoryRoot(string path)
        {
            var configuredWorkspace = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
            if (!string.IsNullOrWhiteSpace(configuredWorkspace) && Directory.Exists(configuredWorkspace))
            {
                return Path.GetFullPath(configuredWorkspace);
            }

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

            var request = new ReportRequest(commandArgs[1], int.Parse(commandArgs[2]), commandArgs[3],
                commandArgs[4], commandArgs[5], commandArgs[6], commandArgs[7], commandArgs[8], FindRepositoryRoot(commandArgs[8]));
            WireReportRenderer.Render(request);
        }

        void CopyExecutionProvenance(string sourcePath, string destinationDirectory)
        {
            var targetPath = Path.Combine(destinationDirectory, "execution-provenance.json");
            File.Copy(sourcePath, targetPath, overwrite: true);
            var sourceRoot = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(sourcePath))!, "source-files");
            var targetRoot = Path.Combine(destinationDirectory, "source-files");
            if (Directory.Exists(sourceRoot))
            {
                foreach (var file in Directory.GetFiles(sourceRoot, "*", SearchOption.AllDirectories))
                {
                    var relativePath = Path.GetRelativePath(sourceRoot, file);
                    var target = Path.Combine(targetRoot, relativePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                    File.Copy(file, target, overwrite: true);
                }
            }
        }

        // Share the guarded source-edit policy with the Runner.
        void AdaptRtiCharacters(string[] commandArgs)
        {
            RequireArgs(4);
            RtiCharacterReferenceAdapter.Apply(commandArgs[1], commandArgs[2], commandArgs[3]);
        }

    }
}
