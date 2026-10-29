# Hello World with Wireloom

Build a C# application that publishes and subscribes to a small DDS data type.
This guide follows the progression of RTI's [C# Publish/Subscribe tutorial][rti-tutorial]:
define `HelloWorld` in IDL, create a Topic and a DataWriter or DataReader,
build, and run a publisher and subscriber in separate terminals. Wireloom
supplies the C# contracts during the normal .NET build.

You will send ten messages, receive and print them, and let both processes exit.
The [repository sample](../../samples/HelloWorld/README.md) contains the
files for a ready-to-build project.
One console application selects its role with `--pub` or `--sub`, as in RTI's
example. The application uses RTI's C# APIs for all DDS operations.

## Before you start

- Use a .NET SDK that can load the generator and build the example's .NET 8 /
  C# 12 target. Current package integration is checked with the repository's
  pinned SDK; see [consumer requirements](../../src/Wireloom.Dds.Generator/README.md).
- Have access to `Wireloom.Dds.Generator` and `Rti.ConnextDds` through NuGet.
  The project below uses the existing documented baseline, RTI 7.7.0.
- For the NuGet.org RTI runtime, make a valid RTI license available to **both**
  processes. Follow RTI's [C# license instructions][rti-license]. You can set
  `RTI_LICENSE_FILE` to your license file's full path in each terminal. The
  commands below show an example path to replace with your own.

This tutorial uses `net8.0` and C# 12 to demonstrate the consumer baseline.
The repository sample uses the pinned producer SDK and `net10.0`; both exercise
the same application flow. Its build-only CI does not qualify a live exchange.

The commands use PowerShell. Run the two processes on the same machine for
this exercise, in a fresh directory without unrelated DDS configuration files.
Generation uses the .NET SDK and NuGet package; the application still needs
RTI's runtime, native assets, and applicable license.

## How this maps to RTI's setup

| RTI getting-started step                         | Wireloom step                                                       |
| :----------------------------------------------- | :------------------------------------------------------------------ |
| Get the example application                      | Create a console project and add the application code below.        |
| Set up the generator executable and preprocessor | Restore the generator package and declare the IDL root.             |
| Run the code generator                           | Run `dotnet build`; the compiler invokes Wireloom.                  |
| Examine generated types and example code         | Inspect Roslyn generated documents; maintain `Program.cs` yourself. |
| Build and run `--pub` / `--sub`                  | Build once and run the same roles in two terminals.                 |

The DDS concepts remain familiar: a DomainParticipant joins a domain, a Topic
names a stream and its data type, a DataWriter publishes samples, and a
DataReader receives them. RTI discovers compatible peers. The two processes
must use the same domain and Topic name.

## Create the project

```powershell
dotnet new console --name WireloomHelloWorld --framework net8.0
cd WireloomHelloWorld
```

Replace `WireloomHelloWorld.csproj` with:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>12.0</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
    <PackageReference Include="Wireloom.Dds.Generator"
                      Version="0.5.0"
                      PrivateAssets="all" />
    <DdsIdl Include="hello_world.idl" />
  </ItemGroup>
</Project>
```

## Define the data type

Save this as `hello_world.idl` beside the project file:

```idl
struct HelloWorld {
  string<256> msg;
};
```

The generated `HelloWorld` class has a `msg` property. The narrow-string bound
is 256 UTF-8 bytes, sufficient for the short ASCII messages in this exercise.
Wireloom also generates `HelloWorldSupport` and the RTI adapters. RTI's typed
Topic API discovers that generated support when the application creates its
`Topic<HelloWorld>`.

| File or document                                       | Owner and purpose                                                                                      |
| :----------------------------------------------------- | :----------------------------------------------------------------------------------------------------- |
| `hello_world.idl`                                      | You maintain the shared data contract.                                                                 |
| `WireloomHelloWorld.csproj`                            | You maintain packages, compiler settings, and the IDL root.                                            |
| `Program.cs`                                           | You maintain the publisher/subscriber application.                                                     |
| Generated `HelloWorld`, support, and adapter documents | Wireloom supplies these to Roslyn during compilation; inspect them in the IDE's generated-source view. |

Generated documents do not need to be checked into the source tree. For
physical copies under `obj`, see [inspecting generated source](../../src/Wireloom.Dds.Generator/README.md#inspect-generated-source).

## Write the publisher and subscriber

Replace `Program.cs` with the complete application below. It creates the
`HelloWorld Topic` in domain `0`. Both roles explicitly select reliable
delivery and keep-all history for this ten-message exercise.

The publisher waits for a matching reader before sending its first sample.
The subscriber uses the familiar RTI WaitSet/StatusCondition pattern, returning
sample loans promptly with `using`. QoS is configured in C# here, so this
exercise does not need a `USER_QOS_PROFILES.xml` file.

```csharp
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

static async Task PublishAsync(
    DomainParticipant participant,
    Topic<HelloWorld> topic,
    int count,
    CancellationToken cancellationToken)
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

static void Subscribe(
    DomainParticipant participant,
    Topic<HelloWorld> topic,
    int count,
    CancellationToken cancellationToken)
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
```

### Follow the data flow

`CreateTopic<HelloWorld>` associates the Topic name with the generated data
type. The participant's implicit Publisher and Subscriber create the
DataWriter or DataReader. `Write` sends a sample through RTI's runtime.

The reader enables `DataAvailable` on its status condition and attaches that
condition to a WaitSet. `Dispatch` invokes the handler when samples arrive.
The handler takes available samples and checks `ValidData` before reading
`sample.Data`: DDS also reports instance lifecycle changes without a payload.
Disposing the loaned collection returns its storage; use its data inside the
`using` scope rather than retaining references to it afterward.

The publisher's final acknowledgment wait concerns reliable DDS delivery.
The subscriber's printed messages and completion line show that the
application actually read the samples. Both processes must complete to finish
the exercise successfully.

## Build the application

From the directory containing `WireloomHelloWorld.csproj`, run:

```powershell
dotnet build
```

Restore supplies the generator and RTI packages. The build reads
`hello_world.idl`, generates the C# documents, and compiles them with
`Program.cs`. There is no separate generator command or output directory to
wire into the project. You should see a successful build before running peers.

## Run the applications

In the first terminal, from the project directory, configure your own license
path and start the subscriber:

```powershell
$env:RTI_LICENSE_FILE = 'C:\licenses\rti_license.dat'
dotnet run --no-build -- --sub
```

It prints `Waiting for samples...`. Within 30 seconds, open a second terminal
in the **same project directory**, configure the license there too, and start
the publisher:

```powershell
$env:RTI_LICENSE_FILE = 'C:\licenses\rti_license.dat'
dotnet run --no-build -- --pub
```

The publisher reports discovery and the messages it sends:

```text
Waiting for a matching subscriber...
Sent: Hello world! 1
Sent: Hello world! 2
...
Sent: Hello world! 10
Acknowledged 10 samples.
```

The subscriber prints the corresponding payloads and finishes:

```text
Waiting for samples...
Received: Hello world! 1
Received: Hello world! 2
...
Received: Hello world! 10
Received 10 samples.
```

Both processes exit successfully. You can also start the publisher first and
start the subscriber within its 30-second discovery window. For another run,
restart both roles so their ten-message counters start together. This exercise
assumes one publisher and one subscriber; extra publishers contribute samples
to the same subscriber count.

Press Ctrl+C to stop a role early. Cancellation interrupts polling and send
delays, and the reader checks cancellation between 250 ms WaitSet dispatches.
The final acknowledgment call can take up to its ten-second timeout before
shutdown completes.

## Troubleshooting

| Symptom                                                    | Check                                                                                                                                                          |
| :--------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| The build cannot find `HelloWorld`                         | Check the package reference, `DdsIdl` item, and `hello_world.idl` location; resolve generator diagnostics before running.                                      |
| A license error appears                                    | Replace the example license path, verify the file is available to each process, and follow [RTI's license setup][rti-license].                                 |
| The native RTI library cannot load                         | Check the restored runtime/native assets and RTI's installation guidance for your application environment.                                                     |
| No subscriber matches                                      | Run both roles with domain `0` and the same `HelloWorld Topic`; start the other role within 30 seconds.                                                        |
| The subscriber times out or prints fewer than ten messages | Restart both roles, ensure no unrelated publishers use this Topic, and inspect discovery/QoS configuration using [RTI's troubleshooting][rti-troubleshooting]. |
| Changes are absent when running                            | Run `dotnet build` after editing IDL or C#; `--no-build` runs the last compiled application.                                                                   |

Discovery takes time. Reliable delivery alone does not provide past volatile
samples to a reader discovered after those samples were sent; waiting for a
match before the first write is part of this example's startup procedure.
See RTI's tutorial for further discovery and QoS exercises.

## Take it further

- Change the greeting in `Program.cs`, rebuild, and run both roles again.
- Add a member to the IDL, rebuild, and inspect the updated generated API and
  IntelliSense documentation.
- Learn [generated value semantics](generated-value-semantics.md), or move
  shared IDL roots into a [contracts library](../../src/Wireloom.Dds.Generator/README.md#share-generated-contracts).
- Follow RTI's [remaining publish/subscribe exercises][rti-tutorial] for more
  DDS concepts. Its external code-generation commands correspond to a normal
  Wireloom build; its DDS runtime concepts still apply.

[rti-tutorial]: https://community.rti.com/static/documentation/connext-dds/current/doc/manuals/connext_dds_professional/getting_started_guide/csharp/intro_pubsub_csharp.html
[rti-license]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/installation_guide/license_management.html#installing-license-file-for-c-or-python
[rti-troubleshooting]: https://community.rti.com/static/documentation/connext-dds/current/doc/manuals/connext_dds_professional/getting_started_guide/csharp/intro_pubsub_csharp.html#troubleshooting
