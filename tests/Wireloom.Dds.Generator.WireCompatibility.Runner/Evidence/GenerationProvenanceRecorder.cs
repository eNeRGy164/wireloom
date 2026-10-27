namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns the provenance capture step between a successful managed build and peer execution.</summary>
internal sealed class GenerationProvenanceRecorder(MatrixOptions options, MatrixWorkspace workspace)
{
    /// <summary>Captures actual inputs and emitted files and retains one evidence artifact for the case.</summary>
    internal void Record(CaseWorkspace paths)
    {
        var provenance = GenerationProvenanceBuilder.Create(options.RepositoryRoot, workspace.Manifest,
            paths.CaseId, paths.WireloomGenerated, options.RtiVersion);

        GenerationProvenanceWriter.Write(provenance, workspace.GenerationProvenancePath(paths.CaseId));
    }
}
