namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Deterministic safety limits shared by preprocessing stages.</summary>
internal static class PreprocessorLimits
{
    internal const int MaximumMacroWork = 100_000;
    internal const int MaximumMacroDepth = 64;
    internal const int MaximumOriginAlignmentWork = 100_000;
    internal const int MaximumOutputLength = 4 * 1024 * 1024;
    internal const int MaximumExpressionNesting = 256;
}
