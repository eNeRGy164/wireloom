namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>Checks the packed global setting and explicit legacy per-file override together.</summary>
public sealed class SequenceArraySettingSpecs
{
    /// <summary>Compiles both generated APIs in the same consumer assembly.</summary>
    [Fact]
    public void PreservesExplicitLegacyOverride()
    {
        // Arrange
        var legacy = new PackageLegacySequenceArrays.Sample();
        var structured = new RuntimeSequenceArrays.Sample();

        // Act
        legacy.values.Add(17);
        structured.values[1].Value.Add(42);

        // Assert
        legacy.values.ShouldBe([17]);
        structured.values.Length.ShouldBe(2);
        structured.values[0].Value.ShouldBeEmpty();
        structured.values[1].Value.ShouldBe([42]);
    }
}
