using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

/// <summary>Checks annotation composition independently of the order of parser handlers.</summary>
public sealed class AnnotationOrderingReviewSpecs
{
    [Theory]
    [InlineData("@custom @default(7) typedef long Count;")]
    [InlineData("@default(7) @custom typedef long Count;")]
    [InlineData("@default(7) @Vendor::custom(value=(1 + (2 * 3)), text=\"a),b\") typedef long Count;")]
    public void IgnoresCompleteUnknownApplicationsAmongSupportedAnnotations(string source)
    {
        // Arrange
        var input = Input("annotation-order.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldNotBeEmpty();
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
        var expectedOffset = source.Contains("@Vendor::", StringComparison.Ordinal)
            ? source.IndexOf("@Vendor::", StringComparison.Ordinal)
            : source.IndexOf("@custom", StringComparison.Ordinal);
        diagnostics.Single(diagnostic => diagnostic.Id == "DDSG0101").Offset.ShouldBe(expectedOffset);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("long value", false)]
    [InlineData("@", false)]
    [InlineData("@invalid:", false)]
    [InlineData("@Vendor::", false)]
    [InlineData("@custom(", false)]
    [InlineData("@custom(\"unterminated)", false)]
    [InlineData("@custom", true)]
    [InlineData("@custom  ", true)]
    [InlineData("@::Vendor::custom(value=(1 + (2)), text=\"),\") ", true)]
    public void ReadsCompleteAnnotationSyntaxWithoutInterpretingUnknownArguments(string source, bool expected)
    {
        // Arrange/Act
        var success = IdlSyntaxReader.TryReadAnnotationApplication(source, out _, out var length);

        // Assert
        success.ShouldBe(expected);
        if (success)
        {
            length.ShouldBe(source.Length);
        }
    }

    [Theory]
    [InlineData("@min(1) @default(FALSE) boolean value;")]
    [InlineData("@max(0) @default(TRUE) boolean value;")]
    [InlineData("@min(66) @default('A') char value;")]
    [InlineData("@max(64) @default(L'A') wchar value;")]
    [InlineData("@default('A') typedef char Letter; struct Inner { @min(66) Letter value; };")]
    public void RejectsTypedDefaultsOutsideDeclaredRanges(string member)
    {
        // Arrange
        var source = member.StartsWith("@default", StringComparison.Ordinal)
            ? member
            : $"struct Sample {{ {member} }};";
        var input = Input("typed-default-range.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its declared range");
    }
}
