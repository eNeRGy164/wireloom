namespace Wireloom.Dds.CSharp.Compiler.Naming;

/// <summary>Reserves generated C# type identities and detects enclosing-name collisions.</summary>
internal sealed class GeneratedIdentitySet
{
    private readonly GeneratedIdentityIndex generatedIdentities = new();

    /// <summary>Registers one generated identity when it does not collide.</summary>
    public bool AddGeneratedIdentity(string name) => generatedIdentities.TryAdd(name);

    /// <summary>Registers generated identities atomically when none collide.</summary>
    public bool AddGeneratedIdentities(IReadOnlyList<string> generatedNames)
    {
        if (generatedNames.Count == 1)
        {
            return AddGeneratedIdentity(generatedNames[0]);
        }

        var batchIndex = new GeneratedIdentityIndex();

        foreach (var name in generatedNames)
        {
            if (!generatedIdentities.CanAdd(name) || !batchIndex.TryAdd(name))
            {
                return false;
            }
        }

        foreach (var name in generatedNames)
        {
            generatedIdentities.TryAdd(name);
        }

        return true;
    }

    private sealed class GeneratedIdentityIndex
    {
        private readonly IdentityNode root = new();

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

        public bool CanAdd(string identity)
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
                    return true;
                }

                node = child;
            }

            return !node.IsIdentity && node.Children.Count == 0;
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

        }
    }
}
