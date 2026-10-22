using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class ReviewAnnotationCompositionSpecs
{
    [Theory]
    [InlineData("key", "false")]
    [InlineData("key", "true")]
    [InlineData("optional", "false")]
    [InlineData("optional", "true")]
    [InlineData("external", "false")]
    [InlineData("external", "true")]
    [InlineData("must_understand", "false")]
    [InlineData("must_understand", "true")]
    public void BooleanAnnotationArgumentsHaveTheSameMeaningInEitherCase(string annotation, string value)
    {
        // Arrange
        var lower = Input("annotations.idl", $"struct Sample {{ @{annotation}({value}) long value; }};");
        var upper = Input("annotations.idl", $"struct Sample {{ @{annotation}({value.ToUpperInvariant()}) long value; }};");

        // Act
        var expected = Compile(lower);
        var actual = Compile(upper);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("@topic @autoid(HASH) @autoid(SEQUENTIAL) struct Sample { long value; };")]
    [InlineData("@autoid(HASH) @topic @autoid(SEQUENTIAL) struct Sample { long value; };")]
    [InlineData("@appendable @autoid(HASH) @autoid(SEQUENTIAL) union Sample switch(long) { case 0: long value; };")]
    public void RejectsDuplicateAutoIdThroughoutMixedPrefixLists(string source)
    {
        // Arrange
        var input = Input("duplicate-autoid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Duplicate @autoid");
    }

    [Theory]
    [InlineData("@appendable", "union Sample switch(long) { case 0: long value; };")]
    [InlineData("@nested(FALSE) @mutable", "struct Sample { long value; };")]
    public void AutoIdComposesWithOtherAggregateAnnotations(string prefix, string declaration)
    {
        // Arrange
        var first = Input("composition.idl", $"@autoid(HASH) {prefix} {declaration}");
        var last = Input("composition.idl", $"{prefix} @autoid(HASH) {declaration}");

        // Act
        var expected = Compile(first);
        var actual = Compile(last);

        // Assert
        actual.ShouldBe(expected);
    }
}
