using System.Collections;
using System.Reflection;
using System.Text.Json;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>
/// Compares a generated managed sample to the fixture through public member
/// values, independently of Wireloom's generated conversion and equality code.
/// </summary>
internal static class IndependentSampleVerifier
{
    internal static void Verify(DynamicData expectedFixture, object actualSample, string context)
    {
        VerifySnapshots(SnapshotDynamic(expectedFixture), SnapshotManaged(actualSample), context);
    }

    internal static void VerifyOptionalStringSequenceFixture(object actualSample, string fixture)
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
        VerifySnapshots(expected, SnapshotManaged(actualSample), $"optional string fixture {fixture}");
    }

    private static object SequenceSnapshot(IEnumerable<string> values) =>
        new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["$kind"] = "sequence",
            ["$items"] = values.Cast<object?>().ToArray()
        };

    private static void VerifySnapshots(object? expectedSnapshot, object? actualSnapshot, string context)
    {
        var expected = JsonSerializer.Serialize(expectedSnapshot);
        var actual = JsonSerializer.Serialize(actualSnapshot);
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                $"Received sample differs from {context}. Expected {expected}; received {actual}.");
        }
    }

    private static object? SnapshotDynamic(DynamicData data)
    {
        switch (data.Type)
        {
            case StructType structure:
            {
                var members = new SortedDictionary<string, object?>(StringComparer.Ordinal);
                foreach (var member in EnumerateMembers(structure))
                {
                    if (!data.MemberExists(member.Name))
                    {
                        members.Add(member.Name, null);
                        continue;
                    }

                    members.Add(member.Name, SnapshotDynamicValue(data.GetAnyValue(member.Name)));
                }

                return members;
            }
            case UnionType union:
            {
                var discriminator = data.GetDiscriminator();
                var selectedMember = FindUnionMember(union, discriminator);
                return new SortedDictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["$branch"] = selectedMember.Name,
                    ["$discriminator"] = NormalizeScalar(discriminator),
                    [selectedMember.Name] = SnapshotDynamicValue(data.GetAnyValue(selectedMember.Name))
                };
            }
            case SequenceType sequence:
            {
                var elements = new List<object?>();
                for (var index = 0; index < data.MemberCount; index++)
                {
                    elements.Add(SnapshotDynamicValue(data.GetAnyValue(index)));
                }

                return new SortedDictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["$kind"] = "sequence",
                    ["$items"] = elements
                };
            }
            case ArrayType array:
            {
                var elements = new List<object?>();
                for (var index = 0; index < array.TotalElementCount; index++)
                {
                    elements.Add(SnapshotDynamicValue(data.GetAnyValue(index)));
                }

                var dimensions = Enumerable.Range(0, checked((int)array.DimensionCount))
                    .Select(index => array.GetDimension(checked((uint)index)))
                    .ToArray();
                return new SortedDictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["$dimensions"] = dimensions,
                    ["$kind"] = "array",
                    ["$items"] = elements
                };
            }
            case AliasType:
                return SnapshotDynamicValue(data.GetAnyValue(0));
            default:
                throw new InvalidDataException(
                    $"Cannot independently inspect DynamicData type {data.Type.GetType().Name}.");
        }
    }

    private static object? SnapshotDynamicValue(object? value)
    {
        if (value is DynamicData nestedData)
        {
            using (nestedData)
            {
                if (nestedData.Type is AliasType)
                {
                    return SnapshotDynamicValue(nestedData.GetAnyValue(0));
                }

                return SnapshotDynamic(nestedData);
            }
        }

        return NormalizeScalar(value);
    }

    private static object? SnapshotManaged(object? value)
    {
        if (value is null)
        {
            return null;
        }

        var valueType = value.GetType();
        if (value is string || valueType.IsPrimitive || value is decimal)
        {
            return NormalizeScalar(value);
        }

        if (valueType.IsEnum)
        {
            return Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        if (value is Array array)
        {
            var items = array.Cast<object?>().Select(SnapshotManaged).ToArray();
            return new SortedDictionary<string, object?>(StringComparer.Ordinal)
            {
                ["$dimensions"] = Enumerable.Range(0, array.Rank).Select(array.GetLength).ToArray(),
                ["$kind"] = "array",
                ["$items"] = items
            };
        }

        if (value is IEnumerable sequence)
        {
            return new SortedDictionary<string, object?>(StringComparer.Ordinal)
            {
                ["$kind"] = "sequence",
                ["$items"] = sequence.Cast<object?>().Select(SnapshotManaged).ToArray()
            };
        }

        var properties = new SortedDictionary<string, object?>(StringComparer.Ordinal);
        var discriminator = valueType.GetProperty("Discriminator", BindingFlags.Instance | BindingFlags.Public);
        if (discriminator is not null)
        {
            properties["$discriminator"] = NormalizeScalar(discriminator.GetValue(value));
        }

        foreach (var property in valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     .Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
                     .OrderBy(property => property.Name, StringComparer.Ordinal))
        {
            if (property.Name == "Discriminator")
            {
                continue;
            }

            try
            {
                properties[property.Name] = SnapshotManaged(property.GetValue(value));
            }
            catch (TargetInvocationException exception)
                when (exception.InnerException is InvalidOperationException)
            {
                // Generated union getters reject access to an inactive branch.
            }
        }

        if (discriminator is not null)
        {
            properties["$branch"] = properties.Keys.FirstOrDefault(key => !key.StartsWith('$'));
        }

        return properties;
    }

    private static IEnumerable<StructMember> EnumerateMembers(StructType structure)
    {
        if (structure.Parent is not null)
        {
            foreach (var inherited in EnumerateMembers(structure.Parent))
            {
                yield return inherited;
            }
        }

        foreach (var member in structure.Members)
        {
            yield return member;
        }
    }

    private static UnionMember FindUnionMember(UnionType union, object discriminator)
    {
        var label = Convert.ToInt32(discriminator, System.Globalization.CultureInfo.InvariantCulture);
        var explicitMember = union.Members.FirstOrDefault(member => member.Labels.Contains(label));
        return explicitMember
            ?? union.Members.FirstOrDefault(member => member.Labels.Contains(UnionMember.DefaultLabel))
            ?? throw new InvalidDataException($"Union discriminator {label} has no declared branch.");
    }

    private static object? NormalizeScalar(object? value) => value switch
    {
        null => null,
        Enum enumValue => Convert.ToInt64(enumValue, System.Globalization.CultureInfo.InvariantCulture),
        _ => value
    };
}
