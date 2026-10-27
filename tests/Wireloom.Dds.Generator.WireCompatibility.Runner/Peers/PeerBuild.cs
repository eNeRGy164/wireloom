namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Availability and launch details for one managed peer.</summary>
/// <param name="TypeName">Qualified type registered by the peer.</param>
/// <param name="Path">Entry assembly.</param>
/// <param name="Available">Whether compilation succeeded.</param>
/// <param name="Failure">Scenario diagnostic used when the peer is unavailable.</param>
internal sealed record ManagedPeer(string TypeName, string Path, bool Available, string Failure);

/// <summary>A fully staged case waiting for native compilation.</summary>
/// <param name="Descriptor">Case identity and source types.</param>
/// <param name="Paths">Isolated build paths.</param>
/// <param name="Wireloom">Wireloom managed peer.</param>
/// <param name="Rti">RTI-generated managed control peer.</param>
internal sealed record PreparedCase(CaseDescriptor Descriptor, CaseWorkspace Paths, ManagedPeer Wireloom, ManagedPeer Rti);

/// <summary>A case that can participate in the exchange scheduler.</summary>
/// <param name="Case">Staged case and managed peer availability.</param>
/// <param name="NativePeer">Successfully compiled native executable.</param>
internal sealed record BuiltCase(PreparedCase Case, string NativePeer);

/// <summary>A case build or preparation outcome, retained independently of workflow exit status.</summary>
/// <param name="CaseId">Corpus identifier.</param>
/// <param name="Status">Worker or preparation status.</param>
/// <param name="Detail">Artifact path or failure detail.</param>
/// <param name="ElapsedSeconds">Duration of a managed or native build.</param>
internal sealed record BuildOutcome(string CaseId, string Status, string Detail, double ElapsedSeconds = 0)
{
    /// <summary>Whether the requested peer compiled.</summary>
    internal bool IsCompiled => Status == "compiled";

    /// <summary>Whether native sources have a configured build tree ready for compilation.</summary>
    internal bool IsConfigured => Status == "configured";
}

/// <summary>Typed inputs for one managed peer build after bootstrap.</summary>
/// <param name="Paths">Case-specific build paths.</param>
/// <param name="Input">IDL mapping policy.</param>
/// <param name="IdlPath">Absolute IDL path.</param>
internal sealed record ManagedBuildRequest(CaseWorkspace Paths, IdlInput Input, string IdlPath);

/// <summary>Completed managed build batch and any handled build failure.</summary>
/// <param name="Outcomes">Build results indexed by case.</param>
/// <param name="HasIssues">Whether any requested build or worker failed.</param>
internal sealed record ManagedBuildBatch(IReadOnlyDictionary<string, BuildOutcome> Outcomes, bool HasIssues);

/// <summary>Peer artifacts and build evidence passed to execution and reporting.</summary>
/// <param name="BuiltCases">Cases with compiled native peers.</param>
/// <param name="NativeOutcomes">Every native preparation and compilation outcome.</param>
/// <param name="HasIssues">Whether managed preparation, fixtures, or native builds failed.</param>
internal sealed record PeerPreparationResult(IReadOnlyList<BuiltCase> BuiltCases, IReadOnlyList<BuildOutcome> NativeOutcomes, bool HasIssues)
{
    /// <summary>Recovery-report context before peer preparation completes.</summary>
    internal static PeerPreparationResult Empty { get; } = new([], [], false);
}
