using Wireloom;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class StringConstantBindingSpecs
{
    [Theory]
    [InlineData("typedef string<2> Small; const Small Text = \"too long\";")]
    [InlineData("typedef string<2> Small; const Small Text = \"€\";")]
    [InlineData("typedef wstring<2> Small; const Small Text = L\"abc\";")]
    [InlineData("const long Size = 2; typedef string<Size> Small; const Small Text = \"abc\";")]
    [InlineData("const string Original = \"abc\"; typedef string<2> Small; const Small Text = Original;")]
    public void RejectsConstantsThatExceedTheirResolvedStringBound(string declaration)
    {
        // Arrange
        var input = Input("bounded-string-constant.idl", declaration);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("bound of 2");
    }

    [Fact]
    public void DecodesIdlStringConstantEscapesBeforeEmissionAndBoundChecks()
    {
        // Arrange
        var input = Input("string-constant-escape.idl", """
            typedef string<1> Small;
            const Small Text = "\?";
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Text.g.cs"].Source.ShouldContain("public const string Value = \"?\";");
    }
}
