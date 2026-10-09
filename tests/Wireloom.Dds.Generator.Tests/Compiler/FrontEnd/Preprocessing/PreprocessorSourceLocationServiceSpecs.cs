using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP050")]
public sealed class PreprocessorSourceLocationServiceSpecs
{
    [Fact]
    public void UsesMappedOffsetsAndFallsBackToInputEndPastTheMap()
    {
        // Arrange
        var service = new PreprocessorSourceLocationService(
            () => Input("mapped.idl", "source"),
            () => [4, 2],
            () => 0);

        // Act
        var first = service.OriginalOffset(0);
        var mapped = service.OriginalOffset(1);
        var pastEnd = service.OriginalOffset(2);

        // Assert
        first.ShouldBe(4);
        mapped.ShouldBe(2);
        pastEnd.ShouldBe(6);
    }

    [Fact]
    public void LeavesOffsetsUnchangedWithoutAMapOrForNegativeOffsets()
    {
        // Arrange
        var service = new PreprocessorSourceLocationService(
            () => Input("plain.idl", "source"),
            () => null,
            () => 0);
        var mapped = new PreprocessorSourceLocationService(
            () => Input("mapped.idl", "source"),
            () => [1],
            () => 0);

        // Act
        var withoutMap = service.OriginalOffset(12);
        var negative = mapped.OriginalOffset(-3);

        // Assert
        withoutMap.ShouldBe(12);
        negative.ShouldBe(-3);
    }

    [Theory]
    [InlineData(-2, 1)]
    [InlineData(0, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(6, 3)]
    [InlineData(50, 3)]
    public void ClampsOffsetsAndFindsContainingPhysicalLine(int offset, int expected)
    {
        // Arrange
        var service = new PreprocessorSourceLocationService(
            () => Input("lines.idl", "ab\ncd\nef"),
            () => null,
            () => 0);

        // Act
        var line = service.GetPhysicalLineNumber(offset, [0, 3, 6]);

        // Assert
        line.ShouldBe(expected);
    }

    [Fact]
    public void AddsLogicalLineOffsetToThePhysicalLine()
    {
        // Arrange
        var service = new PreprocessorSourceLocationService(
            () => Input("logical.idl", "ab\ncd\nef"),
            () => null,
            () => 8);

        // Act
        var line = service.GetLogicalLineNumber(4, [0, 3, 6]);

        // Assert
        line.ShouldBe(10);
    }
}
