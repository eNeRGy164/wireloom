namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP008")]
[Trait("Preprocessor", "PP009")]
[Trait("Preprocessor", "PP015")]
[Trait("Preprocessor", "PP019")]
public sealed class PreprocessorMacroArgumentBinderSpecs
{
    [Fact]
    public void PrescansFixedArgumentsAtTheNextExpansionDepthAndRestoresDisabledMacroState()
    {
        // Arrange
        const string macroName = "CALL";
        var expanding = new HashSet<string>(StringComparer.Ordinal) { macroName };
        var observedDepth = -1;
        var binder = new PreprocessorMacroArgumentBinder(
            (_, currentExpanding, depth, _) =>
            {
                currentExpanding.Contains(macroName).ShouldBeFalse();
                observedDepth = depth;
                return "expanded";
            },
            (_, _) => true);
        var macro = new PreprocessorMacro(["value"], "value", variadic: false);

        // Act
        var substitutions = binder.Bind(macroName, macro, [" raw "], expanding, depth: 7, offset: 11);

        // Assert
        observedDepth.ShouldBe(8);
        substitutions["value"].Raw.ShouldBe("raw");
        substitutions["value"].Expanded.ShouldBe("expanded");
        expanding.ShouldContain(macroName);
    }

    [Fact]
    public void PrescansVariadicArgumentsWhenTheNamedParameterIsExpanded()
    {
        // Arrange
        var observedDepth = -1;
        var observedArgument = string.Empty;
        var binder = new PreprocessorMacroArgumentBinder(
            (argument, _, depth, _) =>
            {
                observedArgument = argument;
                observedDepth = depth;
                return "expanded-list";
            },
            (_, parameter) => parameter == "rest");
        var macro = new PreprocessorMacro(["head", "rest"], "rest", variadic: true, variadicParameterName: "rest");

        // Act
        var substitutions = binder.Bind("PACK", macro, ["head", "first", "second"], [], depth: 4, offset: 17);

        // Assert
        observedArgument.ShouldBe("first, second");
        observedDepth.ShouldBe(5);
        substitutions["rest"].Raw.ShouldBe("first, second");
        substitutions["rest"].Expanded.ShouldBe("expanded-list");
        substitutions["__VA_ARGS__"].ShouldBe(substitutions["rest"]);
    }
}
