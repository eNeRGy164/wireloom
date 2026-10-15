using RuntimeManaged;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Exercises generated aggregate, collection, and optional managed contracts.</summary>
public sealed class CollectionSpecs
{
    /// <summary>Creates Independent Containers And Absent Optionals.</summary>
    [Fact]
    public void CreatesIndependentContainersAndAbsentOptionals()
    {
        // Arrange
        var first = new Collections();

        // Act
        var second = new Collections();
        var nullCopy = new Collections((Collections?)null);

        // Assert
        first.child.ShouldNotBeNull();
        first.numbers.ShouldBeEmpty();
        first.children.ShouldBeEmpty();
        first.texts.ShouldBeEmpty();
        first.wideTexts.ShouldBeEmpty();
        first.vector.Length.ShouldBe(2);
        first.matrix.GetLength(0).ShouldBe(2);
        first.matrix.GetLength(1).ShouldBe(3);
        first.vector.ShouldNotBeSameAs(second.vector);
        first.matrix.ShouldNotBeSameAs(second.matrix);
        first.child.ShouldNotBeSameAs(second.child);
        first.optionalNumber.ShouldBeNull();
        first.optionalText.ShouldBeNull();
        first.optionalChild.ShouldBeNull();
        first.optionalNumbers.ShouldBeNull();
        first.optionalPoints.ShouldBeNull();
        first.optionalState.ShouldBeNull();
        first.optionalWideText.ShouldBeNull();
        first.optionalWideTexts.ShouldBeNull();
        first.optionalMatrix.ShouldBeNull();
        // The documented null-copy path retains initializers, not constructor-only allocations.
        nullCopy.optionalChild.ShouldBeNull();
        nullCopy.child.ShouldNotBeNull();
        nullCopy.numbers.ShouldBeNull();
        nullCopy.children.ShouldBeNull();
        nullCopy.vector.ShouldBeNull();
        nullCopy.matrix.ShouldBeNull();
        nullCopy.points.ShouldBeNull();
    }

    /// <summary>Copies Required And Optional Containers Independently.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CopiesRequiredAndOptionalContainersIndependently(bool present)
    {
        // Arrange
        var source = CollectionSamples.Create(present);

        // Act
        var copy = new Collections(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Collections?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.child.ShouldNotBeSameAs(source.child);
        copy.numbers.ShouldNotBeSameAs(source.numbers);
        copy.children.ShouldNotBeSameAs(source.children);
        copy.children[0].ShouldNotBeSameAs(source.children[0]);
        copy.vector.ShouldNotBeSameAs(source.vector);
        copy.matrix.ShouldNotBeSameAs(source.matrix);
        copy.points[0].ShouldNotBeSameAs(source.points[0]);
        copy.children[0].id = 999;
        copy.matrix[1, 2] = 888;
        source.children[0].id.ShouldBe(21);
        source.matrix[1, 2].ShouldBe(6);
        copy.Equals(source).ShouldBeFalse();
        if (present)
        {
            copy.optionalNumber.ShouldBe(7);
            copy.optionalText.ShouldBe("optional");
            copy.optionalChild.ShouldNotBeSameAs(source.optionalChild);
            copy.optionalNumbers.ShouldNotBeSameAs(source.optionalNumbers);
            copy.optionalPoints![0].ShouldNotBeSameAs(source.optionalPoints![0]);
            copy.optionalState.ShouldBe(State.Ready);
            copy.optionalWideText.ShouldBe("世界");
            copy.optionalWideTexts.ShouldNotBeSameAs(source.optionalWideTexts);
            copy.optionalMatrix.ShouldNotBeSameAs(source.optionalMatrix);
            copy.optionalChild!.id = 777;
            source.optionalChild!.id.ShouldBe(31);
        }
    }

    /// <summary>Optional Properties Can Be Set And Cleared.</summary>
    [Fact]
    public void OptionalPropertiesCanBeSetAndCleared()
    {
        // Arrange
        var sample = CollectionSamples.Create(present: true);

        // Act
        sample.optionalNumber = null;
        sample.optionalText = null;
        sample.optionalChild = null;
        sample.optionalNumbers = null;
        sample.optionalPoints = null;
        sample.optionalState = null;
        sample.optionalWideText = null;
        sample.optionalWideTexts = null;
        sample.optionalMatrix = null;

        // Assert
        sample.Equals(CollectionSamples.Create(present: false)).ShouldBeTrue();
        sample.ToString().ShouldBe(CollectionsSupport.Instance.ToString(sample));
    }
}
