using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP008")]
[Trait("Preprocessor", "PP009")]
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

    [Fact]
    public void HonorsCancellationDuringLongReplacementScans()
    {
        // Arrange
        var input = Input("expansion.idl", "__has_include(\"missing.idl\") " + new string('x', 100_000));
        using var cancellation = new CancellationTokenSource();
        var preprocessor = new IdlPreprocessor([], [], cancellation.Token);

        // Act
        Action act = () => preprocessor.ProcessWithMetadata(
            input,
            (_, _, _) => { },
            includeProbe: (_, _) =>
            {
                cancellation.Cancel();
                return false;
            });

        // Assert
        Should.Throw<OperationCanceledException>(act);
    }
}
