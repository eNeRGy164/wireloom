namespace Wireloom;

/// <summary>Stores declaration names and lookup tables for the front end.</summary>
internal sealed class IdlSymbolTable
{
    private readonly HashSet<string> names = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlEnum> enums = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlUnion> unions = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlTypedef> typedefs = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlConstantDeclaration> constants = new(StringComparer.Ordinal);

    public IEnumerable<string> TypedefNames => typedefs.Keys;

    public IEnumerable<IdlConstantDeclaration> Constants => constants.Values;

    public bool AddName(string name) => names.Add(name);

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
