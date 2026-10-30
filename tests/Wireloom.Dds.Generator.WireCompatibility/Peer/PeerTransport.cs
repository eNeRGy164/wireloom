#nullable enable annotations

using System.Diagnostics;
using Rti.Dds.Core.Policy;
using Rti.Dds.Domain;
using Rti.Dds.Publication;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types.Dynamic;
using Wireloom.Dds.Generator.TestSupport;
using LifecyclePhaseProtocol = global::Wireloom.Dds.Generator.WireCompatibility.Shared.LifecyclePhaseProtocol;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Runs reliable DDS publication and reception, preserving payload and lifecycle statistics.</summary>
internal static class PeerTransport
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20);
    private static readonly TimeSpan LifecyclePhaseTimeout = TimeSpan.FromSeconds(45);

    /// <summary>Reads effective participant transport settings after creation for the fragmented lifecycle cases.</summary>
    internal static void WriteLifecycleParticipantQos(DomainParticipant participant, PeerOptions options)
    {
        if (!PeerQos.IsFragmentedLifecycle(options.CaseId, options.Fixture))
        {
            return;
        }

        const string messageSizeProperty = "dds.transport.UDPv4.builtin.parent.message_size_max";

        var qos = participant.Qos;
        var transportUdpOnly = qos.TransportBuiltin.Mask == TransportBuiltinMask.Udpv4;
        var messageSize = qos.Property.Value.TryGetValue(messageSizeProperty, out var entry) ? entry.Value : string.Empty;
        if (!transportUdpOnly || messageSize != "16384")
        {
            throw new InvalidOperationException("Effective participant transport QoS is not UDPv4-only with a 16384-byte message limit.");
        }

        Console.WriteLine($"LIFECYCLE_PARTICIPANT_QOS transport_udp_only={transportUdpOnly.ToString().ToLowerInvariant()} message_size_max={messageSize}");
    }

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

        if (PeerQos.UsesXcdr1(options.CaseId, options.Fixture))
        {
            writerQos = writerQos.WithRepresentation(PeerQos.Xcdr1Only);
        }
        else if (PeerQos.UsesXcdr2(options.CaseId, options.Fixture))
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

        if (PeerQos.UsesXcdr1(options.CaseId, options.Fixture))
        {
            readerQos = readerQos.WithRepresentation(PeerQos.Xcdr1Only);
        }
        else if (PeerQos.UsesXcdr2(options.CaseId, options.Fixture))
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

    /// <summary>Publishes five mutated samples, waiting for the runner's application-level acknowledgment per phase.</summary>
    internal static void PublishLifecycle<T>(DomainParticipant participant, Topic<T> topic,
        ISerializer<T> serializer, T sample, PeerOptions options)
    {
        var fragmented = PeerQos.IsFragmentedLifecycle(options.CaseId, options.Fixture);
        var writerQos = participant.ImplicitPublisher.DefaultDataWriterQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable)
            .WithProtocol(policy =>
            {
                var protocol = policy.RtpsReliableWriter;
                var heartbeatPeriod = Omg.Dds.Core.Duration.FromMilliseconds(50);
                protocol.HeartbeatPeriod = heartbeatPeriod;
                protocol.FastHeartbeatPeriod = heartbeatPeriod;
                protocol.LateJoinerHeartbeatPeriod = heartbeatPeriod;
                protocol.MaxHeartbeatRetries = 600;
            });

        if (PeerQos.UsesXcdr1(options.CaseId, options.Fixture))
        {
            writerQos = writerQos.WithRepresentation(PeerQos.Xcdr1Only);
        }
        else if (PeerQos.UsesXcdr2(options.CaseId, options.Fixture))
        {
            writerQos = writerQos.WithRepresentation(PeerQos.Xcdr2Only);
        }

        if (fragmented)
        {
            writerQos = writerQos
                .WithHistory(policy =>
                {
                    policy.Kind = HistoryKind.KeepLast;
                    policy.Depth = 1;
                })
                .WithProperty(properties => properties.Add(
                    "dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size",
                    new Property.Entry("4096", propagate: false)));

            writerQos = writerQos.WithPublishMode(policy =>
            {
                policy.Kind = PublishModeKind.Asynchronous;
                policy.FlowControllerName = PublishMode.FixedRateFlowControllerName;
            });
        }

        using var writer = participant.ImplicitPublisher.CreateDataWriter(topic, writerQos);
        WaitForSubscriptions(writer, options);

        var effectiveWriterQos = writer.Qos;
        var effectiveWriterProperties = effectiveWriterQos.Property.Value;
        var writerPoolSize = effectiveWriterProperties.TryGetValue(
            "dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size", out var poolEntry)
            ? poolEntry.Value
            : string.Empty;

        if (fragmented && (effectiveWriterQos.PublishMode.Kind != PublishModeKind.Asynchronous
            || effectiveWriterQos.PublishMode.FlowControllerName != PublishMode.FixedRateFlowControllerName
            || effectiveWriterQos.History.Kind != HistoryKind.KeepLast
            || effectiveWriterQos.History.Depth != 1
            || writerPoolSize != "4096"))
        {
            throw new InvalidOperationException("Fragmented lifecycle writer effective QoS differs from its configured publish, flow-control, history, or memory-pool settings.");
        }

        Console.WriteLine($"LIFECYCLE_WRITER_QOS asynchronous={(effectiveWriterQos.PublishMode.Kind == PublishModeKind.Asynchronous).ToString().ToLowerInvariant()} flow_controller={effectiveWriterQos.PublishMode.FlowControllerName} history_kind={effectiveWriterQos.History.Kind} history_depth={effectiveWriterQos.History.Depth} fast_pool_max_buffer_size={(writerPoolSize.Length == 0 ? "unset" : writerPoolSize)}");
        var protocol = new LifecyclePhaseProtocol(options.TopicName);

        for (var phaseId = 0; phaseId < LifecyclePhaseProtocol.PhaseCount; phaseId++)
        {
            UnboundedReceiveLifecycleFixture.Initialize(sample!, phaseId, fragmented);

            var serializedBytes = UnboundedReceiveLifecycleFixture.ExpectedSerializedSize(serializer, sample!);

            Console.WriteLine($"PHASE_PUBLISHED {options.TopicName} {phaseId} serialized_bytes={serializedBytes} fragmented={fragmented}");

            if (fragmented && phaseId is 1 or 4 && serializedBytes <= 16 * 1024)
            {
                throw new InvalidDataException($"Fragmentation phase {phaseId} serialized to only {serializedBytes} bytes.");
            }

            protocol.MarkWriterPublished(phaseId);
            writer.Write(sample);

            using var deadline = new CancellationTokenSource(LifecyclePhaseTimeout);

            var acknowledgment = Console.In.ReadLineAsync(deadline.Token).GetAwaiter().GetResult() ?? throw new EndOfStreamException($"Runner ended before acknowledging lifecycle phase {phaseId}.");
            var acknowledgedPhase = protocol.AcceptWriterAcknowledgment(acknowledgment);
            if (acknowledgedPhase != phaseId)
            {
                throw new InvalidDataException($"Runner acknowledged phase {acknowledgedPhase} while phase {phaseId} was current.");
            }
        }

        protocol.EnsureComplete();
        writer.WaitForAcknowledgments(new Omg.Dds.Core.Duration(10, 0));
        Console.WriteLine("SENT");
    }

    /// <summary>Receives and validates every lifecycle sample before its DDS loan is returned and the event is emitted.</summary>
    internal static void ReceiveLifecycle<T>(DomainParticipant participant, Topic<T> topic,
        ISerializer<T> serializer, PeerOptions options)
    {
        var fragmented = PeerQos.IsFragmentedLifecycle(options.CaseId, options.Fixture);
        var readerQos = participant.ImplicitSubscriber.DefaultDataReaderQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable)
            .WithHistory(policy =>
            {
                policy.Kind = HistoryKind.KeepLast;
                policy.Depth = 1;
            });

        if (PeerQos.UsesXcdr1(options.CaseId, options.Fixture))
        {
            readerQos = readerQos.WithRepresentation(PeerQos.Xcdr1Only);
        }
        else if (PeerQos.UsesXcdr2(options.CaseId, options.Fixture))
        {
            readerQos = readerQos.WithRepresentation(PeerQos.Xcdr2Only);
        }

        if (fragmented)
        {
            readerQos = readerQos.WithReaderResourceLimits(policy =>
            {
                policy.DisableFragmentationSupport = false;
                policy.InitialFragmentedSamples = 1;
                policy.MaxFragmentedSamples = 2;
                policy.MaxFragmentedSamplesPerRemoteWriter = 1;
                policy.MaxFragmentsPerSample = 4096;
                policy.DynamicallyAllocateFragmentedSamples = true;
            });
        }

        using var reader = participant.ImplicitSubscriber.CreateDataReader(topic, readerQos);

        if (fragmented)
        {
            var effectiveReaderQos = reader.Qos;
            var resources = effectiveReaderQos.ReaderResourceLimits;
            if (resources.DisableFragmentationSupport
                || !resources.DynamicallyAllocateFragmentedSamples
                || resources.InitialFragmentedSamples != 1
                || resources.MaxFragmentedSamples != 2
                || resources.MaxFragmentedSamplesPerRemoteWriter != 1
                || resources.MaxFragmentsPerSample != 4096
                || effectiveReaderQos.History.Kind != HistoryKind.KeepLast
                || effectiveReaderQos.History.Depth != 1)
            {
                throw new InvalidOperationException("Effective reader fragmentation, history, or resource limits differ from the qualification settings.");
            }

            Console.WriteLine($"LIFECYCLE_READER_QOS fragmentation_enabled={(!resources.DisableFragmentationSupport).ToString().ToLowerInvariant()} dynamic_fragmented_allocation={resources.DynamicallyAllocateFragmentedSamples.ToString().ToLowerInvariant()} initial_fragmented_samples={resources.InitialFragmentedSamples} max_fragmented_samples={resources.MaxFragmentedSamples} max_fragmented_samples_per_remote_writer={resources.MaxFragmentedSamplesPerRemoteWriter} max_fragments_per_sample={resources.MaxFragmentsPerSample} history_kind={effectiveReaderQos.History.Kind} history_depth={effectiveReaderQos.History.Depth}");
        }

        Console.WriteLine($"READY {options.CaseId} fixture={options.Fixture} {options.Role} {options.RuntimeVersion}");

        var lifecycle = new LifecyclePhaseProtocol(options.TopicName);
        var lastFragmentCount = reader.DataReaderProtocolStatus.ReceivedFragmentCount;
        var lastReassembledCount = reader.DataReaderProtocolStatus.ReassembledSampleCount;
        var lastDroppedCount = reader.DataReaderProtocolStatus.DroppedFragmentCount;
        var totalValidSamples = 0;
        var totalRejectedSamples = reader.DataReaderProtocolStatus.RejectedSampleCount.Value;

        for (var expectedPhase = 0; expectedPhase < Wireloom.Dds.Generator.WireCompatibility.Shared.LifecyclePhaseProtocol.PhaseCount; expectedPhase++)
        {
            var deadline = Stopwatch.StartNew();
            var observed = false;
            while (deadline.Elapsed < LifecyclePhaseTimeout && !observed)
            {
                var phaseId = -1;
                var serializedBytes = 0;
                var writerGuid = string.Empty;
                var sequenceNumber = string.Empty;
                var samplesTaken = 0;
                var invalidSamples = 0;

                using (var samples = reader.Take())
                {
                    var batch = ReaderSampleBatch.Collect(
                        samples,
                        sample => sample.Info.ValidData,
                        sample => sample.Data);

                    invalidSamples = batch.InvalidSamples;
                    samplesTaken = batch.ValidData.Count;
                    if (samplesTaken > 1)
                    {
                        throw new InvalidDataException($"Lifecycle phase {expectedPhase} received {samplesTaken} valid samples before its phase acknowledgment.");
                    }

                    if (samplesTaken == 1)
                    {
                        var actual = batch.ValidData[0];
                        phaseId = UnboundedReceiveLifecycleFixture.GetPhaseId(actual!);
                        if (phaseId != expectedPhase)
                        {
                            throw new InvalidDataException($"Received lifecycle phase {phaseId} while phase {expectedPhase} was expected.");
                        }

                        UnboundedReceiveLifecycleFixture.Verify(actual!, phaseId, fragmented);
                        serializedBytes = UnboundedReceiveLifecycleFixture.ExpectedSerializedSize(serializer, actual!);

                        var sampleInfo = samples.First(sample => sample.Info.ValidData).Info;
                        var identity = sampleInfo.PublicationVirtualSampleIdentity;

                        writerGuid = identity.WriterGuid.ToString();
                        sequenceNumber = identity.SequenceNumber.ToString();
                        observed = true;
                    }
                }

                if (!observed)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                    continue;
                }

                totalValidSamples += samplesTaken;

                var status = reader.DataReaderProtocolStatus;
                var receivedDelta = status.ReceivedFragmentCount - lastFragmentCount;
                var reassembledDelta = status.ReassembledSampleCount - lastReassembledCount;
                var droppedDelta = status.DroppedFragmentCount - lastDroppedCount;
                var rejectedDelta = status.RejectedSampleCount.Value - totalRejectedSamples;

                if (fragmented && expectedPhase is 1 or 4
                    && (serializedBytes <= 16 * 1024 || receivedDelta <= 0 || reassembledDelta <= 0))
                {
                    throw new InvalidDataException($"Fragmentation evidence is incomplete for phase {expectedPhase}: bytes={serializedBytes}, fragments={receivedDelta}, reassembled={reassembledDelta}.");
                }

                Console.WriteLine(
                    $"LIFECYCLE_PHASE phase={phaseId} serialized_bytes={serializedBytes} valid_samples={samplesTaken} invalid_samples={invalidSamples} " +
                    $"writer_guid={writerGuid} sequence={sequenceNumber} received_fragments_delta={receivedDelta} " +
                    $"reassembled_samples_delta={reassembledDelta} dropped_fragments_delta={droppedDelta} rejected_samples_delta={rejectedDelta}");

                var phaseEvent = $"PHASE_VERIFIED {options.TopicName} {phaseId}";

                lifecycle.AcceptVerifiedEvent(phaseEvent);

                Console.WriteLine(phaseEvent);
                Console.Out.Flush();

                lastFragmentCount = status.ReceivedFragmentCount;
                lastReassembledCount = status.ReassembledSampleCount;
                lastDroppedCount = status.DroppedFragmentCount;
                totalRejectedSamples = status.RejectedSampleCount.Value;
            }

            if (!observed)
            {
                throw new TimeoutException($"No valid sample received for lifecycle phase {expectedPhase}.");
            }
        }

        lifecycle.EnsureComplete();

        if (totalValidSamples != LifecyclePhaseProtocol.PhaseCount)
        {
            throw new InvalidDataException($"Received {totalValidSamples} lifecycle samples; expected five.");
        }

        var finalStatus = reader.DataReaderProtocolStatus;

        Console.WriteLine($"LIFECYCLE_READER_FINAL valid_samples={totalValidSamples} rejected_samples={finalStatus.RejectedSampleCount.Value} dropped_fragments={finalStatus.DroppedFragmentCount}");
        var implementation = typeof(T).Assembly.GetName().Name?.Contains("RtiOracleProbe", StringComparison.Ordinal) == true
            ? "rti-csharp"
            : "wireloom-csharp";
        Console.WriteLine($"LIFECYCLE_RECEIVE_STORAGE implementation={implementation} phases=5 history_depth=1 optional_nested_transitions=true loan_returned_before_ack=true content_verified=true");
    }

    private static void WaitForSubscriptions<T>(DataWriter<T> writer, PeerOptions options)
    {
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

        Console.WriteLine($"WRITER_MATCHED case={options.CaseId} fixture={options.Fixture} readers_current={matchStatus.CurrentCount} readers_total={matchStatus.TotalCount}");
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
