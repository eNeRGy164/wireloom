using Rti.Types;
using Rti.Types.Dynamic;
using RuntimeUnboundedSequence;
using RuntimeUnboundedSequence.Implementation;
using RuntimeUnboundedStrings;
using RuntimeUnboundedStrings.Implementation;

namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Checks F1 regression fixtures through a freshly packed analyzer.</summary>
public sealed class UnboundedAliasSpecs
{
    /// <summary>Checks that the packed generator emits compilable unbounded aliases and consistent metadata.</summary>
    [Fact]
    public void CompilesUnboundedAliasesAndDescribesTheirEffectiveBounds()
    {
        // Arrange
        var text = new Text();
        var wideText = new WideText();
        var numbers = new Numbers(new Sequence<int> { 17, 18 });

        // Act
        var metadata = NumbersSupport.Instance.DynamicType.ShouldBeOfType<AliasType>();
        var copy = new Numbers(numbers);

        // Assert
        text.Value.ShouldBeEmpty();
        wideText.Value.ShouldBeEmpty();
        metadata.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(100U);
        copy.Value.ShouldBe([17, 18]);
        copy.Value.ShouldNotBeSameAs(numbers.Value);
    }

    /// <summary>Checks native string and sequence conversion using code emitted by the packed analyzer.</summary>
    [Fact]
    public void ConvertsUnboundedValuesThroughNativeStorage()
    {
        // Arrange
        var nativeText = new TextUnmanaged();
        var nativeNumbers = new NumbersUnmanaged();
        var resultText = new Text();
        var resultNumbers = new Numbers();
        try
        {
            nativeText.Initialize();
            nativeNumbers.Initialize();

            // Act
            nativeText.ToNative(new Text(new string('x', 255)));
            nativeNumbers.ToNative(new Numbers(new Sequence<int>(Enumerable.Range(1, 100))));
            nativeText.FromNative(resultText);
            nativeNumbers.FromNative(resultNumbers);

            // Assert
            resultText.Value.ShouldBe(new string('x', 255));
            resultNumbers.Value.ShouldBe(Enumerable.Range(1, 100));
        }
        finally
        {
            nativeText.Destroy(optionalsOnly: false);
            nativeNumbers.Destroy(optionalsOnly: false);
        }
    }
}
