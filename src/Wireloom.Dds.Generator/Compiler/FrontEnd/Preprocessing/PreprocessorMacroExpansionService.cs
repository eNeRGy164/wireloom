using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Owns the recursive macro-expansion scan and delegates replacement rescanning.</summary>
internal sealed class PreprocessorMacroExpansionService(
    PreprocessorMacroTable macros,
    PreprocessorMacroTokenService macroTokens,
    PreprocessorExpansionState expansionState,
    CancellationToken cancellationToken,
    Func<IdlInput> currentInput,
    Func<ICollection<IdlDiagnostic>?> diagnostics,
    Func<string, int, (bool Handled, int Index, bool Value)> tryExpandHasInclude,
    Func<string, int, (bool Handled, int Index)> tryConsumeInlinePragma,
    Func<string, PreprocessorMacro, List<string>, HashSet<string>, int, int, string> expandFunctionMacro,
    Func<string, string, string, HashSet<string>, int, int, int, int, IReadOnlyList<int>?, string> rescanReplacementAndSuffix,
    Func<string> stringifyFile,
    Func<int, IReadOnlyList<int>?, int> logicalLine,
    Func<int> nextCounter)
{
    /// <summary>Expands macros in source text and preserves source offsets.</summary>
    internal string Expand(string text, int offset = 0, IReadOnlyList<int>? sourceOffsets = null) =>
        Expand(text, new HashSet<string>(StringComparer.Ordinal), 0, offset, sourceOffsets);

    /// <summary>Expands macros recursively using the supplied expansion state.</summary>
    internal string Expand(string text, HashSet<string> expanding, int depth, int offset, IReadOnlyList<int>? sourceOffsets)
    {
        cancellationToken.ThrowIfCancellationRequested();
        expansionState.CountOperation(currentInput(), offset, sourceOffsets, depth);

        if (text.Length == 0)
        {
            return text;
        }

        var output = new StringBuilder(text.Length);
        var index = 0;
        while (index < text.Length)
        {
            var pragma = tryConsumeInlinePragma(text, index);
            if (pragma.Handled)
            {
                index = pragma.Index;
                if (index >= text.Length)
                {
                    break;
                }

                continue;
            }

            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                var literalStart = index;
                var quoteIndex = PreprocessorLexicalService.LiteralQuoteIndex(text, index);
                AppendExpansion(output, text[index..quoteIndex], offset + index, sourceOffsets);
                index = quoteIndex;
                var quote = text[index++];
                AppendExpansion(output, quote, offset + index - 1, sourceOffsets);
                var closed = false;
                while (index < text.Length)
                {
                    var character = text[index++];
                    AppendExpansion(output, character, offset + index - 1, sourceOffsets);
                    if (character == '\\' && index < text.Length)
                    {
                        var escaped = text[index++];
                        AppendExpansion(output, escaped, offset + index - 1, sourceOffsets);
                    }
                    else if (character == quote)
                    {
                        closed = true;
                        break;
                    }
                }

                if (!closed)
                {
                    throw new IdlException(currentInput(), ExpansionSourceOffset(offset + literalStart, sourceOffsets), "Unterminated string literal.");
                }

                continue;
            }

            if (!PreprocessorLexicalService.IsIdentifierStart(text[index]))
            {
                var character = text[index++];
                AppendExpansion(output, character, offset + index - 1, sourceOffsets);
                continue;
            }

            var tokenStart = index++;
            while (index < text.Length && PreprocessorLexicalService.IsIdentifierPart(text[index]))
            {
                index++;
            }

            var name = text[tokenStart..index];
            var include = name == "__has_include"
                ? tryExpandHasInclude(text, index)
                : (false, index, false);
            if (include.Item1)
            {
                index = include.Item2;
                AppendExpansion(output, include.Item3 ? '1' : '0', offset + tokenStart, sourceOffsets);
                continue;
            }

            if (!macros.TryGetValue(name, out var macro))
            {
                if (name == "__FILE__")
                {
                    AppendExpansion(output, stringifyFile(), offset + tokenStart, sourceOffsets);
                }
                else if (name == "__LINE__")
                {
                    AppendExpansion(output, logicalLine(ExpansionSourceOffset(offset + tokenStart, sourceOffsets), sourceOffsets), offset + tokenStart, sourceOffsets);
                }
                else if (name == "__COUNTER__")
                {
                    AppendExpansion(output, nextCounter(), offset + tokenStart, sourceOffsets);
                }
                else
                {
                    AppendExpansion(output, name, offset + tokenStart, sourceOffsets);
                }

                continue;
            }

            if (expanding.Contains(name))
            {
                AppendExpansion(output, name, offset + tokenStart, sourceOffsets);
                continue;
            }

            if (macro.Parameters is not null)
            {
                var open = index;
                while (open < text.Length && char.IsWhiteSpace(text[open]))
                {
                    open++;
                }

                if (open >= text.Length
                    || text[open] != '('
                    || !macroTokens.TryReadArguments(text, open, out var arguments, out var end))
                {
                    AppendExpansion(output, name, offset + tokenStart, sourceOffsets);
                    continue;
                }

                var argumentCount = arguments.Count == 0 && macro.Parameters.Count > 0 ? 1 : arguments.Count;
                if ((!macro.Variadic && argumentCount != macro.Parameters.Count)
                    || (macro.Variadic && argumentCount < macro.Parameters.Count - 1))
                {
                    diagnostics()?.Add(new IdlDiagnostic("DDSG0104", currentInput(), ExpansionSourceOffset(offset + tokenStart, sourceOffsets), $"Function-like macro '{name}' was invoked with the wrong number of arguments; expansion will continue."));
                }

                expanding.Add(name);
                try
                {
                    index = end;
                    var tokenOffset = MapOffset(offset + tokenStart, sourceOffsets);
                    var replacement = expandFunctionMacro(name, macro, arguments, expanding, depth, tokenOffset);
                    AppendExpansion(output, rescanReplacementAndSuffix(name, replacement, text[index..], expanding, depth + 1, tokenOffset, index, offset, sourceOffsets), tokenOffset, null);
                    return output.ToString();
                }
                finally
                {
                    expanding.Remove(name);
                }
            }

            expanding.Add(name);
            try
            {
                var tokenOffset = MapOffset(offset + tokenStart, sourceOffsets);
                var replacement = Expand(macro.Body, expanding, depth + 1, tokenOffset, null);
                AppendExpansion(output, rescanReplacementAndSuffix(name, replacement, text[index..], expanding, depth + 1, tokenOffset, index, offset, sourceOffsets), tokenOffset, null);
                return output.ToString();
            }
            finally
            {
                expanding.Remove(name);
            }
        }

        return output.ToString();
    }

    private void AppendExpansion(StringBuilder output, string value, int offset, IReadOnlyList<int>? offsets)
    {
        var remaining = PreprocessorLimits.MaximumOutputLength - expansionState.BaseOutputLength - output.Length;
        if (value.Length > remaining)
        {
            throw new IdlException(currentInput(), ExpansionSourceOffset(offset, offsets), $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
        }

        output.Append(value);
    }

    private void AppendExpansion(StringBuilder output, char value, int offset, IReadOnlyList<int>? offsets)
    {
        if (output.Length >= PreprocessorLimits.MaximumOutputLength - expansionState.BaseOutputLength)
        {
            throw new IdlException(currentInput(), ExpansionSourceOffset(offset, offsets), $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
        }

        output.Append(value);
    }

    private void AppendExpansion(StringBuilder output, int value, int offset, IReadOnlyList<int>? offsets) =>
        AppendExpansion(output, value.ToString(), offset, offsets);

    private static int ExpansionSourceOffset(int offset, IReadOnlyList<int>? offsets) => MapOffset(offset, offsets);

    private static int MapOffset(int offset, IReadOnlyList<int>? offsets) =>
        offsets is not null && offset >= 0 && offset < offsets.Count ? offsets[offset] : offset;
}
