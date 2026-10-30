using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks report rejection of missing, malformed, and incomplete receive-lifecycle evidence.</summary>
internal static class LifecycleEvidenceVerification
{
    internal static void Run(string directory)
    {
        Directory.CreateDirectory(directory);

        var log = Path.Combine(directory, "lifecycle.log");
        WritePassingLog(log);

        var passing = LifecycleEvidenceBuilder.Build("fragmented-xcdr1", "PASS", log);
        Check((int?)passing["phaseCount"] == 5
            && ((JsonArray)passing["readerQos"]!).Count == 1
            && (string?)passing["readerQos"]![0]!["fragmentation_enabled"] == "true"
            && ((JsonArray)passing["receiveStorage"]!).Count == 1,
            "A complete lifecycle log must retain effective reader QoS and receive-storage transition evidence.");

        var validText = File.ReadAllText(log);
        ExpectPassingLogRejected(log, validText.Replace("fragmentation_enabled=true", "fragmentation_enabled=false", StringComparison.Ordinal),
            "A fragmented PASS with fragmentation disabled must be rejected.");
        ExpectPassingLogRejected(log, validText.Replace("dynamic_fragmented_allocation=true", "dynamic_fragmented_allocation=false", StringComparison.Ordinal),
            "A fragmented PASS without dynamic fragment allocation must be rejected.");
        ExpectPassingLogRejected(log, validText.Replace("message_size_max=16384", "message_size_max=8192", StringComparison.Ordinal),
            "Contradictory participant message limits must be rejected.");
        ExpectPassingLogRejected(log, validText + Environment.NewLine + "LIFECYCLE_READER_QOS fragmentation_enabled=false",
            "Duplicate reader QoS records must be rejected.");
        ExpectPassingLogRejected(log, string.Join(Environment.NewLine, validText.Split(Environment.NewLine).Where(line => !line.StartsWith("LIFECYCLE_PARTICIPANT_QOS ", StringComparison.Ordinal))),
            "Missing participant QoS evidence must be rejected.");
        ExpectPassingLogRejected(log, validText.Replace("max_fragmented_samples=2", "max_fragmented_samples=999999", StringComparison.Ordinal),
            "Out-of-range reader resource limits must be rejected.");
        ExpectPassingLogRejected(log, validText.Replace("history_depth=1", "history_depth=0", StringComparison.Ordinal),
            "Invalid reader history depth must be rejected.");
        ExpectPassingLogRejected(log, string.Join(Environment.NewLine, validText.Split(Environment.NewLine)
            .Where(line => !line.StartsWith("LIFECYCLE_RECEIVE_STORAGE ", StringComparison.Ordinal))),
            "A passing lifecycle report must require receive-storage transition evidence.");

        File.WriteAllLines(log, Enumerable.Range(0, 4).Select(phase => PhaseLine(phase)));
        Throws<InvalidDataException>(() => LifecycleEvidenceBuilder.Build("lifecycle-xcdr1", "PASS", log),
            "A passing scenario with a missing phase must be rejected.");

        WritePassingLog(log, phase => phase == 1 ? "received_fragments_delta=0" : null);
        Throws<InvalidDataException>(() => LifecycleEvidenceBuilder.Build("fragmented-xcdr1", "PASS", log),
            "Fragmented large samples must prove received DATA_FRAG and reassembly counts.");

        WritePassingLog(log, phase => phase == 3 ? "rejected_samples_delta=1" : null);
        Throws<InvalidDataException>(() => LifecycleEvidenceBuilder.Build("lifecycle-xcdr1", "PASS", log),
            "A passing phase with rejected samples must be rejected.");

        File.WriteAllText(log, "LIFECYCLE_PHASE phase=0 serialized_bytes=1 valid_samples=1 invalid_samples=0");
        var partialFailure = LifecycleEvidenceBuilder.Build("lifecycle-xcdr1", "FAIL", log);
        Check((int?)partialFailure["phaseCount"] == 1 && ((JsonArray)partialFailure["malformedPhaseEvidence"]!).Count == 0,
            "Failed scenarios must retain partial lifecycle evidence for diagnosis.");

        File.WriteAllText(log, "LIFECYCLE_PHASE malformed trailing control\nLIFECYCLE_READER_QOS fragmentation_enabled=false");
        var malformedFailure = LifecycleEvidenceBuilder.Build("fragmented-xcdr1", "FAIL", log);
        Check((int?)malformedFailure["phaseCount"] == 0
            && ((JsonArray)malformedFailure["malformedPhaseEvidence"]!).Count == 1,
            "Malformed phase logs on failed scenarios must remain reportable as partial evidence.");

        Console.WriteLine("Lifecycle evidence passed: effective QoS, invalid/duplicate/missing settings, phase/reassembly/final counters, and malformed partial failure evidence.");
    }

    private static void WritePassingLog(string path, Func<int, string?>? phaseReplacement = null)
    {
        var lines = Enumerable.Range(0, 5).Select(phase => PhaseLine(phase, phaseReplacement?.Invoke(phase))).ToList();
        lines.Add("LIFECYCLE_COORDINATION expected_phases=5 accepted_phases=5 completed=true reader_output_drained=true outcome=complete");
        lines.Add("LIFECYCLE_READER_FINAL valid_samples=5 rejected_samples=0 dropped_fragments=0");
        lines.Add("LIFECYCLE_RECEIVE_STORAGE implementation=wireloom-csharp phases=5 history_depth=1 optional_nested_transitions=true loan_returned_before_ack=true content_verified=true");
        lines.Add("LIFECYCLE_PARTICIPANT_QOS transport_udp_only=true message_size_max=16384 endpoint=writer");
        lines.Add("LIFECYCLE_PARTICIPANT_QOS transport_udp_only=true message_size_max=16384 endpoint=reader");
        lines.Add("LIFECYCLE_WRITER_QOS asynchronous=true flow_controller=DDS_FIXED_RATE_FLOW_CONTROLLER_NAME history_kind=KeepLast history_depth=1 fast_pool_max_buffer_size=4096 endpoint=writer");
        lines.Add("LIFECYCLE_READER_QOS fragmentation_enabled=true dynamic_fragmented_allocation=true initial_fragmented_samples=1 max_fragmented_samples=2 max_fragmented_samples_per_remote_writer=1 max_fragments_per_sample=4096 history_kind=KeepLast history_depth=1 endpoint=reader");

        File.WriteAllLines(path, lines);
    }

    private static void ExpectPassingLogRejected(string path, string logText, string message)
    {
        File.WriteAllText(path, logText);
        Throws<InvalidDataException>(() => LifecycleEvidenceBuilder.Build("fragmented-xcdr1", "PASS", path), message);
    }

    private static string PhaseLine(int phase, string? replacement = null)
    {
        var fields = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["phase"] = phase.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["serialized_bytes"] = "24000",
            ["valid_samples"] = "1",
            ["invalid_samples"] = "0",
            ["writer_guid"] = "guid-a",
            ["sequence"] = (phase + 10).ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["received_fragments_delta"] = phase is 1 or 4 ? "2" : "0",
            ["reassembled_samples_delta"] = phase is 1 or 4 ? "1" : "0",
            ["dropped_fragments_delta"] = "0",
            ["rejected_samples_delta"] = "0",
        };

        if (replacement is not null)
        {
            var pair = replacement.Split('=', 2);
            fields[pair[0]] = pair[1];
        }

        return "LIFECYCLE_PHASE " + string.Join(' ', fields.Select(pair => $"{pair.Key}={pair.Value}"));
    }

    private static void Throws<TException>(Action action, string message) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
