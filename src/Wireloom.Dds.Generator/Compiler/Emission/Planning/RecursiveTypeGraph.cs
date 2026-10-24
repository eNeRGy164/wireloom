using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Finds struct types that participate in cycles through sequence members.</summary>
internal sealed class RecursiveTypeGraph
{
    private readonly Dictionary<string, HashSet<string>> sequenceEdges;
    private readonly Dictionary<string, HashSet<string>> recursiveComponents = new(StringComparer.Ordinal);

    /// <summary>Creates the graph from the complete bound declaration set.</summary>
    internal RecursiveTypeGraph(IReadOnlyList<IdlDeclaration> declarations)
    {
        var classes = declarations.OfType<IdlClassDeclaration>()
            .ToDictionary(QualifiedName, StringComparer.Ordinal);
        sequenceEdges = classes.Keys.ToDictionary(
            typeName => typeName,
            _ => new HashSet<string>(StringComparer.Ordinal),
            StringComparer.Ordinal);

        foreach (var declaration in classes.Values)
        {
            var owner = QualifiedName(declaration);
            foreach (var field in declaration.Fields)
            {
                AddSequenceTargets(field.Type, throughSequence: false, owner, classes.Keys);
            }
        }

        foreach (var typeName in sequenceEdges.Keys)
        {
            var reachable = ReachableFrom(typeName);
            var component = reachable
                .Where(candidate => ReachableFrom(candidate).Contains(typeName))
                .ToHashSet(StringComparer.Ordinal);
            if (component.Count > 1 || sequenceEdges[typeName].Contains(typeName))
            {
                recursiveComponents[typeName] = component;
            }
        }
    }

    /// <summary>Gets the sequence-recursive component containing a struct, or an empty set.</summary>
    internal IReadOnlyCollection<string> ComponentFor(string typeName) =>
        recursiveComponents.TryGetValue(typeName, out var component)
            ? component
            : EmptyComponent;

    private static IReadOnlyCollection<string> EmptyComponent { get; } = Array.Empty<string>();

    private void AddSequenceTargets(IdlType type, bool throughSequence, string owner, IEnumerable<string> knownTypes)
    {
        switch (type)
        {
            case IdlType.Alias alias:
                AddSequenceTargets(alias.Target, throughSequence, owner, knownTypes);
                break;
            case IdlType.Sequence sequence:
                AddSequenceTargets(sequence.Element, throughSequence: true, owner, knownTypes);
                break;
            case IdlType.Array array:
                AddSequenceTargets(array.Element, throughSequence, owner, knownTypes);
                break;
            case IdlType.Struct structure when throughSequence && knownTypes.Contains(structure.QualifiedName):
                sequenceEdges[owner].Add(structure.QualifiedName);
                break;
        }
    }

    private HashSet<string> ReachableFrom(string typeName)
    {
        var reachable = new HashSet<string>(StringComparer.Ordinal);
        var pending = new Stack<string>();
        pending.Push(typeName);

        while (pending.Count > 0)
        {
            var current = pending.Pop();
            if (!reachable.Add(current))
            {
                continue;
            }

            foreach (var target in sequenceEdges[current])
            {
                pending.Push(target);
            }
        }

        return reachable;
    }

    private static string QualifiedName(IdlClassDeclaration declaration) =>
        declaration.Namespace is null ? declaration.Name : $"{declaration.Namespace}.{declaration.Name}";
}
