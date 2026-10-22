using Wireloom;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class ScalarConstantValidationSpecs
{
    [Fact]
    public void RejectsDefaultAnnotationsWithoutAFollowingDeclaration()
    {
        // Arrange
        var input = Input("dangling-default.idl", "@default(7)");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("following typedef declaration");
    }

    [Theory]
    [InlineData("const boolean Broken = 1;")]
    [InlineData("const char Broken = Missing;")]
    [InlineData("const float Broken = 1e100;")]
    [InlineData("const double Broken = 1.0 / 0.0;")]
    [InlineData("const double Scale = 1.0; const double Broken = :Scale;")]
    [InlineData("const double Broken = ::;")]
    [InlineData("const long Broken = Unknown::;")]
    [InlineData("const double First = Second; const double Second = First;")]
    [InlineData("const long double Broken = 1.0;")]
    [InlineData("const string Broken = \"\\z\";")]
    public void RejectsInvalidUnusedConstantsBeforeEmission(string declaration)
    {
        // Arrange
        var input = Input("invalid-unused-constant.idl", declaration);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldNotBeEmpty();
    }
}
