using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Applies RTI's leading-underscore identifier escape after preprocessing.</summary>
internal static class IdlEscapeCharNormalizer
{
    /// <summary>Removes one leading underscore from each identifier and preserves source origins.</summary>
    internal static PreprocessedIdl Normalize(PreprocessedIdl input, IdlInput idlInput, CancellationToken cancellationToken)
    {
        var text = input.Text;
        var output = new StringBuilder(text.Length);
        var outputOrigins = new List<int>(text.Length + 1);
        var escapedIdentifierOffsets = new HashSet<int>();
        var index = 0;

        while (index < text.Length)
        {
            if ((index & 1023) == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index))
            {
                var literalEnd = PreprocessorLexicalService.SkipLiteral(text, index);
                AppendUnchanged(text, index, literalEnd, input.SourceOrigins, output, outputOrigins, cancellationToken);
                index = literalEnd;
                continue;
            }

            if (PreprocessorLexicalService.IsIdentifierStart(text[index]))
            {
                var identifierEnd = index + 1;
                while (identifierEnd < text.Length && PreprocessorLexicalService.IsIdentifierPart(text[identifierEnd]))
                {
                    identifierEnd++;
                }

                if (text[index] == '_' && identifierEnd - index == 1)
                {
                    throw new IdlException(
                        idlInput,
                        MapSourceOffset(index, input.SourceOrigins),
                        "The IDL identifier '_' cannot be used with DdsIdlEnableEscapeChar because removing its leading underscore leaves an empty name.");
                }

                if (text[index] == '_')
                {
                    escapedIdentifierOffsets.Add(output.Length);
                    AppendUnchanged(text, index + 1, identifierEnd, input.SourceOrigins, output, outputOrigins, cancellationToken, index);
                }
                else
                {
                    AppendUnchanged(text, index, identifierEnd, input.SourceOrigins, output, outputOrigins, cancellationToken);
                }

                index = identifierEnd;
                continue;
            }

            output.Append(text[index]);
            outputOrigins.Add(MapSourceOffset(index, input.SourceOrigins));
            index++;
        }

        outputOrigins.Add(input.SourceOrigins.Count == 0 ? text.Length : input.SourceOrigins[^1].SourceStart);

        return new PreprocessedIdl(
            output.ToString(),
            input.HasPragmaOnce,
            PreprocessorSourceOriginTracker.Compress(outputOrigins),
            escapedIdentifierOffsets);
    }

    private static void AppendUnchanged(
        string text,
        int start,
        int end,
        IReadOnlyList<SourceOriginSpan> sourceOrigins,
        StringBuilder output,
        List<int> outputOrigins,
        CancellationToken cancellationToken,
        int? firstCharacterSourceOffset = null)
    {
        for (var index = start; index < end; index++)
        {
            if ((index & 1023) == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            output.Append(text[index]);
            outputOrigins.Add(MapSourceOffset(
                index == start && firstCharacterSourceOffset is not null ? firstCharacterSourceOffset.Value : index,
                sourceOrigins));
        }
    }

    private static int MapSourceOffset(int offset, IReadOnlyList<SourceOriginSpan> sourceOrigins)
    {
        if (sourceOrigins.Count == 0)
        {
            return offset;
        }

        var low = 0;
        var high = sourceOrigins.Count - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var origin = sourceOrigins[middle];
            if (offset < origin.OutputStart)
            {
                high = middle - 1;
            }
            else if (offset >= origin.OutputStart + origin.OutputLength)
            {
                low = middle + 1;
            }
            else
            {
                return origin.Map(offset);
            }
        }

        return sourceOrigins[^1].SourceStart;
    }
}
