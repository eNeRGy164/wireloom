#nullable enable annotations

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
            while (exception is TargetInvocationException or TypeInitializationException && exception.InnerException is not null)
            {
                exception = exception.InnerException;
            }

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
        if (options.CaseId == "09-optional-recursive-array")
        {
            // Both generated array plugins match RTI's Support.Instance access.
            // Start from the plugin so its recursive placeholder exists before that read.
            var plugin = typeof(T).Assembly.GetType($"{typeof(T).Namespace}.Implementation.NodePlugin", throwOnError: true)!;
            plugin.GetMethod("CreateDynamicType", BindingFlags.Public | BindingFlags.Static)!.Invoke(null, [false]);
        }

        // Use Wireloom's generated type and serializer end-to-end. The reference
        // DynamicData fixture only seeds the expected managed sample; typed
        // DDS writers/readers exercise generated ToNative/FromNative code.
        var typeSupport = (TypeSupport<T>)TypeSupportHelper.GetTypeSupportForType(typeof(T));
        using var serializer = typeSupport.CreateSerializer();
        DynamicData? fixture = null;
        if (options.CaseId is not ("09-optional-recursive-array" or "05-unbounded-sequence-aliases"
            or "07-union-char-numeric" or "07-union-string-sequence-aliases"))
        {
            fixture = FixtureCatalog.CreateFixture(typeSupport.DynamicType, options.CaseId, options.Fixture);
        }

        using var expectedDynamic = fixture;
        var expected = options.CaseId switch
        {
            "09-optional-recursive-array" => CreateOptionalRecursiveArrayFixture<T>(options.Fixture),
            "09-optional-string-sequences" => CreateOptionalStringSequenceFixture<T>(options.Fixture),
            "09-optional-aggregate-member" => CreateOptionalAggregateMemberFixture<T>(options.Fixture),
            "07-union-char-boundaries" => CreateCharacterUnionFixture<T>(options.Fixture),
#if WIRE_COMPATIBILITY_UNBOUNDED_SEQUENCE_ALIASES
            "05-unbounded-sequence-aliases" => CreateUnboundedSequenceAliasesFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_UNION_STRING_SEQUENCE_ALIASES
            "07-union-string-sequence-aliases" => CreateStringSequenceUnionFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_NUMERIC_CHAR_UNION
            "07-union-char-numeric" => CreateNumericCharacterUnionFixture<T>(options.Fixture),
#endif
            _ => serializer.FromDynamicData(expectedDynamic!)
        };

        var participantQos = DomainParticipantFactory.Instance.DefaultParticipantQos;
        if (options.CaseId == "09-optional-recursive-array")
        {
            // RTI 7.7.0 cannot register recursive types with TypeObject v2 enabled.
            participantQos = participantQos.WithResourceLimits(policy =>
            {
                policy.TypeObjectMaxSerializedLength = 0;
                policy.TypeCodeMaxSerializedLength = 0;
            });
        }

        using var participant = DomainParticipantFactory.Instance.CreateParticipant(domainId: 0, participantQos);
        using var topic = participant.CreateTopic<T>(options.TopicName);

        if (options.Role == PeerRole.Writer)
        {
            Publish(participant, topic, expected, options);
        }
        else
        {
            Receive(participant, topic, expected, expectedDynamic, options);
        }
    }

#if WIRE_COMPATIBILITY_UNBOUNDED_SEQUENCE_ALIASES
    private static T CreateUnboundedSequenceAliasesFixture<T>(string fixture)
    {
        var sample = new CorpusUnboundedSequences.SequenceAliasesSample();
        if (fixture == "sequence-empty")
        {
            return (T)(object)sample;
        }
        if (fixture != "sequence-multiple")
        {
            throw new FixtureException($"Unknown unbounded sequence fixture '{fixture}'.");
        }

        sample.numbers.Value.Value.Add(101);
        sample.numbers.Value.Value.Add(-102);
        sample.states.Value.Add(CorpusUnboundedSequences.State.Idle);
        sample.states.Value.Add(CorpusUnboundedSequences.State.Ready);
        sample.children.Value.Add(new CorpusUnboundedSequences.Child(301));
        sample.children.Value.Add(new CorpusUnboundedSequences.Child(-302));
        sample.scalars.Value.Add(new CorpusUnboundedSequences.Scalar(201));
        sample.scalars.Value.Add(new CorpusUnboundedSequences.Scalar(-202));
        sample.texts.Value.Add("narrow-1");
        sample.texts.Value.Add("narrow-2");
        sample.wideTexts.Value.Add("wide-λ");
        sample.wideTexts.Value.Add("wide-2");
        sample.boundedTexts.Value.Add("12345678");
        sample.boundedTexts.Value.Add("bounded");
        sample.boundedWideTexts.Value.Add("ABCD");
        sample.boundedWideTexts.Value.Add("wide");
        return (T)(object)sample;
    }
#endif

#if WIRE_COMPATIBILITY_UNION_STRING_SEQUENCE_ALIASES
    private static T CreateStringSequenceUnionFixture<T>(string fixture)
    {
        var sample = new RuntimeUnionStringSequences.StringSequenceUnionsSample();
        var wide = fixture is "union-direct-wide" or "union-alias-wide" or "union-bounded-wide";
        switch (fixture)
        {
            case "union-direct-narrow":
            case "union-direct-wide":
                var directValues = new Sequence<string>();
                directValues.Add(wide ? "wide-λ" : "narrow-1");
                directValues.Add(wide ? "wide-2" : "narrow-2");
                directValues.Add(wide ? "wide-3" : "narrow-3");
                var direct = new RuntimeUnionStringSequences.DirectStringSequences();
                if (wide)
                {
                    direct.wide = directValues;
                }
                else
                {
                    direct.narrow = directValues;
                }
                sample.direct = direct;
                break;
            case "union-alias-narrow":
            case "union-alias-wide":
                var aliasValues = new RuntimeUnionStringSequences.StringSequenceAliases();
                if (wide)
                {
                    var values = new RuntimeUnionStringSequences.WideTexts();
                    values.Value.Add("wide-λ");
                    values.Value.Add("wide-2");
                    values.Value.Add("wide-3");
                    aliasValues.wide = values;
                }
                else
                {
                    var values = new RuntimeUnionStringSequences.NarrowTexts();
                    values.Value.Add("narrow-1");
                    values.Value.Add("narrow-2");
                    values.Value.Add("narrow-3");
                    aliasValues.narrow = values;
                }
                sample.aliases = aliasValues;
                break;
            case "union-bounded-narrow":
            case "union-bounded-wide":
                var boundedValues = new RuntimeUnionStringSequences.StringSequenceAliases();
                if (wide)
                {
                    var values = new RuntimeUnionStringSequences.BoundedWideTexts();
                    values.Value.Add("ABCD");
                    values.Value.Add("wide");
                    values.Value.Add("LAST");
                    boundedValues.boundedWide = values;
                }
                else
                {
                    var values = new RuntimeUnionStringSequences.BoundedNarrowTexts();
                    values.Value.Add("12345678");
                    values.Value.Add("bounded");
                    values.Value.Add("ABCDEFGH");
                    boundedValues.boundedNarrow = values;
                }
                sample.aliases = boundedValues;
                break;
            default:
                throw new FixtureException($"Unknown string-sequence union fixture '{fixture}'.");
        }
        return (T)(object)sample;
    }
#endif

#if WIRE_COMPATIBILITY_NUMERIC_CHAR_UNION
    private static T CreateNumericCharacterUnionFixture<T>(string fixture)
    {
        var choice = new CorpusNumericCharUnion.Choice();
        switch (fixture)
        {
            case "union-127":
                choice.Setvalue(13701, (char)127);
                break;
            case "union-255":
                choice.Setvalue(13702, (char)255);
                break;
            case "union-default":
                choice.Setfallback(13703, (char)65);
                break;
            default:
                throw new FixtureException($"Unknown numeric character union fixture '{fixture}'.");
        }
        return (T)(object)choice;
    }
#endif

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
        DynamicData? expectedDynamic,
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
        var invalidSamples = 0;

        while (deadline.Elapsed < Timeout)
        {
            using var samples = reader.Take();
            takeCalls++;
            var validData = new List<T>();
            foreach (var sample in samples)
            {
                returnedSamples++;
                if (sample.Info.ValidData)
                {
                    validSamples++;
                    validData.Add(sample.Data);
                }
                else
                {
                    invalidSamples++;
                }
            }

            // ValidData=false also describes ordinary instance lifecycle notifications,
            // including a writer exiting after its reliable sample was acknowledged.
            // Count those notifications, but require and verify a valid payload.
            foreach (var sample in validData)
            {
                try
                {
                    if (options.CaseId == "07-union-char-boundaries")
                    {
                        IndependentSampleVerifier.VerifyCharacterUnionFixture(
                            expectedDynamic!.Type, sample!, options.Fixture);
                    }
                    else if (options.CaseId == "09-optional-string-sequences")
                    {
                        IndependentSampleVerifier.VerifyOptionalStringSequenceFixture(
                            sample!, options.Fixture);
                    }
                    else if (options.CaseId == "09-optional-recursive-array")
                    {
                        IndependentSampleVerifier.VerifyOptionalRecursiveArrayFixture(
                            expected!, sample!, $"{options.CaseId} fixture {options.Fixture}");
                    }
                    else if (options.CaseId == "09-optional-aggregate-member")
                    {
                        IndependentSampleVerifier.VerifyOptionalAggregateMemberFixture(
                            expected!,
                            sample!,
                            $"{options.CaseId} fixture {options.Fixture}");
                    }
                    else if (options.CaseId == "05-unbounded-sequence-aliases")
                    {
                        if (!EqualityComparer<T>.Default.Equals(expected, sample))
                        {
                            throw new InvalidDataException(
                                $"Expected {expected}; received {sample} for {options.CaseId} fixture {options.Fixture}.");
                        }
                    }
                    else if (options.CaseId == "07-union-char-numeric")
                    {
                        if (!EqualityComparer<T>.Default.Equals(expected, sample))
                        {
                            throw new InvalidDataException(
                                $"Expected {expected}; received {sample} for {options.CaseId} fixture {options.Fixture}.");
                        }
                    }
                    else if (options.CaseId == "07-union-string-sequence-aliases")
                    {
                        if (!EqualityComparer<T>.Default.Equals(expected, sample))
                        {
                            throw new InvalidDataException(
                                $"Expected {expected}; received {sample} for {options.CaseId} fixture {options.Fixture}.");
                        }
                    }
                    else
                    {
                        IndependentSampleVerifier.Verify(
                            expectedDynamic!,
                            sample!,
                            $"{options.CaseId} fixture {options.Fixture}");
                    }
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
        throw new TimeoutException(
            $"No sample received for {options.CaseId} fixture {options.Fixture}.");
    }

    /// <summary>Seeds character branches directly so RTI DynamicData signedness cannot alter the fixture.</summary>
    private static T CreateCharacterUnionFixture<T>(string fixture)
    {
        var sample = Activator.CreateInstance<T>()!;
        var branch = fixture switch
        {
            "union-minimum" => "minimum",
            "union-maximum" => "maximum",
            "union-default" => "fallback",
            _ => throw new FixtureException($"Unknown character union fixture '{fixture}'.")
        };
        typeof(T).GetProperty(branch)!.SetValue(sample, 8201);
        return sample;
    }

    /// <summary>Constructs a finite recursive tree without requesting public DynamicType metadata.</summary>
    private static T CreateOptionalRecursiveArrayFixture<T>(string fixture)
    {
        if (fixture is not ("optional-absent" or "optional-present" or "optional-nested"))
        {
            throw new FixtureException($"Unknown optional recursive array fixture '{fixture}'.");
        }

        var root = CreateNode(9801);
        if (fixture != "optional-absent")
        {
            var children = new[] { CreateNode(9811), CreateNode(9812) };
            typeof(T).GetProperty("children")!.SetValue(root, children);
            if (fixture == "optional-nested")
            {
                typeof(T).GetProperty("children")!.SetValue(children[0], new[] { CreateNode(9821), CreateNode(9822) });
            }
        }

        return root;

        T CreateNode(int value)
        {
            var node = Activator.CreateInstance<T>()!;
            typeof(T).GetProperty("value")!.SetValue(node, value);
            return node;
        }
    }

    private static T CreateOptionalAggregateMemberFixture<T>(string fixture)
    {
        var sample = Activator.CreateInstance<T>()!;
        if (fixture == "optional-absent")
        {
            return sample;
        }

        if (fixture != "optional-present")
        {
            throw new FixtureException($"Unknown optional aggregate fixture '{fixture}'.");
        }

        var sampleType = typeof(T);
        var payloadType = sampleType.Assembly.GetType($"{sampleType.Namespace}.Payload", throwOnError: true)!;
        var choiceType = GetMemberValueType(sampleType, "choice");
        var choiceAliasType = GetMemberValueType(sampleType, "choiceAlias");
        SetPayloadMember(sample, "payload", payloadType, 9701);
        SetPayloadMember(sample, "payloadAlias", payloadType, 9703);
        SetChoiceMember(sample, "choice", choiceType, payloadType, 9702);
        SetChoiceMember(sample, "choiceAlias", choiceAliasType, payloadType, 9704);
        return sample;
    }

    private static Type GetMemberValueType(Type sampleType, string memberName)
    {
        var memberType = sampleType.GetProperty(memberName)!.PropertyType;
        return Nullable.GetUnderlyingType(memberType) ?? memberType;
    }

    private static void SetPayloadMember<T>(T sample, string memberName, Type payloadType, int value)
    {
        var payload = Activator.CreateInstance(payloadType)!;
        payloadType.GetProperty("value")!.SetValue(payload, value);
        typeof(T).GetProperty(memberName)!.SetValue(sample, payload);
    }

    private static void SetChoiceMember<T>(
        T sample,
        string memberName,
        Type choiceType,
        Type payloadType,
        int value)
    {
        var payload = Activator.CreateInstance(payloadType)!;
        payloadType.GetProperty("value")!.SetValue(payload, value);
        var choice = Activator.CreateInstance(choiceType)!;
        choiceType.GetProperty("payload")!.SetValue(choice, payload);
        typeof(T).GetProperty(memberName)!.SetValue(sample, choice);
    }

    private static void WriteReaderStatistics<T>(
        DataReader<T> reader,
        int takeCalls,
        int returnedSamples,
        int validSamples,
        int invalidSamples)
    {
        var matchStatus = reader.SubscriptionMatchedStatus;
        Console.WriteLine(
            $"READER_STATS matched_publications_current={matchStatus.CurrentCount} " +
            $"matched_publications_total={matchStatus.TotalCount} take_calls={takeCalls} " +
            $"returned_samples={returnedSamples} valid_samples={validSamples} " +
            $"invalid_samples={invalidSamples}");
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
