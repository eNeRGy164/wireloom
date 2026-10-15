using RuntimeManaged;
using RuntimeManaged.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks that RTI native conversion and generated support can load.</summary>
public sealed class RuntimeSmokeSpecs
{
    /// <summary>Converts ANative String And Loads Type Support.</summary>
    [Fact]
    public void ConvertsANativeStringAndLoadsTypeSupport()
    {
        // Arrange
        var source = new Child { id = 17, text = "native smoke" };
        var native = new ChildUnmanaged();

        try
        {
            // Act
            native.Initialize();
            native.ToNative(source);
            var destination = new Child();
            native.FromNative(destination);

            // Assert
            destination.id.ShouldBe(17);
            destination.text.ShouldBe("native smoke");
            ChildSupport.Instance.ShouldNotBeNull();
            ChildPlugin.CreateDynamicType(isPublic: true).ShouldNotBeNull();
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }
}
