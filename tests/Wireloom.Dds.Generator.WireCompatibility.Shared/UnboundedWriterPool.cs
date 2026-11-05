namespace Wireloom.Dds.Generator.WireCompatibility.Shared;

/// <summary>Defines the finite writer pool used by unbounded wire fixtures.</summary>
public static class UnboundedWriterPool
{
    /// <summary>The RTI writer history property that controls fast-pool buffer size.</summary>
    public const string PropertyName = "dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size";

    /// <summary>The fast-pool threshold in bytes. Larger samples allocate separately.</summary>
    public const string BufferSize = "4096";

    /// <summary>Identifies fixtures that use the finite writer pool.</summary>
    public static bool AppliesTo(string caseId, string fixture) =>
        caseId is "04-unbounded-string-aliases"
            or "05-unbounded-sequence-aliases"
            or "09-optional-recursive-array"
            or "14-unbounded-support"
            or "14-unbounded-receive-lifecycle"
        || (caseId == "06-aggregates" && fixture == "direct-recursive-sequence");
}
