namespace Wireloom.Dds.Generator.RuntimeContracts;

/// <summary>Exposes the resolved RTI package profile to version-sensitive runtime assertions.</summary>
internal static class RtiRuntimeProfile
{
    internal static bool Uses731Profile =>
        typeof(Rti.Dds.Core.ServiceEnvironment).Assembly.GetName().Version < new Version(7, 7, 0);
}
