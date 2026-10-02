using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class IdlAliasResolutionCoverageSpecs
{
    [Fact]
    public void ResolvesStringTypedefsToTheirUnderlyingIdlType()
    {
        // Arrange
        var symbols = new IdlSymbolTable();
        symbols.AddTypedef("Example.Text", new IdlTypedef("Text", "Example", "string", null, null));
        var resolver = new CompilationResolver(symbols);

        // Act
        var underlyingType = resolver.ResolveUnderlyingType("Text", "Example");

        // Assert
        underlyingType.ShouldBe("string");
    }

    [Fact]
    public void StopsResolvingWhenATypedefCycleIsDetected()
    {
        // Arrange
        var symbols = new IdlSymbolTable();
        symbols.AddTypedef("Example.First", new IdlTypedef("First", "Example", "Second", null, null));
        symbols.AddTypedef("Example.Second", new IdlTypedef("Second", "Example", "First", null, null));
        var resolver = new CompilationResolver(symbols);

        // Act
        var underlyingType = resolver.ResolveUnderlyingType("First", "Example");

        // Assert
        underlyingType.ShouldBe("Example.First");
    }

}
