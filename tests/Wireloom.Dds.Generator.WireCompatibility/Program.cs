using System.Diagnostics;
using System.Reflection;
using Omg.Types;
using Rti.Config;
using Rti.Dds.Core.Policy;
using Rti.Dds.Domain;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static class Program
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20);

    private static int Main(string[] args)
    {
        try
        {
            Logger.Instance.SetVerbosity(Verbosity.Warning);
            var options = PeerOptions.Parse(args);
            Run(options);
            Console.WriteLine($"PASS {options.CaseId} {options.Role} {options.RuntimeVersion}");
            return 0;
        }
        catch (Exception exception)
        {
            var detail = exception is FixtureException fixtureException
                ? $" {fixtureException.Message}"
                : string.Empty;
            Console.Error.WriteLine($"FAIL {exception.GetType().Name} {exception.TargetSite?.Name}{detail}");
            if (exception is not FixtureException)
            {
                Console.Error.WriteLine($"FAIL_DETAIL {exception.Message.Replace(Environment.NewLine, " ")}");
            }
            return 1;
        }
    }

    private static void Run(PeerOptions options)
    {
        var sampleType = Assembly.GetExecutingAssembly().GetType(options.TypeName, throwOnError: true)!;
        var runTypedPeer = typeof(Program).GetMethod(
            nameof(RunTypedPeer),
            BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(sampleType);
        try
        {
            runTypedPeer.Invoke(null, [options]);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    private static void RunTypedPeer<T>(PeerOptions options)
    {
        // Use Wireloom's generated type and serializer end-to-end. The reference
        // DynamicData fixture only seeds the expected managed sample; typed
        // DDS writers/readers exercise generated ToNative/FromNative code.
        var typeSupport = (TypeSupport<T>)TypeSupportHelper.GetTypeSupportForType(typeof(T));
        using var serializer = typeSupport.CreateSerializer();
        using var expectedDynamic = FixtureCatalog.CreateFixture(
            typeSupport.DynamicType,
            options.CaseId,
            options.Fixture);
        var expected = options.CaseId == "09-optional-string-sequences"
            ? CreateOptionalStringSequenceFixture<T>(options.Fixture)
            : serializer.FromDynamicData(expectedDynamic);

        using var participant = DomainParticipantFactory.Instance.CreateParticipant(domainId: 0);
        using var topic = participant.CreateTopic<T>(options.TopicName);

        if (options.Role == PeerRole.Writer)
        {
            Publish(participant, topic, expected, options);
        }
        else
        {
            Receive(participant, topic, expected, options);
        }
    }

    private static T CreateOptionalStringSequenceFixture<T>(string fixture)
    {
        var sample = Activator.CreateInstance<T>()!;
        if (fixture == "optional-absent")
        {
            return sample;
        }

        var narrowValues = fixture switch
        {
            "optional-empty" => Array.Empty<string>(),
            "optional-multiple" => ["narrow-one", "narrow-two", "narrow-three"],
            "optional-narrow-only" => ["narrow-only"],
            "optional-wide-only" => null,
            _ => throw new FixtureException($"Unknown optional string sequence fixture '{fixture}'.")
        };
        var wideValues = fixture switch
        {
            "optional-empty" => Array.Empty<string>(),
            "optional-multiple" => ["wide-one", "wide-two"],
            "optional-wide-only" => ["wide-only"],
            "optional-narrow-only" => null,
            _ => throw new FixtureException($"Unknown optional string sequence fixture '{fixture}'.")
        };
        var sequenceType = typeof(Sequence<string>);
        if (narrowValues is not null)
        {
            var narrowSequence = Activator.CreateInstance(sequenceType, [narrowValues])!;
            typeof(T).GetProperty("narrowValues")!.SetValue(sample, narrowSequence);
        }
        if (wideValues is not null)
        {
            var wideSequence = Activator.CreateInstance(sequenceType, [wideValues])!;
            typeof(T).GetProperty("wideValues")!.SetValue(sample, wideSequence);
        }
        return sample;
    }

    private static void Publish<T>(
        DomainParticipant participant,
        Topic<T> topic,
        T sample,
        PeerOptions options)
    {
        var writerQos = participant.ImplicitPublisher.DefaultDataWriterQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable);
        if (UsesXcdr2(options.CaseId))
        {
            writerQos = writerQos.WithRepresentation(Xcdr2Only);
        }

        using var writer = participant.ImplicitPublisher.CreateDataWriter(topic, writerQos);
        var deadline = Stopwatch.StartNew();

        while (!writer.MatchedSubscriptions.Any() && deadline.Elapsed < Timeout)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        if (!writer.MatchedSubscriptions.Any())
        {
            throw new TimeoutException(
                $"No reader discovered for {options.CaseId} fixture {options.Fixture}.");
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

    private static void Receive<T>(
        DomainParticipant participant,
        Topic<T> topic,
        T expected,
        PeerOptions options)
    {
        var readerQos = participant.ImplicitSubscriber.DefaultDataReaderQos
            .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable);
        if (UsesXcdr2(options.CaseId))
        {
            readerQos = readerQos.WithRepresentation(Xcdr2Only);
        }

        using var reader = participant.ImplicitSubscriber.CreateDataReader(topic, readerQos);
        Console.WriteLine(
            $"READY {options.CaseId} fixture={options.Fixture} {options.Role} {options.RuntimeVersion}");
        var deadline = Stopwatch.StartNew();
        var takeCalls = 0;
        var returnedSamples = 0;
        var validSamples = 0;

        while (deadline.Elapsed < Timeout)
        {
            using var samples = reader.Take();
            takeCalls++;
            var validData = samples.ValidData().ToArray();
            returnedSamples += samples.Count();
            validSamples += validData.Length;

            foreach (var sample in validData)
            {
                if (!Equals(expected, sample))
                {
                    WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples);
                    throw new InvalidDataException(
                        $"Received sample differs from {options.CaseId} fixture {options.Fixture}.");
                }

                WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples);
                return;
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }

        WriteReaderStatistics(reader, takeCalls, returnedSamples, validSamples);
        throw new TimeoutException(
            $"No sample received for {options.CaseId} fixture {options.Fixture}.");
    }

    private static void WriteReaderStatistics<T>(
        DataReader<T> reader,
        int takeCalls,
        int returnedSamples,
        int validSamples)
    {
        var matchStatus = reader.SubscriptionMatchedStatus;
        Console.WriteLine(
            $"READER_STATS matched_publications_current={matchStatus.CurrentCount} " +
            $"matched_publications_total={matchStatus.TotalCount} take_calls={takeCalls} " +
            $"returned_samples={returnedSamples} valid_samples={validSamples} " +
            $"invalid_samples={returnedSamples - validSamples}");
    }

    private static bool UsesXcdr2(string caseId) =>
        caseId is "09-data-representation"
            or "09-allowed-data-representation"
            or "10-flat-data-binding";

    private static DataRepresentation Xcdr2Only => DataRepresentation.Default.With(
        policy =>
        {
            policy.Value.Clear();
            policy.Value.Add(DataRepresentation.Xcdr2);
        });

    private sealed record PeerOptions(
        string CaseId,
        string TypeName,
        string TopicName,
        string RuntimeVersion,
        string Fixture,
        PeerRole Role)
    {
        public static PeerOptions Parse(string[] args)
        {
            var values = new Dictionary<string, string>(StringComparer.Ordinal);

            for (var index = 0; index < args.Length; index += 2)
            {
                if (index + 1 >= args.Length || !args[index].StartsWith("--", StringComparison.Ordinal))
                {
                    throw new ArgumentException("Expected option/value pairs.");
                }

                values.Add(args[index][2..], args[index + 1]);
            }

            var role = values["role"] switch
            {
                "writer" => PeerRole.Writer,
                "reader" => PeerRole.Reader,
                _ => throw new ArgumentException("Role must be writer or reader.")
            };

            return new PeerOptions(
                values["case"],
                values["type"],
                values["topic"],
                values["runtime-version"],
                values.GetValueOrDefault("fixture", "default"),
                role);
        }
    }

    private enum PeerRole
    {
        Writer,
        Reader
    }
}

internal sealed class FixtureException(string message) : Exception(message);
