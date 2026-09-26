using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlConstantSpecs
{
    [Fact]
    [Trait("Corpus", "C004")]
    public void EmitsTypedIntegralConstantsIncludingSigned64Extrema()
    {
        // Arrange
        var input = Input("02-names-constants.idl",
            """
            module Constants {
                const short shortValue = -32768;
                const unsigned long unsignedValue = 4294967295;
                const long longValue = -2147483648;
                const long long longLongMin = -9223372036854775807 - 1;
                const long long longLongMax = 9223372036854775807;
                const int64 int64Max = 9223372036854775807;
                const uint64 uint64Max = 18446744073709551615;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var shortValue = documents["Constants.shortValue.g.cs"].Source;
        shortValue.ShouldContain("public const short Value = -32768;");

        var unsignedValue = documents["Constants.unsignedValue.g.cs"].Source;
        unsignedValue.ShouldContain("public const uint Value = 4294967295U;");

        var longLongMin = documents["Constants.longLongMin.g.cs"].Source;
        longLongMin.ShouldContain("public const long Value = long.MinValue;");

        var longLongMax = documents["Constants.longLongMax.g.cs"].Source;
        longLongMax.ShouldContain("public const long Value = 9223372036854775807L;");

        var uint64Max = documents["Constants.uint64Max.g.cs"].Source;
        uint64Max.ShouldContain("public const ulong Value = ulong.MaxValue;");
    }

    [Fact]
    [Trait("Corpus", "C006")]
    public void ResolvesConstantExpressionsInCollectionAndStringBounds()
    {
        // Arrange
        var input = Input("02-constant-expressions.idl",
            """
            module Constants {
                const long Bound = (2 + 1) << 1;
                typedef sequence<long, Bound> Values;
                typedef long Samples[Bound - 1];
                struct Sample {
                    string<Bound> name;
                    long values[Bound];
                    Values sequenceValues;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var samplePlugin = documents["Constants.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("CreateString(6)");
        samplePlugin.ShouldContain("new uint[] { 6 }");

        var valuesPlugin = documents["Constants.Implementation.ValuesPlugin.g.cs"].Source;
        valuesPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf");

        var samplesPlugin = documents["Constants.Implementation.SamplesPlugin.g.cs"].Source;
        samplesPlugin.ShouldContain("new uint[] { 5 }");
    }

    [Fact]
    public void RejectsIntegralConstantsOutsideTheirIdlRange()
    {
        // Arrange
        var input = Input("constant-range.idl", "const long long TooLarge = 9223372036854775808;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its representable range");
    }
}
