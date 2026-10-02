using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class MemberEmissionRenderingSpecs
{
    [Fact]
    public void RendersBoundAndValueConstraintSummariesFromImmutableFacts()
    {
        // Arrange
        var field = new IdlEmissionField(
            name: "value",
            type: new PrimitiveEmissionType("long", "int"),
            metadata: new EmissionMetadata(
                isKey: false,
                memberId: null,
                isOptional: false,
                valueMetadata: new IdlMemberValueMetadata(
                    defaultValue: 3,
                    minimum: 1,
                    maximum: 5,
                    defaultExpression: "3",
                    unit: "m"),
                isExternal: false,
                isMustUnderstand: false,
                memberIdHashSource: null,
                usesAutoIdHash: false));
        var facts = new MemberEmissionFacts(field, currentNamespace: "Example");
        var renderer = new MemberEmissionRenderer(facts, currentNamespace: "Example");

        // Act
        var summary = renderer.ValueConstraintSummary;

        // Assert
        summary.ShouldBe("Its value must be between <c>1</c> and <c>5</c>. Its default value is <c>3</c>.");
        renderer.BoundSummary.ShouldBeNull();
    }

    [Fact]
    public void MemberShapePreservesCollectionElementClassification()
    {
        // Arrange
        var type = new SequenceEmissionType(new StructEmissionType("Sample"), 4, "ISequence<Sample>");

        // Act
        var shape = new MemberEmissionShape(type);

        // Assert
        shape.Kind.ShouldBe(EmissionShapeKind.Sequence);
        shape.IsSequence.ShouldBeTrue();
        shape.HasAggregateElement.ShouldBeTrue();
        shape.IsString.ShouldBeFalse();
    }
}
