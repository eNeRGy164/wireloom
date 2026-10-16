using CorpusOptionalRecursiveArray;

namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Checks RTI-compatible managed behavior from the freshly packed analyzer.</summary>
public sealed class OptionalRecursiveArraySpecs
{
    [Fact]
    public void RecursiveArraySupportRejectsPublicDynamicType()
    {
        // Arrange
        var support = NodeSupport.Instance;

        // Act
        var exception = Should.Throw<NotSupportedException>(() => support.DynamicType);

        // Assert
        support.ShouldNotBeNull();
        exception.Message.ShouldContain("Recursive types don't support");
    }

    [Fact]
    public void PreservesRecursiveSequenceSupportInitialization()
    {
        // Arrange
        var support = CorpusAggregates.RecursiveSupport.Instance;

        // Act
        var exception = Should.Throw<NotSupportedException>(() => support.DynamicType);

        // Assert
        support.ShouldNotBeNull();
        exception.Message.ShouldContain("Recursive types don't support");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HashesTheFirstChildOrTheAbsentSentinel(bool present)
    {
        // Arrange
        var sample = new Node { value = 9801 };
        if (present)
        {
            sample.children = [new Node { value = 9811 }, new Node { value = 9812 }];
        }

        var expected = new HashCode();
        expected.Add(sample.value);
        if (present)
        {
            expected.Add(sample.children![0]);
        }
        else
        {
            expected.Add(-1);
        }

        // Act
        var actual = sample.GetHashCode();

        // Assert
        actual.ShouldBe(expected.ToHashCode());
        if (present)
        {
            sample.children![1].value++;
            sample.GetHashCode().ShouldBe(actual);
        }
    }

    [Fact]
    public void CopiesNestedOptionalArraysIntoIndependentEqualTrees()
    {
        // Arrange
        var sample = new Node
        {
            value = 9801,
            children =
            [
                new Node { value = 9811, children = [new Node { value = 9821 }, new Node { value = 9822 }] },
                new Node { value = 9812 }
            ]
        };

        // Act
        var copy = new Node(sample);

        // Assert
        copy.ShouldBe(sample);
        copy.children.ShouldNotBeSameAs(sample.children);
        copy.children![0].ShouldNotBeSameAs(sample.children![0]);
        copy.children[0].children.ShouldNotBeSameAs(sample.children[0].children);
        copy.children[0].children![0].value++;
        sample.children[0].children![0].value.ShouldBe(9821);
        copy.ShouldNotBe(sample);
    }

    [Fact]
    public void PreservesAbsentChildrenAndOptionalAnnotations()
    {
        // Arrange
        var sample = new Node();

        // Act
        var copy = new Node(sample);
        var annotations = typeof(Node).GetProperty("children")!.GetCustomAttributes(inherit: false);

        // Assert
        sample.children.ShouldBeNull();
        copy.children.ShouldBeNull();
        copy.ShouldBe(sample);
        annotations.ShouldContain(attribute => attribute.GetType().Name == "OptionalAttribute");
    }
}
