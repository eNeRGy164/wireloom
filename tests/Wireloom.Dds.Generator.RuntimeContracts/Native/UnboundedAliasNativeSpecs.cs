using Rti.Types;
using RuntimeUnboundedSequence;
using RuntimeUnboundedSequence.Implementation;
using Text = RuntimeUnboundedStrings.Text;
using TextUnmanaged = RuntimeUnboundedStrings.Implementation.TextUnmanaged;
using WideText = RuntimeUnboundedStrings.WideText;
using WideTextUnmanaged = RuntimeUnboundedStrings.Implementation.WideTextUnmanaged;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks native conversions and lifecycle for the repaired unbounded typedefs.</summary>
public sealed class UnboundedAliasNativeSpecs
{
    /// <summary>Checks narrow strings through the documented effective 255-byte limit.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedStrings(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Text(new string('x', 255));

        // Act
        var result = NativeSample.RoundTrip<Text, TextUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(new string('x', 255));
    }

    /// <summary>Checks wide strings through the effective 255-code-unit limit.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedWideStrings(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new WideText(new string('Ω', 255));

        // Act
        var result = NativeSample.RoundTrip<WideText, WideTextUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(new string('Ω', 255));
    }

    /// <summary>Checks primitive sequences at the effective 100-element limit.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedNumbers(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Numbers(new Sequence<int>(Enumerable.Range(1, 100)));

        // Act
        var result = NativeSample.RoundTrip<Numbers, NumbersUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(Enumerable.Range(1, 100));
    }

    /// <summary>Checks enum sequence native storage.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedEnums(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new States(new Sequence<State> { State.Ready, State.Idle });

        // Act
        var result = NativeSample.RoundTrip<States, StatesUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe([State.Ready, State.Idle]);
    }

    /// <summary>Checks aggregate sequence allocation and conversion.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedAggregates(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Children(new Sequence<Child> { new Child { value = 17 }, new Child { value = 18 } });

        // Act
        var result = NativeSample.RoundTrip<Children, ChildrenUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.Count.ShouldBe(2);
        result.Value[0].value.ShouldBe(17);
        result.Value[1].value.ShouldBe(18);
    }

    /// <summary>Checks sequences of scalar typedef elements.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedScalarAliases(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Scalars(new Sequence<int> { 17, 18 });

        // Act
        var result = NativeSample.RoundTrip<Scalars, ScalarsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value[0].ShouldBe(17);
        result.Value[1].ShouldBe(18);
    }

    /// <summary>Checks narrow-string sequence allocation with default element bounds.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedTextSequences(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Texts(new Sequence<string> { "hello", new string('x', 255) });

        // Act
        var result = NativeSample.RoundTrip<Texts, TextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["hello", new string('x', 255)]);
    }

    /// <summary>Checks wide-string sequence allocation with default element bounds.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedWideTextSequences(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new WideTexts(new Sequence<string> { "世界", new string('Ω', 255) });

        // Act
        var result = NativeSample.RoundTrip<WideTexts, WideTextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["世界", new string('Ω', 255)]);
    }

    /// <summary>Checks an unbounded sequence whose narrow-string elements are bounded.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void PreservesNarrowStringElementBounds(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new BoundedTexts(new Sequence<string> { "12345678" });

        // Act
        var result = NativeSample.RoundTrip<BoundedTexts, BoundedTextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["12345678"]);
    }

    /// <summary>Checks an unbounded sequence whose wide-string elements are bounded.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void PreservesWideStringElementBounds(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new BoundedWideTexts(new Sequence<string> { "世界世界" });

        // Act
        var result = NativeSample.RoundTrip<BoundedWideTexts, BoundedWideTextsUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.ShouldBe(["世界世界"]);
    }

    /// <summary>Checks a typedef chain wrapping an unbounded sequence.</summary>
    [Theory]
    [MemberData(nameof(NativeCases.All), MemberType = typeof(NativeCases))]
    public void ConvertsUnboundedSequenceAliasChains(bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new NumbersChain(new Numbers(new Sequence<int> { 17, 18 }));

        // Act
        var result = NativeSample.RoundTrip<NumbersChain, NumbersChainUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Value.Value.ShouldBe([17, 18]);
    }
}
