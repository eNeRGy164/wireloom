using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlValidationSpecs
{
    [Theory]
    [InlineData("struct Bad { string<0> value; };", "String bound")]
    [InlineData("#error no\nstruct Bad { int32 value; };", "Preprocessor error")]
    [InlineData("@mutable enum Kind { A };", "Unsupported IDL syntax near '@mutable'")]
    public void ReportsUnsupportedSyntaxAtItsOriginalSource(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("bad.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
        exception.Input.Path.ShouldEndWith("bad.idl");
    }

    [Fact]
    public void ReportsNestedUnsupportedSyntaxAtItsOriginalSourceOffset()
    {
        // Arrange
        var input = Input("nested-invalid.idl",
            "module Outer { module Inner { nonsense; }; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Offset.ShouldBe(input.Text.IndexOf("nonsense", System.StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsTopicAnnotationsOnModules()
    {
        // Arrange
        var input = Input("topic-module.idl",
            "@topic module Invalid { struct Sample { long value; }; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unsupported IDL syntax near 'module'");
    }

    [Fact]
    public void ReportsUnknownTypedefTargets()
    {
        // Arrange
        var input = Input("unknown-typedef.idl",
            "module P03 { typedef Missing Alias; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown typedef target");
    }

    [Fact]
    public void ReportsTypedefAliasCycles()
    {
        // Arrange
        var input = Input("alias-cycle.idl",
            "module P03 { typedef B A; typedef A B; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Typedef alias cycle");
    }

    [Theory]
    [InlineData("enum Broken { RED, };", "Malformed enum member")]
    [InlineData("typedef sequence<Missing, 3> Values;", "Unknown typedef target")]
    [InlineData("struct Broken { Missing value; };", "Unknown struct type")]
    public void ReportsMalformedOrUnknownDeclarations(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("p03-invalid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
        exception.Input.Path.ShouldEndWith("p03-invalid.idl");
    }

    [Theory]
    [InlineData("enum Broken { FIRST @value(7), SECOND };", "Malformed enum member")]
    [InlineData("enum Broken { @value(7) FIRST = 8, SECOND };", "Combined @value")]
    [InlineData("enum Broken { @value(0x10) FIRST, SECOND };", "Malformed enum member")]
    [InlineData("enum Broken { FIRST = 2147483648, SECOND };", "signed Int32")]
    public void ReportsUnsupportedEnumValueFormsWithOriginalSource(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("enum-values-invalid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
        exception.Input.Path.ShouldEndWith("enum-values-invalid.idl");
        exception.Offset.ShouldBeGreaterThan(0);
    }

    [Theory]
    [InlineData("struct Broken { string<0> value; };", "String bound")]
    [InlineData("struct Broken { wstring<invalid> value; };", "String bound")]
    [InlineData("struct Broken { wstring<-1> value; };", "Unsupported member")]
    public void ReportsInvalidStringBounds(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("p04-invalid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
    }

    [Theory]
    [InlineData("struct Broken { @key string<0> name; };", "String bound")]
    [InlineData("struct Broken { @key string<-1> name; };", "Unsupported member")]
    public void ReportsInvalidStringKeyBounds(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("p08-invalid-string-key.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
        exception.Input.Path.ShouldEndWith("p08-invalid-string-key.idl");
    }

    [Theory]
    [InlineData("struct Broken { long values[0]; };", "Array dimensions")]
    [InlineData("struct Broken { sequence<Missing> values; };", "Unknown collection element type")]
    [InlineData("typedef sequence<long, 0> Values;", "Collection bound")]
    public void ReportsMalformedCollectionDeclarations(string source, string expectedDiagnostic)
    {
        // Arrange
        var input = Input("p05-invalid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedDiagnostic);
    }

    [Fact]
    public void ReportsUnknownAggregateBaseTypes()
    {
        // Arrange
        var input = Input("p06-invalid.idl",
            "struct Derived : Missing { long value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct base type");
    }

    [Fact]
    public void StrictValidationRejectsKeysDeclaredOnDerivedAggregates()
    {
        // Arrange
        var input = new IdlInput("p08-strict.idl",
            "struct Base { @key long tenant; }; struct Derived : Base { @key long localId; };",
            generate: true,
            strict: true);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("derived from a struct/valuetype can not contain @key fields");
        exception.Message.ShouldContain("strict validation");
        exception.Input.Path.ShouldEndWith("p08-strict.idl");
        exception.Input.Text.ShouldContain("@key long localId");
    }

    [Fact]
    public void DefaultValidationRetainsTheAcceptedDerivedKeyBehavior()
    {
        // Arrange
        var input = new IdlInput("p08-default.idl",
            "struct Base { @key long tenant; }; struct Derived : Base { @key long localId; };",
            generate: true,
            strict: false);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("DerivedPlugin");
    }

    [Fact]
    public void AcceptsNestedAnnotationAndRetainsCSharpTypeSupport()
    {
        // Arrange
        var input = Input("nested.idl",
            "module Example { @nested struct Value { long number; }; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public partial class Value");
        output.ShouldContain("ValueSupport");
        output.ShouldContain("ValuePlugin");
    }
}
