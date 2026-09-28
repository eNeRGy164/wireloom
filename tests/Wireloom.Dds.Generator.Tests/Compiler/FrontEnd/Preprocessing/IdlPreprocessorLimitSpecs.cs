using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorLimitSpecs : IdlPreprocessorTestBase
{
    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void ResetsMacroWorkForEachIncludedInput()
    {
        // Arrange
        var define = """#define VALUE 1""" + Environment.NewLine;
        var childText = define + string.Join(Environment.NewLine, Enumerable.Repeat("VALUE", 20_000));
        var parentText = """
            #include "child.idl"
            """ + Environment.NewLine +
            string.Join(Environment.NewLine, Enumerable.Repeat("VALUE", 5_001));
        var parent = Input("parent.idl", parentText);
        var child = Input("child.idl", childText, generate: false);
        var preprocessor = new IdlPreprocessor([], []);

        // Act
        var source = preprocessor.Process(parent, (_, _, _) => preprocessor.Process(child, (_, _, _) => { }));

        // Assert
        source.Count(character => character == '1').ShouldBeGreaterThan(2);
    }

    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void RejectsExcessiveMacroExpansionWorkOnOneLine()
    {
        // Arrange
        var input = Input(
            "macro-limit.idl",
            "#define VALUE 1" + Environment.NewLine +
            string.Join(" ", Enumerable.Repeat("VALUE", 40_000)));

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Macro expansion exceeds");
    }

    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void RejectsExcessivePreprocessorOutput()
    {
        // Arrange
        var input = Input("output-limit.idl", new string('x', (4 * 1024 * 1024) + 1));

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
    }

    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void RejectsMacroGeneratedOutputBeforeRescanningTheWholeReplacement()
    {
        // Arrange
        var seed = new string('x', (2 * 1024 * 1024) + 1);
        var input = Input("generated-output-limit.idl",
            "#define DUP(value) value value" + Environment.NewLine +
            "DUP(" + seed + ")");

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(input.Text.LastIndexOf("DUP", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP051")]
    public void ObservesCancellationBeforeProcessingInput()
    {
        // Arrange
        var input = Input("cancelled.idl", """struct Sample { long value; };""");
        var cancellation = new CancellationToken(canceled: true);

        // Act
        Action act = () => new IdlPreprocessor([], [], cancellation).Process(input, (_, _, _) => { });

        // Assert
        Should.Throw<OperationCanceledException>(act);
    }
}
