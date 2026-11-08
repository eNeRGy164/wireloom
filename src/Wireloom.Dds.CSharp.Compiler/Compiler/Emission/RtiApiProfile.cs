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
    string? pluginComplianceMask,
    bool supportsRecursiveTypes = true,
    bool supportsOptionalCollections = true,
    bool usesLegacyUnionDefaults = false,
    bool usesUnsignedInt8 = false,
    bool sharesCollectionAliasMembers = false)
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

    /// <summary>Gets whether recursive aggregate metadata can be constructed.</summary>
    internal bool SupportsRecursiveTypes { get; } = supportsRecursiveTypes;

    /// <summary>Gets whether optional sequences and arrays have native conversion support.</summary>
    internal bool SupportsOptionalCollections { get; } = supportsOptionalCollections;

    /// <summary>Gets whether unions use the pre-7.7 discriminator initialization rules.</summary>
    internal bool UsesLegacyUnionDefaults { get; } = usesLegacyUnionDefaults;

    /// <summary>Gets whether IDL int8 uses the legacy unsigned C# representation.</summary>
    internal bool UsesUnsignedInt8 { get; } = usesUnsignedInt8;

    /// <summary>Gets whether containing-value copies retain collection-alias member instances.</summary>
    internal bool SharesCollectionAliasMembers { get; } = sharesCollectionAliasMembers;

    /// <summary>Gets the scoped RTI 6.1.2 C# API profile.</summary>
    internal static RtiApiProfile Rtiddsgen312 { get; } = new(
        "3.1.2",
        "6.1.2",
        supportsDynamicTypeAnnotations: false,
        supportsRecursiveTypeFlag: false,
        supportsMustUnderstand: false,
        supportsNativeEnumArrayDefaultValueInitialization: false,
        octetDynamicType: "byte",
        pluginComplianceMask: null,
        supportsRecursiveTypes: false,
        supportsOptionalCollections: false,
        usesLegacyUnionDefaults: true,
        usesUnsignedInt8: true,
        sharesCollectionAliasMembers: true);

    /// <summary>Gets the scoped RTI 7.3.1 C# API profile.</summary>
    internal static RtiApiProfile Rtiddsgen431 { get; } = new(
        "4.3.1",
        "7.3.1",
        supportsDynamicTypeAnnotations: false,
        supportsRecursiveTypeFlag: false,
        supportsMustUnderstand: false,
        supportsNativeEnumArrayDefaultValueInitialization: false,
        octetDynamicType: "byte",
        pluginComplianceMask: "0x0000068C",
        usesLegacyUnionDefaults: true);

    /// <summary>Gets the scoped RTI 7.7.0 C# API profile.</summary>
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
