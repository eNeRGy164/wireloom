using RuntimeUnionNullCopy;

namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Checks union default and null-source construction through the packed analyzer.</summary>
public sealed class UnionConstructionSpecs
{
    /// <summary>Compiles negative enum defaults and initializes null copies consistently.</summary>
    [Fact]
    public void InitializesTypeDefaultsAndReferenceBranches()
    {
        // Arrange
        var negative = new NegativeEnum();
        var child = new ZeroAggregate();

        // Act
        var negativeCopy = new NegativeEnum(null);
        var childCopy = new ZeroAggregate(null);
        var integerCopy = new NegativeAndZero(null);

        // Assert
        negativeCopy.Discriminator.ShouldBe(NegativeMode.NegativeFirst);
        negativeCopy.number.ShouldBe(0);
        negativeCopy.Equals(negative).ShouldBeTrue();
        negative.Equals(negativeCopy).ShouldBeTrue();
        negativeCopy.GetHashCode().ShouldBe(negative.GetHashCode());
        integerCopy.Discriminator.ShouldBe(0);
        integerCopy.zero.ShouldBe(0);
        childCopy.child.id.ShouldBe(0);
        childCopy.Equals(child).ShouldBeTrue();
        childCopy.child.ShouldNotBeSameAs(child.child);
    }
}
