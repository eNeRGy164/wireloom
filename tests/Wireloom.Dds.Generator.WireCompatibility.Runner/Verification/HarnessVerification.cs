namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>License-free regression checks for lifecycle policy, recovery artifacts, and build contracts.</summary>
internal static class HarnessVerification
{
    /// <summary>Executes the harness checks without initializing RTI or launching DDS peers.</summary>
    internal static async Task<int> RunAsync()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), "wireloom-runner-verification-" + Guid.NewGuid().ToString("N"));

        try
        {
            VerifyFinalizationPolicy();

            LifecyclePhaseProtocolVerification.Run();
            LifecycleEvidenceVerification.Run(Path.Combine(temporaryDirectory, "lifecycle-evidence"));
            SourceSnapshotVerification.Run(Path.Combine(temporaryDirectory, "source-snapshot"));

            await LifecycleSchedulerVerification.RunAsync(Path.Combine(temporaryDirectory, "lifecycle-scheduler"));

            var options = MatrixOptions.FromEnvironment() with
            {
                CompileOnly = true,
                ScratchDirectory = Path.Combine(temporaryDirectory, "scratch"),
                OutputDirectory = Path.Combine(temporaryDirectory, "output"),
            };
            VerifyReportRecovery(options);

            ScenarioPlanningVerification.Run(options);
            NativePeerScopePlanningVerification.Run(options);
            PeerPairingVerification.Run(options);
            CaseDiscoveryVerification.Run(new MatrixWorkspace(options));
            FixtureCatalogVerification.Run();
            WireOracleCatalogVerification.Run(options);
            CppOracleCatalogVerification.Run(options);
            WireloomTypeDiscoveryVerification.Run(options);
            GenerationProvenanceVerification.Run(options);
            ReportRenderingVerification.Run(options);

            var processes = new ProcessRunner(options.RepositoryRoot);

            await VerifyInProcessBuildsAsync(options);
            await CaseMetadataVerification.RunAsync(options);
            await RtiTypeDiscoveryVerification.RunAsync(options, processes);

            Console.WriteLine("Harness recovery and in-process build contracts passed: partial evidence, stale-artifact isolation, report failures, native build failure, empty scheduler input; no RTI license required.");

            return 0;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception.ToString());

            return 1;
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
            {
                foreach (var file in Directory.GetFiles(temporaryDirectory, "*", SearchOption.AllDirectories))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }

                Directory.Delete(temporaryDirectory, recursive: true);
            }
        }
    }

    private static void VerifyFinalizationPolicy()
    {
        // Arrange
        var expectedFatalFailure = new FinalizationOutcome(7, "error");
        var expectedHandledFailure = new FinalizationOutcome(0, "warning");
        var expectedFinalizationFailure = new FinalizationOutcome(1, "error");

        // Act
        var setupFailure = FinalizationOutcome.Resolve(7, 0, 1, compileOnly: false);
        var peerFailure = FinalizationOutcome.Resolve(0, 0, 1, compileOnly: false);
        var reportFailure = FinalizationOutcome.Resolve(7, 1, 1, compileOnly: false);
        var compileFailure = FinalizationOutcome.Resolve(0, 0, 1, compileOnly: true);

        // Assert
        Check(setupFailure == expectedFatalFailure, "A late setup failure remains a failure.");
        Check(peerFailure == expectedHandledFailure, "A handled peer failure is reported without failing the workflow.");
        Check(reportFailure == expectedFinalizationFailure, "A report failure remains visible and fails the workflow.");
        Check(compileFailure == expectedFinalizationFailure, "A C++ compile sweep fails when a peer fails to compile.");

        Console.WriteLine("Harness finalization passed: 4 checks, no RTI license required.");
    }

    private static void VerifyReportRecovery(MatrixOptions options)
    {
        // Arrange
        var workspace = new MatrixWorkspace(options);
        workspace.Initialize();
        File.WriteAllText(workspace.Scenarios, "partial-plan\n");
        PeerBuildFiles.WriteCompilation(workspace.CppResults, [new BuildOutcome("example", "compile-failed", "build.log")]);

        var log = new WorkflowLog(emitAnnotations: false);
        var report = new MatrixReport(options, workspace, log);
        var failedRun = new MatrixRunOutcome(MatrixPlan.Empty, PeerPreparationResult.Empty, 7, ExchangeIssues: false);

        // Act
        var exitCode = report.Complete(failedRun);

        // Assert
        Check(exitCode == 7, "Recovery reporting must preserve the fatal execution exit code.");
        Check(File.ReadAllText(workspace.Output("scenarios.tsv")) == "partial-plan\n", "Recovery reporting must retain the partial scenario plan.");
        Check(File.ReadAllText(workspace.Output("cpp-compilation.tsv")).Contains("compile-failed", StringComparison.Ordinal), "Recovery reporting must retain native build failures.");

        // Arrange: an invocation failing before initialization must not relabel old scratch files.
        var uninitialized = new MatrixWorkspace(options);
        var earlyFailureReport = new MatrixReport(options, uninitialized, log);
        File.WriteAllText(workspace.Output("scenarios.tsv"), "previous-report\n");

        // Act
        var earlyFailureStatus = earlyFailureReport.Complete(failedRun);

        // Assert
        Check(earlyFailureStatus == 7, "Early setup failure must preserve its exit status.");
        Check(File.ReadAllText(workspace.Output("scenarios.tsv")) == "previous-report\n", "Early setup failure must not publish stale scratch evidence.");

        // Arrange
        File.Delete(workspace.Output("cpp-compilation.tsv"));
        Directory.CreateDirectory(workspace.Output("cpp-compilation.tsv"));

        var originalError = Console.Error;
        using var diagnostics = new StringWriter();

        try
        {
            Console.SetError(diagnostics);

            // Act
            var reportFailure = report.Complete(failedRun);

            // Assert
            Check(reportFailure == 1, "An artifact write failure must fail finalization.");
            Check(diagnostics.ToString().Contains("Failed to generate", StringComparison.Ordinal), "An artifact write failure must remain visible.");
        }
        finally
        {
            Console.SetError(originalError);
        }

        Directory.Delete(workspace.Output("cpp-compilation.tsv"));
    }

    private static async Task VerifyInProcessBuildsAsync(MatrixOptions options)
    {
        // Arrange
        var workspace = new MatrixWorkspace(options);
        var processes = new ProcessRunner(options.RepositoryRoot);
        var paths = workspace.ForCase("example");

        Directory.CreateDirectory(paths.Directory);

        var prepared = new PreparedCase(new CaseDescriptor("example", "Example.Type", "Example::Type", "example.idl"), paths,
            new ManagedPeer("Example.Type", paths.WireloomPeer, true, "Wireloom unavailable"),
            new ManagedPeer("-", paths.RtiPeer, false, "RTI unavailable"), false, "Wireloom C++ is not selected for this case.");

        var cases = new CorpusCaseCatalog(workspace.Manifest);
        var oracles = new WireOracleCatalog(workspace.OracleManifest);
        var sources = new NativePeerSources(options, workspace, cases, oracles);
        var nativeBuilder = new NativePeerBuilder(options, workspace, sources, cases, processes, new WorkflowLog());

        // Act: a nonexistent CMake tree is a handled build failure in the in-process builder.
        var build = await nativeBuilder.BuildAsync([prepared]);
        var outcome = build.Outcomes.Single();

        // Assert
        Check(build.HasIssues && !outcome.IsCompiled, "A missing CMake build tree must be reported as a handled native compilation failure.");
        Check(build.Cases is [{ NativePeerAvailable: false, WireloomCppPeer: null }],
            "A case must remain schedulable when both native peers fail to build.");
        var managedOnlyPrepared = prepared with
        {
            Rti = new ManagedPeer("Example.RtiType", paths.RtiPeer, true, "RTI unavailable"),
        };
        var retainedCaseBuild = ExchangeCaseBuild.From(build.Cases.Single() with { Case = managedOnlyPrepared });
        Check(ExchangeScheduler.MissingPeerReason(retainedCaseBuild, "wireloom-cs", "rti-cs") is null,
            "Native build failures must not block an available managed-only exchange.");
        Check(ExchangeScheduler.MissingPeerReason(retainedCaseBuild, "rti-cpp", "wireloom-cpp") is not null,
            "Only exchanges requiring a missing native peer must be marked unavailable.");
        var retainedBuildLog = Path.Combine(workspace.Logs, "example-rti-cpp-build.log");
        var retainedBuildLogReference = Path.GetRelativePath(options.RepositoryRoot, retainedBuildLog).Replace('\\', '/');
        Check(outcome.CaseId == "example" && outcome.Detail == $"see {retainedBuildLogReference}"
            && File.Exists(retainedBuildLog), "Native build identity and retained failure-log paths must survive the in-process build flow.");

        // Arrange
        await File.WriteAllTextAsync(workspace.Scenarios, string.Empty);

        // Act: the in-process scheduler accepts an empty plan without launching peers.
        var exchangeOptions = options with
        {
            CompileOnly = false
        };
        var exchangeRunner = new ExchangeRunner(exchangeOptions, workspace, new WorkflowLog());
        var exchange = await exchangeRunner.RunAsync(MatrixPlan.Empty,
            new PeerPreparationResult([new BuiltCase(prepared, paths.NativePeer, true, string.Empty, null)], [], false));

        // Assert
        Check(!exchange, "The in-process scheduler must accept an empty plan without launching peers.");

        // Arrange: a planned row whose native configuration failed must still receive explicit failure evidence.
        var scenario = new PlannedScenario("example", "default", PeerNames.WireloomCpp, PeerNames.RtiCpp, "7.7.0");
        var scenarioPlan = new ScenarioPlan("7.7.0", 0, 1,
            [new PlannedCase("example", 0, [scenario])], [scenario],
            new HashSet<string>(StringComparer.Ordinal),
            new HashSet<string>(["example"], StringComparer.Ordinal),
            new HashSet<string>(StringComparer.Ordinal));
        var matrixPlan = new MatrixPlan(["example"], new HashSet<string>(StringComparer.Ordinal), scenarioPlan);
        var scenarioRow = new ScenarioRow("example", "", "", "", scenario.Writer, "7.7.0",
            scenario.Reader, "7.7.0", "NOT_RUN", scenario.Fixture);
        await File.WriteAllTextAsync(workspace.Scenarios, scenarioRow.ToTsv() + Environment.NewLine);
        var unpreparedPeerOutcomes = new PeerPreparationResult([],
            [new BuildOutcome("example", "cmake-configure-failed", "cmake-configure.log")], true);

        // Act
        var missingPeerExchange = await exchangeRunner.RunAsync(matrixPlan, unpreparedPeerOutcomes);
        var updatedScenario = ScenarioRow.Parse((await File.ReadAllTextAsync(workspace.Scenarios)).Trim());
        var failureLog = await File.ReadAllTextAsync(Path.Combine(exchangeOptions.OutputDirectory, "logs",
            "example-default-wireloom-cpp-rti-cpp.log"));

        // Assert
        Check(missingPeerExchange && updatedScenario.Status == "FAIL",
            "A planned exchange for an unprepared case must be marked FAIL instead of remaining NOT_RUN.");
        Check(failureLog.Contains("cmake-configure-failed: cmake-configure.log", StringComparison.Ordinal),
            "An unprepared exchange failure log must retain its matching native build diagnostic.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
