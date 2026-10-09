using System.Text.RegularExpressions;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorMacroSpecs : IdlPreprocessorTestBase
{
    [Fact]
    [Trait("Preprocessor", "PP006")]
    public void AcceptsFunctionBodiesWithoutWhitespace()
    {
        // Arrange
        const string input =
            """
            #define ID(value)value
            ID(kept)
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("kept");
    }

    [Fact]
    [Trait("Preprocessor", "PP008")]
    public void LeavesFunctionLikeMacroNameUnexpandedWithoutInvocationArguments()
    {
        // Arrange
        const string input = "#define IDENTITY(value) value\nIDENTITY";

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("IDENTITY");
    }

    [Fact]
    [Trait("Preprocessor", "PP007")]
    public void AcceptsWhitespaceBetweenAFunctionMacroNameAndInvocation()
    {
        // Arrange
        const string input =
            """
            #define ID(value) value
            ID   (kept)
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("kept");
    }

    [Fact]
    [Trait("Preprocessor", "PP006")]
    public void RejectsDuplicateMacroParameters()
    {
        // Arrange
        const string input = """#define BAD(value, value) value""";

        // Act
        var exception = Should.Throw<IdlException>(() => Process(input));

        // Assert
        exception.Message.ShouldContain("Duplicate macro parameter");
    }

    [Fact]
    [Trait("Preprocessor", "PP046")]
    public void DoesNotExpandUnusedCounterArguments()
    {
        // Arrange
        const string input =
            """
            #define IGNORE(value) 0
            IGNORE(__COUNTER__) __COUNTER__
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("0 0");
    }

    [Fact]
    [Trait("Preprocessor", "PP007")]
    [Trait("Preprocessor", "PP008")]
    [Trait("Preprocessor", "PP010")]
    public void PrescansArgumentsAndRescansNestedFunctionMacroResults()
    {
        // Arrange
        const string input =
            """
            #define VALUE 4
            #define DOUBLE(x) ((x) + (x))
            #define APPLY(function, value) function(value)
            APPLY(DOUBLE, VALUE)
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("((4) + (4))");
    }

    [Fact]
    [Trait("Preprocessor", "PP008")]
    public void PreservesParenthesizedSuffixAfterAnEmptyMacroReplacement()
    {
        // Arrange
        const string input =
            """
            #define EMPTY()
            const long Value = EMPTY()(7);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = (7);");
    }

    [Fact]
    [Trait("Preprocessor", "PP007")]
    [Trait("Preprocessor", "PP008")]
    public void PrescansNestedInvocationsOfTheCurrentMacro()
    {
        // Arrange
        const string input =
            """
            #define ID(value) value
            ID(ID(1))
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("1");
    }

    [Fact]
    [Trait("Preprocessor", "PP011")]
    public void KeepsCommasInsideLiteralMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define PICK(value) value
            const string Selected = PICK("a,b");
            const char Comma = PICK(',');
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = \"a,b\";");
        source.ShouldContain("const char Comma = ',';");
    }

    [Fact]
    [Trait("Preprocessor", "PP011")]
    public void KeepsEscapedCommasInsideLiteralMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define PICK(value) value
            const string Selected = PICK("a\",b");
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = \"a\\\",b\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP020")]
    public void DoesNotSubstituteMacroParametersInsideLiterals()
    {
        // Arrange
        const string input =
            """
            #define SHOW(value) "value"
            const string Selected = SHOW(sample);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = \"value\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP020")]
    public void DoesNotTreatPrefixedLiteralReplacementTextAsMacroSyntax()
    {
        // Arrange
        const string input =
            """
            #define SHOW(value) u8"value # ##"
            const string Selected = SHOW(sample);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = u8\"value # ##\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP020")]
    public void ReplacesOnlyCompleteMacroParameterTokens()
    {
        // Arrange
        const string input =
            """
            #define VALUE 7
            #define PROJECT(value) value value_suffix value.member
            PROJECT(VALUE)
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("7 value_suffix 7.member");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void SuppressesSelfReferentialMacrosWithoutRejectingTheInput()
    {
        // Arrange
        const string selfReferentialInput =
            """
            #define SELF SELF
            SELF
            """;
        const string mutuallyReferentialInput =
            """
            #define FIRST SECOND
            #define SECOND FIRST
            FIRST
            """;

        // Act
        var selfReferentialOutput = Process(selfReferentialInput);
        var mutuallyReferentialOutput = Process(mutuallyReferentialInput);

        // Assert
        selfReferentialOutput.Trim().ShouldBe("SELF");
        mutuallyReferentialOutput.Trim().ShouldBe("FIRST");
    }

    [Fact]
    [Trait("Preprocessor", "PP012")]
    public void StringificationNormalizesWhitespaceAndEscapesQuotesAndBackslashes()
    {
        // Arrange
        const string input =
            """
            #define TEXT(value) #value
            TEXT(  a   "b"   c  )
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("\"a \\\"b\\\" c\"");
    }

    [Fact]
    [Trait("Preprocessor", "PP012")]
    public void StringificationPreservesWhitespaceInsideLiteralArguments()
    {
        // Arrange
        const string input =
            """
            #define TEXT(value) #value
            const string Value = TEXT("a   b");
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Value = \"\\\"a   b\\\"\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP012")]
    [Trait("Preprocessor", "PP046")]
    public void DoesNotExpandArgumentsBeforeSpacedStringification()
    {
        // Arrange
        const string input =
            """
            #define TEXT(value) # value
            const string Text = TEXT(__COUNTER__);
            const long Counter = __COUNTER__;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Text = \"__COUNTER__\";");
        source.ShouldContain("const long Counter = 0;");
    }

    [Fact]
    [Trait("Preprocessor", "PP014")]
    public void CharizingWrapsRawMacroArgumentsInCharacterQuotes()
    {
        // Arrange
        const string input =
            """
            #define CHAR(value) #@value
            const char Selected = CHAR(x);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const char Selected = 'x';");
    }

    [Fact]
    [Trait("Preprocessor", "PP014")]
    public void DoesNotCharizeTextInsideLiteralReplacementTokens()
    {
        // Arrange
        const string input =
            """
            #define CHAR(value) "#@value"
            const string Selected = CHAR(x);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = \"#@value\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    [Trait("Preprocessor", "PP012")]
    public void DoesNotApplyStringificationInsidePrefixedLiteralReplacements()
    {
        // Arrange
        const string input =
            """
            #define SHOW(value) u8"# value" # value
            const string Selected = SHOW(sample);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = u8\"# value\" \"sample\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP013")]
    public void TokenPastingUsesRawOperandsAndRescansTheResult()
    {
        // Arrange
        const string input =
            """
            #define LEFT joined
            #define joinedName 9
            #define CAT(a,b) a ## b
            CAT(LEFT, Name)
            """;

        // Act
        var source = Process(input);

        // Assert
        source.Trim().ShouldBe("LEFTName");
    }

    [Fact]
    [Trait("Preprocessor", "PP018")]
    public void TreatsEmptyTokenPasteOperandsAsPlacemarkerTokens()
    {
        // Arrange
        const string input =
            """
            #define CAT(left, right) left ## right
            const long LeftEmpty = CAT(, 7);
            const long RightEmpty = CAT(7, );
            const long BothEmpty = CAT(, );
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long LeftEmpty = 7;");
        source.ShouldContain("const long RightEmpty = 7;");
        source.ShouldContain("const long BothEmpty = ;");
    }

    [Fact]
    [Trait("Preprocessor", "PP019")]
    public void ExpandsNamedVariadicMacroParameters()
    {
        // Arrange
        const string input =
            """
            #define JOIN(parts...) parts
            const string Values = JOIN(alpha, beta, gamma);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Values = alpha, beta, gamma;");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void DoesNotExpandVaOptInsideLiteralReplacementTokens()
    {
        // Arrange
        const string input =
            """
            #define OPTIONAL(...) "__VA_OPT__(literal)" __VA_OPT__(present)
            const string Empty = OPTIONAL();
            const string Present = OPTIONAL(value);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Empty = \"__VA_OPT__(literal)\";");
        source.ShouldContain("const string Present = \"__VA_OPT__(literal)\" present;");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void PreservesVaOptTextWhenItsParenthesesAreMissingOrUnbalanced()
    {
        // Arrange
        const string input =
            """
            #define MISSING(...) __VA_OPT__ value
            #define BROKEN(...) __VA_OPT__(value
            const string Missing = MISSING(1);
            const string Broken = BROKEN(1);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Missing = __VA_OPT__ value;");
        source.ShouldContain("const string Broken = __VA_OPT__(value;");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void SkipsParenthesesInsideVaOptLiteralArguments()
    {
        // Arrange
        const string input =
            """
            #define OPTIONAL(...) __VA_OPT__(")")
            const string Value = OPTIONAL(argument);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Value = \")\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP003")]
    [Trait("Preprocessor", "PP016")]
    public void SkipsParenthesesInsidePrefixedVaOptLiteralArguments()
    {
        // Arrange
        const string input =
            """
            #define OPTIONAL(...) __VA_OPT__(L")")
            const string Value = OPTIONAL(argument);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Value = L\")\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void SkipsParenthesesInsideEncodingPrefixedVaOptLiterals()
    {
        // Arrange
        const string input =
            """
            #define OPTIONAL(...) __VA_OPT__(u8")")
            const string Value = OPTIONAL(argument);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Value = u8\")\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP046")]
    public void ExpandsCounterPredefinedMacroDeterministically()
    {
        // Arrange
        const string input =
            """
            const long First = __COUNTER__;
            const long Second = __COUNTER__;
            const long ThroughMacro = __COUNTER__;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long First = 0;");
        source.ShouldContain("const long Second = 1;");
        source.ShouldContain("const long ThroughMacro = 2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP015")]
    public void AcceptsEmptyVariadicArgumentsWithoutAnArityDiagnostic()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = Input("empty-variadic.idl",
            """
            #define OPTIONAL(value, ...) value __VA_ARGS__
            OPTIONAL(7)
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        source.Trim().ShouldBe("7");
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    [Trait("Preprocessor", "PP033")]
    public void ExpandsMacroGeneratedIncludeOperands()
    {
        // Arrange
        string? included = null;
        var input = Input("macro-include.idl",
            """
            #define HEADER "generated.idl"
            #include HEADER
            """);

        // Act
        new IdlPreprocessor([], []).Process(input, (name, _, _) => included = name);

        // Assert
        included.ShouldBe("generated.idl");
    }

    [Fact]
    [Trait("Preprocessor", "PP044")]
    public void ExpandsFileAndLinePredefinedMacros()
    {
        // Arrange
        const string input =
            """
            #define CAPTURE(value) value
            const long DirectLine = __LINE__;
            const string SourceFile = __FILE__;
            const long ExpandedLine = CAPTURE(__LINE__);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long DirectLine = 2;");
        source.ShouldContain("const string SourceFile = \"preprocessor.idl\";");
        source.ShouldContain("const long ExpandedLine = 4;");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void RescansMacroReplacementsWithFollowingTokens()
    {
        // Arrange
        const string input =
            """
            #define FORWARD TARGET
            #define TARGET(value) value + 1
            const long Result = FORWARD(2);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Result = 2 + 1;");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void InvokesFunctionLikeMacroProducedByAnObjectLikeReplacement()
    {
        // Arrange
        const string input =
            """
            #define ALIAS FUNCTION
            #define FUNCTION(value) value + 1
            const long Result = ALIAS(2);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Result = 2 + 1;");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void ReEnablesMacroBeforeScanningAnIndependentSuffixInvocation()
    {
        // Arrange
        const string input =
            """
            #define ID(value) value
            const long Values = ID(1) ID(2);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Values = 1 2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP041")]
    public void RestoresParentContextAfterRecursivePreprocessing()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var parent = Input("parent.idl",
            """
            #include "child.idl"
            const long ParentLine = __LINE__;
            const string ParentFile = __FILE__;
            """);
        var child = Input("child.idl",
            """
            #line 900 "child.logical.idl"
            const long ChildLine = __LINE__;
            """);

        // Act
        var source = preprocessor.Process(parent, (_, _, _) => preprocessor.Process(child, (_, _, _) => { }));

        // Assert
        source.ShouldContain("const long ParentLine = 2;");
        source.ShouldContain("const string ParentFile = \"parent.idl\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP041")]
    public void RestoresParentPhysicalLineTableAfterRecursivePreprocessing()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var parent = Input("parent.idl",
            """
            #include "child.idl"
            const long ParentLine = __LINE__;
            """);
        var child = Input("child.idl",
            """
            const long First = 1; \
            + 2;
            const long Second = 3;
            const long Third = 4;
            const long Fourth = 5;
            const long Fifth = 6;
            """);

        // Act
        var source = preprocessor.Process(parent, (_, _, _) => preprocessor.Process(child, (_, _, _) => { }));

        // Assert
        source.ShouldContain("const long ParentLine = 2;");
    }

    [Fact]
    [Trait("Preprocessor", "PP044")]
    public void MapsPredefinedLineAfterLineSplicing()
    {
        // Arrange
        const string input =
            """
            const long Value = 1 + \
            2;
            const long Line = __LINE__;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Line = 3;");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    [Trait("Preprocessor", "PP044")]
    public void MapsPredefinedLineInSuffixAfterMacroExpansionAndLineSplicing()
    {
        // Arrange
        const string input =
            """
            #define PREFIX 1 +
            const long Value = PREFIX \
            __LINE__;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1 + 3;");
    }

    [Fact]
    [Trait("Preprocessor", "PP004")]
    [Trait("Preprocessor", "PP050")]
    public void UsesTheJoinedOffsetWhenExpandingPredefinedMacrosInConditions()
    {
        // Arrange
        const string input =
            """
            const long Value = 1 + \
            2;
            #if __LINE__ == 3
            const long Selected = 1;
            #else
            const long Selected = 0;
            #endif
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Selected = 1;");
        source.ShouldNotContain("const long Selected = 0;");
    }

    [Fact]
    [Trait("Preprocessor", "PP013")]
    public void DoesNotPasteTokensInsideLiteralReplacementTokens()
    {
        // Arrange
        const string input =
            """
            #define TEXT() "a##b"
            const string Value = TEXT();
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Value = \"a##b\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP013")]
    public void KeepsArgumentsRawWhenTokenPasteUsesSpacedOperators()
    {
        // Arrange
        const string input =
            """
            #define VALUE 7
            #define CAT(left, right) left  ##  right
            const long Result = CAT(VALUE, 2);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Result = VALUE2;");
        source.ShouldNotContain("const long Result = 72;");
    }

    [Fact]
    [Trait("Preprocessor", "PP011")]
    public void KeepsCommasInsideEncodingPrefixedLiteralMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define PICK(value) value
            const string Selected = PICK(u8"a,b");
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Selected = u8\"a,b\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP027")]
    public void TreatsDigitSeparatorsAsNumericTextWhenScanningMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define FIRST(value, ignored) value
            const long Value = FIRST(1'000, 2);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1'000;");
    }

    [Fact]
    [Trait("Preprocessor", "PP011")]
    public void PreservesDigitSeparatorsInMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define ID(value) value
            const long Number = ID(1'000); // removed
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Number = 1'000;");
        source.ShouldNotContain("removed");
    }

    [Fact]
    [Trait("Preprocessor", "PP012")]
    public void PreservesDigitSeparatorsWhenStringizingMacroArguments()
    {
        // Arrange
        const string input =
            """
            #define TEXT(value) #value
            const string Text = TEXT(1'000);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const string Text = \"1'000\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void PreservesDigitSeparatorsWhenExpandingVaOptArguments()
    {
        // Arrange
        const string input =
            """
            #define OPTIONAL(...) __VA_OPT__(__VA_ARGS__)
            const long Optional = OPTIONAL(1'000);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Optional = 1'000;");
    }

    [Theory]
    [InlineData("L")]
    [InlineData("u")]
    [InlineData("U")]
    [InlineData("u8")]
    [Trait("Preprocessor", "PP003")]
    public void PreservesEncodingPrefixedCharacterLiteralsDuringExpansion(string prefix)
    {
        // Arrange
        var input = $$"""
            #define a 7
            const char Value = {{prefix}}'a';
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain($"const char Value = {prefix}'a';");
    }

    [Theory]
    [InlineData("L")]
    [InlineData("u")]
    [InlineData("U")]
    [InlineData("u8")]
    [Trait("Preprocessor", "PP003")]
    public void DoesNotExpandMacrosUsedAsCharacterLiteralEncodingPrefixes(string prefix)
    {
        // Arrange
        var input = $$"""
            #define {{prefix}} 7
            const char Value = {{prefix}}'a';
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain($"const char Value = {prefix}'a';");
        source.ShouldNotContain("const char Value = 7'a';");
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void PreservesTokenSeparationWhenVaOptIsEmpty()
    {
        // Arrange
        const string input =
            """
            #define F(...) left __VA_OPT__(middle)right
            const long Value = F();
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = left right;");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void DoesNotExhaustExpansionDepthForIndependentMacroInvocations()
    {
        // Arrange
        var invocations = string.Join(" ", Enumerable.Repeat("VALUE", 80));
        var input = $$"""
            #define VALUE 1
            const string Values = {{invocations}};
            """;

        // Act
        var source = Process(input);

        // Assert
        Regex.IsMatch(source, @"\bVALUE\b").ShouldBeFalse();
    }

    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void RejectsExcessiveMacroReplacementDepth()
    {
        // Arrange
        var definitions = string.Join(
            Environment.NewLine,
            Enumerable.Range(0, 65).Select(index => $"#define LEVEL{index} LEVEL{index + 1}"))
            + Environment.NewLine +
            "#define LEVEL65 1";
        var input = Input("macro-depth.idl",
            $$"""
            {{definitions}}
            LEVEL0
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Macro expansion exceeds the 64-level nesting limit");
        exception.Input.ShouldBeSameAs(input);
    }
}
