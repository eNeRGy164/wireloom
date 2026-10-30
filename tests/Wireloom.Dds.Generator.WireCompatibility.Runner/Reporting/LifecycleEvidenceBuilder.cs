using System.Globalization;
using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Validates the retained receive-lifecycle and fragment-reassembly evidence for C174.</summary>
internal static class LifecycleEvidenceBuilder
{
    internal static JsonObject Build(string fixture, string status, string scenarioLog, bool requiresNativeCdrEvidence = false)
    {
        var lines = File.Exists(scenarioLog)
            ? File.ReadLines(scenarioLog).ToArray()
            : [];
        var phases = new List<JsonObject>();
        var malformedPhaseEvidence = new List<JsonObject>();

        foreach (var line in lines.Where(line => line.StartsWith("LIFECYCLE_PHASE ", StringComparison.Ordinal)))
        {
            try
            {
                var fields = ParseFields(line);
                if (!int.TryParse(fields.GetValueOrDefault("phase"), NumberStyles.None, CultureInfo.InvariantCulture, out var phase))
                {
                    throw new InvalidDataException("Lifecycle phase evidence has no valid phase number.");
                }

                var phaseEvidence = new JsonObject { ["phase"] = phase };

                foreach (var (key, value) in fields)
                {
                    if (key != "phase")
                    {
                        phaseEvidence[key] = value;
                    }
                }

                phases.Add(phaseEvidence);
            }
            catch (InvalidDataException exception) when (status != "PASS")
            {
                malformedPhaseEvidence.Add(new JsonObject { ["raw"] = line, ["error"] = exception.Message });
            }
        }

        var participantQos = ReadRecords(lines, "LIFECYCLE_PARTICIPANT_QOS", status);
        var writerQos = ReadRecords(lines, "LIFECYCLE_WRITER_QOS", status);
        var readerQos = ReadRecords(lines, "LIFECYCLE_READER_QOS", status);
        var readerFinal = ReadRecords(lines, "LIFECYCLE_READER_FINAL", status);
        var coordination = ReadRecords(lines, "LIFECYCLE_COORDINATION", status);
        var nativeCdr = ReadRecords(lines, "LIFECYCLE_NATIVE_CDR", status);
        var receiveStorage = ReadRecords(lines, "LIFECYCLE_RECEIVE_STORAGE", status);

        if (status == "PASS")
        {
            ValidatePassingEvidence(fixture, phases);
            ValidateCoordination(coordination);
            ValidateReaderFinal(readerFinal);
            ValidateReceiveStorage(receiveStorage);
            if (requiresNativeCdrEvidence)
            {
                ValidateNativeCdr(nativeCdr);
            }
        }

        if (status == "PASS" && fixture.StartsWith("fragmented-", StringComparison.Ordinal))
        {
            ValidateEffectiveFragmentationQos(participantQos, writerQos, readerQos);
        }

        return new JsonObject
        {
            ["phaseCount"] = phases.Count,
            ["phases"] = new JsonArray(phases.Select(phase => phase.DeepClone()).ToArray()),
            ["malformedPhaseEvidence"] = new JsonArray(malformedPhaseEvidence.Select(item => item.DeepClone()).ToArray()),
            ["participantQos"] = CloneRecords(participantQos),
            ["writerQos"] = CloneRecords(writerQos),
            ["readerQos"] = CloneRecords(readerQos),
            ["readerFinal"] = CloneRecords(readerFinal),
            ["coordination"] = CloneRecords(coordination),
            ["nativeCdr"] = CloneRecords(nativeCdr),
            ["receiveStorage"] = CloneRecords(receiveStorage),
            ["readerFragmentationQosObserved"] = readerQos.Count == 1,
            ["log"] = Path.GetFileName(scenarioLog),
        };
    }

    private static void ValidatePassingEvidence(string fixture, IReadOnlyList<JsonObject> phases)
    {
        if (phases.Count != 5)
        {
            throw new InvalidDataException($"Passing lifecycle scenario has {phases.Count} phase records; expected five.");
        }

        string? writerGuid = null;
        var identities = new HashSet<string>(StringComparer.Ordinal);

        for (var index = 0; index < phases.Count; index++)
        {
            var phase = phases[index];

            if ((int?)phase["phase"] != index
                || ReadInt(phase, "valid_samples") != 1
                || ReadInt(phase, "invalid_samples") != 0
                || ReadInt(phase, "serialized_bytes") <= 0
                || ReadInt(phase, "dropped_fragments_delta") != 0
                || ReadInt(phase, "rejected_samples_delta") != 0)
            {
                throw new InvalidDataException($"Passing lifecycle phase {index} has incomplete or inconsistent sample evidence.");
            }

            var guid = (string?)phase["writer_guid"];
            var sequence = (string?)phase["sequence"];
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(sequence))
            {
                throw new InvalidDataException($"Passing lifecycle phase {index} has no publication identity.");
            }

            writerGuid ??= guid;
            if (guid != writerGuid || !identities.Add(sequence))
            {
                throw new InvalidDataException("Lifecycle phases did not come from one writer with distinct sample identities.");
            }

            if (fixture.StartsWith("fragmented-", StringComparison.Ordinal) && index is 1 or 4
                && (ReadInt(phase, "serialized_bytes") <= 16 * 1024
                    || ReadInt(phase, "received_fragments_delta") <= 0
                    || ReadInt(phase, "reassembled_samples_delta") <= 0))
            {
                throw new InvalidDataException($"Passing fragmented phase {index} lacks DATA_FRAG and reassembly evidence.");
            }
        }
    }

    private static Dictionary<string, string> ParseFields(string line)
    {
        var fields = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var field in line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1))
        {
            var separator = field.IndexOf('=');
            if (separator <= 0 || separator == field.Length - 1 || !fields.TryAdd(field[..separator], field[(separator + 1)..]))
            {
                throw new InvalidDataException("Lifecycle evidence contains a malformed or duplicate field.");
            }
        }

        return fields;
    }

    private static List<JsonObject> ReadRecords(IEnumerable<string> lines, string prefix, string status)
    {
        var records = new List<JsonObject>();
        foreach (var line in lines.Where(line => line.StartsWith(prefix + " ", StringComparison.Ordinal)))
        {
            try
            {
                var fields = ParseFields(line);
                records.Add(new JsonObject(fields.Select(pair => KeyValuePair.Create<string, JsonNode?>(pair.Key, JsonValue.Create(pair.Value))).ToArray()));
            }
            catch (InvalidDataException exception) when (status != "PASS")
            {
                records.Add(new JsonObject { ["raw"] = line, ["parseError"] = exception.Message });
            }
        }

        return records;
    }

    private static void ValidateEffectiveFragmentationQos(
        IReadOnlyList<JsonObject> participants, IReadOnlyList<JsonObject> writers, IReadOnlyList<JsonObject> readers)
    {
        if (participants.Count != 2
            || participants.Select(record => (string?)record["endpoint"]).ToHashSet(StringComparer.Ordinal)
                .SetEquals(["writer", "reader"]) is false
            || participants.Any(record => (string?)record["transport_udp_only"] != "true"
                || (string?)record["message_size_max"] != "16384"))
        {
            throw new InvalidDataException("Passing fragmented scenario must retain effective UDPv4-only/16384-byte participant QoS for both peers.");
        }

        if (writers.Count != 1
            || (string?)writers[0]["endpoint"] != "writer"
            || (string?)writers[0]["asynchronous"] != "true"
            || (string?)writers[0]["flow_controller"] != "DDS_FIXED_RATE_FLOW_CONTROLLER_NAME"
            || !IsKeepLastOne(writers[0], "history_kind", "history_depth")
            || (string?)writers[0]["fast_pool_max_buffer_size"] != "4096")
        {
            throw new InvalidDataException("Passing fragmented scenario has missing or ineffective writer QoS evidence.");
        }

        if (readers.Count != 1
            || (string?)readers[0]["endpoint"] != "reader"
            || (string?)readers[0]["fragmentation_enabled"] != "true"
            || (string?)readers[0]["dynamic_fragmented_allocation"] != "true"
            || ReadInt(readers[0], "initial_fragmented_samples") != 1
            || ReadInt(readers[0], "max_fragmented_samples") != 2
            || ReadInt(readers[0], "max_fragmented_samples_per_remote_writer") != 1
            || ReadInt(readers[0], "max_fragments_per_sample") != 4096
            || !IsKeepLastOne(readers[0], "history_kind", "history_depth"))
        {
            throw new InvalidDataException("Passing fragmented scenario has missing, contradictory, or out-of-range reader QoS evidence.");
        }
    }

    private static bool IsKeepLastOne(JsonObject qos, string historyName, string depthName) =>
        (string.Equals((string?)qos[historyName], "KeepLast", StringComparison.OrdinalIgnoreCase)
            || string.Equals((string?)qos[historyName], "KEEP_LAST", StringComparison.OrdinalIgnoreCase))
        && ReadInt(qos, depthName) == 1;

    private static void ValidateCoordination(IReadOnlyList<JsonObject> records)
    {
        if (records.Count != 1
            || ReadInt(records[0], "expected_phases") != 5
            || ReadInt(records[0], "accepted_phases") != 5
            || (string?)records[0]["completed"] != "true"
            || (string?)records[0]["reader_output_drained"] != "true"
            || (string?)records[0]["outcome"] != "complete")
        {
            throw new InvalidDataException("Passing lifecycle scenario has incomplete scheduler coordination evidence.");
        }
    }

    private static void ValidateReaderFinal(IReadOnlyList<JsonObject> records)
    {
        if (records.Count != 1
            || ReadInt(records[0], "valid_samples") != 5
            || ReadInt(records[0], "rejected_samples") != 0
            || ReadInt(records[0], "dropped_fragments") != 0)
        {
            throw new InvalidDataException("Passing lifecycle scenario has missing or invalid final reader counters.");
        }
    }

    private static void ValidateNativeCdr(IReadOnlyList<JsonObject> records)
    {
        if (records.Count is < 1 or > 2
            || records.Any(record => (string?)record["phases"] != "5"
                || (string?)record["cycles"] != "10"
                || (string?)record["reused_storage"] != "true"
                || (string?)record["aliases"] != "true"
                || (string?)record["string_sequence_elements"] != "true"
                || (string?)record["ownership_snapshot"] != "true"
                || (string?)record["cleanup"] != "scope-exit"
                || (string?)record["optional_transitions"] != "true"
                || (string?)record["nested_transitions"] != "true")
            || records.Select(record => (string?)record["endpoint"]).Where(endpoint => endpoint is not null)
                .Distinct(StringComparer.Ordinal).Count() != records.Select(record => (string?)record["endpoint"]).Count(endpoint => endpoint is not null))
        {
            throw new InvalidDataException("Passing RTI C++ lifecycle scenario has missing, duplicate, or contradictory reused-CDR evidence.");
        }
    }

    private static void ValidateReceiveStorage(IReadOnlyList<JsonObject> records)
    {
        if (records.Count != 1
            || (string?)records[0]["implementation"] is not ("wireloom-csharp" or "rti-csharp" or "rti-cpp")
            || (string?)records[0]["phases"] != "5"
            || (string?)records[0]["history_depth"] != "1"
            || (string?)records[0]["optional_nested_transitions"] != "true"
            || (string?)records[0]["loan_returned_before_ack"] != "true"
            || (string?)records[0]["content_verified"] != "true")
        {
            throw new InvalidDataException("Passing lifecycle scenario has missing or invalid receive-storage reuse evidence.");
        }
    }

    private static JsonArray CloneRecords(IEnumerable<JsonObject> records) =>
        new(records.Select(record => record.DeepClone()).ToArray());

    private static int ReadInt(JsonObject phase, string name) =>
        int.TryParse((string?)phase[name], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : throw new InvalidDataException($"Lifecycle phase evidence field '{name}' is missing or invalid.");
}
