using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private string Expand(string text, int offset = 0, IReadOnlyList<int>? sourceOffsets = null) =>
        Expand(text, new HashSet<string>(StringComparer.Ordinal), 0, offset, sourceOffsets);

    private string Expand(string text, HashSet<string> expanding, int depth, int offset, IReadOnlyList<int>? sourceOffsets = null)
    {
        cancellationToken.ThrowIfCancellationRequested();
        macroWork = checked(macroWork + 1);
        if (macroWork > MaximumMacroWork)
        {
            throw new IdlException(currentInput!, ExpansionSourceOffset(offset, sourceOffsets), $"Macro expansion exceeds the {MaximumMacroWork}-operation limit.");
        }

        if (depth > MaximumMacroDepth)
        {
            throw new IdlException(currentInput!, ExpansionSourceOffset(offset, sourceOffsets), $"Macro expansion exceeds the {MaximumMacroDepth}-level nesting limit.");
        }

        if (text.Length == 0)
        {
            return text;
        }

        var output = new StringBuilder(text.Length);
        var index = 0;

        while (index < text.Length)
        {
            if (TryConsumeInlinePragma(text, ref index))
            {
                if (index >= text.Length)
                {
                    break;
                }

                continue;
            }

            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                var literalStart = index;
                var quoteIndex = LiteralQuoteIndex(text, index);
                AppendExpansion(output, text[index..quoteIndex], offset + index, sourceOffsets);
                index = quoteIndex;
                var quote = text[index++];
                var closed = false;
                AppendExpansion(output, quote, offset + index - 1, sourceOffsets);

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
                    throw new IdlException(currentInput!, ExpansionSourceOffset(offset + literalStart, sourceOffsets), "Unterminated string literal.");
                }

                continue;
            }

            if (!IsIdentifierStart(text[index]))
            {
                var character = text[index++];
                AppendExpansion(output, character, offset + index - 1, sourceOffsets);
                continue;
            }

            var tokenStart = index++;
            while (index < text.Length && IsIdentifierPart(text[index]))
            {
                index++;
            }

            var name = text[tokenStart..index];
            if (name == "__has_include" && TryExpandHasInclude(text, ref index, out var hasInclude))
            {
                AppendExpansion(output, hasInclude ? '1' : '0', offset + tokenStart, sourceOffsets);
                continue;
            }

            if (!macros.TryGetValue(name, out var macro))
            {
                if (name == "__FILE__")
                {
                    AppendExpansion(output, Stringify(logicalFileName ?? currentInput!.Path), offset + tokenStart, sourceOffsets);
                }
                else if (name == "__LINE__")
                {
                    AppendExpansion(output, GetLogicalLineNumber(ExpansionSourceOffset(offset + tokenStart, sourceOffsets)), offset + tokenStart, sourceOffsets);
                }
                else if (name == "__COUNTER__")
                {
                    AppendExpansion(output, counter++, offset + tokenStart, sourceOffsets);
                }
                else
                {
                    AppendExpansion(output, name, offset + tokenStart, sourceOffsets);
                }

                continue;
            }

            if (expanding.Contains(name))
            {
                // A macro disabled during its own replacement is left as a
                // token. This suppresses direct and indirect recursion.
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
                    || !TryReadArguments(text, open, out var arguments, out var end))
                {
                    AppendExpansion(output, name, offset + tokenStart, sourceOffsets);
                    continue;
                }

                var argumentCount = arguments.Count == 0 && macro.Parameters.Count > 0 ? 1 : arguments.Count;
                if ((!macro.Variadic && argumentCount != macro.Parameters.Count)
                    || (macro.Variadic && argumentCount < macro.Parameters.Count - 1))
                {
                    diagnostics?.Add(new IdlDiagnostic("DDSG0104", currentInput!, ExpansionSourceOffset(offset + tokenStart, sourceOffsets), $"Function-like macro '{name}' was invoked with the wrong number of arguments; expansion will continue."));
                }

                expanding.Add(name);
                try
                {
                    index = end;

                    var tokenOffset = MapOffset(offset + tokenStart, sourceOffsets);
                    var replacement = ExpandFunctionMacro(name, macro, arguments, expanding, depth, tokenOffset);
                    AppendExpansion(output, RescanReplacementAndSuffix(name, replacement, text[index..], expanding, depth + 1, tokenOffset, index, offset, sourceOffsets), tokenOffset, null);

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
                var replacement = Expand(macro.Body, expanding, depth + 1, tokenOffset);
                AppendExpansion(output, RescanReplacementAndSuffix(name, replacement, text[index..], expanding, depth + 1, tokenOffset, index, offset, sourceOffsets), tokenOffset, null);

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
        var remaining = MaximumOutputLength - expansionBaseOutputLength - output.Length;
        if (value.Length > remaining)
        {
            throw new IdlException(currentInput!, ExpansionSourceOffset(offset, offsets), $"Preprocessor output exceeds the {MaximumOutputLength}-character limit.");
        }

        output.Append(value);
    }

    private void AppendExpansion(StringBuilder output, char value, int offset, IReadOnlyList<int>? offsets)
    {
        if (output.Length >= MaximumOutputLength - expansionBaseOutputLength)
        {
            throw new IdlException(currentInput!, ExpansionSourceOffset(offset, offsets), $"Preprocessor output exceeds the {MaximumOutputLength}-character limit.");
        }

        output.Append(value);
    }

    private void AppendExpansion(StringBuilder output, int value, int offset, IReadOnlyList<int>? offsets) =>
        AppendExpansion(output, value.ToString(), offset, offsets);

    private int ExpansionSourceOffset(int offset, IReadOnlyList<int>? offsets) =>
        offsets is null ? offset : MapOffset(offset, offsets);

    private string RescanReplacementAndSuffix(
        string macroName,
        string replacement,
        string suffix,
        HashSet<string> expanding,
        int depth,
        int tokenOffset,
        int suffixIndex,
        int sourceBaseOffset,
        IReadOnlyList<int>? sourceOffsets)
    {
        var expandedReplacement = Expand(replacement, expanding, depth, tokenOffset);
        var suffixExpanding = new HashSet<string>(expanding, StringComparer.Ordinal);
        suffixExpanding.Remove(macroName);

        var invocationStart = 0;
        while (invocationStart < suffix.Length && char.IsWhiteSpace(suffix[invocationStart]))
        {
            invocationStart++;
        }

        var joinsFunctionInvocation = invocationStart < suffix.Length
            && suffix[invocationStart] == '('
            && expandedReplacement.Length > 0
            && IsIdentifierPart(expandedReplacement[^1]);

        EnsureExpansionLength(expandedReplacement.Length, tokenOffset);

        if (joinsFunctionInvocation && expandedReplacement.Length > MaximumOutputLength - suffix.Length)
        {
            throw new IdlException(currentInput!, tokenOffset, $"Preprocessor output exceeds the {MaximumOutputLength}-character limit.");
        }

        return joinsFunctionInvocation
            ? Expand(
                expandedReplacement + suffix,
                suffixExpanding,
                depth + 1,
                0,
                BuildCombinedOffsets(expandedReplacement.Length, suffix, tokenOffset, suffixIndex, sourceBaseOffset, sourceOffsets))
            : CombineReplacementAndSuffix(expandedReplacement, suffix, suffixExpanding, sourceOffsets, suffixIndex, sourceBaseOffset, tokenOffset);
    }

    private string CombineReplacementAndSuffix(
        string replacement,
        string suffix,
        HashSet<string> expanding,
        IReadOnlyList<int>? sourceOffsets,
        int suffixStart,
        int sourceBaseOffset,
        int tokenOffset)
    {
        var expandedSuffix = ExpandSuffix(suffix, expanding, sourceOffsets, suffixStart, sourceBaseOffset, tokenOffset);
        EnsureExpansionLength(replacement.Length + expandedSuffix.Length, tokenOffset);
        return replacement + expandedSuffix;
    }

    private string ExpandSuffix(string suffix, HashSet<string> expanding, IReadOnlyList<int>? sourceOffsets, int suffixStart, int sourceBaseOffset, int tokenOffset)
    {
        if (suffix.Length == 0)
        {
            return string.Empty;
        }

        var output = new StringBuilder(suffix.Length);
        var index = 0;

        while (index < suffix.Length)
        {
            var chunkStart = index;
            var chunkEnd = FindNextSuffixChunkEnd(suffix, index);
            var chunk = suffix[chunkStart..chunkEnd];
            var chunkOffset = sourceOffsets is null
                ? tokenOffset + suffixStart + chunkStart
                : sourceBaseOffset + suffixStart + chunkStart;
            var expandedChunk = Expand(chunk, expanding, 0, chunkOffset, sourceOffsets);

            AppendExpansion(output, expandedChunk, chunkOffset, sourceOffsets);
            index = chunkEnd;
        }

        return output.ToString();
    }

    private int FindNextSuffixChunkEnd(string suffix, int start)
    {
        if (StartsPrefixedLiteral(suffix, start) || IsLiteralStart(suffix, start))
        {
            return SkipLiteral(suffix, start);
        }

        if (!IsIdentifierStart(suffix[start]))
        {
            var nextCandidate = start + 1;
            while (nextCandidate < suffix.Length && !IsPotentialExpansionStart(suffix, nextCandidate))
            {
                nextCandidate++;
            }

            return nextCandidate;
        }

        var identifierEnd = start + 1;
        while (identifierEnd < suffix.Length && IsIdentifierPart(suffix[identifierEnd]))
        {
            identifierEnd++;
        }

        var name = suffix[start..identifierEnd];
        var hasMacroInvocation = name is "__has_include" or "_Pragma" or "__pragma"
            || macros.ContainsKey(name);

        if (!hasMacroInvocation)
        {
            var nextCandidate = identifierEnd;
            while (nextCandidate < suffix.Length && !IsPotentialExpansionStart(suffix, nextCandidate))
            {
                nextCandidate++;
            }

            return nextCandidate;
        }

        var open = identifierEnd;
        while (open < suffix.Length && char.IsWhiteSpace(suffix[open]))
        {
            open++;
        }

        return open < suffix.Length
            && suffix[open] == '('
            && TryReadArguments(suffix, open, out _, out var end)
            ? end
            : identifierEnd;
    }

    private static bool IsPotentialExpansionStart(string text, int index) =>
        StartsPrefixedLiteral(text, index)
        || IsLiteralStart(text, index)
        || IsIdentifierStart(text[index]);

    private static int[] BuildCombinedOffsets(int replacementLength, string suffix, int tokenOffset, int suffixStart, int sourceBaseOffset, IReadOnlyList<int>? sourceOffsets)
    {
        var offsets = new int[replacementLength + suffix.Length];
        for (var index = 0; index < replacementLength; index++)
        {
            offsets[index] = tokenOffset;
        }

        BuildSuffixOffsets(suffix, sourceOffsets, suffixStart, sourceBaseOffset, tokenOffset).CopyTo(offsets, replacementLength);

        return offsets;
    }

    private static int[] BuildSuffixOffsets(string suffix, IReadOnlyList<int>? sourceOffsets, int suffixStart, int sourceBaseOffset, int tokenOffset)
    {
        var offsets = new int[suffix.Length];
        for (var index = 0; index < offsets.Length; index++)
        {
            offsets[index] = sourceOffsets is null
                ? tokenOffset + suffixStart + index
                : sourceOffsets[sourceBaseOffset + suffixStart + index];
        }

        return offsets;
    }

    private static int MapOffset(int offset, IReadOnlyList<int>? sourceOffsets) =>
        sourceOffsets is not null && offset >= 0 && offset < sourceOffsets.Count
            ? sourceOffsets[offset]
            : offset;

    private string ExpandFunctionMacro(string macroName, Macro macro, List<string> arguments, HashSet<string> expanding, int depth, int offset)
    {
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>(StringComparer.Ordinal);
        var fixedCount = macro.Variadic ? macro.Parameters!.Count - 1 : macro.Parameters!.Count;
        var wasDisabled = expanding.Remove(macroName);

        try
        {
            for (var index = 0; index < fixedCount; index++)
            {
                var raw = index < arguments.Count ? arguments[index].Trim() : string.Empty;
                var parameter = macro.Parameters[index];
                substitutions[parameter] = (raw, NeedsExpandedParameter(macro.Body, parameter)
                    ? Expand(raw, expanding, depth + 1, offset)
                    : raw);
            }

            if (macro.Variadic)
            {
                var raw = string.Join(", ", arguments.Skip(fixedCount).Select(argument => argument.Trim()));
                var expanded = NeedsExpandedParameter(macro.Body, macro.VariadicParameterName!)
                    || NeedsExpandedParameter(macro.Body, "__VA_ARGS__")
                    ? Expand(raw, expanding, depth + 1, offset)
                    : raw;
                substitutions[macro.VariadicParameterName!] = (raw, expanded);
                substitutions["__VA_ARGS__"] = (raw, expanded);
            }
        }
        finally
        {
            if (wasDisabled)
            {
                expanding.Add(macroName);
            }
        }

        var replacement = macro.Variadic
            ? ExpandVaOpt(macro.Body, substitutions["__VA_ARGS__"].Raw.Length != 0)
            : macro.Body;

        replacement = ReplaceOutsideLiterals(replacement, CharizePattern, match =>
            substitutions.TryGetValue(match.Groups["name"].Value, out var charSubstitution)
                ? Charize(charSubstitution.Raw)
                : match.Value);

        replacement = ReplaceOutsideLiterals(replacement, StringifyPattern, match =>
            substitutions.TryGetValue(match.Groups["name"].Value, out var substitution)
                ? Stringify(substitution.Raw)
                : match.Value);

        while (true)
        {
            var pasted = ReplaceOutsideLiterals(replacement, TokenPastePattern, match =>
            {
                var left = substitutions.TryGetValue(match.Groups["left"].Value, out var leftValue)
                    ? leftValue.Raw
                    : match.Groups["left"].Value;
                var right = substitutions.TryGetValue(match.Groups["right"].Value, out var rightValue)
                    ? rightValue.Raw
                    : match.Groups["right"].Value;
                return left + right;
            });

            if (string.Equals(pasted, replacement, StringComparison.Ordinal))
            {
                break;
            }

            replacement = pasted;
        }

        replacement = ReplaceMacroParameters(replacement, substitutions);

        EnsureExpansionLength(replacement.Length, offset);
        return Expand(replacement, expanding, depth + 1, offset);
    }

    private void EnsureExpansionLength(int length, int offset)
    {
        if (length > MaximumOutputLength - expansionBaseOutputLength)
        {
            throw new IdlException(currentInput!, offset, $"Preprocessor output exceeds the {MaximumOutputLength}-character limit.");
        }
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

    private static bool TryReadArguments(string text, int open, out List<string> arguments, out int end)
    {
        arguments = [];
        end = open;
        var depth = 0;
        var start = open + 1;

        for (var index = open; index < text.Length; index++)
        {
            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                // The outer loop increments once more after this branch. Keep
                // the first character after the literal available to the
                // parenthesis/comma scanner.
                index = SkipLiteral(text, index) - 1;
                continue;
            }

            switch (text[index])
            {
                case '(':
                    depth++;
                    break;

                case ')':
                    depth--;
                    if (depth == 0)
                    {
                        var argument = text.Substring(start, index - start).Trim();
                        if (argument.Length != 0 || arguments.Count != 0)
                        {
                            arguments.Add(argument);
                        }

                        end = index + 1;
                        return true;
                    }

                    break;

                case ',' when depth == 1:
                    arguments.Add(text.Substring(start, index - start).Trim());
                    start = index + 1;
                    break;
            }
        }

        return false;
    }

}
