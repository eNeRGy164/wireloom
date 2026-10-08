using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionShapeSpecs
{
    [Fact]
    public void CollectionShapeRetainsElementAggregateFacts()
    {
        // Arrange
        var sequence = new SequenceEmissionType(
            new StructEmissionType("Sample"),
            3,
            "ISequence<Sample>");

        // Act
        var shape = sequence.Shape;

        // Assert
        shape.Kind.ShouldBe(EmissionShapeKind.Sequence);
        shape.IsAggregate.ShouldBeFalse();
        shape.HasAggregateElement.ShouldBeTrue();
        shape.Element!.Kind.ShouldBe(EmissionShapeKind.Struct);
    }

    [Fact]
    public void AliasShapePreservesResolvedEnumAndUnionFacts()
    {
        // Arrange
        var enumAlias = new AliasEmissionType(
            "Example::StatusAlias",
            new EnumEmissionType("Example.Status", 1, "Ready"),
            "Example.Status");
        var unionAlias = new AliasEmissionType(
            "Example::ValueAlias",
            new UnionEmissionType("Example.Value"),
            "Example.Value");

        // Act
        var enumShape = enumAlias.Shape;
        var unionShape = unionAlias.Shape;

        // Assert
        enumShape.IsEnum.ShouldBeTrue();
        unionShape.IsUnion.ShouldBeTrue();
        unionShape.IsAggregate.ShouldBeTrue();
    }

    [Fact]
    public void CollectionElementProjectionPreservesNamedAliasIdentityAndShape()
    {
        // Arrange
        var alias = new IdlType.Alias(
            "Example::ItemAlias",
            new IdlType.Struct("Example::Item"));

        // Act
        var plan = EmissionTypeProjector.ToCollectionElementType(alias, "Example");

        // Assert
        plan.CSharpType.ShouldBe("ItemAlias");
        plan.Shape.IsAggregate.ShouldBeTrue();
        plan.Shape.Kind.ShouldBe(EmissionShapeKind.Alias);
    }

    [Fact]
    public void AliasToAggregateCollectionRetainsWrapperAndElementFacts()
    {
        // Arrange
        var sequence = new SequenceEmissionType(
            new StructEmissionType("Example::Point"),
            3,
            "ISequence<Point>");
        var alias = new AliasEmissionType(
            "Example::Points",
            sequence,
            "ISequence<Point>");

        // Act
        var shape = alias.Shape;

        // Assert
        shape.IsAggregate.ShouldBeTrue();
        shape.HasAggregateElement.ShouldBeTrue();
        shape.IsSequence.ShouldBeFalse();
    }
}
