using RuntimeStringAliasContracts;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Checks initialized managed values through narrow and wide string typedef chains.</summary>
public sealed class StringAliasChainSpecs
{
    /// <summary>Checks defaults, copies, equality, and mutation for narrow string chains.</summary>
    [Fact]
    public void NarrowStringChainsPreserveInitializedValues()
    {
        // Arrange
        var oneHopDefault = new BoundedTextChain();
        var twoHopDefault = new BoundedTextChain2();
        var source = new BoundedTextChain2("bounded");
        var nullCopy = new BoundedTextChain2((BoundedTextChain2?)null);

        // Act
        var copy = new BoundedTextChain2(source);

        // Assert
        oneHopDefault.Value.ShouldBeEmpty();
        twoHopDefault.Value.ShouldBeEmpty();
        nullCopy.Value.ShouldBeEmpty();
        copy.Value.ShouldBe("bounded");
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = "changed";
        source.Value.ShouldBe("bounded");
        copy.Equals(source).ShouldBeFalse();
        new TextChain().Value.ShouldBeEmpty();
    }

    /// <summary>Checks defaults, copies, equality, and mutation for wide string chains.</summary>
    [Fact]
    public void WideStringChainsPreserveInitializedValues()
    {
        // Arrange
        var oneHopDefault = new BoundedWideTextChain();
        var twoHopDefault = new BoundedWideTextChain2();
        var source = new BoundedWideTextChain2("世界");
        var nullCopy = new BoundedWideTextChain2((BoundedWideTextChain2?)null);

        // Act
        var copy = new BoundedWideTextChain2(source);

        // Assert
        oneHopDefault.Value.ShouldBeEmpty();
        twoHopDefault.Value.ShouldBeEmpty();
        nullCopy.Value.ShouldBeEmpty();
        copy.Value.ShouldBe("世界");
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = "Ω";
        source.Value.ShouldBe("世界");
        copy.Equals(source).ShouldBeFalse();
        new WideTextChain().Value.ShouldBeEmpty();
    }
}
