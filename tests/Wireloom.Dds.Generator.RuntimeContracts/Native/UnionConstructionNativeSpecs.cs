using RuntimeUnionNullCopy;
using RuntimeUnionNullCopy.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks that initialized null copies are usable by generated native converters.</summary>
public sealed class UnionConstructionNativeSpecs
{
    /// <summary>Preserves active reference defaults and signed enum defaults through owned native storage.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void ConvertsInitializedNullCopies(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var aggregate = new ZeroAggregate(null);
        var sequence = new ZeroSequence(null);
        var array = new ZeroArray(null);
        var enumeration = new NegativeEnum(null);
        var mixed = new MixedEnum(null);

        // Act
        var aggregateResult = NativeSample.RoundTrip<ZeroAggregate, ZeroAggregateUnmanaged>(aggregate, allocatePointers, allocateMemory);
        var sequenceResult = NativeSample.RoundTrip<ZeroSequence, ZeroSequenceUnmanaged>(sequence, allocatePointers, allocateMemory);
        var arrayResult = NativeSample.RoundTrip<ZeroArray, ZeroArrayUnmanaged>(array, allocatePointers, allocateMemory);
        var enumResult = NativeSample.RoundTrip<NegativeEnum, NegativeEnumUnmanaged>(enumeration, allocatePointers, allocateMemory);
        var mixedResult = NativeSample.RoundTrip<MixedEnum, MixedEnumUnmanaged>(mixed, allocatePointers, allocateMemory);

        // Assert
        aggregateResult.Discriminator.ShouldBeFalse();
        aggregateResult.child.id.ShouldBe(0);
        aggregateResult.child.ShouldNotBeSameAs(aggregate.child);
        sequenceResult.numbers.Value.ShouldBeEmpty();
        arrayResult.vector.Value.ShouldBe([0, 0]);
        enumResult.Discriminator.ShouldBe(NegativeMode.NegativeFirst);
        enumResult.number.ShouldBe(0);
        mixedResult.Discriminator.ShouldBe(MixedMode.MixedFirst);
        mixedResult.number.ShouldBe(0);
    }
}
