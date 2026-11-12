namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Describes the RTI Connext C++ API surface targeted by generated code.</summary>
internal sealed class RtiApiProfile(
    string rtiVersion,
    bool usesWireloom770PrivateTypeCodeLayout,
    bool supportsStlSampleAccessAdapter)
{
    private static readonly Version MinimumRuntimeVersion = new(6, 1, 2);
    private static readonly Version Rti731RuntimeVersion = new(7, 3, 1);
    private static readonly Version Rti770RuntimeVersion = new(7, 7, 0);

    /// <summary>Gets the canonical RTI runtime version represented by this profile.</summary>
    internal string RtiVersion { get; } = rtiVersion;

    /// <summary>Gets whether generated adapters may access the selected private TypeCode layout.</summary>
    internal bool UsesWireloom770PrivateTypeCodeLayout { get; } = usesWireloom770PrivateTypeCodeLayout;

    /// <summary>Gets whether Wireloom has qualified its STL sample-access adapter for this profile.</summary>
    internal bool SupportsStlSampleAccessAdapter { get; } = supportsStlSampleAccessAdapter;

    /// <summary>Gets the matching profile for a resolved RTI runtime version.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="runtimeVersion"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The runtime version is below 6.1.2.</exception>
    internal static RtiApiProfile GetForRuntimeVersion(Version runtimeVersion)
    {
        ArgumentNullException.ThrowIfNull(runtimeVersion);

        if (runtimeVersion < MinimumRuntimeVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(runtimeVersion), runtimeVersion,
                $"RTI runtime version {MinimumRuntimeVersion} or later is required.");
        }

        if (runtimeVersion >= Rti770RuntimeVersion)
        {
            return Rti770;
        }

        return runtimeVersion >= Rti731RuntimeVersion ? Rti731 : Rti612;
    }

    /// <summary>Gets the scoped RTI Connext 6.1.2 API profile.</summary>
    internal static RtiApiProfile Rti612 { get; } = new("6.1.2", false, false);

    /// <summary>Gets the scoped RTI Connext 7.3.1 API profile.</summary>
    internal static RtiApiProfile Rti731 { get; } = new("7.3.1", false, true);

    /// <summary>Gets the scoped RTI Connext 7.7.0 API profile.</summary>
    internal static RtiApiProfile Rti770 { get; } = new("7.7.0", true, true);
}
