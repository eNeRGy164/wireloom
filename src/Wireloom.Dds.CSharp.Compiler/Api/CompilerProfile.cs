namespace Wireloom.Dds.CSharp.Compiler;

/// <summary>Identifies a supported RTI C# API surface for source generation.</summary>
[PublicAPI]
public enum IdlCompilerProfile
{
    /// <summary>RTI Connext DDS 7.3.1 API, corresponding to rtiddsgen 4.3.1.</summary>
    Rti731,

    /// <summary>RTI Connext DDS 7.7.0 API, corresponding to rtiddsgen 4.7.0.</summary>
    Rti770,

    /// <summary>RTI Connext DDS 6.1.2 API, corresponding to rtiddsgen 3.1.2.</summary>
    Rti612
}
