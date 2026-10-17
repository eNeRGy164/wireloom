using Omg.Types.Dynamic;
using Rti.Types.Dynamic;
using Wireloom.Dds.Generator.TestSupport;

namespace Wireloom.Dds.Generator.Tests.Generation;

/// <summary>Checks independent fixture verification across native discriminator representations.</summary>
public sealed class WireSampleVerifierSpecs
{
    /// <summary>Verifies logical values despite signed RTI label and discriminator storage.</summary>
    [Theory]
    [InlineData(TypeKind.UInt32, -1, 4294967295L)]
    [InlineData(TypeKind.UInt32, -2147483648, 2147483648L)]
    [InlineData(TypeKind.Char8, -1, 255L)]
    [InlineData(TypeKind.Char8, -128, 128L)]
    [InlineData(TypeKind.Char8, 127, 127L)]
    [InlineData(TypeKind.Int16, -32768, -32768L)]
    [InlineData(TypeKind.Uint16, 65535, 65535L)]
    [InlineData(TypeKind.Uint8, 255, 255L)]
    public void SignedRuntimeLabelsMatchLogicalDiscriminators(TypeKind kind, int label, long logicalValue)
    {
        // Arrange
        using var expected = CreateFixture(kind, label, "value");
        var actual = new UnionSample(ManagedDiscriminator(kind, logicalValue), value: 42, fallback: 0);

        // Act
        var verify = () => IndependentSampleVerifier.Verify(expected, actual, "boundary");

        // Assert
        Should.NotThrow(verify);
    }

    /// <summary>Does not confuse normalized labels with default branches or suppress payload mismatches.</summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DefaultBranchesAndWrongPayloadsRemainDistinct(bool useDefault)
    {
        // Arrange
        using var expected = CreateFixture(TypeKind.UInt32, -1, useDefault ? "fallback" : "value");
        var discriminator = useDefault ? 0U : uint.MaxValue;
        var correct = new UnionSample(discriminator, value: 42, fallback: 42);
        var incorrect = new UnionSample(discriminator, value: 43, fallback: 43);

        // Act
        IndependentSampleVerifier.Verify(expected, correct, "correct payload");
        var verifyIncorrect = () => IndependentSampleVerifier.Verify(expected, incorrect, "wrong payload");

        // Assert
        Should.Throw<InvalidDataException>(verifyIncorrect).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Checks independent character payloads against the printable ASCII wire fixture.</summary>
    [Theory]
    [InlineData("union-minimum", '\0')]
    [InlineData("union-default", '\u0001')]
    [InlineData("union-maximum", '~')]
    public void CharacterFixturesCheckPayloads(string fixture, char discriminator)
    {
        // Arrange
        var factory = DynamicTypeFactory.Instance;
        var type = factory.BuildUnion()
            .WithName($"CharacterVerifier_{(int)discriminator}")
            .WithDiscriminator(factory.GetPrimitiveType<char>())
            .AddMembers([
                new UnionMember("minimum", factory.GetPrimitiveType<int>(), [0]),
                new UnionMember("maximum", factory.GetPrimitiveType<int>(), [126]),
                new UnionMember("fallback", factory.GetPrimitiveType<int>(), [UnionMember.DefaultLabel])])
            .Create();
        var correct = new CharacterSample(discriminator, value: 8201);
        var incorrect = new CharacterSample(discriminator, value: 0);

        // Act
        IndependentSampleVerifier.VerifyCharacterUnionFixture(type, correct, fixture);
        var verifyIncorrect = () => IndependentSampleVerifier.VerifyCharacterUnionFixture(type, incorrect, fixture);

        // Assert
        Should.Throw<InvalidDataException>(verifyIncorrect).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Rejects the obsolete high-bit fixture even when its branch and payload match.</summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(255)]
    public void CharacterFixturesRejectObsoleteHighBitDiscriminators(int maximumLabel)
    {
        // Arrange
        var factory = DynamicTypeFactory.Instance;
        var type = factory.BuildUnion()
            .WithName($"ObsoleteCharacterVerifier_{maximumLabel}")
            .WithDiscriminator(factory.GetPrimitiveType<char>())
            .AddMembers([
                new UnionMember("minimum", factory.GetPrimitiveType<int>(), [0]),
                new UnionMember("maximum", factory.GetPrimitiveType<int>(), [maximumLabel]),
                new UnionMember("fallback", factory.GetPrimitiveType<int>(), [UnionMember.DefaultLabel])])
            .Create();
        var obsolete = new CharacterSample('\u00ff', value: 8201);

        // Act
        var verify = () => IndependentSampleVerifier.VerifyCharacterUnionFixture(type, obsolete, "union-maximum");

        // Assert
        var exception = Should.Throw<InvalidDataException>(verify);
        exception.Message.ShouldContain("\"$discriminator\":126");
        exception.Message.ShouldContain("\"$discriminator\":255");
    }

    /// <summary>Creates a real RTI DynamicData fixture with explicit and default union branches.</summary>
    private static DynamicData CreateFixture(TypeKind kind, int label, string branch)
    {
        var factory = DynamicTypeFactory.Instance;
        var type = factory.BuildUnion()
            .WithName($"Verifier_{kind}_{label}")
            .WithDiscriminator(factory.GetPrimitiveType(kind))
            .AddMembers([
                new UnionMember("value", factory.GetPrimitiveType<int>(), [label]),
                new UnionMember("fallback", factory.GetPrimitiveType<int>(), [UnionMember.DefaultLabel])])
            .Create();
        var data = new DynamicData(type);
        data.SetAnyValue(branch, 42);
        return data;
    }

    /// <summary>Boxes the public managed discriminator using its declared type.</summary>
    private static object ManagedDiscriminator(TypeKind kind, long value) => kind switch
    {
        TypeKind.UInt32 => checked((uint)value),
        TypeKind.Char8 => checked((char)value),
        TypeKind.Int16 => checked((short)value),
        TypeKind.Uint16 => checked((ushort)value),
        TypeKind.Uint8 => checked((byte)value),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    /// <summary>Provides the public members inspected independently of generated equality and conversions.</summary>
    private sealed class UnionSample(object discriminator, int value, int fallback)
    {
        /// <summary>Gets the managed discriminator.</summary>
        public object Discriminator { get; } = discriminator;

        /// <summary>Gets the explicit branch payload.</summary>
        public int value { get; } = value;

        /// <summary>Gets the default branch payload.</summary>
        public int fallback { get; } = fallback;
    }

    /// <summary>Exposes character fixture payloads without depending on runtime DynamicData conversion.</summary>
    private sealed class CharacterSample(char discriminator, int value)
    {
        /// <summary>Gets the managed discriminator.</summary>
        public char Discriminator { get; } = discriminator;

        /// <summary>Gets the minimum branch value.</summary>
        public int minimum { get; } = value;

        /// <summary>Gets the maximum branch value.</summary>
        public int maximum { get; } = value;

        /// <summary>Gets the fallback branch value.</summary>
        public int fallback { get; } = value;
    }
}
