#nullable enable annotations

using System.Collections;
using System.Reflection;
using System.Text.Json;
using Omg.Types.Dynamic;
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
        VerifySnapshots(
            SnapshotDynamic(expectedFixture),
            SnapshotManaged(expectedFixture.Type, actualSample),
            context);
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
        VerifySnapshots(expected, SnapshotOptionalStringSequenceManaged(actualSample), $"optional string fixture {fixture}");
    }

    private static object SnapshotOptionalStringSequenceManaged(object value) =>
        new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["narrowValues"] = SnapshotOptionalSequence(GetManagedMember(value, "narrowValues")),
            ["wideValues"] = SnapshotOptionalSequence(GetManagedMember(value, "wideValues"))
        };

    private static object? SnapshotOptionalSequence(object? value) => value is null
        ? null
        : SequenceSnapshot(((IEnumerable)value).Cast<string>());

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

                    members.Add(member.Name, SnapshotDynamicMember(data, member.Name, member.Type));
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
                    ["$discriminator"] = NormalizeDiscriminator(discriminator),
                    [selectedMember.Name] = SnapshotDynamicMember(
                        data,
                        selectedMember.Name,
                        selectedMember.Type)
                };
            }
            case SequenceType sequence:
            {
                var elements = new List<object?>();
                for (var index = 0; index < data.MemberCount; index++)
                {
                    elements.Add(SnapshotDynamicMember(data, index, sequence.ContentType));
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
                    elements.Add(SnapshotDynamicMember(data, index, array.ContentType));
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
            {
                var alias = (AliasType)data.Type;
                if (IsComplexDynamicType(alias.RelatedType))
                {
                    using var relatedValue = data.LoanValue(0);
                    return SnapshotDynamic(relatedValue.Data);
                }

                return SnapshotDynamicValue(alias.RelatedType, data.GetAnyValue(0));
            }
            default:
                throw new InvalidDataException(
                    $"Cannot independently inspect DynamicData type {data.Type.GetType().Name}.");
        }
    }

    private static object? SnapshotDynamicMember(
        DynamicData parent,
        string memberName,
        IDynamicType memberType)
    {
        if (IsComplexDynamicType(memberType))
        {
            using var member = parent.LoanValue(memberName);
            return SnapshotDynamic(member.Data);
        }

        return SnapshotDynamicValue(memberType, parent.GetAnyValue(memberName));
    }

    private static object? SnapshotDynamicMember(
        DynamicData parent,
        int memberIndex,
        IDynamicType memberType)
    {
        var dynamicMemberId = parent.Type is SequenceType or ArrayType ? memberIndex + 1 : memberIndex;
        if (IsComplexDynamicType(memberType))
        {
            using var member = parent.LoanValue(dynamicMemberId);
            return SnapshotDynamic(member.Data);
        }

        try
        {
            return SnapshotDynamicValue(memberType, parent.GetAnyValue(dynamicMemberId));
        }
        catch (InvalidOperationException exception) when (
            exception.Message.Contains("use LoanValue", StringComparison.Ordinal))
        {
            using var member = parent.LoanValue(dynamicMemberId);
            return SnapshotDynamic(member.Data);
        }
    }

    private static bool IsComplexDynamicType(IDynamicType type) => type switch
    {
        AliasType alias => IsComplexDynamicType(alias.RelatedType),
        StructType or UnionType or SequenceType or ArrayType => true,
        _ => false
    };

    private static bool IsCollectionAlias(AliasType alias) => alias.RelatedType switch
    {
        AliasType nestedAlias => IsCollectionAlias(nestedAlias),
        SequenceType or ArrayType => true,
        _ => false
    };

    private static object? SnapshotDynamicValue(IDynamicType expectedType, object? value)
    {
        if (value is DynamicData nestedData)
        {
            using (nestedData)
            {
                return SnapshotDynamic(nestedData);
            }
        }

        if (expectedType is AliasType alias)
        {
            return SnapshotDynamicValue(alias.RelatedType, value);
        }

        if (expectedType is SequenceType sequenceType && value is IEnumerable sequence)
        {
            return new SortedDictionary<string, object?>(StringComparer.Ordinal)
            {
                ["$kind"] = "sequence",
                ["$items"] = sequence.Cast<object?>()
                    .Select(item => SnapshotDynamicValue(sequenceType.ContentType, item))
                    .ToArray()
            };
        }

        if (expectedType is ArrayType arrayType && value is IEnumerable array)
        {
            return new SortedDictionary<string, object?>(StringComparer.Ordinal)
            {
                ["$dimensions"] = Enumerable.Range(0, checked((int)arrayType.DimensionCount))
                    .Select(index => arrayType.GetDimension(checked((uint)index)))
                    .ToArray(),
                ["$kind"] = "array",
                ["$items"] = array.Cast<object?>()
                    .Select(item => SnapshotDynamicValue(arrayType.ContentType, item))
                    .ToArray()
            };
        }

        return NormalizeScalar(value);
    }

    private static object? SnapshotManaged(IDynamicType expectedType, object? value)
    {
        if (value is null)
        {
            return null;
        }

        if (expectedType is AliasType alias)
        {
            if (IsCollectionAlias(alias))
            {
                var valueProperty = value.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public)
                    ?? throw new InvalidDataException(
                        $"Managed collection alias {value.GetType().FullName} has no public Value property.");
                return SnapshotManaged(alias.RelatedType, valueProperty.GetValue(value));
            }

            return SnapshotManaged(alias.RelatedType, value);
        }

        var valueType = value.GetType();
        if (value is Rti.Types.LongDouble longDouble)
        {
            return NormalizeScalar(longDouble.ToDecimal());
        }

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
            var items = array.Cast<object?>()
                .Select(item => SnapshotManaged(((ArrayType)expectedType).ContentType, item))
                .ToArray();
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
                ["$items"] = sequence.Cast<object?>()
                    .Select(item => SnapshotManaged(((SequenceType)expectedType).ContentType, item))
                    .ToArray()
            };
        }

        if (expectedType is StructType structure)
        {
            var members = new SortedDictionary<string, object?>(StringComparer.Ordinal);
            foreach (var member in EnumerateMembers(structure))
            {
                members.Add(member.Name, SnapshotManaged(member.Type, GetManagedMember(value, member.Name)));
            }

            return members;
        }

        var properties = new SortedDictionary<string, object?>(StringComparer.Ordinal);
        var discriminator = valueType.GetProperty("Discriminator", BindingFlags.Instance | BindingFlags.Public);
        if (discriminator is not null)
        {
            var discriminatorValue = discriminator.GetValue(value)
                ?? throw new InvalidDataException($"Managed union {valueType.FullName} has a null discriminator.");
            var selectedMember = FindUnionMember((UnionType)expectedType, discriminatorValue);
            properties["$branch"] = selectedMember.Name;
            properties["$discriminator"] = NormalizeDiscriminator(discriminatorValue);
            properties[selectedMember.Name] = SnapshotManaged(
                selectedMember.Type,
                GetManagedMember(value, selectedMember.Name));
            return properties;
        }

        return NormalizeScalar(value);
    }

    private static object? GetManagedMember(object value, string memberName)
    {
        var property = value.GetType().GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public)
            ?? throw new InvalidDataException(
                $"Managed sample {value.GetType().FullName} has no public member '{memberName}'.");
        return property.GetValue(value);
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
        Rti.Types.LongDouble longDouble => longDouble.ToDecimal(),
        _ => value
    };

    private static object? NormalizeDiscriminator(object value) => value switch
    {
        bool boolean => boolean ? 1 : 0,
        char character => (int)character,
        _ => NormalizeScalar(value)
    };
}
