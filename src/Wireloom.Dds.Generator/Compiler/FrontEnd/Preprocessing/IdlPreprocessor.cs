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
        lexical = new(OriginalOffset, cancellationToken);
        originTracker = new(OriginalOffset, cancellationToken);
        macroTokens = new();
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
            expansionState,
            cancellationToken,
            () => currentInput!,
            () => diagnostics,
            (text, index) =>
            {
                var nextIndex = index;
                var handled = TryExpandHasInclude(text, ref nextIndex, out var value);
                return (Handled: handled, Index: nextIndex, Value: value);
            },
            (text, index) =>
            {
                var nextIndex = index;
                var handled = TryConsumeInlinePragma(text, ref nextIndex);
                return (Handled: handled, Index: nextIndex);
            },
            ExpandFunctionMacro,
            rescanService.RescanReplacementAndSuffix,
            () => macroTokens.Stringify(logicalFileName ?? currentInput!.Path),
            (offset, _) => GetLogicalLineNumber(offset),
            () => counter++);
        directiveProcessor = new(
            macros,
            EvaluateCondition,
            (text, offset) => Expand(text, offset, sourceOffsetMap),
            Define,
            (lineOffset, fileName) =>
            {
                logicalLineOffset = lineOffset;
                if (fileName is not null)
                {
                    logicalFileName = fileName;
                }
            },
            GetPhysicalLineNumber,
            () => diagnostics);
        conditionEvaluator = new(
            macros,
            (text, offset) => Expand(text, offset, sourceOffsetMap),
            (text, index) => PreprocessorLexicalService.StartsPrefixedLiteral(text, index)
                || PreprocessorLexicalService.IsLiteralStart(text, index),
            PreprocessorLexicalService.SkipLiteral,
            PreprocessorLexicalService.IsIdentifierStart,
            PreprocessorLexicalService.IsIdentifierPart,
            text => new ConditionalExpression(text).Evaluate(),
            EvaluateUnsignedComparison,
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
                expansionState.BaseOutputLength = output.Length;

                if (directive.Success)
                {
                    ProcessDirective(directive, input, OriginalOffset(offset), offset, include, conditionals, ref hasPragmaOnce, pragmaOnceEncountered, output, line.Length);
                }
                else if (conditionals.IsActive)
                {
                    var expanded = Expand(line, offset, sourceOffsetMap);
                    output.Append(expanded);
                    originTracker.AppendExpandedOrigins(outputOffsets, line, expanded, offset);
                }
                else
                {
                    output.Append(new string(' ', line.Length));
                }

                if (directive.Success || !conditionals.IsActive)
                {
                    originTracker.AppendOutputOffsets(outputOffsets, output.Length - outputLength, offset, lineEnd);
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

                if (output.Length > PreprocessorLimits.MaximumOutputLength)
                {
                    throw new IdlException(input, OriginalOffset(offset), $"Preprocessor output exceeds the {PreprocessorLimits.MaximumOutputLength}-character limit.");
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
