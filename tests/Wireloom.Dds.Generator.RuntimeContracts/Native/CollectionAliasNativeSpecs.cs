using RuntimeAliases;
using RuntimeAliases.Implementation;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Exercises native collection typedef contracts.</summary>
public sealed class CollectionAliasNativeSpecs
{
    /// <summary>Checks Numbers conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsNumbers(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Numbers(new Sequence<int> { 17, 18 });

        // Act
        var result = NativeSample.RoundTrip<Numbers, NumbersUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe([17, 18]);
    }

    /// <summary>Checks NumbersChain conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsNumbersChain(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new NumbersChain(new Numbers(new Sequence<int> { 17, 18 }));

        // Act
        var result = NativeSample.RoundTrip<NumbersChain, NumbersChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.Value.ShouldBe([17, 18]);
    }

    /// <summary>Checks Children conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsChildren(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Children(new Sequence<Child> { new Child(17, "nested") });

        // Act
        var result = NativeSample.RoundTrip<Children, ChildrenUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value[0].id.ShouldBe(17);
    }

    /// <summary>Checks Texts conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsTexts(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Texts(new Sequence<string> { "hello", "other" });

        // Act
        var result = NativeSample.RoundTrip<Texts, TextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["hello", "other"]);
    }

    /// <summary>Checks WideTexts conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsWideTexts(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new WideTexts(new Sequence<string> { "世界", "Ω" });

        // Act
        var result = NativeSample.RoundTrip<WideTexts, WideTextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["世界", "Ω"]);
    }

    /// <summary>Checks Vector conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsVector(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Vector([17, 18]);

        // Act
        var result = NativeSample.RoundTrip<Vector, VectorUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe([17, 18]);
    }

    /// <summary>Checks Matrix conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsMatrix(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });

        // Act
        var result = NativeSample.RoundTrip<Matrix, MatrixUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value[1, 2].ShouldBe(6);
    }

    /// <summary>Checks MatrixChain conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsMatrixChain(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new MatrixChain(new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }));

        // Act
        var result = NativeSample.RoundTrip<MatrixChain, MatrixChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.Value[1, 2].ShouldBe(6);
    }

    /// <summary>Checks Points conversion and lifecycle with both key and allocation flag values.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsPoints(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Points([new Child(17, "one"), new Child(18, "other")]);

        // Act
        var result = NativeSample.RoundTrip<Points, PointsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value[1].text.ShouldBe("other");
    }

}
