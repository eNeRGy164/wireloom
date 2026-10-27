using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Preserves the standalone discovery-registry contract at the command boundary.</summary>
internal static class CaseRegistry
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    };

    /// <summary>Writes the legacy registry only when requested by a standalone caller.</summary>
    internal static void Write(IReadOnlyList<CaseDescriptor> descriptors, string destination)
    {
        var cases = descriptors.Select(item => new { id = item.CaseId, csharpType = item.WireloomType, cppType = item.CppType }).ToArray();

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(destination))!);

        File.WriteAllText(destination, JsonSerializer.Serialize(new { cases }, JsonOptions) + "\n");
    }
}
