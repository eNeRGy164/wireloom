using Wireloom.Compiler.Emission;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionResultSpecs
{
    [Fact]
    public void GeneratedNameUsesEscapedTypeAndStableHintName()
    {
        // Arrange
        var hintName = new GeneratedName("Example.Implementation", "@class").HintName;

        // Assert
        hintName.ShouldBe("Example.Implementation.@class.g.cs");
    }

    [Fact]
    public void EmissionResultPreservesEmissionOrderAndRejectsDuplicateHints()
    {
        // Arrange
        var result = new EmissionResult();
        var firstName = new GeneratedName(string.Empty, "first");
        var duplicateName = new GeneratedName(string.Empty, "first");

        // Act
        result.Add(firstName, "first");

        // Assert
        result.Sources[0].Source.ShouldBe("first");
        Should.Throw<InvalidOperationException>(() => result.Add(duplicateName, "duplicate"));
    }

    [Fact]
    public void MemberPoliciesKeepCollectionInitializationAndLifecycleDecisionsTogether()
    {
        // Arrange
        var sequenceShape = new SequenceEmissionType(
            new PrimitiveEmissionType("long", "int"),
            3,
            "ISequence<int>").Shape.Kind;

        // Act
        var initialization = MemberEmissionPolicies.GetManagedInitialization(
            sequenceShape,
            isOptional: false,
            isSequence: true,
            isArray: false,
            isAggregate: false);
        var destruction = MemberEmissionPolicies.GetDestroyKind(
            isAggregate: false,
            isSequence: true,
            isArray: false,
            isString: false,
            isOptionalScalar: false);

        // Assert
        initialization.ShouldBe(ManagedInitializationKind.Sequence);
        destruction.ShouldBe(NativeDestroyKind.Collection);
    }
}
