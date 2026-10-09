namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

/// <summary>Verifies the conditional-expression parser's character-literal boundaries.</summary>
[Trait("Preprocessor", "PP024")]
[Trait("Preprocessor", "PP025")]
[Trait("Preprocessor", "PP026")]
[Trait("Preprocessor", "PP027")]
[Trait("Preprocessor", "PP028")]
[Trait("Preprocessor", "PP029")]
[Trait("Preprocessor", "PP030")]
[Trait("Preprocessor", "PP051")]
public sealed class PreprocessorConditionalExpressionParserSpecs
{
    [Theory]
    [InlineData("")]
    [InlineData(" \t ")]
    public void ReportsAnEmptyExpressionAtItsEntryPoint(string expression)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain("expected an expression");
    }

    [Fact]
    public void ReportsAMissingConditionalSeparatorAtTheConditionalExpression()
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser("1 ? 2");

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain("expected ':' in conditional expression");
    }

    [Theory]
    [InlineData("'", "unterminated character constant", "column 2")]
    [InlineData("'a", "expected closing quote for character constant", "column 3")]
    [InlineData("'1", "expected closing quote for character constant", "column 3")]
    [InlineData("'\\", "unterminated character escape", "column 3")]
    [InlineData("'\\7777'", "expected closing quote for character constant", "column 7")]
    public void RejectsIncompleteCharacterConstants(
        string expression,
        string expectedDiagnostic,
        string expectedColumn)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
        exception.Message.ShouldContain(expectedColumn);
    }

    [Theory]
    [InlineData("0", 0L)]
    [InlineData("0x2a", 42L)]
    [InlineData("0b101010", 42L)]
    [InlineData("052", 42L)]
    [InlineData("1'000", 1000L)]
    [InlineData("9'9", 99L)]
    [InlineData("0'0", 0L)]
    [InlineData("0x0'0", 0L)]
    [InlineData("0x9'9", 153L)]
    [InlineData("0xA'a", 170L)]
    [InlineData("0xA'A", 170L)]
    [InlineData("0xF'f", 255L)]
    [InlineData("42ULL", 42L)]
    [InlineData("0xffffffffffffffffu", -1L)]
    [InlineData("18446744073709551615u", -1L)]
    [InlineData("0x8000000000000000u", long.MinValue)]
    [InlineData("0xA", 10L)]
    [InlineData("0xF", 15L)]
    [InlineData("0xf", 15L)]
    [InlineData("0xFf", 255L)]
    [InlineData("0b0", 0L)]
    [InlineData("0b1", 1L)]
    [InlineData("0777", 511L)]
    [InlineData("'\\0'", 0L)]
    [InlineData("'\\a'", 7L)]
    [InlineData("'\\101'", 65L)]
    [InlineData("'\\1'", 1L)]
    [InlineData("'\\7'", 7L)]
    [InlineData("'\\77'", 63L)]
    [InlineData("'\\777'", 511L)]
    [InlineData("'\\x41'", 65L)]
    [InlineData("'\\xAf'", 175L)]
    [InlineData("'\\xFf'", 255L)]
    [InlineData("L'A'", 65L)]
    [InlineData("u'A'", 65L)]
    [InlineData("U'A'", 65L)]
    [InlineData("u8'A'", 65L)]
    [InlineData("L", 0L)]
    [InlineData("u", 0L)]
    [InlineData("U", 0L)]
    [InlineData("u8", 0L)]
    public void EvaluatesIntegerLiteralRadicesSeparatorsAndUnsignedValues(string expression, long expected)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var value = parser.Evaluate();

        // Assert
        value.ShouldBe(expected);
    }

    [Theory]
    [InlineData("0x")]
    [InlineData("0b")]
    [InlineData("08")]
    [InlineData("0b2")]
    [InlineData("0xg")]
    [InlineData("1''0")]
    [InlineData("1'")]
    [InlineData("0x10000000000000000u")]
    [InlineData("18446744073709551616u")]
    [InlineData("0b10000000000000000000000000000000000000000000000000000000000000000u")]
    public void RejectsMalformedAndOverflowingIntegerLiterals(string expression)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain("invalid integer");
    }

    [Fact]
    public void ParsesBinaryValuesAtSignedAndUnsignedBoundaries()
    {
        // Arrange
        var signedMaximum = new PreprocessorConditionalExpressionParser($"0b{new string('1', 63)}");
        var unsignedMaximum = new PreprocessorConditionalExpressionParser($"0b{new string('1', 64)}u");
        var signedOverflow = new PreprocessorConditionalExpressionParser($"0b1{new string('0', 63)}");

        // Act
        var signedValue = signedMaximum.Evaluate();
        var unsignedValue = unsignedMaximum.Evaluate();
        var exception = Should.Throw<InvalidOperationException>(() => signedOverflow.Evaluate());

        // Assert
        signedValue.ShouldBe(long.MaxValue);
        unsignedValue.ShouldBe(-1L);
        exception.Message.ShouldContain("invalid integer");
    }

    [Fact]
    public void EnforcesTheExpressionNestingLimitAtItsExactBoundary()
    {
        // Arrange
        var allowed = new PreprocessorConditionalExpressionParser(
            new string('!', PreprocessorLimits.MaximumExpressionNesting - 2) + "1");
        var excessive = new PreprocessorConditionalExpressionParser(
            new string('!', PreprocessorLimits.MaximumExpressionNesting - 1) + "1");

        // Act
        var allowedValue = allowed.Evaluate();
        var exception = Should.Throw<InvalidOperationException>(() => excessive.Evaluate());

        // Assert
        allowedValue.ShouldBe(1);
        exception.Message.ShouldContain("expression nesting exceeds");
    }

    [Theory]
    [InlineData("'\\x'", "invalid hexadecimal character escape")]
    [InlineData("'\\8'", "unsupported character escape")]
    public void RejectsInvalidCharacterEscapes(string expression, string diagnostic)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain(diagnostic);
    }

    [Theory]
    [InlineData("'\\7", "expected closing quote for character constant")]
    [InlineData("'\\x", "invalid hexadecimal character escape")]
    [InlineData("1 &", "expected an integer or identifier")]
    [InlineData("1 |", "expected an integer or identifier")]
    public void ReportsExpectedErrorsWhenScannersReachEndOfInput(string expression, string diagnostic)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain(diagnostic);
    }

    [Theory]
    [InlineData("1 || (1 / 0)", 1L)]
    [InlineData("0 && (1 / 0)", 0L)]
    [InlineData("1 ? 7 : (1 / 0)", 7L)]
    [InlineData("0 ? (1 / 0) : 9", 9L)]
    public void SkipsEvaluationOfUnselectedLogicalAndConditionalBranches(string expression, long expected)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var value = parser.Evaluate();

        // Assert
        value.ShouldBe(expected);
    }

    [Theory]
    [InlineData("1 || (1 + )")]
    [InlineData("0 && (1 / )")]
    [InlineData("1 ? 7 : (1 + )")]
    public void StillRejectsMalformedSyntaxInUnselectedBranches(string expression)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => parser.Evaluate());

        // Assert
        exception.Message.ShouldContain("expected an integer or identifier");
    }

    [Theory]
    [InlineData("2 || 3", 1L)]
    [InlineData("0 || -2", 1L)]
    [InlineData("2 && 3", 1L)]
    [InlineData("0 && -2", 0L)]
    [InlineData("!0", 1L)]
    [InlineData("!1", 0L)]
    [InlineData("1 ? 0 ? 2 : 3 : 4", 3L)]
    [InlineData("0 ? 4 : 1 ? 5 : 6", 5L)]
    [InlineData("0 ? 1 / 0 : 1 || 1 / 0", 1L)]
    public void EvaluatesLogicalAndRightAssociativeConditionalExpressions(string expression, long expected)
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser(expression);

        // Act
        var value = parser.Evaluate();

        // Assert
        value.ShouldBe(expected);
    }
}
