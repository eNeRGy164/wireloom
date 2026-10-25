
using System.Collections;
using Omg.Types.Dynamic;

namespace Wireloom.Dds.Generator.TestSupport;

public static partial class IndependentSampleVerifier
{
    /// <summary>Checks character boundary values independently of RTI's DynamicData branch selection.</summary>
    public static void VerifyCharacterUnionFixture(IDynamicType type, object actualSample, string fixture)
    {
        var (branch, discriminator) = fixture switch
        {
            "union-minimum" => ("minimum", 0L),
            "union-maximum" => ("maximum", 126L),
            "union-default" => ("fallback", 1L),
            _ => throw new InvalidDataException($"Unknown character union fixture '{fixture}'.")
        };
        var expected = new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["$branch"] = branch,
            ["$discriminator"] = discriminator,
            [branch] = 8201
        };
        VerifySnapshots(expected, SnapshotManaged(type, actualSample), $"character union fixture {fixture}");
    }

    /// <summary>Checks optional aggregate members against an independently populated fixture.</summary>
    // ReSharper disable once UnusedMember.Global
    // Called by the WireCompatibility project, which Qodana analyzes separately.
    public static void VerifyOptionalAggregateMemberFixture(object expectedSample, object actualSample, string context)
    {
        VerifySnapshots(
            SnapshotOptionalAggregateManaged(expectedSample),
            SnapshotOptionalAggregateManaged(actualSample),
            context);
    }

    /// <summary>Checks the complete finite recursive tree through public members.</summary>
    // ReSharper disable once UnusedMember.Global
    // Called by the WireCompatibility project, which Qodana analyzes separately.
    public static void VerifyOptionalRecursiveArrayFixture(object expectedSample, object actualSample, string context) =>
        VerifySnapshots(SnapshotRecursiveNode(expectedSample, depth: 0), SnapshotRecursiveNode(actualSample, depth: 0), context);

    private static object SnapshotRecursiveNode(object sample, int depth)
    {
        if (depth > 2)
        {
            throw new InvalidDataException($"Recursive fixture exceeds its expected tree depth at level {depth}.");
        }

        var type = sample.GetType();
        var children = (Array?)type.GetProperty("children")!.GetValue(sample);
        object[]? childSnapshots = null;
        if (children is not null)
        {
            if (children.Rank != 1 || children.Length != 2)
            {
                throw new InvalidDataException("Recursive fixture children must have exactly two elements.");
            }

            childSnapshots = children.Cast<object?>().Select(child =>
                SnapshotRecursiveNode(child ?? throw new InvalidDataException("Recursive fixture has a null child."), depth + 1)).ToArray();
        }

        return new { value = type.GetProperty("value")!.GetValue(sample), children = childSnapshots };
    }

    private static SortedDictionary<string, object?> SnapshotOptionalAggregateManaged(object sample)
    {
        var sampleType = sample.GetType();
        var payload = sampleType.GetProperty("payload")!.GetValue(sample);
        var payloadAlias = sampleType.GetProperty("payloadAlias")!.GetValue(sample);
        var choice = sampleType.GetProperty("choice")!.GetValue(sample);
        var choiceAlias = sampleType.GetProperty("choiceAlias")!.GetValue(sample);

        return new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["payload"] = SnapshotPayload(payload),
            ["payloadAlias"] = SnapshotPayload(payloadAlias),
            ["choice"] = SnapshotPayloadUnion(choice),
            ["choiceAlias"] = SnapshotPayloadUnion(choiceAlias)
        };
    }

    private static SortedDictionary<string, object?>? SnapshotPayload(object? payload) => payload is null
        ? null
        : new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["value"] = Convert.ToInt32(
                payload.GetType().GetProperty("value")!.GetValue(payload),
                System.Globalization.CultureInfo.InvariantCulture)
        };

    private static SortedDictionary<string, object?>? SnapshotPayloadUnion(object? choice)
    {
        if (choice is null)
        {
            return null;
        }

        var choiceType = choice.GetType();
        var discriminator = Convert.ToInt32(
            choiceType.GetProperty("Discriminator")!.GetValue(choice),
            System.Globalization.CultureInfo.InvariantCulture);
        if (discriminator != 0)
        {
            throw new InvalidDataException(
                $"Optional aggregate union selected discriminator {discriminator}; expected branch 0.");
        }
        var selectedPayload = choiceType.GetProperty("payload")!.GetValue(choice);

        return new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["$branch"] = "payload",
            ["$discriminator"] = discriminator,
            ["payload"] = SnapshotPayload(selectedPayload)
        };
    }

    /// <summary>Checks optional narrow and wide string sequences for the named fixture.</summary>
    // ReSharper disable once UnusedMember.Global
    // Called by the WireCompatibility project, which Qodana analyzes separately.
    public static void VerifyOptionalStringSequenceFixture(object actualSample, string fixture)
    {
        var expected = new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["narrowValues"] = fixture switch
            {
                "optional-absent" or "optional-wide-only" => null,
                "optional-empty" => SequenceSnapshot(Array.Empty<string>()),
                "optional-narrow-only" => SequenceSnapshot(["narrow-only"]),
                "optional-multiple" => SequenceSnapshot(["narrow-one", "narrow-two", "narrow-three"]),
                _ => throw new InvalidDataException($"Unknown optional string fixture '{fixture}'.")
            },
            ["wideValues"] = fixture switch
            {
                "optional-absent" or "optional-narrow-only" => null,
                "optional-empty" => SequenceSnapshot(Array.Empty<string>()),
                "optional-wide-only" => SequenceSnapshot(["wide-only"]),
                "optional-multiple" => SequenceSnapshot(["wide-one", "wide-two"]),
                _ => throw new InvalidDataException($"Unknown optional string fixture '{fixture}'.")
            }
        };

        VerifySnapshots(expected, SnapshotOptionalStringSequenceManaged(actualSample), $"optional string fixture {fixture}");
    }

    private static SortedDictionary<string, object?> SnapshotOptionalStringSequenceManaged(object value) =>
        new(StringComparer.Ordinal)
        {
            ["narrowValues"] = SnapshotOptionalSequence(GetManagedMember(value, "narrowValues")),
            ["wideValues"] = SnapshotOptionalSequence(GetManagedMember(value, "wideValues"))
        };

    private static SortedDictionary<string, object?>? SnapshotOptionalSequence(object? value) => value is null
        ? null
        : SequenceSnapshot(((IEnumerable)value).Cast<string>());

    private static SortedDictionary<string, object?> SequenceSnapshot(IEnumerable<string> values) =>
        new(StringComparer.Ordinal)
        {
            ["$kind"] = "sequence",
            ["$items"] = values.Cast<object?>().ToArray()
        };
}
