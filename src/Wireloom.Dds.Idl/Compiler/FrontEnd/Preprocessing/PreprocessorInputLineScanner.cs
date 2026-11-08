namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Enumerates logical input lines while retaining their output and source boundaries.</summary>
internal sealed class PreprocessorInputLineScanner
{
    /// <summary>Scans source text into logical lines while preserving source offsets.</summary>
    /// <summary>Scans source text into logical preprocessor input lines.</summary>
    internal IEnumerable<PreprocessorInputLine> Scan(string source, Func<int, int> originalOffset)
    {
        var offset = 0;
        while (offset < source.Length)
        {
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

            yield return new PreprocessorInputLine(source.Substring(offset, contentEnd - offset), offset, lineEnd, originalOffset(offset));
            offset = lineEnd < source.Length ? lineEnd + 1 : source.Length;
        }
    }
}

/// <summary>Stores one logical input line and its source boundaries.</summary>
internal sealed class PreprocessorInputLine(string text, int offset, int end, int originalOffset)
{
    /// <summary>Gets the line text without its line terminator.</summary>
    internal string Text { get; } = text;
    /// <summary>Gets the zero-based offset of the line in preprocessed text.</summary>
    internal int Offset { get; } = offset;
    /// <summary>Gets the exclusive end offset of the line in preprocessed text.</summary>
    internal int End { get; } = end;
    /// <summary>Gets the corresponding offset in the original input.</summary>
    internal int OriginalOffset { get; } = originalOffset;
}
