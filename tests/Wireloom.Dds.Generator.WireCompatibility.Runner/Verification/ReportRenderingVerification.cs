using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks in-process report artifacts, evidence guards, and finalization failures without a helper DLL.</summary>
internal static class ReportRenderingVerification
{
    /// <summary>Renders all profiles and exercises recovery and partial writes before utilities are built.</summary>
    internal static void Run(MatrixOptions options)
    {
        foreach (var version in new[] { "6.1.2", "7.3.1", "7.7.0" })
        {
            VerifyProfile(options, version);
        }

        Console.WriteLine("Report rendering passed: all profiles, JSON/Markdown, log details, provenance guards, helper-free recovery, malformed rows, and partial artifact writes.");
    }

    private static void VerifyProfile(MatrixOptions options, string version)
    {
        var flatDataEndpoint = ReportEndpointPolicy.Create("10-flat-data-binding", "non-default", "RTI C++", version, isWriter: true);
        var hasLegacyPlainMappingAdaptation = flatDataEndpoint.ContainsKey("adaptation");
        Check(hasLegacyPlainMappingAdaptation == (version != "7.7.0"),
            "W014 must describe only legacy PLAIN-mapping RTI C++ reference profiles.");

        // Arrange
        var root = Path.Combine(options.ScratchDirectory, "report-verification", version);
        var reportOptions = options with
        {
            RtiVersion = version,
            CompileOnly = false,
            ScratchDirectory = Path.Combine(root, "scratch"),
            OutputDirectory = Path.Combine(root, "output"),
        };

        var workspace = new MatrixWorkspace(reportOptions);
        workspace.Initialize();
        var log = new WorkflowLog(emitAnnotations: false);

        var rows = new[]
        {
            Row(version, "Wireloom C#", "RTI C#", "PASS"),
            Row(version, "RTI C#", "Wireloom C#", "FAIL"),
            Row(version, "RTI C++", "RTI C++", "NOT_RUN"),
        };

        File.WriteAllLines(workspace.Scenarios, rows.Select(row => row.ToTsv()));
        File.WriteAllText(Path.Combine(workspace.Logs, "01-boundaries-non-default-rti-cs-wireloom-cs.log"),
            """
            FAIL_DETAIL Expected 10; observed 20.
            ENDPOINT_STATUS writer_exit_code=139 reader_exit_code=0
            """);
        File.WriteAllText(Path.Combine(workspace.Logs, "01-boundaries-non-default-rti-cpp-rti-cpp.log"), "NOT_RUN_DETAIL Native peer was unavailable.\n");

        var request = new ReportRequest(version, rows.Length, workspace.Scenarios, workspace.Output("results.json"),
            workspace.Output("results.md"), workspace.Logs, workspace.Exclusions, workspace.Manifest, options.RepositoryRoot,
            WireloomCppCases: new HashSet<string>(StringComparer.Ordinal));

        // Act
        WireReportRenderer.Render(request);
        var document = JsonNode.Parse(File.ReadAllText(request.JsonPath))!;
        var markdown = File.ReadAllText(request.MarkdownPath);

        // Assert
        Check((string?)document["rtiVersion"] == version && (int)document["scenarioCount"]! == 3 && (int)document["fixtureCount"]! == 1, "Report identity and counts must retain scheduler evidence.");

        var scenarios = document["scenarios"]!.AsArray();
        Check(!string.IsNullOrWhiteSpace((string?)scenarios[0]!["sourceRevision"])
            && ((string?)scenarios[0]!["dirtySourceSnapshotSha256"])?.Length == 64
            && (int?)scenarios[0]!["dirtySourceFileCount"] is >= 0,
            "Every scenario retains its source revision and dirty-source snapshot hash.");
        Check(((string?)scenarios[1]!["observedProblem"])!.Contains("Expected 10; observed 20", StringComparison.Ordinal) &&
            ((string?)scenarios[1]!["observedProblem"])!.Contains("SIGSEGV", StringComparison.Ordinal), "Observed failures must retain detail and endpoint exit interpretation.");
        Check((string?)scenarios[2]!["observedProblem"] == "Native peer was unavailable.", "NOT_RUN_DETAIL must retain precedence.");
        Check(markdown.Contains($"# {version} Wire compatibility results", StringComparison.Ordinal) && markdown.Contains("**3** DDS exchanges", StringComparison.Ordinal), "Markdown must describe the same report.");

        // Arrange / Act / Assert: retained evidence keeps its execution identity when rendered from a different checkout without Git.
        var retainedRevision = (string?)scenarios[0]!["sourceRevision"];
        var retainedHash = (string?)scenarios[0]!["dirtySourceSnapshotSha256"];
        var noGitRequest = request with { RepositoryRoot = root };
        var originalPath = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        try
        {
            Environment.SetEnvironmentVariable("PATH", string.Empty);
            WireReportRenderer.Render(noGitRequest);
        }
        finally
        {
            Environment.SetEnvironmentVariable("PATH", originalPath);
        }

        var rerenderedScenario = JsonNode.Parse(File.ReadAllText(request.JsonPath))!["scenarios"]![0]!;
        Check((string?)rerenderedScenario["sourceRevision"] == retainedRevision
            && (string?)rerenderedScenario["dirtySourceSnapshotSha256"] == retainedHash,
            "Rendering retained execution evidence from a different Git-free checkout must preserve its original source identity.");

        var missingProvenance = Path.Combine(root, "missing-execution-provenance.json");
        CheckRejected<InvalidDataException>(() => WireReportRenderer.Render(request with { ExecutionProvenancePath = missingProvenance }),
            "Rendering must reject missing execution provenance.");
        var malformedProvenance = Path.Combine(root, "malformed-execution-provenance.json");
        File.WriteAllText(malformedProvenance, "{ invalid json");
        CheckRejected<Exception>(() => WireReportRenderer.Render(request with { ExecutionProvenancePath = malformedProvenance }),
            "Rendering must reject malformed execution provenance.");

        // Arrange: a checkout mismatch at finalization marks prior PASS rows as failures while keeping the captured identity.
        var capturedProvenance = SourceSnapshotEvidence.Read(workspace.ExecutionProvenance);
        var changedProvenance = capturedProvenance with { Revision = "different-checkout-revision" };
        changedProvenance.Write(workspace.ExecutionProvenance);
        File.WriteAllLines(workspace.Scenarios, rows.Select(row => row.ToTsv()));

        // Act
        var finalizer = new MatrixReport(reportOptions, workspace, log);
        _ = finalizer.Complete(new MatrixRunOutcome(MatrixPlan.Empty, PeerPreparationResult.Empty, 9, ExchangeIssues: false));

        // Assert
        var driftReport = JsonNode.Parse(File.ReadAllText(request.JsonPath))!;
        Check((string?)driftReport["scenarios"]![0]!["status"] == "FAIL"
            && (string?)driftReport["scenarios"]![0]!["sourceRevision"] == "different-checkout-revision"
            && !(bool)driftReport["executionProvenance"]!["verifiedUnchanged"]!,
            "Source drift during an exchange must prevent a successful report without replacing the captured identity.");
        capturedProvenance.Write(workspace.ExecutionProvenance);

        // Arrange / Act / Assert: fatal setup/execution recovery must still render without tools.
        var failedRun = new MatrixRunOutcome(MatrixPlan.Empty, PeerPreparationResult.Empty, 9, ExchangeIssues: false);
        Check(finalizer.Complete(failedRun) == 9, "Successful recovery reporting must preserve the fatal execution status.");

        var retainedJson = File.ReadAllText(request.JsonPath);

        File.WriteAllText(workspace.Scenarios, "incomplete\trow\n");

        var originalError = Console.Error;

        using var diagnostics = new StringWriter();

        try
        {
            Console.SetError(diagnostics);
            Check(finalizer.Complete(failedRun) == 1, "A renderer failure must fail finalization even after a fatal run error.");
            Check(diagnostics.ToString().Contains("Scenario TSV row must contain 10 fields.", StringComparison.Ordinal), "The rendering failure must retain its underlying diagnostic.");
            Check(File.ReadAllText(request.JsonPath) == retainedJson, "Validation failure must not overwrite an existing report.");

            // Arrange / Act / Assert: JSON write failure must remain a finalization failure.
            File.WriteAllLines(workspace.Scenarios, rows.Select(row => row.ToTsv()));
            File.Delete(request.JsonPath);

            Directory.CreateDirectory(request.JsonPath);

            Check(finalizer.Complete(failedRun) == 1, "JSON artifact-write failure must fail finalization.");

            Directory.Delete(request.JsonPath);
        }
        finally
        {
            Console.SetError(originalError);
        }

        // Arrange / Act / Assert: preserve the valid JSON when the following Markdown write fails.
        var blockedMarkdown = Path.Combine(root, "markdown-is-a-directory");

        Directory.CreateDirectory(blockedMarkdown);

        CheckWriteRejected(() => WireReportRenderer.Render(request with { MarkdownPath = blockedMarkdown }));
        Check((int)JsonNode.Parse(File.ReadAllText(request.JsonPath))!["scenarioCount"]! == 3, "A Markdown write failure must retain the JSON already written.");

        // Arrange / Act / Assert: completed mapped exchanges still require generation provenance.
        var mapped = new ScenarioRow("05-array-of-sequences-idl", "Example.Type", "-", "::Example::Type",
            "Wireloom C#", version, "RTI C++", version, "PASS", "sequence-empty");

        File.WriteAllText(workspace.Scenarios, mapped.ToTsv() + "\n");

        CheckRejected<InvalidDataException>(() => WireReportRenderer.Render(request), "Missing generation provenance must still reject completed exchanges.");

        File.WriteAllText(workspace.Scenarios, (mapped with { Status = "NOT_RUN" }).ToTsv() + "\n");

        WireReportRenderer.Render(request with { ExpectedScenarioCount = 1 });

        Check((int)JsonNode.Parse(File.ReadAllText(request.JsonPath))!["scenarioCount"]! == 1, "Unrun exchanges must remain reportable without generation provenance.");

        if (version == "6.1.2")
        {
            // Arrange / Act: explicitly selected peer scope uses invocation scope, not the 7.7.0 qualification catalog.
            var cppRow = new ScenarioRow("01-boundaries", "Example.Sample", "Example.Sample", "::Example::Sample",
                "Wireloom C++", version, "RTI C++", version, "NOT_RUN", "non-default");
            File.WriteAllText(workspace.Scenarios, cppRow.ToTsv() + "\n");
            var explicitCppRequest = request with
            {
                ExpectedScenarioCount = 1,
                WireloomCppCases = new HashSet<string>(["01-boundaries"], StringComparer.Ordinal),
            };
            WireReportRenderer.Render(explicitCppRequest);

            // Assert
            var explicitCppReport = JsonNode.Parse(File.ReadAllText(request.JsonPath))!;
            Check((string?)explicitCppReport["scenarios"]![0]!["writer"]!["implementation"] == "Wireloom"
                  && (string?)explicitCppReport["scenarios"]![0]!["writer"]!["language"] == "C++",
                "An explicit 6.1.2 native peer scope must render under its selected exact RTI profile.");

            VerifyNativePeerScopeReport(request);
        }

        // Arrange / Act / Assert: FlatData workaround labels follow the selected RTI C++ oracle profile.
        var flatDataRow = new ScenarioRow("10-flat-data-binding", "CorpusFlatDataBinding.Sample", "CorpusFlatDataBinding.Sample",
            "::CorpusFlatDataBinding::Sample", "RTI C++", version, "RTI C++", version, "PASS", "non-default");
        File.WriteAllText(workspace.Scenarios, flatDataRow.ToTsv() + "\n");

        WireReportRenderer.Render(request with { ExpectedScenarioCount = 1 });

        var flatDataMarkdown = File.ReadAllText(request.MarkdownPath);
        Check(flatDataMarkdown.Contains("Workaround W014", StringComparison.Ordinal) == (version != "7.7.0"),
            "W014 must be reported only for legacy PLAIN-mapping RTI C++ reference profiles.");
    }

    private static void VerifyNativePeerScopeReport(ReportRequest request)
    {
        // Arrange
        const string caseId = "09-optional-collections";
        var fixtures = WireCompatibilityCaseCatalog.FixturesFor(caseId, "6.1.2");
        var rows = fixtures.SelectMany(fixture => new[]
        {
            new ScenarioRow(caseId, "CorpusOptionalCollections.Sample", "", "::CorpusOptionalCollections::Sample",
                PeerNames.RtiCpp, "6.1.2", PeerNames.WireloomCpp, "6.1.2", "NOT_RUN", fixture),
            new ScenarioRow(caseId, "CorpusOptionalCollections.Sample", "", "::CorpusOptionalCollections::Sample",
                PeerNames.WireloomCpp, "6.1.2", PeerNames.RtiCpp, "6.1.2", "NOT_RUN", fixture),
        }).ToArray();
        File.WriteAllLines(request.ScenarioPath, rows.Select(row => row.ToTsv()));
        var nativeRequest = request with
        {
            ExpectedScenarioCount = rows.Length,
            WireloomCppCases = new HashSet<string>([caseId], StringComparer.Ordinal),
            NativePeerScopeCases = new HashSet<string>([caseId], StringComparer.Ordinal),
        };

        // Act
        WireReportRenderer.Render(nativeRequest);
        var report = JsonNode.Parse(File.ReadAllText(nativeRequest.JsonPath))!;
        var scenarios = report["scenarios"]!.AsArray();
        var markdown = File.ReadAllText(nativeRequest.MarkdownPath);
        var fixtureRow = markdown.Split('\n').Single(line =>
            line.Contains($"`{caseId}`", StringComparison.Ordinal)
            && line.Contains($"| `{fixtures[0]}` |", StringComparison.Ordinal));

        // Assert
        Check((int)report["scenarioCount"]! == fixtures.Count * 2,
            "Native peer-scope reports must include both directed C++ exchanges for every fixture.");
        Check(scenarios.All(scenario => (string?)scenario!["writer"]!["language"] == "C++"
            && (string?)scenario["reader"]!["language"] == "C++"),
            "Native peer-scope report scenarios must contain only C++ endpoints.");
        var fixtureCells = fixtureRow.Split('|', StringSplitOptions.TrimEntries);
        Check(fixtureCells.Count(cell => cell == "N/A") == 14,
            $"Native peer-scope report must mark 14 managed peer cells N/A; row was: {fixtureRow}");
        Check(fixtureCells.Count(cell => cell == "❔") == 2,
            "Native peer-scope report must retain both selected C++ cells as unknown before execution.");
        Check(report["notApplicableCases"]!.AsArray().Any(item => (string?)item!["id"] == caseId
            && ((string?)item["reason"])!.Contains("managed peer pairing scope", StringComparison.Ordinal)
            && ((string?)item["reason"])!.Contains("native peer-scope RTI C++ / Wireloom C++ exchanges are included", StringComparison.Ordinal)),
            "Native peer-scope exclusions must explain the managed peer pairing scope.");
        Check(markdown.Contains("managed peer pairing scope", StringComparison.Ordinal)
            && markdown.Contains("native peer-scope RTI C++ / Wireloom C++ exchanges are included", StringComparison.Ordinal)
            && markdown.Contains("case(s) with peer exclusions", StringComparison.Ordinal)
            && markdown.Contains("The listed peer scopes are N/A and excluded from exchange counts.", StringComparison.Ordinal),
            "Native peer-scope markdown must describe the selected C++ scope and excluded managed peer cells.");

        // Arrange / Act / Assert: an ordinary C++ peer selection does not authorize a profile-excluded case.
        CheckRejected<InvalidDataException>(() => WireReportRenderer.Render(nativeRequest with { NativePeerScopeCases = null }),
            "A profile-excluded case must require explicit native peer-scope selection.");

        // Arrange / Act / Assert: a native peer-scope report must retain both directions for every fixture.
        File.WriteAllLines(nativeRequest.ScenarioPath, rows.Where(row => row.Writer == PeerNames.RtiCpp).Select(row => row.ToTsv()));
        CheckRejected<InvalidDataException>(() => WireReportRenderer.Render(nativeRequest with { ExpectedScenarioCount = fixtures.Count }),
            "Native peer-scope report finalization must reject a missing directed C++ pairing.");

        // Arrange / Act / Assert: managed and same-peer C++ exchanges cannot enter a native peer-scope report.
        var invalidRows = rows.ToList();
        invalidRows.Add(new ScenarioRow(caseId, "CorpusOptionalCollections.Sample", "", "::CorpusOptionalCollections::Sample",
            PeerNames.RtiCpp, "6.1.2", PeerNames.RtiCpp, "6.1.2", "NOT_RUN", fixtures[0]));
        File.WriteAllLines(nativeRequest.ScenarioPath, invalidRows.Select(row => row.ToTsv()));
        CheckRejected<InvalidDataException>(() => WireReportRenderer.Render(nativeRequest with { ExpectedScenarioCount = invalidRows.Count }),
            "Native peer-scope report finalization must reject pairings outside RTI C++ / Wireloom C++.");
    }

    private static ScenarioRow Row(string version, string writer, string reader, string status) =>
        new("01-boundaries", "Example.Sample", "Example.Sample", "::Example::Sample", writer, version, reader, version, status, "non-default");

    private static void CheckWriteRejected(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return;
        }

        throw new InvalidOperationException("The blocked Markdown destination must reject the write.");
    }

    private static void CheckRejected<TException>(Action action, string message) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
