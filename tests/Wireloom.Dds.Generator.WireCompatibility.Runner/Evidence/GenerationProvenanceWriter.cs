using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Preserves the retained generation-evidence JSON contract consumed by reports and shard merging.</summary>
internal static class GenerationProvenanceWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    };

    /// <summary>Writes the typed evidence directly to its retained artifact path.</summary>
    internal static void Write(GenerationProvenance provenance, string destination)
    {
        ArgumentNullException.ThrowIfNull(provenance);
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);

        var evidence = new
        {
            sequenceArrayMapping = provenance.SequenceArrayMapping,
            rtiVersion = provenance.RtiVersion,
            idl = provenance.Idl,
            idlSha256 = provenance.IdlSha256,
            oracleIdl = provenance.OracleIdl,
            oracleIdlSha256 = provenance.OracleIdlSha256,
            oracleDependencies = provenance.OracleDependencies.Select(file => new { path = file.Path, sha256 = file.Sha256 }),
            generatedSources = provenance.GeneratedSources.Select(file => new { path = file.Path, sha256 = file.Sha256 }),
        };

        File.WriteAllText(destination, JsonSerializer.Serialize(evidence, JsonOptions));
    }
}
