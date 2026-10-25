namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP027")]
[Trait("Preprocessor", "PP029")]
[Trait("Preprocessor", "PP030")]
public sealed class PreprocessorUnsignedComparisonSpecs
{
    [Theory]
    [InlineData("2ULL == 2", true)]
    [InlineData("2ULL != 3", true)]
    [InlineData("2ULL < 3", true)]
    [InlineData("3ULL > 2", true)]
    [InlineData("2ULL <= 2", true)]
    [InlineData("2ULL >= 2", true)]
    [InlineData("2ULL < 2", false)]
    [InlineData("2ULL > 2", false)]
    [InlineData("1u == 1", true)]
    [InlineData("1LU == 1", true)]
    [InlineData("1LLU == 1", true)]
    [InlineData("1uLL == 1", true)]
    [InlineData("1UL == 1", true)]
    [InlineData("1lU == 1", true)]
    [InlineData("1uLl == 1", true)]
    [InlineData("1lLu == 1", true)]
    [InlineData("0ULL == + 0", true)]
    [InlineData("+1ULL == 1", true)]
    [InlineData("-0ULL == 0", true)]
    [InlineData("-1ULL > 1", true)]
    public void EvaluatesEachComparisonAtItsBoundary(string expression, bool expected)
    {
        // Arrange

        // Act
        var result = PreprocessorUnsignedComparison.Evaluate(expression);

        // Assert
        result.Matched.ShouldBeTrue();
        result.Result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("0b101ULL == 5", true)]
    [InlineData("0ULL == 0", true)]
    [InlineData("077ULL == 63", true)]
    [InlineData("0xABCULL == 2748", true)]
    [InlineData("0x7fffffffffffffffLL == 9223372036854775807ULL", true)]
    [InlineData("0x8000000000000000ULL > 9223372036854775807ULL", true)]
    [InlineData("0xffffffffffffffffULL == 18446744073709551615ULL", true)]
    [InlineData("01777777777777777777777ULL == 18446744073709551615ULL", true)]
    [InlineData("0xffffffffffffffffULL > 0LL", true)]
    [InlineData("18446744073709551615ULL == -1", true)]
    [InlineData("1'000ULL == 1000", true)]
    [InlineData("9'9ULL == 99", true)]
    [InlineData("0'0ULL == 0", true)]
    [InlineData("0b1'0ULL == 2", true)]
    [InlineData("0'7ULL == 7", true)]
    [InlineData("077'7ULL == 511", true)]
    [InlineData("0x0'0ULL == 0", true)]
    [InlineData("0x9'9ULL == 153", true)]
    [InlineData("0xA'BCULL == 2748", true)]
    [InlineData("0xA'aULL == 170", true)]
    [InlineData("0xF'fULL == 255", true)]
    [InlineData("0xa'FLL == 175ULL", true)]
    public void ParsesSupportedUnsignedIntegerForms(string expression, bool expected)
    {
        // Arrange

        // Act
        var result = PreprocessorUnsignedComparison.Evaluate(expression);

        // Assert
        result.Matched.ShouldBeTrue();
        result.Result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("1 < 2")]
    [InlineData("0b2ULL == 0")]
    [InlineData("09ULL == 9")]
    [InlineData("1''0ULL == 10")]
    [InlineData("'1ULL == 1")]
    [InlineData("1'ULL == 1")]
    [InlineData("0b1'2ULL == 2")]
    [InlineData("0b'1ULL == 1")]
    [InlineData("0b1'ULL == 1")]
    [InlineData("077'8ULL == 511")]
    [InlineData("0xA'GULL == 160")]
    [InlineData("0x8000000000000000LL == 0")]
    [InlineData("18446744073709551616ULL == 0")]
    [InlineData("0b10000000000000000000000000000000000000000000000000000000000000000ULL == 0")]
    [InlineData("0b1000000000000000000000000000000000000000000000000000000000000000LL == 0")]
    [InlineData("017777777777777777777777ULL == 0")]
    [InlineData("0bULL == 0")]
    [InlineData("0xULL == 0")]
    [InlineData("1ULL == U")]
    [InlineData("ULL == 1ULL")]
    [InlineData("U == 1ULL")]
    [InlineData("1UU == 1")]
    [InlineData("1LLLU == 1")]
    [InlineData("9223372036854775808LL == 0")]
    [InlineData("1ULL + 2 == 3")]
    [InlineData("1ULL < 2ULL < 3")]
    public void RejectsUnsupportedOrOutOfRangeUnsignedComparisons(string expression)
    {
        // Arrange

        // Act
        var result = PreprocessorUnsignedComparison.Evaluate(expression);

        // Assert
        result.Matched.ShouldBeFalse();
        result.Result.ShouldBeFalse();
    }

    [Fact]
    public void ParsesSignedBinaryAndOctalValuesAtTheirMaximumBoundary()
    {
        // Arrange
        var maximumSignedBinary = $"0b{new string('1', 63)}LL == 9223372036854775807ULL";
        var signedBinaryOverflow = $"0b1{new string('0', 63)}LL == 0ULL";
        var maximumSignedOctal = $"0{new string('7', 21)}LL == 9223372036854775807ULL";
        var signedOctalOverflow = $"01{new string('0', 21)}LL == 0ULL";

        // Act
        var maximumBinaryResult = PreprocessorUnsignedComparison.Evaluate(maximumSignedBinary);
        var binaryOverflowResult = PreprocessorUnsignedComparison.Evaluate(signedBinaryOverflow);
        var maximumOctalResult = PreprocessorUnsignedComparison.Evaluate(maximumSignedOctal);
        var octalOverflowResult = PreprocessorUnsignedComparison.Evaluate(signedOctalOverflow);

        // Assert
        maximumBinaryResult.ShouldBe((true, true));
        binaryOverflowResult.ShouldBe((false, false));
        maximumOctalResult.ShouldBe((true, true));
        octalOverflowResult.ShouldBe((false, false));
    }
}
