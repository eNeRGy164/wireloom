using Wireloom.Dds.Generator.TestSupport;

namespace Wireloom.Dds.Generator.Tests.Generation;

/// <summary>Checks exact character-union fixtures independently of generated equality and fixture defaults.</summary>
public sealed class NumericCharacterFixtureSpecs
{
    /// <summary>Accepts each declared discriminator and payload pair.</summary>
    [Theory]
    [InlineData("07-union-char-numeric", "union-127", (char)127, 13701)]
    [InlineData("07-union-char-numeric-255", "union-255", (char)255, 13702)]
    [InlineData("07-union-char-numeric", "union-default", (char)65, 13703)]
    [InlineData("07-union-char-numeric-255", "union-default", (char)65, 13703)]
    public void RequiresTheDeclaredValues(string caseId, string fixture, char discriminator, int payload)
    {
        // Arrange
        var sample = new Sample(discriminator, payload);

        // Act
        void Verify() => IndependentSampleVerifier.VerifyNumericCharacterUnionFixture(sample, caseId, fixture);

        // Assert
        Should.NotThrow(Verify);
    }

    /// <summary>Rejects a different discriminator even when it still selects the fallback branch.</summary>
    [Theory]
    [InlineData("07-union-char-numeric", "union-127", 13701)]
    [InlineData("07-union-char-numeric-255", "union-255", 13702)]
    [InlineData("07-union-char-numeric", "union-default", 13703)]
    [InlineData("07-union-char-numeric-255", "union-default", 13703)]
    public void RejectsAnIncorrectDiscriminator(string caseId, string fixture, int payload)
    {
        // Arrange
        var sample = new Sample(discriminator: (char)0, payload);

        // Act
        void Verify() => IndependentSampleVerifier.VerifyNumericCharacterUnionFixture(sample, caseId, fixture);

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Detects the observed zero payload even if the discriminator is correct.</summary>
    [Theory]
    [InlineData("07-union-char-numeric", "union-127", (char)127)]
    [InlineData("07-union-char-numeric-255", "union-255", (char)255)]
    [InlineData("07-union-char-numeric", "union-default", (char)65)]
    [InlineData("07-union-char-numeric-255", "union-default", (char)65)]
    public void RejectsAnIncorrectPayload(string caseId, string fixture, char discriminator)
    {
        // Arrange
        var sample = new Sample(discriminator, payload: 0);

        // Act
        void Verify() => IndependentSampleVerifier.VerifyNumericCharacterUnionFixture(sample, caseId, fixture);

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Received sample differs");
    }

    /// <summary>Does not silently apply expectations for another case or fixture.</summary>
    [Theory]
    [InlineData("07-union-char-numeric", "union-255")]
    [InlineData("07-union-char-numeric-255", "union-127")]
    [InlineData("unknown", "union-default")]
    public void RejectsUnknownFixtureCombinations(string caseId, string fixture)
    {
        // Arrange
        var sample = new Sample(discriminator: (char)65, payload: 13703);

        // Act
        void Verify() => IndependentSampleVerifier.VerifyNumericCharacterUnionFixture(sample, caseId, fixture);

        // Assert
        Should.Throw<InvalidDataException>(Verify).Message.ShouldContain("Unknown numeric character union fixture");
    }

    // ReSharper disable UnusedMember.Local
    // Fixture properties are read by IndependentSampleVerifier through reflection.
    private sealed class Sample(char discriminator, int payload)
    {
        /// <summary>Gets the supplied character discriminator.</summary>
        public char Discriminator { get; } = discriminator;

        /// <summary>Gets the explicit-branch payload.</summary>
        public int value { get; } = payload;

        /// <summary>Gets the fallback-branch payload.</summary>
        public int fallback { get; } = payload;
    }
    // ReSharper restore UnusedMember.Local
}
