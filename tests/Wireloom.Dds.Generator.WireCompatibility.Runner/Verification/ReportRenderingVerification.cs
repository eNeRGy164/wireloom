using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks in-process report artifacts, evidence guards, and finalization failures without a helper DLL.</summary>
internal static class ReportRenderingVerification
{
    /// <summary>Renders both profiles and exercises recovery and partial writes before utilities are built.</summary>
    internal static void Run(MatrixOptions options)
    {
        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            VerifyProfile(options, version);
        }

        Console.WriteLine("Report rendering passed: both profiles, JSON/Markdown, log details, provenance guards, helper-free recovery, malformed rows, and partial artifact writes.");
    }

    private static void VerifyProfile(MatrixOptions options, string version)
    {
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

        var rows = new[]
        {
            Row(version, "Wireloom C#", "RTI C#", "PASS"),
            Row(version, "RTI C#", "Wireloom C#", "FAIL"),
            Row(version, "RTI C++", "RTI C++", "NOT_RUN"),
        };

        File.WriteAllLines(workspace.Scenarios, rows.Select(row => row.ToTsv()));
        File.WriteAllText(Path.Combine(workspace.Logs, "01-primitives-default-rti-cs-wireloom-cs.log"),
            """
            FAIL_DETAIL Expected 10; observed 20.
            ENDPOINT_STATUS writer_exit_code=139 reader_exit_code=0
            """);
        File.WriteAllText(Path.Combine(workspace.Logs, "01-primitives-default-rti-cpp-rti-cpp.log"), "NOT_RUN_DETAIL Native peer was unavailable.\n");

        var request = new ReportRequest(version, rows.Length, workspace.Scenarios, workspace.Output("results.json"),
            workspace.Output("results.md"), workspace.Logs, workspace.Exclusions, workspace.Manifest, options.RepositoryRoot);

        // Act
        WireReportRenderer.Render(request);
        var document = JsonNode.Parse(File.ReadAllText(request.JsonPath))!;
        var markdown = File.ReadAllText(request.MarkdownPath);

        // Assert
        Check((string?)document["rtiVersion"] == version && (int)document["scenarioCount"]! == 3 && (int)document["fixtureCount"]! == 1, "Report identity and counts must retain scheduler evidence.");

        var scenarios = document["scenarios"]!.AsArray();
        Check(((string?)scenarios[1]!["observedProblem"])!.Contains("Expected 10; observed 20", StringComparison.Ordinal) &&
            ((string?)scenarios[1]!["observedProblem"])!.Contains("SIGSEGV", StringComparison.Ordinal), "Observed failures must retain detail and endpoint exit interpretation.");
        Check((string?)scenarios[2]!["observedProblem"] == "Native peer was unavailable.", "NOT_RUN_DETAIL must retain precedence.");
        Check(markdown.Contains($"# {version} Wire compatibility results", StringComparison.Ordinal) && markdown.Contains("**3** DDS exchanges", StringComparison.Ordinal), "Markdown must describe the same report.");

        // Arrange / Act / Assert: fatal setup/execution recovery must still render without tools.
        var finalizer = new MatrixReport(reportOptions, workspace, new WorkflowLog());
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
    }

    private static ScenarioRow Row(string version, string writer, string reader, string status) =>
        new("01-primitives", "Example.Sample", "Example.Sample", "::Example::Sample", writer, version, reader, version, status, "default");

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
