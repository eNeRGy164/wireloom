using Rti.Dds.Topics;
using RuntimeRecursiveStartup;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Checks typed recursive samples without invoking generated equality or formatting.</summary>
internal static class RecursiveStartupCases
{
    /// <summary>Exercises independently uninitialized support types through the requested entry point.</summary>
    internal static void Verify(string mode)
    {
        VerifyShape(mode, () => NodeSupport.Instance,
            () => new Node { value = 10 },
            (node, value) => node.value = value, node => node.value,
            (node, children) => node.children = children, node => node.children);
        VerifyShape(mode, () => AliasNodeSupport.Instance,
            () => new AliasNode { value = 10 },
            (node, value) => node.value = value, node => node.value,
            (node, children) => node.children = children, node => node.children);
        VerifyMatrix(mode);
        VerifySequence(mode);
    }

    private static TypeSupport<T> Support<T>(string mode, Func<TypeSupport<T>> instance)
    {
        if (mode == "discovery")
        {
            return (TypeSupport<T>)TypeSupportHelper.GetTypeSupportForType(typeof(T));
        }

        mode.ShouldBe("instance");
        return instance();
    }

    private static void VerifyShape<T>(string mode, Func<TypeSupport<T>> instance, Func<T> create,
        Action<T, int> setValue, Func<T, int> value, Action<T, T[]> setChildren, Func<T, T[]?> children)
    {
        // Arrange
        var support = Support(mode, instance);
        using var serializer = support.CreateSerializer();
        var root = create();

        // Act / Assert: absent, present, then nested, including absent leaf optionals.
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        var first = create();
        var second = create();
        setValue(first, 21);
        setValue(second, 22);
        setChildren(root, [first, second]);
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        var leaf = create();
        var sibling = create();
        setValue(leaf, 31);
        setValue(sibling, 32);
        setChildren(first, [leaf, sibling]);
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        Should.Throw<NotSupportedException>(() => support.DynamicType);

        void Check(T expected, T actual)
        {
            value(actual).ShouldBe(value(expected));
            var expectedChildren = children(expected);
            var actualChildren = children(actual);
            if (expectedChildren is null)
            {
                actualChildren.ShouldBeNull();
                return;
            }

            actualChildren.ShouldNotBeNull();
            actualChildren.Length.ShouldBe(2);
            for (var index = 0; index < 2; index++)
            {
                Check(expectedChildren[index], actualChildren[index]);
            }
        }
    }

    private static void VerifyMatrix(string mode)
    {
        // Arrange
        var support = Support(mode, () => MatrixNodeSupport.Instance);
        using var serializer = support.CreateSerializer();
        var root = new MatrixNode { value = 40 };

        // Act / Assert
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        root.children = Children(50);
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        root.children[1, 2].children = Children(60);
        Check(root, serializer.Deserialize(serializer.Serialize(root)));
        Should.Throw<NotSupportedException>(() => support.DynamicType);

        static MatrixNode[,] Children(int offset)
        {
            var result = new MatrixNode[2, 3];
            for (var row = 0; row < 2; row++)
            {
                for (var column = 0; column < 3; column++)
                {
                    result[row, column] = new MatrixNode { value = offset + row * 3 + column };
                }
            }

            return result;
        }

        static void Check(MatrixNode expected, MatrixNode actual)
        {
            actual.value.ShouldBe(expected.value);
            if (expected.children is null)
            {
                actual.children.ShouldBeNull();
                return;
            }

            actual.children.ShouldNotBeNull();
            actual.children.GetLength(0).ShouldBe(2);
            actual.children.GetLength(1).ShouldBe(3);
            for (var row = 0; row < 2; row++)
            {
                for (var column = 0; column < 3; column++)
                {
                    Check(expected.children[row, column], actual.children[row, column]);
                }
            }
        }
    }

    private static void VerifySequence(string mode)
    {
        // Arrange
        var support = Support(mode, () => SequenceNodeSupport.Instance);

        // Act / Assert: this control checks initialization, not recursive sequence allocation.
        support.ShouldNotBeNull();
        Should.Throw<NotSupportedException>(() => support.DynamicType);
    }
}
