namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Binds macro arguments and performs only the prescans allowed by replacement syntax.</summary>
internal sealed class PreprocessorMacroArgumentBinder(
    Func<string, HashSet<string>, int, int, string> expand,
    Func<string, string, bool> needsExpandedParameter)
{
    internal Dictionary<string, (string Raw, string Expanded)> Bind(
        string macroName,
        PreprocessorMacro macro,
        List<string> arguments,
        HashSet<string> expanding,
        int depth,
        int offset)
    {
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>(StringComparer.Ordinal);
        var fixedCount = macro.Variadic ? macro.Parameters!.Count - 1 : macro.Parameters!.Count;
        var wasDisabled = expanding.Remove(macroName);

        try
        {
            for (var index = 0; index < fixedCount; index++)
            {
                var raw = index < arguments.Count ? arguments[index].Trim() : string.Empty;
                var parameter = macro.Parameters[index];
                substitutions[parameter] = (
                    raw,
                    needsExpandedParameter(macro.Body, parameter)
                        ? expand(raw, expanding, depth + 1, offset)
                        : raw);
            }

            if (macro.Variadic)
            {
                var raw = string.Join(", ", arguments.Skip(fixedCount).Select(argument => argument.Trim()));
                var expanded = needsExpandedParameter(macro.Body, macro.VariadicParameterName!)
                    || needsExpandedParameter(macro.Body, "__VA_ARGS__")
                    ? expand(raw, expanding, depth + 1, offset)
                    : raw;
                substitutions[macro.VariadicParameterName!] = (raw, expanded);
                substitutions["__VA_ARGS__"] = (raw, expanded);
            }
        }
        finally
        {
            if (wasDisabled)
            {
                expanding.Add(macroName);
            }
        }

        return substitutions;
    }
}
