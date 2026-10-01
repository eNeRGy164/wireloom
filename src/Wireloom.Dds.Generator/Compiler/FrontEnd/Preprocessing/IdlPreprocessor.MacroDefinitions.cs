namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private void Define(IdlInput input, int offset, string text)
    {
        var index = 0;
        while (index < text.Length && char.IsWhiteSpace(text[index]))
        {
            index++;
        }

        var nameStart = index;
        if (index >= text.Length || !IsIdentifierStart(text[index]))
        {
            throw new IdlException(input, offset, "Malformed #define directive.");
        }

        while (++index < text.Length && IsIdentifierPart(text[index]))
        {
        }

        var name = text[nameStart..index];
        var variadic = false;
        string? variadicParameterName = null;
        List<string>? parameters = null;

        // Function-like macros require the opening parenthesis immediately
        // after the name. Whitespace instead starts an object-like body.
        if (index < text.Length && text[index] == '(')
        {
            if (!macroTokens.TryReadBalancedText(text, index, out var parameterText, out var end))
            {
                throw new IdlException(input, offset, "Malformed #define parameter list.");
            }

            parameters = ParseParameters(input, offset, parameterText, out variadic, out variadicParameterName);
            index = end;
        }

        while (index < text.Length && (text[index] == ' ' || text[index] == '\t'))
        {
            index++;
        }

        var body = index < text.Length ? text[index..] : string.Empty;
        macros[name] = new PreprocessorMacro(parameters, body, parameters is not null && variadic, variadicParameterName);
    }

    private static List<string> ParseParameters(IdlInput input, int offset, string text, out bool variadic, out string? variadicParameterName)
    {
        variadic = false;
        variadicParameterName = null;

        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var parameters = text.Split(',');
        if (parameters.Length > 0)
        {
            var last = parameters[^1].Trim();
            if (last == "...")
            {
                variadic = true;
                variadicParameterName = "__VA_ARGS__";
                parameters = [.. parameters.Take(parameters.Length - 1)];
            }
            else if (last.EndsWith("...", StringComparison.Ordinal) && IsIdentifier(last[..^3]))
            {
                variadic = true;
                variadicParameterName = last[..^3];
                parameters = [.. parameters.Take(parameters.Length - 1)];
            }
        }

        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var parameter in parameters)
        {
            var name = parameter.Trim();
            if (!IsIdentifier(name))
            {
                throw new IdlException(input, offset, "Malformed #define parameter list.");
            }

            if (!seen.Add(name))
            {
                throw new IdlException(input, offset, $"Duplicate macro parameter '{name}'.");
            }

            result.Add(name);
        }

        if (variadic)
        {
            if (!seen.Add(variadicParameterName!))
            {
                throw new IdlException(input, offset, $"Duplicate macro parameter '{variadicParameterName}'.");
            }

            result.Add(variadicParameterName!);
        }

        return result;
    }

}
