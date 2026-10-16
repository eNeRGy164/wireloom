#nullable enable annotations

using Rti.Dds.Core.Policy;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Keeps case-specific wire representation requirements in one place.</summary>
internal static class PeerQos
{
    /// <summary>Identifies cases whose IDL requires XCDR2 representation.</summary>
    internal static bool UsesXcdr2(string caseId) =>
        caseId is "09-data-representation"
            or "09-allowed-data-representation"
            or "10-flat-data-binding";

    /// <summary>Creates a representation policy containing only XCDR2.</summary>
    internal static DataRepresentation Xcdr2Only => DataRepresentation.Default.With(
        policy =>
        {
            policy.Value.Clear();
            policy.Value.Add(DataRepresentation.Xcdr2);
        });
}
