using System.Diagnostics;
using System.Globalization;
using System.Threading.Channels;

using LifecyclePhaseProtocol = Wireloom.Dds.Generator.WireCompatibility.Shared.LifecyclePhaseProtocol;
using ScenarioRow = Wireloom.Dds.Generator.WireCompatibility.Shared.ScenarioRow;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Runs pending directed exchanges with bounded concurrency and isolated DDS domains.</summary>
internal sealed class ExchangeScheduler(MatrixOptions options, IReadOnlyDictionary<string, ExchangeCaseBuild> caseBuilds)
{
    private const int FirstDomainId = 20;
    private readonly SemaphoreSlim concurrency = new(options.ExchangeConcurrency, options.ExchangeConcurrency);
    private readonly System.Collections.Concurrent.ConcurrentBag<int> availableDomains = new(Enumerable.Range(FirstDomainId, options.ExchangeConcurrency));
    private int completedExchanges;

    internal async Task<ExchangeResult[]> RunAsync(ScenarioRow[] scenarios)
    {
        using var cancellation = new ConsoleCancellationScope();

        var tasks = scenarios.Select(async scenario =>
        {
            var result = await RunWithLeaseAsync(scenario, cancellation.Token);
            var completed = Interlocked.Increment(ref completedExchanges);
            Console.WriteLine($"WIRE_EXCHANGE_PROGRESS completed={completed}/{scenarios.Length}");
            return result;
        }).ToArray();

        return await Task.WhenAll(tasks);
    }

    private async Task<ExchangeResult> RunWithLeaseAsync(ScenarioRow scenario, CancellationToken cancellationToken)
    {
        await concurrency.WaitAsync(cancellationToken);
        if (!availableDomains.TryTake(out var domainId))
        {
            concurrency.Release();
            throw new InvalidOperationException("The DDS domain pool is exhausted while a scheduler slot is available.");
        }

        try
        {
            return await RunExchangeAsync(scenario, domainId, cancellationToken);
        }
        finally
        {
            availableDomains.Add(domainId);
            concurrency.Release();
        }
    }

    private async Task<ExchangeResult> RunExchangeAsync(ScenarioRow scenario, int domainId, CancellationToken cancellationToken)
    {
        var caseBuild = caseBuilds[scenario.CaseId];
        var writerId = PeerId(scenario.Writer);
        var readerId = PeerId(scenario.Reader);
        var label = $"{scenario.CaseId}-{scenario.Fixture}-{writerId}-{readerId}";
        var topic = $"wl_{scenario.CaseId.Replace('-', '_')}_{scenario.Fixture.Replace('-', '_')}_{writerId}_{readerId}_{Guid.NewGuid():N}";
        var endpointDirectory = Path.Combine(options.ScratchDirectory, "endpoints", label);
        Directory.CreateDirectory(endpointDirectory);
        var readerLog = Path.Combine(endpointDirectory, "reader.log");
        var writerLog = Path.Combine(endpointDirectory, "writer.log");
        var resultLog = Path.Combine(options.OutputDirectory, "logs", label + ".log");
        Directory.CreateDirectory(Path.GetDirectoryName(resultLog)!);

        var fixture = scenario.Fixture;
        var wireloomType = caseBuild.WireloomType;
        var rtiType = caseBuild.RtiType;
        if (scenario.CaseId == "06-aggregates" && fixture == "direct-recursive-sequence")
        {
            wireloomType = "CorpusAggregates.Recursive";
            rtiType = "CorpusAggregates.Recursive";
        }

        var unavailable = MissingPeerReason(caseBuild, writerId, readerId);
        if (unavailable is not null)
        {
            await File.WriteAllTextAsync(resultLog, $"FAIL_DETAIL {unavailable}{Environment.NewLine}", cancellationToken);
            return new ExchangeResult("FAIL");
        }

        RunningPeer? reader = null;
        RunningPeer? writer = null;
        LifecycleCoordinationResult? lifecycleOutcome = null;
        var failure = false;
        try
        {
            reader = StartPeer(caseBuild, scenario.Reader, "reader", scenario, topic, domainId, wireloomType, rtiType, readerLog);
            if (!await WaitForReaderReadyAsync(reader, TimeSpan.FromSeconds((int)MatrixOptions.ExchangeTimeout.TotalSeconds)))
            {
                failure = true;
                await KillIfRunningAsync(reader.Process);
                await File.WriteAllTextAsync(writerLog, $"FAIL ReaderReadiness Reader did not become ready for {scenario.CaseId} fixture {fixture}{Environment.NewLine}", cancellationToken);
            }
            else
            {
                writer = StartPeer(caseBuild, scenario.Writer, "writer", scenario, topic, domainId, wireloomType, rtiType, writerLog);
                bool completed;
                if (IsLifecycleFixture(scenario))
                {
                    lifecycleOutcome = await RunLifecycleExchangeAsync(reader, writer, topic, cancellationToken);
                    completed = lifecycleOutcome.Completed;
                }
                else
                {
                    completed = await WaitForEndpointsAsync(writer.Process, reader.Process,
                        TimeSpan.FromSeconds((int)MatrixOptions.ExchangeTimeout.TotalSeconds + 5));
                }
                if (!completed)
                {
                    failure = true;
                    await KillIfRunningAsync(writer.Process);
                    await KillIfRunningAsync(reader.Process);
                }
            }

            var writerExit = await FinishPeerAsync(writer);
            var readerExit = await FinishPeerAsync(reader);
            var writerPassed = EndpointPassed(writerId, "writer", scenario, writer?.Output);
            var readerPassed = EndpointPassed(readerId, "reader", scenario, reader.Output);
            failure |= writerExit != 0 || readerExit != 0 || !writerPassed || !readerPassed;
            await WriteSanitizedLogAsync(resultLog, writerLog, readerLog, writerId, writerExit, readerId, readerExit, domainId, topic);
            if (lifecycleOutcome is not null)
            {
                await File.AppendAllTextAsync(resultLog,
                    $"LIFECYCLE_COORDINATION expected_phases={LifecyclePhaseProtocol.PhaseCount} accepted_phases={lifecycleOutcome.AcceptedPhases} completed={lifecycleOutcome.Completed.ToString().ToLowerInvariant()} reader_output_drained={lifecycleOutcome.ReaderOutputDrained.ToString().ToLowerInvariant()} outcome={lifecycleOutcome.Outcome.Replace(' ', '_')}{Environment.NewLine}",
                    CancellationToken.None);
            }
        }
        catch (Exception exception)
        {
            failure = true;
            await KillIfRunningAsync(writer?.Process);
            await KillIfRunningAsync(reader?.Process);
            await FinishPeerAsync(writer);
            await FinishPeerAsync(reader);
            await File.AppendAllTextAsync(resultLog, $"DDS_DOMAIN domain_id={domainId} topic={topic}{Environment.NewLine}", CancellationToken.None);
            await File.AppendAllTextAsync(resultLog,
                $"FAIL_DETAIL {exception.Message.Replace(Environment.NewLine, " ", StringComparison.Ordinal)}{Environment.NewLine}", CancellationToken.None);
        }

        var status = failure ? "FAIL" : "PASS";
        Console.WriteLine($"{status} {scenario.CaseId} [{fixture}] {scenario.Writer} -> {scenario.Reader} domain={domainId}");
        return new ExchangeResult(status);
    }

    private static string? MissingPeerReason(ExchangeCaseBuild build, string writerId, string readerId)
    {
        if ((writerId == "rti-cs" || readerId == "rti-cs") && !build.RtiAvailable)
        {
            return build.RtiFailure;
        }

        if ((writerId == "wireloom-cs" || readerId == "wireloom-cs") && !build.WireloomAvailable)
        {
            return build.WireloomFailure;
        }

        return null;
    }

    private static bool EndpointPassed(string peerId, string role, ScenarioRow scenario, IEnumerable<string>? output)
    {
        if (output is null)
        {
            return false;
        }

        if (peerId == "rti-cpp" && scenario is not { CaseId: "06-aggregates", Fixture: "direct-recursive-sequence" })
        {
            var successLine = role == "writer" ? "SENT" : "RECEIVED";
            return output.Contains(successLine, StringComparer.Ordinal);
        }

        var expectedRole = role == "writer" ? "Writer" : "Reader";
        var passStatusLine = $"PASS {scenario.CaseId} {expectedRole}";
        return output.Any(line => line.Equals(passStatusLine, StringComparison.Ordinal)
            || line.StartsWith(passStatusLine + " ", StringComparison.Ordinal));
    }

    private RunningPeer StartPeer(ExchangeCaseBuild caseBuild, string peer, string role, ScenarioRow scenario,
        string topic, int domainId, string wireloomType, string rtiType, string logPath)
    {
        var peerId = PeerId(peer);
        var recursiveFixture = scenario is { CaseId: "06-aggregates", Fixture: "direct-recursive-sequence" };
        var lifecycleFixture = IsLifecycleFixture(scenario);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = peerId switch
                {
                    "wireloom-cs" or "rti-cs" => "dotnet",
                    "rti-cpp" when recursiveFixture => Path.Combine(caseBuild.CaseDirectory, "native", "build", "wire-peer-recursive-sequence"),
                    "rti-cpp" => caseBuild.CppPeer,
                    _ => throw new InvalidOperationException($"Unknown peer {peer}."),
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = lifecycleFixture && role == "writer",
                UseShellExecute = false,
            },
            EnableRaisingEvents = true,
        };
        process.StartInfo.Environment["RTI_MONITORING2_ENABLE"] = "false";
        if (peerId == "wireloom-cs")
        {
            process.StartInfo.ArgumentList.Add(caseBuild.WireloomPeer);
            AddManagedArguments(process, scenario.CaseId, wireloomType, topic, role, scenario.Fixture, domainId);
        }
        else if (peerId == "rti-cs")
        {
            process.StartInfo.ArgumentList.Add(caseBuild.RtiPeer);
            AddManagedArguments(process, scenario.CaseId, rtiType, topic, role, scenario.Fixture, domainId);
        }
        else
        {
            process.StartInfo.ArgumentList.Add(role);
            process.StartInfo.ArgumentList.Add(topic);
            process.StartInfo.ArgumentList.Add(((int)MatrixOptions.ExchangeTimeout.TotalSeconds).ToString(CultureInfo.InvariantCulture));
            process.StartInfo.ArgumentList.Add(scenario.CaseId);
            process.StartInfo.ArgumentList.Add(scenario.Fixture);
            process.StartInfo.ArgumentList.Add(domainId.ToString(CultureInfo.InvariantCulture));
        }

        var output = new System.Collections.Concurrent.ConcurrentQueue<string>();
        var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lifecycleEvents = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });
        var writer = new StreamWriter(logPath, append: false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        var writerLock = new SemaphoreSlim(1, 1);

        async Task PumpAsync(StreamReader source, bool protocolStream)
        {
            while (await source.ReadLineAsync() is { } line)
            {
                output.Enqueue(line);
                if (line.StartsWith("READY ", StringComparison.Ordinal))
                {
                    ready.TrySetResult();
                }

                if (protocolStream && (line == "PHASE_VERIFIED" || line.StartsWith("PHASE_VERIFIED ", StringComparison.Ordinal)))
                {
                    await lifecycleEvents.Writer.WriteAsync(line);
                }

                await writerLock.WaitAsync();
                try
                {
                    await writer.WriteLineAsync(line);
                    await writer.FlushAsync();
                }
                finally
                {
                    writerLock.Release();
                }
            }
        }

        if (!process.Start())
        {
            writer.Dispose();
            throw new InvalidOperationException($"Could not start {role} process for {scenario.CaseId}.");
        }

        var standardOutput = PumpAsync(process.StandardOutput, protocolStream: true);
        var standardError = PumpAsync(process.StandardError, protocolStream: false);
        _ = Task.WhenAll(standardOutput, standardError).ContinueWith(
            _ => lifecycleEvents.Writer.TryComplete(),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

        return new RunningPeer(process, output, ready.Task, standardOutput, standardError, writer, writerLock,
            lifecycleEvents.Reader, process.StartInfo.RedirectStandardInput ? process.StandardInput : null);
    }

    private void AddManagedArguments(Process process, string caseId, string type, string topic, string role, string fixture, int domainId)
    {
        AddOption(process, "case", caseId);
        AddOption(process, "type", type);
        AddOption(process, "topic", topic);
        AddOption(process, "runtime-version", options.RtiVersion);
        AddOption(process, "role", role);
        AddOption(process, "fixture", fixture);
        AddOption(process, "domain-id", domainId.ToString(CultureInfo.InvariantCulture));
    }

    private static void AddOption(Process process, string name, string value)
    {
        process.StartInfo.ArgumentList.Add($"--{name}");
        process.StartInfo.ArgumentList.Add(value);
    }

    private async Task<bool> WaitForReaderReadyAsync(RunningPeer reader, TimeSpan timeout)
    {
        var deadline = Stopwatch.StartNew();
        while (deadline.Elapsed < timeout)
        {
            if (reader.Ready.IsCompleted)
            {
                return true;
            }

            if (reader.Process.HasExited)
            {
                return false;
            }

            await Task.Delay(50);
        }

        return false;
    }

    private static async Task<bool> WaitForEndpointsAsync(Process writer, Process reader, TimeSpan timeout)
    {
        using var cancellation = new CancellationTokenSource(timeout);
        try
        {
            await Task.WhenAll(writer.WaitForExitAsync(cancellation.Token), reader.WaitForExitAsync(cancellation.Token));
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    internal static async Task<LifecycleCoordinationResult> RunLifecycleExchangeAsync(RunningPeer reader, RunningPeer writer,
        string topic, CancellationToken cancellationToken, TimeSpan? phaseTimeoutOverride = null,
        TimeSpan? overallTimeoutOverride = null)
    {
        const int phaseCount = LifecyclePhaseProtocol.PhaseCount;
        var overallDeadline = Stopwatch.StartNew();
        var phaseTimeout = phaseTimeoutOverride ?? TimeSpan.FromSeconds(45);
        var overallTimeout = overallTimeoutOverride ?? TimeSpan.FromSeconds((int)MatrixOptions.ExchangeTimeout.TotalSeconds + 5);
        var protocol = new LifecyclePhaseProtocol(topic);
        var acceptedPhases = 0;
        var readerOutputDrained = false;

        for (var phase = 0; phase < phaseCount; phase++)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var remaining = overallTimeout - overallDeadline.Elapsed;
            if (remaining <= TimeSpan.Zero)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "overall-timeout", readerOutputDrained);
            }

            timeout.CancelAfter(phaseTimeout < remaining ? phaseTimeout : remaining);

            string line;
            var nextEvent = reader.LifecycleEvents.ReadAsync(timeout.Token).AsTask();

            try
            {
                var readerExit = reader.Process.WaitForExitAsync(timeout.Token);
                var writerExit = writer.Process.WaitForExitAsync(timeout.Token);
                var completed = await Task.WhenAny(nextEvent, readerExit, writerExit);

                if (completed == writerExit && writer.Process.HasExited)
                {
                    return new LifecycleCoordinationResult(false, acceptedPhases, "writer-exited-before-phase-acknowledgment", readerOutputDrained);
                }

                if (completed == nextEvent)
                {
                    line = await nextEvent;
                }
                else
                {
                    // Process exit can become observable before a delayed stdout pump publishes its final line.
                    // Drain both reader streams under the same phase deadline, then resolve the one outstanding read.
                    await Task.WhenAll(reader.StandardOutput, reader.StandardError).WaitAsync(timeout.Token);
                    readerOutputDrained = true;
                    line = await nextEvent.WaitAsync(timeout.Token);
                }
            }
            catch (TimeoutException)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "phase-timeout", readerOutputDrained);
            }
            catch (OperationCanceledException)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases,
                    cancellationToken.IsCancellationRequested ? "cancelled" : "phase-timeout", readerOutputDrained);
            }
            catch (ChannelClosedException)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "reader-event-stream-closed", readerOutputDrained);
            }
            catch (Exception exception) when (exception is IOException or InvalidDataException)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "reader-event-invalid: " + exception.Message, readerOutputDrained);
            }

            try
            {
                var verified = protocol.AcceptVerifiedEvent(line);
                if (verified.PhaseId != phase)
                {
                    return new LifecycleCoordinationResult(false, acceptedPhases, "reader-phase-out-of-order", readerOutputDrained);
                }
            }
            catch (InvalidDataException exception)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "reader-event-invalid: " + exception.Message, readerOutputDrained);
            }

            acceptedPhases++;

            if (writer.StandardInput is null)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "writer-stdin-not-redirected", readerOutputDrained);
            }

            try
            {
                await writer.StandardInput.WriteLineAsync(protocol.CreateAcknowledgment(phase).AsMemory(), timeout.Token);
                await writer.StandardInput.FlushAsync(timeout.Token);
            }
            catch (Exception exception) when (exception is IOException or OperationCanceledException or ObjectDisposedException)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "writer-acknowledgment-failed: " + exception.Message, readerOutputDrained);
            }
        }

        try
        {
            protocol.EnsureComplete();
            var remaining = overallTimeout - overallDeadline.Elapsed;
            if (remaining <= TimeSpan.Zero)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "overall-timeout", readerOutputDrained);
            }

            using var completionTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            completionTimeout.CancelAfter(remaining);
            var completion = Task.WhenAll(writer.Process.WaitForExitAsync(completionTimeout.Token),
                reader.Process.WaitForExitAsync(completionTimeout.Token), reader.StandardOutput, reader.StandardError,
                writer.StandardOutput, writer.StandardError);
            await completion.WaitAsync(completionTimeout.Token);
            if (overallDeadline.Elapsed > overallTimeout || cancellationToken.IsCancellationRequested)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, cancellationToken.IsCancellationRequested
                    ? "cancelled-after-final-phase" : "endpoint-timeout", readerOutputDrained);
            }
            readerOutputDrained = true;
        }
        catch (OperationCanceledException)
        {
            return new LifecycleCoordinationResult(false, acceptedPhases,
                cancellationToken.IsCancellationRequested ? "cancelled-after-final-phase" : "endpoint-timeout", readerOutputDrained);
        }
        catch (TimeoutException)
        {
            return new LifecycleCoordinationResult(false, acceptedPhases, "endpoint-timeout", readerOutputDrained);
        }
        catch (Exception exception) when (exception is IOException or InvalidOperationException)
        {
            return new LifecycleCoordinationResult(false, acceptedPhases, "endpoint-output-drain-failed: " + exception.Message, readerOutputDrained);
        }

        while (reader.LifecycleEvents.TryRead(out var trailingEvent))
        {
            try
            {
                _ = protocol.AcceptVerifiedEvent(trailingEvent);
                return new LifecycleCoordinationResult(false, acceptedPhases, "unexpected-trailing-lifecycle-event", readerOutputDrained);
            }
            catch (InvalidDataException exception)
            {
                return new LifecycleCoordinationResult(false, acceptedPhases, "invalid-trailing-lifecycle-event: " + exception.Message, readerOutputDrained);
            }
        }

        var publishedPhases = writer.Output
            .Where(line => line.StartsWith("PHASE_PUBLISHED ", StringComparison.Ordinal))
            .Select(ParsePublishedPhase)
            .ToArray();
        if (publishedPhases.Length != phaseCount
            || publishedPhases.Where((phase, index) => phase != index).Any())
        {
            return new LifecycleCoordinationResult(false, acceptedPhases, "writer-phase-publication-incomplete", readerOutputDrained);
        }

        if (!reader.LifecycleEvents.Completion.IsCompleted)
        {
            return new LifecycleCoordinationResult(false, acceptedPhases, "reader-event-stream-not-complete-after-process-exit", readerOutputDrained);
        }

        return new LifecycleCoordinationResult(true, acceptedPhases, "complete", readerOutputDrained);

        static int ParsePublishedPhase(string line)
        {
            var fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return fields.Length >= 3
                && int.TryParse(fields[2], NumberStyles.None, CultureInfo.InvariantCulture, out var phase)
                ? phase
                : -1;
        }
    }

    private static bool IsLifecycleFixture(ScenarioRow scenario) =>
        scenario.CaseId == "14-unbounded-receive-lifecycle";

    internal static async Task<int> FinishPeerAsync(RunningPeer? peer)
    {
        if (peer is null)
        {
            return 0;
        }

        if (!peer.Process.HasExited)
        {
            await KillIfRunningAsync(peer.Process);
        }

        await ObserveWithinBoundAsync(peer.Process.WaitForExitAsync(), TimeSpan.FromSeconds(2));

        if (peer.StandardInput is not null)
        {
            await DisposeWithinBoundAsync(peer.StandardInput, TimeSpan.FromSeconds(1));
        }

        var pumps = Task.WhenAll(peer.StandardOutput, peer.StandardError);
        if (!await ObserveWithinBoundAsync(pumps, TimeSpan.FromSeconds(2)))
        {
            // A descendant may retain an inherited pipe after the peer exits. Close our
            // read ends to release the pumps, then observe any disposal exceptions.
            peer.Process.StandardOutput.Dispose();
            peer.Process.StandardError.Dispose();
            await ObserveWithinBoundAsync(pumps, TimeSpan.FromSeconds(1));
        }

        if (pumps.IsCompleted)
        {
            await ObserveTaskFailureAsync(pumps);
            await DisposeWithinBoundAsync(peer.LogWriter, TimeSpan.FromSeconds(1));
            peer.LogLock.Dispose();
        }
        else
        {
            _ = pumps.ContinueWith(completed => _ = completed.Exception,
                CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }

        var exitCode = peer.Process.HasExited ? peer.Process.ExitCode : -1;
        peer.Process.Dispose();
        return exitCode;
    }

    private static async Task KillIfRunningAsync(Process? process)
    {
        if (process is { HasExited: false })
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            await ObserveWithinBoundAsync(process.WaitForExitAsync(), TimeSpan.FromSeconds(2));
        }
    }

    private static async Task<bool> ObserveWithinBoundAsync(Task task, TimeSpan timeout)
    {
        try
        {
            await task.WaitAsync(timeout);
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
        catch (Exception)
        {
            return true;
        }
    }

    private static async Task ObserveTaskFailureAsync(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception)
        {
            // Pump failures are already represented in partial endpoint output and the
            // exchange result. Observing them here prevents unobserved task exceptions.
        }
    }

    private static async Task DisposeWithinBoundAsync(IAsyncDisposable resource, TimeSpan timeout)
    {
        try
        {
            await resource.DisposeAsync().AsTask().WaitAsync(timeout);
        }
        catch (Exception)
        {
            // Cleanup is best effort and bounded; primary exchange evidence is retained.
        }
    }

    private static async Task WriteSanitizedLogAsync(string resultLog, string writerLog, string readerLog,
        string writerId, int writerExit, string readerId, int readerExit, int domainId, string topic)
    {
        var statusPrefixes = new[]
        {
            "READY", "PASS", "RECEIVED", "SENT", "FAIL", "FAIL_DETAIL", "WRITER_MATCHED",
            "OPTIONAL_FIXTURE", "READER_STATS", "EXPECTED_SAMPLE", "ACTUAL_SAMPLE",
            "PHASE_VERIFIED", "PHASE_PUBLISHED", "LIFECYCLE_PHASE", "LIFECYCLE_COORDINATION", "LIFECYCLE_PARTICIPANT_QOS", "LIFECYCLE_READER_QOS",
            "LIFECYCLE_READER_FINAL", "LIFECYCLE_WRITER_QOS", "LIFECYCLE_NATIVE_CDR", "LIFECYCLE_RECEIVE_STORAGE", "FRAGMENT_EVIDENCE",
        };
        var diagnostics = new List<string>();
        await using var output = new StreamWriter(resultLog, append: false, new System.Text.UTF8Encoding(false));
        foreach (var path in new[] { writerLog, readerLog })
        {
            if (!File.Exists(path))
            {
                continue;
            }

            foreach (var line in await File.ReadAllLinesAsync(path))
            {
                if (statusPrefixes.Any(prefix => line.StartsWith(prefix + " ", StringComparison.Ordinal) || line == prefix))
                {
                    var endpoint = path == writerLog ? "writer" : "reader";
                    var needsEndpoint = !line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Any(field => field.StartsWith("endpoint=", StringComparison.Ordinal));
                    var retainedLine = needsEndpoint && (line.StartsWith("LIFECYCLE_PARTICIPANT_QOS ", StringComparison.Ordinal)
                        || line.StartsWith("LIFECYCLE_WRITER_QOS ", StringComparison.Ordinal)
                        || line.StartsWith("LIFECYCLE_READER_QOS ", StringComparison.Ordinal)
                        || line.StartsWith("LIFECYCLE_NATIVE_CDR ", StringComparison.Ordinal))
                            ? $"{line} endpoint={endpoint}"
                            : line;
                    await output.WriteLineAsync(retainedLine);
                }
                else if (line.Contains("not assignable", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("type consistency", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("typeobject", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("type object", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("incompatible", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("data representation", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("qos policy", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("representation", StringComparison.OrdinalIgnoreCase))
                {
                    diagnostics.Add("RTI_DIAGNOSTIC " + line);
                }
            }
        }

        foreach (var diagnostic in diagnostics.Take(100))
        {
            await output.WriteLineAsync(diagnostic);
        }

        await output.WriteLineAsync($"DDS_DOMAIN domain_id={domainId} topic={topic}");
        await output.WriteLineAsync($"ENDPOINT_STATUS writer_peer={writerId} writer_exit_code={writerExit} reader_peer={readerId} reader_exit_code={readerExit}");
    }

    private sealed class ConsoleCancellationScope : IDisposable
    {
        private readonly CancellationTokenSource source = new();
        private readonly ConsoleCancelEventHandler handler;

        internal ConsoleCancellationScope()
        {
            handler = CancelOnConsoleSignal;
            Console.CancelKeyPress += handler;
        }

        internal CancellationToken Token => source.Token;

        public void Dispose()
        {
            Console.CancelKeyPress -= handler;
            source.Dispose();
        }

        private void CancelOnConsoleSignal(object? sender, ConsoleCancelEventArgs eventArgs)
        {
            eventArgs.Cancel = true;
            source.Cancel();
        }
    }

    private static string PeerId(string peer) => peer switch
    {
        "Wireloom C#" => "wireloom-cs",
        "RTI C#" => "rti-cs",
        "RTI C++" => "rti-cpp",
        _ => throw new InvalidDataException($"Unknown wire peer '{peer}'."),
    };
}
