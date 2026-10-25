using Wireloom.Dds.Generator.TestSupport;
using Wireloom.Dds.Generator.WireCompatibility;

namespace Wireloom.Dds.Generator.Tests.Generation;

/// <summary>Checks typed fixture construction without relying on DynamicData signedness or constructor defaults.</summary>
public sealed class WireScalarFixtureSpecs
{
    /// <summary>Leaves the enum read-only so assigning an expected default cannot conceal a constructor defect.</summary>
    [Fact]
    public void KeepsTheEnumConstructorDefaultUntouched()
    {
        // Arrange
        const string fixture = "non-default";

        // Act
        var sample = ManagedFixtures.CreateDefaultsAndRangesFixture<DefaultedSample>(fixture);

        // Assert
        IndependentSampleVerifier.VerifyDefaultsAndRangesFixture(sample, "declared defaults");
        sample.color.ShouldBe(Color.Blue);
    }

    /// <summary>Rejects a wrong constructor default instead of repairing it in the fixture builder.</summary>
    [Fact]
    public void RejectsAnIncorrectEnumConstructorDefault()
    {
        // Arrange
        var sample = ManagedFixtures.CreateDefaultsAndRangesFixture<WrongDefaultSample>("non-default");

        // Act
        void Verify() => IndependentSampleVerifier.VerifyDefaultsAndRangesFixture(sample, "wrong default");

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Checks all signed boundary values and rejects corruption in each scalar member.</summary>
    [Theory]
    [InlineData("shortValue")]
    [InlineData("longValue")]
    [InlineData("longLongValue")]
    [InlineData("int8Value")]
    [InlineData("int64Value")]
    [InlineData("floatValue")]
    [InlineData("doubleValue")]
    public void BoundaryFixturesPreserveAndIndependentlyCheckEveryMember(string changedMember)
    {
        // Arrange
        var sample = ManagedFixtures.CreatePrimitiveBoundaryFixture<BoundarySample>("01-boundaries", "non-default");
        IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(sample, "01-boundaries", "correct boundary");
        var property = typeof(BoundarySample).GetProperty(changedMember)!;
        property.SetValue(sample, Activator.CreateInstance(property.PropertyType));

        // Act
        void Verify() => IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(sample, "01-boundaries", "changed boundary");

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Does not confuse signed int8 with uint8 or omit any full-width field from verification.</summary>
    [Theory]
    [InlineData("i8")]
    [InlineData("i16")]
    [InlineData("i32")]
    [InlineData("i64")]
    [InlineData("u8")]
    [InlineData("u16")]
    [InlineData("u32")]
    [InlineData("u64")]
    public void FullWidthFixturesPreserveAndIndependentlyCheckEveryMember(string changedMember)
    {
        // Arrange
        var sample = ManagedFixtures.CreatePrimitiveBoundaryFixture<FullWidthSample>("01-full-widths", "non-default");
        IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(sample, "01-full-widths", "correct full widths");
        var property = typeof(FullWidthSample).GetProperty(changedMember)!;
        property.SetValue(sample, Activator.CreateInstance(property.PropertyType));

        // Act
        void Verify() => IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(sample, "01-full-widths", "changed full widths");

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Rejects unknown fixture names instead of silently applying an adaptation.</summary>
    [Fact]
    public void RejectsUnknownScalarFixtures()
    {
        // Arrange
        const string unknown = "unknown";

        // Act
        DefaultedSample Defaults() => ManagedFixtures.CreateDefaultsAndRangesFixture<DefaultedSample>(unknown);
        BoundarySample Boundary() => ManagedFixtures.CreatePrimitiveBoundaryFixture<BoundarySample>("01-boundaries", unknown);
        BoundarySample UnknownCase() => ManagedFixtures.CreatePrimitiveBoundaryFixture<BoundarySample>(unknown, "non-default");
        void VerifyUnknown() => IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(new object(), unknown, "unknown case");

        // Assert
        Should.Throw<FixtureException>(Defaults);
        Should.Throw<FixtureException>(Boundary);
        Should.Throw<FixtureException>(UnknownCase);
        Should.Throw<InvalidDataException>(VerifyUnknown);
    }

    private enum Color { Green = 0, Blue = 2 }

    // ReSharper disable UnusedMember.Local
    // Fixture properties are read by IndependentSampleVerifier through reflection.
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class DefaultedSample
    {
        /// <summary>Gets the constructor's untouched declared default.</summary>
        public Color color { get; } = Color.Blue;
        /// <summary>Gets or sets the bounded value.</summary>
        public int value { get; set; }
        /// <summary>Gets or sets the ranged value.</summary>
        public int ranged { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class WrongDefaultSample
    {
        /// <summary>Gets an intentionally incorrect constructor default.</summary>
        public Color color { get; } = Color.Green;
        /// <summary>Gets or sets the bounded value.</summary>
        public int value { get; set; }
        /// <summary>Gets or sets the ranged value.</summary>
        public int ranged { get; set; }
    }

    private sealed class BoundarySample
    {
        /// <summary>Gets or sets the signed short boundary.</summary>
        public short shortValue { get; set; }
        /// <summary>Gets or sets the signed long boundary.</summary>
        public int longValue { get; set; }
        /// <summary>Gets or sets the signed long-long boundary.</summary>
        public long longLongValue { get; set; }
        /// <summary>Gets or sets the signed byte boundary.</summary>
        public sbyte int8Value { get; set; }
        /// <summary>Gets or sets the signed 64-bit boundary.</summary>
        public long int64Value { get; set; }
        /// <summary>Gets or sets the float fixture.</summary>
        public float floatValue { get; set; }
        /// <summary>Gets or sets the double fixture.</summary>
        public double doubleValue { get; set; }
    }

    private sealed class FullWidthSample
    {
        /// <summary>Gets or sets the signed byte fixture.</summary>
        public sbyte i8 { get; set; }
        /// <summary>Gets or sets the signed short fixture.</summary>
        public short i16 { get; set; }
        /// <summary>Gets or sets the signed 32-bit fixture.</summary>
        public int i32 { get; set; }
        /// <summary>Gets or sets the signed 64-bit fixture.</summary>
        public long i64 { get; set; }
        /// <summary>Gets or sets the unsigned byte fixture.</summary>
        public byte u8 { get; set; }
        /// <summary>Gets or sets the unsigned short fixture.</summary>
        public ushort u16 { get; set; }
        /// <summary>Gets or sets the unsigned 32-bit fixture.</summary>
        public uint u32 { get; set; }
        /// <summary>Gets or sets the unsigned 64-bit fixture.</summary>
        public ulong u64 { get; set; }
    }
    // ReSharper restore UnusedMember.Local
}
