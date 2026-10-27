using System.Diagnostics;
using System.Globalization;

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
        var tasks = scenarios.Select(async scenario =>
        {
            var result = await RunWithLeaseAsync(scenario);
            var completed = Interlocked.Increment(ref completedExchanges);
            Console.WriteLine($"WIRE_EXCHANGE_PROGRESS completed={completed}/{scenarios.Length}");
            return result;
        }).ToArray();
        return await Task.WhenAll(tasks);
    }

    private async Task<ExchangeResult> RunWithLeaseAsync(ScenarioRow scenario)
    {
        await concurrency.WaitAsync();
        if (!availableDomains.TryTake(out var domainId))
        {
            concurrency.Release();
            throw new InvalidOperationException("The DDS domain pool is exhausted while a scheduler slot is available.");
        }

        try
        {
            return await RunExchangeAsync(scenario, domainId);
        }
        finally
        {
            availableDomains.Add(domainId);
            concurrency.Release();
        }
    }

    private async Task<ExchangeResult> RunExchangeAsync(ScenarioRow scenario, int domainId)
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
            await File.WriteAllTextAsync(resultLog, $"FAIL_DETAIL {unavailable}{Environment.NewLine}");
            return new ExchangeResult("FAIL");
        }

        RunningPeer? reader = null;
        RunningPeer? writer = null;
        var failure = false;
        try
        {
            reader = StartPeer(caseBuild, scenario.Reader, "reader", scenario, topic, domainId, wireloomType, rtiType, readerLog);
            if (!await WaitForReaderReadyAsync(reader, TimeSpan.FromSeconds((int)MatrixOptions.ExchangeTimeout.TotalSeconds)))
            {
                failure = true;
                await KillIfRunningAsync(reader.Process);
                await File.WriteAllTextAsync(writerLog, $"FAIL ReaderReadiness Reader did not become ready for {scenario.CaseId} fixture {fixture}{Environment.NewLine}");
            }
            else
            {
                writer = StartPeer(caseBuild, scenario.Writer, "writer", scenario, topic, domainId, wireloomType, rtiType, writerLog);
                var completed = await WaitForEndpointsAsync(writer.Process, reader.Process, TimeSpan.FromSeconds((int)MatrixOptions.ExchangeTimeout.TotalSeconds + 5));
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
        }
        catch (Exception exception)
        {
            failure = true;
            await KillIfRunningAsync(writer?.Process);
            await KillIfRunningAsync(reader?.Process);
            await FinishPeerAsync(writer);
            await FinishPeerAsync(reader);
            await File.AppendAllTextAsync(resultLog, $"DDS_DOMAIN domain_id={domainId} topic={topic}{Environment.NewLine}");
            await File.AppendAllTextAsync(resultLog, $"FAIL_DETAIL {exception.Message.Replace(Environment.NewLine, " ", StringComparison.Ordinal)}{Environment.NewLine}");
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
        var writer = new StreamWriter(logPath, append: false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        var writerLock = new SemaphoreSlim(1, 1);
        async Task PumpAsync(StreamReader source)
        {
            while (await source.ReadLineAsync() is { } line)
            {
                output.Enqueue(line);
                if (line.StartsWith("READY ", StringComparison.Ordinal))
                {
                    ready.TrySetResult();
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

        return new RunningPeer(process, output, ready.Task, PumpAsync(process.StandardOutput), PumpAsync(process.StandardError), writer, writerLock);
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

    private static async Task<int> FinishPeerAsync(RunningPeer? peer)
    {
        if (peer is null)
        {
            return 0;
        }

        if (!peer.Process.HasExited)
        {
            await KillIfRunningAsync(peer.Process);
        }

        await peer.Process.WaitForExitAsync();
        await Task.WhenAll(peer.StandardOutput, peer.StandardError);
        await peer.LogWriter.DisposeAsync();
        peer.LogLock.Dispose();
        return peer.Process.ExitCode;
    }

    private static async Task KillIfRunningAsync(Process? process)
    {
        if (process is { HasExited: false })
        {
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
        }
    }

    private static async Task WriteSanitizedLogAsync(string resultLog, string writerLog, string readerLog,
        string writerId, int writerExit, string readerId, int readerExit, int domainId, string topic)
    {
        var statusPrefixes = new[]
        {
            "READY", "PASS", "RECEIVED", "SENT", "FAIL", "FAIL_DETAIL", "WRITER_MATCHED",
            "OPTIONAL_FIXTURE", "READER_STATS", "EXPECTED_SAMPLE", "ACTUAL_SAMPLE",
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
                    await output.WriteLineAsync(line);
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

    private static string PeerId(string peer) => peer switch
    {
        "Wireloom C#" => "wireloom-cs",
        "RTI C#" => "rti-cs",
        "RTI C++" => "rti-cpp",
        _ => throw new InvalidDataException($"Unknown wire peer '{peer}'."),
    };
}
