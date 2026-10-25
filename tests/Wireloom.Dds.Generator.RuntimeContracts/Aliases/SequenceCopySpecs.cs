using RuntimeSequenceCopy;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Checks that generated sequence copies own their mutable elements.</summary>
public sealed class SequenceCopySpecs
{
    /// <summary>Copies aggregate elements independently through typedefs and alias chains.</summary>
    [Fact]
    public void SequenceTypedefCopiesOwnAggregateElements()
    {
        // Arrange
        var boundedSource = new Children(ChildrenWithChild());
        var unboundedSource = new UnboundedChildren(ChildrenWithChild());
        var chainSource = new ChildrenChain(new Children(ChildrenWithChild()));
        var aliasSource = new AliasChildren(new Sequence<Child> { ChildWithValue() });

        // Act
        var boundedCopy = new Children(boundedSource);
        var unboundedCopy = new UnboundedChildren(unboundedSource);
        var chainCopy = new ChildrenChain(chainSource);
        var aliasCopy = new AliasChildren(aliasSource);

        // Assert
        ExpectEqualIndependentCopy(boundedCopy, boundedSource);
        ExpectEqualIndependentCopy(unboundedCopy, unboundedSource);
        chainCopy.Equals(chainSource).ShouldBeTrue();
        chainCopy.GetHashCode().ShouldBe(chainSource.GetHashCode());
        chainCopy.Value.Value[0].ShouldNotBeSameAs(chainSource.Value.Value[0]);
        aliasCopy.Equals(aliasSource).ShouldBeTrue();
        aliasCopy.GetHashCode().ShouldBe(aliasSource.GetHashCode());
        aliasCopy.Value[0].ShouldNotBeSameAs(aliasSource.Value[0]);

        boundedCopy.Value[0].id = 99;
        boundedCopy.Value[0].values[0] = 77;
        unboundedCopy.Value[0].id = 98;
        chainCopy.Value.Value[0].id = 97;
        aliasCopy.Value[0].id = 96;
        aliasCopy.Value[0].values[0] = 95;

        boundedSource.Value[0].id.ShouldBe(17);
        boundedSource.Value[0].values[0].ShouldBe(7);
        unboundedSource.Value[0].id.ShouldBe(17);
        chainSource.Value.Value[0].id.ShouldBe(17);
        aliasSource.Value[0].id.ShouldBe(17);
        aliasSource.Value[0].values[0].ShouldBe(7);
    }

    /// <summary>Copies scalar alias elements as primitive values.</summary>
    [Fact]
    public void SequenceTypedefCopiesScalarAliasElementsByValue()
    {
        // Arrange
        var source = new Scalars(new Sequence<int> { 17 });

        // Act
        var copy = new Scalars(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);

        // Act
        copy.Value[0] = 99;

        // Assert
        source.Value[0].ShouldBe(17);
    }

    /// <summary>Copies union elements and their active aggregate branches independently.</summary>
    [Fact]
    public void SequenceTypedefCopiesOwnUnionElements()
    {
        // Arrange
        var source = new Choices(new Sequence<Choice> { new () { child = ChildWithValue() } });

        // Act
        var copy = new Choices(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
        copy.Value[0].child.ShouldNotBeSameAs(source.Value[0].child);
        copy.Value[0].child.values[0] = 99;
        source.Value[0].child.values[0].ShouldBe(7);
    }

    /// <summary>Copies direct struct sequence members whose scalar aliases are projected to values.</summary>
    [Fact]
    public void StructCopiesErasedScalarAliasSequenceElementsByValue()
    {
        // Arrange
        var source = new ScalarHolder();
        source.values.Add(17);

        // Act
        var copy = new ScalarHolder(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.values.ShouldNotBeSameAs(source.values);
        copy.values[0] = 99;
        source.values[0].ShouldBe(17);
    }

    /// <summary>Copies direct and typedef-backed struct sequence members independently.</summary>
    [Fact]
    public void StructCopiesOwnRequiredAndOptionalSequenceElements()
    {
        // Arrange
        var source = new Holder();
        source.direct.Add(ChildWithValue());
        source.wrapped = new Children(ChildrenWithChild());
        source.optionalChildren = ChildrenWithChild();

        // Act
        var copy = new Holder(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.direct.ShouldNotBeSameAs(source.direct);
        copy.direct[0].ShouldNotBeSameAs(source.direct[0]);
        copy.wrapped.Value[0].ShouldNotBeSameAs(source.wrapped.Value[0]);
        copy.optionalChildren.ShouldNotBeSameAs(source.optionalChildren);
        copy.optionalChildren![0].ShouldNotBeSameAs(source.optionalChildren[0]);

        copy.direct[0].id = 99;
        copy.wrapped.Value[0].id = 98;
        copy.optionalChildren![0].id = 97;

        source.direct[0].id.ShouldBe(17);
        source.wrapped.Value[0].id.ShouldBe(17);
        source.optionalChildren![0].id.ShouldBe(17);
    }

    /// <summary>Copies direct and typedef-backed union sequence branches independently.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void UnionCopiesOwnSequenceElements(bool useTypedef)
    {
        // Arrange
        var source = useTypedef
            ? new Container { wrapped = new Children(ChildrenWithChild()) }
            : new Container { direct = ChildrenWithChild() };

        // Act
        var copy = new Container(source);

        // Assert
        copy.Discriminator.ShouldBe(source.Discriminator);
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        if (useTypedef)
        {
            copy.wrapped.Value[0].ShouldNotBeSameAs(source.wrapped.Value[0]);
            copy.wrapped.Value[0].id = 99;
            source.wrapped.Value[0].id.ShouldBe(17);
        }
        else
        {
            copy.direct.ShouldNotBeSameAs(source.direct);
            copy.direct[0].ShouldNotBeSameAs(source.direct[0]);
            copy.direct[0].values[0] = 99;
            source.direct[0].values[0].ShouldBe(7);
        }
    }

    private static void ExpectEqualIndependentCopy(Children copy, Children source)
    {
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
        copy.Value[0].values.ShouldNotBeSameAs(source.Value[0].values);
    }

    private static void ExpectEqualIndependentCopy(UnboundedChildren copy, UnboundedChildren source)
    {
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.Value.ShouldNotBeSameAs(source.Value);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
        copy.Value[0].values.ShouldNotBeSameAs(source.Value[0].values);
    }

    private static Child ChildWithValue()
    {
        var child = new Child { id = 17 };
        child.values.Add(7);
        return child;
    }

    private static Sequence<Child> ChildrenWithChild() => [ChildWithValue()];
}
