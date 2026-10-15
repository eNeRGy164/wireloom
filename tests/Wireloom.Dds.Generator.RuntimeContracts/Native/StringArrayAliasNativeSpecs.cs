using RuntimeStringArrayAliases;
using RuntimeStringArrayAliases.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Exercises native conversion and lifecycle for narrow and wide fixed string arrays.</summary>
public sealed class StringArrayAliasNativeSpecs
{
    /// <summary>Checks narrow fixed-array conversion and cleanup across native allocation modes.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsTextVector(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new TextVector(["first", "second"]);

        // Act
        var result = NativeSample.RoundTrip<TextVector, TextVectorUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["first", "second"]);
    }

    /// <summary>Checks wide multidimensional array conversion and cleanup across native allocation modes.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsWideMatrix(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new WideMatrix(new string[,] { { "世界", "Ω" } });

        // Act
        var result = NativeSample.RoundTrip<WideMatrix, WideMatrixUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value[0, 0].ShouldBe("世界");
        result.Value[0, 1].ShouldBe("Ω");
    }
}
