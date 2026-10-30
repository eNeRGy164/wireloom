using Wireloom.Dds.Generator.WireCompatibility.Runner;

if (args.Length > 0 && args[0] == "--lifecycle-verification-peer")
{
    return await LifecycleVerificationPeer.RunAsync(args[1..]);
}

if (args.SequenceEqual(["--verify-harness"]))
{
    return await HarnessVerification.RunAsync();
}

try
{
    var options = MatrixOptions.FromEnvironment();
    var workspace = new MatrixWorkspace(options);
    var processes = new ProcessRunner(options.RepositoryRoot);
    var log = new WorkflowLog();
    var cases = new CorpusCaseCatalog(workspace.Manifest);
    var oracles = new WireOracleCatalog(workspace.OracleManifest);
    var discovery = new WireloomTypeDiscovery(cases);
    var planner = new MatrixPlanner(options, workspace, log);
    var managed = new ManagedPeerBuilder(options, workspace, cases, oracles, processes, log);
    var sources = new NativePeerSources(options, workspace, cases, oracles);
    var native = new NativePeerBuilder(options, workspace, sources, processes, log);
    var provenance = new GenerationProvenanceRecorder(options, workspace);
    var preparation = new PeerPreparation(options, workspace, discovery, managed, native, provenance, log);
    var exchanges = new ExchangeRunner(options, workspace, log);
    var report = new MatrixReport(options, workspace, log);
    var runner = new MatrixRunner(options, workspace, new RtiEnvironment(options.RepositoryRoot), planner, preparation, exchanges, report, log);

    return await runner.RunAsync();
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);

    new WorkflowLog().Annotate("error", "Wire compatibility runner configuration failed", exception.Message);

    return 2;
}
