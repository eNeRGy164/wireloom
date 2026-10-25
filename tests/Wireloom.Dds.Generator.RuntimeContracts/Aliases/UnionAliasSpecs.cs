using RuntimeUnions;
using RuntimeUnions.Implementation;
using Wireloom.Dds.Generator.RuntimeContracts.Native;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Checks aliases whose underlying aggregate is a union.</summary>
public sealed class UnionAliasSpecs
{
    /// <summary>Checks managed constructors, copying, equality, and formatting.</summary>
    [Fact]
    public void CopiesTheSelectedUnionIndependently()
    {
        // Arrange
        var defaults = new ChoiceAlias();
        var source = new ChoiceAlias(new Choice { child = new Child(17, "nested") });

        // Act
        var copy = new ChoiceAlias(source);
        var nullCopy = new ChoiceAlias((ChoiceAlias?)null);

        // Assert
        defaults.Value.Discriminator.ShouldBe(Choice.DefaultDiscriminator);
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals(null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        source.ToString().ShouldContain("nested");
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value.child.id++;
        source.Value.child.id.ShouldBe(17);
        copy.Equals(source).ShouldBeFalse();
        copy.Value = new Choice { number = 23 };
        copy.Value.number.ShouldBe(23);
    }

    /// <summary>Checks union alias native conversion, allocation flags, and cleanup.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsTheSelectedUnion(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new ChoiceAlias(new Choice { text = "alias union" });

        // Act
        var result = NativeSample.RoundTrip<ChoiceAlias, ChoiceAliasUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.Discriminator.ShouldBe(1);
        result.Value.text.ShouldBe("alias union");
    }
}
