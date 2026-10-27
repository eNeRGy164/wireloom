using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Typed build fields selected from one positive corpus case.</summary>
/// <param name="Idl">Corpus-relative IDL path, or an empty string when omitted.</param>
/// <param name="SequenceArrayMapping">Configured mapping or the RTI C# default.</param>
/// <param name="Defines">Ordered preprocessor definitions.</param>
/// <param name="RtiCppCodegenOptions">Ordered native generator options.</param>
/// <param name="RtiCppIdlAdaptations">Ordered native IDL adaptation tokens.</param>
[PublicAPI]
internal sealed record CaseMetadata(string Idl, string SequenceArrayMapping, IReadOnlyList<string> Defines,
    IReadOnlyList<string> RtiCppCodegenOptions, IReadOnlyList<string> RtiCppIdlAdaptations)
{
    /// <summary>Generation inputs passed to an isolated managed build.</summary>
    internal IdlInput Input => new(Idl, SequenceArrayMapping);

    /// <summary>Decodes optional fields with the standalone command's defaults and declaration order.</summary>
    internal static CaseMetadata Read(JsonNode definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        return new CaseMetadata((string?)definition["idl"] ?? "", (string?)definition["sequenceArrayMapping"] ?? "RtiCSharp",
            ReadList(definition["defines"]), ReadList(definition["wireMatrix"]?["rtiCppCodegenOptions"]),
            ReadList(definition["wireMatrix"]?["rtiCppIdlAdaptations"]));
    }

    /// <summary>Projects typed fields onto the retained standalone command's line-oriented contract.</summary>
    internal IReadOnlyList<string> FieldLines(string field) => field switch
    {
        "idl" => [Idl],
        "sequence-array-mapping" => [SequenceArrayMapping],
        "defines" => Defines,
        "rti-cpp-codegen-options" => RtiCppCodegenOptions,
        "rti-cpp-idl-adaptations" => RtiCppIdlAdaptations,
        _ => throw new ArgumentException($"Unknown case field: {field}"),
    };

    private static string[] ReadList(JsonNode? values) => [.. (values?.AsArray() ?? []).Select(value => (string)value!)];
}

// ReSharper disable once NotAccessedPositionalProperty.Global
// ManagedPeerBuilder consumes both inputs through its queued build request.
// ReSharper disable once NotAccessedPositionalProperty.Global
/// <summary>Generation inputs needed by a case-specific managed build.</summary>
/// <param name="Idl">Corpus-relative IDL path.</param>
/// <param name="Mapping">Sequence-array mapping selected by the manifest.</param>
[PublicAPI]
internal sealed record IdlInput(string Idl, string Mapping);
