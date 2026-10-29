#nullable enable annotations

using System.Reflection;
using Rti.Dds.Core.Policy;
using Rti.Dds.Domain;
using Rti.Dds.Topics;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Owns one typed DDS exchange and the lifetime of its support, fixture, participant, and topic.</summary>
internal static class TypedPeer
{
    /// <summary>Creates the expected sample and runs the requested writer or reader.</summary>
    internal static void Run<T>(PeerOptions options)
    {
#if RTI_REFERENCE_PEER
        if (options.CaseId == "09-optional-recursive-array")
        {
            // RTI 7.7.0 array metadata reads Support.Instance before it is assigned.
            // Wireloom uses its recursive accessor and needs no bootstrap.
            var plugin = typeof(T).Assembly.GetType($"{typeof(T).Namespace}.Implementation.NodePlugin", throwOnError: true)!;
            plugin.GetMethod("CreateDynamicType", BindingFlags.Public | BindingFlags.Static)!.Invoke(null, [false]);
        }
#endif

        // Use Wireloom's generated type and serializer end-to-end. The reference
        // DynamicData fixture only seeds the expected managed sample; typed
        // DDS writers/readers exercise generated ToNative/FromNative code.
        var typeSupport = (TypeSupport<T>)TypeSupportHelper.GetTypeSupportForType(typeof(T));
        using var serializer = typeSupport.CreateSerializer();
        DynamicData? fixture = null;
        var isDirectRecursiveSequenceFixture = options.CaseId == "06-aggregates"
            && options.Fixture == "direct-recursive-sequence";
        if (!isDirectRecursiveSequenceFixture
            && options.CaseId is not ("09-optional-recursive-array" or "05-unbounded-sequence-aliases"
            or "07-union-char-numeric" or "07-union-char-numeric-255" or "07-union-string-sequence-aliases"
            or "09-defaults-ranges" or "01-boundaries" or "01-full-widths"
            or "10-default-literal-enum-array" or "05-array-of-sequences-idl"
            or "14-extensibility-full" or "14-autoid-scope" or "14-annotation-arguments"
            or "14-member-declarators" or "14-union-label-expressions" or "14-union-scoped-enum"
            or "14-union-array-branch" or "14-string-expression-bound" or "14-typed-defaults"
            or "14-enum-constant" or "14-unbounded-support"))
        {
            fixture = FixtureCatalog.CreateFixture(typeSupport.DynamicType, options.CaseId, options.Fixture);
        }

        using var expectedDynamic = fixture;
        var expected = ManagedFixtures.Create(options, serializer, expectedDynamic);
#if WIRE_COMPATIBILITY_UNBOUNDED_SUPPORT
        if (options.CaseId == "14-unbounded-support")
        {
            ManagedFixtures.VerifyUnboundedSupportFixture(expected!, options);
        }
        else
#endif
        if (options.CaseId.StartsWith("14-", StringComparison.Ordinal))
        {
            ManagedFixtures.VerifyIdlGapFixture(expected!, options);
        }

        if (options.FixtureOnly)
        {
            return;
        }

        var participantQos = DomainParticipantFactory.Instance.DefaultParticipantQos;
        var recursiveTypeObjectProfile =
            Environment.GetEnvironmentVariable("WIRELOOM_RECURSIVE_TYPEOBJECT_PROFILE") ?? "v1";
        if (isDirectRecursiveSequenceFixture && recursiveTypeObjectProfile == "v1")
        {
            participantQos = participantQos
                .WithResourceLimits(policy => policy.TypeObjectMaxSerializedLength = 8192)
                .WithDiscoveryConfig(policy => policy.EnabledBuiltinChannels = BuiltinChannelsKind.ServiceRequest);
        }
        else if (isDirectRecursiveSequenceFixture && recursiveTypeObjectProfile != "v2")
        {
            throw new ArgumentException(
                "WIRELOOM_RECURSIVE_TYPEOBJECT_PROFILE must be 'v1' or 'v2'.",
                nameof(options));
        }

        if (options.CaseId == "09-optional-recursive-array")
        {
            // RTI 7.7.0 cannot register recursive types with TypeObject v2 enabled.
            participantQos = participantQos.WithResourceLimits(policy =>
            {
                policy.TypeObjectMaxSerializedLength = 0;
                policy.TypeCodeMaxSerializedLength = 0;
            });
        }

        using var participant = DomainParticipantFactory.Instance.CreateParticipant(options.DomainId, participantQos);
        using var topic = participant.CreateTopic<T>(options.TopicName);

        if (options.Role == PeerRole.Writer)
        {
            PeerTransport.Publish(participant, topic, expected, options);
        }
        else
        {
            PeerTransport.Receive(participant, topic, expected, expectedDynamic, options);
        }
    }
}
