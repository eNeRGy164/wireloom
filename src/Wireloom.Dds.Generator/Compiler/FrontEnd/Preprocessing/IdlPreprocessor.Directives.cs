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

    private static readonly Regex LinePattern = new(
        @"^(?<line>[0-9]+)(?:\s+""(?<file>[^""]*)""\s*)?$",
        RegexOptions.Compiled);

    private void ProcessDirective(
        Match directive,
        IdlInput input,
        int offset,
        int sourceOffset,
        Action<string, bool, int> include,
        Stack<ConditionalFrame> conditionals,
        ref bool active,
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
                PushConditional(active && EvaluateCondition(rest, input, restOffset, offset), conditionals, ref active);
                AppendBlankLine(output, lineLength);

                return;

            case "ifdef":
                if (!IsIdentifier(rest))
                {
                    throw new IdlException(input, offset, "Malformed #ifdef directive.");
                }

                PushConditional(macros.ContainsKey(rest), conditionals, ref active);
                AppendBlankLine(output, lineLength);

                return;

            case "ifndef":
                if (!IsIdentifier(rest))
                {
                    throw new IdlException(input, offset, "Malformed #ifndef directive.");
                }

                PushConditional(!macros.ContainsKey(rest), conditionals, ref active);
                AppendBlankLine(output, lineLength);

                return;

            case "elif":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #elif.");
                }

                var elifFrame = conditionals.Pop();
                if (elifFrame.ElseSeen)
                {
                    throw new IdlException(input, offset, "Unexpected #elif after #else.");
                }

                var elifActive = elifFrame is { ParentActive: true, BranchTaken: false }
                    && EvaluateCondition(rest, input, restOffset, offset);
                conditionals.Push(new ConditionalFrame(elifFrame.ParentActive, elifFrame.BranchTaken || elifActive, false));
                active = elifActive;

                AppendBlankLine(output, lineLength);

                return;

            case "else":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #else.");
                }

                var elseFrame = conditionals.Pop();
                if (elseFrame.ElseSeen)
                {
                    throw new IdlException(input, offset, "Unexpected second #else.");
                }

                var elseActive = elseFrame is { ParentActive: true, BranchTaken: false };
                conditionals.Push(new ConditionalFrame(elseFrame.ParentActive, true, true));
                active = elseActive;

                AppendBlankLine(output, lineLength);

                return;

            case "endif":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #endif.");
                }

                var endifFrame = conditionals.Pop();
                active = endifFrame.ParentActive;

                AppendBlankLine(output, lineLength);

                return;

            case "define":
                if (active)
                {
                    Define(input, offset, rest);
                }

                AppendBlankLine(output, lineLength);

                return;

            case "undef":
                if (active)
                {
                    if (!IsIdentifier(rest))
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
                if (active)
                {
                    var match = IncludePattern.Match(Expand(rest, restOffset, sourceOffsetMap).Trim());
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
                if (active)
                {
                    var lineMatch = LinePattern.Match(rest);
                    if (!lineMatch.Success || !int.TryParse(lineMatch.Groups["line"].Value, out var logicalLine))
                    {
                        throw new IdlException(input, offset, "Malformed #line directive.");
                    }

                    logicalLineOffset = logicalLine - (GetPhysicalLineNumber(offset) + 1);
                    if (lineMatch.Groups["file"].Success)
                    {
                        logicalFileName = lineMatch.Groups["file"].Value;
                    }
                }

                AppendBlankLine(output, lineLength);

                return;

            case "pragma":
                if (active && string.Equals(rest, "once", StringComparison.Ordinal))
                {
                    hasPragmaOnce = true;
                    pragmaOnceEncountered?.Invoke();
                }
                else if (active
                    && rest.StartsWith("message", StringComparison.Ordinal)
                    && (rest.Length == "message".Length
                        || char.IsWhiteSpace(rest["message".Length])
                        || rest["message".Length] == '('))
                {
                    diagnostics?.Add(new IdlDiagnostic("DDSG0107", input, offset, $"Preprocessor message: {rest}"));
                }

                AppendBlankLine(output, lineLength);

                return;

            case "error":
                if (active)
                {
                    throw new IdlException(input, offset, $"Preprocessor error: {rest}");
                }

                AppendBlankLine(output, lineLength);

                return;

            case "warning":
                if (active)
                {
                    diagnostics?.Add(new IdlDiagnostic("DDSG0106", input, offset, $"Preprocessor warning: {rest}"));
                }

                AppendBlankLine(output, lineLength);

                return;

            default:
                if (active)
                {
                    throw new IdlException(input, offset, $"Unsupported preprocessor directive: #{name}.");
                }

                AppendBlankLine(output, lineLength);

                return;
        }
    }

    private static void PushConditional(bool condition, Stack<ConditionalFrame> conditionals, ref bool active)
    {
        var parentActive = active;
        var branchActive = parentActive && condition;
        conditionals.Push(new ConditionalFrame(parentActive, branchActive, false));
        active = branchActive;
    }
}
