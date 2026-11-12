using Wireloom.Dds.Cpp.Emission;

namespace Wireloom.Dds.Cpp.Compiler.Tests;

/// <summary>Verifies runtime version mapping for the C++ generator profiles.</summary>
public sealed class RtiApiProfileSpecs
{
    /// <summary>Maps runtime versions to the matching generator profile boundaries.</summary>
    [Theory]
    [InlineData("6.1.2", "6.1.2")]
    [InlineData("7.3.0", "6.1.2")]
    [InlineData("7.3.1", "7.3.1")]
    [InlineData("7.6.9", "7.3.1")]
    [InlineData("7.7.0", "7.7.0")]
    [InlineData("8.0.0", "7.7.0")]
    public void SelectsNearestSupportedProfile(string runtimeVersion, string expectedProfile)
    {
        // Arrange
        var version = Version.Parse(runtimeVersion);

        // Act
        var profile = RtiApiProfile.GetForRuntimeVersion(version);

        // Assert
        profile.RtiVersion.ShouldBe(expectedProfile);
    }

    /// <summary>Marks the Wireloom-specific private ABI and sample-access paths by profile.</summary>
    [Theory]
    [InlineData("6.1.2", false, false)]
    [InlineData("7.3.1", false, true)]
    [InlineData("7.7.0", true, true)]
    public void SelectsWireloom770PrivateLayoutsOnlyFor770(
        string runtimeVersion, bool usesTypeCodeLayout, bool usesStlSampleAccessLayout)
    {
        // Arrange
        var version = Version.Parse(runtimeVersion);

        // Act
        var profile = RtiApiProfile.GetForRuntimeVersion(version);

        // Assert
        profile.UsesWireloom770PrivateTypeCodeLayout.ShouldBe(usesTypeCodeLayout);
        profile.SupportsStlSampleAccessAdapter.ShouldBe(usesStlSampleAccessLayout);
    }
}
