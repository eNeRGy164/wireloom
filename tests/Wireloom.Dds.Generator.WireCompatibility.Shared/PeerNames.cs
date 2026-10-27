namespace Wireloom.Dds.Generator.WireCompatibility.Shared;

/// <summary>Peer labels shared by manifest rules and worker protocols.</summary>
public static class PeerNames
{
    /// <summary>Wireloom-generated managed peer.</summary>
    public const string WireloomCSharp = "Wireloom C#";

    /// <summary>RTI-generated managed control peer.</summary>
    public const string RtiCSharp = "RTI C#";

    /// <summary>Native RTI control peer.</summary>
    public const string RtiCpp = "RTI C++";
}
