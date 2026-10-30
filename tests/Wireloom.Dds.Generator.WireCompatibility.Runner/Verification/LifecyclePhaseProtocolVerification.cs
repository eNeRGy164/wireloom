namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Verifies strict lifecycle event parsing and monotonically ordered phase handling.</summary>
internal static class LifecyclePhaseProtocolVerification
{
    internal static void Run()
    {
        // Arrange
        var protocol = new LifecyclePhaseProtocol("case-fixture-writer-reader");

        // Act
        for (var phase = 0; phase < LifecyclePhaseProtocol.PhaseCount; phase++)
        {
            protocol.AcceptVerifiedEvent($"PHASE_VERIFIED case-fixture-writer-reader {phase}");
            var acknowledgment = protocol.CreateAcknowledgment(phase);
            Check(acknowledgment == $"PHASE_ACK case-fixture-writer-reader {phase}", "Accepted reader events produce only canonical acknowledgments.");
        }

        // Assert
        protocol.EnsureComplete();
        Throws<InvalidDataException>(() => protocol.AcceptVerifiedEvent("PHASE_VERIFIED case-fixture-writer-reader 4"), "A duplicate event after completion is rejected.");

        var missingFinal = new LifecyclePhaseProtocol("missing-final");
        for (var phase = 0; phase < LifecyclePhaseProtocol.PhaseCount - 1; phase++)
        {
            missingFinal.AcceptVerifiedEvent($"PHASE_VERIFIED missing-final {phase}");
        }
        Throws<InvalidDataException>(missingFinal.EnsureComplete, "A missing final phase cannot produce complete lifecycle evidence.");

        var malformed = new LifecyclePhaseProtocol("malformed");
        Throws<InvalidDataException>(() => malformed.AcceptVerifiedEvent("PHASE_VERIFIED malformed 0 unexpected"), "Malformed control events are rejected.");

        var wrongIdentity = new LifecyclePhaseProtocol("identity");
        Throws<InvalidDataException>(() => wrongIdentity.AcceptVerifiedEvent("PHASE_VERIFIED other 0"), "Events from another scenario are rejected.");

        var outOfOrder = new LifecyclePhaseProtocol("order");
        Throws<InvalidDataException>(() => outOfOrder.AcceptVerifiedEvent("PHASE_VERIFIED order 1"), "Out-of-order events are rejected.");

        var duplicate = new LifecyclePhaseProtocol("duplicate");
        duplicate.AcceptVerifiedEvent("PHASE_VERIFIED duplicate 0");
        Throws<InvalidDataException>(() => duplicate.AcceptVerifiedEvent("PHASE_VERIFIED duplicate 0"), "Duplicate events are rejected.");

        var writer = new LifecyclePhaseProtocol("writer");
        Throws<InvalidDataException>(() => writer.AcceptWriterAcknowledgment("PHASE_ACK writer 0"), "A writer cannot advance before the runner accepts the reader event.");

        for (var phase = 0; phase < LifecyclePhaseProtocol.PhaseCount; phase++)
        {
            writer.MarkWriterPublished(phase);
            Check(writer.AcceptWriterAcknowledgment($"PHASE_ACK writer {phase}") == phase, "A writer accepts only the acknowledgment for its currently published phase.");
        }
        writer.EnsureComplete();

        Console.WriteLine("Lifecycle phase protocol passed: complete, missing-final, malformed, identity, duplicate, order, and premature-ack cases.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void Throws<TException>(Action action, string message) where TException : Exception
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
}
