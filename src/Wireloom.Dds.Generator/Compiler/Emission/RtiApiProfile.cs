namespace Wireloom.Compiler.Emission;

/// <summary>Describes the RTI C# API surface produced by a code generator and used by source emission.</summary>
/// <remarks>
/// Profile identities follow the matching <c>rtiddsgen</c> version. Selection still uses the resolved
/// <c>Rti.ConnextDds</c> runtime version because that is the API surface the generated code must compile against.
/// </remarks>
internal sealed class RtiApiProfile(
    string rtiddsgenVersion,
    string rtiVersion,
    Version minimumRtiVersion,
    bool supportsDynamicTypeAnnotations,
    bool supportsRecursiveTypeFlag,
    bool supportsMustUnderstand,
    string octetDynamicType,
    string? pluginComplianceMask)
{
    internal string RtiddsgenVersion { get; } = rtiddsgenVersion;

    internal string RtiVersion { get; } = rtiVersion;

    private Version MinimumRtiVersion { get; } = minimumRtiVersion;

    internal bool SupportsDynamicTypeAnnotations { get; } = supportsDynamicTypeAnnotations;

    internal bool SupportsRecursiveTypeFlag { get; } = supportsRecursiveTypeFlag;

    internal bool SupportsMustUnderstand { get; } = supportsMustUnderstand;

    internal string OctetDynamicType { get; } = octetDynamicType;

    internal string? PluginComplianceMask { get; } = pluginComplianceMask;

    internal static RtiApiProfile Rtiddsgen431 { get; } = new(
        "4.3.1",
        "7.3.1",
        new Version(7, 3, 1),
        supportsDynamicTypeAnnotations: false,
        supportsRecursiveTypeFlag: false,
        supportsMustUnderstand: false,
        octetDynamicType: "byte",
        pluginComplianceMask: "0x0000068C");

    internal static RtiApiProfile Rtiddsgen470 { get; } = new(
        "4.7.0",
        "7.7.0",
        new Version(7, 7, 0),
        supportsDynamicTypeAnnotations: true,
        supportsRecursiveTypeFlag: true,
        supportsMustUnderstand: true,
        octetDynamicType: "Octet",
        pluginComplianceMask: null);

    /// <summary>Selects the nearest supported code-generator API profile for a resolved RTI runtime version.</summary>
    internal static RtiApiProfile ForVersion(Version version) =>
        version >= Rtiddsgen470.MinimumRtiVersion ? Rtiddsgen470 : Rtiddsgen431;
}
