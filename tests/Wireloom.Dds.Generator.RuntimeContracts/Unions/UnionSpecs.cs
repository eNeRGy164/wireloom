using RuntimeUnions;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Exercises union selection, transitions, copying, and value semantics.</summary>
public sealed class UnionSpecs
{
    /// <summary>Initializes Default And No Active Branch States.</summary>
    [Fact]
    public void InitializesDefaultAndNoActiveBranchStates()
    {
        // Arrange
        var choice = new Choice();
        var sparse = new Sparse();

        // Act
        var choiceCopy = new Choice(choice);
        var sparseCopy = new Sparse(sparse);

        // Assert
        choice.Discriminator.ShouldBe(Choice.DefaultDiscriminator);
        if (RtiRuntimeProfile.Uses731Profile)
        {
            choice.Discriminator.ShouldBe(7);
            choice.flag.ShouldBeFalse();
            choice.Get().ShouldBe(false);
        }
        else
        {
            choice.Discriminator.ShouldBe(0);
            choice.number.ShouldBe(0);
            choice.Get().ShouldBe(0);
        }
        choiceCopy.Equals(choice).ShouldBeTrue();
        sparse.Discriminator.ShouldBe(Sparse.DefaultDiscriminator);
        if (RtiRuntimeProfile.Uses731Profile)
        {
            sparse.Discriminator.ShouldBe(1);
            sparse.Get().ShouldBe(0);
            sparse.number.ShouldBe(0);
        }
        else
        {
            sparse.Get().ShouldBeNull();
            Should.Throw<InvalidOperationException>(() => sparse.number);
        }
        sparseCopy.Equals(sparse).ShouldBeTrue();
        sparse.GetHashCode().ShouldBe(sparseCopy.GetHashCode());
    }

    /// <summary>Selects Multiple Labels And Rejects Invalid Discriminators.</summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void SelectsMultipleLabelsAndRejectsInvalidDiscriminators(int discriminator)
    {
        // Arrange
        var choice = new Choice { number = 17 };

        // Act
        choice.Settext("selected", discriminator);

        // Assert
        choice.Discriminator.ShouldBe(discriminator);
        choice.text.ShouldBe("selected");
        choice.Get().ShouldBe("selected");
        Should.Throw<InvalidOperationException>(() => choice.number);
        Should.Throw<ArgumentException>(() => choice.Settext("invalid", 0));
        choice.text.ShouldBe("selected");
        Should.Throw<ArgumentException>(() => choice.Setflag(true, 1));
        choice.Setflag(true, 99);
        choice.Discriminator.ShouldBe(99);
        choice.flag.ShouldBeTrue();
        choice.number = 23;
        choice.Get().ShouldBe(23);
        Should.Throw<InvalidOperationException>(() => choice.flag);
        choice.text = "first label";
        choice.Discriminator.ShouldBe(1);
    }

    /// <summary>Copies And Formats Every Branch.</summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void CopiesAndFormatsEveryBranch(int branch)
    {
        // Arrange
        var source = UnionSamples.Create(branch);

        // Act
        var copy = new Choice(source);
        var text = source.ToString();

        // Assert
        copy.Discriminator.ShouldBe(source.Discriminator);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Choice?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        text.ShouldBe(ChoiceSupport.Instance.ToString(source));
        text.ShouldNotBeNullOrWhiteSpace();
        copy.number = -99;
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Copies Aggregate And Collection Branches Independently.</summary>
    [Fact]
    public void CopiesAggregateAndCollectionBranchesIndependently()
    {
        // Arrange
        var source = new Choice { child = new Child(17, "nested") };
        var sequence = new Choice { numbers = new Numbers(new Sequence<int> { 1, 2 }) };
        var array = new Choice { vector = new Vector([3, 4]) };

        // Act
        var childCopy = new Choice(source);
        var sequenceCopy = new Choice(sequence);
        var arrayCopy = new Choice(array);
        var directArray = new Choice { values = [5, 6] };
        var directArrayCopy = new Choice(directArray);

        // Assert
        childCopy.child.ShouldNotBeSameAs(source.child);
        childCopy.child.id++;
        source.child.id.ShouldBe(17);
        sequenceCopy.numbers.ShouldNotBeSameAs(sequence.numbers);
        sequenceCopy.numbers.Value[0] = 99;
        sequence.numbers.Value[0].ShouldBe(1);
        arrayCopy.vector.Value[0] = 99;
        array.vector.Value[0].ShouldBe(3);
        directArrayCopy.values[0] = 99;
        directArray.values.ShouldBe([5, 6]);
        directArrayCopy.Discriminator.ShouldBe(6);
    }
}
