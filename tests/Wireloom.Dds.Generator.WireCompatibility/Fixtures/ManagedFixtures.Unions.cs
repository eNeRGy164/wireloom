#nullable enable annotations

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds character unions without DynamicData discriminator coercion.</summary>
internal static partial class ManagedFixtures
{
#if WIRE_COMPATIBILITY_NUMERIC_CHAR_UNION
    private static T CreateNumericCharacterUnionFixture<T>(string fixture)
    {
        var choice = new CorpusNumericCharUnion.Choice();
        switch (fixture)
        {
            case "union-127":
                choice.value = 13701;
                break;
            case "union-default":
                choice.Setfallback(13703, (char)65);
                break;
            default:
                throw new FixtureException($"Unknown numeric character union fixture '{fixture}'.");
        }
        return (T)(object)choice;
    }
#endif

#if WIRE_COMPATIBILITY_NUMERIC_CHAR_255_UNION
    private static T CreateHighNumericCharacterUnionFixture<T>(string fixture)
    {
        var choice = new CorpusNumericChar255Union.Choice();
        switch (fixture)
        {
            case "union-255":
                choice.value = 13702;
                break;
            case "union-default":
                choice.Setfallback(13703, (char)65);
                break;
            default:
                throw new FixtureException($"Unknown high numeric character union fixture '{fixture}'.");
        }
        return (T)(object)choice;
    }
#endif

    /// <summary>Seeds character branches directly so RTI DynamicData signedness cannot alter the fixture.</summary>
    private static T CreateCharacterUnionFixture<T>(string fixture)
    {
        var sample = Activator.CreateInstance<T>()!;
        var branch = fixture switch
        {
            "union-minimum" => "minimum",
            "union-maximum" => "maximum",
            "union-default" => "fallback",
            _ => throw new FixtureException($"Unknown character union fixture '{fixture}'.")
        };
        typeof(T).GetProperty(branch)!.SetValue(sample, 8201);
        return sample;
    }
}
