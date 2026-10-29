#nullable enable annotations

using System.Linq;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds and checks large values for the unbounded-support wire case.</summary>
internal static partial class ManagedFixtures
{
#if WIRE_COMPATIBILITY_UNBOUNDED_SUPPORT
    internal static T CreateUnboundedSupportFixture<T>(PeerOptions options)
    {
        RequireUnboundedFixture(options);
        var sample = new CorpusUnboundedSupport.Sample
        {
            text = new string('n', 300),
            wideText = new string('λ', 16),
            boundedText = "12345678"
        };

        foreach (var value in Enumerable.Range(0, 103).Select(index => index - 51))
        {
            sample.values.Add(value);
        }

        foreach (var index in Enumerable.Range(0, 9))
        {
            sample.labels.Add($"label-{index:D6}");
        }

        foreach (var value in Enumerable.Range(0, 101).Select(index => index + 1000))
        {
            sample.aliasValues.Value.Add(value);
        }

        foreach (var value in new[] { 4, -4, int.MinValue, int.MaxValue })
        {
            sample.boundedValues.Add(value);
        }

        VerifyUnboundedSupportFixture(sample, options);
        return (T)(object)sample;
    }

    internal static void VerifyUnboundedSupportFixture<T>(T value, PeerOptions options)
    {
        RequireUnboundedFixture(options);
        var sample = (CorpusUnboundedSupport.Sample)(object)value!;
        AssertValue(sample.text, new string('n', 300), "text", options);
        AssertValue(sample.wideText, new string('λ', 16), "wideText", options);
        AssertValue(sample.boundedText, "12345678", "boundedText", options);
        AssertSequence(sample.values.ToArray(), Enumerable.Range(0, 103).Select(index => index - 51).ToArray(), "values", options);
        AssertSequence(sample.labels.ToArray(), Enumerable.Range(0, 9).Select(index => $"label-{index:D6}").ToArray(), "labels", options);
        AssertSequence(sample.aliasValues.Value.ToArray(), Enumerable.Range(0, 101).Select(index => index + 1000).ToArray(), "aliasValues", options);
        AssertSequence(sample.boundedValues.ToArray(), [4, -4, int.MinValue, int.MaxValue], "boundedValues", options);
    }

    private static void RequireUnboundedFixture(PeerOptions options)
    {
        if (options.Fixture is not ("xcdr1-unbounded" or "xcdr2-unbounded"))
        {
            throw UnknownFixture(options);
        }
    }
#endif
}
