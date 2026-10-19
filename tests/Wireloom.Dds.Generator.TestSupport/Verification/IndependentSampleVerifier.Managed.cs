#nullable enable annotations

using System.Collections;
using System.Reflection;
using Omg.Types.Dynamic;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.TestSupport;

public static partial class IndependentSampleVerifier
{
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

            var scalarValueProperty = value.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            if (scalarValueProperty is not null)
            {
                value = scalarValueProperty.GetValue(value);
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
            properties["$discriminator"] = NormalizeDiscriminator((UnionType)expectedType, discriminatorValue);
            properties[selectedMember.Name] = SnapshotManaged(
                selectedMember.Type,
                GetManagedMember(value, selectedMember.Name));
            return properties;
        }

        return NormalizeScalar(value);
    }
}
