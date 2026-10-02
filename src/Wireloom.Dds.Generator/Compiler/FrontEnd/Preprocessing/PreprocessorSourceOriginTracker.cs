namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Tracks expanded-text origins and compresses them into source spans.</summary>
internal sealed class PreprocessorSourceOriginTracker(
    Func<int, int> originalOffset,
    CancellationToken cancellationToken)
{
    /// <summary>Appends source-origin entries for expanded text.</summary>
    internal void AppendExpandedOrigins(List<int> origins, string source, string expanded, int sourceOffset)
    {
        var sourceTokens = Tokenize(source);
        var expandedTokens = Tokenize(expanded);
        var sourceLookup = BuildTokenLookup(source, sourceTokens);
        var sourceIndex = 0;
        var expandedIndex = 0;
        var alignmentWork = 0;

        while (expandedIndex < expandedTokens.Count)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (sourceIndex < sourceTokens.Count
                && TokensEqual(source, sourceTokens[sourceIndex], expanded, expandedTokens[expandedIndex]))
            {
                AppendExact(origins, sourceTokens[sourceIndex], sourceOffset);
                sourceIndex++;
                expandedIndex++;
                continue;
            }

            var anchor = FindNextMatchingToken(source, sourceTokens, sourceLookup, sourceIndex, expanded, expandedTokens, expandedIndex, ref alignmentWork);
            var fallback = sourceIndex < sourceTokens.Count
                ? originalOffset(sourceOffset + sourceTokens[sourceIndex].Start)
                : originalOffset(sourceOffset + source.Length);
            var end = anchor.ExpandedIndex >= 0 ? anchor.ExpandedIndex : expandedTokens.Count;
            while (expandedIndex < end)
            {
                AppendRepeated(origins, expandedTokens[expandedIndex].Length, fallback);
                expandedIndex++;
            }

            if (anchor.SourceIndex < 0)
            {
                break;
            }

            sourceIndex = anchor.SourceIndex;
        }
    }

    /// <summary>Appends physical output offsets for generated text.</summary>
    internal void AppendOutputOffsets(List<int> offsets, int count, int sourceOffset, int sourceLineEnd)
    {
        for (var index = 0; index < count; index++)
        {
            offsets.Add(originalOffset(Math.Min(sourceOffset + index, sourceLineEnd)));
        }
    }

    /// <summary>Compresses per-character origins into contiguous source spans.</summary>
    internal static IReadOnlyList<SourceOriginSpan> Compress(IReadOnlyList<int> origins)
    {
        var spans = new List<SourceOriginSpan>();
        var outputStart = 0;
        var eofOutput = origins.Count - 1;

        while (outputStart < eofOutput)
        {
            var sourceStart = origins[outputStart];
            var outputEnd = outputStart + 1;
            var sourceLength = 0;
            if (outputEnd < eofOutput && origins[outputEnd] == sourceStart + 1)
            {
                sourceLength = 1;
                while (outputEnd < eofOutput && origins[outputEnd] == sourceStart + sourceLength)
                {
                    sourceLength++;
                    outputEnd++;
                }
            }
            else
            {
                while (outputEnd < eofOutput && origins[outputEnd] == sourceStart)
                {
                    outputEnd++;
                }
            }

            spans.Add(new SourceOriginSpan(outputStart, outputEnd - outputStart, sourceStart, sourceLength));
            outputStart = outputEnd;
        }

        spans.Add(new SourceOriginSpan(eofOutput, 1, origins[eofOutput], 0));
        return spans;
    }

    private static List<(int Start, int Length)> Tokenize(string text)
    {
        var tokens = new List<(int Start, int Length)>();
        var index = 0;
        while (index < text.Length)
        {
            var start = index;
            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                index = PreprocessorLexicalService.SkipLiteral(text, index);
            }
            else if (char.IsWhiteSpace(text[index]))
            {
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                {
                    index++;
                }
            }
            else if (PreprocessorLexicalService.IsIdentifierStart(text[index]) || char.IsDigit(text[index]))
            {
                index++;
                while (index < text.Length
                    && (PreprocessorLexicalService.IsIdentifierPart(text[index]) || text[index] == '\''))
                {
                    index++;
                }
            }
            else
            {
                index++;
            }

            tokens.Add((start, index - start));
        }

        return tokens;
    }

    private static Dictionary<(int Length, uint Hash), List<int>> BuildTokenLookup(
        string text,
        IReadOnlyList<(int Start, int Length)> tokens)
    {
        var lookup = new Dictionary<(int Length, uint Hash), List<int>>();
        for (var index = 0; index < tokens.Count; index++)
        {
            var key = GetTokenKey(text, tokens[index]);
            if (!lookup.TryGetValue(key, out var matches))
            {
                matches = [];
                lookup.Add(key, matches);
            }

            matches.Add(index);
        }

        return lookup;
    }

    private static (int Length, uint Hash) GetTokenKey(string text, (int Start, int Length) token)
    {
        const uint offsetBasis = 2166136261;
        const uint prime = 16777619;
        var hash = offsetBasis;
        for (var index = token.Start; index < token.Start + token.Length; index++)
        {
            hash ^= text[index];
            hash *= prime;
        }

        return (token.Length, hash);
    }

    private static bool TokensEqual(string left, (int Start, int Length) leftToken, string right, (int Start, int Length) rightToken) =>
        leftToken.Length == rightToken.Length
        && string.CompareOrdinal(left, leftToken.Start, right, rightToken.Start, leftToken.Length) == 0;

    private (int SourceIndex, int ExpandedIndex) FindNextMatchingToken(
        string source,
        IReadOnlyList<(int Start, int Length)> sourceTokens,
        IReadOnlyDictionary<(int Length, uint Hash), List<int>> sourceLookup,
        int sourceIndex,
        string expanded,
        IReadOnlyList<(int Start, int Length)> expandedTokens,
        int expandedIndex,
        ref int alignmentWork)
    {
        var best = (SourceIndex: -1, ExpandedIndex: -1, Length: 0);
        for (var output = expandedIndex + 1; output < expandedTokens.Count; output++)
        {
            if (++alignmentWork > PreprocessorLimits.MaximumOriginAlignmentWork)
            {
                return (best.SourceIndex, best.ExpandedIndex);
            }

            if ((alignmentWork & 1023) == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (char.IsWhiteSpace(expanded[expandedTokens[output].Start])
                || !sourceLookup.TryGetValue(GetTokenKey(expanded, expandedTokens[output]), out var matches))
            {
                continue;
            }

            foreach (var input in matches)
            {
                if (input < sourceIndex || input >= sourceTokens.Count || char.IsWhiteSpace(source[sourceTokens[input].Start]))
                {
                    continue;
                }

                var run = 0;
                while (input + run < sourceTokens.Count
                    && output + run < expandedTokens.Count
                    && TokensEqual(source, sourceTokens[input + run], expanded, expandedTokens[output + run]))
                {
                    if (++alignmentWork > PreprocessorLimits.MaximumOriginAlignmentWork)
                    {
                        return (best.SourceIndex, best.ExpandedIndex);
                    }

                    run++;
                }

                if (run > best.Length
                    || (run == best.Length && input == best.SourceIndex && output > best.ExpandedIndex))
                {
                    best = (input, output, run);
                }

                if (run == sourceTokens.Count - input && output + run == expandedTokens.Count)
                {
                    return (best.SourceIndex, best.ExpandedIndex);
                }
            }
        }

        return (best.SourceIndex, best.ExpandedIndex);
    }

    private void AppendExact(List<int> origins, (int Start, int Length) token, int sourceOffset)
    {
        for (var index = 0; index < token.Length; index++)
        {
            origins.Add(originalOffset(sourceOffset + token.Start + index));
        }
    }

    private static void AppendRepeated(List<int> origins, int length, int origin)
    {
        for (var index = 0; index < length; index++)
        {
            origins.Add(origin);
        }
    }
}
