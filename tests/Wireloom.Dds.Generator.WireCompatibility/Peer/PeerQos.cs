#nullable enable annotations

using Rti.Dds.Core.Policy;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Keeps case-specific wire representation requirements in one place.</summary>
internal static class PeerQos
{
    /// <summary>Identifies cases whose IDL requires XCDR2 representation.</summary>
    internal static bool UsesXcdr2(string caseId, string fixture) =>
        (caseId == "14-unbounded-support" && fixture == "xcdr2-unbounded")
        || (caseId == "14-unbounded-receive-lifecycle" && fixture.EndsWith("xcdr2", StringComparison.Ordinal))
        || caseId is "09-data-representation"
            or "09-allowed-data-representation"
            or "10-flat-data-binding";

    /// <summary>Identifies unbounded support fixtures that explicitly require XCDR1.</summary>
    internal static bool UsesXcdr1(string caseId, string fixture) =>
        (caseId == "14-unbounded-support" && fixture == "xcdr1-unbounded")
        || (caseId == "14-unbounded-receive-lifecycle" && fixture.EndsWith("xcdr1", StringComparison.Ordinal));

    /// <summary>Identifies lifecycle fixtures whose sample size must exceed a configured transport limit.</summary>
    internal static bool IsFragmentedLifecycle(string caseId, string fixture) =>
        caseId == "14-unbounded-receive-lifecycle" && fixture.StartsWith("fragmented-", StringComparison.Ordinal);

    /// <summary>Creates a representation policy containing only XCDR2.</summary>
    internal static DataRepresentation Xcdr2Only => DataRepresentation.Default.With(
        policy =>
        {
            policy.Value.Clear();
            policy.Value.Add(DataRepresentation.Xcdr2);
        });

    /// <summary>Creates a representation policy containing only XCDR1.</summary>
    internal static DataRepresentation Xcdr1Only => DataRepresentation.Default.With(
        policy =>
        {
            policy.Value.Clear();
            policy.Value.Add(DataRepresentation.Xcdr);
        });
}
