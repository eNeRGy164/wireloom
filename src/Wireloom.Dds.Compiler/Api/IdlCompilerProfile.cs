namespace Wireloom;

/// <summary>Identifies a supported RTI C# API surface for source generation.</summary>
[PublicAPI]
public enum IdlCompilerProfile
{
    /// <summary>RTI Connext DDS 7.3.1 API, corresponding to rtiddsgen 4.3.1.</summary>
    Rti731,

    /// <summary>RTI Connext DDS 7.7.0 API, corresponding to rtiddsgen 4.7.0.</summary>
    Rti770
}
