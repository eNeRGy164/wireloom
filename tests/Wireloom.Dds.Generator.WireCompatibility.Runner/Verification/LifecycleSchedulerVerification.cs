using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Channels;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Exercises the real scheduler boundary against deterministic subprocess peers.</summary>
internal static class LifecycleSchedulerVerification
{
    private const string ScenarioId = "scheduler-test";

    internal static async Task RunAsync(string directory)
    {
        Directory.CreateDirectory(directory);

        var delayedFinal = await RunCaseAsync(directory, "success", pumpDelayOnFinal: true);
        Check(delayedFinal.Result is { Completed: true, AcceptedPhases: 5, ReaderOutputDrained: true },
            "A final event delivered by a delayed stdout pump after process exit must complete the exchange.");

        foreach (var pump in new[] { "reader-stdout", "reader-stderr", "writer-stdout", "writer-stderr" })
        {
            var stalledDrain = await RunCaseAsync(directory, "stalled-" + pump, blockedPump: pump);
            Check(stalledDrain.Result is { Completed: false, AcceptedPhases: 5, Outcome: "endpoint-timeout" },
                $"A pending {pump} pump after all phases must fail at the overall deadline; got {stalledDrain.Result}.");
            Check(stalledDrain.Elapsed < TimeSpan.FromSeconds(2), "A stalled output pump must not exceed the overall deadline by more than scheduler cleanup tolerance.");
            Check(stalledDrain.LogContainsFinalPhase, "The partial endpoint log must retain evidence written before a stalled drain.");
        }

        var cancelledDrain = await RunCaseAsync(directory, "cancelled-drain", blockedPump: "reader-stdout", cancelAfterFinalAcknowledgment: true);
        Check(cancelledDrain.Result is { Completed: false, AcceptedPhases: 5, Outcome: "cancelled-after-final-phase" },
            "External cancellation during final output drainage must fail, never complete the exchange.");
        Check(cancelledDrain.Elapsed < TimeSpan.FromSeconds(2), "External cancellation must bound final output drainage.");

        var timeline = delayedFinal.Timeline.ToArray();
        for (var phase = 0; phase < LifecyclePhaseProtocol.PhaseCount; phase++)
        {
            var returned = Array.IndexOf(timeline, $"LOAN_RETURNED {phase}");
            var verified = Array.IndexOf(timeline, $"PHASE_VERIFIED {ScenarioId} {phase}");
            var acknowledged = Array.IndexOf(timeline, $"ACK_ACCEPTED {ScenarioId} {phase}");
            Check(returned >= 0 && verified > returned && acknowledged > verified,
                $"Phase {phase} acknowledgment must follow reader loan return and verification.");
        }

        var missingFinal = await RunCaseAsync(directory, "missing-final");
        Check(missingFinal.Result is { Completed: false, AcceptedPhases: 4 }, "A missing final event must fail with four accepted phases retained.");

        var duplicate = await RunCaseAsync(directory, "duplicate-final");
        Check(duplicate.Result is { Completed: false, AcceptedPhases: 5 }, "A trailing duplicate final event must fail after all planned phases.");

        var malformed = await RunCaseAsync(directory, "malformed-final");
        Check(malformed.Result is { Completed: false, AcceptedPhases: 5 }, "A malformed trailing lifecycle event must fail.");

        var outOfOrder = await RunCaseAsync(directory, "out-of-order");
        Check(outOfOrder.Result is { Completed: false, AcceptedPhases: 0 }, "An out-of-order lifecycle event must fail before acknowledgment.");

        var wrongIdentity = await RunCaseAsync(directory, "wrong-identity");
        Check(wrongIdentity.Result is { Completed: false, AcceptedPhases: 0 }, "A wrong-scenario lifecycle event must fail before acknowledgment.");

        var readerExit = await RunCaseAsync(directory, "reader-exit-early");
        Check(readerExit.Result is { Completed: false, AcceptedPhases: < LifecyclePhaseProtocol.PhaseCount },
            "An early reader exit must fail and retain the accepted phase count.");

        var writerExit = await RunCaseAsync(directory, "writer-exit-early");
        Check(!writerExit.Result.Completed, "An early writer exit must fail the exchange.");

        var timeout = await RunCaseAsync(directory, "timeout", phaseTimeout: TimeSpan.FromMilliseconds(200));
        Check(timeout.Result is { Completed: false, Outcome: "phase-timeout" }, "A missing phase must honor its bounded deadline.");

        var cancellation = await RunCaseAsync(directory, "cancel", cancelAfterFirstAcknowledgment: true);
        Check(cancellation.Result is { Completed: false, Outcome: "cancelled" }, "Cancellation during an exchange must fail cleanly.");

        Console.WriteLine("Lifecycle scheduler passed: delayed final event, stalled output streams, bounded final drainage and cancellation, missing/duplicate/malformed events, identity/order, early exits, timeout, loan-return acknowledgment order, and partial phase counts.");
    }

    private static async Task<CaseResult> RunCaseAsync(string directory, string behavior,
        bool pumpDelayOnFinal = false, TimeSpan? phaseTimeout = null, bool cancelAfterFirstAcknowledgment = false,
        string? blockedPump = null, bool cancelAfterFinalAcknowledgment = false)
    {
        var readerProcess = StartPeer("reader", behavior);
        var writerProcess = StartPeer("writer", behavior);
        var events = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });
        var output = new ConcurrentQueue<string>();
        var timeline = new ConcurrentQueue<string>();
        using var cancellation = new CancellationTokenSource();
        var pumpRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var readerLogWriter = new StreamWriter(Path.Combine(directory, behavior + ".log"), append: false);
        var readerLogLock = new SemaphoreSlim(1);
        var writerLogWriter = new StreamWriter(Path.Combine(directory, behavior + "-writer.log"), append: false);
        var writerLogLock = new SemaphoreSlim(1);

        async Task PumpAsync(StreamReader source, bool readerStdout, Process readerPeer,
            StreamWriter pumpLogWriter, SemaphoreSlim pumpLogLock, CancellationTokenSource pumpCancellation)
        {
            while (await source.ReadLineAsync(CancellationToken.None) is { } line)
            {
                output.Enqueue(line);
                timeline.Enqueue(line);

                if (line == $"ACK_ACCEPTED {ScenarioId} 4")
                {
                    if (cancelAfterFinalAcknowledgment)
                    {
                        await pumpCancellation.CancelAsync();
                    }
                }

                if (readerStdout && line.StartsWith("PHASE_VERIFIED ", StringComparison.Ordinal))
                {
                    if (pumpDelayOnFinal && line == $"PHASE_VERIFIED {ScenarioId} 4")
                    {
                        await readerPeer.WaitForExitAsync(CancellationToken.None);
                        await Task.Delay(100, CancellationToken.None);
                    }

                    await events.Writer.WriteAsync(line, CancellationToken.None);
                }

                await pumpLogLock.WaitAsync(CancellationToken.None);

                try
                {
                    await pumpLogWriter.WriteLineAsync(line);
                    await pumpLogWriter.FlushAsync(CancellationToken.None);
                }
                finally
                {
                    pumpLogLock.Release();
                }

                if (cancelAfterFirstAcknowledgment && line == $"ACK_ACCEPTED {ScenarioId} 0")
                {
                    pumpCancellation.CancelAfter(75);
                }
            }
        }

        var readerOutput = PumpAsync(readerProcess.StandardOutput, readerStdout: true, readerProcess,
            readerLogWriter, readerLogLock, cancellation);
        var readerError = PumpAsync(readerProcess.StandardError, readerStdout: false, readerProcess,
            readerLogWriter, readerLogLock, cancellation);
        var writerOutput = PumpAsync(writerProcess.StandardOutput, readerStdout: false, readerProcess,
            writerLogWriter, writerLogLock, cancellation);
        var writerError = PumpAsync(writerProcess.StandardError, readerStdout: false, readerProcess,
            writerLogWriter, writerLogLock, cancellation);
        var readerPumps = CompleteChannelAfterPumpsAsync(readerOutput, readerError, events.Writer);
        var writerPumps = Task.WhenAll(writerOutput, writerError);
        Task BoundPump(Task pump, string name) => blockedPump == name ? Task.WhenAll(pump, pumpRelease.Task) : pump;

        var reader = new RunningPeer(readerProcess, output, Task.CompletedTask, BoundPump(readerOutput, "reader-stdout"), BoundPump(readerError, "reader-stderr"),
            readerLogWriter, readerLogLock, events.Reader, null);
        var writerPeer = new RunningPeer(writerProcess, output, Task.CompletedTask,
            BoundPump(writerOutput, "writer-stdout"), BoundPump(writerError, "writer-stderr"), writerLogWriter, writerLogLock, Channel.CreateUnbounded<string>().Reader, writerProcess.StandardInput);

        var stopwatch = Stopwatch.StartNew();
        var cleanupOwned = false;
        var cleanupPeerIsReader = true;
        try
        {
            var result = await ExchangeScheduler.RunLifecycleExchangeAsync(reader, writerPeer, ScenarioId,
                cancellation.Token, phaseTimeout ?? TimeSpan.FromSeconds(2), blockedPump is null ? TimeSpan.FromSeconds(4) : TimeSpan.FromMilliseconds(600));
            var schedulerElapsed = stopwatch.Elapsed;

            if (blockedPump is not null)
            {
                var cleanupWatch = Stopwatch.StartNew();
                cleanupPeerIsReader = !blockedPump.StartsWith("writer-", StringComparison.Ordinal);
                await ExchangeScheduler.FinishPeerAsync(cleanupPeerIsReader ? reader : writerPeer);
                cleanupOwned = true;
                Check(cleanupWatch.Elapsed < TimeSpan.FromSeconds(5), "Peer cleanup must remain bounded when an inherited output pipe is stalled.");
            }

            return new CaseResult(result, timeline, schedulerElapsed,
                timeline.Contains("PHASE_VERIFIED scheduler-test 4", StringComparer.Ordinal));
        }
        finally
        {
            pumpRelease.TrySetResult();
            if (!cleanupOwned)
            {
                await KillAndDrainAsync(readerProcess, writerProcess, readerPumps, writerPumps);
                await readerLogWriter.DisposeAsync();
                await writerLogWriter.DisposeAsync();
                readerLogLock.Dispose();
                writerLogLock.Dispose();
                readerProcess.Dispose();
                writerProcess.Dispose();
            }
            else
            {
                var remainingProcess = cleanupPeerIsReader ? writerProcess : readerProcess;
                if (!remainingProcess.HasExited)
                {
                    remainingProcess.Kill(entireProcessTree: true);
                }

                try
                {
                    await Task.WhenAll(readerOutput, readerError, writerOutput, writerError);
                }
                catch (Exception)
                {
                    // The scheduler already returned a partial failure for the stalled stream.
                }

                await readerLogWriter.DisposeAsync();
                await writerLogWriter.DisposeAsync();
                readerLogLock.Dispose();
                writerLogLock.Dispose();
                remainingProcess.Dispose();
            }
        }
    }

    private static Process StartPeer(string role, string behavior)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo("dotnet")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = role == "writer",
                UseShellExecute = false,
            },
        };

        process.StartInfo.ArgumentList.Add(typeof(LifecycleSchedulerVerification).Assembly.Location);
        process.StartInfo.ArgumentList.Add("--lifecycle-verification-peer");
        process.StartInfo.ArgumentList.Add(role);
        process.StartInfo.ArgumentList.Add(behavior);

        if (!process.Start())
        {
            throw new InvalidOperationException($"Could not start lifecycle verification {role} process.");
        }

        return process;
    }

    private static async Task CompleteChannelAfterPumpsAsync(Task stdout, Task stderr, ChannelWriter<string> writer)
    {
        try
        {
            await Task.WhenAll(stdout, stderr);
            writer.TryComplete();
        }
        catch (Exception exception)
        {
            writer.TryComplete(exception);
            throw;
        }
    }

    private static async Task KillAndDrainAsync(Process reader, Process writer, Task readerPumps, Task writerPumps)
    {
        foreach (var process in new[] { reader, writer })
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }

        await Task.WhenAll(reader.WaitForExitAsync(), writer.WaitForExitAsync());

        try
        {
            await Task.WhenAll(readerPumps, writerPumps);
        }
        catch (IOException)
        {
            // The original coordination result already records a pump failure.
        }
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed record CaseResult(LifecycleCoordinationResult Result, ConcurrentQueue<string> Timeline,
        TimeSpan Elapsed, bool LogContainsFinalPhase);
}
