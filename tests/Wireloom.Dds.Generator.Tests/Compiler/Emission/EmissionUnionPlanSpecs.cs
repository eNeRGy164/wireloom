using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionUnionPlanSpecs
{
    [Fact]
    public void AllocatesTheFirstUnoccupiedDiscriminatorForTheDefaultBranch()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["One", "Two"], [0, 1]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "int", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;
        var branchDiscriminator = plan.BranchDiscriminator(defaultBranch);

        // Assert
        defaultDiscriminator.ShouldBe("2");
        branchDiscriminator.ShouldBe("2");
    }

    [Fact]
    public void BuildsExplicitAndDefaultSelectionConditionsFromTheSameLabels()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["One", "Two"], [1, 2]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "int", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var explicitCondition = plan.SelectionCondition(explicitBranch, negated: false, discriminatorName: "discriminator");
        var defaultCondition = plan.SelectionCondition(defaultBranch, negated: true, discriminatorName: "discriminator");

        // Assert
        explicitCondition.ShouldBe("discriminator == One || discriminator == Two");
        defaultCondition.ShouldBe("discriminator == One || discriminator == Two");
    }

    [Fact]
    public void UsesTheDeclaredEnumDefaultDiscriminator()
    {
        // Arrange
        var branch = CreateBranch("first", ["First"], [0]);
        var plan = CreateUnion("Choice", "ChoiceKind", discriminatorIsEnum: true, discriminatorDefaultValue: 3, [branch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;

        // Assert
        defaultDiscriminator.ShouldBe("(ChoiceKind)3");
    }

    [Fact]
    public void IdentifiesExhaustiveBooleanUnionsWithoutADefaultBranch()
    {
        // Arrange
        var branch = CreateBranch("enabled", ["true"], [1]);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [branch]);

        // Assert
        plan.IsExhaustiveBoolean.ShouldBeTrue();
        plan.ManagedDefaultDiscriminator.ShouldBe("false");
    }

    [Fact]
    public void CentralizesNativeDiscriminatorConversions()
    {
        // Arrange
        var branch = CreateBranch("first", ["One"], [1]);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [branch]);

        // Assert
        plan.NativeDiscriminatorType.ShouldBe("byte");
        plan.NativeDiscriminatorReadExpression("_discriminator").ShouldBe("global::System.Convert.ToBoolean(_discriminator)");
        plan.NativeDiscriminatorWriteExpression("sample.Discriminator").ShouldBe("global::System.Convert.ToByte(sample.Discriminator)");
    }

    private static UnionBranchEmissionPlan CreateBranch(
        string name,
        IReadOnlyList<string> labels,
        IReadOnlyList<int> labelValues,
        bool isDefault = false)
    {
        var field = new IdlEmissionField(
            name,
            new PrimitiveEmissionType("long", "int"),
            new EmissionMetadata(
                isKey: false,
                memberId: null,
                isOptional: false,
                valueMetadata: null,
                isExternal: false,
                isMustUnderstand: false,
                memberIdHashSource: null,
                usesAutoIdHash: false));

        return new UnionBranchEmissionPlan(field, new MemberEmissionPlan(field, "Example"), labels, labelValues, isDefault);
    }

    private static IdlEmissionUnion CreateUnion(
        string name,
        string discriminatorType,
        bool discriminatorIsEnum,
        int? discriminatorDefaultValue,
        IReadOnlyList<UnionBranchEmissionPlan> branches) =>
        new(
            name,
            "Example",
            discriminatorType,
            discriminatorIsEnum,
            discriminatorDefaultValue,
            branches,
            IdlExtensibilityKind.Extensible);
}
