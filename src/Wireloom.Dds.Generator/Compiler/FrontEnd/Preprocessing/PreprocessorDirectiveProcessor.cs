using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Dispatches preprocessing directives using explicit processor dependencies.</summary>
internal sealed class PreprocessorDirectiveProcessor(
    PreprocessorMacroTable macros,
    Func<string, IdlInput, int, int, bool> evaluateCondition,
    Func<string, int, string> expand,
    Action<IdlInput, int, string> define,
    Action<int, string?> setLogicalLocation,
    Func<int, int> getPhysicalLineNumber,
    Func<ICollection<IdlDiagnostic>?> diagnostics)
{
    private static readonly Regex IncludePattern = new(
        @"^(?:""(?<quoted>[^""]+)""|<(?<angle>[^>]+)>)\s*$",
        RegexOptions.Compiled);

    private static readonly Regex LinePattern = new(
        @"^(?<line>[0-9]+)(?:\s+""(?<file>[^""]*)""\s*)?$",
        RegexOptions.Compiled);

    internal void Process(
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
        var name = directive.Groups["name"].Value;
        var rawRest = directive.Groups["rest"].Value;
        var leadingWhitespace = rawRest.Length - rawRest.TrimStart().Length;
        var rest = rawRest.Trim();
        var restOffset = sourceOffset + directive.Groups["rest"].Index + leadingWhitespace;

        switch (name)
        {
            case "if":
                conditionals.Enter(conditionals.IsActive && evaluateCondition(rest, input, restOffset, offset));
                AppendBlankLine(output, lineLength);
                return;

            case "ifdef":
                if (!PreprocessorLexicalService.IsIdentifier(rest))
                {
                    throw new IdlException(input, offset, "Malformed #ifdef directive.");
                }

                conditionals.Enter(macros.ContainsKey(rest));
                AppendBlankLine(output, lineLength);
                return;

            case "ifndef":
                if (!PreprocessorLexicalService.IsIdentifier(rest))
                {
                    throw new IdlException(input, offset, "Malformed #ifndef directive.");
                }

                conditionals.Enter(!macros.ContainsKey(rest));
                AppendBlankLine(output, lineLength);
                return;

            case "elif":
                conditionals.SelectElseIf(
                    () => evaluateCondition(rest, input, restOffset, offset),
                    input,
                    offset);
                AppendBlankLine(output, lineLength);
                return;

            case "else":
                conditionals.SelectElse(input, offset);
                AppendBlankLine(output, lineLength);
                return;

            case "endif":
                conditionals.End(input, offset);
                AppendBlankLine(output, lineLength);
                return;

            case "define":
                if (conditionals.IsActive)
                {
                    define(input, offset, rest);
                }

                AppendBlankLine(output, lineLength);
                return;

            case "undef":
                if (conditionals.IsActive)
                {
                    if (!PreprocessorLexicalService.IsIdentifier(rest))
                    {
                        throw new IdlException(input, offset, "Malformed #undef directive.");
                    }

                    macros.Remove(rest);
                }

                AppendBlankLine(output, lineLength);
                return;

            case "include":
            case "import":
            case "using":
                if (conditionals.IsActive)
                {
                    var match = IncludePattern.Match(expand(rest, restOffset).Trim());
                    if (!match.Success)
                    {
                        throw new IdlException(input, offset, $"Malformed #{name} directive; expected a quoted or angle-bracket filename, for example #include \"Common.idl\".");
                    }

                    include(
                        match.Groups["quoted"].Success ? match.Groups["quoted"].Value : match.Groups["angle"].Value,
                        match.Groups["angle"].Success,
                        offset);
                }

                AppendBlankLine(output, lineLength);
                return;

            case "line":
                if (conditionals.IsActive)
                {
                    var lineMatch = LinePattern.Match(rest);
                    if (!lineMatch.Success || !int.TryParse(lineMatch.Groups["line"].Value, out var logicalLine))
                    {
                        throw new IdlException(input, offset, "Malformed #line directive.");
                    }

                    setLogicalLocation(logicalLine - (getPhysicalLineNumber(offset) + 1),
                        lineMatch.Groups["file"].Success ? lineMatch.Groups["file"].Value : null);
                }

                AppendBlankLine(output, lineLength);
                return;

            case "pragma":
                if (conditionals.IsActive && string.Equals(rest, "once", StringComparison.Ordinal))
                {
                    hasPragmaOnce = true;
                    pragmaOnceEncountered?.Invoke();
                }
                else if (conditionals.IsActive
                    && rest.StartsWith("message", StringComparison.Ordinal)
                    && (rest.Length == "message".Length
                        || char.IsWhiteSpace(rest["message".Length])
                        || rest["message".Length] == '('))
                {
                    diagnostics()?.Add(new IdlDiagnostic("DDSG0107", input, offset, $"Preprocessor message: {rest}"));
                }

                AppendBlankLine(output, lineLength);
                return;

            case "error":
                if (conditionals.IsActive)
                {
                    throw new IdlException(input, offset, $"Preprocessor error: {rest}");
                }

                AppendBlankLine(output, lineLength);
                return;

            case "warning":
                if (conditionals.IsActive)
                {
                    diagnostics()?.Add(new IdlDiagnostic("DDSG0106", input, offset, $"Preprocessor warning: {rest}"));
                }

                AppendBlankLine(output, lineLength);
                return;

            default:
                if (conditionals.IsActive)
                {
                    throw new IdlException(input, offset, $"Unsupported preprocessor directive: #{name}.");
                }

                AppendBlankLine(output, lineLength);
                return;
        }
    }

    private static void AppendBlankLine(StringBuilder output, int lineLength) => output.Append(' ', lineLength);
}
