using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Symbols;

/// <summary>Stores declaration names and lookup tables for the front end.</summary>
internal sealed class IdlSymbolTable
{
    private readonly HashSet<string> names = new(StringComparer.Ordinal);
    private readonly HashSet<string> generatedIdentities = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlEnum> enums = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlUnion> unions = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlTypedef> typedefs = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlConstantDeclaration> constants = new(StringComparer.Ordinal);

    /// <summary>Registers a declaration name and reports whether it was new.</summary>
    public bool AddName(string name) => names.Add(name);

    public bool AddGeneratedIdentity(string name)
    {
        if (generatedIdentities.Any(existing => Conflicts(existing, name)))
        {
            return false;
        }

        generatedIdentities.Add(name);

        return true;
    }

    public bool AddGeneratedIdentities(IReadOnlyList<string> generatedNames)
    {
        if (generatedNames
            .SelectMany((name, index) => generatedNames.Skip(index + 1).Select(other => (name, other)))
            .Any(pair => Conflicts(pair.name, pair.other))
            || generatedNames.Any(name => generatedIdentities.Any(existing => Conflicts(existing, name))))
        {
            return false;
        }

        foreach (var name in generatedNames)
        {
            generatedIdentities.Add(name);
        }

        return true;
    }

    private static bool Conflicts(string first, string second) =>
        string.Equals(first, second, StringComparison.Ordinal)
        || first.StartsWith($"{second}.", StringComparison.Ordinal)
        || second.StartsWith($"{first}.", StringComparison.Ordinal);

    /// <summary>Checks whether a declaration name has been registered.</summary>
    public bool ContainsName(string name) => names.Contains(name);

    /// <summary>Checks whether an enum has been registered.</summary>
    public bool ContainsEnum(string name) => enums.ContainsKey(name);

    /// <summary>Checks whether a union has been registered.</summary>
    public bool ContainsUnion(string name) => unions.ContainsKey(name);

    /// <summary>Checks whether a typedef has been registered.</summary>
    public bool ContainsTypedef(string name) => typedefs.ContainsKey(name);

    /// <summary>Checks whether a constant has been registered.</summary>
    public bool ContainsConstant(string name) => constants.ContainsKey(name);

    public bool TryGetConstant(string name, out IdlConstantDeclaration declaration) =>
        constants.TryGetValue(name, out declaration!);

    public bool TryGetEnum(string name, out IdlEnum declaration) => enums.TryGetValue(name, out declaration!);

    public bool TryGetTypedef(string name, out IdlTypedef declaration) => typedefs.TryGetValue(name, out declaration!);

    /// <summary>Registers an enum while retaining the first declaration for binding.</summary>
    public void AddEnum(string name, IdlEnum declaration) => enums.TryAdd(name, declaration);

    /// <summary>Registers a union while retaining the first declaration for binding.</summary>
    public void AddUnion(string name, IdlUnion declaration) => unions.TryAdd(name, declaration);

    /// <summary>Registers a typedef while retaining the first declaration for binding.</summary>
    public void AddTypedef(string name, IdlTypedef declaration) => typedefs.TryAdd(name, declaration);

    /// <summary>Registers a constant while retaining the first declaration for binding.</summary>
    public void AddConstant(string name, IdlConstantDeclaration declaration) => constants.TryAdd(name, declaration);
}
