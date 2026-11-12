using System.Security.Cryptography;

namespace Wireloom.Dds.Idl.Tests.Api;

public sealed class IdlAnnotationSpecs
{
    [Fact]
    public void ExposesAnnotationLocationAndParsedPositionalAndNamedArguments()
    {
        // Arrange
        const string annotationText = "@vendor_custom(42, enabled=true)";
        var source = $"{annotationText} struct Sample {{ long value; }};";
        var input = new IdlInput("annotation.idl", source);

        // Act
        var analysis = IdlAnalyzer.Analyze([input], MD5.HashData, TestContext.Current.CancellationToken);
        var annotation = analysis.Declarations.OfType<IdlClassDeclaration>()
            .ShouldHaveSingleItem()
            .Annotations.ShouldHaveSingleItem();
        var arguments = annotation.Arguments;

        // Assert
        annotation.Name.ShouldBe("vendor_custom");
        annotation.Offset.ShouldBe(source.IndexOf(annotationText, StringComparison.Ordinal));
        annotation.Length.ShouldBe(source.IndexOf("struct Sample", StringComparison.Ordinal) - annotation.Offset);
        source.Substring(annotation.Offset, annotation.Length).TrimEnd().ShouldBe(annotationText);
        arguments.Count.ShouldBe(2);
        arguments[0].Name.ShouldBeNull();
        arguments[0].Expression.ShouldBe("42");
        arguments[0].RelativeOffset.ShouldBe(annotationText.IndexOf("42", StringComparison.Ordinal));
        arguments[1].Name.ShouldBe("enabled");
        arguments[1].Expression.ShouldBe("true");
        arguments[1].RelativeOffset.ShouldBe(annotationText.IndexOf("enabled=true", StringComparison.Ordinal) + "enabled=".Length);
    }
}
