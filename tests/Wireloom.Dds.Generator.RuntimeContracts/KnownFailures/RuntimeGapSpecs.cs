using RuntimeAliases;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.KnownFailures;

/// <summary>Retains desired contracts blocked by findings documented under issue 39.</summary>
public sealed class RuntimeGapSpecs
{
    private const string Findings = "https://github.com/eNeRGy164/wireloom/issues/39; see README.md known findings. ";

    /// <summary>Reproduces typedef formatting failures without disabling other alias contracts.</summary>
    [Theory(Skip = Findings + "F2: RTI DynamicData cannot format these alias kinds.")]
    [InlineData("scalar", "17")]
    [InlineData("scalar-chain", "17")]
    [InlineData("enum", "Ready")]
    [InlineData("enum-chain", "Ready")]
    [InlineData("text", "hello")]
    [InlineData("wide-text", "世界")]
    [InlineData("numbers", "17")]
    [InlineData("numbers-chain", "17")]
    [InlineData("children", "17")]
    [InlineData("texts", "hello")]
    [InlineData("wide-texts", "世界")]
    [InlineData("vector", "17")]
    [InlineData("matrix", "17")]
    [InlineData("matrix-chain", "17")]
    [InlineData("points", "17")]
    public void FormatsAliasValues(string shape, string expected)
    {
        // Arrange
        var child = new Child(17, "hello");

        // Act
        var text = shape switch
        {
            "scalar" => new Scalar(17).ToString(),
            "scalar-chain" => new ScalarChain(17).ToString(),
            "enum" => new EnumAlias(State.Ready).ToString(),
            "enum-chain" => new EnumChain(State.Ready).ToString(),
            "text" => new Text("hello").ToString(),
            "wide-text" => new WideText("世界").ToString(),
            "numbers" => new Numbers(new Sequence<int> { 17 }).ToString(),
            "numbers-chain" => new NumbersChain(new Numbers(new Sequence<int> { 17 })).ToString(),
            "children" => new Children(new Sequence<Child> { child }).ToString(),
            "texts" => new Texts(new Sequence<string> { "hello" }).ToString(),
            "wide-texts" => new WideTexts(new Sequence<string> { "世界" }).ToString(),
            "vector" => new Vector([17, 18]).ToString(),
            "matrix" => new Matrix(new int[,] { { 17, 18, 19 }, { 20, 21, 22 } }).ToString(),
            "matrix-chain" => new MatrixChain(new Matrix(new int[,] { { 17, 18, 19 }, { 20, 21, 22 } })).ToString(),
            "points" => new Points([child, new Child(18, "other")]).ToString(),
            _ => throw new ArgumentException("Unknown alias shape", nameof(shape))
        };

        // Assert
        text.ShouldContain(expected);
    }

    /// <summary>Reproduces the wide-string annotation kind mismatch.</summary>
    [Theory(Skip = Findings + "F3: wide-string alias metadata uses narrow-string annotations.")]
    [InlineData(false)]
    [InlineData(true)]
    public void CreatesWideStringAliasMetadata(bool isPublic)
    {
        // Arrange
        const int bound = 16;

        // Act
        var type = RuntimeAliases.Implementation.WideTextPlugin.CreateDynamicType(isPublic);

        // Assert
        type.ShouldNotBeNull();
        WideTextSupport.Instance.ShouldNotBeNull();
        new WideText(new string('Ω', bound)).ToString().ShouldContain("Ω");
    }

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
