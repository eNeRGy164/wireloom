using Omg.Types.Dynamic;
using Rti.Types.Dynamic;
using RuntimeAliases;
using RuntimeAliases.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Exercises metadata creation and support initialization for typedef families.</summary>
public sealed class AliasMetadataSpecs
{
    /// <summary>Checks F3's wide-string annotation correction for both metadata visibilities.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CreatesWideStringAliasMetadata(bool isPublic)
    {
        // Arrange
        const int bound = 16;

        // Act
        var type = WideTextPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        type.RelatedType.ShouldBeOfType<WideStringType>().Bounds.ShouldBe((uint)bound);
        WideTextSupport.Instance.ShouldBeSameAs(WideTextSupport.Instance);
        new WideText(new string('Ω', bound)).ToString().ShouldBe(new string('Ω', bound));
    }

    /// <summary>Checks scalar and enum aliases and their chains.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InitializesScalarAndEnumAliases(bool isPublic)
    {
        // Arrange
        var scalar = ScalarPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var scalarChain = ScalarChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Act
        var enumeration = EnumAliasPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var enumChain = EnumChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        scalar.RelatedType.Kind.ShouldBe(TypeKind.Int32);
        scalarChain.RelatedType.Kind.ShouldBe(TypeKind.Int32);
        enumeration.RelatedType.ShouldBeOfType<EnumType>().MemberCount.ShouldBe(2U);
        enumChain.RelatedType.Kind.ShouldBe(TypeKind.Enumeration);
        ScalarSupport.Instance.ShouldNotBeNull();
        ScalarChainSupport.Instance.ShouldNotBeNull();
        EnumAliasSupport.Instance.ShouldNotBeNull();
        EnumChainSupport.Instance.ShouldNotBeNull();
        StateSupport.Instance.ShouldNotBeNull();
    }

    /// <summary>Checks string, collection, and collection-chain metadata.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InitializesCollectionAliases(bool isPublic)
    {
        // Arrange
        var texts = TextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var wideTexts = WideTextsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Act
        var vector = VectorPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var points = PointsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var numbersChain = NumbersChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var matrixChain = MatrixChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        texts.RelatedType.ShouldBeOfType<SequenceType>().ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(16U);
        wideTexts.RelatedType.ShouldBeOfType<SequenceType>().ContentType.Kind.ShouldBe(TypeKind.WideString);
        vector.RelatedType.ShouldBeOfType<ArrayType>().TotalElementCount.ShouldBe(2U);
        points.RelatedType.ShouldBeOfType<ArrayType>().ContentType.ShouldBeOfType<StructType>();
        numbersChain.RelatedType.ShouldBeOfType<AliasType>();
        matrixChain.RelatedType.ShouldBeOfType<AliasType>();
        TextSupport.Instance.ShouldNotBeNull();
        TextsSupport.Instance.ShouldNotBeNull();
        WideTextsSupport.Instance.ShouldNotBeNull();
        ChildrenSupport.Instance.ShouldNotBeNull();
        VectorSupport.Instance.ShouldNotBeNull();
        PointsSupport.Instance.ShouldNotBeNull();
        NumbersChainSupport.Instance.ShouldNotBeNull();
        MatrixSupport.Instance.ShouldNotBeNull();
        MatrixChainSupport.Instance.ShouldNotBeNull();
    }

    /// <summary>Checks struct/union aliases and their support-backed formatting.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InitializesAggregateAliases(bool isPublic)
    {
        // Arrange
        var aggregate = AggregatePlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Act
        var chain = AggregateChainPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var union = RuntimeUnions.Implementation.ChoiceAliasPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        aggregate.RelatedType.ShouldBeOfType<StructType>();
        chain.RelatedType.ShouldBeOfType<StructType>();
        union.RelatedType.ShouldBeOfType<UnionType>();
        AggregateSupport.Instance.ShouldNotBeNull();
        AggregateChainSupport.Instance.ShouldNotBeNull();
        RuntimeUnions.ChoiceAliasSupport.Instance.ShouldNotBeNull();
        new Aggregate(new Child(17, "nested")).ToString().ShouldContain("nested");
        new AggregateChain(new Child(17, "nested")).ToString().ShouldContain("17");
    }
}
