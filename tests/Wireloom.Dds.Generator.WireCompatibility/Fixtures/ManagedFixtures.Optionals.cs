#nullable enable annotations

using Rti.Types;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds optional members and finite recursive trees through generated public properties.</summary>
internal static partial class ManagedFixtures
{
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

        static T CreateNode(int value)
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
}
