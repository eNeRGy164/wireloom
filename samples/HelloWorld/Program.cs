using System.Diagnostics;
using Omg.Dds.Core;
using Rti.Dds.Core;
using Rti.Dds.Core.Policy;
using Rti.Dds.Core.Status;
using Rti.Dds.Domain;
using Rti.Dds.Topics;

if (args.Length != 1 || args[0] is not ("--pub" or "--sub"))
{
    Console.Error.WriteLine("Usage: dotnet run -- --pub | --sub");
    return 2;
}

const int sampleCount = 10;
using var cancellation = new CancellationTokenSource();
ConsoleCancelEventHandler cancelHandler = (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};
Console.CancelKeyPress += cancelHandler;

try
{
    using var participant = DomainParticipantFactory.Instance.CreateParticipant(domainId: 0);
    using var topic = participant.CreateTopic<HelloWorld>("HelloWorld Topic");

    if (args[0] == "--pub")
    {
        await PublishAsync(participant, topic, sampleCount, cancellation.Token);
    }
    else
    {
        Subscribe(participant, topic, sampleCount, cancellation.Token);
    }

    return 0;
}
catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
{
    Console.WriteLine("Stopped.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}
finally
{
    Console.CancelKeyPress -= cancelHandler;
}

static async Task PublishAsync(DomainParticipant participant, Topic<HelloWorld> topic, int count, CancellationToken cancellationToken)
{
    var qos = participant.ImplicitPublisher.DefaultDataWriterQos
        .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable)
        .WithHistory(policy => policy.Kind = HistoryKind.KeepAll);
    using var writer = participant.ImplicitPublisher.CreateDataWriter(topic, qos);

    Console.WriteLine("Waiting for a matching subscriber...");
    var elapsed = Stopwatch.StartNew();

    while (!writer.MatchedSubscriptions.Any())
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (elapsed.Elapsed >= TimeSpan.FromSeconds(30))
        {
            throw new TimeoutException("No subscriber matched within 30 seconds.");
        }

        await Task.Delay(100, cancellationToken);
    }

    var sample = new HelloWorld();
    for (var number = 1; number <= count; number++)
    {
        cancellationToken.ThrowIfCancellationRequested();

        sample.msg = $"Hello world! {number}";
        writer.Write(sample);

        Console.WriteLine($"Sent: {sample.msg}");

        await Task.Delay(500, cancellationToken);
    }

    writer.WaitForAcknowledgments(Duration.FromSeconds(10));

    Console.WriteLine($"Acknowledged {count} samples.");
}

static void Subscribe(DomainParticipant participant, Topic<HelloWorld> topic, int count, CancellationToken cancellationToken)
{
    var qos = participant.ImplicitSubscriber.DefaultDataReaderQos
        .WithReliability(policy => policy.Kind = ReliabilityKind.Reliable)
        .WithHistory(policy => policy.Kind = HistoryKind.KeepAll);
    using var reader = participant.ImplicitSubscriber.CreateDataReader(topic, qos);

    var received = 0;
    var condition = reader.StatusCondition;
    condition.EnabledStatuses = StatusMask.DataAvailable;
    condition.Triggered += _ =>
    {
        using var samples = reader.Take();
        foreach (var sample in samples)
        {
            if (!sample.Info.ValidData)
            {
                continue;
            }

            Console.WriteLine($"Received: {sample.Data.msg}");
            received++;
        }
    };

    using var waitSet = new WaitSet();
    waitSet.AttachCondition(condition);

    Console.WriteLine("Waiting for samples...");

    var elapsed = Stopwatch.StartNew();

    while (received < count)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (elapsed.Elapsed >= TimeSpan.FromSeconds(30))
        {
            throw new TimeoutException($"Received {received} of {count} samples within 30 seconds.");
        }

        waitSet.Dispatch(Duration.FromMilliseconds(250));
    }

    Console.WriteLine($"Received {received} samples.");
}
