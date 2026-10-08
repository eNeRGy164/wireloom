using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionPlanDecisionSpecs
{
    [Fact]
    public void CollectionAliasPlanRetainsStringAndCollectionShapeFacts()
    {
        // Arrange
        var plan = new CollectionAliasEmissionPlan(
            new StringEmissionType(isWide: false, bound: 9, isBounded: true),
            isSequence: true,
            isArray: false,
            nativeValueRequiresCast: true);

        // Act
        var isCollection = plan.IsCollection;
        var stringBound = plan.StringBound;

        // Assert
        isCollection.ShouldBeTrue();
        plan.IsString.ShouldBeTrue();
        plan.IsPrimitive.ShouldBeFalse();
        plan.NativeValueRequiresCast.ShouldBeTrue();
        stringBound.ShouldBe(9);
    }

    [Fact]
    public void CollectionAliasPlanTreatsNamedAliasesAsAggregateElements()
    {
        // Arrange
        var primitiveAlias = new AliasEmissionType(
            "Example::Scalar",
            new PrimitiveEmissionType("long", "int"),
            "Example.Scalar");
        var stringAlias = new AliasEmissionType(
            "Example::Name",
            new StringEmissionType(isWide: false, bound: 9, isBounded: true),
            "Example.Name");
        var primitivePlan = new CollectionAliasEmissionPlan(
            primitiveAlias,
            isSequence: true,
            isArray: false,
            nativeValueRequiresCast: false);
        var stringPlan = new CollectionAliasEmissionPlan(
            stringAlias,
            isSequence: true,
            isArray: false,
            nativeValueRequiresCast: false);

        // Assert
        primitivePlan.IsString.ShouldBeFalse();
        primitivePlan.CollectionElementIsAggregate.ShouldBeTrue();
        stringPlan.IsString.ShouldBeFalse();
        stringPlan.CollectionElementIsAggregate.ShouldBeTrue();
    }

    [Theory]
    [InlineData((int)EmissionShapeKind.Sequence, false, true, false, false, (int)ManagedInitializationKind.Sequence)]
    [InlineData((int)EmissionShapeKind.Sequence, true, true, false, false, (int)ManagedInitializationKind.None)]
    [InlineData((int)EmissionShapeKind.Array, false, false, true, false, (int)ManagedInitializationKind.Array)]
    [InlineData((int)EmissionShapeKind.Struct, false, false, false, true, (int)ManagedInitializationKind.Aggregate)]
    public void ManagedInitializationPolicyCoversShapeAndOptionality(
        int shapeValue,
        bool isOptional,
        bool isSequence,
        bool isArray,
        bool isAggregate,
        int expectedValue)
    {
        // Act
        var actual = MemberEmissionPolicies.GetManagedInitialization((EmissionShapeKind)shapeValue, isOptional, isSequence, isArray, isAggregate);

        // Assert
        actual.ShouldBe((ManagedInitializationKind)expectedValue);
    }

    [Theory]
    [InlineData(false, false, false, false, false, (int)NativeDestroyKind.None)]
    [InlineData(true, false, false, false, false, (int)NativeDestroyKind.Nested)]
    [InlineData(false, true, false, false, false, (int)NativeDestroyKind.Collection)]
    [InlineData(false, false, false, true, false, (int)NativeDestroyKind.String)]
    [InlineData(false, false, false, false, true, (int)NativeDestroyKind.OptionalPrimitive)]
    public void NativeDestroyPolicyCoversMemberStorage(
        bool isAggregate,
        bool isSequence,
        bool isArray,
        bool isString,
        bool isOptionalScalar,
        int expectedValue)
    {
        // Act
        var actual = MemberEmissionPolicies.GetDestroyKind(isAggregate, isSequence, isArray, isString, isOptionalScalar);

        // Assert
        actual.ShouldBe((NativeDestroyKind)expectedValue);
    }

    [Fact]
    public void ProjectsOptionalPrimitiveMembersWithNullableManagedShape()
    {
        // Arrange
        var member = new IdlMember(
            name: "value",
            type: new IdlType.Primitive("long"),
            metadata: new IdlMemberMetadata(isOptional: true));

        // Act
        var field = EmissionTypeProjector.ToEmissionField(member, currentNamespace: "Example");

        // Assert
        field.CSharpType.ShouldBe("int?");
        field.Type.ShouldBeOfType<OptionalEmissionType>().Target.CSharpType.ShouldBe("int");
    }

    [Fact]
    public void NativeSequencePlanUsesBoundedLifecycleAndConversionOperations()
    {
        // Arrange
        var field = new IdlEmissionField(
            name: "values",
            type: new SequenceEmissionType(new PrimitiveEmissionType("long", "int"), 3, "ISequence<int>"),
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
        var initialize = plan.BuildInitializeStatement();
        var destroy = plan.BuildDestroyStatement();
        var fromNative = plan.BuildFromNativeStatement(forwardKeysOnly: false);
        var toNative = plan.BuildToNativeStatement(forwardKeysOnly: false);

        // Assert
        initialize.ShouldBe("values.Initialize<int>(max: 3, absoluteMax: 3, allocateMemory: allocateMemory);");
        destroy.ShouldBe("values.Destroy(optionalsOnly);");
        fromNative.ShouldBe("values.FromNative((Sequence<int>)sample.values);");
        toNative.ShouldBe("values.ToNative((Sequence<int>)sample.values);");
    }
}
