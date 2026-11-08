using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionPlanDecisionSpecs
{
    [Fact]
    public void ScalarAndAggregateAliasChainsKeepTheirMemberMapping()
    {
        // Arrange
        IdlType[] targets =
        [
            new IdlType.Primitive("long"),
            new IdlType.Struct("Example::Payload"),
            new IdlType.Union("Example::Choice")
        ];
        string[] expectedTypes = ["int", "Payload", "Choice"];

        // Act
        var projected = targets.Select(target => EmissionTypeProjector.ToCollectionElementType(
            new IdlType.Alias("Example::Outer", new IdlType.Alias("Example::Inner", target)),
            currentNamespace: "Example")).ToArray();

        // Assert
        projected.Select(type => type.CSharpType).ShouldBe(expectedTypes);
    }

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
    public void CollectionAliasPlanTreatsTransparentAliasesAsUnderlyingValues()
    {
        // Arrange
        var primitiveAlias = new AliasEmissionType(
            "Example::Scalar",
            new PrimitiveEmissionType("long", "int"),
            "int");
        var stringAlias = new AliasEmissionType(
            "Example::Name",
            new StringEmissionType(isWide: false, bound: 9, isBounded: true),
            "string");
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
        primitivePlan.CollectionElementIsAggregate.ShouldBeFalse();
        CollectionElementCopyPolicy.RequiresManagedCopy(primitiveAlias).ShouldBeFalse();
        stringPlan.IsString.ShouldBeTrue();
        stringPlan.CollectionElementIsAggregate.ShouldBeFalse();
        CollectionElementCopyPolicy.RequiresManagedCopy(stringAlias).ShouldBeFalse();
        CollectionElementCopyPolicy.RequiresManagedCopy(new PrimitiveEmissionType("long", "int")).ShouldBeFalse();
        CollectionElementCopyPolicy.RequiresManagedCopy(new AliasEmissionType(
            "Example::ErasedScalar",
            new PrimitiveEmissionType("long", "int"),
            "int")).ShouldBeFalse();
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
        initialize.ShouldBe("values.Initialize<int>(max: 3, absoluteMax: 3, allocateMemory);");
        destroy.ShouldBe("values.Destroy(optionalsOnly);");
        fromNative.ShouldBe("values.FromNative((Sequence<int>)sample.values);");
        toNative.ShouldBe("values.ToNative((Sequence<int>)sample.values);");
    }

    [Fact]
    public void RecursiveSequenceDynamicTypeFallbackUsesTheUnboundedBound()
    {
        // Arrange
        var sequence = new SequenceEmissionType(
            new StructEmissionType("Node", supportType: "Example.Node"),
            bound: 100,
            cSharpType: "ISequence<Node>",
            isBounded: false,
            isUnbounded: true);
        var field = new IdlEmissionField(
            "children",
            sequence,
            new EmissionMetadata(
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
        var expression = plan.BuildDynamicTypeExpression(
            "Example.Implementation",
            recursiveTypeName: "Example.Node",
            isRecursive: true);

        // Assert
        expression.ShouldContain("CreateSequenceWithAccessInfo");
        expression.ShouldContain("SequenceType.Unbounded");
    }

    [Fact]
    public void NativeEnumInitializationUsesZeroWithoutRedundantCasts()
    {
        // Arrange
        var enumType = new EnumEmissionType("Example.State", 0, "Zero");
        var implicitDefaultPlan = CreateEnumMemberPlan(enumType, valueMetadata: null);
        var explicitDefaultPlan = CreateEnumMemberPlan(enumType, new IdlMemberValueMetadata(defaultValue: new IdlScalarValue.Enumeration("Example.State", "Zero", 0)));

        // Act
        var implicitInitialization = implicitDefaultPlan.BuildInitializeStatement();
        var explicitInitialization = explicitDefaultPlan.BuildInitializeStatement();

        // Assert
        implicitInitialization.ShouldBe("state = 0;");
        explicitInitialization.ShouldBe("state = 0;");
    }

    private static MemberEmissionPlan CreateEnumMemberPlan(EnumEmissionType enumType, IdlMemberValueMetadata? valueMetadata)
    {
        var field = new IdlEmissionField(
            name: "state",
            type: enumType,
            metadata: new EmissionMetadata(
                isKey: false,
                memberId: null,
                isOptional: false,
                valueMetadata: valueMetadata,
                isExternal: false,
                isMustUnderstand: false,
                memberIdHashSource: null,
                usesAutoIdHash: false));

        return new MemberEmissionPlan(field, currentNamespace: "Example");
    }
}
