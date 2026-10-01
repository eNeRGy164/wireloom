namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP050")]
public sealed class PreprocessorSourceOriginTrackerSpecs
{
    [Fact]
    public void CompressesSequentialAndRepeatedOriginsAndPreservesEof()
    {
        // Arrange
        var origins = new[] { 10, 11, 20, 20, 25 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.Count.ShouldBe(3);
        spans[0].Map(0).ShouldBe(10);
        spans[1].Map(2).ShouldBe(20);
        spans[2].Map(4).ShouldBe(25);
    }

    [Fact]
    public void MapsExpandedTextAndEofThroughExplicitOffsetDependency()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset + 100, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "VALUE", "VALUE", 3);
        tracker.AppendOutputOffsets(origins, 1, 3, 3);
        var spans = PreprocessorSourceOriginTracker.Compress([.. origins, 200]);

        // Assert
        origins.ShouldBe([103, 104, 105, 106, 107, 103]);
        spans[^1].Map(spans[^1].OutputStart).ShouldBe(200);
    }
}
