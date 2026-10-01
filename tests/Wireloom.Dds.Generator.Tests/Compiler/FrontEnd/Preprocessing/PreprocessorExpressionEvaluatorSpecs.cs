namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP023")]
[Trait("Preprocessor", "PP024")]
[Trait("Preprocessor", "PP025")]
[Trait("Preprocessor", "PP026")]
[Trait("Preprocessor", "PP027")]
public sealed class PreprocessorExpressionEvaluatorSpecs
{
    [Fact]
    public void ReplacesDefinedOperatorsBeforeExpressionEvaluation()
    {
        // Arrange
        var macros = new PreprocessorMacroTable();
        macros["ENABLED"] = new PreprocessorMacro(null, "1", false);
        var evaluated = string.Empty;
        var evaluator = CreateEvaluator(macros, expression =>
        {
            evaluated = expression;
            return 1;
        });
        var input = new IdlInput("condition.idl", "#if defined(ENABLED)");

        // Act
        var result = evaluator.Evaluate("defined(ENABLED)", input, 0, 0);

        // Assert
        result.ShouldBeTrue();
        evaluated.ShouldBe("1");
    }

    [Fact]
    public void PreservesMalformedDefinedDiagnosticOffset()
    {
        // Arrange
        var evaluator = CreateEvaluator(new PreprocessorMacroTable(), _ => 0);
        var input = new IdlInput("condition.idl", "0123456789");

        // Act
        var exception = Should.Throw<IdlException>(() => evaluator.Evaluate("defined(", input, 0, 7));

        // Assert
        exception.Message.ShouldContain("Invalid preprocessor condition: malformed defined operator");
        exception.Offset.ShouldBe(7);
    }

    private static PreprocessorExpressionEvaluator CreateEvaluator(
        PreprocessorMacroTable macros,
        Func<string, long> evaluateExpression) =>
        new(
            macros,
            (text, _) => text,
            (_, _) => false,
            (_, index) => index + 1,
            character => character == '_'
                || char.IsLetter(character),
            character => character == '_'
                || char.IsLetterOrDigit(character),
            evaluateExpression,
            _ => (false, false),
            CancellationToken.None);
}
