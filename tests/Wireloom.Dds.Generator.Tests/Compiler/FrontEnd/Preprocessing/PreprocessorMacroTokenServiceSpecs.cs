namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP010")]
[Trait("Preprocessor", "PP012")]
[Trait("Preprocessor", "PP013")]
[Trait("Preprocessor", "PP014")]
[Trait("Preprocessor", "PP015")]
[Trait("Preprocessor", "PP016")]
[Trait("Preprocessor", "PP018")]
[Trait("Preprocessor", "PP019")]
[Trait("Preprocessor", "PP020")]
public sealed class PreprocessorMacroTokenServiceSpecs
{
    [Fact]
    public void AppliesStringificationAndTokenPastingOutsideLiterals()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["name"] = ("hello world", "expanded")
        };

        // Act
        var stringified = service.TransformReplacement("#name", false, false, substitutions);
        var pasted = service.TransformReplacement("name ## suffix", false, false, substitutions);

        // Assert
        stringified.ShouldBe("\"hello world\"");
        pasted.ShouldBe("hello worldsuffix");
    }

    [Fact]
    public void ExpandsVariadicOptionalTokensAndReadsNestedArguments()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["__VA_ARGS__"] = ("one, two", "one, two")
        };

        // Act
        var replacement = service.TransformReplacement("prefix __VA_OPT__(, __VA_ARGS__)", true, true, substitutions);
        var parsed = service.TryReadArguments("(one, (two, three))", 0, out var arguments, out var end);

        // Assert
        replacement.ShouldBe("prefix , one, two");
        parsed.ShouldBeTrue();
        arguments.ShouldBe(["one", "(two, three)"]);
        end.ShouldBe(19);
    }

    [Fact]
    public void ExpandsVaOptWhenWhitespaceSeparatesTheMarkerAndParenthesis()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["__VA_ARGS__"] = ("one", "one")
        };

        // Act
        var replacement = service.TransformReplacement(
            "prefix __VA_OPT__ \t(, __VA_ARGS__)",
            true,
            true,
            substitutions);

        // Assert
        replacement.ShouldBe("prefix , one");
    }

    [Fact]
    public void RemovesVaOptContentsAndAdjacentWhitespaceWhenVariadicArgumentsAreEmpty()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var replacement = service.TransformReplacement(
            "prefix __VA_OPT__(, __VA_ARGS__) suffix",
            true,
            false,
            new Dictionary<string, (string Raw, string Expanded)>());

        // Assert
        replacement.ShouldBe("prefix suffix");
    }

    [Fact]
    public void RemovesLeadingVaOptContentsWithoutTrimmingAnEmptyOutputBuffer()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var replacement = service.TransformReplacement(
            "__VA_OPT__(discard)this",
            true,
            false,
            new Dictionary<string, (string Raw, string Expanded)>());

        // Assert
        replacement.ShouldBe("this");
    }

    [Fact]
    public void ReadsQuotedDelimitersWithoutSplittingMacroArguments()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var text = "(\"a,b)\", (nested, values), tail)";

        // Act
        var parsed = service.TryReadArguments(text, 0, out var arguments, out var end);

        // Assert
        parsed.ShouldBeTrue();
        arguments.ShouldBe(["\"a,b)\"", "(nested, values)", "tail"]);
        end.ShouldBe(text.Length);
    }

    [Fact]
    public void ReadsEmptyInvocationAsNoArguments()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var parsed = service.TryReadArguments("()", 0, out var arguments, out var end);

        // Assert
        parsed.ShouldBeTrue();
        arguments.ShouldBeEmpty();
        end.ShouldBe(2);
    }

    [Theory]
    [InlineData("X__VA_OPT__(contents)")]
    [InlineData("__VA_OPT__suffix(contents)")]
    [InlineData("__VA_OPT__")]
    [InlineData("__VA_OPT__   ")]
    [InlineData("__VA_OPT__(unclosed")]
    public void PreservesInvalidOrUnbalancedVaOptMarkers(string body)
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var replacement = service.TransformReplacement(
            body,
            true,
            true,
            new Dictionary<string, (string Raw, string Expanded)>());

        // Assert
        replacement.ShouldBe(body);
    }

    [Fact]
    public void ExpandsNestedVaOptContentsContainingQuotedParentheses()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var replacement = service.TransformReplacement(
            "__VA_OPT__(keep(\" ) \"))",
            true,
            true,
            new Dictionary<string, (string Raw, string Expanded)>());

        // Assert
        replacement.ShouldBe("keep(\" ) \")");
    }

    [Fact]
    public void LeavesStringifyAndCharizeMarkersInsideLiteralsUntouched()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["name"] = ("hello world", "expanded")
        };

        // Act
        var replacement = service.TransformReplacement(
            "\"#name\" #name \"#@name\" #@name",
            false,
            false,
            substitutions);

        // Assert
        replacement.ShouldBe("\"#name\" \"hello world\" \"#@name\" 'hello world'");
    }

    [Fact]
    public void PreservesEscapedQuotesAndMarkersInsideReplacementLiterals()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["name"] = ("value", "expanded")
        };

        // Act
        var replacement = service.TransformReplacement("\"before \\\" #name after\" name", false, false, substitutions);

        // Assert
        replacement.ShouldBe("\"before \\\" #name after\" expanded");
    }

    [Fact]
    public void EscapesQuotesWhenStringifyingAnExistingLiteral()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var stringified = service.Stringify("\"word\"");

        // Assert
        stringified.ShouldBe("\"\\\"word\\\"\"");
    }

    [Fact]
    public void RejectsUnterminatedBalancedTextAndArguments()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();

        // Act
        var balanced = service.TryReadBalancedText("(unfinished", 0, out var contents, out var balancedEnd);
        var arguments = service.TryReadArguments("(first, unfinished", 0, out var values, out var argumentsEnd);

        // Assert
        balanced.ShouldBeFalse();
        contents.ShouldBeEmpty();
        balancedEnd.ShouldBe(0);
        arguments.ShouldBeFalse();
        values.ShouldBe(["first"]);
        argumentsEnd.ShouldBe(0);
    }
}
