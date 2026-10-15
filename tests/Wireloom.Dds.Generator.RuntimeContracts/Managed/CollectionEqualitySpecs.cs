using RuntimeManaged;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Checks each aggregate, collection, and optional equality branch.</summary>
public sealed class CollectionEqualitySpecs
{
    /// <summary>Changes one member at a time while preserving all other values.</summary>
    [Theory]
    [InlineData("child")]
    [InlineData("numbers")]
    [InlineData("children")]
    [InlineData("vector")]
    [InlineData("matrix")]
    [InlineData("points")]
    [InlineData("texts")]
    [InlineData("wide-texts")]
    [InlineData("optional-number")]
    [InlineData("optional-text")]
    [InlineData("optional-child")]
    [InlineData("optional-numbers")]
    [InlineData("optional-points")]
    [InlineData("optional-state")]
    [InlineData("optional-wide-text")]
    [InlineData("optional-wide-texts")]
    [InlineData("optional-matrix")]
    public void DetectsEachChangedMember(string member)
    {
        // Arrange
        var sample = CollectionSamples.Create(present: true);
        var copy = new Collections(sample);

        // Act
        switch (member)
        {
            case "child":
                sample.child.id++;
                break;
            case "numbers":
                sample.numbers.Add(99);
                break;
            case "children":
                sample.children[0].id++;
                break;
            case "vector":
                sample.vector[1]++;
                break;
            case "matrix":
                sample.matrix[1, 2]++;
                break;
            case "points":
                sample.points[1].id++;
                break;
            case "texts":
                sample.texts[0] = "changed";
                break;
            case "wide-texts":
                sample.wideTexts[0] = "changed";
                break;
            case "optional-number":
                sample.optionalNumber = null;
                break;
            case "optional-text":
                sample.optionalText = null;
                break;
            case "optional-child":
                sample.optionalChild = null;
                break;
            case "optional-numbers":
                sample.optionalNumbers = null;
                break;
            case "optional-points":
                sample.optionalPoints = null;
                break;
            case "optional-state":
                sample.optionalState = null;
                break;
            case "optional-wide-text":
                sample.optionalWideText = null;
                break;
            case "optional-wide-texts":
                sample.optionalWideTexts = null;
                break;
            case "optional-matrix":
                sample.optionalMatrix = null;
                break;
            default:
                throw new ArgumentException("Unknown member", nameof(member));
        }

        // Assert
        sample.Equals(copy).ShouldBeFalse(member);
        copy.Equals(sample).ShouldBeFalse(member);
    }
}
