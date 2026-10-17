using System.Globalization;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionUnionPlanSpecs
{
    /// <summary>Allows a boolean fallback exactly when no explicit branch matches.</summary>
    [Theory]
    [InlineData(0, false, "discriminator")]
    [InlineData(0, true, "!discriminator")]
    [InlineData(1, false, "!discriminator")]
    [InlineData(1, true, "discriminator")]
    public void BooleanDefaultConditionsComplementTheExplicitBranch(long explicitLabel, bool negated, string expected)
    {
        // Arrange
        var explicitBranch = CreateBranch("selected", [explicitLabel == 0 ? "false" : "true"], [explicitLabel]);
        var fallback = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, fallback]);

        // Act
        var condition = plan.SelectionCondition(fallback, negated, discriminatorName: "discriminator");

        // Assert
        condition.ShouldBe(expected);
    }

    [Fact]
    public void SeparatesTypeDefaultFromDefaultBranchSelection()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["One", "Two"], [0, 1]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "int", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;
        var branchDiscriminator = plan.BranchDiscriminator(defaultBranch);

        // Assert
        defaultDiscriminator.ShouldBe("0");
        plan.DefaultDiscriminatorBranch.ShouldBeSameAs(explicitBranch);
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
    public void BuildsUnconditionalSelectionConditionsForBooleanDefaultOnlyUnions()
    {
        // Arrange
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [defaultBranch]);

        // Act
        var getterCondition = plan.SelectionCondition(defaultBranch, negated: true);
        var setterCondition = plan.SelectionCondition(defaultBranch, negated: false);

        // Assert
        getterCondition.ShouldBe("false");
        setterCondition.ShouldBe("true");
    }

    [Fact]
    public void BuildsBooleanSelectionConditionsWithoutComparingAgainstBooleanLiterals()
    {
        // Arrange
        var trueBranch = CreateBranch("affirmative", ["true"], [1]);
        var falseBranch = CreateBranch("negative", ["false"], [0]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [trueBranch, falseBranch, defaultBranch]);

        // Act
        var trueGetter = plan.SelectionCondition(trueBranch, negated: true, discriminatorName: "discriminator");
        var falseSetter = plan.SelectionCondition(falseBranch, negated: false, discriminatorName: "discriminator");
        var defaultGetter = plan.SelectionCondition(defaultBranch, negated: true, discriminatorName: "discriminator");
        var defaultSetter = plan.SelectionCondition(defaultBranch, negated: false, discriminatorName: "discriminator");

        // Assert
        trueGetter.ShouldBe("!discriminator");
        falseSetter.ShouldBe("!discriminator");
        defaultGetter.ShouldBe("discriminator || !discriminator");
        defaultSetter.ShouldBe("!discriminator && discriminator");
    }

    [Theory]
    [InlineData(3, "(ChoiceKind)3")]
    [InlineData(-7, "(ChoiceKind)(-7)")]
    [InlineData(0, "0")]
    public void UsesTheDeclaredEnumDefaultDiscriminator(int value, string expression)
    {
        // Arrange
        var branch = CreateBranch("first", ["First"], [0]);
        var plan = CreateUnion("Choice", "ChoiceKind", discriminatorIsEnum: true, discriminatorDefaultValue: value, [branch]);

        // Act
        var defaultDiscriminator = plan.ManagedDefaultDiscriminator;
        var nativeDiscriminatorType = plan.NativeDiscriminatorType;

        // Assert
        defaultDiscriminator.ShouldBe(expression);
        nativeDiscriminatorType.ShouldBe("ChoiceKind");
    }

    [Fact]
    public void FormatsIntegerDiscriminatorsUsingInvariantCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            // Arrange
            var labelValues = Enumerable.Range(0, sbyte.MaxValue + 1).Select(value => (long)value).ToArray();
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
            var defaultDiscriminator = plan.BranchDiscriminator(defaultBranch);

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
    [InlineData("char", true)]
    [InlineData("wchar", false)]
    public void KeepsCharacterDefaultSelectionWithinTheNativeRange(string discriminatorIdlType, bool isExhaustive)
    {
        // Arrange
        var values = Enumerable.Range(0, 256).Select(value => (long)value).ToArray();
        var labels = values.Select(value => value.ToString(CultureInfo.InvariantCulture)).ToArray();
        var branch = CreateBranch("first", labels, values);
        var fallback = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "char", discriminatorIsEnum: false, discriminatorDefaultValue: null,
            [branch, fallback], discriminatorIdlType: discriminatorIdlType);

        // Act
        Func<string> selectDefault = () => plan.BranchDiscriminator(fallback);

        // Assert
        if (isExhaustive)
        {
            Should.Throw<InvalidOperationException>(selectDefault)
                .Message.ShouldContain("no representable default discriminator");
        }
        else
        {
            selectDefault().ShouldBe("(char)256");
        }
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
        var defaultDiscriminator = plan.BranchDiscriminator(defaultBranch);

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
        var defaultDiscriminator = plan.BranchDiscriminator(defaultBranch);

        // Assert
        defaultDiscriminator.ShouldBe("(char)1");
        plan.ManagedDefaultDiscriminator.ShouldBe("'\\0'");
    }

    [Theory]
    [InlineData("char", 254)]
    [InlineData("wchar", 937)]
    public void FormatsNumericCharacterLabelsAsCharacterExpressions(string discriminatorIdlType, int labelValue)
    {
        // Arrange
        var label = labelValue.ToString(CultureInfo.InvariantCulture);
        var branch = CreateBranch("value", [label], [labelValue]);
        var plan = CreateUnion(
            "Choice",
            "char",
            discriminatorIsEnum: false,
            discriminatorDefaultValue: null,
            [branch],
            discriminatorIdlType: discriminatorIdlType);

        // Act
        var expression = plan.ManagedDiscriminatorLabel(label);

        // Assert
        expression.ShouldBe($"(char){label}");
        plan.BranchDiscriminator(branch).ShouldBe(expression);
        plan.SelectionCondition(branch, negated: false).ShouldBe($"Discriminator == {expression}");
    }

    [Fact]
    public void RejectsAnExhaustiveBooleanDefaultBranchAtEmissionPlanning()
    {
        // Arrange
        var explicitBranch = CreateBranch("first", ["false", "true"], [0, 1]);
        var defaultBranch = CreateBranch("fallback", [], [], isDefault: true);
        var plan = CreateUnion("Choice", "bool", discriminatorIsEnum: false, discriminatorDefaultValue: null, [explicitBranch, defaultBranch]);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => plan.BranchDiscriminator(defaultBranch));

        // Assert
        exception.Message.ShouldContain("no representable default discriminator");
    }

    private static UnionBranchEmissionPlan CreateBranch(
        string name,
        IReadOnlyList<string> labels,
        IReadOnlyList<long> labelValues,
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
            discriminatorFirstValue: discriminatorDefaultValue,
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
