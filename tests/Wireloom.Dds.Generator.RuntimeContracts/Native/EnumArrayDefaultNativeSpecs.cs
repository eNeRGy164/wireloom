using RuntimeEnumArrayDefaults;
using RuntimeEnumArrayDefaults.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks native array initialization for enums with a nonzero default literal.</summary>
public sealed class EnumArrayDefaultNativeSpecs
{
    [Fact]
    public void NativeEnumArraysStartAtTheDeclaredDefaultLiteral()
    {
        // Arrange
        var arrayNative = new ColorArrayUnmanaged();
        var shapeNative = new ShapeUnmanaged();
        var array = new ColorArray();
        var sample = new Shape();

        try
        {
            // Act
            arrayNative.Initialize();
            shapeNative.Initialize();
            arrayNative.FromNative(array);
            shapeNative.FromNative(sample);

            // Assert
            array.Value.ShouldBe([Color.RED, Color.RED, Color.RED]);
            sample.color.ShouldBe(Color.RED);
            sample.inlineColors.Cast<Color>().ShouldBe([Color.RED, Color.RED, Color.RED, Color.RED]);
            sample.colorArray.Value.ShouldBe([Color.RED, Color.RED, Color.RED]);
        }
        finally
        {
            arrayNative.Destroy(optionalsOnly: false);
            shapeNative.Destroy(optionalsOnly: false);
        }
    }
}
