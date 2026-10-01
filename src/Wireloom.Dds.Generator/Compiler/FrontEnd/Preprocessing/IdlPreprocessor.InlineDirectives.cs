namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private bool TryExpandHasInclude(string text, ref int index, out bool result)
    {
        result = false;

        var open = index;
        while (open < text.Length && char.IsWhiteSpace(text[open]))
        {
            open++;
        }

        if (open >= text.Length || text[open] != '(' || !macroTokens.TryReadArguments(text, open, out var arguments, out var end))
        {
            return false;
        }

        if (arguments.Count == 1)
        {
            var match = IncludePattern.Match(Expand(arguments[0]).Trim());
            if (match.Success)
            {
                var name = match.Groups["quoted"].Success
                    ? match.Groups["quoted"].Value
                    : match.Groups["angle"].Value;

                result = includeProbeCallback?.Invoke(name, match.Groups["angle"].Success) == true;
            }
        }

        index = end;

        return true;
    }

    private bool TryConsumeInlinePragma(string text, ref int index)
    {
        string? name = null;
        if (text.AsSpan(index).StartsWith("_Pragma", StringComparison.Ordinal))
        {
            name = "_Pragma";
        }
        else if (text.AsSpan(index).StartsWith("__pragma", StringComparison.Ordinal))
        {
            name = "__pragma";
        }

        if (name is null || (index > 0 && IsIdentifierPart(text[index - 1])))
        {
            return false;
        }

        var open = index + name.Length;
        while (open < text.Length && char.IsWhiteSpace(text[open]))
        {
            open++;
        }

        if (open >= text.Length || text[open] != '(' || !macroTokens.TryReadArguments(text, open, out var arguments, out var end))
        {
            return false;
        }

        if (arguments.Count == 1)
        {
            var argument = arguments[0].Trim();
            var isOnce = name == "_Pragma"
                ? argument is ['"', .., '"']
                    && string.Equals(argument[1..^1], "once", StringComparison.Ordinal)
                : string.Equals(argument, "once", StringComparison.Ordinal);

            if (isOnce)
            {
                pragmaOnceCallback?.Invoke();
            }
        }

        index = end;

        return true;
    }
}
