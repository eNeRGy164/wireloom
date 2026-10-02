namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Maps preprocessed offsets to physical and logical source locations for one run.</summary>
internal sealed class PreprocessorSourceLocationService(
    Func<IdlInput> input,
    Func<int[]?> sourceOffsetMap,
    Func<int> logicalLineOffset)
{
    /// <summary>Maps a preprocessed offset back to the original input.</summary>
    /// <summary>Maps a preprocessed offset to its original source offset.</summary>
    internal int OriginalOffset(int offset)
    {
        var map = sourceOffsetMap();
        if (map is null || offset < 0)
        {
            return offset;
        }

        return offset < map.Length ? map[offset] : input().Text.Length;
    }

    /// <summary>Gets the one-based physical source line for an offset.</summary>
    /// <summary>Gets the physical source line containing an offset.</summary>
    internal int GetPhysicalLineNumber(int offset, int[] physicalLineStarts)
    {
        offset = Math.Min(Math.Max(offset, 0), input().Text.Length);
        var index = Array.BinarySearch(physicalLineStarts, offset);
        return index >= 0 ? index + 1 : ~index;
    }

    /// <summary>Gets the one-based logical source line for an offset.</summary>
    /// <summary>Gets the logical source line containing an offset.</summary>
    internal int GetLogicalLineNumber(int offset, int[] physicalLineStarts) =>
        GetPhysicalLineNumber(offset, physicalLineStarts) + logicalLineOffset();
}
