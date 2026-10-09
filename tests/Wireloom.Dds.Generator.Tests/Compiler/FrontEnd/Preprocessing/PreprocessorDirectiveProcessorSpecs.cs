using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorDirectiveProcessorSpecs
{
    [Fact]
    [Trait("Preprocessor", "PP006")]
    [Trait("Preprocessor", "PP041")]
    public void DispatchesDefineAndLineThroughExplicitDependencies()
    {
        // Arrange
        var macros = new PreprocessorMacroTable();
        var defined = string.Empty;
        var lineOffset = 0;
        string? logicalFile = null;
        var processor = new PreprocessorDirectiveProcessor(
            macros,
            (_, _, _, _) => true,
            (text, _) => text,
            (_, _, text) => defined = text,
            (offset, file) =>
            {
                lineOffset = offset;
                logicalFile = file;
            },
            _ => 1,
            () => null);
        var input = new IdlInput("directive.idl", "#define VALUE 1\n#line 12 \"mapped.idl\"");
        var conditionals = new PreprocessorConditionalState();
        var output = new StringBuilder();
        var define = Regex.Match("#define VALUE 1", @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$");
        var line = Regex.Match("#line 12 \"mapped.idl\"", @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$");
        var hasPragmaOnce = false;

        // Act
        processor.Process(define, input, 0, 0, (_, _, _) => { }, conditionals, ref hasPragmaOnce, null, output, 17);
        processor.Process(line, input, 20, 20, (_, _, _) => { }, conditionals, ref hasPragmaOnce, null, output, 27);

        // Assert
        defined.ShouldBe("VALUE 1");
        lineOffset.ShouldBe(10);
        logicalFile.ShouldBe("mapped.idl");
        output.Length.ShouldBe(44);
    }

    [Fact]
    [Trait("Preprocessor", "PP021")]
    [Trait("Preprocessor", "PP050")]
    public void EvaluatesTrimmedConditionalAtItsOriginalSourceOffset()
    {
        // Arrange
        string? evaluatedText = null;
        var evaluatedOffset = -1;
        var processor = new PreprocessorDirectiveProcessor(
            new PreprocessorMacroTable(),
            (text, _, sourceOffset, _) =>
            {
                evaluatedText = text;
                evaluatedOffset = sourceOffset;
                return true;
            },
            (text, _) => text,
            (_, _, _) => { },
            (_, _) => { },
            _ => 1,
            () => null);
        var directive = Regex.Match("  #if    FEATURE", @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$");
        var input = new IdlInput("directive.idl", directive.Value);
        const int sourceOffset = 100;
        var conditionals = new PreprocessorConditionalState();
        var output = new StringBuilder();
        var hasPragmaOnce = false;

        // Act
        processor.Process(directive, input, 12, sourceOffset, (_, _, _) => { }, conditionals,
            ref hasPragmaOnce, null, output, directive.Length);

        // Assert
        evaluatedText.ShouldBe("FEATURE");
        evaluatedOffset.ShouldBe(sourceOffset + directive.Groups["rest"].Index + 4);
        conditionals.IsActive.ShouldBeTrue();
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void EmitsDiagnosticForPragmaMessageWithoutPayload()
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var processor = new PreprocessorDirectiveProcessor(
            new PreprocessorMacroTable(),
            (_, _, _, _) => true,
            (text, _) => text,
            (_, _, _) => { },
            (_, _) => { },
            _ => 1,
            () => diagnostics);
        var directive = Regex.Match("#pragma message", @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$");
        var input = new IdlInput("directive.idl", directive.Value);
        var conditionals = new PreprocessorConditionalState();
        var output = new StringBuilder();
        var hasPragmaOnce = false;

        // Act
        processor.Process(directive, input, 0, 0, (_, _, _) => { }, conditionals,
            ref hasPragmaOnce, null, output, directive.Length);

        // Assert
        diagnostics.ShouldContain(d => d.Id == "DDSG0107" && d.Message == "Preprocessor message: message");
    }
}
