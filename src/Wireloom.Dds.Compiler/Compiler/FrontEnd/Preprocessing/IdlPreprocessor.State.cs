namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Stores mutable state used by IDL preprocessing.</summary>
internal sealed partial class IdlPreprocessor
{
    private readonly PreprocessorMacroTable macros = new();
    private IdlInput? currentInput;
    private ICollection<IdlDiagnostic>? diagnostics;
    private Action? pragmaOnceCallback;
    private Func<string, bool, bool>? includeProbeCallback;
    private int logicalLineOffset;
    private string? logicalFileName;
    private int[]? sourceOffsetMap;
    private int counter;
    private readonly CancellationToken cancellationToken;
    private readonly PreprocessorExpansionState expansionState = new();
    private int[] physicalLineStarts = [0];
    private readonly PreprocessorExpressionEvaluator conditionEvaluator;
    private readonly PreprocessorLexicalService lexical;
    private readonly PreprocessorSourceOriginTracker originTracker;
    private readonly PreprocessorDirectiveProcessor directiveProcessor;
    private readonly PreprocessorMacroTokenService macroTokens;
    private readonly PreprocessorMacroExpansionService expansionService;
    private readonly PreprocessorMacroArgumentBinder argumentBinder;
    private readonly PreprocessorMacroRescanService rescanService;
    private readonly PreprocessorInlineDirectiveService inlineDirectiveService;
    private readonly PreprocessorMacroDefinitionService macroDefinitionService;
    private readonly PreprocessorInputLineScanner inputLineScanner = new();
    private readonly PreprocessorSourceLocationService sourceLocations;
}
