using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.KnownFailures;

/// <summary>Retains desired contracts blocked by findings documented under issue 39.</summary>
public sealed class RuntimeGapSpecs
{
    private const string Findings = "https://github.com/eNeRGy164/wireloom/issues/39; see README.md known findings. ";

    /// <summary>Reproduces the nonzero-default discriminator null-copy defect.</summary>
    [Fact(Skip = Findings + "F4: null-source union construction retains CLR zero discriminator.")]
    public void NullUnionCopyKeepsTheDefaultBranch()
    {
        // Arrange
        var expected = new RuntimeUnions.Choice();

        // Act
        var actual = new RuntimeUnions.Choice((RuntimeUnions.Choice?)null);

        // Assert
        actual.Discriminator.ShouldBe(RuntimeUnions.Choice.DefaultDiscriminator);
        actual.Equals(expected).ShouldBeTrue();
        actual.flag.ShouldBeFalse();
    }

    /// <summary>Preserves an explicit deep-copy improvement proposal separately from characterization.</summary>
    [Fact(Skip = Findings + "F5: aggregate sequence typedefs currently copy only their container.")]
    public void CopiesAggregateSequenceElementsIndependently()
    {
        // Arrange
        var source = new Children(new Sequence<Child> { new Child(17, "original") });

        // Act
        var copy = new Children(source);
        copy.Value[0].id = 99;

        // Assert
        source.Value[0].id.ShouldBe(17);
        copy.Value[0].ShouldNotBeSameAs(source.Value[0]);
    }
}
