using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorExtractedServiceSpecs
{
    /// <summary>Verifies the extracted conditional-expression parser.</summary>
    [Fact]
    [Trait("Preprocessor", "PP024")]
    [Trait("Preprocessor", "PP025")]
    [Trait("Preprocessor", "PP026")]
    public void EvaluatesExtractedConditionalExpressionGrammar()
    {
        // Arrange
        var parser = new PreprocessorConditionalExpressionParser("(2 + 3) * 4 == 20 ? 'A' : 0");

        // Act
        var result = parser.Evaluate();

        // Assert
        result.ShouldBe(65);
    }

    /// <summary>Verifies function-like macro parameter extraction.</summary>
    [Fact]
    [Trait("Preprocessor", "PP007")]
    [Trait("Preprocessor", "PP013")]
    public void ExtractedMacroDefinitionServicePreservesFunctionMacroParameters()
    {
        // Arrange
        var macros = new PreprocessorMacroTable();
        var service = new PreprocessorMacroDefinitionService(macros, new PreprocessorMacroTokenService());

        // Act
        service.Define(Input("macro.idl", string.Empty), 0, "JOIN(left, right) left ## right");

        // Assert
        macros.TryGetValue("JOIN", out var macro).ShouldBeTrue();
        macro.Parameters.ShouldBe(["left", "right"]);
        macro.Body.ShouldBe("left ## right");
    }

    /// <summary>Verifies line text and source-boundary preservation.</summary>
    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void ExtractedLineScannerPreservesCrLfAndOriginalBoundaries()
    {
        // Arrange
        var scanner = new PreprocessorInputLineScanner();

        // Act
        var lines = scanner.Scan("first\r\nsecond", offset => offset + 10).ToArray();

        // Assert
        lines.Select(line => line.Text).ShouldBe(["first", "second"]);
        lines.Select(line => line.OriginalOffset).ShouldBe([10, 17]);
        lines[0].End.ShouldBe(6);
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void ExtractedLineScannerPreservesLeadingEmptyLineBoundaries()
    {
        // Arrange
        var scanner = new PreprocessorInputLineScanner();

        // Act
        var lines = scanner.Scan("\nnext", offset => offset + 10).ToArray();

        // Assert
        lines.Select(line => line.Text).ShouldBe([string.Empty, "next"]);
        lines.Select(line => line.Offset).ShouldBe([0, 1]);
        lines.Select(line => line.End).ShouldBe([0, 5]);
        lines.Select(line => line.OriginalOffset).ShouldBe([10, 11]);
    }
}
