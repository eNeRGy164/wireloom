namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorMacroTokenServiceSpecs
{
    [Fact]
    public void AppliesStringificationAndTokenPastingOutsideLiterals()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["name"] = ("hello world", "expanded")
        };

        // Act
        var stringified = service.TransformReplacement("#name", false, false, substitutions);
        var pasted = service.TransformReplacement("name ## suffix", false, false, substitutions);

        // Assert
        stringified.ShouldBe("\"hello world\"");
        pasted.ShouldBe("hello worldsuffix");
    }

    [Fact]
    public void ExpandsVariadicOptionalTokensAndReadsNestedArguments()
    {
        // Arrange
        var service = new PreprocessorMacroTokenService();
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>
        {
            ["__VA_ARGS__"] = ("one, two", "one, two")
        };

        // Act
        var replacement = service.TransformReplacement("prefix __VA_OPT__(, __VA_ARGS__)", true, true, substitutions);
        var parsed = service.TryReadArguments("(one, (two, three))", 0, out var arguments, out var end);

        // Assert
        replacement.ShouldBe("prefix , one, two");
        parsed.ShouldBeTrue();
        arguments.ShouldBe(["one", "(two, three)"]);
        end.ShouldBe(19);
    }
}
