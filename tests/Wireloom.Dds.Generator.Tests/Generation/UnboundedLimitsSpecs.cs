using Wireloom;

namespace Wireloom.Generation.Tests;

/// <summary>Protects configured effective limits in the public compiler API.</summary>
public sealed class UnboundedLimitsSpecs
{
    /// <summary>Uses each declaring input's limits when a file is included from another file.</summary>
    [Fact]
    public void AppliesPerInputLimitsToIncludedDeclarations()
    {
        // Arrange
        var root = new IdlInput(
            "root.idl",
            "#include \"shared.idl\"\nmodule Local { struct Root { sequence<long> values; string text; Shared shared; }; };",
            sequenceSize: 7,
            stringSize: 19);
        var shared = new IdlInput(
            "shared.idl",
            "struct Shared { sequence<long> values; string text; };",
            generate: false,
            sequenceSize: 3,
            stringSize: 5);

        // Act
        var generated = IdlCompiler.CompileSources([root, shared], TestContext.Current.CancellationToken);

        // Assert
        generated["Local.Root.g.cs"].Source.ShouldContain("effective limit of 7 elements");
        generated["Local.Root.g.cs"].Source.ShouldContain("effective limit of 19 UTF-8 bytes");
        generated["Shared.g.cs"].Source.ShouldContain("effective limit of 3 elements");
        generated["Shared.g.cs"].Source.ShouldContain("effective limit of 5 UTF-8 bytes");
    }

    /// <summary>Rejects non-positive settings supplied through the public compiler API.</summary>
    [Theory]
    [InlineData(0, 255, "SequenceSize")]
    [InlineData(100, -1, "StringSize")]
    public void RejectsInvalidPerInputLimits(int sequenceSize, int stringSize, string settingName)
    {
        // Arrange
        var input = new IdlInput("sample.idl", "struct Sample { long value; };",
            sequenceSize: sequenceSize, stringSize: stringSize);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Message.ShouldContain(settingName);
    }
}
