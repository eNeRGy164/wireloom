using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class PreprocessorDirectiveProcessorSpecs
{
    [Fact]
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
}
