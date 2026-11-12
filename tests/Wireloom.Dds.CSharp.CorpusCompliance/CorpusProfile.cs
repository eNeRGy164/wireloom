using Wireloom.Dds.CSharp.Compiler;
using Wireloom.Dds.Idl;

namespace Wireloom.Dds.CSharp.CorpusCompliance;

/// <summary>Describes the compiler and unavailable corpus features for an exact RTI profile.</summary>
internal sealed record CorpusProfile(string Version, IdlCompilerProfile CompilerProfile, IReadOnlyDictionary<string, string> UnsupportedCases)
{
    /// <summary>Gets the profile selected for the current corpus run.</summary>
    internal static CorpusProfile Selected => ForVersion(Environment.GetEnvironmentVariable("CORPUS_RTI_PROFILE") ?? "7.7.0");

    /// <summary>Resolves one supported exact version without inferring feature support from oracle acceptance.</summary>
    internal static CorpusProfile ForVersion(string version) => version switch
    {
        "6.1.2" => Rti612,
        "7.3.1" => Rti731,
        "7.7.0" => Rti770,
        _ => throw new InvalidOperationException($"Unsupported corpus RTI profile: {version}")
    };

    private static CorpusProfile Rti612 { get; } = new("6.1.2", IdlCompilerProfile.Rti612,
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["06-aggregates"] = "Recursive aggregate types",
            ["09-optional-collections"] = "Optional sequences and arrays",
            ["09-must-understand"] = "@must_understand",
            ["09-optional-string-sequences"] = "Optional sequences and arrays",
            ["09-optional-recursive-array"] = "Optional sequences and arrays"
        });

    private static CorpusProfile Rti731 { get; } = new("7.3.1", IdlCompilerProfile.Rti731,
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["09-must-understand"] = "@must_understand"
        });

    private static CorpusProfile Rti770 { get; } = new("7.7.0", IdlCompilerProfile.Rti770,
        new Dictionary<string, string>(StringComparer.Ordinal));
}
