using Omg.Types.Dynamic;
using Rti.Types;
using Rti.Types.Dynamic;
using RuntimeUnboundedSequence;
using RuntimeUnboundedSequence.Implementation;
using Text = RuntimeUnboundedStrings.Text;
using WideText = RuntimeUnboundedStrings.WideText;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Executes unbounded typedef contracts previously blocked by F1 compilation failures.</summary>
public sealed class UnboundedAliasSpecs
{
    /// <summary>Checks the formatting fallback for every unbounded element family.</summary>
    [Fact]
    public void FormatsUnboundedElementFamilies()
    {
        // Arrange
        var child = new Child { value = 17 };

        // Act / Assert
        new Text("hello").ToString().ShouldBe("hello");
        new WideText("世界").ToString().ShouldBe("世界");
        new Numbers(new Sequence<int> { 17 }).ToString().ShouldBe("[17]");
        new NumbersChain(new Numbers(new Sequence<int> { 17 })).ToString().ShouldBe("[17]");
        new States(new Sequence<State> { State.Ready }).ToString().ShouldBe("[Ready]");
        new Children(new Sequence<Child> { child }).ToString().ShouldBe($"[{child}]");
        new Scalars(new Sequence<int> { 17 }).ToString().ShouldBe("[17]");
        new Texts(new Sequence<string> { "hello" }).ToString().ShouldBe("[\"hello\"]");
        new WideTexts(new Sequence<string> { "世界" }).ToString().ShouldBe("[\"世界\"]");
        new BoundedTexts(new Sequence<string> { "hello" }).ToString().ShouldBe("[\"hello\"]");
        new BoundedWideTexts(new Sequence<string> { "世界" }).ToString().ShouldBe("[\"世界\"]");
    }

    /// <summary>Checks default, value, and copy constructors for unbounded narrow and wide strings.</summary>
    [Fact]
    public void ConstructsAndCopiesUnboundedStrings()
    {
        // Arrange
        var text = new Text("unbounded");
        var wideText = new WideText("世界");

        // Act
        var textCopy = new Text(text);
        var wideCopy = new WideText(wideText);

        // Assert
        new Text().Value.ShouldBeEmpty();
        new WideText().Value.ShouldBeEmpty();
        new Text((Text?)null).Value.ShouldBeEmpty();
        new WideText((WideText?)null).Value.ShouldBeEmpty();
        textCopy.Value.ShouldBe("unbounded");
        wideCopy.Value.ShouldBe("世界");
        textCopy.Equals(text).ShouldBeTrue();
        wideCopy.Equals(wideText).ShouldBeTrue();
        textCopy.GetHashCode().ShouldBe(text.GetHashCode());
        wideCopy.GetHashCode().ShouldBe(wideText.GetHashCode());
        textCopy.Value = "changed";
        wideCopy.Value = "Ω";
        textCopy.Equals(text).ShouldBeFalse();
        wideCopy.Equals(wideText).ShouldBeFalse();
    }

    /// <summary>Checks default/copy construction and the native sequence alias chain surface.</summary>
    [Fact]
    public void ConstructsAndCopiesUnboundedSequences()
    {
        // Arrange
        var numbers = new Numbers(new Sequence<int> { 17, 18 });
        var chain = new NumbersChain(numbers);

        // Act
        var copy = new Numbers(numbers);
        var chainCopy = new NumbersChain(chain);

        // Assert
        new Numbers().Value.ShouldBeEmpty();
        new Numbers((Numbers?)null).Value.ShouldBeNull();
        copy.Value.ShouldBe([17, 18]);
        copy.Value.ShouldNotBeSameAs(numbers.Value);
        copy.Equals(numbers).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(numbers.GetHashCode());
        chainCopy.Equals(chain).ShouldBeTrue();
        chainCopy.Value.ShouldNotBeSameAs(numbers);
        copy.Value[0] = 99;
        chainCopy.Value.Value[0] = 77;
        numbers.Value[0].ShouldBe(17);
        copy.Equals(numbers).ShouldBeFalse();
    }

    /// <summary>Checks effective metadata limits for every unbounded sequence element family.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DescribesEffectiveUnboundedSequenceLimits(bool isPublic)
    {
        // Arrange
        var numbers = NumbersPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var children = ChildrenPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var scalars = ScalarsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Act
        var texts = TextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var wideTexts = WideTextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var states = StatesPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var boundedTexts = BoundedTextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var boundedWideTexts = BoundedWideTextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        numbers.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(100U);
        children.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(100U);
        var scalarSequence = scalars.RelatedType.ShouldBeOfType<SequenceType>();
        scalarSequence.Bounds.ShouldBe(100U);
        scalarSequence.ContentType.Kind.ShouldBe(TypeKind.Int32);
        states.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(100U);
        var textSequence = texts.RelatedType.ShouldBeOfType<SequenceType>();
        textSequence.Bounds.ShouldBe(100U);
        textSequence.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(255U);
        var wideSequence = wideTexts.RelatedType.ShouldBeOfType<SequenceType>();
        wideSequence.Bounds.ShouldBe(100U);
        wideSequence.ContentType.Kind.ShouldBe(TypeKind.WideString);
        wideSequence.ContentType.ShouldBeOfType<WideStringType>().Bounds.ShouldBe(255U);
        boundedTexts.RelatedType.ShouldBeOfType<SequenceType>().ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(8U);
        boundedWideTexts.RelatedType.ShouldBeOfType<SequenceType>().ContentType.ShouldBeOfType<WideStringType>().Bounds.ShouldBe(4U);
        NumbersSupport.Instance.ShouldNotBeNull();
        NumbersChainSupport.Instance.ShouldNotBeNull();
        ChildrenSupport.Instance.ShouldNotBeNull();
        ScalarsSupport.Instance.ShouldNotBeNull();
        StatesSupport.Instance.ShouldNotBeNull();
        TextsSupport.Instance.ShouldNotBeNull();
        WideTextsSupport.Instance.ShouldNotBeNull();
        BoundedTextsSupport.Instance.ShouldNotBeNull();
        BoundedWideTextsSupport.Instance.ShouldNotBeNull();
    }
}
