using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorConditionalSpecs : IdlPreprocessorTestBase
{
    [Theory]
    [InlineData("UNDEFINED_NAME", false)]
    [InlineData("1 + 1 == 3", false)]
    [InlineData("1 + 1 == 0", false)]
    [InlineData("2 + 3 * 4 == 14", true)]
    [InlineData("2 + 3 * 4 == 15", false)]
    [InlineData("1 + 2 << 2 == 12", true)]
    [InlineData("1 + 2 << 2 == 16", false)]
    [InlineData("8 >> 1 < 5", true)]
    [InlineData("8 >> 1 < 3", false)]
    [InlineData("1 != 2", true)]
    [InlineData("2 <= 2", true)]
    [InlineData("2 >= 2", true)]
    [InlineData("2 > 1", true)]
    [InlineData("(6 & 3) == 2", true)]
    [InlineData("(6 & 3) == 3", false)]
    [InlineData("(4 | 1) == 5", true)]
    [InlineData("(4 | 1) == 4", false)]
    [InlineData("(7 ^ 3) == 4", true)]
    [InlineData("(7 ^ 3) == 5", false)]
    [InlineData("20 / 5 * 2 == 8", true)]
    [InlineData("20 % 6 == 2", true)]
    [InlineData("10 - 3 - 2 == 5", true)]
    [InlineData("-2 * -3 == 6", true)]
    [InlineData("+5 == 5", true)]
    [InlineData("~0 == -1", true)]
    [InlineData("0x10 + 0x0f == 31", true)]
    [InlineData("'A' == 65", true)]
    [InlineData("'A' == 66", false)]
    [InlineData("'\\n' == 10", true)]
    [InlineData("'\\n' == 13", false)]
    [InlineData("'\\101' == 65", true)]
    [InlineData("'\\x41' == 65", true)]
    [InlineData("010 == 8", true)]
    [InlineData("010 == 10", false)]
    [InlineData("0b1010u == 10", true)]
    [InlineData("0b1010u == 11", false)]
    [InlineData("0x10UL == 16", true)]
    [InlineData("0x10UL == 17", false)]
    [InlineData("0xffffffffffffffffULL == -1", true)]
    [InlineData("18446744073709551615ULL == -1", true)]
    [InlineData("1'000'000 == 1000000", true)]
    [InlineData("0xFFFF'FFFF == 4294967295U", true)]
    [InlineData("0b1111111111111111111111111111111111111111111111111111111111111111ULL == -1", true)]
    [InlineData("-1 < 1ULL", false)]
    [InlineData("1ULL > -1", false)]
    [InlineData("-1 == 0xffffffffffffffffULL", true)]
    [InlineData("3 < 4 == 1", true)]
    [InlineData("1 || 0 && 0", true)]
    [InlineData("0 && 1 || 1", true)]
    [InlineData("!!7", true)]
    [InlineData("(1 + (2 * (3 + 1))) == 9", true)]
    [InlineData("1 || (10 / 0)", true)]
    [InlineData("0 && (10 % 0)", false)]
    [InlineData("1 ? 2 : 3", true)]
    [InlineData("0 ? 2 : 3", true)]
    [InlineData("0 ? 1 : 2", true)]
    [InlineData("1 ? 0 : 3", false)]
    [InlineData("0 ? 2 : 0", false)]
    [InlineData("1 ? 2 : (10 / 0)", true)]
    [InlineData("0 ? (10 / 0) : 3", true)]
    [InlineData("1 ? 0 : (10 / 0)", false)]
    [InlineData("0 ? (10 / 0) : 0", false)]
    [Trait("Preprocessor", "PP024")]
    [Trait("Preprocessor", "PP025")]
    [Trait("Preprocessor", "PP026")]
    [Trait("Preprocessor", "PP027")]
    [Trait("Preprocessor", "PP028")]
    public void EvaluatesConditionalIntegerExpressionsWithCPrecedence(string expression, bool expected)
    {
        // Arrange
        var input = Input("expression.idl",
            $$"""
            #if {{expression}}
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.Contains("selected", StringComparison.Ordinal).ShouldBe(expected);
        source.Contains("rejected", StringComparison.Ordinal).ShouldBe(!expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1 +")]
    [InlineData("* 2")]
    [InlineData("(1 + 2")]
    [InlineData("1 + 2)")]
    [InlineData("1 2")]
    [InlineData("12invalid")]
    [InlineData("4 / 0")]
    [InlineData("4 % 0")]
    [InlineData("1 & & 1")]
    [InlineData("0xnothex")]
    [InlineData("1ULL != 0xnothex")]
    [InlineData("-0xnothex < 1ULL")]
    [InlineData("0b")]
    [InlineData("0ba")]
    [InlineData("0bA")]
    [InlineData("0bX")]
    [InlineData("0b2")]
    [InlineData("0b11111111111111111111111111111111111111111111111111111111111111111")]
    [InlineData("0b1111111111111111111111111111111111111111111111111111111111111111")]
    [InlineData("1''000")]
    [InlineData("1'U")]
    [InlineData("0x'1")]
    [InlineData("0'x1")]
    [InlineData("1UU")]
    [InlineData("1LLL")]
    [InlineData("1ULUL")]
    [InlineData("defined")]
    [InlineData("defined(1)")]
    [InlineData("defined()")]
    [InlineData("defined ( FEATURE")]
    [InlineData("1 ? 2")]
    [InlineData("\"text\"")]
    [InlineData("'ab'")]
    [InlineData("'\\x'")]
    [InlineData("'\\q'")]
    [InlineData("1 << -1")]
    [InlineData("1 >> 64")]
    [InlineData("1++2")]
    [InlineData("--1")]
    [InlineData("(-9223372036854775807 - 1) / -1")]
    [Trait("Preprocessor", "PP024")]
    public void RejectsMalformedOrInvalidConditionalArithmetic(string expression)
    {
        // Arrange
        var input = Input("invalid-expression.idl",
            $$"""
            #if {{expression}}
            selected
            #endif
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldStartWith("Invalid preprocessor condition:");
        exception.Offset.ShouldBe(0);
        exception.Input.ShouldBeSameAs(input);
    }

    [Fact]
    [Trait("Preprocessor", "PP024")]
    public void RejectsExcessiveParenthesizedExpressionNesting()
    {
        // Arrange
        var expression = new string('(', 300) + "1" + new string(')', 300);
        var input = Input("deep-parentheses.idl", $$"""
            #if {{expression}}
            selected
            #endif
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("expression nesting exceeds");
        exception.Input.ShouldBeSameAs(input);
    }

    [Fact]
    [Trait("Preprocessor", "PP024")]
    public void RejectsExcessiveUnaryExpressionNesting()
    {
        // Arrange
        var expression = new string('!', 300) + "1";
        var input = Input("deep-unary.idl", $$"""
            #if {{expression}}
            selected
            #endif
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("expression nesting exceeds");
        exception.Input.ShouldBeSameAs(input);
    }

    [Fact]
    [Trait("Preprocessor", "PP024")]
    public void RejectsExcessiveConditionalExpressionNesting()
    {
        // Arrange
        var expression = string.Join(" ? 1 : ", Enumerable.Repeat("1", 301));
        var input = Input("deep-conditional.idl", $$"""
            #if {{expression}}
            selected
            #endif
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("expression nesting exceeds");
        exception.Input.ShouldBeSameAs(input);
    }

    [Fact]
    [Trait("Preprocessor", "PP028")]
    public void DoesNotEvaluateInvalidArithmeticInInactiveConditionalGroups()
    {
        // Arrange
        var input =
            """
            #if 0
            #if 1 / 0
            wrong
            #endif
            #endif
            valid
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("valid");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    public void DoesNotTreatDefinedAsAKeywordPrefix()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor(["ness"], []);
        var input = Input("defined-prefix.idl",
            """
            #if definedness
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldNotContain("selected");
        source.ShouldContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    public void DoesNotTreatDefinedAsAnOperandAsAnotherOperator()
    {
        // Arrange
        var input = Input("defined-operand.idl",
            """
            #if defined(defined)
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.ShouldNotContain("selected");
        source.ShouldContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    [Trait("Preprocessor", "PP026")]
    public void RejectsDefinedTextInsideACharacterLiteral()
    {
        // Arrange
        var input = Input("defined-character-literal.idl",
            """
            #if 'defined'
            selected
            #endif
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldStartWith("Invalid preprocessor condition:");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    public void AcceptsWhitespaceInBothFormsOfDefined()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor(["FEATURE"], []);
        var input = Input("defined-whitespace.idl",
            """
            #if defined   (   FEATURE   ) && defined FEATURE
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Theory]
    [InlineData("1ULL != 2", true)]
    [InlineData("2ULL <= 2", true)]
    [InlineData("2ULL >= 2", true)]
    [InlineData("2ULL > 1", true)]
    [InlineData("2ULL > 2", false)]
    [InlineData("+1ULL == 1", true)]
    [Trait("Preprocessor", "PP027")]
    public void EvaluatesAllUnsignedComparisonOperators(string expression, bool expected)
    {
        // Arrange
        var input = Input("unsigned-comparison.idl",
            $$"""
            #if {{expression}}
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.Contains("selected", StringComparison.Ordinal).ShouldBe(expected);
        source.Contains("rejected", StringComparison.Ordinal).ShouldBe(!expected);
    }

    [Theory]
    [InlineData("1 + +2 == 3")]
    [InlineData("1 - -2 == 3")]
    [Trait("Preprocessor", "PP024")]
    public void AllowsWhitespaceSeparatedUnarySigns(string expression)
    {
        // Arrange
        var input = Input("separated-signs.idl",
            $$"""
            #if {{expression}}
            selected
            #else
            rejected
            #endif
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    public void EvaluatesEncodingPrefixedCharacterConstants()
    {
        // Arrange
        const string input =
            """
            #if L'a' == 97
            selected
            #else
            rejected
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    public void EvaluatesTheU8CharacterEncodingPrefix()
    {
        // Arrange
        const string input =
            """
            #if u8'a' == 97
            selected
            #else
            rejected
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP026")]
    public void EvaluatesTheSupportedSimpleCharacterEscapes()
    {
        // Arrange
        const string input =
            """
            #if '\\' == 92 && '\'' == 39 && '"' == 34 && '\a' == 7 && '\b' == 8 && '\f' == 12 && '\r' == 13 && '\t' == 9 && '\v' == 11
            selected
            #else
            rejected
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP026")]
    public void EvaluatesAnEscapedDoubleQuoteCharacter()
    {
        // Arrange
        const string input =
            """
            #if '\"' == 34
            selected
            #else
            rejected
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }

    [Fact]
    [Trait("Preprocessor", "PP026")]
    public void EvaluatesOctalAndUnboundedHexCharacterEscapes()
    {
        // Arrange
        const string input =
            """
            #if '\101' == 65 && '\x4142' == 0x4142
            selected
            #else
            rejected
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("rejected");
    }
}
