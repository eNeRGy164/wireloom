using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class IdlAliasResolutionCoverageSpecs
{
    [Theory]
    [InlineData("short", "short")]
    [InlineData("long", "int")]
    [InlineData("long long", "long")]
    [InlineData("unsigned short", "ushort")]
    [InlineData("unsigned long", "uint")]
    [InlineData("unsigned long long", "ulong")]
    [InlineData("int8", "sbyte")]
    [InlineData("uint8", "byte")]
    [InlineData("boolean", "bool")]
    [InlineData("char", "char")]
    [InlineData("wchar", "char")]
    [InlineData("float", "float")]
    [InlineData("double", "double")]
    public void ResolvesIdlPrimitiveAliasesToTheirNativeTypes(string idlType, string expectedNativeType)
    {
        // Arrange
        var resolver = CreateResolver();

        // Act
        var nativeType = resolver.ResolveAliasNativeType(idlType, currentNamespace: "Example");

        // Assert
        nativeType.ShouldBe(expectedNativeType);
    }

    [Theory]
    [InlineData("bool")]
    [InlineData("byte")]
    [InlineData("sbyte")]
    [InlineData("ushort")]
    [InlineData("int")]
    [InlineData("uint")]
    [InlineData("ulong")]
    [InlineData("char")]
    [InlineData("float")]
    [InlineData("double")]
    public void PreservesCSharpPrimitiveSpellings(string csharpType)
    {
        // Arrange
        var resolver = CreateResolver();

        // Act
        var nativeType = resolver.ResolveAliasNativeType(csharpType, currentNamespace: "Example");

        // Assert
        nativeType.ShouldBe(csharpType);
    }

    [Fact]
    public void ResolvesAnEnumAliasInTheCurrentNamespaceToItsQualifiedIdentity()
    {
        // Arrange
        var symbols = new IdlSymbolTable();
        AddEnum(symbols, "Example.Color", "Color", "Example");
        var resolver = new CompilationResolver(symbols);

        // Act
        var nativeType = resolver.ResolveAliasNativeType("Color", currentNamespace: "Example");

        // Assert
        nativeType.ShouldBe("Example.Color");
    }

    [Fact]
    public void ResolvesAQualifiedEnumAliasRelativeToTheCurrentNamespace()
    {
        // Arrange
        var symbols = new IdlSymbolTable();
        AddEnum(symbols, "Example.Shared.Color", "Color", "Example.Shared");
        var resolver = new CompilationResolver(symbols);

        // Act
        var nativeType = resolver.ResolveAliasNativeType("Shared.Color", currentNamespace: "Example");

        // Assert
        nativeType.ShouldBe("Example.Shared.Color");
    }

    [Fact]
    public void UsesTheUnmanagedIdentityForAnAggregateWithoutNamespace()
    {
        // Arrange
        var resolver = CreateResolver();

        // Act
        var nativeType = resolver.ResolveAliasNativeType("Aggregate", currentNamespace: null);

        // Assert
        nativeType.ShouldBe("AggregateUnmanaged");
    }

    [Fact]
    public void UsesTheImplementationIdentityForAQualifiedAggregateAlias()
    {
        // Arrange
        var resolver = CreateResolver();

        // Act
        var nativeType = resolver.ResolveAliasNativeType("Example.Shared.Aggregate", currentNamespace: "Example");

        // Assert
        nativeType.ShouldBe("Example.Shared.Implementation.AggregateUnmanaged");
    }

    private static CompilationResolver CreateResolver() => new(new IdlSymbolTable());

    private static void AddEnum(IdlSymbolTable symbols, string qualifiedName, string name, string @namespace) =>
        symbols.AddEnum(
            qualifiedName,
            new IdlEnum(
                name,
                @namespace,
                [new IdlEnumMember("Value", 0, hasExplicitValue: false, isDefaultLiteral: false)],
                IdlExtensibilityKind.Final));
}
