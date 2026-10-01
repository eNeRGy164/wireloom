namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

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
}
