using RuntimeEnumArrayDefaults;
using RuntimeEnumArrayDefaults.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks native array initialization for enums with a nonzero default literal.</summary>
public sealed class EnumArrayDefaultNativeSpecs
{
    [Fact]
    public void NativeUnionEnumArraysStartAtTheDeclaredDefaultLiteral()
    {
        // Arrange
        var native = new PaletteUnmanaged();
        var sample = new Palette();

        try
        {
            // Act
            native.Initialize();
            native.FromNative(sample);

            // Assert
            sample.colors.ShouldBe([Color.RED, Color.RED]);
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    [Fact]
    public void NativeEnumArraysStartAtTheDeclaredDefaultLiteral()
    {
        // Arrange
        var arrayNative = new ColorArrayUnmanaged();
        var shapeNative = new ShapeUnmanaged();
        var array = new ColorArray();
        var sample = new Shape();
        var scalarNative = new ColorAliasUnmanaged();
        var scalar = new ColorAlias();

        try
        {
            // Act
            arrayNative.Initialize();
            shapeNative.Initialize();
            scalarNative.Initialize();
            scalarNative.FromNative(scalar);
            arrayNative.FromNative(array);
            shapeNative.FromNative(sample);

            // Assert
            array.Value.ShouldBe([Color.RED, Color.RED, Color.RED]);
            sample.color.ShouldBe(Color.RED);
            scalar.Value.ShouldBe(Color.RED);
            sample.inlineColors.Cast<Color>().ShouldBe([Color.RED, Color.RED, Color.RED, Color.RED]);
            sample.colorArray.Value.ShouldBe([Color.RED, Color.RED, Color.RED]);
        }
        finally
        {
            arrayNative.Destroy(optionalsOnly: false);
            shapeNative.Destroy(optionalsOnly: false);
            scalarNative.Destroy(optionalsOnly: false);
        }
    }
}
