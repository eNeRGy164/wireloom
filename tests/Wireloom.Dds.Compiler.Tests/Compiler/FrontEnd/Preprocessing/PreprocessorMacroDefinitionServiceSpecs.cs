namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP006")]
public sealed class PreprocessorMacroDefinitionServiceSpecs
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("9INVALID body")]
    public void RejectsDefinitionsWithoutAnIdentifierStart(string definition)
    {
        // Arrange
        var service = new PreprocessorMacroDefinitionService(
            new PreprocessorMacroTable(),
            new PreprocessorMacroTokenService());

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Define(new IdlInput("macro.idl", string.Empty), 4, definition));

        // Assert
        exception.Message.ShouldContain("Malformed #define directive");
    }

    [Fact]
    public void DefinesObjectMacroEndingAtInputEndAndSkipsTrailingWhitespace()
    {
        // Arrange
        var macros = new PreprocessorMacroTable();
        var service = new PreprocessorMacroDefinitionService(macros, new PreprocessorMacroTokenService());
        var input = new IdlInput("macro.idl", string.Empty);

        // Act
        service.Define(input, 0, "FLAG");
        service.Define(input, 0, "  EMPTY   ");

        // Assert
        macros.TryGetValue("FLAG", out var flag).ShouldBeTrue();
        flag.Parameters.ShouldBeNull();
        flag.Body.ShouldBeEmpty();
        macros.TryGetValue("EMPTY", out var empty).ShouldBeTrue();
        empty.Parameters.ShouldBeNull();
        empty.Body.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("EMPTY() body", "", "")]
    [InlineData("LOG(format, args...) format", "format,args", "args")]
    [InlineData("CALL(format, ...) format", "format,__VA_ARGS__", "__VA_ARGS__")]
    [Trait("Preprocessor", "PP007")]
    [Trait("Preprocessor", "PP015")]
    [Trait("Preprocessor", "PP019")]
    public void ParsesEmptyAndNamedVariadicParameterLists(string definition, string expectedParameters, string expectedVariadicName)
    {
        // Arrange
        var macros = new PreprocessorMacroTable();
        var service = new PreprocessorMacroDefinitionService(macros, new PreprocessorMacroTokenService());

        // Act
        service.Define(new IdlInput("macro.idl", string.Empty), 0, definition);
        var name = definition[..definition.IndexOf('(')];
        macros.TryGetValue(name, out var macro).ShouldBeTrue();

        // Assert
        macro.Parameters.ShouldBe(expectedParameters.Length == 0 ? [] : expectedParameters.Split(','));
        macro.Variadic.ShouldBe(expectedVariadicName.Length != 0);
        macro.VariadicParameterName.ShouldBe(expectedVariadicName.Length == 0 ? null : expectedVariadicName);
    }
}
