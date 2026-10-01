using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private static readonly Regex DirectivePattern = new(
        @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$",
        RegexOptions.Compiled);

    private static readonly Regex IncludePattern = new(
        @"^(?:""(?<quoted>[^""]+)""|<(?<angle>[^>]+)>)\s*$",
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
