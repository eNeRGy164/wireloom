using System.Diagnostics;

using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Executes planned exchanges through the in-process bounded, domain-isolated scheduler.</summary>
internal sealed class ExchangeRunner(MatrixOptions options, MatrixWorkspace workspace, WorkflowLog log)
{
    /// <summary>Runs prepared cases and returns whether any exchange failed.</summary>
    internal async Task<bool> RunAsync(MatrixPlan plan, PeerPreparationResult peers)
    {
        if (options.CompileOnly || peers.BuiltCases.Count == 0)
        {
            return false;
        }

        log.Phase("exchange-run", $"built-cases={peers.BuiltCases.Count} planned-exchanges={plan.ExpectedScenarioCount}");
        var caseBuilds = peers.BuiltCases.ToDictionary(i => i.Case.Descriptor.CaseId, ExchangeCaseBuild.From, StringComparer.Ordinal);
        var scenarios = File.ReadLines(workspace.Scenarios)
            .Where(line => line.Length > 0)
            .Select(ScenarioRow.Parse)
            .Where(row => row.Status == "NOT_RUN" && caseBuilds.ContainsKey(row.CaseId))
            .ToArray();

        if (scenarios.Length == 0)
        {
            Console.WriteLine("No pending exchanges for built cases.");
            return false;
        }

        Directory.CreateDirectory(options.OutputDirectory);
        Directory.CreateDirectory(options.ScratchDirectory);

        var scheduler = new ExchangeScheduler(options, caseBuilds);

        var results = await scheduler.RunAsync(scenarios);

        UpdateScenarioFile(caseBuilds, scenarios, results);

        var failures = results.Count(result => result.Status == "FAIL");

        Console.WriteLine($"WIRE_EXCHANGE_MATRIX planned={results.Length} passed={results.Length - failures} failed={failures} max_concurrency={options.ExchangeConcurrency}");

        return failures > 0;
    }

    private void UpdateScenarioFile(Dictionary<string, ExchangeCaseBuild> caseBuilds, ScenarioRow[] selectedRows, ExchangeResult[] results)
    {
        var remaining = File.ReadLines(workspace.Scenarios)
            .Where(line => line.Length > 0)
            .Select(ScenarioRow.Parse)
            .ToArray();

        foreach (var (row, result) in selectedRows.Zip(results))
        {
            var index = Array.FindIndex(remaining, candidate => ScenarioKey(candidate) == ScenarioKey(row));
            if (index < 0 || remaining[index].Status != "NOT_RUN")
            {
                throw new InvalidDataException($"The exchange plan changed while running {row.CaseId} [{row.Fixture}] {row.Writer} -> {row.Reader}.");
            }

            var build = caseBuilds[row.CaseId];
            remaining[index] = row with
            {
                WireloomType = build.WireloomType,
                RtiCSharpType = build.RtiType,
                RtiCppType = build.CppType,
                Status = result.Status,
            };
        }

        var temporary = $"{workspace.Scenarios}.updated";

        File.WriteAllLines(temporary, remaining.Select(r => r.ToTsv()));
        File.Move(temporary, workspace.Scenarios, overwrite: true);
    }

    private static string ScenarioKey(ScenarioRow row) => string.Join('\t', row.CaseId, row.Fixture, row.Writer, row.Reader);
}

/// <summary>Peer artifacts needed to launch one corpus case in an exchange.</summary>
/// <param name="WireloomType">Generated managed type.</param>
/// <param name="RtiType">RTI managed control type.</param>
/// <param name="CppType">RTI native control type.</param>
/// <param name="CaseDirectory">Isolated case build directory.</param>
/// <param name="WireloomPeer">Wireloom managed peer assembly.</param>
/// <param name="RtiPeer">RTI managed peer assembly.</param>
/// <param name="CppPeer">RTI native peer executable.</param>
/// <param name="WireloomAvailable">Whether the Wireloom managed peer built.</param>
/// <param name="WireloomFailure">Wireloom peer build diagnostic.</param>
/// <param name="RtiAvailable">Whether the RTI managed peer built.</param>
/// <param name="RtiFailure">RTI peer build diagnostic.</param>
internal sealed record ExchangeCaseBuild(string WireloomType, string RtiType, string CppType,
    string CaseDirectory, string WireloomPeer, string RtiPeer, string CppPeer,
    bool WireloomAvailable, string WireloomFailure, bool RtiAvailable, string RtiFailure)
{
    internal static ExchangeCaseBuild From(BuiltCase built)
    {
        var item = built.Case;

        return new ExchangeCaseBuild(item.Wireloom.TypeName, item.Rti.TypeName, item.Descriptor.CppType,
            item.Paths.Directory, item.Wireloom.Path, item.Rti.Path, built.NativePeer,
            item.Wireloom.Available, item.Wireloom.Failure, item.Rti.Available, item.Rti.Failure);
    }
}

/// <summary>Outcome of one scheduled peer exchange.</summary>
/// <param name="Status">PASS or FAIL status.</param>
internal sealed record ExchangeResult(string Status);

/// <summary>Started peer process and its captured output and log resources.</summary>
/// <param name="Process">Peer process.</param>
/// <param name="Output">Captured standard output and error lines.</param>
/// <param name="Ready">Signal completed when the peer reports readiness.</param>
/// <param name="StandardOutput">Output pump task.</param>
/// <param name="StandardError">Error pump task.</param>
/// <param name="LogWriter">Per-peer log writer.</param>
/// <param name="LogLock">Synchronization for concurrent output streams.</param>
internal sealed record RunningPeer(Process Process, System.Collections.Concurrent.ConcurrentQueue<string> Output, Task Ready,
    Task StandardOutput, Task StandardError, StreamWriter LogWriter, SemaphoreSlim LogLock);
