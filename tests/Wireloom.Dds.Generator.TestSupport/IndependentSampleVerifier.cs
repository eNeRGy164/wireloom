#nullable enable annotations

using System.Reflection;
using System.Text.Json;
using Omg.Types.Dynamic;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.TestSupport;

/// <summary>Compares managed and DynamicData snapshots without generated equality or conversion code.</summary>
public static partial class IndependentSampleVerifier
{
    /// <summary>Compares an independently populated RTI fixture with the public members of a managed sample.</summary>
    public static void Verify(DynamicData expectedFixture, object actualSample, string context)
    {
        VerifySnapshots(
            SnapshotDynamic(expectedFixture),
            SnapshotManaged(expectedFixture.Type, actualSample),
            context);
    }

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
        var label = NormalizeDiscriminator(union, discriminator);
        var explicitMember = union.Members.FirstOrDefault(member => member.Labels
            .Where(value => value != UnionMember.DefaultLabel)
            .Any(value => NormalizeDiscriminator(union, value) == label));
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

    /// <summary>Normalizes RTI's signed label storage to the declared discriminator's value range.</summary>
    private static long NormalizeDiscriminator(UnionType union, object value)
    {
        var numericValue = value is char character
            ? character
            : Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
        IDynamicType discriminatorType = union.Discriminator;
        while (discriminatorType is AliasType alias)
        {
            discriminatorType = alias.RelatedType;
        }

        return discriminatorType.Kind switch
        {
            TypeKind.UInt32 => unchecked((uint)numericValue),
            TypeKind.Uint16 or TypeKind.Char16 => unchecked((ushort)numericValue),
            TypeKind.Uint8 or TypeKind.Octet or TypeKind.Char8 => unchecked((byte)numericValue),
            _ => numericValue
        };
    }
}
