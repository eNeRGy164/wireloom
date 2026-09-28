namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private int OriginalOffset(int offset)
    {
        if (sourceOffsetMap is null || offset < 0)
        {
            return offset;
        }

        return offset < sourceOffsetMap.Length ? sourceOffsetMap[offset] : currentInput!.Text.Length;
    }

    private int GetPhysicalLineNumber(int offset)
    {
        offset = Math.Min(Math.Max(offset, 0), currentInput!.Text.Length);
        var index = Array.BinarySearch(physicalLineStarts, offset);
        return index >= 0 ? index + 1 : ~index;
    }

    private int GetLogicalLineNumber(int offset) =>
        GetPhysicalLineNumber(offset) + logicalLineOffset;
}
