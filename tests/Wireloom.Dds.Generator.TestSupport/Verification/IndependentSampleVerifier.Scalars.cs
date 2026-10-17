#nullable enable annotations

namespace Wireloom.Dds.Generator.TestSupport;

public static partial class IndependentSampleVerifier
{
    /// <summary>Requires the IDL-declared BLUE constructor default without deriving it from the expected sample.</summary>
    public static void VerifyDefaultsAndRangesFixture(object sample, string context)
    {
        var expected = new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["color"] = 2,
            ["ranged"] = -17,
            ["value"] = 97
        };
        VerifyScalarMembers(expected, sample, context);
    }

    /// <summary>Checks every boundary member against independent fixture values, preserving signed and unsigned ranges.</summary>
    public static void VerifyPrimitiveBoundaryFixture(object sample, string caseId, string context)
    {
        SortedDictionary<string, object?> expected = caseId switch
        {
            "01-boundaries" => new(StringComparer.Ordinal)
            {
                ["shortValue"] = short.MinValue,
                ["longValue"] = int.MaxValue,
                ["longLongValue"] = long.MinValue,
                ["int8Value"] = sbyte.MinValue,
                ["int64Value"] = long.MaxValue,
                ["floatValue"] = 1.25f,
                ["doubleValue"] = -2.5d
            },
            "01-full-widths" => new(StringComparer.Ordinal)
            {
                ["i8"] = (sbyte)-101,
                ["i16"] = (short)-16001,
                ["i32"] = -320001,
                ["i64"] = -6400001L,
                ["u8"] = (byte)201,
                ["u16"] = (ushort)52001,
                ["u32"] = 3200001U,
                ["u64"] = 6400001UL
            },
            _ => throw new InvalidDataException($"Unknown primitive boundary case '{caseId}'.")
        };
        VerifyScalarMembers(expected, sample, context);
    }

    /// <summary>Checks the exact IDL character discriminator and selected payload without using generated equality.</summary>
    public static void VerifyNumericCharacterUnionFixture(object sample, string caseId, string fixture)
    {
        var (branch, discriminator, payload) = (caseId, fixture) switch
        {
            ("07-union-char-numeric", "union-127") => ("value", (char)127, 13701),
            ("07-union-char-numeric-255", "union-255") => ("value", (char)255, 13702),
            ("07-union-char-numeric" or "07-union-char-numeric-255", "union-default") => ("fallback", (char)65, 13703),
            _ => throw new InvalidDataException($"Unknown numeric character union fixture '{caseId}/{fixture}'.")
        };
        var expected = new SortedDictionary<string, object?>(StringComparer.Ordinal)
        {
            ["Discriminator"] = discriminator,
            [branch] = payload
        };
        VerifyScalarMembers(expected, sample, $"{caseId} fixture {fixture}");
    }

    private static void VerifyScalarMembers(SortedDictionary<string, object?> expected, object sample, string context)
    {
        var actual = new SortedDictionary<string, object?>(StringComparer.Ordinal);
        foreach (var memberName in expected.Keys)
        {
            actual.Add(memberName, NormalizeScalar(GetManagedMember(sample, memberName)));
        }

        VerifySnapshots(expected, actual, context);
    }
}
