using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.IdlDeclarations.Tests;

/// <summary>Checks formatter selection while preserving RTI aggregate support.</summary>
public sealed class AliasFormattingSpecs
{
    /// <summary>Checks string annotations needed to initialize the original RTI support instance.</summary>
    [Theory]
    [InlineData("string", "String")]
    [InlineData("string<16>", "String")]
    [InlineData("wstring", "WideString")]
    [InlineData("wstring<16>", "WideString")]
    public void PreservesTheStringWidthInAliasAnnotations(string target, string kind)
    {
        // Arrange
        var input = Input("string-annotations.idl", $$"""
            module Example { typedef {{target}} Text; };
            """);

        // Act
        var source = CompileSources(input)["Example.Implementation.TextPlugin.g.cs"].Source;

        // Assert
        source.ShouldContain($"TypeKind.{kind},");
        source.ShouldContain($"defaultValue: new AnnotationParameterValue {{ {kind}Value = \"\" }}");
    }

    /// <summary>Checks that root aliases select formatting according to their resolved value shape.</summary>
    [Theory]
    [InlineData("typedef long Sample;", "global::System.Convert.ToString(sample.Value, global::System.Globalization.CultureInfo.InvariantCulture)")]
    [InlineData("typedef long double Sample;", "sample.Value.ToDecimal().ToString(global::System.Globalization.CultureInfo.InvariantCulture)")]
    [InlineData("typedef string<16> Sample;", "sample.Value ?? \"null\"")]
    [InlineData("enum State { Ready }; typedef State Sample;", "sample.Value.ToString()")]
    [InlineData("struct Child { long id; }; typedef Child Sample;", "SampleSupport.Instance.ToString(this)")]
    [InlineData("union Choice switch(long) { case 0: long number; }; typedef Choice Sample;", "SampleSupport.Instance.ToString(this)")]
    [InlineData("typedef sequence<long, 4> Items; typedef Items Sample;", "sample.Value?.ToString() ?? \"null\"")]
    public void SelectsTheFormatterForTheUnderlyingShape(string declarations, string expected)
    {
        // Arrange
        var input = Input("formatting.idl", $$"""
            module Example { {{declarations}} };
            """);

        // Act
        var sources = CompileSources(input);
        var source = sources["Example.Sample.g.cs"].Source;
        var support = sources["Example.SampleSupport.g.cs"].Source;

        // Assert
        if (expected.Contains("Support.Instance", StringComparison.Ordinal))
        {
            source.ShouldContain($"public override string ToString() => {expected};");
        }
        else
        {
            source.ShouldContain("public override string ToString() => SampleSupport.Instance.ToString(this);");
            support.ShouldContain("public new string ToString(Sample sample)");
            support.ShouldContain($"return {expected};");
        }
    }

    /// <summary>Checks that alias classes delegate to typed instance formatters in the existing support document.</summary>
    [Theory]
    [InlineData("sequence<string<16>, 3>", "")]
    [InlineData("string<16>", "[2]")]
    public void UsesATypeSpecificPrivateStringFormatter(string target, string dimensions)
    {
        // Arrange
        var input = Input("string-formatting.idl", $$"""
            module Example { typedef {{target}} FormatString{{dimensions}}; };
            """);

        // Act
        var sources = CompileSources(input);
        var source = sources["Example.FormatString.g.cs"].Source;
        var helper = sources["Example.FormatStringSupport.g.cs"].Source;

        // Assert
        source.ShouldContain("public override string ToString() => FormatStringSupport.Instance.ToString(this);");
        source.ShouldNotContain("StringBuilder");
        helper.ShouldContain("private static string FormatString(string? value)");
        helper.ShouldContain("text.Append(FormatString(value[");
        helper.ShouldContain("public new string ToString(FormatString sample)");
    }

    /// <summary>Checks string formatting through named scalar aliases in collection elements.</summary>
    [Fact]
    public void FormatsStringAliasesInsideCollections()
    {
        // Arrange
        var input = Input("string-alias-collections.idl", """
            module Example {
                typedef string Text;
                typedef Text Label;
                typedef sequence<Text, 4> Texts;
                typedef sequence<Label, 4> Labels;
            };
            """);

        // Act
        var sources = CompileSources(input);

        // Assert
        sources["Example.TextsSupport.g.cs"].Source.ShouldContain("FormatString(value[index]?.Value)");
        sources["Example.LabelsSupport.g.cs"].Source.ShouldContain("FormatString(value[index]?.Value)");
    }

}
