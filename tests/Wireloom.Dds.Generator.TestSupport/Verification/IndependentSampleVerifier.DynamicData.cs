#nullable enable annotations

using System.Collections;
using Omg.Types.Dynamic;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.TestSupport;

public static partial class IndependentSampleVerifier
{
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
                    ["$discriminator"] = NormalizeDiscriminator(union, discriminator),
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

    private static object? SnapshotDynamicMember(DynamicData parent, string memberName, IDynamicType memberType)
    {
        if (IsComplexDynamicType(memberType))
        {
            using var member = parent.LoanValue(memberName);
            return SnapshotDynamic(member.Data);
        }

        return SnapshotDynamicValue(memberType, parent.GetAnyValue(memberName));
    }

    private static object? SnapshotDynamicMember(DynamicData parent, int memberIndex, IDynamicType memberType)
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
}
