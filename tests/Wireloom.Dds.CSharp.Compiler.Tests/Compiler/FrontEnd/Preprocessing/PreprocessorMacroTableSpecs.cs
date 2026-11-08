namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP006")]
public sealed class PreprocessorMacroTableSpecs
{
    [Fact]
    public void StoresLooksUpAndRemovesMacroDefinitions()
    {
        // Arrange
        var table = new PreprocessorMacroTable();
        var macro = new PreprocessorMacro(["value"], "value", false);

        // Act
        table["IDENTITY"] = macro;
        var found = table.TryGetValue("IDENTITY", out var resolved);
        table.Remove("IDENTITY");

        // Assert
        found.ShouldBeTrue();
        resolved.ShouldBeSameAs(macro);
        table.ContainsKey("IDENTITY").ShouldBeFalse();
    }
}
