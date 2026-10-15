using RuntimeUnionStringSequences;
using RuntimeUnionStringSequences.Implementation;
using Rti.Types;
using Wireloom.Dds.Generator.RuntimeContracts.Native;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Exercises direct and typedef-backed string-sequence union branches.</summary>
public sealed class StringSequenceUnionSpecs
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void RoundTripsDirectAndAliasStringSequenceBranchesThroughNativeStorage(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        StringSequences[] samples =
        [
            new() { directNarrow = new Sequence<string> { "narrow" } },
            new() { directWide = new Sequence<string> { "世界" } },
            new() { boundedNarrow = new Sequence<string> { "bounded" } },
            new() { boundedWide = new Sequence<string> { "Ω" } },
            new() { narrowAlias = new NarrowTexts(new Sequence<string> { "alias" }) },
            new() { wideAlias = new WideTexts(new Sequence<string> { "別名" }) },
            new() { boundedNarrowAlias = new BoundedNarrowTexts(new Sequence<string> { "bound" }) },
            new() { boundedWideAlias = new BoundedWideTexts(new Sequence<string> { "値" }) }
        ];

        // Act
        var copies = samples
            .Select(sample => NativeSample.RoundTrip<StringSequences, StringSequencesUnmanaged>(sample, allocatePointers, allocateMemory))
            .ToArray();

        // Assert
        copies.Length.ShouldBe(samples.Length);
        for (var index = 0; index < samples.Length; index++)
        {
            copies[index].ShouldBe(samples[index]);
            copies[index].GetHashCode().ShouldBe(samples[index].GetHashCode());
        }

        copies[0].directNarrow[0].ShouldBe("narrow");
        copies[1].directWide[0].ShouldBe("世界");
        copies[2].boundedNarrow[0].ShouldBe("bounded");
        copies[3].boundedWide[0].ShouldBe("Ω");
        copies[4].narrowAlias.Value[0].ShouldBe("alias");
        copies[5].wideAlias.Value[0].ShouldBe("別名");
        copies[6].boundedNarrowAlias.Value[0].ShouldBe("bound");
        copies[7].boundedWideAlias.Value[0].ShouldBe("値");
    }
}
