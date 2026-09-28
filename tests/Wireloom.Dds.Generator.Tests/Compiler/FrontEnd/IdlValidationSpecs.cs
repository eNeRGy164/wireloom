using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

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
    [Trait("Preprocessor", "PP004")]
    [Trait("Preprocessor", "PP050")]
    public void MapsParserDiagnosticsAfterLineSplicingBackToOriginalOffsets()
    {
        // Arrange
        var input = Input("spliced-parser.idl",
            """
            const long Value = 1 + \
            2;
            struct Broken { Missing value; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type");
        exception.Offset.ShouldBe(input.Text.IndexOf("Missing", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsDiagnosticsBetweenMacroInvocationsToUnchangedSource()
    {
        // Arrange
        var input = Input("two-macro-origins.idl",
            """
            #define VALUE_TYPE long
            module First { struct Broken { VALUE_TYPE first; Unknown value; VALUE_TYPE second; }; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type");
        exception.Offset.ShouldBe(input.Text.IndexOf("Unknown", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsDiagnosticsAfterALongUnchangedRegionBetweenMacroInvocations()
    {
        // Arrange
        var fillerMembers = string.Join(
            " ",
            Enumerable.Range(0, 80).Select(index => $"long filler{index};"));
        var input = Input(
            "long-macro-origin-gap.idl",
            $"#define VALUE_TYPE long\nmodule First {{ struct Broken {{ VALUE_TYPE first; {fillerMembers} Unknown value; VALUE_TYPE second; }}; }};");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type");
        exception.Offset.ShouldBe(input.Text.IndexOf("Unknown", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsNestedParserDiagnosticsThroughMacroExpansion()
    {
        // Arrange
        var input = Input("nested-macro-offset.idl",
            """
            #define MISSING_TYPE Missing
            module Outer { struct Broken { MISSING_TYPE value; }; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type");
        exception.Offset.ShouldBe(input.Text.LastIndexOf("MISSING_TYPE", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsDiagnosticsAfterLargeMacroExpansionToTheFollowingSourceToken()
    {
        // Arrange
        var fields = string.Join(" ", Enumerable.Range(0, 40).Select(index => $"long field{index};"));
        var source = $"#define MANY struct Generated {{ {fields} }};\nMANY struct Broken {{ Missing value; }};";
        var input = Input("large-macro-origin.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type");
        exception.Offset.ShouldBe(input.Text.IndexOf("Missing", StringComparison.Ordinal));
    }

    [Fact]
    public void IgnoresBracesInsideStringLiteralsWhenFindingModuleBoundaries()
    {
        // Arrange
        var input = Input("module-string-brace.idl",
            "module Outer { const string Marker = \"{}\"; struct Sample { long value; }; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public class Sample");
    }

    [Fact]
    public void IgnoresEscapedCharactersAndBracesInsideCharacterLiteralsWhenFindingModuleBoundaries()
    {
        // Arrange
        var input = Input("module-character-literal.idl",
            "module Outer { const char Backslash = '\\\\'; const wchar Brace = L'{'; struct Sample { long value; }; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public class Sample");
    }

    [Fact]
    public void RequiresASemicolonAfterAModuleDeclaration()
    {
        // Arrange
        var input = Input("module-without-semicolon.idl", "module Broken { struct Sample { long value; }; }");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Module declaration must end with a semicolon");
    }

    [Fact]
    public void IgnoresDeclarationLevelDataRepresentationAnnotations()
    {
        // Arrange
        var input = Input(
            "data-representation.idl",
            "@data_representation(XCDR2) module Example { struct Sample { long value; }; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public class Sample");
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

    [Fact]
    public void RejectsDuplicateMemberNames()
    {
        // Arrange
        var input = Input("duplicate-member.idl", "struct Broken { long value; long value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Duplicate member: value");
    }

    [Fact]
    public void RejectsOptionalAggregateMembers()
    {
        // Arrange
        var input = Input(
            "optional-aggregate.idl",
            "struct Payload { long value; }; struct Holder { @optional Payload payload; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Optional aggregate members are not supported yet");
    }

    [Theory]
    [InlineData("@key @key long value;", "Duplicate @key annotation")]
    [InlineData("@optional @optional long value;", "Duplicate @optional annotation")]
    [InlineData("@id(1) @id(2) long value;", "Duplicate or invalid @id annotation")]
    [InlineData("@id(1) @hashid long value;", "Duplicate or conflicting @id/@hashid annotation")]
    [InlineData("@min(0) @min(1) long value;", "Duplicate @min or @range annotation")]
    [InlineData("@max(1) @max(2) long value;", "Duplicate @max or @range annotation")]
    [InlineData("@min(0) @range(min=0,max=1) long value;", "Duplicate @min, @max, or @range annotation")]
    [InlineData("@default(1) @default(2) long value;", "Duplicate @default annotation")]
    [InlineData("@unit(\"m\") @unit(\"s\") long value;", "Duplicate @unit annotation")]
    [InlineData("@external @external long value;", "Duplicate @external annotation")]
    [InlineData("@must_understand @must_understand long value;", "Duplicate @must_understand annotation")]
    public void RejectsDuplicateMemberAnnotations(string member, string expectedMessage)
    {
        // Arrange
        var input = Input("duplicate-member-annotation.idl", $"struct Broken {{ {member} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Theory]
    [InlineData("@default(0) Payload payload;", "only supported on primitive and enum members")]
    [InlineData("@min(0) Kind kind;", "require a primitive member")]
    public void RejectsValueMetadataOnUnsupportedMemberTypes(string member, string expectedMessage)
    {
        // Arrange
        var source = member.Contains("Kind", StringComparison.Ordinal)
            ? $"enum Kind {{ Zero }}; struct Broken {{ {member} }};"
            : $"struct Payload {{ long value; }}; struct Broken {{ {member} }};";
        var input = Input("unsupported-member-metadata.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Fact]
    public void RejectsMemberRangesWithTheMinimumAboveTheMaximum()
    {
        // Arrange
        var input = Input("reversed-member-range.idl", "struct Broken { @range(min=10,max=1) long value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("minimum value cannot be greater than its maximum value");
    }

    [Fact]
    public void RejectsUnknownEnumDefaultValues()
    {
        // Arrange
        var input = Input("unknown-enum-default.idl", "enum Kind { Zero }; struct Broken { @default(Missing) Kind kind; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown default enum value");
    }

    [Fact]
    public void RejectsInvalidMemberRangeExpressions()
    {
        // Arrange
        var input = Input("invalid-member-range.idl", "struct Broken { @min(not_an_expression) long value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Invalid minimum expression");
    }

    [Theory]
    [InlineData("struct Broken { Missing values[2]; };", "Unknown collection element type")]
    [InlineData("struct Broken { sequence<long, 1, 2> values; };", "Malformed sequence declaration")]
    public void RejectsMalformedMemberCollectionShapes(string source, string expectedMessage)
    {
        // Arrange
        var input = Input("malformed-member-collection.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Theory]
    [InlineData("union Choice switch(long) { };", "at least one branch")]
    [InlineData("union Choice switch(long) { default: long first; default: long second; };", "at most one default branch")]
    public void RejectsInvalidUnionBranchCounts(string source, string expectedMessage)
    {
        // Arrange
        var input = Input("invalid-union-count.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Theory]
    [InlineData("union Choice switch(long) { invalid; };", "Unsupported union branch declaration")]
    [InlineData("union Choice switch(long) { case 1: long value; case 2: long value; };", "Duplicate union branch")]
    [InlineData("union Choice switch(long) { case Missing: long value; };", "Unknown union discriminator label")]
    [InlineData("union Choice switch(char) { case '\\a': long value; };", "Unknown union discriminator label")]
    [InlineData("union Choice switch(char) { case '\\0': long value; };", "Unknown union discriminator label")]
    [InlineData("union Choice switch(long) { case 1: sequence<Missing> values; };", "Unknown union collection element type")]
    [InlineData("union Choice switch(long) { case 1: Missing value; };", "Unknown union branch type")]
    public void RejectsInvalidUnionBranches(string source, string expectedMessage)
    {
        // Arrange
        var input = Input("invalid-union-branch.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Fact]
    public void AcceptsTheSupportedCharacterUnionLabels()
    {
        // Arrange
        var input = Input(
            "character-union-labels.idl",
            "union Choice switch(char) { case '\\n': long newline; case '\\r': long carriage; case '\\t': long tab; case '\\\\': long backslash; case '\\'': long apostrophe; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public class Choice");
    }
}
