using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Symbols;

/// <summary>Stores declaration names and lookup tables for the front end.</summary>
internal sealed class IdlSymbolTable
{
    private readonly HashSet<string> names = new(StringComparer.Ordinal);
    private GeneratedIdentityIndex generatedIdentities = new();
    private readonly Dictionary<string, IdlEnum> enums = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlUnion> unions = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlTypedef> typedefs = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IdlConstantDeclaration> constants = new(StringComparer.Ordinal);

    /// <summary>Registers a declaration name and reports whether it was new.</summary>
    public bool AddName(string name) => names.Add(name);

    /// <summary>Registers one generated identity when it does not collide.</summary>
    public bool AddGeneratedIdentity(string name) => generatedIdentities.TryAdd(name);

    /// <summary>Registers generated identities atomically when none collide.</summary>
    public bool AddGeneratedIdentities(IReadOnlyList<string> generatedNames)
    {
        if (generatedNames.Count == 1)
        {
            return AddGeneratedIdentity(generatedNames[0]);
        }

        var candidateIndex = generatedIdentities.Clone();
        foreach (var name in generatedNames)
        {
            if (!candidateIndex.TryAdd(name))
            {
                return false;
            }
        }

        generatedIdentities = candidateIndex;
        return true;
    }

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

    private sealed class GeneratedIdentityIndex
    {
        private readonly IdentityNode root = new();

        private GeneratedIdentityIndex(IdentityNode root)
        {
            this.root = root;
        }

        public GeneratedIdentityIndex()
        {
        }

        public GeneratedIdentityIndex Clone() => new(root.Clone());

        public bool TryAdd(string identity)
        {
            var node = root;
            var segments = identity.Split('.');

            foreach (var segment in segments)
            {
                if (node.IsIdentity)
                {
                    return false;
                }

                if (!node.Children.TryGetValue(segment, out var child))
                {
                    child = AddChild(node, segment);
                }

                node = child;
            }

            if (node.IsIdentity || node.Children.Count > 0)
            {
                return false;
            }

            node.IsIdentity = true;
            return true;
        }

        private static IdentityNode AddChild(IdentityNode parent, string segment)
        {
            var child = new IdentityNode();
            parent.Children.Add(segment, child);
            return child;
        }

        private sealed class IdentityNode
        {
            public Dictionary<string, IdentityNode> Children { get; } = new(StringComparer.Ordinal);

            public bool IsIdentity { get; set; }

            public IdentityNode Clone()
            {
                var clone = new IdentityNode { IsIdentity = IsIdentity };
                foreach (var child in Children)
                {
                    clone.Children.Add(child.Key, child.Value.Clone());
                }

                return clone;
            }
        }
    }
}
