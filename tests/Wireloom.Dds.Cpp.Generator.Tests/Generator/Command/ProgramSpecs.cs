namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Verifies the native executable entry point delegates to its command.</summary>
public sealed class ProgramSpecs
{
    [Fact]
    public void MainWritesHelpToStandardOutput()
    {
        // Arrange
        var originalOutput = Console.Out;
        var originalErrors = Console.Error;
        using var output = new StringWriter();
        using var errors = new StringWriter();

        try
        {
            Console.SetOut(output);
            Console.SetError(errors);

            // Act
            var exitCode = Program.Main(["--help"]);

            // Assert
            exitCode.ShouldBe(0);
            output.ToString().ShouldContain("Wireloom.Dds.Cpp generate");
            errors.ToString().ShouldBeEmpty();
        }
        finally
        {
            Console.SetOut(originalOutput);
            Console.SetError(originalErrors);
        }
    }
}
