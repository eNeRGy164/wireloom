using Wireloom.Compiler.Emission;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionSupportSpecs
{
    [Fact]
    public void UsesSimpleNameForUnmanagedTypeInTheCurrentImplementationNamespace()
    {
        // Arrange
        const string typeName = "Example.Payload";
        const string currentNamespace = "Example.Implementation";

        // Act
        var unmanagedType = EmissionSupport.GetUnmanagedType(typeName, currentNamespace);

        // Assert
        unmanagedType.ShouldBe("PayloadUnmanaged");
    }

    [Fact]
    public void KeepsExternalUnmanagedTypesFullyQualified()
    {
        // Arrange
        const string typeName = "Other.Payload";
        const string currentNamespace = "Example.Implementation";

        // Act
        var unmanagedType = EmissionSupport.GetUnmanagedType(typeName, currentNamespace);

        // Assert
        unmanagedType.ShouldBe("global::Other.Implementation.PayloadUnmanaged");
    }
}
