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

    [Fact]
    public void ClampsOutputOffsetsAtTheEndOfTheSourceLine()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset * 2, CancellationToken.None);
        var offsets = new List<int> { -1 };

        // Act
        tracker.AppendOutputOffsets(offsets, 4, 5, 7);

        // Assert
        offsets.ShouldBe([-1, 10, 12, 14, 14]);
    }

    [Fact]
    public void CompressesLongSequentialAndRepeatedRunsBeforeEof()
    {
        // Arrange
        var origins = new[] { 30, 31, 32, 40, 40, 40, 50 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.Count.ShouldBe(3);
        spans[0].ShouldBe(new SourceOriginSpan(0, 3, 30, 3));
        spans[1].ShouldBe(new SourceOriginSpan(3, 3, 40, 0));
        spans[2].ShouldBe(new SourceOriginSpan(6, 1, 50, 0));
    }

    [Fact]
    public void PreservesEofWhenItIsTheOnlyOrigin()
    {
        // Arrange
        var origins = new[] { 12 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.ShouldBe([new SourceOriginSpan(0, 1, 12, 0)]);
    }

    [Fact]
    public void MapsGeneratedTextToTheReplacedTokenAndRetainsTheFollowingSourceAnchor()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "before VALUE after", "before 7 after", sourceOffset: 20);

        // Assert
        origins.ShouldBe([20, 21, 22, 23, 24, 25, 26, 27, 27, 33, 34, 35, 36, 37]);
    }

    [Fact]
    public void MapsAddedOutputAfterTheLastSourceTokenToSourceEof()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "A", "A B", sourceOffset: 10);

        // Assert
        origins.ShouldBe([10, 11, 11]);
    }

    [Fact]
    public void HonorsCancellationBeforeAppendingExpandedOrigins()
    {
        // Arrange
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, cancellation.Token);
        var origins = new List<int>();

        // Act
        var exception = Should.Throw<OperationCanceledException>(() =>
            tracker.AppendExpandedOrigins(origins, "A", "A", sourceOffset: 0));

        // Assert
        exception.CancellationToken.ShouldBe(cancellation.Token);
        origins.ShouldBeEmpty();
    }

    [Fact]
    public void DoesNotAlignDifferentLengthTokensByTheirSharedPrefix()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "A", "AB", sourceOffset: 4);

        // Assert
        origins.Count.ShouldBe(2);
        origins.ShouldBe([4, 4]);
    }

    [Fact]
    public void AlignsRepeatedTokenFromCurrentSourcePositionBeforeLaterMatches()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "A X B C", "A Y X B C", sourceOffset: 0);

        // Assert
        origins.ShouldBe([0, 1, 2, 2, 2, 3, 4, 5, 6]);
    }

    [Fact]
    public void AlignsTheFirstOriginalTokenAfterInsertedOutput()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "A B", "X A B", sourceOffset: 0);

        // Assert
        origins.ShouldBe([0, 0, 0, 1, 2]);
    }

    [Fact]
    public void AlignsTheFirstExpandedTokenWhenEarlierSourceTokensWereRemoved()
    {
        // Arrange
        var tracker = new PreprocessorSourceOriginTracker(offset => offset, CancellationToken.None);
        var origins = new List<int>();

        // Act
        tracker.AppendExpandedOrigins(origins, "X A B", "A B", sourceOffset: 10);

        // Assert
        origins.ShouldBe([12, 13, 14]);
    }

    [Fact]
    public void KeepsEofOutOfThePrecedingSequentialSpan()
    {
        // Arrange
        var origins = new[] { 10, 11, 12 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.ShouldBe([new SourceOriginSpan(0, 2, 10, 2), new SourceOriginSpan(2, 1, 12, 0)]);
    }

    [Fact]
    public void KeepsASequentialEofOutOfASingleCharacterSpan()
    {
        // Arrange
        var origins = new[] { 10, 11 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.ShouldBe([new SourceOriginSpan(0, 1, 10, 0), new SourceOriginSpan(1, 1, 11, 0)]);
    }

    [Fact]
    public void KeepsEofOutOfThePrecedingRepeatedSpan()
    {
        // Arrange
        var origins = new[] { 10, 10, 10 };

        // Act
        var spans = PreprocessorSourceOriginTracker.Compress(origins);

        // Assert
        spans.ShouldBe([new SourceOriginSpan(0, 2, 10, 0), new SourceOriginSpan(2, 1, 10, 0)]);
    }
}
