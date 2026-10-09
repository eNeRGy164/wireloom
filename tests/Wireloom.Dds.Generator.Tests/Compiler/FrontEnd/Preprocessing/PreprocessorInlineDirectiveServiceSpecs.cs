namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorInlineDirectiveServiceSpecs
{
    [Theory]
    [InlineData("(\"types.idl\")", "types.idl", false)]
    [InlineData("(<types.idl>)", "types.idl", true)]
    [Trait("Preprocessor", "PP048")]
    public void ExpandsQuotedAndAngledHasIncludeProbes(string text, string expectedName, bool expectedAngle)
    {
        // Arrange
        string? includedName = null;
        var angled = false;
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            (name, isAngled) =>
            {
                includedName = name;
                angled = isAngled;
                return true;
            },
            null);
        var index = 0;

        // Act
        var expanded = service.TryExpandHasInclude(text, ref index, out var result);

        // Assert
        expanded.ShouldBeTrue();
        result.ShouldBeTrue();
        index.ShouldBe(text.Length);
        includedName.ShouldBe(expectedName);
        angled.ShouldBe(expectedAngle);
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void ConsumesHasIncludeWithUnsupportedArgumentShapeAsFalse()
    {
        // Arrange
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            (_, _) => true,
            null);
        var index = 0;

        // Act
        var text = "(\"a.idl\", \"b.idl\")";
        var expanded = service.TryExpandHasInclude(text, ref index, out var result);

        // Assert
        expanded.ShouldBeTrue();
        result.ShouldBeFalse();
        index.ShouldBe(text.Length);
    }

    [Theory]
    [InlineData("_Pragma(\"once\")")]
    [InlineData("__pragma(once)")]
    [Trait("Preprocessor", "PP035")]
    [Trait("Preprocessor", "PP042")]
    [Trait("Preprocessor", "PP047")]
    public void ConsumesPragmaOnceFormsAndInvokesCallback(string text)
    {
        // Arrange
        var count = 0;
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            null,
            () => count++);
        var index = 0;

        // Act
        var consumed = service.TryConsumePragma(text, ref index);

        // Assert
        consumed.ShouldBeTrue();
        index.ShouldBe(text.Length);
        count.ShouldBe(1);
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    [Trait("Preprocessor", "PP047")]
    public void LeavesIdentifierSuffixAndMalformedPragmaUnconsumed()
    {
        // Arrange
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            null,
            null);
        var suffixIndex = 1;
        var malformedIndex = 0;

        // Act
        var suffix = service.TryConsumePragma("x_Pragma(\"once\")", ref suffixIndex);
        var malformed = service.TryConsumePragma("_Pragma \"once\"", ref malformedIndex);

        // Assert
        suffix.ShouldBeFalse();
        suffixIndex.ShouldBe(1);
        malformed.ShouldBeFalse();
        malformedIndex.ShouldBe(0);
    }

    [Fact]
    [Trait("Preprocessor", "PP047")]
    public void ConsumesPragmaStartingAfterNonIdentifierPrefix()
    {
        // Arrange
        var count = 0;
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            null,
            () => count++);
        var text = "x _Pragma(\"once\")";
        var index = 2;

        // Act
        var consumed = service.TryConsumePragma(text, ref index);

        // Assert
        consumed.ShouldBeTrue();
        index.ShouldBe(text.Length);
        count.ShouldBe(1);
    }

    [Theory]
    [InlineData("_Pragma")]
    [InlineData("_Pragma   ")]
    [Trait("Preprocessor", "PP047")]
    public void LeavesPragmaNameWithoutInvocationUnconsumed(string text)
    {
        // Arrange
        var service = new PreprocessorInlineDirectiveService(
            new PreprocessorMacroTokenService(),
            value => value,
            null,
            null);
        var index = 0;

        // Act
        var consumed = service.TryConsumePragma(text, ref index);

        // Assert
        consumed.ShouldBeFalse();
        index.ShouldBe(0);
    }
}
