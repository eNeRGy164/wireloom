using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Executes generated ScalarAlias contracts.</summary>
public sealed class ScalarAliasSpecs
{
    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Scalar.</summary>
    [Fact]
    public void ScalarPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Scalar();
        var nullCopy = new Scalar((Scalar?)null);
        var source = new Scalar(17);

        // Act
        var copy = new Scalar(source);

        // Assert
        defaults.Value.ShouldBe(0);
        nullCopy.Value.ShouldBe(0);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Scalar?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = 18;
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for ScalarChain.</summary>
    [Fact]
    public void ScalarChainPreservesItsValueContract()
    {
        // Arrange
        var defaults = new ScalarChain();
        var nullCopy = new ScalarChain((ScalarChain?)null);
        var source = new ScalarChain(17);

        // Act
        var copy = new ScalarChain(source);

        // Assert
        defaults.Value.ShouldBe(0);
        nullCopy.Value.ShouldBe(0);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((ScalarChain?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = 18;
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for EnumAlias.</summary>
    [Fact]
    public void EnumAliasPreservesItsValueContract()
    {
        // Arrange
        var defaults = new EnumAlias();
        var nullCopy = new EnumAlias((EnumAlias?)null);
        var source = new EnumAlias(State.Ready);

        // Act
        var copy = new EnumAlias(source);

        // Assert
        defaults.Value.ShouldBe(State.Idle);
        nullCopy.Value.ShouldBe(State.Idle);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((EnumAlias?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = State.Idle;
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for EnumChain.</summary>
    [Fact]
    public void EnumChainPreservesItsValueContract()
    {
        // Arrange
        var defaults = new EnumChain();
        var nullCopy = new EnumChain((EnumChain?)null);
        var source = new EnumChain(State.Ready);

        // Act
        var copy = new EnumChain(source);

        // Assert
        defaults.Value.ShouldBe(State.Idle);
        nullCopy.Value.ShouldBe(State.Idle);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((EnumChain?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = State.Idle;
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for Text.</summary>
    [Fact]
    public void TextPreservesItsValueContract()
    {
        // Arrange
        var defaults = new Text();
        var nullCopy = new Text((Text?)null);
        var source = new Text("hello");

        // Act
        var copy = new Text(source);

        // Assert
        defaults.Value.ShouldBe(string.Empty);
        nullCopy.Value.ShouldBe(string.Empty);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Text?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = "changed";
        copy.Equals(source).ShouldBeFalse();
    }

    /// <summary>Exercises constructors, assignment, copying, equality, and hashing for WideText.</summary>
    [Fact]
    public void WideTextPreservesItsValueContract()
    {
        // Arrange
        var defaults = new WideText();
        var nullCopy = new WideText((WideText?)null);
        var source = new WideText("世界");

        // Act
        var copy = new WideText(source);

        // Assert
        defaults.Value.ShouldBe(string.Empty);
        nullCopy.Value.ShouldBe(string.Empty);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((WideText?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value = "Ω";
        copy.Equals(source).ShouldBeFalse();
    }
}
