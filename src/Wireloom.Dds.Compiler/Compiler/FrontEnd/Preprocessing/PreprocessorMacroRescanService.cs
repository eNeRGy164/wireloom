using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Coordinates replacement rescanning, suffix continuation, and suffix-origin offsets.</summary>
internal sealed class PreprocessorMacroRescanService(
    PreprocessorMacroTable macros,
    PreprocessorMacroTokenService macroTokens,
    CancellationToken cancellationToken,
    Func<IdlInput> currentInput,
    Func<string, HashSet<string>, int, int, IReadOnlyList<int>?, string> expand,
    Action<StringBuilder, string, int, IReadOnlyList<int>?> appendExpansion,
    Action<int, int> ensureExpansionLength)
{
    internal string RescanReplacementAndSuffix(
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
        var expandedReplacement = expand(replacement, expanding, depth, tokenOffset, null);
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
            && PreprocessorLexicalService.IsIdentifierPart(expandedReplacement[^1]);

        ensureExpansionLength(expandedReplacement.Length, tokenOffset);
        if (joinsFunctionInvocation && expandedReplacement.Length > PreprocessorLimits.MaximumOutputLength - suffix.Length)
        {
            throw new IdlException(currentInput(), tokenOffset, $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
        }

        if (joinsFunctionInvocation)
        {
            return expand(
                expandedReplacement + suffix,
                suffixExpanding,
                depth + 1,
                0,
                BuildCombinedOffsets(expandedReplacement.Length, suffix, tokenOffset, suffixIndex, sourceBaseOffset, sourceOffsets));
        }

        return CombineReplacementAndSuffix(expandedReplacement, suffix, suffixExpanding, sourceOffsets, suffixIndex, sourceBaseOffset, tokenOffset);
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
        ensureExpansionLength(replacement.Length + expandedSuffix.Length, tokenOffset);
        return replacement + expandedSuffix;
    }

    private string ExpandSuffix(
        string suffix,
        HashSet<string> expanding,
        IReadOnlyList<int>? sourceOffsets,
        int suffixStart,
        int sourceBaseOffset,
        int tokenOffset)
    {
        if (suffix.Length == 0)
        {
            return string.Empty;
        }

        var output = new StringBuilder(suffix.Length);
        var index = 0;
        while (index < suffix.Length)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var chunkStart = index;
            var chunkEnd = FindNextSuffixChunkEnd(suffix, index);
            var chunk = suffix[chunkStart..chunkEnd];
            var chunkOffset = sourceOffsets is null
                ? tokenOffset + suffixStart + chunkStart
                : sourceBaseOffset + suffixStart + chunkStart;
            var expandedChunk = expand(chunk, expanding, 0, chunkOffset, sourceOffsets);
            appendExpansion(output, expandedChunk, chunkOffset, sourceOffsets);
            index = chunkEnd;
        }

        return output.ToString();
    }

    private int FindNextSuffixChunkEnd(string suffix, int start)
    {
        if (PreprocessorLexicalService.StartsPrefixedLiteral(suffix, start)
            || PreprocessorLexicalService.IsLiteralStart(suffix, start))
        {
            return PreprocessorLexicalService.SkipLiteral(suffix, start);
        }

        if (!PreprocessorLexicalService.IsIdentifierStart(suffix[start]))
        {
            var nextCandidate = start + 1;
            while (nextCandidate < suffix.Length && !IsPotentialExpansionStart(suffix, nextCandidate))
            {
                nextCandidate++;
            }

            return nextCandidate;
        }

        var identifierEnd = start + 1;
        while (identifierEnd < suffix.Length && PreprocessorLexicalService.IsIdentifierPart(suffix[identifierEnd]))
        {
            identifierEnd++;
        }

        var name = suffix[start..identifierEnd];
        var hasMacroInvocation = name is "__has_include" or "_Pragma" or "__pragma" || macros.ContainsKey(name);
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

        if (open < suffix.Length
            && suffix[open] == '('
            && macroTokens.TryReadArguments(suffix, open, out _, out var end))
        {
            return end;
        }

        return identifierEnd;
    }

    private static bool IsPotentialExpansionStart(string text, int index) =>
        PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
        || PreprocessorLexicalService.IsLiteralStart(text, index)
        || PreprocessorLexicalService.IsIdentifierStart(text[index]);

    private static int[] BuildCombinedOffsets(
        int replacementLength,
        string suffix,
        int tokenOffset,
        int suffixStart,
        int sourceBaseOffset,
        IReadOnlyList<int>? sourceOffsets)
    {
        var offsets = new int[replacementLength + suffix.Length];
        for (var index = 0; index < replacementLength; index++)
        {
            offsets[index] = tokenOffset;
        }

        BuildSuffixOffsets(suffix, sourceOffsets, suffixStart, sourceBaseOffset, tokenOffset).CopyTo(offsets, replacementLength);
        return offsets;
    }

    private static int[] BuildSuffixOffsets(
        string suffix,
        IReadOnlyList<int>? sourceOffsets,
        int suffixStart,
        int sourceBaseOffset,
        int tokenOffset)
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
}
