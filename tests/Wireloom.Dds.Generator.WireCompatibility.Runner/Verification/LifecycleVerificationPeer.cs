using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Deterministic subprocess used by the license-free lifecycle scheduler verification.</summary>
internal static class LifecycleVerificationPeer
{
    internal static async Task<int> RunAsync(string[] args)
    {
        if (args.Length != 2)
        {
            return 2;
        }

        var role = args[0];
        var behavior = args[1];
        if (role == "writer")
        {
            return await RunWriterAsync(behavior);
        }

        if (role == "reader")
        {
            return await RunReaderAsync(behavior);
        }

        return 2;
    }

    private static async Task<int> RunReaderAsync(string behavior)
    {
        var count = behavior == "missing-final" ? 4 : behavior == "reader-exit-early" ? 2 : LifecyclePhaseProtocol.PhaseCount;
        for (var phase = 0; phase < count; phase++)
        {
            if (behavior == "cancel" && phase == 1)
            {
                await Task.Delay(300, CancellationToken.None);
            }

            if (behavior == "timeout" && phase == 4)
            {
                await Task.Delay(2_000, CancellationToken.None);
            }

            var emittedPhase = behavior == "out-of-order" && phase == 0 ? 1 : phase;
            var identity = behavior == "wrong-identity" ? "wrong-scenario" : "scheduler-test";

            Console.WriteLine($"LOAN_RETURNED {phase.ToString(CultureInfo.InvariantCulture)}");
            Console.WriteLine($"PHASE_VERIFIED {identity} {emittedPhase.ToString(CultureInfo.InvariantCulture)}");

            await Console.Out.FlushAsync(CancellationToken.None);
        }

        if (behavior == "duplicate-final")
        {
            Console.WriteLine("PHASE_VERIFIED scheduler-test 4");
        }
        else if (behavior == "malformed-final")
        {
            Console.WriteLine("PHASE_VERIFIED scheduler-test nope");
        }

        await Console.Out.FlushAsync(CancellationToken.None);

        return 0;
    }

    private static async Task<int> RunWriterAsync(string behavior)
    {
        for (var phase = 0; phase < LifecyclePhaseProtocol.PhaseCount; phase++)
        {
            var acknowledgment = await Console.In.ReadLineAsync(CancellationToken.None);
            if (acknowledgment is null)
            {
                return behavior == "reader-exit-early" ? 0 : 3;
            }

            var parts = acknowledgment.Split(' ');
            if (parts.Length != 3 || parts[0] != "PHASE_ACK")
            {
                return 4;
            }

            Console.WriteLine($"ACK_ACCEPTED {parts[1]} {parts[2]}");
            Console.WriteLine($"PHASE_PUBLISHED {parts[1]} {phase.ToString(CultureInfo.InvariantCulture)}");

            await Console.Out.FlushAsync(CancellationToken.None);

            if (behavior == "writer-exit-early" && phase == 0)
            {
                return 0;
            }
        }

        return 0;
    }
}
