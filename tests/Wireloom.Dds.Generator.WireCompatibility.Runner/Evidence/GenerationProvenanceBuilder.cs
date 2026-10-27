using System.Security.Cryptography;
using System.Text.Json;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Captures the manifest selection and hashes actual build inputs and emitted sources in-process.</summary>
internal static class GenerationProvenanceBuilder
{
    /// <summary>Creates typed evidence without writing an intermediate or retained artifact.</summary>
    internal static GenerationProvenance Create(string repositoryRoot, string manifestPath, string caseId, string generatedRoot, string version)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(manifestPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(caseId);
        ArgumentException.ThrowIfNullOrWhiteSpace(generatedRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(version);

        using var manifest = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var definition = manifest.RootElement.GetProperty("positiveCases").EnumerateArray()
            .FirstOrDefault(item => item.GetProperty("id").GetString() == caseId);
        if (definition.ValueKind == JsonValueKind.Undefined)
        {
            throw new InvalidOperationException($"No positive corpus case found for {caseId}.");
        }

        var idl = definition.GetProperty("idl").GetString() ?? throw new InvalidDataException($"Missing IDL path for {caseId}.");

        var mapping = "RtiCSharp";
        if (definition.TryGetProperty("sequenceArrayMapping", out var configuredMapping))
        {
            mapping = configuredMapping.GetString() ?? mapping;
        }

        var canonicalIdl = Path.GetFullPath(Path.Combine(repositoryRoot, "docs", "corpus", idl));
        var oracleIdl = IdlSourceResolver.ResolveOracleIdl(repositoryRoot, caseId, canonicalIdl);
        var dependencies = IdlSourceResolver.ResolveIncludedIdlFiles(oracleIdl,
            Path.Combine(repositoryRoot, "docs", "corpus", "idl"), Path.Combine(repositoryRoot, "docs", "corpus", "idl", "includes"));
        var generated = Directory.GetFiles(generatedRoot, "*.g.cs", SearchOption.AllDirectories).Order(StringComparer.Ordinal).ToArray();
        if (generated.Length == 0)
        {
            throw new InvalidDataException("Generation provenance requires emitted source files.");
        }

        return new GenerationProvenance(
            SequenceArrayMapping: mapping,
            RtiVersion: version,
            Idl: idl,
            IdlSha256: HashFile(canonicalIdl),
            OracleIdl: RelativePath(repositoryRoot, oracleIdl),
            OracleIdlSha256: HashFile(oracleIdl),
            OracleDependencies: [.. dependencies.Select(path => DescribeFile(repositoryRoot, path))],
            GeneratedSources: [.. generated.Select(path => DescribeFile(generatedRoot, path))]);
    }

    private static ProvenanceFile DescribeFile(string root, string path) => new(RelativePath(root, path), HashFile(path));

    private static string HashFile(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream));
    }

    private static string RelativePath(string root, string path) => Path.GetRelativePath(root, path).Replace('\\', '/');
}
