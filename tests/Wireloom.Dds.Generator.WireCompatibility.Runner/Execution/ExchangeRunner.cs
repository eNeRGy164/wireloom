using System.Diagnostics;
using System.Threading.Channels;

using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Executes planned exchanges through the in-process bounded, domain-isolated scheduler.</summary>
internal sealed class ExchangeRunner(MatrixOptions options, MatrixWorkspace workspace, WorkflowLog log)
{
    /// <summary>Runs prepared cases and returns whether any exchange failed.</summary>
    internal async Task<bool> RunAsync(MatrixPlan plan, PeerPreparationResult peers)
    {
        if (options.CompileOnly)
        {
            return false;
        }

        log.Phase("exchange-run", $"built-cases={peers.BuiltCases.Count} planned-exchanges={plan.ExpectedScenarioCount}");
        var caseBuilds = peers.BuiltCases.ToDictionary(i => i.Case.Descriptor.CaseId, ExchangeCaseBuild.From, StringComparer.Ordinal);
        var pendingScenarios = File.ReadLines(workspace.Scenarios)
            .Where(line => line.Length > 0)
            .Select(ScenarioRow.Parse)
            .Where(row => row.Status == "NOT_RUN")
            .ToArray();

        if (pendingScenarios.Length == 0)
        {
            Console.WriteLine("No pending exchanges for built cases.");
            return false;
        }

        Directory.CreateDirectory(options.OutputDirectory);
        Directory.CreateDirectory(options.ScratchDirectory);

        var schedulable = pendingScenarios.Where(row => caseBuilds.ContainsKey(row.CaseId)).ToArray();
        var unprepared = pendingScenarios.Except(schedulable).ToArray();
        var completed = new List<(ScenarioRow Row, ExchangeResult Result)>(pendingScenarios.Length);

        if (schedulable.Length > 0)
        {
            var scheduler = new ExchangeScheduler(options, caseBuilds);
            var results = await scheduler.RunAsync(schedulable);
            completed.AddRange(schedulable.Zip(results));
        }

        foreach (var scenario in unprepared)
        {
            var diagnostic = PreparationFailure(scenario, peers.NativeOutcomes);
            WritePreparationFailureLog(scenario, diagnostic);
            completed.Add((scenario, new ExchangeResult("FAIL")));
        }

        UpdateScenarioFile(caseBuilds, completed);

        var failures = completed.Count(item => item.Result.Status == "FAIL");
        var notRun = completed.Count(item => item.Result.Status == "NOT_RUN");
        var passed = completed.Count - failures - notRun;
        Console.WriteLine($"WIRE_EXCHANGE_MATRIX planned={pendingScenarios.Length} passed={passed} failed={failures} not_run={notRun} unprepared={unprepared.Length} max_concurrency={options.ExchangeConcurrency}");

        return failures > 0 || notRun > 0;
    }

    private void UpdateScenarioFile(Dictionary<string, ExchangeCaseBuild> caseBuilds,
        IReadOnlyList<(ScenarioRow Row, ExchangeResult Result)> completed)
    {
        var remaining = File.ReadLines(workspace.Scenarios)
            .Where(line => line.Length > 0)
            .Select(ScenarioRow.Parse)
            .ToArray();

        foreach (var (row, result) in completed)
        {
            var index = Array.FindIndex(remaining, candidate => ScenarioKey(candidate) == ScenarioKey(row));
            if (index < 0 || remaining[index].Status != "NOT_RUN")
            {
                throw new InvalidDataException($"The exchange plan changed while running {row.CaseId} [{row.Fixture}] {row.Writer} -> {row.Reader}.");
            }

            var updated = row with { Status = result.Status };
            if (caseBuilds.TryGetValue(row.CaseId, out var build))
            {
                updated = updated with
                {
                    WireloomType = build.WireloomType,
                    RtiCSharpType = build.RtiType,
                    RtiCppType = build.CppType,
                };
            }

            remaining[index] = updated;
        }

        var temporary = $"{workspace.Scenarios}.updated";

        File.WriteAllLines(temporary, remaining.Select(r => r.ToTsv()));
        File.Move(temporary, workspace.Scenarios, overwrite: true);
    }

    private void WritePreparationFailureLog(ScenarioRow scenario, string diagnostic)
    {
        var resultLog = Path.Combine(options.OutputDirectory, "logs",
            $"{scenario.CaseId}-{scenario.Fixture}-{PeerId(scenario.Writer)}-{PeerId(scenario.Reader)}.log");
        Directory.CreateDirectory(Path.GetDirectoryName(resultLog)!);
        File.WriteAllText(resultLog,
            $"FAIL_DETAIL {diagnostic}{Environment.NewLine}EXCHANGE_TELEMETRY case={scenario.CaseId} fixture={scenario.Fixture} reader_started=false reader_ready=false writer_started=false endpoints_completed=false elapsed_ms=0{Environment.NewLine}");
    }

    private static string PreparationFailure(ScenarioRow scenario, IReadOnlyList<BuildOutcome> outcomes)
    {
        var caseOutcomes = outcomes.Where(outcome => outcome.CaseId == scenario.CaseId
            || outcome.CaseId == "wireloom-cpp:" + scenario.CaseId).ToArray();
        if (caseOutcomes.Length == 0)
        {
            return $"Peer preparation did not produce a runnable case for {scenario.CaseId}; no matching native build outcome was retained.";
        }

        var details = caseOutcomes.Select(outcome =>
            $"{outcome.CaseId} {outcome.Status}: {outcome.Detail}").Distinct(StringComparer.Ordinal);
        return $"Peer preparation did not produce a runnable case for {scenario.CaseId}. {string.Join("; ", details)}";
    }

    private static string ScenarioKey(ScenarioRow row) => string.Join('\t', row.CaseId, row.Fixture, row.Writer, row.Reader);

    private static string PeerId(string peer) => peer switch
    {
        PeerNames.WireloomCSharp => "wireloom-cs",
        PeerNames.RtiCSharp => "rti-cs",
        PeerNames.RtiCpp => "rti-cpp",
        PeerNames.WireloomCpp => "wireloom-cpp",
        _ => throw new InvalidDataException($"Unknown wire peer '{peer}'."),
    };
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
    string CaseDirectory, string WireloomPeer, string RtiPeer, string CppPeer, bool CppAvailable, string CppFailure, string? WireloomCppPeer,
    bool WireloomAvailable, string WireloomFailure, bool RtiAvailable, string RtiFailure,
    string WireloomCppFailure)
{
    internal static ExchangeCaseBuild From(BuiltCase built)
    {
        var item = built.Case;

        return new ExchangeCaseBuild(item.Wireloom.TypeName, item.Rti.TypeName, item.Descriptor.CppType,
            item.Paths.Directory, item.Wireloom.Path, item.Rti.Path, built.NativePeer, built.NativePeerAvailable,
            built.NativePeerFailure, built.WireloomCppPeer,
            item.Wireloom.Available, item.Wireloom.Failure, item.Rti.Available, item.Rti.Failure,
            item.WireloomCppFailure);
    }
}

/// <summary>Outcome of one scheduled peer exchange.</summary>
/// <param name="Status">PASS or FAIL status.</param>
internal sealed record ExchangeResult(string Status);

/// <summary>Lifecycle coordination result retained even when a peer exits before completing the exchange.</summary>
internal sealed record LifecycleCoordinationResult(bool Completed, int AcceptedPhases, string Outcome, bool ReaderOutputDrained);

/// <summary>Started peer process and its captured output and log resources.</summary>
/// <param name="Process">Peer process.</param>
/// <param name="Output">Captured standard output and error lines.</param>
/// <param name="Ready">Signal completed when the peer reports readiness.</param>
/// <param name="StandardOutput">Output pump task.</param>
/// <param name="StandardError">Error pump task.</param>
/// <param name="LogWriter">Per-peer log writer.</param>
/// <param name="LogLock">Synchronization for concurrent output streams.</param>
internal sealed record RunningPeer(Process Process, System.Collections.Concurrent.ConcurrentQueue<string> Output, Task Ready,
    Task StandardOutput, Task StandardError, StreamWriter LogWriter, SemaphoreSlim LogLock,
    ChannelReader<string> LifecycleEvents, StreamWriter? StandardInput);
