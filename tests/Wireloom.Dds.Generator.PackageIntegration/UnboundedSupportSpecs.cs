using Omg.Types;
using Rti.Types;
using Rti.Types.Dynamic;
using UnboundedOption;
using UnboundedOption.Implementation;

namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Verifies unbounded metadata and native conversion from the packed analyzer.</summary>
public sealed class UnboundedSupportSpecs
{
    /// <summary>Uses RTI unbounded sentinels and keeps explicit nested bounds finite.</summary>
    [Fact]
    public void EmitsUnboundedMetadataAndBoundedControls()
    {
        // Arrange
        var type = SamplePlugin.CreateDynamicType(isPublic: true).ShouldBeOfType<StructType>();

        // Act
        var values = type.Members.Single(member => member.Name == "values").Type.ShouldBeOfType<SequenceType>();
        var text = type.Members.Single(member => member.Name == "text").Type.ShouldBeOfType<StringType>();
        var wideText = type.Members.Single(member => member.Name == "wideText").Type.ShouldBeOfType<WideStringType>();
        var texts = type.Members.Single(member => member.Name == "texts").Type.ShouldBeOfType<SequenceType>();
        var boundedTexts = type.Members.Single(member => member.Name == "boundedTexts").Type.ShouldBeOfType<SequenceType>();
        var boundedValues = type.Members.Single(member => member.Name == "boundedValues").Type.ShouldBeOfType<SequenceType>();

        // Assert
        values.Bounds.ShouldBe((object)Rti.Types.Dynamic.SequenceType.Unbounded);
        text.Bounds.ShouldBe((object)Rti.Types.Dynamic.StringType.Unbounded);
        wideText.Bounds.ShouldBe((object)Rti.Types.Dynamic.WideStringType.Unbounded);
        texts.Bounds.ShouldBe((object)Rti.Types.Dynamic.SequenceType.Unbounded);
        texts.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe((object)Rti.Types.Dynamic.StringType.Unbounded);
        boundedTexts.Bounds.ShouldBe((object)Rti.Types.Dynamic.SequenceType.Unbounded);
        boundedTexts.ContentType.ShouldBeOfType<StringType>().Bounds.ShouldBe((object)8U);
        boundedValues.Bounds.ShouldBe((object)4U);
    }

    /// <summary>Reuses native storage across empty, large, and smaller samples.</summary>
    [Fact]
    public void ReusesNativeStorageForValuesBeyondConfiguredFallbacks()
    {
        // Arrange
        var native = new SampleUnmanaged();
        var result = new Sample();
        try
        {
            native.Initialize();

            // Act
            native.ToNative(CreateSample(string.Empty, string.Empty));
            var large = CreateSample(new string('x', 300), new string('界', 300));
            large.aliasTexts.Value[0] = new string('é', 300);
            large.aliasWideTexts.Value[1] = new string('界', 300);
            foreach (var value in Enumerable.Range(0, 129))
            {
                large.values.Add(value);
            }
            large.texts.Add(new string('y', 300));
            native.ToNative(large);
            native.FromNative(result);
            var largeCount = result.values.Count;
            var largeText = result.text;
            var largeWideText = result.wideText;
            var largeArrayText = result.aliasTexts.Value[0];
            var largeArrayWideText = result.aliasWideTexts.Value[1];

            var small = CreateSample("small", "短");
            small.values.Add(4);
            native.ToNative(small);
            native.FromNative(result);

            // Assert
            largeCount.ShouldBe(129);
            largeText.Length.ShouldBe(300);
            largeWideText.Length.ShouldBe(300);
            largeArrayText.ShouldBe(new string('é', 300));
            largeArrayWideText.ShouldBe(new string('界', 300));
            result.values.ShouldBe([4]);
            result.text.ShouldBe("small");
            result.wideText.ShouldBe("短");
            result.texts.Count.ShouldBe(0);
            result.aliasTexts.Value.ShouldAllBe(value => value.Length == 0);
            result.aliasWideTexts.Value.ShouldAllBe(value => value.Length == 0);
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Creates a sample with non-null string-array elements for native conversion.</summary>
    private static Sample CreateSample(string text, string wideText)
    {
        var sample = new Sample { text = text, wideText = wideText };
        Array.Fill(sample.aliasTexts.Value, string.Empty);
        Array.Fill(sample.aliasWideTexts.Value, string.Empty);
        return sample;
    }
}
