using System.Numerics;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class IdlConstantRangeCoverageSpecs
{
    [Theory]
    [InlineData("int8", "-128", "127")]
    [InlineData("short", "-32768", "32767")]
    [InlineData("int16", "-32768", "32767")]
    [InlineData("long", "-2147483648", "2147483647")]
    [InlineData("int32", "-2147483648", "2147483647")]
    [InlineData("long long", "-9223372036854775808", "9223372036854775807")]
    [InlineData("int64", "-9223372036854775808", "9223372036854775807")]
    [InlineData("uint8", "0", "255")]
    [InlineData("octet", "0", "255")]
    [InlineData("unsigned short", "0", "65535")]
    [InlineData("uint16", "0", "65535")]
    [InlineData("unsigned long", "0", "4294967295")]
    [InlineData("uint32", "0", "4294967295")]
    [InlineData("unsigned long long", "0", "18446744073709551615")]
    [InlineData("uint64", "0", "18446744073709551615")]
    public void AcceptsExactMinimumAndMaximumForEveryIntegralIdlType(string type, string minimum, string maximum)
    {
        // Arrange
        var input = Input(
            "integral-boundaries.idl",
            $$"""
            const {{type}} Minimum = {{minimum}};
            const {{type}} Maximum = {{maximum}};
            """
        );

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.ShouldContainKey("Minimum.g.cs");
        documents.ShouldContainKey("Maximum.g.cs");
    }

    [Theory]
    [InlineData("int8", "-129")]
    [InlineData("int8", "128")]
    [InlineData("short", "-32769")]
    [InlineData("short", "32768")]
    [InlineData("int16", "-32769")]
    [InlineData("int16", "32768")]
    [InlineData("long", "-2147483649")]
    [InlineData("long", "2147483648")]
    [InlineData("int32", "-2147483649")]
    [InlineData("int32", "2147483648")]
    [InlineData("long long", "-9223372036854775809")]
    [InlineData("long long", "9223372036854775808")]
    [InlineData("int64", "-9223372036854775809")]
    [InlineData("int64", "9223372036854775808")]
    [InlineData("uint8", "-1")]
    [InlineData("uint8", "256")]
    [InlineData("octet", "-1")]
    [InlineData("octet", "256")]
    [InlineData("unsigned short", "-1")]
    [InlineData("unsigned short", "65536")]
    [InlineData("uint16", "-1")]
    [InlineData("uint16", "65536")]
    [InlineData("unsigned long", "-1")]
    [InlineData("unsigned long", "4294967296")]
    [InlineData("uint32", "-1")]
    [InlineData("uint32", "4294967296")]
    [InlineData("unsigned long long", "-1")]
    [InlineData("unsigned long long", "18446744073709551616")]
    [InlineData("uint64", "-1")]
    [InlineData("uint64", "18446744073709551616")]
    public void RejectsValuesImmediatelyOutsideEveryIntegralIdlRange(string type, string value)
    {
        // Arrange
        var input = Input(
            "integral-out-of-range.idl",
            $$"""
            const {{type}} Invalid = {{value}};
            """
        );

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain($"{type} constant value is outside its representable range");
    }

    [Fact]
    public void RejectsUnsupportedTypesWhenRangeValidationIsCalledDirectly()
    {
        // Arrange
        var input = Input("unsupported-integral-range.idl", "const boolean Flag = TRUE;");

        // Act
        var exception = Should.Throw<InvalidOperationException>(() =>
            IdlConstantParser.ValidateConstantRange(input, 0, "boolean", BigInteger.Zero));

        // Assert
        exception.Message.ShouldBe("Unsupported integral constant type: boolean");
    }
}
