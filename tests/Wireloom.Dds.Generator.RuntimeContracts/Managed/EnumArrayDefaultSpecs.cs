using RuntimeEnumArrayDefaults;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Checks managed array initialization for enums with a nonzero default literal.</summary>
public sealed class EnumArrayDefaultSpecs
{
    [Fact]
    public void ManagedEnumArraysStartAtTheDeclaredDefaultLiteral()
    {
        // Arrange
        var array = new ColorArray();
        var nullCopy = new ColorArray((ColorArray?)null);
        var sample = new Shape();
        var scalarAlias = new ColorAlias(null);

        // Act
        var typedefValues = array.Value;
        var nullCopyValues = nullCopy.Value;
        var inlineValues = sample.inlineColors.Cast<Color>().ToArray();
        var memberAliasValues = sample.colorArray.Value;

        // Assert
        typedefValues.ShouldBe([Color.RED, Color.RED, Color.RED]);
        nullCopyValues.ShouldBe([Color.RED, Color.RED, Color.RED]);
        sample.color.ShouldBe(Color.RED);
        scalarAlias.Value.ShouldBe(Color.RED);
        inlineValues.ShouldBe([Color.RED, Color.RED, Color.RED, Color.RED]);
        memberAliasValues.ShouldBe([Color.RED, Color.RED, Color.RED]);
    }
}
