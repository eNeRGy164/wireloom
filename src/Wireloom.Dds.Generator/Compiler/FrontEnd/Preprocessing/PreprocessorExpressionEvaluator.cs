using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Evaluates preprocessor conditions using explicit macro and lexical dependencies.</summary>
internal sealed class PreprocessorExpressionEvaluator(
    PreprocessorMacroTable macros,
    Func<string, int, string> expand,
    Func<string, int, bool> isLiteralStart,
    Func<string, int, int> skipLiteral,
    Func<char, bool> isIdentifierStart,
    Func<char, bool> isIdentifierPart,
    Func<string, long> evaluateExpression,
    Func<string, (bool Matched, bool Result)> evaluateUnsignedComparison,
    CancellationToken cancellationToken)
{
    /// <summary>Evaluates a preprocessor expression and reports diagnostics against the input.</summary>
    internal bool Evaluate(string text, IdlInput input, int sourceOffset, int diagnosticOffset)
    {
        var expanded = ReplaceDefinedOperators(text, input, diagnosticOffset);
        var expression = expand(expanded, sourceOffset).Trim();

        try
        {
            if (expression.Contains("++", StringComparison.Ordinal)
                || expression.Contains("--", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("increment and decrement operators are not valid in preprocessor conditions");
            }

            var unsignedComparison = evaluateUnsignedComparison(expression);
            if (unsignedComparison.Matched)
            {
                return unsignedComparison.Result;
            }

            return evaluateExpression(expression) != 0;
        }
        catch (InvalidOperationException exception)
        {
            throw new IdlException(input, diagnosticOffset, $"Invalid preprocessor condition: {exception.Message}");
        }
        catch (ArithmeticException exception)
        {
            throw new IdlException(input, diagnosticOffset, $"Invalid preprocessor condition: {exception.Message}");
        }
    }

    private string ReplaceDefinedOperators(string text, IdlInput input, int offset)
    {
        var output = new StringBuilder(text.Length);
        var copiedThrough = 0;
        var scanIndex = 0;

        while (scanIndex < text.Length)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (isLiteralStart(text, scanIndex))
            {
                scanIndex = skipLiteral(text, scanIndex);
                continue;
            }

            if (!IsDefinedOperatorAt(text, scanIndex))
            {
                scanIndex++;
                continue;
            }

            output.Append(text, copiedThrough, scanIndex - copiedThrough);
            var position = scanIndex + "defined".Length;
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }

            var name = ReadDefinedName(text, ref position, input, offset);
            output.Append(macros.ContainsKey(name) ? '1' : '0');
            copiedThrough = position;
            scanIndex = position;
        }

        output.Append(text, copiedThrough, text.Length - copiedThrough);
        return output.ToString();
    }

    private string ReadDefinedName(string text, ref int position, IdlInput input, int offset)
    {
        var parenthesized = position < text.Length && text[position] == '(';
        if (parenthesized)
        {
            position++;
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }
        }

        var nameStart = position;
        if (position >= text.Length || !isIdentifierStart(text[position]))
        {
            throw InvalidDefinedOperator(input, offset);
        }

        while (position < text.Length && isIdentifierPart(text[position]))
        {
            position++;
        }

        var name = text[nameStart..position];
        if (parenthesized)
        {
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }

            if (position >= text.Length || text[position] != ')')
            {
                throw InvalidDefinedOperator(input, offset);
            }

            position++;
        }

        return name;
    }

    private bool IsDefinedOperatorAt(string text, int index) =>
        text.AsSpan(index).StartsWith("defined", StringComparison.Ordinal)
        && (index == 0 || !isIdentifierPart(text[index - 1]))
        && (index + "defined".Length == text.Length
            || !isIdentifierPart(text[index + "defined".Length]));

    private static IdlException InvalidDefinedOperator(IdlInput input, int offset) =>
        new(input, offset, "Invalid preprocessor condition: malformed defined operator.");
}
