namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Writes retained peer compilation evidence.</summary>
internal static class PeerBuildFiles
{
    /// <summary>Header of retained native-compilation evidence.</summary>
    internal const string CompilationHeader = "case_id\tstatus\tdetail";

    /// <summary>Writes native evidence, including cases rejected before CMake could run.</summary>
    internal static void WriteCompilation(string path, IEnumerable<BuildOutcome> outcomes) => File.WriteAllLines(path,
        new[] { CompilationHeader }.Concat(outcomes.Select(outcome => string.Join('\t', outcome.CaseId, outcome.Status, outcome.Detail))));

}
