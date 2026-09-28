namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private readonly Dictionary<string, Macro> macros = new(StringComparer.Ordinal);
    private IdlInput? currentInput;
    private ICollection<IdlDiagnostic>? diagnostics;
    private Action? pragmaOnceCallback;
    private Func<string, bool, bool>? includeProbeCallback;
    private int logicalLineOffset;
    private string? logicalFileName;
    private int[]? sourceOffsetMap;
    private int counter;
    private readonly CancellationToken cancellationToken;
    private int macroWork;
    private int expansionBaseOutputLength;
    private int[] physicalLineStarts = [0];

    private const int MaximumMacroWork = 100_000;
    private const int MaximumMacroDepth = 64;
    private const int MaximumOriginAlignmentWork = 100_000;
    private const int MaximumOutputLength = 4 * 1024 * 1024;
}
