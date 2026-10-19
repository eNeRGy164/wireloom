#nullable enable annotations

using System.Diagnostics;
using Rti.Dds.Core.Policy;
using Rti.Dds.Domain;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types.Dynamic;
using Wireloom.Dds.Generator.TestSupport;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Runs reliable DDS publication and reception, preserving payload and lifecycle statistics.</summary>
internal static class PeerTransport
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20);

    /// <summary>Waits for a matched reader, publishes once, and waits for reliable acknowledgments.</summary>
    internal static void Publish<T>(DomainParticipant participant, Topic<T> topic, T sample, PeerOptions options)
    {
        var writerQos = participant.ImplicitPublisher.DefaultDataWriterQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable)
            .WithProtocol(policy =>
            {
                // Use the same acknowledgment cadence on both RTI LTS versions.
                // Keep the default 30-second inactive-reader detection window.
                var protocol = policy.RtpsReliableWriter;
                var heartbeatPeriod = Omg.Dds.Core.Duration.FromMilliseconds(50);
                protocol.HeartbeatPeriod = heartbeatPeriod;
                protocol.FastHeartbeatPeriod = heartbeatPeriod;
                protocol.LateJoinerHeartbeatPeriod = heartbeatPeriod;
                protocol.MaxHeartbeatRetries = 600;
            });

        if (PeerQos.UsesXcdr2(options.CaseId))
        {
            writerQos = writerQos.WithRepresentation(PeerQos.Xcdr2Only);
        }

        if (options.CaseId == "09-optional-recursive-array")
        {
            // The recursive maximum size makes RTI 7.3.1 preallocate about 64 GiB.
            // Larger samples can still allocate outside this small fast pool.
            writerQos = writerQos.WithProperty(properties => properties.Add(
                "dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size",
                new Property.Entry("4096", false)));
        }

        using var writer = participant.ImplicitPublisher.CreateDataWriter(topic, writerQos);
        var deadline = Stopwatch.StartNew();

        while (!writer.MatchedSubscriptions.Any() && deadline.Elapsed < Timeout)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        if (!writer.MatchedSubscriptions.Any())
        {
            throw new TimeoutException($"No reader discovered for {options.CaseId} fixture {options.Fixture}.");
        }

        var matchStatus = writer.PublicationMatchedStatus;
        Console.WriteLine(
            $"WRITER_MATCHED case={options.CaseId} fixture={options.Fixture} " +
            $"readers_current={matchStatus.CurrentCount} " +
            $"readers_total={matchStatus.TotalCount}");
        writer.Write(sample);
        writer.WaitForAcknowledgments(new Omg.Dds.Core.Duration(10, 0));
        Console.WriteLine("SENT");
    }

    /// <summary>Ignores lifecycle notifications and requires one verified valid payload before the deadline.</summary>
    internal static void Receive<T>(
        DomainParticipant participant,
        Topic<T> topic,
        T expected,
        DynamicData? expectedDynamic,
        PeerOptions options)
    {
        var readerQos = participant.ImplicitSubscriber.DefaultDataReaderQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable);
        if (PeerQos.UsesXcdr2(options.CaseId))
        {
            readerQos = readerQos.WithRepresentation(PeerQos.Xcdr2Only);
        }

        using var reader = participant.ImplicitSubscriber.CreateDataReader(topic, readerQos);
        Console.WriteLine(
            $"READY {options.CaseId} fixture={options.Fixture} {options.Role} {options.RuntimeVersion}");
        var deadline = Stopwatch.StartNew();
        var takeCalls = 0;
        var returnedSamples = 0;
        var validSamples = 0;
        var invalidSamples = 0;

        while (deadline.Elapsed < Timeout)
        {
            using var samples = reader.Take();
            takeCalls++;
            var batch = ReaderSampleBatch.Collect(
                samples,
                sample => sample.Info.ValidData,
                sample => sample.Data);
            returnedSamples += batch.ReturnedSamples;
            validSamples += batch.ValidData.Count;
            invalidSamples += batch.InvalidSamples;

            // ValidData=false also describes ordinary instance lifecycle notifications,
            // including a writer exiting after its reliable sample was acknowledged.
            // Count those notifications, but require and verify a valid payload.
            foreach (var sample in batch.ValidData)
            {
                try
                {
                    ReceivedSampleVerifier.Verify(expected, sample, expectedDynamic, options);
                }
                catch (InvalidDataException)
                {
                    WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples, invalidSamples);
                    throw;
                }

                WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples, invalidSamples);
                return;
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples, invalidSamples);
        throw new TimeoutException($"No sample received for {options.CaseId} fixture {options.Fixture}.");
    }

    private static void WriteReaderStatistics<T>(DataReader<T> reader, int takeCalls, int returnedSamples, int validSamples, int invalidSamples)
    {
        var matchStatus = reader.SubscriptionMatchedStatus;
        Console.WriteLine(
            $"READER_STATS matched_publications_current={matchStatus.CurrentCount} " +
            $"matched_publications_total={matchStatus.TotalCount} take_calls={takeCalls} " +
            $"returned_samples={returnedSamples} valid_samples={validSamples} " +
            $"invalid_samples={invalidSamples}");
    }
}
