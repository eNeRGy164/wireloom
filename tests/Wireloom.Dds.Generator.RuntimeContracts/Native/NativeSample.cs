using Rti.Dds.NativeInterface.TypePlugin;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Owns native storage locally for one generated conversion pair.</summary>
internal static class NativeSample
{
    /// <summary>Copies a sample through native storage and releases that storage.</summary>
    /// <typeparam name="TManaged">The generated managed type.</typeparam>
    /// <typeparam name="TNative">The generated native representation.</typeparam>
    /// <param name="source">The source sample.</param>
    /// <param name="allocatePointers">Whether initialization allocates pointers.</param>
    /// <param name="allocateMemory">Whether initialization allocates memory.</param>
    /// <param name="keysOnly">Whether both conversions copy only key members.</param>
    /// <returns>A new destination populated from native storage.</returns>
    internal static TManaged RoundTrip<TManaged, TNative>(TManaged source, bool allocatePointers = true, bool allocateMemory = true, bool keysOnly = false)
        where TManaged : class, new()
        where TNative : struct, INativeTopicType<TManaged>
    {
        var native = new TNative();
        try
        {
            // RTI's allocateMemory=false mode requires existing backing storage.
            if (!allocateMemory)
            {
                native.Initialize(allocatePointers: true, allocateMemory: true);
            }

            native.Initialize(allocatePointers, allocateMemory);
            native.ToNative(source, keysOnly);
            var destination = new TManaged();
            native.FromNative(destination, keysOnly);
            native.Destroy(optionalsOnly: true);
            return destination;
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }
}
