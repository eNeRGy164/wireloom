namespace Wireloom.Compiler.Emission;

/// <summary>Describes the RTI C# API surface produced by a code generator and used by source emission.</summary>
/// <remarks>
/// Profile identities follow the matching <c>rtiddsgen</c> version. Selection still uses the resolved
/// <c>Rti.ConnextDds</c> runtime version because that is the API surface the generated code must compile against.
/// </remarks>
internal sealed class RtiApiProfile(
    string rtiddsgenVersion,
    string rtiVersion,
    bool supportsDynamicTypeAnnotations,
    bool supportsRecursiveTypeFlag,
    bool supportsMustUnderstand,
    bool supportsNativeEnumArrayDefaultValueInitialization,
    string octetDynamicType,
    string? pluginComplianceMask)
{
    internal string RtiddsgenVersion { get; } = rtiddsgenVersion;

    internal string RtiVersion { get; } = rtiVersion;

    internal bool SupportsDynamicTypeAnnotations { get; } = supportsDynamicTypeAnnotations;

    internal bool SupportsRecursiveTypeFlag { get; } = supportsRecursiveTypeFlag;

    internal bool SupportsMustUnderstand { get; } = supportsMustUnderstand;

    /// <summary>Gets whether native enum arrays accept an explicit default value during initialization.</summary>
    internal bool SupportsNativeEnumArrayDefaultValueInitialization { get; } = supportsNativeEnumArrayDefaultValueInitialization;

    internal string OctetDynamicType { get; } = octetDynamicType;

    internal string? PluginComplianceMask { get; } = pluginComplianceMask;

    internal static RtiApiProfile Rtiddsgen431 { get; } = new(
        "4.3.1",
        "7.3.1",
        supportsDynamicTypeAnnotations: false,
        supportsRecursiveTypeFlag: false,
        supportsMustUnderstand: false,
        supportsNativeEnumArrayDefaultValueInitialization: false,
        octetDynamicType: "byte",
        pluginComplianceMask: "0x0000068C");

    internal static RtiApiProfile Rtiddsgen470 { get; } = new(
        "4.7.0",
        "7.7.0",
        supportsDynamicTypeAnnotations: true,
        supportsRecursiveTypeFlag: true,
        supportsMustUnderstand: true,
        supportsNativeEnumArrayDefaultValueInitialization: true,
        octetDynamicType: "Octet",
        pluginComplianceMask: null);
}
