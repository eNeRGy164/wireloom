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

    public IEnumerable<string> TypedefNames => typedefs.Keys;

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

    public bool ContainsName(string name) => names.Contains(name);

    public bool ContainsEnum(string name) => enums.ContainsKey(name);

    public bool ContainsUnion(string name) => unions.ContainsKey(name);

    public bool ContainsTypedef(string name) => typedefs.ContainsKey(name);

    public bool ContainsConstant(string name) => constants.ContainsKey(name);

    public bool TryGetConstant(string name, out IdlConstantDeclaration declaration) =>
        constants.TryGetValue(name, out declaration!);

    public bool TryGetEnum(string name, out IdlEnum declaration) => enums.TryGetValue(name, out declaration!);

    public bool TryGetTypedef(string name, out IdlTypedef declaration) => typedefs.TryGetValue(name, out declaration!);

    public void AddEnum(string name, IdlEnum declaration) => enums.Add(name, declaration);

    public void AddUnion(string name, IdlUnion declaration) => unions.Add(name, declaration);

    public void AddTypedef(string name, IdlTypedef declaration) => typedefs.Add(name, declaration);

    public void AddConstant(string name, IdlConstantDeclaration declaration) => constants.Add(name, declaration);
}
