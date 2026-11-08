namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Owns the mutable macro definitions shared by preprocessing stages.</summary>
internal sealed class PreprocessorMacroTable
{
    private readonly Dictionary<string, PreprocessorMacro> definitions = new(StringComparer.Ordinal);

    internal PreprocessorMacro this[string name]
    {
        set => definitions[name] = value;
    }

    /// <summary>Determines whether a macro with the supplied name exists.</summary>
    internal bool ContainsKey(string name) => definitions.ContainsKey(name);

    /// <summary>Removes a macro definition when present.</summary>
    internal void Remove(string name) => definitions.Remove(name);

    /// <summary>Gets a macro definition by name when present.</summary>
    internal bool TryGetValue(string name, out PreprocessorMacro macro) =>
        definitions.TryGetValue(name, out macro!);
}

internal sealed class PreprocessorMacro(
    List<string>? parameters,
    string body,
    bool variadic,
    string? variadicParameterName = null)
{
    public List<string>? Parameters { get; } = parameters;
    public string Body { get; } = body;
    public bool Variadic { get; } = variadic;
    public string? VariadicParameterName { get; } = variadicParameterName;
}
