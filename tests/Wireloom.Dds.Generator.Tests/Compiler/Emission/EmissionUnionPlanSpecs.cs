using System.Globalization;
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
    public void BuildsPositiveDefaultSelectionConditionsFromExplicitLabels()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["One", "Two"], [1, 2]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "int", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var condition = plan.SelectionCondition(defaultBranch, negated: false, discriminatorName: "discriminator");

        // Assert
        condition.ShouldBe("discriminator != One && discriminator != Two");
    }

    [Fact]
    public void BuildsUnconditionalSelectionConditionsForDefaultOnlyUnions()
    {
        // Arrange
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "int", discriminatorIsEnum: false, discriminatorDefaultValue: null, [defaultBranch]);

        // Act
        var getterCondition = plan.SelectionCondition(defaultBranch, negated: true);
        var setterCondition = plan.SelectionCondition(defaultBranch, negated: false);

        // Assert
        getterCondition.ShouldBe("false");
        setterCondition.ShouldBe("true");
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
    public void FormatsIntegerDiscriminatorsUsingInvariantCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            // Arrange
            var labelValues = Enumerable.Range(0, sbyte.MaxValue + 1).ToArray();
            var labels = labelValues.Select(value => value.ToString(CultureInfo.InvariantCulture)).ToArray();
            var explicitBranch = CreateBranch("first", labels, labelValues);
            var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
            var plan = CreateUnion(
                "Choice",
                "sbyte",
                discriminatorIsEnum: false,
                discriminatorDefaultValue: null,
                [explicitBranch, defaultBranch]);
            var customCulture = (CultureInfo)originalCulture.Clone();
            customCulture.NumberFormat.NegativeSign = "−";
            CultureInfo.CurrentCulture = customCulture;

            // Act
            var defaultDiscriminator = plan.ManagedDefaultDiscriminator;

            // Assert
            defaultDiscriminator.ShouldBe("-128");
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void IdentifiesExhaustiveBooleanUnionsWithoutADefaultBranch()
    {
        // Arrange
        var branch = CreateBranch("enabled", ["false", "true"], [0, 1]);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [branch]);

        // Assert
        plan.IsExhaustiveBoolean.ShouldBeTrue();
        plan.ManagedDefaultDiscriminator.ShouldBe("false");
    }

    [Fact]
    public void DoesNotIdentifyBooleanUnionsWithAnUncoveredLabelAsExhaustive()
    {
        // Arrange
        var branch = CreateBranch("enabled", ["true"], [1]);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [branch]);

        // Assert
        plan.IsExhaustiveBoolean.ShouldBeFalse();
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

    [Fact]
    public void PreservesWideCharacterNativeDiscriminatorMapping()
    {
        // Arrange
        var branch = CreateBranch("first", ["97"], [97]);
        var plan = CreateUnion(
            "Choice",
            "char",
            discriminatorIsEnum: false,
            discriminatorDefaultValue: null,
            [branch],
            discriminatorIdlType: "wchar");

        // Assert
        plan.NativeDiscriminatorType.ShouldBe("short");
        plan.NativeDiscriminatorReadExpression("_discriminator").ShouldBe("(char)_discriminator");
        plan.NativeDiscriminatorWriteExpression("sample.Discriminator").ShouldBe("(short)sample.Discriminator");
    }

    [Theory]
    [InlineData("byte")]
    [InlineData("short")]
    [InlineData("ushort")]
    public void ResolvesUnsignedAndShortDiscriminatorRanges(string discriminatorType)
    {
        // Arrange
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", discriminatorType, discriminatorIsEnum: false, discriminatorDefaultValue: null, [defaultBranch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;

        // Assert
        defaultDiscriminator.ShouldBe("0");
    }

    [Fact]
    public void FormatsNonzeroCharacterDefaultDiscriminators()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["0"], [0]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "char", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;

        // Assert
        defaultDiscriminator.ShouldBe("(char)1");
    }

    [Fact]
    public void RejectsAnExhaustiveBooleanDefaultBranchAtEmissionPlanning()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["false", "true"], [0, 1]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => plan.ManagedDefaultDiscriminator);

        // Assert
        exception.Message.ShouldContain("no representable default discriminator");
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
        IReadOnlyList<UnionBranchEmissionPlan> branches,
        string? discriminatorIdlType = null) =>
        new(
            name,
            "Example",
            ResolveDiscriminatorIdlType(discriminatorType, discriminatorIdlType),
            discriminatorType,
            discriminatorIsEnum,
            discriminatorDefaultValue,
            branches,
            IdlExtensibilityKind.Extensible);

    private static string ResolveDiscriminatorIdlType(string discriminatorType, string? discriminatorIdlType)
    {
        if (discriminatorIdlType is not null)
        {
            return discriminatorIdlType;
        }

        if (discriminatorType == "bool")
        {
            return "boolean";
        }

        return discriminatorType;
    }
}
