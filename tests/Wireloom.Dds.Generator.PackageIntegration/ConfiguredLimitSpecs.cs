using ConfiguredLimits.Implementation;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Verifies that packed-package file metadata reaches RTI type metadata.</summary>
public sealed class ConfiguredLimitSpecs
{
    /// <summary>Applies per-file sequence and string limits to members, aliases, and unions.</summary>
    [Fact]
    public void AppliesConfiguredLimitsToGeneratedMetadata()
    {
        // Arrange
        var sample = SamplePlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<StructType>();
        var choice = ChoicePlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<UnionType>();
        var numbersAlias = NumbersPlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<AliasType>();
        var labelsAlias = LabelsPlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<AliasType>();
        var labelAlias = LabelPlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<AliasType>();

        // Act
        var directNumbers = sample.Members.Single(member => member.Name == "directNumbers").Type.ShouldBeOfType<SequenceType>();
        var directLabels = sample.Members.Single(member => member.Name == "directLabels").Type.ShouldBeOfType<SequenceType>();
        var directLabel = sample.Members.Single(member => member.Name == "directLabel").Type.ShouldBeOfType<StringType>();
        var directWideLabel = sample.Members.Single(member => member.Name == "directWideLabel").Type.ShouldBeOfType<WideStringType>();
        var aliasedNumbers = sample.Members.Single(member => member.Name == "aliasedNumbers").Type.ShouldBeOfType<AliasType>();
        var aliasedLabels = sample.Members.Single(member => member.Name == "aliasedLabels").Type.ShouldBeOfType<AliasType>();
        var aliasedLabel = sample.Members.Single(member => member.Name == "aliasedLabel").Type.ShouldBeOfType<AliasType>();
        var unionNumbers = choice.Members.Single(member => member.Name == "numbers").Type.ShouldBeOfType<SequenceType>();
        var unionLabel = choice.Members.Single(member => member.Name == "label").Type.ShouldBeOfType<StringType>();

        // Assert
        directNumbers.Bounds.ShouldBe(7U);
        directLabels.Bounds.ShouldBe(7U);
        directLabels.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(19U);
        directLabel.Bounds.ShouldBe(19U);
        directWideLabel.Bounds.ShouldBe(19U);
        aliasedNumbers.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(7U);
        var aliasedLabelSequence = aliasedLabels.RelatedType.ShouldBeOfType<SequenceType>();
        aliasedLabelSequence.Bounds.ShouldBe(7U);
        aliasedLabelSequence.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(19U);
        aliasedLabel.RelatedType.ShouldBeOfType<StringType>().Bounds.ShouldBe(19U);
        numbersAlias.RelatedType.ShouldBeOfType<SequenceType>().Bounds.ShouldBe(7U);
        var typedefLabelSequence = labelsAlias.RelatedType.ShouldBeOfType<SequenceType>();
        typedefLabelSequence.Bounds.ShouldBe(7U);
        typedefLabelSequence.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe(19U);
        labelAlias.RelatedType.ShouldBeOfType<StringType>().Bounds.ShouldBe(19U);
        unionNumbers.Bounds.ShouldBe(7U);
        unionLabel.Bounds.ShouldBe(19U);
    }

    /// <summary>Uses the package consumer's project properties when a file has no override.</summary>
    [Fact]
    public void AppliesProjectDefaultLimitsToGeneratedMetadata()
    {
        // Arrange
        var sample = ProjectLimits.Implementation.SamplePlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<StructType>();

        // Act
        var numbers = sample.Members.Single(member => member.Name == "numbers").Type.ShouldBeOfType<SequenceType>();
        var label = sample.Members.Single(member => member.Name == "label").Type.ShouldBeOfType<StringType>();
        var wideLabel = sample.Members.Single(member => member.Name == "wideLabel").Type.ShouldBeOfType<WideStringType>();

        // Assert
        numbers.Bounds.ShouldBe(11U);
        label.Bounds.ShouldBe(21U);
        wideLabel.Bounds.ShouldBe(21U);
    }
}
