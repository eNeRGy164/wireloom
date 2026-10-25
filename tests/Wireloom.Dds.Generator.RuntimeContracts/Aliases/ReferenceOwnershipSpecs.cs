using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Distinguishes value-constructor ownership from copy-constructor ownership.</summary>
public sealed class ReferenceOwnershipSpecs
{
    /// <summary>Checks that value constructors retain supplied aggregate and collection references.</summary>
    [Fact]
    public void RetainsReferencesPassedToValueConstructors()
    {
        // Arrange
        var child = new Child(17, "original");
        var numbers = new Sequence<int> { 1, 2 };
        var vector = new[] { 3, 4 };
        var matrix = new [,] { { 1, 2, 3 }, { 4, 5, 6 } };

        // Act
        var aggregateAlias = new Aggregate(child);
        var sequenceAlias = new Numbers(numbers);
        var vectorAlias = new Vector(vector);
        var matrixAlias = new Matrix(matrix);

        // Assert
        aggregateAlias.Value.ShouldBeSameAs(child);
        sequenceAlias.Value.ShouldBeSameAs(numbers);
        vectorAlias.Value.ShouldBeSameAs(vector);
        matrixAlias.Value.ShouldBeSameAs(matrix);
        child.id = 99;
        aggregateAlias.Value.id.ShouldBe(99);
        numbers.Add(3);
        sequenceAlias.Value.ShouldBe([1, 2, 3]);
    }

    /// <summary>Checks array dimensions and empty sequence defaults independently of copies.</summary>
    [Fact]
    public void InitializesCollectionAliasDefaults()
    {
        // Arrange
        var vector = new Vector();
        var matrix = new Matrix();

        // Act
        var numbers = new Numbers();
        var texts = new Texts();
        var wideTexts = new WideTexts();

        // Assert
        vector.Value.ShouldBe([0, 0]);
        matrix.Value.GetLength(0).ShouldBe(2);
        matrix.Value.GetLength(1).ShouldBe(3);
        matrix.Value[1, 2].ShouldBe(0);
        numbers.Value.ShouldBeEmpty();
        texts.Value.ShouldBeEmpty();
        wideTexts.Value.ShouldBeEmpty();
    }
}
