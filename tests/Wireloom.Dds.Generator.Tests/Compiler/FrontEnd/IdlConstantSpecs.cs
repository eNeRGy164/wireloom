namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlConstantSpecs
{
    [Fact]
    [Trait("Corpus", "C004")]
    public void EmitsTypedIntegralConstantsIncludingSigned64Extrema()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C004-02-names-constants.idl",
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
                """
            )
        ], TestContext.Current.CancellationToken);

        documents["Constants.shortValue.g.cs"].Source.ShouldContain("public const short Value = -32768;");
        documents["Constants.unsignedValue.g.cs"].Source.ShouldContain("public const uint Value = 4294967295U;");
        documents["Constants.longLongMin.g.cs"].Source.ShouldContain("public const long Value = long.MinValue;");
        documents["Constants.longLongMax.g.cs"].Source.ShouldContain("public const long Value = 9223372036854775807L;");
        documents["Constants.uint64Max.g.cs"].Source.ShouldContain("public const ulong Value = ulong.MaxValue;");
    }

    [Fact]
    [Trait("Corpus", "C006")]
    public void ResolvesConstantExpressionsInCollectionAndStringBounds()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input(
                "C006-02-constant-expressions.idl",
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
                """
            )
        );

        output.ShouldContain("CreateString(6)");
        output.ShouldContain("new uint[] { 6 }");
        output.ShouldContain("CreateSequenceWithAccessInfo(dtf");
        output.ShouldContain("new uint[] { 5 }");
    }

    [Fact]
    public void RejectsIntegralConstantsOutsideTheirIdlRange()
    {
        var exception = Should.Throw<IdlException>(() => CompilerTestSupport.Compile(
            CompilerTestSupport.Input("constant-range.idl", "const long long TooLarge = 9223372036854775808;")));

        exception.Message.ShouldContain("outside its representable range");
    }
}
