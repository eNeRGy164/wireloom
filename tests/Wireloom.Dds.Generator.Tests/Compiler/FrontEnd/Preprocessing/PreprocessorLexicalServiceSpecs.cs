namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP001")]
[Trait("Preprocessor", "PP002")]
[Trait("Preprocessor", "PP003")]
[Trait("Preprocessor", "PP004")]
[Trait("Preprocessor", "PP005")]
public sealed class PreprocessorLexicalServiceSpecs
{
    [Fact]
    public void RemovesCommentsWhilePreservingLiteralTextAndLineBreaks()
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset, CancellationToken.None);
        var input = new IdlInput("lexical.idl", "value /* comment */ \"not // a comment\"\n// line\nnext");

        // Act
        var result = service.RemoveComments(input, input.Text);

        // Assert
        result.ShouldContain("\"not // a comment\"");
        result.ShouldContain("\n");
        result.ShouldContain("next");
    }

    [Fact]
    public void RecognizesIdentifiersAndPrefixedLiteralsUsingSharedRules()
    {
        // Arrange
        const string source = "u8\"text\" _name";

        // Act
        var literalEnd = PreprocessorLexicalService.SkipLiteral(source, 0);

        // Assert
        PreprocessorLexicalService.IsIdentifier("_name").ShouldBeTrue();
        PreprocessorLexicalService.IsIdentifier("9name").ShouldBeFalse();
        PreprocessorLexicalService.StartsPrefixedLiteral(source, 0).ShouldBeTrue();
        literalEnd.ShouldBe(8);
    }

    [Fact]
    public void ReportsUnterminatedCommentsAtTheirOriginalOffset()
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset + 10, CancellationToken.None);
        var input = new IdlInput("lexical.idl", "prefix /* unfinished");

        // Act
        var exception = Should.Throw<IdlException>(() => service.RemoveComments(input, input.Text));

        // Assert
        exception.Message.ShouldContain("Unterminated comment");
        exception.Offset.ShouldBe(17);
    }
}
