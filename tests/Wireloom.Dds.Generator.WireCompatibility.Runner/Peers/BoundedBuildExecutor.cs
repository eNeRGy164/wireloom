namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Runs independent build requests concurrently while preserving their input order.</summary>
internal static class BoundedBuildExecutor
{
    /// <summary>Executes requests with a fixed maximum number of concurrent builds.</summary>
    /// <param name="requests">Build requests to execute, in the order results should be returned.</param>
    /// <param name="maximumConcurrency">Maximum number of simultaneous build processes.</param>
    /// <param name="execute">Build operation for one request.</param>
    /// <returns>Each build result in the same order as its request.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximumConcurrency"/> is not positive.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="execute"/> is null.</exception>
    internal static async Task<IReadOnlyList<TResult>> RunAsync<TRequest, TResult>(IReadOnlyList<TRequest> requests,
        int maximumConcurrency, Func<TRequest, Task<TResult>> execute)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumConcurrency);
        ArgumentNullException.ThrowIfNull(execute);

        using var concurrency = new SemaphoreSlim(maximumConcurrency, maximumConcurrency);
        var tasks = requests.Select(async request =>
        {
            await concurrency.WaitAsync();
            try
            {
                return await execute(request);
            }
            finally
            {
                concurrency.Release();
            }
        }).ToArray();

        return await Task.WhenAll(tasks);
    }
}
