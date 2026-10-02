using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Symbols.Tests;

public sealed class IdlSymbolTableSpecs
{
    [Theory]
    [InlineData("Example.Sample", "Example.Sample")]
    [InlineData("Example.Sample", "Example.Sample.Nested")]
    [InlineData("Example.Sample.Nested", "Example.Sample")]
    public void RejectsExactAndNamespacePrefixIdentityCollisions(string first, string second)
    {
        // Arrange
        var symbols = new IdlSymbolTable();

        // Act
        var firstAdded = symbols.AddGeneratedIdentity(first);
        var secondAdded = symbols.AddGeneratedIdentity(second);

        // Assert
        firstAdded.ShouldBeTrue();
        secondAdded.ShouldBeFalse();
    }

    [Fact]
    public void PreservesGeneratedIdentitySetWhenACompoundRegistrationCollides()
    {
        // Arrange
        var symbols = new IdlSymbolTable();
        symbols.AddGeneratedIdentity("Example.Existing").ShouldBeTrue();

        // Act
        var added = symbols.AddGeneratedIdentities(["Example.New", "Example"]);

        // Assert
        added.ShouldBeFalse();
        symbols.AddGeneratedIdentity("Example.New").ShouldBeTrue();
    }

    [Fact]
    public void AcceptsManagedSupportPluginAndUnmanagedIdentitiesTogether()
    {
        // Arrange
        var symbols = new IdlSymbolTable();

        // Act
        var added = symbols.AddGeneratedIdentities(
        [
            "Example.Sample",
            "Example.SampleSupport",
            "Example.Implementation.SamplePlugin",
            "Example.Implementation.SampleUnmanaged"
        ]);

        // Assert
        added.ShouldBeTrue();
        symbols.AddGeneratedIdentity("Other.Sample").ShouldBeTrue();
        symbols.AddGeneratedIdentity("Example.Implementation.SamplePlugin").ShouldBeFalse();
    }
}
