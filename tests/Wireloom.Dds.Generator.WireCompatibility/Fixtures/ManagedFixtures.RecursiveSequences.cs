#nullable enable annotations

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds finite recursive sequence samples for targeted DDS wire probes.</summary>
internal static partial class ManagedFixtures
{
    private static T CreateDirectRecursiveSequenceFixture<T>()
    {
        // Arrange
        var root = CreateNode(6101);
        var child = CreateNode(6102);

        // Act
        AddChild(root, child);
        AddChild(child, CreateNode(6103));

        // Assert
        return root;

        static T CreateNode(int value)
        {
            var node = Activator.CreateInstance<T>()!;
            typeof(T).GetProperty("value")!.SetValue(node, value);
            return node;
        }

        static void AddChild(T parent, T child)
        {
            var children = typeof(T).GetProperty("children")!.GetValue(parent)!;
            var add = children.GetType().GetMethods()
                .Single(method => method.Name == "Add"
                    && method.GetParameters() is [{ ParameterType: var parameterType }]
                    && parameterType.IsAssignableFrom(typeof(T)));
            add.Invoke(children, [child]);
        }
    }
}
