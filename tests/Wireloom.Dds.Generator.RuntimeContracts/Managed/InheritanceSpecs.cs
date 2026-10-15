using RuntimeManaged;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Exercises inherited struct and valuetype managed APIs.</summary>
public sealed class InheritanceSpecs
{
    /// <summary>Copies And Compares Inherited Struct Members.</summary>
    [Fact]
    public void CopiesAndComparesInheritedStructMembers()
    {
        // Arrange
        var source = new Derived { id = 17, note = "base", text = "derived", extra = 19 };

        // Act
        var copy = new Derived(source);

        // Assert
        copy.id.ShouldBe(17);
        copy.note.ShouldBe("base");
        copy.text.ShouldBe("derived");
        copy.extra.ShouldBe(19);
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.id++;
        copy.Equals(source).ShouldBeFalse();
        new Derived().text.ShouldBeEmpty();
        new Derived((Derived?)null).note.ShouldBeNull();
        source.ToString().ShouldBe(DerivedSupport.Instance.ToString(source));
    }

    /// <summary>Copies And Compares Inherited Value Type Members.</summary>
    [Fact]
    public void CopiesAndComparesInheritedValueTypeMembers()
    {
        // Arrange
        var source = new DerivedValue { id = 23, text = "value" };

        // Act
        var copy = new DerivedValue(source);

        // Assert
        copy.id.ShouldBe(23);
        copy.text.ShouldBe("value");
        copy.Equals(source).ShouldBeTrue();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.text = "changed";
        copy.Equals(source).ShouldBeFalse();
        new DerivedValue().text.ShouldBeEmpty();
        new DerivedValue((DerivedValue?)null).id.ShouldBe(0);
        source.ToString().ShouldBe(DerivedValueSupport.Instance.ToString(source));
    }
}
