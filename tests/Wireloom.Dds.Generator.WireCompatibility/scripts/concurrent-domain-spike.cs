#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

const string usage = "Usage: dotnet run --file concurrent-domain-spike.cs -- --peer-dll <path> --type <generated-type> [--case 01-boundaries] [--fixture-a non-default] [--fixture-b non-default] [--runtime-version 7.7.0] [--domain-a 20] [--domain-b 21] [--timeout-seconds 20]";
if (args.Contains("--help", StringComparer.Ordinal))
{
    Console.WriteLine(usage);
    return 0;
}

var options = ParseArguments(args);
if (options is null)
{
    Console.Error.WriteLine(usage);
    return 2;
}

var peerPath = Path.GetFullPath(options.PeerDll);
if (!File.Exists(peerPath))
{
    Console.Error.WriteLine($"Peer assembly not found: {peerPath}");
    return 2;
}

if (options.DomainA == options.DomainB)
{
    Console.Error.WriteLine("The two concurrent exchanges must use different domain IDs.");
    return 2;
}

var topic = $"wireloom_domain_spike_{Guid.NewGuid():N}";
var exchanges = new[]
{
    new Exchange("A", options.FixtureA, options.DomainA),
    new Exchange("B", options.FixtureB, options.DomainB)
};

Console.WriteLine($"Case: {options.CaseId}; topic: {topic}; RTI profile: {options.RuntimeVersion}");
var sequentialTimer = Stopwatch.StartNew();
foreach (var exchange in exchanges)
{
    await RunExchangeAsync(peerPath, options, topic, exchange);
}
sequentialTimer.Stop();

Console.WriteLine("Running both exchanges concurrently on distinct DDS domains.");
var concurrentTimer = Stopwatch.StartNew();
await Task.WhenAll(exchanges.Select(exchange => RunExchangeAsync(peerPath, options, topic, exchange)));
concurrentTimer.Stop();

Console.WriteLine($"SEQUENTIAL_MS {sequentialTimer.ElapsedMilliseconds}");
Console.WriteLine($"CONCURRENT_MS {concurrentTimer.ElapsedMilliseconds}");
Console.WriteLine($"SPEEDUP {sequentialTimer.Elapsed.TotalSeconds / Math.Max(concurrentTimer.Elapsed.TotalSeconds, 0.001):F2}x");
return 0;

static Options? ParseArguments(string[] arguments)
{
    if (arguments.Length == 0)
    {
        return null;
    }

    var values = new Dictionary<string, string>(StringComparer.Ordinal);
    for (var index = 0; index < arguments.Length; index += 2)
    {
        if (index + 1 >= arguments.Length || !arguments[index].StartsWith("--", StringComparison.Ordinal))
        {
            throw new ArgumentException(usage);
        }

        values.Add(arguments[index][2..], arguments[index + 1]);
    }

    var options = new Options(
        values["peer-dll"], values["type"], values.GetValueOrDefault("case", "01-boundaries"),
        values.GetValueOrDefault("fixture-a", "non-default"), values.GetValueOrDefault("fixture-b", "non-default"),
        values.GetValueOrDefault("runtime-version", "7.7.0"),
        ParseInteger(values.GetValueOrDefault("domain-a", "20")),
        ParseInteger(values.GetValueOrDefault("domain-b", "21")),
        ParseInteger(values.GetValueOrDefault("timeout-seconds", "20")));

    if (options.TimeoutSeconds < 1 || options.DomainA < 0 || options.DomainB < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(arguments), "Timeout must be positive and domain IDs non-negative.");
    }

    return options;
}

static int ParseInteger(string value) => int.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture);

static async Task RunExchangeAsync(string peerDll, Options options, string topic, Exchange exchange)
{
    var timer = Stopwatch.StartNew();
    var reader = StartPeer(peerDll, options, topic, exchange, "reader");
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(options.TimeoutSeconds + 10));
    RunningPeer? writer = null;

    try
    {
        var readerExit = reader.Process.WaitForExitAsync(timeout.Token);
        if (await Task.WhenAny(reader.Ready.Task, readerExit) != reader.Ready.Task)
        {
            await readerExit;
            reader.Process.WaitForExit();
            throw new InvalidOperationException(
                $"Reader exited before READY for exchange {exchange.Name} (exit={reader.Process.ExitCode}).\n" +
                string.Join(Environment.NewLine, reader.Output));
        }

        writer = StartPeer(peerDll, options, topic, exchange, "writer");
        await Task.WhenAll(
            writer.Process.WaitForExitAsync(timeout.Token),
            reader.Process.WaitForExitAsync(timeout.Token));
        var writerPassed = writer.Output.Any(line => line.StartsWith($"PASS {options.CaseId} Writer ", StringComparison.Ordinal));
        var readerPassed = reader.Output.Any(line => line.StartsWith($"PASS {options.CaseId} Reader ", StringComparison.Ordinal));
        if (writer.Process.ExitCode != 0 || reader.Process.ExitCode != 0 || !writerPassed || !readerPassed)
        {
            throw new InvalidOperationException(
                $"Exchange {exchange.Name} failed (writer={writer.Process.ExitCode}/{writerPassed}, reader={reader.Process.ExitCode}/{readerPassed}).\n" +
                $"Writer:\n{string.Join(Environment.NewLine, writer.Output)}\n" +
                $"Reader:\n{string.Join(Environment.NewLine, reader.Output)}");
        }

        Console.WriteLine($"PASS exchange={exchange.Name} fixture={exchange.Fixture} domain={exchange.DomainId} elapsed_ms={timer.ElapsedMilliseconds}");
    }
    catch
    {
        KillIfRunning(writer?.Process);
        KillIfRunning(reader.Process);
        throw;
    }
    finally
    {
        writer?.Process.Dispose();
        reader.Process.Dispose();
    }
}

static RunningPeer StartPeer(string peerDll, Options options, string topic, Exchange exchange, string role)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        },
        EnableRaisingEvents = true
    };
    process.StartInfo.ArgumentList.Add(peerDll);
    AddOption(process, "case", options.CaseId);
    AddOption(process, "type", options.TypeName);
    AddOption(process, "topic", topic);
    AddOption(process, "runtime-version", options.RuntimeVersion);
    AddOption(process, "role", role);
    AddOption(process, "fixture", exchange.Fixture);
    AddOption(process, "domain-id", exchange.DomainId.ToString(CultureInfo.InvariantCulture));

    var output = new ConcurrentQueue<string>();
    var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
    process.OutputDataReceived += (_, eventArgs) =>
    {
        if (eventArgs.Data is not { } line)
        {
            return;
        }

        output.Enqueue(line);
        if (line.StartsWith("READY ", StringComparison.Ordinal))
        {
            ready.TrySetResult();
        }
    };
    process.ErrorDataReceived += (_, eventArgs) =>
    {
        if (eventArgs.Data is { } line)
        {
            output.Enqueue($"STDERR {line}");
        }
    };

    if (!process.Start())
    {
        throw new InvalidOperationException($"Could not start {role} process for exchange {exchange.Name}.");
    }

    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    return new RunningPeer(process, ready, output);
}

static void AddOption(Process process, string name, string value)
{
    process.StartInfo.ArgumentList.Add($"--{name}");
    process.StartInfo.ArgumentList.Add(value);
}

static void KillIfRunning(Process? process)
{
    if (process is not null && !process.HasExited)
    {
        process.Kill(entireProcessTree: true);
    }
}

sealed record Options(
    string PeerDll, string TypeName, string CaseId, string FixtureA, string FixtureB,
    string RuntimeVersion, int DomainA, int DomainB, int TimeoutSeconds);

sealed record Exchange(string Name, string Fixture, int DomainId);

sealed record RunningPeer(Process Process, TaskCompletionSource Ready, ConcurrentQueue<string> Output);
