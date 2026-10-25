namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Parses and registers object-like and function-like macro definitions.</summary>
internal sealed class PreprocessorMacroDefinitionService(
    PreprocessorMacroTable macros,
    PreprocessorMacroTokenService macroTokens)
{
    /// <summary>Registers one object-like or function-like macro definition.</summary>
    /// <summary>Parses and stores a macro definition directive.</summary>
    internal void Define(IdlInput input, int offset, string text)
    {
        var index = 0;
        while (index < text.Length && char.IsWhiteSpace(text[index])) index++;
        var nameStart = index;
        if (index >= text.Length || !PreprocessorLexicalService.IsIdentifierStart(text[index]))
        {
            throw new IdlException(input, offset, "Malformed #define directive.");
        }

        while (++index < text.Length && PreprocessorLexicalService.IsIdentifierPart(text[index]))
        {
        }
        var name = text[nameStart..index];
        var variadic = false;
        string? variadicParameterName = null;
        List<string>? parameters = null;

        if (index < text.Length && text[index] == '(')
        {
            if (!macroTokens.TryReadBalancedText(text, index, out var parameterText, out var end))
            {
                throw new IdlException(input, offset, "Malformed #define parameter list.");
            }

            parameters = ParseParameters(input, offset, parameterText, out variadic, out variadicParameterName);
            index = end;
        }

        while (index < text.Length && text[index] is ' ' or '\t') index++;
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
        var last = parameters[^1].Trim();
        if (last == "...")
        {
            variadic = true;
            variadicParameterName = "__VA_ARGS__";
            parameters = [.. parameters.Take(parameters.Length - 1)];
        }
        else if (last.EndsWith("...", StringComparison.Ordinal) && PreprocessorLexicalService.IsIdentifier(last[..^3]))
        {
            variadic = true;
            variadicParameterName = last[..^3];
            parameters = [.. parameters.Take(parameters.Length - 1)];
        }

        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var parameter in parameters)
        {
            var name = parameter.Trim();
            if (!PreprocessorLexicalService.IsIdentifier(name))
                throw new IdlException(input, offset, "Malformed #define parameter list.");
            if (!seen.Add(name))
                throw new IdlException(input, offset, $"Duplicate macro parameter '{name}'.");
            result.Add(name);
        }

        if (variadic)
        {
            if (!seen.Add(variadicParameterName!))
                throw new IdlException(input, offset, $"Duplicate macro parameter '{variadicParameterName}'.");
            result.Add(variadicParameterName!);
        }

        return result;
    }
}
