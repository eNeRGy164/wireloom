using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorSpecs
{
    [Fact]
    public void ReportsThatIncludeFilenamesNeedQuotesOrAngleBrackets()
    {
        // Arrange
        var input = Input("unquoted-include.idl",
            """
            #include CommonHeader.idl
            struct Sample { long value; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("expected a quoted or angle-bracket filename");
        exception.Offset.ShouldBe(0);
        exception.Input.Path.ShouldEndWith("unquoted-include.idl");
    }

    [Fact]
    [Trait("Corpus", "C048")]
    [Trait("Corpus", "C049")]
    [Trait("Corpus", "C050")]
    public void CompilesTheCorpusPreprocessingShape()
    {
        // Arrange
        var root = new IdlInput(Path.Combine("fixtures", "C048-11-preprocessing.idl"),
            """
            #include "C048-11-common.idl"
            #define CORPUS_VALUE(x) ((x) + 1)
            #if defined(CORPUS_CAPTURE_DEFINE)
            const long BranchValue = CORPUS_VALUE(IncludedConstant);
            #else
            const long BranchValue = 0;
            #endif
            module CorpusPreprocessing { struct Preprocessed { long value; }; };
            """,
            defines: ["CORPUS_CAPTURE_DEFINE"]);

        var common = new IdlInput(Path.Combine("fixtures", "C048-11-common.idl"),
            """
            #ifndef CORPUS_COMMON
            #define CORPUS_COMMON
            const long IncludedConstant = 7;
            struct IncludedType { long value; };
            #endif
            """,
            generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("BranchValue");
        output.ShouldContain("IncludedConstant");
        output.ShouldContain("public const int Value");
        output.ShouldContain("+ 1");
        output.ShouldContain("public class Preprocessed");
        output.ShouldContain("public class IncludedType");
        output.ShouldNotContain("public const int Value = 0;");
    }

    [Fact]
    [Trait("Corpus", "C049")]
    public void AppliesSourceUndefineAndExternalUndefineToConditionalBranches()
    {
        // Arrange
        var input = new IdlInput(
            Path.Combine("fixtures", "C049-11-conditionals.idl"),
            """
            #define SOURCE_FEATURE
            #if defined(SOURCE_FEATURE)
            struct Before { int32 value; };
            #endif
            #undef SOURCE_FEATURE
            #if defined(SOURCE_FEATURE)
            struct WrongAfterUndef { int32 value; };
            #else
            struct After { boolean value; };
            #endif
            #if defined(EXTERNAL_FEATURE)
            struct WrongExternal { int32 value; };
            #else
            struct ExternalAfterUndef { boolean value; };
            #endif
            """,
            undefines: ["EXTERNAL_FEATURE"]);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public class Before");
        output.ShouldContain("public class After");
        output.ShouldContain("public class ExternalAfterUndef");
        output.ShouldNotContain("WrongAfterUndef");
        output.ShouldNotContain("WrongExternal");
    }

    [Fact]
    [Trait("Corpus", "C050")]
    public void ExpandsObjectLikeMacrosInTypesAndBounds()
    {
        // Arrange
        var input = Input("C050-11-macros.idl",
            """
            #define VALUE_TYPE int32
            #define VALUE_BOUND 3
            struct MacroUse { VALUE_TYPE value; string<VALUE_BOUND> text; };
            """);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public int value");
        output.ShouldContain("Bound(3)");
    }

    [Fact]
    [Trait("Corpus", "C051")]
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
    }

    [Fact]
    public void RejectsRecursiveMacroExpansion()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("recursive-macro.idl", "#define LOOP LOOP\nLOOP");

        // Act
        var exception = Should.Throw<IdlException>(() => preprocessor.Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Recursive macro expansion");
    }

    [Fact]
    public void RejectsUnsupportedActivePreprocessorDirectives()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("unsupported-directive.idl", "#pragma once");

        // Act
        var exception = Should.Throw<IdlException>(() => preprocessor.Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unsupported preprocessor directive");
    }

    [Fact]
    public void ReportsAngleBracketIncludeFilenames()
    {
        // Arrange
        var includeName = string.Empty;
        var includeIsAngle = false;
        var includeOffset = -1;
        var includePreprocessor = new IdlPreprocessor([], []);
        var includeInput = Input("include.idl", "#include <Common.idl>");

        // Act
        includePreprocessor.Process(includeInput, (name, isAngle, offset) =>
        {
            includeName = name;
            includeIsAngle = isAngle;
            includeOffset = offset;
        });

        // Assert
        includeName.ShouldBe("Common.idl");
        includeIsAngle.ShouldBeTrue();
        includeOffset.ShouldBe(0);
    }

    [Fact]
    public void IgnoresUnsupportedDirectivesInInactiveBranches()
    {
        // Arrange
        var preprocessor = new IdlPreprocessor([], []);
        var input = Input("inactive.idl", "#if 0\n#pragma ignored\n#endif");

        // Act
        var source = preprocessor.Process(input, (_, _, _) => { });

        // Assert
        source.ShouldNotContain("#pragma ignored");
    }

    [Fact]
    public void RejectsMalformedDefineDirectives()
    {
        // Arrange
        var input = Input("bad-define.idl", "#define 1");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(
            input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Malformed #define");
    }

    [Fact]
    public void RejectsMalformedUndefDirectives()
    {
        // Arrange
        var input = Input("bad-undef.idl", "#undef 1");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Malformed #undef");
    }

    [Fact]
    public void RejectsUnexpectedElseDirectives()
    {
        // Arrange
        var input = Input("bad-else.idl", "#else");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unexpected #else");
    }

    [Fact]
    public void RejectsUnexpectedEndifDirectives()
    {
        // Arrange
        var input = Input("bad-endif.idl", "#endif");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unexpected #endif");
    }

    [Fact]
    public void RejectsErrorDirectives()
    {
        // Arrange
        var input = Input("bad-error.idl", "#error stop");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Preprocessor error");
    }

    [Fact]
    public void RejectsUnterminatedPreprocessorConditionals()
    {
        // Arrange
        var input = Input("unterminated.idl", "#if 1\nvalue");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated preprocessor conditional");
    }

    [Fact]
    public void RejectsUnterminatedStringLiterals()
    {
        // Arrange
        var input = Input("unterminated-string.idl", "const string Value = \"value;");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
    }
}
