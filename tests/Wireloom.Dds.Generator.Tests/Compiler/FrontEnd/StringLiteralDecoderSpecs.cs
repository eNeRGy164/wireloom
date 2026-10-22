using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class StringLiteralDecoderSpecs
{
    [Theory]
    [InlineData("")]
    [InlineData("\"unfinished")]
    [InlineData("\"unfinished\\")]
    [InlineData("\"escaped end\\\"")]
    public void RejectsMissingOrUnterminatedLiteralTokens(string expression)
    {
        // Arrange
        var input = new IdlInput("incomplete-string.idl", expression);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlStringLiteralDecoder.Decode(expression, isWide: false, input, offset: 0));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBe(0);
    }
}
