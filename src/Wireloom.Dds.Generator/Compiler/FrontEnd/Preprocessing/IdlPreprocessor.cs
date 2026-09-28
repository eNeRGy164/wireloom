using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>
/// Processes the deterministic preprocessing subset used by IDL inputs.
/// </summary>
internal sealed partial class IdlPreprocessor
{
    public IdlPreprocessor(IEnumerable<string> defines, IEnumerable<string> undefines, CancellationToken cancellationToken = default)
    {
        this.cancellationToken = cancellationToken;
        foreach (var define in defines)
        {
            var separator = define.IndexOf('=');
            var name = (separator < 0 ? define : define[..separator]).Trim();
            var value = separator < 0 ? "1" : define[(separator + 1)..];

            if (IsIdentifier(name))
            {
                macros[name] = new Macro(null, value, false);
            }
        }

        foreach (var undefine in undefines)
        {
            var name = undefine.Trim();
            if (IsIdentifier(name))
            {
                macros.Remove(name);
            }
        }
    }

    /// <summary>
    /// Processes an input and reports preprocessing metadata needed by the include graph.
    /// </summary>
    internal PreprocessedIdl ProcessWithMetadata(
        IdlInput input,
        Action<string, bool, int> include,
        ICollection<IdlDiagnostic>? diagnosticSink = null,
        Action? pragmaOnceEncountered = null,
        Func<string, bool, bool>? includeProbe = null)
    {
        var previousInput = currentInput;
        var previousDiagnostics = diagnostics;
        var previousPragmaOnceCallback = pragmaOnceCallback;
        var previousIncludeProbe = includeProbeCallback;
        var previousLogicalLineOffset = logicalLineOffset;
        var previousLogicalFileName = logicalFileName;
        var previousSourceOffsetMap = sourceOffsetMap;
        var previousPhysicalLineStarts = physicalLineStarts;
        var previousMacroWork = macroWork;
        var previousExpansionBaseOutputLength = expansionBaseOutputLength;

        currentInput = input;
        diagnostics = diagnosticSink;
        pragmaOnceCallback = pragmaOnceEncountered;
        includeProbeCallback = includeProbe;
        logicalLineOffset = 0;
        logicalFileName = input.Path;
        macroWork = 0;
        expansionBaseOutputLength = 0;
        physicalLineStarts = BuildLineStarts(input.Text);

        try
        {
            var source = JoinContinuations(input.Text, out sourceOffsetMap);
            source = RemoveComments(input, source);

            var output = new StringBuilder(source.Length);
            var outputOffsets = new List<int>(source.Length);
            var conditionals = new Stack<ConditionalFrame>();
            var active = true;
            var hasPragmaOnce = false;
            var offset = 0;

            while (offset < source.Length)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var lineEnd = source.IndexOf('\n', offset);
                if (lineEnd < 0)
                {
                    lineEnd = source.Length;
                }

                var contentEnd = lineEnd;
                if (contentEnd > offset && source[contentEnd - 1] == '\r')
                {
                    contentEnd--;
                }

                var line = source.Substring(offset, contentEnd - offset);
                var directive = DirectivePattern.Match(line);
                var outputLength = output.Length;
                expansionBaseOutputLength = output.Length;

                if (directive.Success)
                {
                    ProcessDirective(directive, input, OriginalOffset(offset), offset, include, conditionals, ref active, ref hasPragmaOnce, pragmaOnceEncountered, output, line.Length);
                }
                else if (active)
                {
                    var expanded = Expand(line, offset, sourceOffsetMap);
                    output.Append(expanded);
                    AppendExpandedOrigins(outputOffsets, line, expanded, offset);
                }
                else
                {
                    output.Append(new string(' ', line.Length));
                }

                if (directive.Success || !active)
                {
                    AppendOutputOffsets(outputOffsets, output.Length - outputLength, offset, lineEnd);
                }

                if (lineEnd < source.Length)
                {
                    output.Append('\n');
                    outputOffsets.Add(OriginalOffset(lineEnd));
                    offset = lineEnd + 1;
                }
                else
                {
                    offset = source.Length;
                }

                if (output.Length > MaximumOutputLength)
                {
                    throw new IdlException(input, OriginalOffset(offset), $"Preprocessor output exceeds the {MaximumOutputLength}-character limit.");
                }
            }

            if (conditionals.Count != 0)
            {
                throw new IdlException(input, input.Text.Length, "Unterminated preprocessor conditional.");
            }

            // The zero-width terminal span gives parser failures at EOF an
            // explicit origin rather than borrowing the last character.
            outputOffsets.Add(input.Text.Length);
            return new PreprocessedIdl(output.ToString(), hasPragmaOnce, CompressOrigins(outputOffsets));
        }
        finally
        {
            currentInput = previousInput;
            diagnostics = previousDiagnostics;
            pragmaOnceCallback = previousPragmaOnceCallback;
            includeProbeCallback = previousIncludeProbe;
            logicalLineOffset = previousLogicalLineOffset;
            logicalFileName = previousLogicalFileName;
            sourceOffsetMap = previousSourceOffsetMap;
            physicalLineStarts = previousPhysicalLineStarts;
            macroWork = previousMacroWork;
            expansionBaseOutputLength = previousExpansionBaseOutputLength;
        }
    }

    private static int[] BuildLineStarts(string source)
    {
        var starts = new List<int> { 0 };
        for (var index = 0; index < source.Length; index++)
        {
            if (source[index] == '\n')
            {
                starts.Add(index + 1);
            }
        }

        return [.. starts];
    }

    private void AppendExpandedOrigins(List<int> origins, string source, string expanded, int sourceOffset)
    {
        var sourceTokens = TokenizeForOrigins(source);
        var expandedTokens = TokenizeForOrigins(expanded);
        var sourceTokenLookup = BuildTokenLookup(source, sourceTokens);
        var sourceIndex = 0;
        var expandedIndex = 0;
        var alignmentWork = 0;

        while (expandedIndex < expandedTokens.Count)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (sourceIndex < sourceTokens.Count
                && TokensEqual(source, sourceTokens[sourceIndex], expanded, expandedTokens[expandedIndex]))
            {
                AppendExactTokenOrigins(origins, sourceTokens[sourceIndex], sourceOffset);
                sourceIndex++;
                expandedIndex++;
                continue;
            }

            var anchor = FindNextMatchingToken(
                source,
                sourceTokens,
                sourceTokenLookup,
                sourceIndex,
                expanded,
                expandedTokens,
                expandedIndex,
                ref alignmentWork);
            var fallback = sourceIndex < sourceTokens.Count
                ? OriginalOffset(sourceOffset + sourceTokens[sourceIndex].Start)
                : OriginalOffset(sourceOffset + source.Length);

            var end = anchor.ExpandedIndex >= 0 ? anchor.ExpandedIndex : expandedTokens.Count;
            while (expandedIndex < end)
            {
                AppendRepeatedOrigin(origins, expandedTokens[expandedIndex].Length, fallback);
                expandedIndex++;
            }

            if (anchor.SourceIndex < 0)
            {
                break;
            }

            sourceIndex = anchor.SourceIndex;
        }
    }

    private static List<(int Start, int Length)> TokenizeForOrigins(string text)
    {
        var tokens = new List<(int Start, int Length)>();
        var index = 0;
        while (index < text.Length)
        {
            var start = index;
            if (StartsPrefixedLiteral(text, index) || IsLiteralStart(text, index))
            {
                index = SkipLiteral(text, index);
            }
            else if (char.IsWhiteSpace(text[index]))
            {
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                {
                    index++;
                }
            }
            else if (IsIdentifierStart(text[index]) || char.IsDigit(text[index]))
            {
                index++;

                while (index < text.Length && (IsIdentifierPart(text[index]) || text[index] == '\''))
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

    private static bool TokensEqual(
        string left,
        (int Start, int Length) leftToken,
        string right,
        (int Start, int Length) rightToken) =>
        leftToken.Length == rightToken.Length
        && string.CompareOrdinal(left, leftToken.Start, right, rightToken.Start, leftToken.Length) == 0;

    private (int SourceIndex, int ExpandedIndex) FindNextMatchingToken(
        string source,
        IReadOnlyList<(int Start, int Length)> sourceTokens,
        IReadOnlyDictionary<(int Length, uint Hash), List<int>> sourceTokenLookup,
        int sourceIndex,
        string expanded,
        IReadOnlyList<(int Start, int Length)> expandedTokens,
        int expandedIndex,
        ref int alignmentWork)
    {
        var sourceEnd = sourceTokens.Count;
        var expandedEnd = expandedTokens.Count;
        var best = (SourceIndex: -1, ExpandedIndex: -1, Length: 0);

        // The token at expandedIndex is the first token that failed to match
        // the source. It is therefore part of the replacement candidate, even
        // when the same spelling occurs later in the original line. Start at
        // the following token so generated text keeps the invocation origin.
        for (var output = expandedIndex + 1; output < expandedEnd; output++)
        {
            if (++alignmentWork > MaximumOriginAlignmentWork)
            {
                return (best.SourceIndex, best.ExpandedIndex);
            }

            if ((alignmentWork & 1023) == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (char.IsWhiteSpace(expanded[expandedTokens[output].Start]))
            {
                continue;
            }

            if (!sourceTokenLookup.TryGetValue(GetTokenKey(expanded, expandedTokens[output]), out var matches))
            {
                continue;
            }

            foreach (var input in matches)
            {
                if (input < sourceIndex || input >= sourceEnd)
                {
                    continue;
                }

                if (char.IsWhiteSpace(source[sourceTokens[input].Start]))
                {
                    continue;
                }

                var run = 0;
                while (input + run < sourceTokens.Count
                    && output + run < expandedTokens.Count
                    && TokensEqual(source, sourceTokens[input + run], expanded, expandedTokens[output + run]))
                {
                    if (++alignmentWork > MaximumOriginAlignmentWork)
                    {
                        return (best.SourceIndex, best.ExpandedIndex);
                    }

                    run++;
                }

                if (run > best.Length
                    || (run == best.Length
                        && input == best.SourceIndex
                        && output > best.ExpandedIndex))
                {
                    best = (input, output, run);
                }

                if (run == sourceEnd - input && output + run == expandedEnd)
                {
                    return (best.SourceIndex, best.ExpandedIndex);
                }
            }
        }

        return (best.SourceIndex, best.ExpandedIndex);
    }

    private void AppendExactTokenOrigins(
        List<int> origins,
        (int Start, int Length) token,
        int sourceOffset)
    {
        for (var index = 0; index < token.Length; index++)
        {
            origins.Add(OriginalOffset(sourceOffset + token.Start + index));
        }
    }

    private static void AppendRepeatedOrigin(List<int> origins, int length, int origin)
    {
        for (var index = 0; index < length; index++)
        {
            origins.Add(origin);
        }
    }

    private static IReadOnlyList<SourceOriginSpan> CompressOrigins(IReadOnlyList<int> origins)
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

    private void AppendOutputOffsets(List<int> outputOffsets, int count, int sourceOffset, int sourceLineEnd)
    {
        for (var index = 0; index < count; index++)
        {
            outputOffsets.Add(OriginalOffset(Math.Min(sourceOffset + index, sourceLineEnd)));
        }
    }
}
