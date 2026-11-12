
namespace Wireloom.Dds.CSharp.Compiler.FrontEnd.Semantic;

public sealed class IdlAliasResolutionCoverageSpecs
{
    [Fact]
    public void ResolvesConstantsFromTheNearestEnclosingNamespace()
    {
        // Arrange
        var input = new IdlInput("constants.idl", string.Empty, generate: false);
        var symbols = new IdlSymbolTable();
        symbols.AddConstant("Outer.Value", Constant("Value", "Outer", input));
        var resolver = new CompilationResolver(symbols);

        // Act
        var resolved = resolver.TryResolveConstant("Value", "Outer.Inner", out var qualifiedName);

        // Assert
        resolved.ShouldBeTrue();
        qualifiedName.ShouldBe("Outer.Value");
    }

    [Fact]
    public void ResolvesAbsoluteConstantNamesFromTheGlobalNamespace()
    {
        // Arrange
        var input = new IdlInput("constants.idl", string.Empty, generate: false);
        var symbols = new IdlSymbolTable();
        symbols.AddConstant("Value", Constant("Value", null, input));
        symbols.AddConstant("Outer.Value", Constant("Value", "Outer", input));
        var resolver = new CompilationResolver(symbols);

        // Act
        var resolved = resolver.TryResolveConstant("::Value", "Outer", out var qualifiedName);

        // Assert
        resolved.ShouldBeTrue();
        qualifiedName.ShouldBe("Value");
    }

    [Fact]
    public void ReturnsTheNormalizedNameWhenAConstantCannotBeResolved()
    {
        // Arrange
        var resolver = new CompilationResolver(new IdlSymbolTable());

        // Act
        var resolved = resolver.TryResolveConstant("::Missing::Value", null, out var qualifiedName);

        // Assert
        resolved.ShouldBeFalse();
        qualifiedName.ShouldBe("Missing.Value");
    }

    [Fact]
    public void FallsBackToTheUnqualifiedIdlNameForAnUnresolvedType()
    {
        // Arrange
        var resolver = new CompilationResolver(new IdlSymbolTable());

        // Act
        var underlyingType = resolver.ResolveUnderlyingType("Unknown", "Outer");

        // Assert
        underlyingType.ShouldBe("Unknown");
    }

    [Fact]
    public void ResolvesEnumsFromTheImplementationNamespaceWhenNoIdlNamespaceIsActive()
    {
        // Arrange
        var resolver = new CompilationResolver(new IdlSymbolTable());
        var enumType = new IdlType.Enum("Implementation.Color", 0, "Red");

        // Act
        var nativeType = resolver.ResolveNativeType(enumType, currentNamespace: null);

        // Assert
        nativeType.ShouldBe("Color");
    }

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

    private static IdlConstantDeclaration Constant(string name, string? currentNamespace, IdlInput input) =>
        new(name, "long", "1", currentNamespace, input.Path, new IdlScalarValue.Integral(1), input, 0);

}
