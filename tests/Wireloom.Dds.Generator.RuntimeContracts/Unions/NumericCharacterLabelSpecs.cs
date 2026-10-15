using RuntimeNumericCharacterLabels;
using RuntimeNumericCharacterLabels.Implementation;
using Rti.Types.Dynamic;
using Wireloom.Dds.Generator.RuntimeContracts.Native;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Checks numeric character labels in managed unions and native conversion.</summary>
public sealed class NumericCharacterLabelSpecs
{
    /// <summary>Preserves high-byte and wide-character numeric labels across union operations.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void NumericCharacterLabelsPreserveManagedAndNativeValues(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var narrow = new Narrow { value = 17 };
        narrow.Setvalue(23, '\u00ff');
        var narrowCopy = new Narrow(narrow);
        var wide = new Wide { value = 31 };
        var wideCopy = new Wide(wide);
        var narrowType = NarrowPlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<UnionType>();
        var wideType = WidePlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<UnionType>();

        // Act
        var narrowResult = NativeSample.RoundTrip<Narrow, NarrowUnmanaged>(narrow, allocatePointers, allocateMemory);
        var wideResult = NativeSample.RoundTrip<Wide, WideUnmanaged>(wide, allocatePointers, allocateMemory);

        // Assert
        narrow.Discriminator.ShouldBe('\u00ff');
        narrow.Get().ShouldBe(23);
        narrowCopy.Equals(narrow).ShouldBeTrue();
        narrowCopy.GetHashCode().ShouldBe(narrow.GetHashCode());
        narrowResult.Discriminator.ShouldBe('\u00ff');
        narrowResult.value.ShouldBe(23);
        narrowType.Members.Single(member => member.Name == "value").Labels.ShouldBe([-2, -1]);

        wide.Discriminator.ShouldBe('\u03a9');
        wide.Get().ShouldBe(31);
        wideCopy.Equals(wide).ShouldBeTrue();
        wideCopy.GetHashCode().ShouldBe(wide.GetHashCode());
        wideResult.Discriminator.ShouldBe('\u03a9');
        wideResult.value.ShouldBe(31);
        wideType.Members.Single(member => member.Name == "value").Labels.ShouldBe([937]);
    }
}
