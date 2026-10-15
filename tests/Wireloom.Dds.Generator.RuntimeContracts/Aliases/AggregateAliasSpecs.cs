using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Executes generated AggregateAlias contracts.</summary>
public sealed class AggregateAliasSpecs
{
    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Aggregate.</summary>
    [Fact]
    public void AggregatePreservesItsValueContract()
    {
        // Arrange
        var defaults = new Aggregate();
        var nullCopy = new Aggregate((Aggregate?)null);
        var source = new Aggregate(new Child(17, "nested"));

        // Act
        var copy = new Aggregate(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Aggregate?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value.id++;
        source.Value.id.ShouldBe(17);
        copy.Equals(source).ShouldBeFalse();
        copy.Value = new Child(18, "different");
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for AggregateChain.</summary>
    [Fact]
    public void AggregateChainPreservesItsValueContract()
    {
        // Arrange
        var defaults = new AggregateChain();
        var nullCopy = new AggregateChain((AggregateChain?)null);
        var source = new AggregateChain(new Child(17, "nested"));

        // Act
        var copy = new AggregateChain(source);

        // Assert
        defaults.Value.ShouldNotBeNull();
        nullCopy.Value.ShouldBeNull();
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((AggregateChain?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value.id++;
        source.Value.id.ShouldBe(17);
        copy.Equals(source).ShouldBeFalse();
        copy.Value = new Child(18, "different");
        copy.Equals(source).ShouldBeFalse();
    }
}
