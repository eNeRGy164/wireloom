// Runs all preplanned exchanges with a bounded, domain-isolated scheduler.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;

const string usage = "Usage: wire-exchange-runner.cs --scenarios <file> --case-builds <file> --rti-version <version> --output-directory <dir> --scratch <dir> [--timeout-seconds 20] [--max-concurrency 16]";

try
{
    var options = Options.Parse(args);
    var caseBuilds = File.ReadAllLines(options.CaseBuildsPath)
        .Where(line => line.Length > 0)
        .Select(CaseBuild.Parse)
        .ToDictionary(build => build.CaseId, StringComparer.Ordinal);
    var rows = File.ReadAllLines(options.ScenariosPath)
        .Where(line => line.Length > 0)
        .Select(Scenario.Parse)
        .Where(row => row.Status == "NOT_RUN" && caseBuilds.ContainsKey(row.CaseId))
        .ToArray();
    if (rows.Length == 0)
    {
        Console.WriteLine("No pending exchanges for built cases.");
        return 0;
    }

    Directory.CreateDirectory(options.OutputDirectory);
    Directory.CreateDirectory(options.ScratchDirectory);
    var scheduler = new ExchangeScheduler(options, caseBuilds);
    var results = await scheduler.RunAsync(rows);
    UpdateScenarioFile(options, caseBuilds, rows, results);

    var failures = results.Count(result => result.Status == "FAIL");
    Console.WriteLine($"WIRE_EXCHANGE_MATRIX planned={results.Length} passed={results.Length - failures} failed={failures} max_concurrency={options.MaxConcurrency}");
    return failures == 0 ? 0 : 1;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception is ArgumentException ? usage : exception.Message);
    return 2;
}

static void UpdateScenarioFile(Options options, IReadOnlyDictionary<string, CaseBuild> caseBuilds,
    Scenario[] selectedRows, ExchangeResult[] results)
{
    var remaining = File.ReadAllLines(options.ScenariosPath).Where(line => line.Length > 0).Select(Scenario.Parse).ToArray();
    foreach (var (row, result) in selectedRows.Zip(results))
    {
        var index = Array.FindIndex(remaining, candidate => candidate.Key == row.Key);
        if (index < 0 || remaining[index].Status != "NOT_RUN")
        {
            throw new InvalidDataException($"The exchange plan changed while running {row.CaseId} [{row.Fixture}] {row.Writer} -> {row.Reader}.");
        }

        var build = caseBuilds[row.CaseId];
        remaining[index] = row with
        {
            WireloomType = build.WireloomType,
            RtiType = build.RtiType,
            CppType = build.CppType,
            Status = result.Status,
        };
    }

    var temporary = options.ScenariosPath + ".updated";
    File.WriteAllLines(temporary, remaining.Select(row => row.ToTsv()));
    File.Move(temporary, options.ScenariosPath, overwrite: true);
}

sealed class ExchangeScheduler(Options options, IReadOnlyDictionary<string, CaseBuild> caseBuilds)
{
    private const int FirstDomainId = 20;
    private readonly SemaphoreSlim concurrency = new(options.MaxConcurrency, options.MaxConcurrency);
    private readonly ConcurrentBag<int> availableDomains = new(Enumerable.Range(FirstDomainId, options.MaxConcurrency));
    private int completedExchanges;

    internal async Task<ExchangeResult[]> RunAsync(Scenario[] scenarios)
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

    private async Task<ExchangeResult> RunWithLeaseAsync(Scenario scenario)
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

    private async Task<ExchangeResult> RunExchangeAsync(Scenario scenario, int domainId)
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
        var cppType = caseBuild.CppType;
        if (scenario.CaseId == "06-aggregates" && fixture == "direct-recursive-sequence")
        {
            wireloomType = "CorpusAggregates.Recursive";
            rtiType = "CorpusAggregates.Recursive";
            cppType = "::CorpusAggregates::Recursive";
        }

        var unavailable = MissingPeerReason(caseBuild, writerId, readerId);
        if (unavailable is not null)
        {
            await File.WriteAllTextAsync(resultLog, $"FAIL_DETAIL {unavailable}{Environment.NewLine}");
            return new ExchangeResult(scenario, "FAIL");
        }

        RunningPeer? reader = null;
        RunningPeer? writer = null;
        var writerExit = -1;
        var readerExit = -1;
        var readerReady = false;
        var failure = false;
        try
        {
            reader = StartPeer(caseBuild, scenario.Reader, "reader", scenario, topic, domainId, wireloomType, rtiType, readerLog);
            readerReady = await WaitForReaderReadyAsync(reader, TimeSpan.FromSeconds(options.TimeoutSeconds));
            if (!readerReady)
            {
                failure = true;
                await KillIfRunningAsync(reader.Process);
                await File.WriteAllTextAsync(writerLog, $"FAIL ReaderReadiness Reader did not become ready for {scenario.CaseId} fixture {fixture}{Environment.NewLine}");
            }
            else
            {
                writer = StartPeer(caseBuild, scenario.Writer, "writer", scenario, topic, domainId, wireloomType, rtiType, writerLog);
                var completed = await WaitForEndpointsAsync(writer.Process, reader.Process, TimeSpan.FromSeconds(options.TimeoutSeconds + 5));
                if (!completed)
                {
                    failure = true;
                    await KillIfRunningAsync(writer.Process);
                    await KillIfRunningAsync(reader.Process);
                }
            }

            writerExit = await FinishPeerAsync(writer);
            readerExit = await FinishPeerAsync(reader);
            var writerPassed = EndpointPassed(writerId, "writer", scenario, writer?.Output);
            var readerPassed = EndpointPassed(readerId, "reader", scenario, reader?.Output);
            failure |= writerExit != 0 || readerExit != 0 || !writerPassed || !readerPassed;
            await WriteSanitizedLogAsync(resultLog, writerLog, readerLog, writerId, writerExit, readerId, readerExit, domainId, topic);
        }
        catch (Exception exception)
        {
            failure = true;
            await KillIfRunningAsync(writer?.Process);
            await KillIfRunningAsync(reader?.Process);
            writerExit = await FinishPeerAsync(writer);
            readerExit = await FinishPeerAsync(reader);
            await File.AppendAllTextAsync(resultLog, $"DDS_DOMAIN domain_id={domainId} topic={topic}{Environment.NewLine}");
            await File.AppendAllTextAsync(resultLog, $"FAIL_DETAIL {exception.Message.Replace(Environment.NewLine, " ", StringComparison.Ordinal)}{Environment.NewLine}");
        }

        var status = failure ? "FAIL" : "PASS";
        Console.WriteLine($"{status} {scenario.CaseId} [{fixture}] {scenario.Writer} -> {scenario.Reader} domain={domainId}");
        return new ExchangeResult(scenario, status);
    }

    private static string? MissingPeerReason(CaseBuild build, string writerId, string readerId)
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

    private static bool EndpointPassed(string peerId, string role, Scenario scenario, IEnumerable<string>? output)
    {
        if (output is null)
        {
            return false;
        }

        if (peerId == "rti-cpp" && !(scenario.CaseId == "06-aggregates" && scenario.Fixture == "direct-recursive-sequence"))
        {
            var successLine = role == "writer" ? "SENT" : "RECEIVED";
            return output.Contains(successLine, StringComparer.Ordinal);
        }

        var expectedRole = role == "writer" ? "Writer" : "Reader";
        return output.Any(line => line.StartsWith($"PASS {scenario.CaseId} {expectedRole} ", StringComparison.Ordinal));
    }

    private RunningPeer StartPeer(CaseBuild caseBuild, string peer, string role, Scenario scenario,
        string topic, int domainId, string wireloomType, string rtiType, string logPath)
    {
        var peerId = PeerId(peer);
        var recursiveFixture = scenario.CaseId == "06-aggregates" && scenario.Fixture == "direct-recursive-sequence";
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
            process.StartInfo.ArgumentList.Add(options.TimeoutSeconds.ToString(CultureInfo.InvariantCulture));
            process.StartInfo.ArgumentList.Add(scenario.CaseId);
            process.StartInfo.ArgumentList.Add(scenario.Fixture);
            process.StartInfo.ArgumentList.Add(domainId.ToString(CultureInfo.InvariantCulture));
        }

        var output = new ConcurrentQueue<string>();
        var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var writer = new StreamWriter(logPath, append: false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
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
        if (process is not null && !process.HasExited)
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
        await using var output = new StreamWriter(resultLog, append: false, new UTF8Encoding(false));
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

sealed record Options(
    string ScenariosPath,
    string CaseBuildsPath,
    string RtiVersion,
    string OutputDirectory,
    string ScratchDirectory,
    int TimeoutSeconds,
    int MaxConcurrency)
{
    internal static Options Parse(string[] arguments)
    {
        if (arguments.Length == 0 || arguments.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid command line arguments.");
        }

        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var index = 0; index < arguments.Length; index += 2)
        {
            if (!arguments[index].StartsWith("--", StringComparison.Ordinal) || !values.TryAdd(arguments[index][2..], arguments[index + 1]))
            {
                throw new ArgumentException("Invalid command line arguments.");
            }
        }

        string Required(string name) => values.TryGetValue(name, out var value) ? value : throw new ArgumentException("Invalid command line arguments.");
        var timeout = int.Parse(values.GetValueOrDefault("timeout-seconds", "20"), CultureInfo.InvariantCulture);
        var parallelismDefault = Math.Clamp(Environment.ProcessorCount, 2, 16);
        var parallelism = int.Parse(values.GetValueOrDefault("max-concurrency", parallelismDefault.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
        if (timeout < 1 || parallelism < 1 || parallelism > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(arguments), "Timeout must be positive and concurrency must be between 1 and 100.");
        }

        var options = new Options(
            Required("scenarios"), Required("case-builds"), Required("rti-version"),
            Required("output-directory"), Required("scratch"), timeout, parallelism);
        if (options.RtiVersion is not ("7.3.1" or "7.7.0"))
        {
            throw new ArgumentException("Only RTI profiles 7.3.1 and 7.7.0 are supported.");
        }

        return options;
    }
}

sealed record CaseBuild(
    string CaseId,
    string WireloomType,
    string RtiType,
    string CppType,
    string CaseDirectory,
    string WireloomPeer,
    string RtiPeer,
    string CppPeer,
    bool WireloomAvailable,
    string WireloomFailure,
    bool RtiAvailable,
    string RtiFailure)
{
    internal static CaseBuild Parse(string line)
    {
        var fields = line.Split('\t');
        if (fields.Length != 12)
        {
            throw new InvalidDataException("A built-case row must have exactly 12 tab-separated fields.");
        }

        static bool ParseBoolean(string value) => value switch
        {
            "true" or "1" => true,
            "false" or "0" => false,
            _ => throw new InvalidDataException($"Invalid peer availability value '{value}'."),
        };

        return new CaseBuild(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7],
            ParseBoolean(fields[8]), fields[9], ParseBoolean(fields[10]), fields[11]);
    }
}

sealed record Scenario(
    string CaseId,
    string WireloomType,
    string RtiType,
    string CppType,
    string Writer,
    string WriterVersion,
    string Reader,
    string ReaderVersion,
    string Status,
    string Fixture)
{
    internal string Key => string.Join('\t', CaseId, Fixture, Writer, Reader);
    internal string ToTsv() => string.Join('\t', CaseId, WireloomType, RtiType, CppType, Writer, WriterVersion, Reader, ReaderVersion, Status, Fixture);

    internal static Scenario Parse(string line)
    {
        var fields = line.Split('\t');
        if (fields.Length != 10)
        {
            throw new InvalidDataException("A planned exchange row must have exactly 10 tab-separated fields.");
        }

        return new Scenario(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9]);
    }
}

sealed record ExchangeResult(Scenario Scenario, string Status);

sealed record RunningPeer(
    Process Process,
    ConcurrentQueue<string> Output,
    Task Ready,
    Task StandardOutput,
    Task StandardError,
    StreamWriter LogWriter,
    SemaphoreSlim LogLock);
