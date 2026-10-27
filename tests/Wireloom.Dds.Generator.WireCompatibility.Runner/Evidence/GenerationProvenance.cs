namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Input and emitted-source evidence for one actual managed peer build.</summary>
/// <param name="SequenceArrayMapping">Manifest mapping, defaulting to RtiCSharp.</param>
/// <param name="RtiVersion">Exact runtime used by the build.</param>
/// <param name="Idl">Canonical corpus-relative input path.</param>
/// <param name="IdlSha256">SHA-256 of the canonical input bytes.</param>
/// <param name="OracleIdl">Repository-relative wrapper or canonical oracle input.</param>
/// <param name="OracleIdlSha256">SHA-256 of the selected oracle input bytes.</param>
/// <param name="OracleDependencies">Ordered transitive oracle include hashes.</param>
/// <param name="GeneratedSources">Ordered emitted-source hashes relative to the generation directory.</param>
internal sealed record GenerationProvenance(string SequenceArrayMapping, string RtiVersion, string Idl, string IdlSha256,
    string OracleIdl, string OracleIdlSha256, IReadOnlyList<ProvenanceFile> OracleDependencies, IReadOnlyList<ProvenanceFile> GeneratedSources);

/// <summary>A normalized relative source path and its exact byte hash.</summary>
/// <param name="Path">Forward-slash relative path.</param>
/// <param name="Sha256">Uppercase SHA-256 digest.</param>
internal sealed record ProvenanceFile(string Path, string Sha256);
