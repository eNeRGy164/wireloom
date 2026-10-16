namespace Wireloom.Dds.Generator.TestSupport;

/// <summary>Separates valid payload samples from instance lifecycle notifications.</summary>
public static class ReaderSampleBatch
{
    /// <summary>Collects valid payloads without accessing data for invalid samples.</summary>
    public static ReaderSampleBatchResult<TData> Collect<TSample, TData>(
        IEnumerable<TSample> samples,
        Func<TSample, bool> isValidData,
        Func<TSample, TData> getData)
    {
        var validData = new List<TData>();
        var invalidSamples = 0;
        var returnedSamples = 0;

        foreach (var sample in samples)
        {
            returnedSamples++;
            if (isValidData(sample))
            {
                validData.Add(getData(sample));
            }
            else
            {
                invalidSamples++;
            }
        }

        return new ReaderSampleBatchResult<TData>(returnedSamples, invalidSamples, validData);
    }
}

/// <summary>Contains the valid payloads and counts observed in one reader take.</summary>
public sealed record ReaderSampleBatchResult<TData>(
    int ReturnedSamples,
    int InvalidSamples,
    IReadOnlyList<TData> ValidData);
