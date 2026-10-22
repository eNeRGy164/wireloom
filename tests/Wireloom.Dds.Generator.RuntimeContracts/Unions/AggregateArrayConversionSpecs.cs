using System.Collections.Generic;
using RuntimeUnionArrays;
using RuntimeUnionArrays.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Checks aggregate element ownership when native conversion activates a union array branch.</summary>
public sealed class AggregateArrayConversionSpecs
{
    [Fact]
    public void DefaultArrayUnionBranchesUseValueBasedHashing()
    {
        // Arrange
        var source = new Choice();
        source.Setfallback([new Item { value = 17 }, new Item { value = 23 }], discriminator: 3);
        var copy = new Choice(source);
        var set = new HashSet<Choice> { source };

        // Act
        var isContained = set.Contains(copy);

        // Assert
        copy.ShouldBe(source);
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        isContained.ShouldBeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void NativeConversionInitializesEveryElementOfPreviouslyInactiveBranches(int discriminator)
    {
        // Arrange
        var source = new Choice();
        var target = new Choice { scalar = 9 };
        var native = new ChoiceUnmanaged();
        if (discriminator == 1)
        {
            source.items = [new Item { value = 17 }, new Item { value = 23 }];
        }
        else if (discriminator == 2)
        {
            source.matrix = new Item[,] { { new() { value = 17 }, new() { value = 23 } }, { new() { value = 31 }, new() { value = 47 } } };
        }
        else
        {
            source.Setfallback([new Item { value = 17 }, new Item { value = 23 }], discriminator);
        }

        try
        {
            native.Initialize();
            native.ToNative(source);

            // Act
            native.FromNative(target);

            // Assert
            target.Discriminator.ShouldBe(discriminator);
            target.ShouldBe(source);
            var values = discriminator == 1 ? target.items : discriminator == 2 ? target.matrix.Cast<Item>().ToArray() : target.fallback;
            values[0].value.ShouldBe(17);
            values[1].value.ShouldBe(23);
            values[0].value = 99;
            target.ShouldNotBe(source);
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }
}
