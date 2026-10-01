using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorMacroExpansionSpecs
{
    [Fact]
    public void RescansNestedReplacementAndSuppressesDirectRecursion()
    {
        // Arrange
        var input = Input(
            "expansion.idl",
            "#define FIRST SECOND\n#define SECOND 42\n#define LOOP LOOP\nFIRST LOOP");

        // Act
        var result = new IdlPreprocessor([], []).Process(input, (_, _, _) => { });

        // Assert
        result.ShouldContain("42 LOOP");
    }

    [Fact]
    public void HonorsCancellationDuringExpansion()
    {
        // Arrange
        var input = Input("expansion.idl", "#define VALUE 42\nVALUE");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        // Act
        var exception = Should.Throw<OperationCanceledException>(() =>
            new IdlPreprocessor([], [], cancellation.Token).Process(input, (_, _, _) => { }));

        // Assert
        exception.ShouldNotBeNull();
    }
}
