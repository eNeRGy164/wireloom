using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

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

    [Fact]
    public void EvaluatesBitwiseUnaryArithmeticAndQualifiedExpressions()
    {
        // Arrange
        var input = Input("constant-operators.idl",
            """
            module Constants {
                const long long Base = 7;
                const long long Arithmetic = (Base * 3 / 2) % 5;
                const long long Bits = 1 | 2 ^ 3 & 1;
                const long long Shift = 8 >> 2;
                const long long Complement = ~1;
                const long long Hex = 0x10;
                const long long Qualified = ::Constants::Base;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var arithmetic = documents["Constants.Arithmetic.g.cs"].Source;
        arithmetic.ShouldContain("public const long Value = 0L;");

        var bits = documents["Constants.Bits.g.cs"].Source;
        bits.ShouldContain("public const long Value = 3L;");

        var shift = documents["Constants.Shift.g.cs"].Source;
        shift.ShouldContain("public const long Value = 2L;");

        var complement = documents["Constants.Complement.g.cs"].Source;
        complement.ShouldContain("public const long Value = -2L;");

        var hex = documents["Constants.Hex.g.cs"].Source;
        hex.ShouldContain("public const long Value = 16L;");

        var qualified = documents["Constants.Qualified.g.cs"].Source;
        qualified.ShouldContain("public const long Value = 7L;");
    }

    [Fact]
    public void RejectsDivisionByZeroInConstantExpressions()
    {
        var exception = Should.Throw<IdlException>(() => Compile(Input("constant-division.idl", "const long Value = 1 / 0;")));

        exception.Message.ShouldContain("Division by zero");
    }

    [Fact]
    public void RejectsNegativeConstantExpressionShiftCounts()
    {
        var exception = Should.Throw<IdlException>(() => Compile(Input("constant-shift.idl", "const long Value = 1 << -1;")));

        exception.Message.ShouldContain("Shift count is outside");
    }

    [Fact]
    public void RejectsUnknownIntegralConstants()
    {
        var exception = Should.Throw<IdlException>(() => Compile(Input("constant-name.idl", "const long Value = Missing;")));

        exception.Message.ShouldContain("Unknown integral constant");
    }

    [Fact]
    public void RejectsMalformedConstantExpressions()
    {
        var exception = Should.Throw<IdlException>(() => Compile(Input("constant-token.idl", "const long Value = 1 + );")));

        exception.Message.ShouldContain("Expected an integer literal");
    }
}
