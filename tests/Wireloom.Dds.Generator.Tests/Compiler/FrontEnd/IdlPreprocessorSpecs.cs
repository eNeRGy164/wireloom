using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

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
}
