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
        if (Uses731Profile)
        {
            integerCopy.Discriminator.ShouldBe(1);
            integerCopy.flag.ShouldBeFalse();
        }
        else
        {
            integerCopy.Discriminator.ShouldBe(0);
            integerCopy.zero.ShouldBe(0);
        }
        childCopy.child.id.ShouldBe(0);
        childCopy.Equals(child).ShouldBeTrue();
        childCopy.child.ShouldNotBeSameAs(child.child);
    }

    /// <summary>Executes a default-only aliased enum union through the freshly packed analyzer.</summary>
    [Fact]
    public void InitializesDefaultOnlyEnumWithTheSelectedProfile()
    {
        // Arrange
        var expected = Uses731Profile ? AnnotatedMode.FirstDeclared : AnnotatedMode.Preferred;

        // Act
        var sample = new EnumDefaultOnly(null);

        // Assert
        sample.Discriminator.ShouldBe(expected);
        sample.fallback.ShouldBe(0);
    }

    private static bool Uses731Profile =>
        typeof(Rti.Dds.Core.ServiceEnvironment).Assembly.GetName().Version < new Version(7, 7, 0);
}
