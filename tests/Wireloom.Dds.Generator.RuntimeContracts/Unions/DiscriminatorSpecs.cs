using RuntimeDiscriminators;
using RuntimeDiscriminators.Implementation;
using Wireloom.Dds.Generator.RuntimeContracts.Native;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Exercises each supported discriminator kind through managed and native APIs.</summary>
public sealed class DiscriminatorSpecs
{
    /// <summary>Checks boolean discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void BooleanDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new BooleanChoice { other = 17 };
        var copy = new BooleanChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<BooleanChoice, BooleanChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe(true);
        result.Discriminator.ShouldBe(true);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((BooleanChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks int8 discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void SignedByteDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new SignedByteChoice { other = 17 };
        var copy = new SignedByteChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<SignedByteChoice, SignedByteChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe((sbyte)1);
        result.Discriminator.ShouldBe((sbyte)1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((SignedByteChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks uint8 discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnsignedByteDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new UnsignedByteChoice { other = 17 };
        var copy = new UnsignedByteChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<UnsignedByteChoice, UnsignedByteChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe((byte)1);
        result.Discriminator.ShouldBe((byte)1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((UnsignedByteChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks octet discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void OctetDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new OctetChoice { other = 17 };
        var copy = new OctetChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<OctetChoice, OctetChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe((byte)1);
        result.Discriminator.ShouldBe((byte)1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((OctetChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks short discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void ShortDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new ShortChoice { other = 17 };
        var copy = new ShortChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<ShortChoice, ShortChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe((short)1);
        result.Discriminator.ShouldBe((short)1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((ShortChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks unsigned short discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnsignedShortDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new UnsignedShortChoice { other = 17 };
        var copy = new UnsignedShortChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<UnsignedShortChoice, UnsignedShortChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe((ushort)1);
        result.Discriminator.ShouldBe((ushort)1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((UnsignedShortChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks long discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void IntDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new IntChoice { other = 17 };
        var copy = new IntChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<IntChoice, IntChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe(1);
        result.Discriminator.ShouldBe(1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((IntChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks unsigned long discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnsignedIntDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new UnsignedIntChoice { other = 17 };
        var copy = new UnsignedIntChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<UnsignedIntChoice, UnsignedIntChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe(1U);
        result.Discriminator.ShouldBe(1U);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((UnsignedIntChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }



    /// <summary>Checks char discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void CharacterDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new CharacterChoice { other = 17 };
        var copy = new CharacterChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<CharacterChoice, CharacterChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe('\u00fe');
        result.Discriminator.ShouldBe('\u00fe');
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((CharacterChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks wchar discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void WideCharacterDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new WideCharacterChoice { other = 17 };
        var copy = new WideCharacterChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<WideCharacterChoice, WideCharacterChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe('\u03a9');
        result.Discriminator.ShouldBe('\u03a9');
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((WideCharacterChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks State discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void EnumDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new EnumChoice { other = 17 };
        var copy = new EnumChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<EnumChoice, EnumChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe(State.Ready);
        result.Discriminator.ShouldBe(State.Ready);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((EnumChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

    /// <summary>Checks Discriminator discriminator selection and native preservation.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void AliasDiscriminatorPreservesItsBranch(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var sample = new AliasChoice { other = 17 };
        var copy = new AliasChoice(sample);

        // Act
        var result = NativeSample.RoundTrip<AliasChoice, AliasChoiceUnmanaged>(sample, allocatePointers, allocateMemory);

        // Assert
        sample.Discriminator.ShouldBe(1);
        result.Discriminator.ShouldBe(1);
        result.other.ShouldBe(17);
        result.Get().ShouldBe(17);
        copy.Equals(sample).ShouldBeTrue();
        copy.Equals((object)sample).ShouldBeTrue();
        copy.Equals((AliasChoice?)null).ShouldBeFalse();
        copy.GetHashCode().ShouldBe(sample.GetHashCode());
        sample.ToString().ShouldContain("17");
        copy.number = 23;
        copy.Equals(sample).ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => copy.other);
    }

}
