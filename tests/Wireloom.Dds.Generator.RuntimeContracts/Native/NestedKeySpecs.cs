using RuntimeManaged;
using RuntimeManaged.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Verifies nested key propagation independently in each conversion direction.</summary>
public sealed class NestedKeySpecs
{
    /// <summary>Checks a keyed aggregate member and non-key sentinel values.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void CopiesNestedKeysOnlyWhenRequested(bool toKeysOnly, bool fromKeysOnly)
    {
        // Arrange
        var native = new KeyedEnvelopeUnmanaged();
        var result = new KeyedEnvelope { identity = new Child(99, "sentinel"), payload = 99 };
        try
        {
            native.Initialize();
            native.ToNative(new KeyedEnvelope { identity = new Child(1, "retained"), payload = 1 });

            // Act
            native.ToNative(new KeyedEnvelope { identity = new Child(17, "updated"), payload = 23 }, keysOnly: toKeysOnly);
            native.FromNative(result, keysOnly: fromKeysOnly);

            // Assert
            result.identity.id.ShouldBe(17);
            if (fromKeysOnly)
            {
                result.identity.text.ShouldBe("sentinel");
                result.payload.ShouldBe(99);
            }
            else if (toKeysOnly)
            {
                result.identity.text.ShouldBe("retained");
                result.payload.ShouldBe(1);
            }
            else
            {
                result.identity.text.ShouldBe("updated");
                result.payload.ShouldBe(23);
            }
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Checks key forwarding through optional aggregate arrays.</summary>
    [Fact]
    public void ForwardsKeysOnlyToOptionalAggregateArrayElements()
    {
        // Arrange
        var source = CollectionSamples.Create(present: true);

        // Act
        var result = NativeSample.RoundTrip<Collections, CollectionsUnmanaged>(source, keysOnly: true);

        // Assert
        result.optionalPoints![0].id.ShouldBe(51);
        result.optionalPoints[0].text.ShouldBeEmpty();
        result.optionalPoints[1].id.ShouldBe(52);
        result.optionalPoints[1].text.ShouldBeEmpty();
        result.points[0].text.ShouldBe("first");
    }
}
