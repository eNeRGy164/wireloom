using System.IO;
using Wireloom;
using Shouldly;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlPreprocessorSpecs
{
    [Fact]
    [Trait("Corpus", "C048")]
    [Trait("Corpus", "C049")]
    [Trait("Corpus", "C050")]
    public void CompilesTheCorpusPreprocessingShape()
    {
        var rootPath = Path.Combine("fixtures", "C048-11-preprocessing.idl");
        var commonPath = Path.Combine("fixtures", "C048-11-common.idl");
        var root = new IdlInput(
            rootPath,
            "#include \"C048-11-common.idl\"\n" +
            "#define CORPUS_VALUE(x) ((x) + 1)\n" +
            "#if defined(CORPUS_CAPTURE_DEFINE)\n" +
            "const long BranchValue = CORPUS_VALUE(IncludedConstant);\n" +
            "#else\n" +
            "const long BranchValue = 0;\n" +
            "#endif\n" +
            "module CorpusPreprocessing { struct Preprocessed { long value; }; };",
            defines: ["CORPUS_CAPTURE_DEFINE"]);
        var common = new IdlInput(
            commonPath,
            "#ifndef CORPUS_COMMON\n" +
            "#define CORPUS_COMMON\n" +
            "const long IncludedConstant = 7;\n" +
            "struct IncludedType { long value; };\n" +
            "#endif\n",
            generate: false);

        var output = IdlCompiler.Compile([root, common], TestContext.Current.CancellationToken);

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
        var source =
            "#define SOURCE_FEATURE\n" +
            "#if defined(SOURCE_FEATURE)\n" +
            "struct Before { int32 value; };\n" +
            "#endif\n" +
            "#undef SOURCE_FEATURE\n" +
            "#if defined(SOURCE_FEATURE)\n" +
            "struct WrongAfterUndef { int32 value; };\n" +
            "#else\n" +
            "struct After { boolean value; };\n" +
            "#endif\n" +
            "#if defined(EXTERNAL_FEATURE)\n" +
            "struct WrongExternal { int32 value; };\n" +
            "#else\n" +
            "struct ExternalAfterUndef { boolean value; };\n" +
            "#endif\n";
        var input = new IdlInput(
            Path.Combine("fixtures", "C049-11-conditionals.idl"),
            source,
            undefines: ["EXTERNAL_FEATURE"]);

        var output = IdlCompiler.Compile([input], TestContext.Current.CancellationToken);

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
        var source =
            "#define VALUE_TYPE int32\n" +
            "#define VALUE_BOUND 3\n" +
            "struct MacroUse { VALUE_TYPE value; string<VALUE_BOUND> text; };\n";

        var output = IdlCompiler.Compile(
            [CompilerTestSupport.Input("C050-11-macros.idl", source)],
            TestContext.Current.CancellationToken);

        output.ShouldContain("public int value");
        output.ShouldContain("Bound(3)");
    }

    [Fact]
    [Trait("Corpus", "C051")]
    public void ExpandsStringificationAndTokenPasting()
    {
        var preprocessor = new IdlPreprocessor([], []);
        var source = preprocessor.Process(
            CompilerTestSupport.Input("C051-11-macro-operators.idl",
                "#define CAT(a,b) a ## b\n" +
                "#define STR(x) #x\n" +
                "const string Name = STR(sample);\n" +
                "struct Sample { long CAT(fi,eld); };"),
            (_, _, _) => { });

        source.ShouldContain("const string Name = \"sample\";");
        source.ShouldContain("struct Sample { long field; };");
    }
}
