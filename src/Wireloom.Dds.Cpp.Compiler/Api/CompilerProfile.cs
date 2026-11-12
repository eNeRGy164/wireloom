namespace Wireloom.Dds.Cpp;

/// <summary>Identifies a supported RTI Connext C++ API surface for source generation.</summary>
[PublicAPI]
public enum CppCompilerProfile
{
    /// <summary>RTI Connext DDS 6.1.2.</summary>
    Rti612,

    /// <summary>RTI Connext DDS 7.3.1.</summary>
    Rti731,

    /// <summary>RTI Connext DDS 7.7.0.</summary>
    Rti770
}
