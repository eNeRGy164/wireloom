using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorDiagnosticSpecs : IdlPreprocessorTestBase
{
    [Theory]
    [InlineData("A_VERY_LONG_REPLACEMENT")]
    [InlineData("x")]
    [Trait("Preprocessor", "PP050")]
    public void KeepsFollowingParserErrorsAtTheirOriginalOffset(string replacement)
    {
        // Arrange
        var input = Input("origin.idl",
            $$"""
            #define VALUE {{replacement}}
            VALUE
            !
            """);

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var bang = result.Text.LastIndexOf('!');
        var origin = result.SourceOrigins.Single(span =>
            bang >= span.OutputStart && bang < span.OutputStart + span.OutputLength);

        origin.Map(bang).ShouldBe(input.Text.LastIndexOf('!'));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void KeepsGeneratedTokensAtTheirMacroInvocationOriginWhenTheyMatchLaterText()
    {
        // Arrange
        var input = Input("matching-origin.idl",
            "#define VALUE long\nVALUE long !;");

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var firstLong = result.Text.IndexOf("long", StringComparison.Ordinal);
        var secondLong = result.Text.IndexOf("long", firstLong + 1, StringComparison.Ordinal);
        var firstOrigin = result.SourceOrigins.Single(span =>
            firstLong >= span.OutputStart && firstLong < span.OutputStart + span.OutputLength);
        var secondOrigin = result.SourceOrigins.Single(span =>
            secondLong >= span.OutputStart && secondLong < span.OutputStart + span.OutputLength);

        firstOrigin.Map(firstLong).ShouldBe(input.Text.LastIndexOf("VALUE", StringComparison.Ordinal));
        secondOrigin.Map(secondLong).ShouldBe(input.Text.LastIndexOf("long", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void KeepsAllTokensInAReplacementAtTheirMacroInvocationOrigin()
    {
        // Arrange
        var input = Input("matching-replacement-origin.idl",
            "#define VALUE long value\nVALUE value !;");

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var firstValue = result.Text.IndexOf("value", StringComparison.Ordinal);
        var secondValue = result.Text.IndexOf("value", firstValue + 1, StringComparison.Ordinal);
        var firstOrigin = result.SourceOrigins.Single(span =>
            firstValue >= span.OutputStart && firstValue < span.OutputStart + span.OutputLength);
        var secondOrigin = result.SourceOrigins.Single(span =>
            secondValue >= span.OutputStart && secondValue < span.OutputStart + span.OutputLength);

        firstOrigin.Map(firstValue).ShouldBe(input.Text.LastIndexOf("VALUE", StringComparison.Ordinal));
        secondOrigin.Map(secondValue).ShouldBe(input.Text.LastIndexOf("value", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void PrefersTheUnchangedSuffixWhenReplacementTokensTieWithIt()
    {
        // Arrange
        var input = Input("tied-origin.idl",
            """
            #define VALUE long value
            VALUE value
            """);

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var firstValue = result.Text.IndexOf("value", StringComparison.Ordinal);
        var secondValue = result.Text.IndexOf("value", firstValue + 1, StringComparison.Ordinal);
        var firstOrigin = result.SourceOrigins.Single(span =>
            firstValue >= span.OutputStart && firstValue < span.OutputStart + span.OutputLength);
        var secondOrigin = result.SourceOrigins.Single(span =>
            secondValue >= span.OutputStart && secondValue < span.OutputStart + span.OutputLength);

        firstOrigin.Map(firstValue).ShouldBe(input.Text.LastIndexOf("VALUE", StringComparison.Ordinal));
        secondOrigin.Map(secondValue).ShouldBe(input.Text.LastIndexOf("value", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void KeepsOriginAlignmentBoundedForRepeatedSuffixTokens()
    {
        // Arrange
        var repeatedTokens = string.Join(" ", Enumerable.Repeat("X", 10_000));
        var input = Input("repeated-origin.idl",
            $$"""
            #define MACRO long
            MACRO {{repeatedTokens}} !
            """);

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var lastOutputToken = result.Text.LastIndexOf("X", StringComparison.Ordinal);
        var lastOutputOrigin = result.SourceOrigins.Single(span =>
            lastOutputToken >= span.OutputStart && lastOutputToken < span.OutputStart + span.OutputLength);

        lastOutputOrigin.Map(lastOutputToken).ShouldBe(input.Text.LastIndexOf("X", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void KeepsFollowingSourceOriginsAfterMoreThanSixtyFourGeneratedTokens()
    {
        // Arrange
        var generatedTokens = string.Join(" ", Enumerable.Range(0, 80).Select(index => $"generated{index}"));
        var input = Input("long-origin.idl", $"#define MANY {generatedTokens}\nMANY Unknown");

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var unknown = result.Text.LastIndexOf("Unknown", StringComparison.Ordinal);
        var origin = result.SourceOrigins.Single(span =>
            unknown >= span.OutputStart && unknown < span.OutputStart + span.OutputLength);
        var generated = result.Text.IndexOf("generated0", StringComparison.Ordinal);
        var generatedOrigin = result.SourceOrigins.Single(span =>
            generated >= span.OutputStart && generated < span.OutputStart + span.OutputLength);

        generatedOrigin.Map(generated).ShouldBe(input.Text.LastIndexOf("MANY", StringComparison.Ordinal));
        origin.Map(unknown).ShouldBe(input.Text.LastIndexOf("Unknown", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void ProvidesAnExplicitEndOfFileOrigin()
    {
        // Arrange
        var input = Input("eof.idl", """VALUE""");

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        var eof = result.SourceOrigins[^1];

        eof.OutputStart.ShouldBe(result.Text.Length);
        eof.SourceStart.ShouldBe(input.Text.Length);
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void ReportsActivePragmaMessagesButIgnoresInactiveMessages()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = Input("pragma-message.idl",
            """
            #pragma message("visible")
            #if 0
            #pragma message("hidden")
            #endif
            """);

        // Act
        new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        diagnostics.ShouldContain(d => d.Id == "DDSG0107");
        diagnostics.ShouldContain(d => d.Message.Contains("visible", StringComparison.Ordinal));
        diagnostics.ShouldNotContain(d => d.Id == "DDSG0106");
        diagnostics.ShouldNotContain(d => d.Message.Contains("hidden", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP006")]
    public void RejectsMalformedDefineDirectives()
    {
        // Arrange
        var input = Input("bad-define.idl", """#define 1""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Malformed #define");
    }

    [Theory]
    [InlineData("#define BROKEN(value", "Malformed #define parameter list")]
    [InlineData("#define BROKEN(value, 1) value", "Malformed #define parameter list")]
    [InlineData("#define BROKEN(__VA_ARGS__, ...) __VA_ARGS__", "Duplicate macro parameter")]
    [Trait("Preprocessor", "PP006")]
    public void RejectsMalformedFunctionMacroDefinitions(string directive, string expectedMessage)
    {
        // Arrange
        var input = Input("bad-function-define.idl", directive);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Fact]
    [Trait("Preprocessor", "PP038")]
    public void RejectsMalformedUndefDirectives()
    {
        // Arrange
        var input = Input("bad-undef.idl", """#undef 1""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Malformed #undef");
    }

    [Theory]
    [InlineData("#ifdef", "#ifdef")]
    [InlineData("#ifndef 123", "#ifndef")]
    [InlineData("#ifdef FEATURE extra", "#ifdef")]
    [Trait("Preprocessor", "PP021")]
    public void RejectsMalformedConditionalMacroOperands(string directive, string name)
    {
        // Arrange
        var input = Input("bad-conditional-operand.idl", directive);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain($"Malformed {name}");
    }

    [Theory]
    [InlineData("#line")]
    [InlineData("#line nope")]
    [InlineData("#line 2147483648")]
    [Trait("Preprocessor", "PP041")]
    public void RejectsMalformedLineDirectives(string directive)
    {
        // Arrange
        var input = Input("bad-line.idl", directive);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Malformed #line");
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void RejectsUnexpectedElseDirectives()
    {
        // Arrange
        var input = Input("bad-else.idl", """#else""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unexpected #else");
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void RejectsUnexpectedElifDirectives()
    {
        // Arrange
        var input = Input("bad-elif.idl", "#elif 1");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unexpected #elif");
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void RejectsUnexpectedEndifDirectives()
    {
        // Arrange
        var input = Input("bad-endif.idl", """#endif""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unexpected #endif");
    }

    [Fact]
    [Trait("Preprocessor", "PP039")]
    public void RejectsErrorDirectives()
    {
        // Arrange
        var input = Input("bad-error.idl", """#error stop""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Preprocessor error");
    }

    [Fact]
    [Trait("Preprocessor", "PP040")]
    public void ReportsActiveWarningDirectivesWithoutStoppingPreprocessing()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = Input("warning.idl",
            """
            #warning use the supported IDL form
            struct Sample { long value; };
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        source.ShouldContain("struct Sample");
        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0106");
        diagnostics.ShouldContain(diagnostic => diagnostic.Message.Contains("supported IDL form", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP040")]
    public void IgnoresWarningsInInactiveConditionalBranches()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = Input("inactive-warning.idl",
            """
            #if 0
            #warning ignored
            #endif
            """);

        // Act
        new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    [Trait("Preprocessor", "PP039")]
    public void IgnoresErrorsAndUnsupportedDirectivesInInactiveBranches()
    {
        // Arrange
        var input = Input("inactive-directives.idl",
            """
            #if 0
            #error ignored
            #unknown ignored
            #endif
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.Trim().ShouldBeEmpty();
    }

    [Fact]
    [Trait("Preprocessor", "PP039")]
    public void RejectsUnsupportedActiveDirectives()
    {
        // Arrange
        var input = Input("unsupported-directive.idl", "#unknown directive");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unsupported preprocessor directive: #unknown");
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void RejectsUnterminatedPreprocessorConditionals()
    {
        // Arrange
        var input = Input("unterminated.idl",
            """
            #if 1
            value
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated preprocessor conditional");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    public void RejectsUnterminatedStringLiterals()
    {
        // Arrange
        var input = Input("unterminated-string.idl", """const string Value = "value;""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    public void RejectsUnterminatedLiteralInAnUnusedMacroBody()
    {
        // Arrange
        var input = Input("unterminated-macro-literal.idl", """#define UNUSED "unterminated""");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
    }
}
