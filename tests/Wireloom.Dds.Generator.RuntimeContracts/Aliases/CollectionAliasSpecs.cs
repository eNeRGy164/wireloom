using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Executes generated CollectionAlias contracts.</summary>
public sealed class CollectionAliasSpecs
{
    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Numbers.</summary>
    [Fact]
    public void NumbersPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Numbers();
        var nullCopy = new Numbers((Numbers?)null);
        var source = new Numbers(new Sequence<int> { 1, 2, 3 });

        // Act
        var copy = new Numbers(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Numbers?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0] = 99;
        copy.Equals(source).ShouldBeFalse();
        copy.Value.Clear();
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for NumbersChain.</summary>
    [Fact]
    public void NumbersChainPreservesItsValueContract()
    {
        // Arrange
        var defaults = new NumbersChain();
        var nullCopy = new NumbersChain((NumbersChain?)null);
        var source = new NumbersChain(new Numbers(new Sequence<int> { 1, 2, 3 }));

        // Act
        var copy = new NumbersChain(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((NumbersChain?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value.Value[0] = 99;
        source.Value.Value[0].ShouldBe(1);
        copy.Value = new Numbers(new Sequence<int> { 4, 5 });
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Children.</summary>
    [Fact]
    public void ChildrenPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Children();
        var nullCopy = new Children((Children?)null);
        var source = new Children(new Sequence<Child> { new Child(17, "nested") });

        // Act
        var copy = new Children(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Children?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
        copy.Value[0].id = 99;
        source.Value[0].id.ShouldBe(17);
        copy.Equals(source).ShouldBeFalse();
        copy.Value.Clear();
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Texts.</summary>
    [Fact]
    public void TextsPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Texts();
        var nullCopy = new Texts((Texts?)null);
        var source = new Texts(new Sequence<string> { "one", "two" });

        // Act
        var copy = new Texts(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Texts?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0] = "changed";
        copy.Equals(source).ShouldBeFalse();
        copy.Value.Clear();
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for WideTexts.</summary>
    [Fact]
    public void WideTextsPreservesItsValueContract()
    {
        // Arrange
        var defaults = new WideTexts();
        var nullCopy = new WideTexts((WideTexts?)null);
        var source = new WideTexts(new Sequence<string> { "世界", "Ω" });

        // Act
        var copy = new WideTexts(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((WideTexts?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0] = "changed";
        copy.Equals(source).ShouldBeFalse();
        copy.Value.Clear();
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Vector.</summary>
    [Fact]
    public void VectorPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Vector();
        var nullCopy = new Vector((Vector?)null);
        var source = new Vector([1, 2]);

        // Act
        var copy = new Vector(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldNotBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Vector?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0] = 99;
        copy.Equals(source).ShouldBeFalse();
        copy.Value = [3, 4];
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Matrix.</summary>
    [Fact]
    public void MatrixPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Matrix();
        var nullCopy = new Matrix((Matrix?)null);
        var source = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });

        // Act
        var copy = new Matrix(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldNotBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Matrix?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[1, 2] = 99;
        copy.Equals(source).ShouldBeFalse();
        copy.Value = new int[,] { { 7, 8, 9 }, { 10, 11, 12 } };
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for MatrixChain.</summary>
    [Fact]
    public void MatrixChainPreservesItsValueContract()
    {
        // Arrange
        var defaults = new MatrixChain();
        var nullCopy = new MatrixChain((MatrixChain?)null);
        var source = new MatrixChain(new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }));

        // Act
        var copy = new MatrixChain(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((MatrixChain?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value.Value[1, 2] = 99;
        source.Value.Value[1, 2].ShouldBe(6);
        copy.Value = new Matrix(new int[,] { { 7, 8, 9 }, { 10, 11, 12 } });
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Points.</summary>
    [Fact]
    public void PointsPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Points();
        var nullCopy = new Points((Points?)null);
        var source = new Points([new Child(17, "one"), new Child(18, "two")]);

        // Act
        var copy = new Points(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldNotBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Points?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
        copy.Value[0].id++;
        source.Value[0].id.ShouldBe(17);
        copy.Equals(source).ShouldBeFalse();
        copy.Value = [new Child(19, "three"), new Child(20, "four")];
        copy.Equals(source).ShouldBeFalse();
    }
}
