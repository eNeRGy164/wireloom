using RuntimeStringAliasContracts;
using RuntimeStringAliasContracts.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks that scalar string alias chains use matching native storage and bounds.</summary>
public sealed class StringAliasChainNativeSpecs
{
    /// <summary>Round-trips bounded narrow and wide values for each native allocation combination.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void BoundedStringChainsRoundTrip(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var narrow = new BoundedTextChain2("wireloom");
        var wide = new BoundedWideTextChain2("世界");

        // Act
        var narrowResult = NativeSample.RoundTrip<BoundedTextChain2, BoundedTextChain2Unmanaged>(narrow, allocatePointers, allocateMemory);
        var wideResult = NativeSample.RoundTrip<BoundedWideTextChain2, BoundedWideTextChain2Unmanaged>(wide, allocatePointers, allocateMemory);

        // Assert
        narrowResult.Value.ShouldBe("wireloom");
        wideResult.Value.ShouldBe("世界");
    }
}
