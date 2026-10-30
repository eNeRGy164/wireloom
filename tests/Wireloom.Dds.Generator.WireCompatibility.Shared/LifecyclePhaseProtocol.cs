using System.Globalization;

namespace Wireloom.Dds.Generator.WireCompatibility.Shared;

/// <summary>Parses and validates the small line protocol used to pace lifecycle samples.</summary>
public sealed class LifecyclePhaseProtocol(string scenarioId)
{
    /// <summary>The five ordered sample phases used by the receive lifecycle fixture.</summary>
    public const int PhaseCount = 5;

    /// <summary>Unique scenario identity echoed by every peer control event.</summary>
    private string ScenarioId { get; } = RequireScenarioId(scenarioId);

    /// <summary>The next phase that a reader event or writer acknowledgment must carry.</summary>
    private int NextPhase { get; set; }
    private int nextWriterPhase;
    private int? publishedWriterPhase;
    private bool writerMode;

    /// <summary>Validates a reader event and advances only after the expected phase arrives.</summary>
    public LifecyclePhaseEvent AcceptVerifiedEvent(string line)
    {
        var item = Parse(line, "PHASE_VERIFIED");
        if (item.ScenarioId != ScenarioId)
        {
            throw new InvalidDataException("Lifecycle event has the wrong scenario identity.");
        }

        if (item.PhaseId != NextPhase)
        {
            throw new InvalidDataException($"Lifecycle event phase {item.PhaseId} arrived while phase {NextPhase} was expected.");
        }

        if (NextPhase >= PhaseCount)
        {
            throw new InvalidDataException("Lifecycle event arrived after the planned phases were complete.");
        }

        NextPhase++;

        return item;
    }

    /// <summary>Validates that a writer is waiting for the current phase acknowledgment.</summary>
    public int AcceptWriterAcknowledgment(string line)
    {
        var item = Parse(line, "PHASE_ACK");
        if (item.ScenarioId != ScenarioId)
        {
            throw new InvalidDataException("Lifecycle acknowledgment has the wrong scenario identity.");
        }

        if (!writerMode || publishedWriterPhase != item.PhaseId
            || item.PhaseId != nextWriterPhase || nextWriterPhase >= PhaseCount)
        {
            throw new InvalidDataException($"Lifecycle acknowledgment phase {item.PhaseId} arrived before that phase was published or while phase {nextWriterPhase} was expected.");
        }

        nextWriterPhase++;
        publishedWriterPhase = null;

        return item.PhaseId;
    }

    /// <summary>Marks a writer phase as published before accepting its runner acknowledgment.</summary>
    public void MarkWriterPublished(int phaseId)
    {
        if (phaseId != nextWriterPhase || phaseId >= PhaseCount || publishedWriterPhase is not null)
        {
            throw new InvalidDataException($"Cannot publish lifecycle phase {phaseId} while writer phase {nextWriterPhase} is expected.");
        }

        writerMode = true;
        publishedWriterPhase = phaseId;
    }

    /// <summary>Creates the runner-owned acknowledgment line after a reader event is accepted.</summary>
    public string CreateAcknowledgment(int phaseId)
    {
        if (phaseId != NextPhase - 1 || phaseId < 0 || phaseId >= PhaseCount)
        {
            throw new InvalidDataException("Cannot acknowledge a phase that has not been accepted.");
        }

        return $"PHASE_ACK {ScenarioId} {phaseId.ToString(CultureInfo.InvariantCulture)}";
    }

    /// <summary>Rejects completion unless every planned phase has been acknowledged or verified.</summary>
    public void EnsureComplete()
    {
        var completed = writerMode ? nextWriterPhase : NextPhase;
        if (completed != PhaseCount)
        {
            throw new InvalidDataException($"Lifecycle exchange completed after {completed} of {PhaseCount} phases.");
        }
    }

    /// <summary>Parses one strict event line without interpreting arbitrary peer output as a command.</summary>
    private static LifecyclePhaseEvent Parse(string line, string expectedKind)
    {
        ArgumentNullException.ThrowIfNull(line);
        var parts = line.Split(' ');
        if (parts.Length != 3
            || parts[0] != expectedKind
            || string.IsNullOrWhiteSpace(parts[1])
            || !int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out var phaseId))
        {
            throw new InvalidDataException("Lifecycle control event is malformed.");
        }

        return new LifecyclePhaseEvent(parts[1], phaseId);
    }

    private static string RequireScenarioId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Any(character => !char.IsAsciiLetterOrDigit(character) && character is not ('_' or '-')))
        {
            throw new ArgumentException("Scenario identity contains unsupported characters.", nameof(value));
        }

        return value;
    }
}

/// <summary>A parsed lifecycle event associated with its scenario and phase.</summary>
/// <param name="ScenarioId">Unique exchange identity.</param>
/// <param name="PhaseId">Zero-based phase number.</param>
public sealed record LifecyclePhaseEvent(string ScenarioId, int PhaseId);
