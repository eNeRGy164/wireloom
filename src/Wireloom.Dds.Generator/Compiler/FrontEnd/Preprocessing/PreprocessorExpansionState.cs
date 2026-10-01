namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Owns mutable limits and accounting shared by recursive macro expansion.</summary>
internal sealed class PreprocessorExpansionState
{
    internal int MacroWork { get; set; }

    internal int BaseOutputLength { get; set; }

    internal void CountOperation(IdlInput input, int offset, IReadOnlyList<int>? sourceOffsets, int depth)
    {
        MacroWork = checked(MacroWork + 1);
        if (MacroWork > PreprocessorLimits.MaximumMacroWork)
        {
            throw new IdlException(input, MapOffset(offset, sourceOffsets), $"Macro expansion exceeds the {PreprocessorLimits.MaximumMacroWork}-operation limit.");
        }

        if (depth > PreprocessorLimits.MaximumMacroDepth)
        {
            throw new IdlException(input, MapOffset(offset, sourceOffsets), $"Macro expansion exceeds the {PreprocessorLimits.MaximumMacroDepth}-level nesting limit.");
        }
    }

    internal void EnsureOutputLength(IdlInput input, int length, int offset)
    {
        if (length > PreprocessorLimits.MaximumOutputLength - BaseOutputLength)
        {
            throw new IdlException(input, offset, $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
        }
    }

    private static int MapOffset(int offset, IReadOnlyList<int>? sourceOffsets) =>
        sourceOffsets is not null && offset >= 0 && offset < sourceOffsets.Count
            ? sourceOffsets[offset]
            : offset;
}
