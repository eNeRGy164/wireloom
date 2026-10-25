
namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds typed scalar fixtures without replacing constructor defaults through DynamicData.</summary>
// ReSharper disable once PartialTypeWithSinglePart
// The remaining partial declarations are compiled by the wire-compatibility project, which Qodana excludes.
internal static partial class ManagedFixtures
{
    /// <summary>Preserves the generated constructor defaults for the enum-array wire fixture.</summary>
    // ReSharper disable once UnusedMember.Global
    // Selected by the typed fixture dispatcher in ManagedFixtures.Create.
    internal static T CreateEnumArrayDefaultFixture<T>(string fixture)
    {
        if (fixture != "default")
        {
            throw new FixtureException($"Unknown enum array default fixture '{fixture}'.");
        }

        return Activator.CreateInstance<T>()!;
    }

    /// <summary>Sets only the non-default members, preserving the declared enum constructor default.</summary>
    internal static T CreateDefaultsAndRangesFixture<T>(string fixture)
    {
        if (fixture != "non-default")
        {
            throw new FixtureException($"Unknown defaults and ranges fixture '{fixture}'.");
        }

        var sample = Activator.CreateInstance<T>()!;
        typeof(T).GetProperty("value")!.SetValue(sample, 97);
        typeof(T).GetProperty("ranged")!.SetValue(sample, -17);
        return sample;
    }

    /// <summary>Assigns signed boundary values through public typed members on both RTI profiles.</summary>
    internal static T CreatePrimitiveBoundaryFixture<T>(string caseId, string fixture)
    {
        if (fixture != "non-default")
        {
            throw new FixtureException($"Unknown primitive boundary fixture '{fixture}'.");
        }

        Dictionary<string, object> values = caseId switch
        {
            "01-boundaries" => new()
            {
                ["shortValue"] = short.MinValue,
                ["longValue"] = int.MaxValue,
                ["longLongValue"] = long.MinValue,
                ["int8Value"] = sbyte.MinValue,
                ["int64Value"] = long.MaxValue,
                ["floatValue"] = 1.25f,
                ["doubleValue"] = -2.5d
            },
            "01-full-widths" => new()
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
            _ => throw new FixtureException($"Unknown primitive boundary case '{caseId}'.")
        };
        var sample = Activator.CreateInstance<T>()!;
        foreach (var (name, value) in values)
        {
            typeof(T).GetProperty(name)!.SetValue(sample, value);
        }

        return sample;
    }
}
