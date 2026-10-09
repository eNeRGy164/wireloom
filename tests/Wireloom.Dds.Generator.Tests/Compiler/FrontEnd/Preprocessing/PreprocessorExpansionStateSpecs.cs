namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP051")]
public sealed class PreprocessorExpansionStateSpecs
{
    [Fact]
    public void CountsWorkAndEnforcesDepthAndOutputLimits()
    {
        // Arrange
        var state = new PreprocessorExpansionState { BaseOutputLength = 10 };
        var input = new IdlInput("macro.idl", "0123456789");

        // Act
        state.CountOperation(input, 2, [], 0);
        var depthException = Should.Throw<IdlException>(() =>
            state.CountOperation(input, 3, [], PreprocessorLimits.MaximumMacroDepth + 1));
        var outputException = Should.Throw<IdlException>(() =>
            state.EnsureOutputLength(input, PreprocessorLimits.MaximumOutputLength, 4));

        // Assert
        state.MacroWork.ShouldBe(2);
        depthException.Message.ShouldContain("level nesting limit");
        outputException.Message.ShouldContain("Preprocessor output exceeds");
    }

    [Fact]
    public void AcceptsExactDepthAndRemainingOutputBoundaries()
    {
        // Arrange
        var state = new PreprocessorExpansionState { BaseOutputLength = 10 };
        var input = new IdlInput("macro.idl", string.Empty);

        // Act
        state.CountOperation(input, 0, null, PreprocessorLimits.MaximumMacroDepth);
        state.EnsureOutputLength(input, PreprocessorLimits.MaximumOutputLength - state.BaseOutputLength, 0);

        // Assert
        state.MacroWork.ShouldBe(1);
    }

    [Fact]
    public void AcceptsMaximumWorkCountAndRejectsTheNextOperation()
    {
        // Arrange
        var state = new PreprocessorExpansionState { MacroWork = PreprocessorLimits.MaximumMacroWork - 1 };
        var input = new IdlInput("macro.idl", string.Empty);

        // Act
        state.CountOperation(input, 0, null, 0);
        var exception = Should.Throw<IdlException>(() => state.CountOperation(input, 0, null, 0));

        // Assert
        state.MacroWork.ShouldBe(PreprocessorLimits.MaximumMacroWork + 1);
        exception.Message.ShouldContain("operation limit");
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsDepthFailureAtOffsetZeroThroughTheSourceOffsetMap()
    {
        // Arrange
        var state = new PreprocessorExpansionState();
        var input = new IdlInput("macro.idl", "original");

        // Act
        var exception = Should.Throw<IdlException>(() =>
            state.CountOperation(input, 0, [6], PreprocessorLimits.MaximumMacroDepth + 1));

        // Assert
        exception.Offset.ShouldBe(6);
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void FallsBackToTheExpandedOffsetWhenTheSourceMapDoesNotContainIt()
    {
        // Arrange
        var state = new PreprocessorExpansionState();
        var input = new IdlInput("macro.idl", "original");

        // Act
        var exception = Should.Throw<IdlException>(() =>
            state.CountOperation(input, 1, [6], PreprocessorLimits.MaximumMacroDepth + 1));

        // Assert
        exception.Offset.ShouldBe(1);
    }
}
