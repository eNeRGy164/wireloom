namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Separates valid payload samples from instance lifecycle notifications.</summary>
internal static class ReaderSampleBatch
{
    /// <summary>Collects valid payloads without accessing data for invalid samples.</summary>
    internal static ReaderSampleBatchResult<TData> Collect<TSample, TData>(
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
internal sealed record ReaderSampleBatchResult<TData>(
    int ReturnedSamples,
    int InvalidSamples,
    IReadOnlyList<TData> ValidData);
