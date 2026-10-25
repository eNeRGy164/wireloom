using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>
/// Processes the deterministic preprocessing subset used by IDL inputs.
/// </summary>
internal sealed partial class IdlPreprocessor
{
    /// <summary>Initializes the deterministic preprocessor with its command-line macro state.</summary>
    /// <param name="defines">Macro definitions to register before processing.</param>
    /// <param name="undefines">Macro names to remove from the initial state.</param>
    /// <param name="cancellationToken">Token used to cancel preprocessing.</param>
    public IdlPreprocessor(IEnumerable<string> defines, IEnumerable<string> undefines, CancellationToken cancellationToken = default)
    {
        this.cancellationToken = cancellationToken;
        sourceLocations = new(() => currentInput!, () => sourceOffsetMap, () => logicalLineOffset);
        lexical = new(offset => sourceLocations.OriginalOffset(offset), cancellationToken);
        originTracker = new(offset => sourceLocations.OriginalOffset(offset), cancellationToken);
        macroTokens = new();
        inlineDirectiveService = new(
            macroTokens,
            text => Expand(text),
            (name, angle) => includeProbeCallback?.Invoke(name, angle) == true,
            () => pragmaOnceCallback?.Invoke());
        macroDefinitionService = new(macros, macroTokens);
        argumentBinder = new(
            (text, expanding, depth, offset) => Expand(text, expanding, depth, offset, sourceOffsetMap),
            NeedsExpandedParameter);
        rescanService = new(
            macros,
            macroTokens,
            cancellationToken,
            () => currentInput!,
            Expand,
            AppendExpansion,
            EnsureExpansionLength);
        expansionService = new(
            macros,
            macroTokens,
            expansionState,
            cancellationToken,
            () => currentInput!,
            () => diagnostics,
            (text, index) =>
            {
                var nextIndex = index;
                var handled = inlineDirectiveService.TryExpandHasInclude(text, ref nextIndex, out var value);
                return (Handled: handled, Index: nextIndex, Value: value);
            },
            (text, index) =>
            {
                var nextIndex = index;
                var handled = inlineDirectiveService.TryConsumePragma(text, ref nextIndex);
                return (Handled: handled, Index: nextIndex);
            },
            ExpandFunctionMacro,
            rescanService.RescanReplacementAndSuffix,
            () => macroTokens.Stringify(logicalFileName ?? currentInput!.Path),
            (offset, _) => sourceLocations.GetLogicalLineNumber(offset, physicalLineStarts),
            () => counter++);
        directiveProcessor = new(
            macros,
            EvaluateCondition,
            (text, offset) => Expand(text, offset, sourceOffsetMap),
            macroDefinitionService.Define,
            (lineOffset, fileName) =>
            {
                logicalLineOffset = lineOffset;
                if (fileName is not null)
                {
                    logicalFileName = fileName;
                }
            },
            offset => sourceLocations.GetPhysicalLineNumber(offset, physicalLineStarts),
            () => diagnostics);
        conditionEvaluator = new(
            macros,
            (text, offset) => Expand(text, offset, sourceOffsetMap),
            (text, index) => PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index),
            PreprocessorLexicalService.SkipLiteral,
            PreprocessorLexicalService.IsIdentifierStart,
            PreprocessorLexicalService.IsIdentifierPart,
            text => new PreprocessorConditionalExpressionParser(text).Evaluate(),
            PreprocessorUnsignedComparison.Evaluate,
            cancellationToken);

        foreach (var define in defines)
        {
            var separator = define.IndexOf('=');
            var name = (separator < 0 ? define : define[..separator]).Trim();
            var value = separator < 0 ? "1" : define[(separator + 1)..];

            if (IsIdentifier(name))
            {
                macros[name] = new PreprocessorMacro(null, value, false);
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

    private bool EvaluateCondition(string text, IdlInput input, int sourceOffset, int diagnosticOffset) =>
        conditionEvaluator.Evaluate(text, input, sourceOffset, diagnosticOffset);

    /// <summary>
    /// Processes an input and reports preprocessing metadata needed by the include graph.
    /// </summary>
    /// <summary>Processes one input and reports include and source-location metadata.</summary>
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
        var previousMacroWork = expansionState.MacroWork;
        var previousExpansionBaseOutputLength = expansionState.BaseOutputLength;

        currentInput = input;
        diagnostics = diagnosticSink;
        pragmaOnceCallback = pragmaOnceEncountered;
        includeProbeCallback = includeProbe;
        logicalLineOffset = 0;
        logicalFileName = input.Path;
        expansionState.MacroWork = 0;
        expansionState.BaseOutputLength = 0;
        physicalLineStarts = PreprocessorSourceOrigin.BuildLineStarts(input.Text);

        try
        {
            var source = PreprocessorLexicalService.JoinContinuations(input.Text, out sourceOffsetMap);
            source = lexical.RemoveComments(input, source);

            var output = new StringBuilder(source.Length);
            var outputOffsets = new List<int>(source.Length);
            var conditionals = new PreprocessorConditionalState();
            var hasPragmaOnce = false;
            foreach (var inputLine in inputLineScanner.Scan(source, sourceLocations.OriginalOffset))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var line = inputLine.Text;
                var directive = DirectivePattern.Match(line);
                var outputLength = output.Length;
                expansionState.BaseOutputLength = output.Length;

                if (directive.Success)
                {
                    ProcessDirective(directive, input, inputLine.OriginalOffset, inputLine.Offset, include, conditionals, ref hasPragmaOnce, pragmaOnceEncountered, output, line.Length);
                }
                else if (conditionals.IsActive)
                {
                    var expanded = Expand(line, inputLine.Offset, sourceOffsetMap);
                    output.Append(expanded);
                    originTracker.AppendExpandedOrigins(outputOffsets, line, expanded, inputLine.Offset);
                }
                else
                {
                    output.Append(new string(' ', line.Length));
                }

                if (directive.Success || !conditionals.IsActive)
                {
                    originTracker.AppendOutputOffsets(outputOffsets, output.Length - outputLength, inputLine.Offset, inputLine.End);
                }

                if (inputLine.End < source.Length)
                {
                    output.Append('\n');
                    outputOffsets.Add(sourceLocations.OriginalOffset(inputLine.End));
                }

                if (output.Length > PreprocessorLimits.MaximumOutputLength)
                {
                    throw new IdlException(input, sourceLocations.OriginalOffset(inputLine.End), $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
                }
            }

            conditionals.EnsureComplete(input);

            // The zero-width terminal span gives parser failures at EOF an
            // explicit origin rather than borrowing the last character.
            outputOffsets.Add(input.Text.Length);
            return new PreprocessedIdl(output.ToString(), hasPragmaOnce, PreprocessorSourceOriginTracker.Compress(outputOffsets));
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
            expansionState.MacroWork = previousMacroWork;
            expansionState.BaseOutputLength = previousExpansionBaseOutputLength;
        }
    }


}
