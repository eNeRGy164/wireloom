using System.Text.Json.Nodes;
using Wireloom.Dds.Generator.WireCompatibility.Runner;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Builds representative scenario evidence and renders its four shard reports.</summary>
internal static class ShardScenarioBuilder
{
    internal static ShardScenarioFixture Build(VerificationContext context)
    {
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;
        var rtiVersion = context.RtiVersion;
        var manifestPath = context.ManifestPath;

        void Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);

        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        // Arrange: unequal case sizes, all peer pairings, and an RTI-only case.
        // TSV columns match ScenarioRow in the shared library: case, three types,
        // writer/version, reader/version, status, fixture.
        var peers = new[] { PeerNames.WireloomCSharp, PeerNames.RtiCSharp, PeerNames.RtiCpp };
        var cases = new Dictionary<string, string[]>
        {
            ["04-strings"] = ["non-default"],
            ["04-boundaries"] = ["string-boundary"],
            ["04-unbounded-string-aliases"] = ["string-empty", "string-boundary"],
            ["04-string-alias-chains"] = ["string-empty", "string-boundary"],
            ["07-union-string-sequence-bounded-direct"] = ["union-narrow-multiple", "union-wide-multiple"],
            ["02-multiple"] = ["non-default"],
            ["03-enum-values-prefix"] = ["default"],
            ["03-enum-values-explicit"] = ["default"],
            ["01-boundaries"] = ["non-default"],
            ["01-full-widths"] = ["non-default"],
            ["09-defaults-ranges"] = ["non-default"],
            ["05-array-of-sequences"] = ["default"],
            ["05-array-of-sequences-idl"] = ["sequence-empty", "sequence-unequal", "sequence-mixed-empty", "sequence-boundary"],
            ["05-collections"] = ["sequence-empty", "sequence-single", "sequence-multiple"],
            ["05-unbounded-sequence-aliases"] = ["sequence-empty", "sequence-multiple"],
            ["07-union-string-sequence-aliases"] = [
                "union-direct-narrow", "union-direct-wide", "union-alias-narrow",
                "union-alias-wide", "union-bounded-narrow", "union-bounded-wide"],
            ["07-union-char-numeric"] = ["union-127", "union-default"],
            ["07-union-char-numeric-255"] = ["union-255", "union-default"],
            ["07-union-char-boundaries"] = ["union-minimum", "union-maximum", "union-default"],
            ["07-union-ulong"] = ["union-minimum", "union-maximum", "union-default"],
            ["09-optional-aggregate-member"] = ["optional-absent", "optional-present"],
            ["09-optional-recursive-array"] = ["optional-absent", "optional-present", "optional-nested"],
            ["10-resolve-name-wstring-aliases"] = ["string-empty", "string-boundary"],
            ["14-extensibility-full"] = ["non-default"],
            ["14-autoid-scope"] = ["scope-nested"],
            ["14-annotation-arguments"] = ["default", "non-default"],
            ["14-member-declarators"] = ["non-default"],
            ["14-union-label-expressions"] = ["symbolic", "arithmetic", "hexadecimal"],
            ["14-union-scoped-enum"] = ["enum-selected", "enum-first"],
            ["14-union-array-branch"] = ["array-branch", "scalar-branch"],
            ["14-string-expression-bound"] = ["string-boundary"],
            ["14-typed-defaults"] = ["default", "non-default"],
            ["14-enum-constant"] = ["constant-and-label"],
            ["14-unbounded-receive-lifecycle"] = ["fragmented-xcdr1", "lifecycle-xcdr1"]
        };

        var rows = new List<ScenarioRow>();

        foreach (var item in cases)
        {
            var pairings = peers.SelectMany(writer => peers.Select(reader => (Writer: writer, Reader: reader))).ToArray();

            foreach (var fixture in item.Value)
            {
                foreach (var (writer, reader) in pairings)
                {
                    if (item.Key == "05-array-of-sequences-idl" && (writer == peers[1] || reader == peers[1]))
                    {
                        continue;
                    }

                    if (item.Key == "09-optional-aggregate-member" && (writer == peers[0] || reader == peers[0]))
                    {
                        continue;
                    }

                    var status = "PASS";
                    if (item.Key == "05-array-of-sequences"
                        && ((writer == peers[0] && reader == peers[2]) || (writer == peers[2] && reader == peers[0])))
                    {
                        status = "FAIL";
                    }
                    else if (item.Key == "07-union-ulong" && (writer == peers[1] || reader == peers[1]))
                    {
                        status = "FAIL";
                    }

                    rows.Add(new ScenarioRow(item.Key, "Managed.Type", "Oracle.Type", "::Native::Type", writer, rtiVersion, reader, rtiVersion, status, fixture));
                }
            }
        }

        var seed = Path.Combine(root, "all.tsv");
        File.WriteAllLines(seed, rows.Select(row => row.ToTsv()));
        var syntheticProvenance = new SourceSnapshotEvidence("synthetic-verifier-revision",
            SourceSnapshotEvidence.ComputeSnapshotHash([]), 0, []);
        syntheticProvenance.Write(Path.Combine(root, "execution-provenance.json"));

        var exclusions = Path.Combine(root, "exclusions.json");
        File.WriteAllText(exclusions,
            """
            {
              "notImplementedCases": [
                {
                  "id": "09-optional-aggregate-member",
                  "diagnostic": "TEST",
                  "reason": "RTI-only control",
                  "issue": "https://github.com/eNeRGy164/wireloom/issues/15"
                }
              ],
              "notApplicableCases": [
                {
                  "id": "09-must-understand",
                  "reason": "RTI feature unavailable in this test profile"
                }
              ]
            }
            """);

        var source = Path.Combine(root, "shards");

        for (var index = 0; index < 4; index++)
        {
            var directory = Path.Combine(source, $"shard-{index}");
            Invoke(succeeds: true, "partition-shard", rtiVersion, index.ToString(), "4", seed, directory);

            var logs = Path.Combine(directory, "logs");
            Directory.CreateDirectory(logs);

            if (File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Any(line => line.StartsWith("05-array-of-sequences-idl\t", StringComparison.Ordinal)))
            {
                var generated = Path.Combine(root, "generated");
                Directory.CreateDirectory(generated);
                File.WriteAllText(Path.Combine(generated, "Sample.g.cs"), "// Synthetic source for black-box provenance validation.");
                Invoke(succeeds: true, "generation-provenance", manifestPath, "05-array-of-sequences-idl", generated,
                    rtiVersion, Path.Combine(logs, "05-array-of-sequences-idl-generation.json"));
            }

            foreach (var manifestCase in ReadJson(manifestPath)["positiveCases"]!.AsArray())
            {
                var caseId = (string)manifestCase!["id"]!;
                var wrapperName = caseId switch
                {
                    "14-autoid-scope" => "14-autoid-scope-topic.idl",
                    "14-enum-constant" => "14-enum-constant-topic.idl",
                    _ => caseId + ".idl"
                };
                var wrapperPath = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers", wrapperName);
                if (!File.Exists(wrapperPath)
                    || !File.ReadAllLines(Path.Combine(directory, "scenarios.tsv"))
                        .Any(line => line.StartsWith(caseId + "\t", StringComparison.Ordinal)))
                {
                    continue;
                }

                var generated = Path.Combine(root, caseId + "-generated");
                Directory.CreateDirectory(generated);
                File.WriteAllText(Path.Combine(generated, "Sample.g.cs"), "// Synthetic source for wrapper provenance validation.");
                Invoke(succeeds: true, "generation-provenance", manifestPath, caseId, generated,
                    rtiVersion, Path.Combine(logs, caseId + "-generation.json"));
            }

            foreach (var row in File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Select(ScenarioRow.Parse))
            {
                var peerIds = new Dictionary<string, string>
                {
                    [peers[0]] = "wireloom-cs",
                    [peers[1]] = "rti-cs",
                    [peers[2]] = "rti-cpp",
                };

                var logText = "FAIL_DETAIL retained symptom: NullReferenceException\n";

                if (row.Status == "NOT_RUN")
                {
                    logText = "NOT_RUN_DETAIL interrupted exchange\n";
                }
                else if (row is { CaseId: "07-union-ulong", Status: "FAIL" })
                {
                    logText = "FAIL_DETAIL RTI-generated C# peer failed to compile; CS0221\n";
                }

                if (row.CaseId == "14-unbounded-receive-lifecycle")
                {
                    logText = LifecycleLog(row, peers[2]);
                }

                File.WriteAllText(Path.Combine(logs, $"{row.CaseId}-{row.Fixture}-{peerIds[row.Writer]}-{peerIds[row.Reader]}.log"), logText);
            }

            var count = File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Length.ToString();

            Invoke(succeeds: true, "render-report", rtiVersion, count, Path.Combine(directory, "scenarios.tsv"),
                Path.Combine(directory, "results.json"), Path.Combine(directory, "results.md"), logs, exclusions,
                Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));
        }

        return new ShardScenarioFixture(cases, rows, peers, seed, exclusions, source);
    }

    private static string LifecycleLog(ScenarioRow row, string nativePeer)
    {
        var fragmented = row.Fixture.StartsWith("fragmented-", StringComparison.Ordinal);
        var lines = new List<string>();
        for (var phase = 0; phase < 5; phase++)
        {
            var hasFragments = fragmented && phase is 1 or 4;
            lines.Add($"LIFECYCLE_PHASE phase={phase} serialized_bytes={(hasFragments ? 24000 : 128)} valid_samples=1 invalid_samples=0 writer_guid=guid-a sequence={phase + 10} received_fragments_delta={(hasFragments ? 2 : 0)} reassembled_samples_delta={(hasFragments ? 1 : 0)} dropped_fragments_delta=0 rejected_samples_delta=0");
        }

        lines.Add("LIFECYCLE_COORDINATION expected_phases=5 accepted_phases=5 completed=true reader_output_drained=true outcome=complete");
        lines.Add("LIFECYCLE_READER_FINAL valid_samples=5 rejected_samples=0 dropped_fragments=0");
        var readerImplementation = row.Reader == nativePeer ? "rti-cpp"
            : row.Reader == "RTI C#" ? "rti-csharp" : "wireloom-csharp";
        lines.Add($"LIFECYCLE_RECEIVE_STORAGE implementation={readerImplementation} phases=5 history_depth=1 optional_nested_transitions=true loan_returned_before_ack=true content_verified=true");
        if (fragmented)
        {
            lines.Add("LIFECYCLE_PARTICIPANT_QOS transport_udp_only=true message_size_max=16384 endpoint=writer");
            lines.Add("LIFECYCLE_PARTICIPANT_QOS transport_udp_only=true message_size_max=16384 endpoint=reader");
            lines.Add("LIFECYCLE_WRITER_QOS asynchronous=true flow_controller=DDS_FIXED_RATE_FLOW_CONTROLLER_NAME history_kind=KeepLast history_depth=1 fast_pool_max_buffer_size=4096 endpoint=writer");
            lines.Add("LIFECYCLE_READER_QOS fragmentation_enabled=true dynamic_fragmented_allocation=true initial_fragmented_samples=1 max_fragmented_samples=2 max_fragmented_samples_per_remote_writer=1 max_fragments_per_sample=4096 history_kind=KeepLast history_depth=1 endpoint=reader");
        }

        if (row.Writer == nativePeer || row.Reader == nativePeer)
        {
            if (row.Writer == nativePeer)
            {
                lines.Add("LIFECYCLE_NATIVE_CDR phases=5 cycles=10 reused_storage=true aliases=true string_sequence_elements=true optional_transitions=true nested_transitions=true ownership_snapshot=true cleanup=scope-exit endpoint=writer");
            }

            if (row.Reader == nativePeer)
            {
                lines.Add("LIFECYCLE_NATIVE_CDR phases=5 cycles=10 reused_storage=true aliases=true string_sequence_elements=true optional_transitions=true nested_transitions=true ownership_snapshot=true cleanup=scope-exit endpoint=reader");
            }
        }

        return string.Join(Environment.NewLine, lines);
    }
}
