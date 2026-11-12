using Wireloom.Dds.Cpp.Emission;

namespace Wireloom.Dds.Cpp.Compiler.Tests;

/// <summary>Checks deterministic layout primitives used for generated C++ source.</summary>
public sealed class SourceWriterSpecs
{
    [Fact]
    public void WritesLfIndentationBlocksIncludesCommentsAndDoxygen()
    {
        // Arrange
        var writer = new SourceWriter();

        // Act
        writer.WriteInclude("cstdint");
        writer.WriteInclude("sample.hpp", quoted: true);
        writer.WriteComment("generated");
        writer.WriteDoxygenSummary("IDL value type.");
        writer.WriteDoxygenParam("value", "Initial value.");
        writer.WriteDoxygenReturn("The result.");
        writer.WriteDoxygenSee("RTI documentation", "https://example.invalid/api");
        writer.WriteDoxygenCode(["Sample value{};"]);
        writer.OpenBlock("struct Sample");
        writer.WriteLine("int value;");
        writer.CloseBlock(";");

        // Assert
        Assert.Equal("#include <cstdint>\n#include \"sample.hpp\"\n// generated\n/// @brief IDL value type.\n/// @param value Initial value.\n/// @return The result.\n/// @see [RTI documentation](https://example.invalid/api)\n/// @code\n/// Sample value{};\n/// @endcode\nstruct Sample\n{\n    int value;\n};\n", writer.ToString());
        Assert.DoesNotContain('\r', writer.ToString());
        writer.WriteDoxygenSee(
            "RTI StructType API",
            "https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1xtypes_1_1StructType.html");
        writer.WriteDoxygenWrapped("@details", new string('x', 180));
        var lines = writer.ToString().Split('\n');
        Assert.True(lines.Where(line => !line.Contains("@see", StringComparison.Ordinal)).Max(line => line.Length) <= 120);
        var seeLine = Assert.Single(lines, line => line.Contains("@see [RTI StructType API](", StringComparison.Ordinal));
        Assert.Contains(
            "https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1xtypes_1_1StructType.html",
            seeLine);
        Assert.DoesNotContain('\\', seeLine);
        Assert.EndsWith(")", seeLine);
    }

    [Fact]
    public void WrapsDoxygenDetailsBetweenWordsAtTheConfiguredColumn()
    {
        // Arrange
        var writer = new SourceWriter();
        var text = new string('a', 80) + " " + new string('b', 40);

        // Act
        writer.WriteDoxygenWrapped("@details", text);

        // Assert
        Assert.Equal(
            "/// @details " + new string('a', 80) + "\n/// " + new string('b', 40) + "\n",
            writer.ToString());
        Assert.All(writer.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries),
            line => Assert.True(line.Length <= 116));
    }

    [Fact]
    public void KeepsShortDoxygenDetailsOnOneLine()
    {
        // Arrange
        var writer = new SourceWriter();

        // Act
        writer.WriteDoxygenWrapped("@details", "A short description.");

        // Assert
        Assert.Equal("/// @details A short description.\n", writer.ToString());
    }

    [Fact]
    public void WritesIndentedLinesAndNormalizesCarriageReturns()
    {
        // Arrange
        var writer = new SourceWriter();
        writer.OpenBlock("struct Sample");

        // Act
        writer.WriteDoxygenBrief("First and second values.");
        writer.WriteDoxygenThrows("sample_error", "When a value is invalid.", indentationLevel: 1);
        writer.WriteLines("int first;\r\nint second;\r\n");

        // Assert
        Assert.Equal(
            "struct Sample\n{\n"
            + "    /// @brief First and second values.\n"
            + "        /// @throws sample_error When a value is invalid.\n"
            + "    int first;\n    int second;\n\n",
            writer.ToString());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(29)]
    public void RejectsDoxygenIndentationThatLeavesNoRoomForText(int indentationLevel)
    {
        // Arrange
        var writer = new SourceWriter();

        // Act
        var exception = Record.Exception(
            () => writer.WriteDoxygenWrapped("@details", "text", indentationLevel));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void RejectsDoxygenCommandThatLeavesNoRoomForContent()
    {
        // Arrange
        var writer = new SourceWriter();
        var command = new string('x', 116);

        // Act
        var exception = Record.Exception(() => writer.WriteDoxygenWrapped(command, "text"));

        // Assert
        Assert.IsAssignableFrom<ArgumentException>(exception);
    }

    [Fact]
    public void SplitsLongDoxygenSeeReferencesWithoutBreakingTheVersionedUrl()
    {
        // Arrange
        var writer = new SourceWriter();
        const string url = "https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm#StringsAndWideStrings";

        // Act
        writer.WriteDoxygenSee("RTI string bounds (7.7.0)", url, indentationLevel: 1);

        // Assert
        var lines = writer.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Contains(lines, line => line.Contains("@see [RTI string bounds (7.7.0)](" + url + ")", StringComparison.Ordinal));
        Assert.Contains(lines, line => line.Contains("RTI string bounds (7.7.0)", StringComparison.Ordinal));
        Assert.All(lines, line => Assert.True(line.Length <= 320));
    }
}
