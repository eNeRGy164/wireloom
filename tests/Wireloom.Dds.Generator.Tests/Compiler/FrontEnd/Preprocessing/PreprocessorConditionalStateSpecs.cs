namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP021")]
[Trait("Preprocessor", "PP028")]
public sealed class PreprocessorConditionalStateSpecs
{
    [Fact]
    public void SelectsOnlyTheFirstActiveBranch()
    {
        // Arrange
        var state = new PreprocessorConditionalState();
        var input = new IdlInput("conditional.idl", string.Empty);

        // Act
        state.Enter(false);
        var inactiveIf = state.IsActive;
        state.SelectElseIf(() => true, input, 4);
        var activeElif = state.IsActive;
        state.SelectElse(input, 10);
        var inactiveElse = state.IsActive;
        state.End(input, 16);

        // Assert
        inactiveIf.ShouldBeFalse();
        activeElif.ShouldBeTrue();
        inactiveElse.ShouldBeFalse();
        state.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void ReportsMalformedConditionalNesting()
    {
        // Arrange
        var state = new PreprocessorConditionalState();
        var input = new IdlInput("conditional.idl", "\n\n");

        // Act
        var exception = Should.Throw<IdlException>(() => state.SelectElse(input, 1));

        // Assert
        exception.Message.ShouldBe("Unexpected #else.");
        exception.Offset.ShouldBe(1);
    }

    [Fact]
    public void ReportsUnterminatedConditionalAtInputEnd()
    {
        // Arrange
        var state = new PreprocessorConditionalState();
        var input = new IdlInput("conditional.idl", "abc");
        state.Enter(true);

        // Act
        var exception = Should.Throw<IdlException>(() => state.EnsureComplete(input));

        // Assert
        exception.Message.ShouldBe("Unterminated preprocessor conditional.");
        exception.Offset.ShouldBe(input.Text.Length);
    }

    [Fact]
    public void DoesNotEvaluateElifInsideAnInactiveParentBranch()
    {
        // Arrange
        var state = new PreprocessorConditionalState();
        var input = new IdlInput("conditional.idl", string.Empty);
        var evaluated = false;
        state.Enter(false);
        state.Enter(true);

        // Act
        state.SelectElseIf(() =>
        {
            evaluated = true;
            return true;
        }, input, 0);
        var nestedActive = state.IsActive;
        state.End(input, 1);
        var parentActive = state.IsActive;
        state.End(input, 2);

        // Assert
        evaluated.ShouldBeFalse();
        nestedActive.ShouldBeFalse();
        parentActive.ShouldBeFalse();
        state.IsActive.ShouldBeTrue();
    }
}
