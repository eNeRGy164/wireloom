using static Wireloom.Dds.Idl.Tests.IdlTestSupport;

namespace Wireloom.Dds.Idl.Tests.Preprocessing;

public sealed class IdlPreprocessorDirectiveSpecs : IdlPreprocessorTestBase
{
    [Fact]
    [Trait("Preprocessor", "PP041")]
    [Trait("Preprocessor", "PP042")]
    public void AcceptsLineAndPragmaDirectivesWithoutChangingPhysicalDiagnostics()
    {
        // Arrange
        var input = Input("physical.idl",
            """
            #line 900 "logical.idl"
            #pragma once
            #error failure
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Input.Path.ShouldEndWith("physical.idl");
        exception.Offset.ShouldBe(input.Text.IndexOf("#error", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP041")]
    public void RemapsPredefinedFileAndLineValuesAfterLineDirective()
    {
        // Arrange
        const string input =
            """
            #line 900 "logical.idl"
            const long LogicalLine = __LINE__;
            const string LogicalFile = __FILE__;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long LogicalLine = 900;");
        source.ShouldContain("const string LogicalFile = \"logical.idl\";");
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void ConsumesInlinePragmaForms()
    {
        // Arrange
        const string input =
            """
            _Pragma("once")
            __pragma(once)
            const long Value = 1;
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long Value = 1;");
        source.ShouldNotContain("_Pragma");
        source.ShouldNotContain("__pragma");
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void DoesNotMarkOtherInlinePragmasAsOnceOnly()
    {
        // Arrange
        var input = Input("inline-pragma.idl",
            """
            _Pragma("pack(1)")
            __pragma(warning(disable: 1234))
            const long Value = 1;
            """);
        var onceEncountered = false;

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { }, pragmaOnceEncountered: () => onceEncountered = true);

        // Assert
        onceEncountered.ShouldBeFalse();
        result.HasPragmaOnce.ShouldBeFalse();
        result.Text.ShouldContain("const long Value = 1;");
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void AcceptsWhitespaceAroundInlinePragmaArguments()
    {
        // Arrange
        var input = Input("spaced-inline-pragma.idl",
            """
            _Pragma   (   "once"   )
            __pragma   (   once   )
            const long Value = 1;
            """);
        var onceEncountered = false;

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(
            input,
            (_, _, _) => { },
            pragmaOnceEncountered: () => onceEncountered = true);

        // Assert
        onceEncountered.ShouldBeTrue();
        result.Text.ShouldContain("const long Value = 1;");
        result.Text.ShouldNotContain("_Pragma");
        result.Text.ShouldNotContain("__pragma");
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void RequiresTheCorrectPayloadFormForEachInlinePragma()
    {
        // Arrange
        var input = Input("invalid-inline-pragma-payload.idl",
            """
            _Pragma(once)
            __pragma("once")
            const long Value = 1;
            """);
        var onceEncountered = false;

        // Act
        var result = new IdlPreprocessor([], []).ProcessWithMetadata(input, (_, _, _) => { }, pragmaOnceEncountered: () => onceEncountered = true);

        // Assert
        onceEncountered.ShouldBeFalse();
        result.HasPragmaOnce.ShouldBeFalse();
        result.Text.ShouldContain("const long Value = 1;");
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void PreservesMalformedInlinePragmasInsteadOfConsumingFollowingText()
    {
        // Arrange
        var input = Input("malformed-inline-pragma.idl", "_Pragma const long Value = 1;");

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        source.ShouldContain("_Pragma const long Value = 1;");
    }




    [Fact]
    [Trait("Corpus", "C051")]
    [Trait("Preprocessor", "PP012")]
    [Trait("Preprocessor", "PP013")]
    public void ExpandsStringificationAndTokenPasting()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("C051-11-macro-operators.idl",
            """
            #define CAT(a,b) a ## b
            #define STR(x) #x
            const string Name = STR(sample);
            struct Sample { long CAT(fi,eld); };
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldContain("const string Name = \"sample\";");
        source.ShouldContain("struct Sample { long field; };");
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void SelectsTheFirstActiveElifBranchAndPreservesInactiveLines()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor(["SECOND"], []);
        var input = Input("elif.idl",
            """
            #if defined(FIRST)
            first
            #elif defined(SECOND)
            selected
            #elif THIRD
            third
            #else
            fallback
            #endif
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldContain("selected");
        source.ShouldNotContain("first");
        source.ShouldNotContain("third");
        source.ShouldNotContain("fallback");
    }

    [Fact]
    [Trait("Preprocessor", "PP015")]
    [Trait("Preprocessor", "PP017")]
    [Trait("Preprocessor", "PP050")]
    public void ExpandsVariadicMacrosAndReportsWrongArgumentCounts()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("variadic.idl",
            """
            #define FIRST(value, ...) value
            #define ADD(left, right) left + right
            const long Selected = FIRST(7, 8, 9);
            const long Invalid = ADD(1);
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        source.ShouldContain("const long Selected = 7;");
        diagnostics.ShouldContain(d => d.Message.Contains("wrong number of arguments", StringComparison.Ordinal));
        var arityDiagnostic = diagnostics.Single(d => d.Message.Contains("wrong number of arguments", StringComparison.Ordinal));
        arityDiagnostic.Offset.ShouldBe(input.Text.IndexOf("ADD(1)", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP016")]
    public void ExpandsVaOptOnlyWhenVariadicTokensArePresent()
    {
        // Arrange
        const string input =
            """
            #define APPEND(first, ...) first __VA_OPT__(+ __VA_ARGS__)
            const long WithoutVariadic = APPEND(1);
            const long WithVariadic = APPEND(1, 2);
            const long WithVariadic2 = APPEND(1, 2, 3);
            """;

        // Act
        var source = Process(input);

        // Assert
        source.ShouldContain("const long WithoutVariadic = 1");
        source.ShouldContain("const long WithVariadic = 1 + 2");
        source.ShouldContain("const long WithVariadic2 = 1 + 2, 3");
    }

    [Fact]
    [Trait("Preprocessor", "PP009")]
    public void PreservesRecursiveMacroExpansionTokens()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("recursive-macro.idl",
            """
            #define LOOP LOOP
            LOOP
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.Trim().ShouldBe("LOOP");
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void AcceptsPragmaDirectives()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("unsupported-directive.idl", """#pragma once""");

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.Trim().ShouldBeEmpty();
    }

    [Fact]
    [Trait("Preprocessor", "PP035")]
    public void RecordsPragmaOnceOnlyWhenItIsActive()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var active = Input("active-pragma.idl",
            """
            #pragma once
            struct Sample { long value; };
            """);
        var inactive = Input("inactive-pragma.idl",
            """
            #if 0
            #pragma once
            #endif
            """);

        // Act
        var activeResult = preprocessor.ProcessWithMetadata(active, (_, _, _) => { });
        var inactiveResult = preprocessor.ProcessWithMetadata(inactive, (_, _, _) => { });

        // Assert
        activeResult.HasPragmaOnce.ShouldBeTrue();
        inactiveResult.HasPragmaOnce.ShouldBeFalse();
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void DoesNotTreatOtherPragmasAsPragmaOnce()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("other-pragma.idl",
            """
            #pragma managed
            struct Sample { long value; };
            """);

        // Act
        var result = preprocessor.ProcessWithMetadata(input, (_, _, _) => { });

        // Assert
        result.HasPragmaOnce.ShouldBeFalse();
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    public void IgnoresUnsupportedDirectivesInInactiveBranches()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("inactive.idl",
            """
            #if 0
            #pragma ignored
            #endif
            """);

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldNotContain("#pragma ignored");
    }

    [Theory]
    [InlineData(
        """
        #else
        #else
        #endif
        """,
        "second #else")]
    [InlineData(
        """
        #else
        #elif 1
        #endif
        """,
        "#elif after #else")]
    [Trait("Preprocessor", "PP021")]
    public void RejectsConditionalBranchesAfterElse(string body, string message)
    {
        // Arrange
        var input = Input("invalid-conditional.idl",
            $$"""
            #if 0
            {{body}}
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain(message);
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void DoesNotTreatPragmaNamesWithMessagePrefixAsMessages()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = Input("pragma-boundary.idl",
            """
            #pragma messageBox
            const long Value = 1;
            """);

        // Act
        var source = new IdlPreprocessor([], []).Process(input, (_, _, _) => { }, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
        source.ShouldContain("const long Value = 1;");
    }

}
