using RuntimeAliases;
using RuntimeAliases.Implementation;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Exercises native aggregate typedef contracts.</summary>
public sealed class AggregateAliasNativeSpecs
{
    /// <summary>Checks Aggregate conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsAggregate(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Aggregate(new Child(17, "nested"));

        // Act
        var result = NativeSample.RoundTrip<Aggregate, AggregateUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.id.ShouldBe(17);
    }

    /// <summary>Checks AggregateChain conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsAggregateChain(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new AggregateChain(new Child(17, "nested"));

        // Act
        var result = NativeSample.RoundTrip<AggregateChain, AggregateChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.text.ShouldBe("nested");
    }

}
