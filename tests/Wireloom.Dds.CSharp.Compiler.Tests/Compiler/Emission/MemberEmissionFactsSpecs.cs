using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class MemberEmissionFactsSpecs
{
    [Fact]
    public void CapturesResolvedMemberShapeAndMetadataAsImmutableFacts()
    {
        // Arrange
        var type = new SequenceEmissionType(new StructEmissionType("Sample"), 4, "ISequence<Sample>");
        var field = new IdlEmissionField(
            name: "values",
            type: type,
            metadata: new EmissionMetadata(
                isKey: true,
                memberId: 3,
                isOptional: true,
                valueMetadata: null,
                isExternal: false,
                isMustUnderstand: true,
                memberIdHashSource: "values",
                usesAutoIdHash: false));

        // Act
        var facts = new MemberEmissionFacts(field, currentNamespace: "Example");

        // Assert
        facts.Name.ShouldBe("values");
        facts.Type.Shape.Kind.ShouldBe(EmissionShapeKind.Sequence);
        facts.IsSequence.ShouldBeTrue();
        facts.IsAggregate.ShouldBeFalse();
        facts.HasAggregateElement.ShouldBeTrue();
        facts.IsOptional.ShouldBeTrue();
        facts.MemberId.ShouldBe(3);
        facts.CurrentNamespace.ShouldBe("Example");
    }

    [Fact]
    public void DerivesUnmanagedNamesFromUnqualifiedAggregateTypes()
    {
        // Arrange
        var field = new IdlEmissionField(
            name: "value",
            type: new StructEmissionType("Sample"),
            metadata: new EmissionMetadata(
                isKey: false,
                memberId: null,
                isOptional: false,
                valueMetadata: null,
                isExternal: false,
                isMustUnderstand: false,
                memberIdHashSource: null,
                usesAutoIdHash: false));
        var plan = new MemberEmissionPlan(field, currentNamespace: "Example");

        // Act
        var unmanagedType = plan.NativeStorageTypeFor("Example.Implementation");

        // Assert
        unmanagedType.ShouldBe("global::Implementation.SampleUnmanaged");
    }
}
