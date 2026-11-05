namespace Wireloom.Dds.Generator.RuntimeContracts;

/// <summary>Exposes the resolved RTI package profile to version-sensitive runtime assertions.</summary>
internal static class RtiRuntimeProfile
{
#if WIRELOOM_RTI_6_1_2
    internal const byte Int8Sample = 127;
    internal const byte Int8Zero = 0;
    internal const byte Int8One = 1;
#else
    internal const sbyte Int8Sample = sbyte.MinValue;
    internal const sbyte Int8Zero = 0;
    internal const sbyte Int8One = 1;
#endif

    internal static bool SharesCollectionAliases =>
        typeof(Rti.Dds.Core.ServiceEnvironment).Assembly.GetName().Version < new Version(7, 3, 1);

    internal static bool Uses731Profile =>
        typeof(Rti.Dds.Core.ServiceEnvironment).Assembly.GetName().Version < new Version(7, 7, 0);
}
