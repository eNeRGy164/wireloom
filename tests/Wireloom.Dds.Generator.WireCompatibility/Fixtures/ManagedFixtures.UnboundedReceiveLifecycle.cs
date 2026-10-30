#nullable enable annotations

using System.Collections;
using System.Reflection;
using Rti.Dds.Topics;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Creates deterministic five-phase payloads for unbounded receive-lifecycle qualification.</summary>
internal static class UnboundedReceiveLifecycleFixture
{
    internal static object Create(Type sampleType, int phaseId, bool fragmented)
    {
        var sample = Activator.CreateInstance(sampleType)
            ?? throw new InvalidOperationException($"Could not create {sampleType.FullName}.");
        Initialize(sample, phaseId, fragmented);
        return sample;
    }

    internal static void Initialize(object sample, int phaseId, bool fragmented)
    {
        ArgumentNullException.ThrowIfNull(sample);
        var sampleType = sample.GetType();

        if (phaseId is < 0 or >= 5)
        {
            throw new ArgumentOutOfRangeException(nameof(phaseId));
        }

        var empty = phaseId is 0 or 3;
        var valuesCount = empty ? 0 : phaseId switch
        {
            1 => 257,
            2 => 3,
            4 => 521,
            _ => throw new InvalidOperationException("Unknown lifecycle phase."),
        };

        var stringLength = empty ? 0 : phaseId switch
        {
            1 => fragmented ? 170 * 1024 : 320,
            2 => 13,
            4 => fragmented ? 350 * 1024 : 640,
            _ => throw new InvalidOperationException("Unknown lifecycle phase."),
        };

        var wideStringLength = empty ? 0 : phaseId switch
        {
            1 => fragmented ? 2_048 : 320,
            2 => 9,
            4 => fragmented ? 4_096 : 640,
            _ => throw new InvalidOperationException("Unknown lifecycle phase."),
        };

        SetMember(sample, "phaseId", phaseId);
        SetMember(sample, "text", Pattern(stringLength, phaseId, narrow: true));
        SetMember(sample, "wideText", Pattern(wideStringLength, phaseId, narrow: false));
        SetMember(sample, "boundedText", $"B{phaseId:00000}");

        if (phaseId is 0 or 3)
        {
            SetMember(sample, "optionalState", null!);
        }
        else
        {
            var optionalType = sampleType.GetProperty("optionalState")?.PropertyType
                ?? throw new InvalidOperationException($"Member {sampleType.FullName}.optionalState was not found.");
            var optionalState = Activator.CreateInstance(optionalType)
                ?? throw new InvalidOperationException($"Could not create {optionalType.FullName}.");
            SetMember(optionalState, "value", 17400 + phaseId);
            SetMember(optionalState, "note", phaseId == 2 ? null! : $"optional-{phaseId}-note");
            ReplaceSequence(GetMember(optionalState, "labels"), Enumerable.Range(0, phaseId == 2 ? 1 : phaseId == 4 ? 5 : 3)
                .Select(index => $"optional-{phaseId}-label-{index}-{Pattern(phaseId == 4 ? 32 : 7, phaseId + index, narrow: true)}"));
            SetMember(sample, "optionalState", optionalState);
        }

        ReplaceSequence(GetMember(sample, "values"), Enumerable.Range(0, valuesCount).Select(index => Value(phaseId, index)));
        ReplaceSequence(GetMember(sample, "labels"), Enumerable.Range(0, empty ? 0 : phaseId == 2 ? 2 : 8)
            .Select(index => $"p{phaseId:D1}-l{index:D3}-{Pattern(empty ? 0 : phaseId == 2 ? 5 : fragmented && phaseId != 2 ? 320 : 300, phaseId + index, narrow: true)}"));

        var aliasValues = GetMember(sample, "aliasValues");
        ReplaceSequence(GetMember(aliasValues, "Value"), Enumerable.Range(0, valuesCount).Select(index => Value(phaseId + 1, index)));
        ReplaceSequence(GetMember(sample, "boundedValues"), Enumerable.Range(0, Math.Min(valuesCount, 4)).Select(index => Value(phaseId + 2, index)));
    }

    internal static void Verify(object actual, int phaseId, bool fragmented)
    {
        var expected = Create(actual.GetType(), phaseId, fragmented);
        VerifyMember(actual, expected, "phaseId");
        VerifyMember(actual, expected, "values");
        VerifyMember(actual, expected, "text");
        VerifyMember(actual, expected, "wideText");
        VerifyMember(actual, expected, "labels");
        VerifyMember(actual, expected, "aliasValues", "Value");
        VerifyMember(actual, expected, "boundedValues");
        VerifyMember(actual, expected, "boundedText");
        VerifyOptionalState(actual, expected, phaseId);
    }

    internal static int GetPhaseId(object sample) => (int)GetMember(sample, "phaseId");

    internal static int ExpectedSerializedSize<T>(ISerializer<T> serializer, object sample) =>
        serializer.Serialize((T)sample).Length;

    private static int Value(int phaseId, int index) => unchecked((phaseId + 1) * 1_000_003 + index * 97 - 41);

    private static string Pattern(int length, int seed, bool narrow)
    {
        var start = narrow ? 0x03BB : 0x03A9;
        var characters = new char[length];

        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] = (char)(start + ((seed + index * 7) % 4));
        }

        return new string(characters);
    }

    private static object GetMember(object instance, string name)
    {
        var type = instance.GetType();

        return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(instance)
            ?? type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(instance) ?? throw new InvalidOperationException($"Member {type.FullName}.{name} was not found or was null.");
    }

    private static object? GetNullableMember(object instance, string name)
    {
        var type = instance.GetType();
        return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(instance)
            ?? type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(instance);
    }

    private static void VerifyOptionalState(object actual, object expected, int phaseId)
    {
        var actualState = GetNullableMember(actual, "optionalState");
        var expectedState = GetNullableMember(expected, "optionalState");
        if (phaseId is 0 or 3)
        {
            if (actualState is not null || expectedState is not null)
            {
                throw new InvalidDataException("Optional nested state must be absent in the empty lifecycle phases.");
            }

            return;
        }

        if (actualState is null || expectedState is null
            || !Equals(GetMember(actualState, "value"), GetMember(expectedState, "value"))
            || !Equals(GetNullableMember(actualState, "note"), GetNullableMember(expectedState, "note"))
            || !((IEnumerable)GetMember(actualState, "labels")).Cast<object?>()
                .SequenceEqual(((IEnumerable)GetMember(expectedState, "labels")).Cast<object?>()))
        {
            throw new InvalidDataException($"Optional nested lifecycle state differs in phase {phaseId}.");
        }
    }

    private static void SetMember(object instance, string name, object value)
    {
        var type = instance.GetType();
        var property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property?.SetMethod is not null)
        {
            property.SetValue(instance, value);
            return;
        }

        var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new InvalidOperationException($"Member {type.FullName}.{name} was not found.");
        field.SetValue(instance, value);
    }

    private static void ReplaceSequence(object sequence, IEnumerable<int> values)
    {
        ReplaceSequence(sequence, values.Cast<object>());
    }

    private static void ReplaceSequence(object sequence, IEnumerable<string> values)
    {
        ReplaceSequence(sequence, values.Cast<object>());
    }

    private static void ReplaceSequence(object sequence, IEnumerable<object> values)
    {
        var type = sequence.GetType();
        var clear = type.GetMethod("Clear", Type.EmptyTypes) ?? throw new InvalidOperationException($"Sequence {type.FullName} does not expose Clear().");
        var add = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .SingleOrDefault(method => method.Name == "Add" && method.GetParameters().Length == 1) ?? throw new InvalidOperationException($"Sequence {type.FullName} does not expose Add(value).");
        clear.Invoke(sequence, null);

        foreach (var value in values)
        {
            add.Invoke(sequence, [value]);
        }
    }

    private static void VerifyMember(object actual, object expected, string name, string? nested = null)
    {
        var actualValue = GetMember(actual, name);
        var expectedValue = GetMember(expected, name);

        if (nested is not null)
        {
            actualValue = GetMember(actualValue, nested);
            expectedValue = GetMember(expectedValue, nested);
        }

        var matches = actualValue is IEnumerable actualItems and not string
            && expectedValue is IEnumerable expectedItems and not string
            ? actualItems.Cast<object?>().SequenceEqual(expectedItems.Cast<object?>())
            : Equals(actualValue, expectedValue);
        if (!matches)
        {
            throw new InvalidDataException($"Received lifecycle member {name}{(nested is null ? string.Empty : $".{nested}")} differs from its phase expectation.");
        }
    }
}
