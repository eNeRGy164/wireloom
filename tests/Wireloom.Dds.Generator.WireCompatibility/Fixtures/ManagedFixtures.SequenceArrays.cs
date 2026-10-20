#if WIRE_COMPATIBILITY_SEQUENCE_ARRAYS
namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Constructs and independently checks the IDL mapping's two collection layers.</summary>
internal static partial class ManagedFixtures
{
    private static T CreateSequenceArrayFixture<T>(string fixture)
    {
        var sample = new CorpusArrayOfSequences.Sample();
        switch (fixture)
        {
            case "sequence-empty":
                break;
            case "sequence-unequal":
                sample.values[0].Value.Add(101);
                sample.values[0].Value.Add(-102);
                sample.values[1].Value.Add(203);
                sample.names[0].Value.Add("first");
                sample.names[1].Value.Add("second");
                sample.names[1].Value.Add("third");
                break;
            case "sequence-mixed-empty":
                sample.values[1].Value.Add(-301);
                sample.values[1].Value.Add(302);
                sample.names[0].Value.Add("only-first");
                break;
            case "sequence-boundary":
                sample.values[0].Value.Add(int.MinValue);
                sample.values[1].Value.Add(int.MaxValue);
                sample.names[0].Value.Add("1234567890123456");
                sample.names[0].Value.Add("");
                sample.names[0].Value.Add("last");
                sample.names[1].Value.Add("other");
                break;
            default:
                throw new FixtureException($"Unknown sequence-array fixture '{fixture}'.");
        }

        return (T)(object)sample;
    }

    /// <summary>Checks every array position, sequence count, and payload without generated equality.</summary>
    internal static void VerifySequenceArrayFixture(object value, string fixture)
    {
        var sample = (CorpusArrayOfSequences.Sample)value;
        if (sample.values.Length != 2 || sample.names.Length != 2)
        {
            throw new InvalidDataException("Sequence-array fixed dimensions must both equal two.");
        }

        (int[][] Numbers, string[][] Names) expected = fixture switch
        {
            "sequence-empty" => ([[], []], [[], []]),
            "sequence-unequal" => ([[101, -102], [203]], [["first"], ["second", "third"]]),
            "sequence-mixed-empty" => ([[], [-301, 302]], [["only-first"], []]),
            "sequence-boundary" => ([[int.MinValue], [int.MaxValue]], [["1234567890123456", "", "last"], ["other"]]),
            _ => throw new FixtureException($"Unknown sequence-array fixture '{fixture}'.")
        };

        for (var index = 0; index < 2; index++)
        {
            if (!sample.values[index].Value.SequenceEqual(expected.Numbers[index])
                || !sample.names[index].Value.SequenceEqual(expected.Names[index]))
            {
                throw new InvalidDataException($"Sequence-array payload mismatch at array index {index} for {fixture}.");
            }
        }
    }
}
#endif
