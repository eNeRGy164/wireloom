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

    [Fact]
    public void JoinsLfAndCrLfContinuationsAndMapsTheEndOffset()
    {
        // Arrange
        const string source = "a\\\nb\\\r\nc\\x";

        // Act
        var joined = PreprocessorLexicalService.JoinContinuations(source, out var offsets);

        // Assert
        joined.ShouldBe("abc\\x");
        offsets.ShouldBe([0, 3, 7, 8, 9, source.Length]);
    }

    [Fact]
    public void JoinsCrOnlyContinuationAndPreservesATerminalBackslash()
    {
        // Arrange
        const string source = "a\\\rb\\";

        // Act
        var joined = PreprocessorLexicalService.JoinContinuations(source, out var offsets);

        // Assert
        joined.ShouldBe("ab\\");
        offsets.ShouldBe([0, 3, 4, source.Length]);
    }

    [Fact]
    public void JoinsBackslashCrAtEndOfInputAndMapsTheEof()
    {
        // Arrange
        const string source = "tail\\\r";

        // Act
        var joined = PreprocessorLexicalService.JoinContinuations(source, out var offsets);

        // Assert
        joined.ShouldBe("tail");
        offsets.ShouldBe([0, 1, 2, 3, source.Length]);
    }

    [Theory]
    [InlineData("L'c'", 1, true)]
    [InlineData("u'c'", 1, true)]
    [InlineData("u8'c'", 2, true)]
    [InlineData("x8'c'", 2, false)]
    [InlineData("can't", 3, false)]
    [InlineData("'c'", 0, true)]
    [InlineData("x'", 1, true)]
    public void DistinguishesEncodingPrefixesFromApostrophes(string text, int index, bool expected)
    {
        // Arrange

        // Act
        var isLiteralStart = PreprocessorLexicalService.IsLiteralStart(text, index);

        // Assert
        isLiteralStart.ShouldBe(expected);
    }

    [Theory]
    [InlineData("L'c'", 0, true)]
    [InlineData("u8\"text\"", 0, true)]
    [InlineData("U\"text\"", 0, true)]
    [InlineData("u8x\"text\"", 0, false)]
    [InlineData("u8", 0, false)]
    public void RecognizesOnlySupportedPrefixedLiteralForms(string text, int index, bool expected)
    {
        // Arrange

        // Act
        var isPrefixedLiteral = PreprocessorLexicalService.StartsPrefixedLiteral(text, index);

        // Assert
        isPrefixedLiteral.ShouldBe(expected);
    }

    [Theory]
    [InlineData("L\"text\"", 0, 1)]
    [InlineData("u\"text\"", 0, 1)]
    [InlineData("U\"text\"", 0, 1)]
    [InlineData("u8\"text\"", 0, 2)]
    [InlineData("\"text\"", 0, 0)]
    public void FindsTheQuoteIndexForEachLiteralPrefix(string text, int index, int expectedQuoteIndex)
    {
        // Arrange

        // Act
        var quoteIndex = PreprocessorLexicalService.LiteralQuoteIndex(text, index);

        // Assert
        quoteIndex.ShouldBe(expectedQuoteIndex);
        text[quoteIndex].ShouldBe('"');
    }

    [Fact]
    public void SkipsEscapedQuotesInPrefixedLiterals()
    {
        // Arrange
        const string text = "u8\"a\\\"b\"tail";

        // Act
        var literalEnd = PreprocessorLexicalService.SkipLiteral(text, 0);

        // Assert
        literalEnd.ShouldBe(8);
        text[literalEnd..].ShouldBe("tail");
    }

    [Theory]
    [InlineData("L\"text\"tail", 7)]
    [InlineData("U\"text\"tail", 7)]
    [InlineData("u8\"text\"tail", 8)]
    public void FindsLiteralEndForEachEncodingPrefix(string text, int expectedEnd)
    {
        // Arrange

        // Act
        var literalEnd = PreprocessorLexicalService.SkipLiteral(text, 0);

        // Assert
        literalEnd.ShouldBe(expectedEnd);
        text[literalEnd..].ShouldBe("tail");
    }

    [Fact]
    public void StopsAtEndWhenAnUnterminatedLiteralEndsWithAnEscape()
    {
        // Arrange
        const string text = "\"unfinished\\";

        // Act
        var literalEnd = PreprocessorLexicalService.SkipLiteral(text, 0);

        // Assert
        literalEnd.ShouldBe(text.Length);
    }

    [Fact]
    public void PreservesCommentWidthWhenBlockCommentEndsAtEndOfInput()
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset, CancellationToken.None);
        const string source = "value/**/";

        // Act
        var result = service.RemoveComments(new IdlInput("lexical.idl", source), source);

        // Assert
        result.ShouldBe("value    ");
        result.Length.ShouldBe(source.Length);
    }

    [Fact]
    public void PreservesTrailingSlashThatCannotStartAComment()
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset, CancellationToken.None);
        const string source = "value/";

        // Act
        var result = service.RemoveComments(new IdlInput("lexical.idl", source), source);

        // Assert
        result.ShouldBe(source);
    }

    [Fact]
    public void RejectsBlockCommentEndingWithAnUnmatchedAsterisk()
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset + 5, CancellationToken.None);
        const string source = "/* unfinished *";

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.RemoveComments(new IdlInput("lexical.idl", source), source));

        // Assert
        exception.Message.ShouldContain("Unterminated comment");
        exception.Offset.ShouldBe(5);
    }

    [Theory]
    [InlineData("/*")]
    [InlineData("/*/")]
    [InlineData("/*x*")]
    public void RejectsBlockCommentsWithoutAClosingDelimiter(string source)
    {
        // Arrange
        var service = new PreprocessorLexicalService(offset => offset + 7, CancellationToken.None);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.RemoveComments(new IdlInput("lexical.idl", source), source));

        // Assert
        exception.Message.ShouldContain("Unterminated comment");
        exception.Offset.ShouldBe(7);
    }
}
