using Rti.Types.Dynamic;
using RuntimeStringArrayAliases;
using RuntimeStringArrayAliases.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Exercises DynamicType metadata for fixed string array typedefs.</summary>
public sealed class StringArrayAliasMetadataSpecs
{
    /// <summary>Checks element kinds, bounds, dimensions, and support instances for both metadata visibilities.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CreatesStringArrayAliasMetadata(bool isPublic)
    {
        // Arrange
        const uint bound = 16;

        // Act
        var textVector = TextVectorPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var wideMatrix = WideMatrixPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        var textArray = textVector.RelatedType.ShouldBeOfType<ArrayType>();
        textArray.DimensionCount.ShouldBe(1U);
        textArray.TotalElementCount.ShouldBe(2U);
        textArray.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(bound);

        var wideArray = wideMatrix.RelatedType.ShouldBeOfType<ArrayType>();
        wideArray.DimensionCount.ShouldBe(2U);
        wideArray.TotalElementCount.ShouldBe(2U);
        wideArray.ContentType.ShouldBeOfType<WideStringType>().Bounds.ShouldBe(bound);

        TextVectorSupport.Instance.ShouldNotBeNull();
        WideMatrixSupport.Instance.ShouldNotBeNull();
    }
}
