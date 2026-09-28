using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorLexicalSpecs : IdlPreprocessorTestBase
{
    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void SplicesContinuedLinesBeforeRemovingComments()
    {
        // Arrange
        const string input =
            """
            // comment \
            hidden
            visible
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldNotContain("hidden");
        source.ShouldContain("visible");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void RemovesBackslashNewlineWithoutInsertingWhitespace()
    {
        // Arrange
        const string input =
            """
            const long Value = 1\
            +2;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1+2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void SplicesCrLfContinuedLines()
    {
        // Arrange
        const string sourceWithLf =
            """
            const long Value = 1\
            +2;
            """;
        var input = sourceWithLf.Replace("\n", "\r\n", StringComparison.Ordinal);

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1+2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void SplicesContinuedLinesWithStandaloneCarriageReturns()
    {
        // Arrange
        const string sourceWithLf =
            """
            const long Value = 1\
            +2;
            """;
        var input = sourceWithLf.Replace("\n", "\r", StringComparison.Ordinal);

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1+2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void JoinsIdentifierTokensAcrossContinuedLines()
    {
        // Arrange
        const string input =
            """
            #define FOO 7
            const long Value = FO\
            O;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 7;");
    }

    [Fact]
    [Trait("Preprocessor", "PP040")]
    public void MapsDiagnosticsAfterLineSplicingBackToOriginalOffsets()
    {
        // Arrange
        var input = Input("spliced-warning.idl",
            """
            const long Value = 1 + \
            2;
            #warning after splice
            """);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        diagnostics.Single(diagnostic => diagnostic.Id == "DDSG0106").Offset
            .ShouldBe(input.Text.IndexOf("#warning", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    public void PreservesCommentMarkersInsideStringAndCharacterLiterals()
    {
        // Arrange
        const string input =
            """
            const string Url = "http://example.test/*path*/";
            const char Slash = '/';
            // this comment is removed
            struct Sample { long value; };
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("http://example.test/*path*/");
        source.ShouldContain("const char Slash = '/';");
        source.ShouldContain("struct Sample");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    public void PreservesCommentMarkersInsidePrefixedStringLiterals()
    {
        // Arrange
        const string input =
            """
            const string Url = L"http://example.test/*path*/";
            struct Sample { long value; };
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("L\"http://example.test/*path*/\"");
        source.ShouldContain("struct Sample");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    public void PreservesCommentMarkersInsideEncodingPrefixedStringLiterals()
    {
        // Arrange
        const string input =
            """
            const string Url = u8"http://example.test/*path*/";
            struct Sample { long value; };
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("u8\"http://example.test/*path*/\"");
        source.ShouldContain("struct Sample");
    }

    [Fact]
    [Trait("Preprocessor", "PP001")]
    [Trait("Preprocessor", "PP002")]
    public void RemovesLineAndBlockComments()
    {
        // Arrange
        const string input =
            """
            // line comment
            struct First { long value; }; /* block comment */
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("struct First");
        source.ShouldNotContain("line comment");
        source.ShouldNotContain("block comment");
    }

    [Fact]
    [Trait("Preprocessor", "PP005")]
    public void RejectsUnterminatedBlockComments()
    {
        // Arrange
        var input = Input("unterminated-comment.idl", """/* unterminated""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated comment");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    [Trait("Preprocessor", "PP005")]
    public void MapsUnterminatedCommentOffsetAfterLineSplicing()
    {
        // Arrange
        var input = Input("spliced-comment.idl",
            """
            x\
            /* unterminated
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Offset.ShouldBe(input.Text.IndexOf("/*", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    public void MapsUnterminatedLiteralOffsetAfterLineSplicing()
    {
        // Arrange
        var input = Input("spliced-string.idl",
            """
            const string Value = \
            "unterminated;
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Offset.ShouldBe(input.Text.IndexOf('"'));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsUnterminatedLiteralInMacroSuffixToItsOriginalOffset()
    {
        // Arrange
        var input = Input("macro-suffix-string.idl",
            """
            #define VALUE 1
            VALUE "unterminated
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Offset.ShouldBe(input.Text.IndexOf('"'));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsUnterminatedLiteralAfterGeneratedFunctionNameToItsOriginalOffset()
    {
        // Arrange
        var input = Input("generated-function-string.idl",
            """
            #define ALIAS FUNCTION
            #define FUNCTION(value) value
            ALIAS("unterminated)
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Offset.ShouldBe(input.Text.LastIndexOf('"'));
    }
}
