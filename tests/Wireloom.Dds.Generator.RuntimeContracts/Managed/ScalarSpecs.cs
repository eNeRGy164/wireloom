using RuntimeManaged;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Exercises generated scalar defaults, assignment, copying, and value semantics.</summary>
public sealed class ScalarSpecs
{
    /// <summary>Initializes Idl Defaults And Copies ANull Source.</summary>
    [Fact]
    public void InitializesIdlDefaultsAndCopiesANullSource()
    {
        // Arrange
        var sample = new Scalars();

        // Act
        var copy = new Scalars((Scalars?)null);

        // Assert
        sample.defaultNumber.ShouldBe(42);
        sample.flag.ShouldBeFalse();
        sample.byteValue.ShouldBe((byte)0);
        sample.signedByte.ShouldBe((sbyte)0);
        sample.unsignedByte.ShouldBe((byte)0);
        sample.shortValue.ShouldBe((short)0);
        sample.unsignedShort.ShouldBe((ushort)0);
        sample.intValue.ShouldBe(0);
        sample.unsignedInt.ShouldBe(0U);
        sample.longValue.ShouldBe(0L);
        sample.unsignedLong.ShouldBe(0UL);
        sample.singleValue.ShouldBe(0F);
        sample.doubleValue.ShouldBe(0D);
        sample.extendedValue.ShouldBe((LongDouble)0);
        sample.character.ShouldBe('\0');
        sample.wideCharacter.ShouldBe('\0');
        sample.state.ShouldBe(State.Idle);
        sample.text.ShouldBeEmpty();
        sample.wideText.ShouldBeEmpty();
        // Explicit IDL defaults run in the parameterless constructor, not the null-copy path.
        copy.defaultNumber.ShouldBe(0);
        sample.defaultNumber = 0;
        copy.Equals(sample).ShouldBeTrue();
    }

    /// <summary>Copies Every Primitive Width And Preserves Value Semantics.</summary>
    [Fact]
    public void CopiesEveryPrimitiveWidthAndPreservesValueSemantics()
    {
        // Arrange
        var source = new Scalars
        {
            flag = true, byteValue = 255, signedByte = -128, unsignedByte = 254,
            shortValue = short.MinValue, unsignedShort = ushort.MaxValue,
            intValue = int.MinValue, unsignedInt = uint.MaxValue,
            longValue = long.MinValue, unsignedLong = ulong.MaxValue,
            singleValue = 1.25F, doubleValue = -2.5D, extendedValue = (LongDouble)3.75M,
            character = '\u00fe', wideCharacter = '\u03a9', state = State.Ready,
            text = "hello", wideText = "世界", defaultNumber = 99
        };

        // Act
        var copy = new Scalars(source);

        // Assert
        copy.Equals(source).ShouldBeTrue();
        copy.Equals((object)source).ShouldBeTrue();
        copy.Equals(copy).ShouldBeTrue();
        copy.Equals((Scalars?)null).ShouldBeFalse();
        copy.Equals((object?)null).ShouldBeFalse();
        copy.Equals(new object()).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(source.GetHashCode());
        copy.unsignedLong.ShouldBe(ulong.MaxValue);
        copy.wideText.ShouldBe("世界");
        copy.intValue++;
        copy.Equals(source).ShouldBeFalse();
        source.intValue.ShouldBe(int.MinValue);
    }

    /// <summary>Constructs From Values And Formats Through Type Support.</summary>
    [Fact]
    public void ConstructsFromValuesAndFormatsThroughTypeSupport()
    {
        // Arrange
        var sample = new Child(id: 17, text: "contract");

        // Act
        var text = sample.ToString();

        // Assert
        sample.id.ShouldBe(17);
        sample.text.ShouldBe("contract");
        text.ShouldBe(ChildSupport.Instance.ToString(sample));
        text.ShouldContain("17");
        text.ShouldContain("contract");
    }
}
