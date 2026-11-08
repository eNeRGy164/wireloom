using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Generation.Tests;

public sealed class FloatingDefaultSpecs
{
    [Fact]
    public void PreservesNegativeZeroInMemberAliasNativeAndMetadataDefaults()
    {
        // Arrange
        var input = Input("negative-zero-defaults.idl", """
            @default(-0.0) typedef double NegativeZero;
            @default(-0.0) typedef float NegativeFloatZero;
            struct Sample { @default(-0.0) double value; NegativeZero inherited; NegativeFloatZero singleValue; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = -0.0D;");
        documents["NegativeZero.g.cs"].Source.ShouldContain("Value = -0.0D;");
        documents["Implementation.SampleUnmanaged.g.cs"].Source.ShouldContain("value = -0.0D;");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("Float64Value = -0.0D");
        documents["Implementation.NegativeZeroPlugin.g.cs"].Source.ShouldContain("Float64Value = -0.0D");
        documents["NegativeFloatZero.g.cs"].Source.ShouldContain("Value = -0F;");
    }

    [Fact]
    public void FormatsDefaultsForNormalSignedAndUnsigned64BitValues()
    {
        // Arrange
        var input = Input("wide-integer-defaults.idl", """
            @default(7) typedef long long Signed;
            @default(9) typedef unsigned long long Unsigned;
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Signed.g.cs"].Source.ShouldContain("Value = 7L;");
        documents["Unsigned.g.cs"].Source.ShouldContain("Value = 9UL;");
    }
}
