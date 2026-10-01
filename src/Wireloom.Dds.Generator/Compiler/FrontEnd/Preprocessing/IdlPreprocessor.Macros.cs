using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private string Expand(string text, int offset = 0, IReadOnlyList<int>? sourceOffsets = null) =>
        expansionService.Expand(text, offset, sourceOffsets);

    private string Expand(string text, HashSet<string> expanding, int depth, int offset, IReadOnlyList<int>? sourceOffsets = null) =>
        expansionService.Expand(text, expanding, depth, offset, sourceOffsets);

    private void AppendExpansion(StringBuilder output, string value, int offset, IReadOnlyList<int>? offsets)
    {
        var remaining = PreprocessorLimits.MaximumOutputLength - expansionState.BaseOutputLength - output.Length;
        if (value.Length > remaining)
        {
            throw new IdlException(currentInput!, ExpansionSourceOffset(offset, offsets), $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
        }

        output.Append(value);
    }

    private int ExpansionSourceOffset(int offset, IReadOnlyList<int>? offsets) =>
        offsets is null ? offset : MapOffset(offset, offsets);

    private static int MapOffset(int offset, IReadOnlyList<int>? offsets) =>
        offsets is not null && offset >= 0 && offset < offsets.Count
            ? offsets[offset]
            : offset;

    private string ExpandFunctionMacro(string macroName, PreprocessorMacro macro, List<string> arguments, HashSet<string> expanding, int depth, int offset)
    {
        var substitutions = argumentBinder.Bind(macroName, macro, arguments, expanding, depth, offset);

        var replacement = macroTokens.TransformReplacement(
            macro.Body,
            macro.Variadic,
            macro.Variadic && substitutions["__VA_ARGS__"].Raw.Length != 0,
            substitutions);

        EnsureExpansionLength(replacement.Length, offset);
        return Expand(replacement, expanding, depth + 1, offset);
    }

    private void EnsureExpansionLength(int length, int offset)
    {
        expansionState.EnsureOutputLength(currentInput!, length, offset);
    }

    private static bool NeedsExpandedParameter(string body, string parameter)
    {
        var index = 0;
        while (index < body.Length)
        {
            if (StartsPrefixedLiteral(body, index) || IsLiteralStart(body, index))
            {
                index = SkipLiteral(body, index);
                continue;
            }

            if (!IsIdentifierStart(body[index]))
            {
                index++;
                continue;
            }

            var start = index++;
            while (index < body.Length && IsIdentifierPart(body[index]))
            {
                index++;
            }

            if (!string.Equals(body[start..index], parameter, StringComparison.Ordinal))
            {
                continue;
            }

            var before = PreviousNonWhitespace(body, start - 1);
            var after = NextNonWhitespace(body, index);
            var stringized = before >= 0
                && body[before] == '#'
                && (before == 0 || body[before - 1] != '#');
            var charized = before >= 1 && body[before] == '@' && body[before - 1] == '#';
            var pastedLeft = before >= 1 && body[before] == '#' && body[before - 1] == '#';
            var pastedRight = after + 1 < body.Length && body[after] == '#' && body[after + 1] == '#';

            if (!stringized && !charized && !pastedLeft && !pastedRight)
            {
                return true;
            }
        }

        return false;
    }

    private static int PreviousNonWhitespace(string text, int index)
    {
        while (index >= 0 && char.IsWhiteSpace(text[index]))
        {
            index--;
        }

        return index;
    }

    private static int NextNonWhitespace(string text, int index)
    {
        while (index < text.Length && char.IsWhiteSpace(text[index]))
        {
            index++;
        }

        return index;
    }
}
