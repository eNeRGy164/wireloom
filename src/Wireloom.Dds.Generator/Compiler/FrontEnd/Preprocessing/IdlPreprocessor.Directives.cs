using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Processes preprocessor directives for the IDL source.</summary>
internal sealed partial class IdlPreprocessor
{
    private static readonly Regex DirectivePattern = new(
        @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$",
        RegexOptions.Compiled);

    private void ProcessDirective(
        Match directive,
        IdlInput input,
        int offset,
        int sourceOffset,
        Action<string, bool, int> include,
        PreprocessorConditionalState conditionals,
        ref bool hasPragmaOnce,
        Action? pragmaOnceEncountered,
        StringBuilder output,
        int lineLength)
    {
        directiveProcessor.Process(
            directive,
            input,
            offset,
            sourceOffset,
            include,
            conditionals,
            ref hasPragmaOnce,
            pragmaOnceEncountered,
            output,
            lineLength);
    }
}
