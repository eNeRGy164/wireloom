using Omg.Types.Dynamic;
using Rti.Types.Dynamic;
using RuntimeManaged;
using RuntimeManaged.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Checks generated metadata through RTI's observable dynamic type APIs.</summary>
public sealed class MetadataSpecs
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PreservesScopedUnionMemberIdPolicies(bool isPublic)
    {
        // Arrange
        var hashed = RuntimeUnionIds.Implementation.HashChoicePlugin.CreateDynamicType(isPublic).ShouldBeOfType<UnionType>();

        // Act
        var sequential = RuntimeUnionIds.Implementation.SequentialChoicePlugin.CreateDynamicType(isPublic).ShouldBeOfType<UnionType>();

        // Assert
        hashed.Members.Single(member => member.Name == "hashed").Id.ShouldBe(24944607);
        hashed.Members.Single(member => member.Name == "other").Id.ShouldBe(36855673);
        sequential.Members.Single(member => member.Name == "hashed").Id.ShouldBe(1);
        sequential.Members.Single(member => member.Name == "other").Id.ShouldBe(2);
    }

    /// <summary>Checks member identities, key flags, and optional flags for both visibility modes.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DescribesStructMembers(bool isPublic)
    {
        // Arrange
        var child = ChildPlugin.CreateDynamicType(isPublic).ShouldBeOfType<StructType>();

        // Act
        var type = CollectionsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<StructType>();

        // Assert
        child.Members.Single(member => member.Name == "id").IsKey.ShouldBeTrue();
        child.Members.Single(member => member.Name == "id").Id.ShouldBe(0);
        child.Members.Single(member => member.Name == "text").Id.ShouldBe(1);
        child.Members.Single(member => member.Name == "text").IsKey.ShouldBeFalse();
        child.Members.Single(member => member.Name == "id").Type.Kind.ShouldBe(TypeKind.Int32);
        type.Members.Single(member => member.Name == "optionalNumber").IsOptional.ShouldBeTrue();
        type.Members.Single(member => member.Name == "optionalChild").IsOptional.ShouldBeTrue();
        type.Members.Single(member => member.Name == "optionalPoints").IsOptional.ShouldBeTrue();
        type.Members.Single(member => member.Name == "child").IsOptional.ShouldBeFalse();
        ChildSupport.Instance.ShouldBeSameAs(ChildSupport.Instance);
        CollectionsSupport.Instance.DynamicType.ShouldBeOfType<StructType>();
    }

    /// <summary>Checks bounds, dimensions, and collection element types.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DescribesCollectionsAndInheritance(bool isPublic)
    {
        // Arrange
        var type = CollectionsPlugin.CreateDynamicType(isPublic).ShouldBeOfType<StructType>();

        // Act
        var numbers = type.Members.Single(member => member.Name == "numbers").Type.ShouldBeOfType<SequenceType>();
        var matrix = type.Members.Single(member => member.Name == "matrix").Type.ShouldBeOfType<ArrayType>();
        var derived = DerivedPlugin.CreateDynamicType(isPublic).ShouldBeOfType<StructType>();

        // Assert
        numbers.Bounds.ShouldBe(4U);
        numbers.ContentType.Kind.ShouldBe(TypeKind.Int32);
        matrix.DimensionCount.ShouldBe(2U);
        matrix.GetDimension(0).ShouldBe(2U);
        matrix.GetDimension(1).ShouldBe(3U);
        matrix.ContentType.Kind.ShouldBe(TypeKind.Int32);
        type.Members.Single(member => member.Name == "wideTexts").Type.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(100U);
        derived.Parent.ShouldNotBeNull();
        derived.Parent!.Members.Single(member => member.Name == "id").IsKey.ShouldBeTrue();
        DerivedSupport.Instance.ShouldNotBeNull();
        DerivedValueSupport.Instance.ShouldNotBeNull();
    }

    /// <summary>Checks enum values, alias relationships, and support singletons.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DescribesEnumsAndTypedefs(bool isPublic)
    {
        // Arrange
        var enumeration = StatePlugin.CreateDynamicType(isPublic).ShouldBeOfType<EnumType>();

        // Act
        var scalar = RuntimeAliases.Implementation.ScalarPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var numbers = RuntimeAliases.Implementation.NumbersPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var matrix = RuntimeAliases.Implementation.MatrixPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();
        var text = RuntimeAliases.Implementation.TextPlugin.CreateDynamicType(isPublic).ShouldBeOfType<AliasType>();

        // Assert
        enumeration.Members.Select(member => member.Name).ShouldBe(["Idle", "Ready"]);
        enumeration.Members.Select(member => member.Ordinal).ShouldBe([0, 1]);
        scalar.RelatedType.Kind.ShouldBe(TypeKind.Int32);
        numbers.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(4U);
        matrix.RelatedType.ShouldBeOfType<ArrayType>().TotalElementCount.ShouldBe(6U);
        text.RelatedType.ShouldBeOfType<StringType>().Bounds.ShouldBe(16U);
        StateSupport.Instance.ShouldNotBeNull();
        RuntimeAliases.ScalarSupport.Instance.ShouldNotBeNull();
        RuntimeAliases.NumbersSupport.Instance.ShouldNotBeNull();
        RuntimeAliases.AggregateSupport.Instance.ShouldNotBeNull();
    }

    /// <summary>Checks union labels including RTI's signed narrow-character metadata.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DescribesUnionLabels(bool isPublic)
    {
        // Arrange
        var choice = RuntimeUnions.Implementation.ChoicePlugin.CreateDynamicType(isPublic).ShouldBeOfType<UnionType>();

        // Act
        var character = RuntimeDiscriminators.Implementation.CharacterChoicePlugin.CreateDynamicType(isPublic).ShouldBeOfType<UnionType>();

        // Assert
        choice.Discriminator.Kind.ShouldBe(TypeKind.Int32);
        choice.Members.Single(member => member.Name == "text").Labels.ShouldBe([1, 2]);
        choice.Members.Single(member => member.Name == "flag").Labels.ShouldContain((int)UnionMember.DefaultLabel);
        character.Members.Single(member => member.Name == "other").Labels.ShouldBe([-2]);
        RuntimeUnions.ChoiceSupport.Instance.ShouldBeSameAs(RuntimeUnions.ChoiceSupport.Instance);
        RuntimeUnions.ChoiceSupport.Instance.DynamicType.ShouldBeOfType<UnionType>();
    }
}
