namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns repository paths and the initial on-disk state for one matrix run.</summary>
internal sealed class MatrixWorkspace(MatrixOptions options)
{
    /// <summary>Whether scratch artifacts belong to this invocation and can be retained as its evidence.</summary>
    internal bool HasRunArtifacts
    {
        get; private set;
    }

    /// <summary>Peer projects and fixture assets.</summary>
    internal string TestRoot => Path.Combine(options.RepositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility");

    /// <summary>Wireloom peer project.</summary>
    internal string WireloomProject => Path.Combine(TestRoot, "Wireloom.Dds.Generator.WireCompatibility.csproj");

    /// <summary>RTI-generated managed control peer project.</summary>
    internal string RtiProject => Path.Combine(TestRoot, "RtiOracleProbe", "RtiOracleProbe.csproj");

    /// <summary>Corpus source of truth.</summary>
    internal string Manifest => Path.Combine(options.RepositoryRoot, "docs", "corpus", "manifest.json");

    /// <summary>Profile-specific eligibility and pairing policy.</summary>
    internal string Exclusions => Path.Combine(TestRoot, $"wire-case-exclusions-{options.RtiVersion}.json");

    /// <summary>Captured oracle selection policy.</summary>
    internal string OracleManifest => Path.Combine(options.RepositoryRoot, "docs", "corpus", options.RtiVersion, "rtiddsgen", "wire-oracles.json");

    /// <summary>Authored native peer templates.</summary>
    internal string NativeTemplates => Path.Combine(TestRoot, "Native");

    /// <summary>Canonical retained scenario evidence, updated directly by the exchange scheduler.</summary>
    internal string Scenarios => Output("scenarios.tsv");

    /// <summary>Native compilation evidence, including preparation failures.</summary>
    internal string CppResults => Scratch("cpp-compilation.tsv");

    /// <summary>Retained per-case diagnostic files.</summary>
    internal string Logs => Output("logs");

    /// <summary>Source identity captured before any peer build or exchange begins.</summary>
    internal string ExecutionProvenance => Output("execution-provenance.json");

    /// <summary>Resets scratch files and initializes artifacts needed by recovery reporting.</summary>
    internal void Initialize(bool captureSourceProvenance = true)
    {
        HasRunArtifacts = false;

        // Scratch
        if (Directory.Exists(options.ScratchDirectory))
        {
            Directory.Delete(options.ScratchDirectory, recursive: true);
        }
        Directory.CreateDirectory(options.ScratchDirectory);

        // Logs
        Directory.CreateDirectory(options.OutputDirectory);
        Directory.CreateDirectory(Logs);
        foreach (var file in Directory.EnumerateFiles(Logs, "*.log").Concat(Directory.EnumerateFiles(Logs, "*-generation.json")))
        {
            File.Delete(file);
        }

        // Discard prior planning evidence, including the obsolete process handoff file.
        foreach (var name in new[] { "plan.json", "shard.json", "selected-cases.txt" })
        {
            File.Delete(Output(name));
        }

        HasRunArtifacts = true;
        File.Delete(ExecutionProvenance);
        File.WriteAllText(Scenarios, string.Empty);
        File.WriteAllText(CppResults, PeerBuildFiles.CompilationHeader + Environment.NewLine);
        File.WriteAllText(Output("results.json"), $$"""{"rtiVersion":"{{options.RtiVersion}}","scenarios":[]}""");

        var retainedSourceFiles = Output("source-files");
        if (Directory.Exists(retainedSourceFiles))
        {
            Directory.Delete(retainedSourceFiles, recursive: true);
        }

        if (captureSourceProvenance)
        {
            SourceSnapshotEvidence.Capture(options.RepositoryRoot, options.OutputDirectory).Write(ExecutionProvenance);
        }
    }

    /// <summary>Locates disposable artifacts for one corpus case.</summary>
    internal CaseWorkspace ForCase(string caseId) => new(caseId, Scratch(caseId));

    /// <summary>Locates a retained report or artifact.</summary>
    internal string Output(string name) => Path.Combine(options.OutputDirectory, name);

    /// <summary>Locates retained source evidence consumed by reporting and shard merging.</summary>
    internal string GenerationProvenancePath(string caseId) => Path.Combine(Logs, caseId + "-generation.json");

    /// <summary>Locates a source capture in the selected exact-profile oracle set.</summary>
    internal string OracleDirectory(string relativePath) => Path.Combine(options.RepositoryRoot, "docs", "corpus", options.RtiVersion, "rtiddsgen", relativePath.Replace('/', Path.DirectorySeparatorChar));

    /// <summary>Locates an IDL document in the checked-in corpus.</summary>
    internal string CorpusFile(string relativePath) => Path.Combine(options.RepositoryRoot, "docs", "corpus", relativePath.Replace('/', Path.DirectorySeparatorChar));

    private string Scratch(string name) => Path.Combine(options.ScratchDirectory, name);
}

/// <summary>Paths owned by one isolated case build.</summary>
/// <param name="CaseId">Corpus case identifier.</param>
/// <param name="Directory">Root of the case's scratch directory.</param>
internal sealed record CaseWorkspace(string CaseId, string Directory)
{
    /// <summary>Published Wireloom peer files.</summary>
    internal string WireloomBin => Path.Combine(Directory, "wireloom-bin");

    /// <summary>Isolated managed intermediate files.</summary>
    internal string WireloomObj => Path.Combine(Directory, "wireloom-obj");

    /// <summary>Generated Wireloom sources.</summary>
    internal string WireloomGenerated => Path.Combine(Directory, "wireloom-generated");

    /// <summary>Managed peer entry assembly.</summary>
    internal string WireloomPeer => Path.Combine(WireloomBin, $"WireCompatibility.{CaseId}.dll");

    /// <summary>Managed peer build diagnostics.</summary>
    internal string WireloomBuildLog => Path.Combine(Directory, "wireloom-csharp-build.log");

    /// <summary>Published RTI-generated managed peer.</summary>
    internal string RtiPeer => Path.Combine(Directory, "rti-bin", "WireCompatibility.RtiOracleProbe.dll");

    /// <summary>Staged C++ sources.</summary>
    internal string Native => Path.Combine(Directory, "native");

    /// <summary>CMake build tree.</summary>
    internal string NativeBuild => Path.Combine(Native, "build");

    /// <summary>Native peer executable.</summary>
    internal string NativePeer => Path.Combine(NativeBuild, "wire-peer");

    /// <summary>Native compilation diagnostics.</summary>
    internal string NativeBuildLog => Path.Combine(Directory, "cmake-build.log");
}
