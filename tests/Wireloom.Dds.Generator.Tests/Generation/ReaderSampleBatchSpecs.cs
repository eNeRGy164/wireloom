using Wireloom.Dds.Generator.WireCompatibility;

namespace Wireloom.Dds.Generator.Tests.Generation;

/// <summary>Checks reader batches that mix payload samples and lifecycle notifications.</summary>
public sealed class ReaderSampleBatchSpecs
{
    /// <summary>Retains and exposes the payload when lifecycle notifications share its take.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MixedBatchKeepsValidPayloadRegardlessOfNotificationOrder(bool notificationFirst)
    {
        // Arrange
        var valid = new ReceivedSample(true, "expected payload");
        var notification = new ReceivedSample(false, "unused");
        ReceivedSample[] samples = notificationFirst ? [notification, valid] : [valid, notification];

        // Act
        var batch = ReaderSampleBatch.Collect(
            samples,
            sample => sample.ValidData,
            sample => sample.GetPayload());

        // Assert
        batch.ReturnedSamples.ShouldBe(2);
        batch.InvalidSamples.ShouldBe(1);
        batch.ValidData.ShouldBe(["expected payload"]);
        notification.PayloadReads.ShouldBe(0);
        valid.PayloadReads.ShouldBe(1);
    }

    /// <summary>Returns no payload for lifecycle-only traffic so the caller can keep waiting for data.</summary>
    [Fact]
    public void LifecycleOnlyBatchContainsNoValidPayload()
    {
        // Arrange
        var samples = new[]
        {
            new ReceivedSample(false, "dispose"),
            new ReceivedSample(false, "unregister")
        };

        // Act
        var batch = ReaderSampleBatch.Collect(
            samples,
            sample => sample.ValidData,
            sample => sample.GetPayload());

        // Assert
        batch.ReturnedSamples.ShouldBe(2);
        batch.InvalidSamples.ShouldBe(2);
        batch.ValidData.ShouldBeEmpty();
        samples.ShouldAllBe(sample => sample.PayloadReads == 0);
    }

    private sealed class ReceivedSample(bool validData, string payload)
    {
        public bool ValidData { get; } = validData;

        public int PayloadReads { get; private set; }

        public string GetPayload()
        {
            PayloadReads++;
            if (!ValidData)
            {
                throw new InvalidOperationException("Lifecycle notifications have no payload.");
            }

            return payload;
        }
    }
}
