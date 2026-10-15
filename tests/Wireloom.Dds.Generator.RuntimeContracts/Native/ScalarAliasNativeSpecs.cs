using RuntimeAliases;
using RuntimeAliases.Implementation;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Exercises native scalar typedef contracts.</summary>
public sealed class ScalarAliasNativeSpecs
{
    /// <summary>Checks Scalar conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsScalar(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Scalar(17);

        // Act
        var result = NativeSample.RoundTrip<Scalar, ScalarUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(17);
    }

    /// <summary>Checks ScalarChain conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsScalarChain(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new ScalarChain(17);

        // Act
        var result = NativeSample.RoundTrip<ScalarChain, ScalarChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(17);
    }

    /// <summary>Checks EnumAlias conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsEnumAlias(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new EnumAlias(State.Ready);

        // Act
        var result = NativeSample.RoundTrip<EnumAlias, EnumAliasUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(State.Ready);
    }

    /// <summary>Checks EnumChain conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsEnumChain(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new EnumChain(State.Ready);

        // Act
        var result = NativeSample.RoundTrip<EnumChain, EnumChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(State.Ready);
    }

    /// <summary>Checks Text conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsText(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Text("hello");

        // Act
        var result = NativeSample.RoundTrip<Text, TextUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe("hello");
    }

    /// <summary>Checks WideText conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsWideText(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new WideText("世界");

        // Act
        var result = NativeSample.RoundTrip<WideText, WideTextUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe("世界");
    }

}
