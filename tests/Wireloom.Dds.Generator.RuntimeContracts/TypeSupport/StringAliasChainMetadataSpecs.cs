using Omg.Types.Dynamic;
using Rti.Types.Dynamic;
using RuntimeStringAliasContracts;
using RuntimeStringAliasContracts.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Checks dynamic string kind and bounds through scalar typedef chains.</summary>
public sealed class StringAliasChainMetadataSpecs
{
    /// <summary>Preserves bounded narrow and wide string metadata for both visibility modes.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PreservesStringTypeAndBoundThroughAliasChains(bool isPublic)
    {
        // Arrange
        var narrow = BoundedTextChain2Plugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var wide = BoundedWideTextChain2Plugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var unboundedWide = WideTextChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Act
        var narrowType = narrow.RelatedType.ShouldBeOfType<StringType>();
        var wideType = wide.RelatedType.ShouldBeOfType<WideStringType>();
        var unboundedWideType = unboundedWide.RelatedType.ShouldBeOfType<WideStringType>();

        // Assert
        narrowType.Bounds.ShouldBe(16U);
        wideType.Bounds.ShouldBe(8U);
        unboundedWideType.Bounds.ShouldBe(255U);
        BoundedTextChain2Support.Instance.ShouldNotBeNull();
        BoundedWideTextChain2Support.Instance.ShouldNotBeNull();
        WideTextChainSupport.Instance.ShouldNotBeNull();
    }
}
