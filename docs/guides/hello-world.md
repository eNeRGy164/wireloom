# Hello World with Wireloom

Choose your application language: follow the **C# walkthrough** below, or jump
to the [Modern C++ walkthrough](#modern-c-walkthrough). Both start with IDL
and finish with ten messages exchanged by two processes.

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
  C# 12 target. The minimum-host check uses SDK 8.0.100 alongside checks with the
  repository's pinned SDK; see [consumer requirements](support-and-compatibility.md#continuous-lts-verification).
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
                      Version="1.0.0"
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
physical copies under `obj`, see [inspecting generated source](../../src/Wireloom.Dds.CSharp.Generator/README.md#inspect-generated-source).

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
  shared IDL roots into a [contracts library](../../src/Wireloom.Dds.CSharp.Generator/README.md#share-generated-contracts).
- Follow RTI's [remaining publish/subscribe exercises][rti-tutorial] for more
  DDS concepts. Its external code-generation commands correspond to a normal
  Wireloom build; its DDS runtime concepts still apply.

## Modern C++ walkthrough

Build the same small DDS application with RTI's Modern C++ API. Wireloom
generates physical C++ sources during the CMake build; RTI supplies the native
SDK, discovery, serialization, and transport. One executable selects its role
with `--pub` or `--sub`, and both processes finish after ten messages.

This follows the progression of RTI's
[Modern C++ publish/subscribe tutorial][rti-cpp-tutorial], using Wireloom's
package and build integration for generation. You maintain the IDL, CMake
project, and application code. You do not need a Wireloom source checkout or
an installed .NET SDK/runtime.

### C++ prerequisites

- CMake 3.20 or later and an x64 Windows or Linux build host. The Windows
  commands use the [VS 2022 generator][cmake-vs2022], which requires CMake 3.21
  or later.
- RTI Connext DDS **7.7.0**, including its Modern C++ headers and the native
  libraries for your platform. A C# RTI NuGet package does not install this SDK.
- An RTI-compatible native compiler: Visual Studio with **Desktop development
  with C++** on Windows, or GCC and Make on Linux. Select a compiler/platform
  combination from RTI's [platform notes][rti-cpp-platforms].
- The applicable RTI license. If your RTI distribution requires a license
  file, set `RTI_LICENSE_FILE` to its full path in each terminal before running.
- An exact `Wireloom.Dds.Cpp.Generator` package version. The first C++ package
  is not published yet; use a retained candidate until publication. Replace
  `<published-version>` below with the actual version you acquired.
- For the installation commands below, the NuGet CLI on Windows or `curl` on
  Linux. Preview artifacts can also be acquired with GitHub CLI as described
  in the package guide.

The commands below use shared Release RTI libraries. Run both roles on the
same machine, in a fresh directory without a `USER_QOS_PROFILES.xml` file.
The application configures reliability and history in code.
This walkthrough uses RTI 7.7.0; consult the
[C++ support contract](support-and-compatibility.md#modern-c-support-and-compatibility)
and the exact package's release record for qualified platforms and profiles.

### Install the C++ generator package

Create a directory named `WireloomHelloWorldCpp` and open a terminal there.
Once a stable C++ package is published, install it on Windows with the NuGet
CLI:

```powershell
$version = '<published-version>'
nuget install Wireloom.Dds.Cpp.Generator -Version $version `
  -Source https://api.nuget.org/v3/index.json -OutputDirectory packages -NonInteractive
```

On Linux, download and extract that exact package:

```bash
version='<published-version>'
package="wireloom.dds.cpp.generator.$version.nupkg"
curl --fail --location --output "$package" \
  "https://api.nuget.org/v3-flatcontainer/wireloom.dds.cpp.generator/$version/$package"
sha256sum "$package" # Compare with the exact version's retained release hash.
mkdir -p "packages/Wireloom.Dds.Cpp.Generator.$version"
(
  cd "packages/Wireloom.Dds.Cpp.Generator.$version"
  cmake -E tar xf "../../$package"
)
```

Until stable publication, use the
[preview or retained-candidate instructions](../../src/Wireloom.Dds.Cpp.Generator/README.md#preview-packages-and-retained-candidates)
instead. They produce the same extracted package directory; the rest of this
walkthrough is unchanged. Use this layout:

```text
WireloomHelloWorldCpp/
  packages/
    Wireloom.Dds.Cpp.Generator.<published-version>/
      build/cmake/WireloomCppConfig.cmake
      tools/win-x64/Wireloom.Dds.Cpp.exe
      tools/linux-x64/Wireloom.Dds.Cpp
  CMakeLists.txt
  hello_world.idl
  main.cpp
```

Only `CMakeLists.txt`, `hello_world.idl`, and `main.cpp` are maintained source
files. The following steps supply their complete contents. Keep the chosen
package version in your build configuration; generation does not download or
silently select a newer version.

### Define the C++ data contract

Save `hello_world.idl` in the project directory:

```idl
struct HelloWorld {
  string<256> msg;
};
```

Wireloom generates a `HelloWorld` value class with a public `std::string msg`
field and RTI type-support code. Assign `sample.msg` directly. The four
generated files are `hello_world.hpp`, `hello_world.cxx`,
`hello_worldPlugin.hpp`, and `hello_worldPlugin.cxx`; CMake places them in the
build directory. Do not create or edit them yourself. See
[C++ value semantics](cpp-generated-value-semantics.md) for other generated APIs.

### Configure the native build

Save this as `CMakeLists.txt`:

```cmake
cmake_minimum_required(VERSION 3.20)
project(WireloomHelloWorldCpp LANGUAGES CXX)

if(NOT DEFINED ENV{NDDSHOME})
    message(FATAL_ERROR "Set NDDSHOME to your RTI Connext DDS 7.7.0 installation.")
endif()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)
find_package(WireloomCpp CONFIG REQUIRED)

add_executable(wireloom_hello_world main.cpp)
set_target_properties(wireloom_hello_world PROPERTIES
    CXX_STANDARD 11
    CXX_STANDARD_REQUIRED YES
    CXX_EXTENSIONS NO
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin")
target_link_libraries(wireloom_hello_world PRIVATE RTIConnextDDS::cpp2_api)
wireloom_add_idl(wireloom_hello_world
    IDL "${CMAKE_CURRENT_SOURCE_DIR}/hello_world.idl")
```

`RTIConnextDDS::cpp2_api` supplies the SDK include directories, libraries, and
platform settings. `wireloom_add_idl` adds generation, all four outputs, and
their include directory to the application target. Both `.cxx` files are
compiled with your application. No separate generation command is needed.

### Write the native publisher and subscriber

Save the complete application as `main.cpp`:

```cpp
#include "hello_world.hpp"
#include <dds/dds.hpp>

#include <chrono>
#include <csignal>
#include <iostream>
#include <stdexcept>
#include <string>
#include <thread>

namespace {
using Clock = std::chrono::steady_clock;
const int sample_count = 10;
volatile std::sig_atomic_t stop_requested = 0;

void stop(int)
{
    stop_requested = 1;
}

void publish(const dds::domain::DomainParticipant& participant,
             const dds::topic::Topic<HelloWorld>& topic)
{
    const dds::pub::Publisher publisher(participant);
    auto qos = publisher.default_datawriter_qos();
    qos << dds::core::policy::Reliability::Reliable()
        << dds::core::policy::History::KeepAll();
    dds::pub::DataWriter<HelloWorld> writer(publisher, topic, qos);

    std::cout << "Waiting for a matching subscriber..." << std::endl;
    const auto deadline = Clock::now() + std::chrono::seconds(30);
    while (!stop_requested
           && writer.publication_matched_status().current_count() == 0) {
        if (Clock::now() >= deadline) {
            throw std::runtime_error("No subscriber matched within 30 seconds.");
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(100));
    }

    HelloWorld sample;
    for (int number = 1; number <= sample_count && !stop_requested; ++number) {
        sample.msg = "Hello world! " + std::to_string(number);
        writer.write(sample);
        std::cout << "Sent: " << sample.msg << std::endl;
        std::this_thread::sleep_for(std::chrono::milliseconds(500));
    }
    if (!stop_requested) {
        writer.wait_for_acknowledgments(dds::core::Duration(10));
        std::cout << "Acknowledged " << sample_count << " samples." << std::endl;
    }
}

void subscribe(const dds::domain::DomainParticipant& participant,
               const dds::topic::Topic<HelloWorld>& topic)
{
    const dds::sub::Subscriber subscriber(participant);
    auto qos = subscriber.default_datareader_qos();
    qos << dds::core::policy::Reliability::Reliable()
        << dds::core::policy::History::KeepAll();
    dds::sub::DataReader<HelloWorld> reader(subscriber, topic, qos);

    int received = 0;
    auto deadline = Clock::now() + std::chrono::seconds(30);
    dds::core::cond::StatusCondition available(reader);
    available.enabled_statuses(dds::core::status::StatusMask::data_available());
    available.extensions().handler([&reader, &received, &deadline]() {
        const auto samples = reader.take();
        for (const auto& sample : samples) {
            if (sample.info().valid()) {
                std::cout << "Received: " << sample.data().msg << std::endl;
                ++received;
                deadline = Clock::now() + std::chrono::seconds(30);
            }
        }
    });
    dds::core::cond::WaitSet waitset;
    waitset += available;

    std::cout << "Waiting for samples..." << std::endl;
    while (received < sample_count && !stop_requested) {
        if (Clock::now() >= deadline) {
            throw std::runtime_error("No sample received within 30 seconds.");
        }
        waitset.dispatch(dds::core::Duration::from_millisecs(250));
    }
    if (!stop_requested) {
        std::cout << "Received " << received << " samples." << std::endl;
    }
}
}

int main(int argc, char** argv)
{
    if (argc != 2 || (std::string(argv[1]) != "--pub"
                      && std::string(argv[1]) != "--sub")) {
        std::cerr << "Usage: wireloom_hello_world --pub | --sub\n";
        return 2;
    }
    std::signal(SIGINT, stop);
    try {
        const dds::domain::DomainParticipant participant(0);
        const dds::topic::Topic<HelloWorld> topic(participant, "HelloWorld Topic");
        if (std::string(argv[1]) == "--pub") {
            publish(participant, topic);
        } else {
            subscribe(participant, topic);
        }
        if (stop_requested) {
            std::cout << "Stopped." << std::endl;
        }
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
```

The writer waits for discovery before sending. Reliable delivery and keep-all
history retain this small batch until the reader can receive it. The reader's
WaitSet invokes the handler on the application thread when data arrives.
`reader.take()` returns a loaned collection; its destructor returns the loan
when the handler exits. Only read `sample.data()` when `sample.info().valid()`
is true, and copy data you want to retain beyond that scope.

The final acknowledgment wait checks DDS delivery; the subscriber's output
shows that the application consumed the samples. Both roles must finish
successfully. RTI entities are released as their C++ objects leave scope.

### Build on Linux

From the project directory, set your SDK and installed RTI platform identifier.
The identifier is the directory name under `$NDDSHOME/lib`, not the generator's
`linux-x64` host identifier. Replace the paths and platform below as needed:

```bash
export NDDSHOME=/path/to/rti_connext_dds-7.7.0
export RTI_ARCH=x64Linux4gcc8.5.0
source "$NDDSHOME/resource/scripts/rtisetenv_$RTI_ARCH.bash"
version='<published-version>'
cmake -S . -B build -DCMAKE_BUILD_TYPE=Release -DBUILD_SHARED_LIBS=ON \
  "-DCONNEXTDDS_ARCH=$RTI_ARCH" -DWIRELOOM_CPP_RTI_VERSION=7.7.0 \
  "-DWireloomCpp_DIR=$PWD/packages/Wireloom.Dds.Cpp.Generator.$version/build/cmake"
cmake --build build --config Release
```

RTI's environment script configures the shared-library search path. The
resulting application is `build/bin/wireloom_hello_world`.

### Build on Windows

Open an **x64 Native Tools Command Prompt for VS 2022**, then start PowerShell
there so the compiler environment is inherited. Change to the project
directory. Set the actual SDK path and installed RTI platform identifier;
`x64Win64VS2017` is RTI's library identifier, while `Visual Studio 17 2022`
selects the CMake generator:

```powershell
$env:NDDSHOME = 'C:\Program Files\rti_connext_dds-7.7.0'
$env:RTI_ARCH = 'x64Win64VS2017'
$env:PATH = "$env:NDDSHOME\lib\$env:RTI_ARCH;$env:PATH"
$version = '<published-version>'
cmake -S . -B build -G "Visual Studio 17 2022" -A x64 -DBUILD_SHARED_LIBS=ON `
  "-DCONNEXTDDS_ARCH=$env:RTI_ARCH" -DWIRELOOM_CPP_RTI_VERSION=7.7.0 `
  "-DWireloomCpp_DIR=$PWD/packages/Wireloom.Dds.Cpp.Generator.$version/build/cmake"
cmake --build build --config Release
```

The application is `build/bin/Release/wireloom_hello_world.exe`. Build Release
consistently; Debug requires the corresponding RTI Debug libraries and runtime.
For an existing `.vcxproj` workflow instead of CMake, see the
[native NuGet/MSBuild instructions](../../src/Wireloom.Dds.Cpp.Generator/README.md#visual-studio--msbuild).

In either build, CMake invokes the packaged host executable and puts the four
generated files in `build/wireloom/wireloom_hello_world/`. Run the build again:
unchanged inputs should skip generation. Editing `hello_world.idl` and
rebuilding reruns generation before compiling the application. Use
`cmake --build build --config Release --clean-first` for a clean rebuild.

### Run the native applications

Prepare both terminals before launching the roles. In **each** terminal, use
the same project directory, SDK, and RTI runtime
environment as for the build. On Linux, repeat the `NDDSHOME`, `RTI_ARCH`, and
`source` lines above. On Windows, repeat the three `$env:` assignments above.
If required by your RTI installation, set your license in both terminals:

```bash
export RTI_LICENSE_FILE=/path/to/rti_license.dat
```

```powershell
$env:RTI_LICENSE_FILE = 'C:\licenses\rti_license.dat'
```

Start the subscriber in the first terminal:

```bash
./build/bin/wireloom_hello_world --sub
```

On Windows, use:

```powershell
.\build\bin\Release\wireloom_hello_world.exe --sub
```

Within 30 seconds, start the publisher in the second terminal:

```bash
./build/bin/wireloom_hello_world --pub
```

On Windows, use:

```powershell
.\build\bin\Release\wireloom_hello_world.exe --pub
```

The publisher prints `Waiting for a matching subscriber...`, then
`Sent: Hello world! 1` through `Sent: Hello world! 10`, followed by
`Acknowledged 10 samples.` The subscriber prints `Waiting for samples...`,
then `Received: Hello world! 1` through `Received: Hello world! 10`, followed
by `Received 10 samples.` Both processes exit with code `0`.

You can start the publisher first, provided the subscriber starts within its
30-second discovery window. The subscriber allows 30 seconds for the first
sample and resets that inactivity timeout when each valid sample arrives.
Restart both roles for another run. Use one
publisher and one subscriber for this exercise. Ctrl+C stops a role; polling
and dispatch check it within 500 ms, while a final acknowledgment wait can
take up to ten seconds. Usage errors return `2`; runtime errors and timeouts
return `1`.

### Troubleshoot the C++ walkthrough

| Symptom | Check |
| :------ | :---- |
| CMake cannot find `WireloomCpp` | Point `WireloomCpp_DIR` at the extracted package's `build/cmake` directory; verify the exact version and both bundled executables. |
| CMake cannot find `RTIConnextDDS` | Set `NDDSHOME` to a complete 7.7.0 native SDK with `resource/cmake/FindRTIConnextDDS.cmake`; a runtime-only or C# package is insufficient. |
| RTI's CMake module warns `Codegen was not found` | The module also looks for `rtiddsgen`. This project generates through Wireloom; if SDK discovery and the build succeed, the RTI code generator is not needed. |
| RTI libraries are missing or incompatible | Select the installed `CONNEXTDDS_ARCH` and an RTI-supported compiler. Match x64, shared/static linkage, and Release/Debug throughout the build. |
| `nddscore` / `nddscpp2` shared library or DLL cannot load | Restore RTI's runtime environment in that terminal: source `rtisetenv` on Linux or add the matching RTI library directory to Windows `PATH`. |
| RTI reports a license error | Set `RTI_LICENSE_FILE` to a readable, applicable license in both terminals; follow your RTI distribution's licensing instructions. |
| Peers time out | Run the same binary with opposite roles, domain `0`, and Topic `HelloWorld Topic`. Start both within 30 seconds. Check firewall, discovery/network settings, and unrelated QoS files using [RTI's troubleshooting guide][rti-cpp-troubleshooting]. |
| Code expects `sample.msg()` | Wireloom exposes the field as `sample.msg`; generated value operations are described in the [C++ semantics guide](cpp-generated-value-semantics.md). |
| Generation repeats after a timestamp-only or comment-only IDL edit | Use a package with the CMake completion-stamp fix. Reconfigure against that package's `build/cmake` directory and rebuild; each edit should generate once, then subsequent unchanged builds skip generation. |
| Old generated sources remain after an upgrade | Follow the [package upgrade/rollback procedure](../../src/Wireloom.Dds.Cpp.Generator/README.md#update-and-roll-back) and configure a clean build directory. |

For the next step, change the IDL, rebuild both roles, and inspect the generated
header in the build directory. Keep package and RTI profile upgrades explicit.
Completing this exchange exercises this bounded-string shape and application
configuration; broader capability claims still follow the
[support contract](support-and-compatibility.md#modern-c-support-and-compatibility).

[rti-cpp-tutorial]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/getting_started_guide/cpp11/intro_pubsub_cpp.html
[rti-cpp-platforms]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/platform_notes/platform_notes/BuildingApplications.htm
[rti-cpp-troubleshooting]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/getting_started_guide/cpp11/intro_pubsub_cpp.html#troubleshooting
[cmake-vs2022]: https://cmake.org/cmake/help/latest/generator/Visual%20Studio%2017%202022.html
[rti-tutorial]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/getting_started_guide/csharp/intro_pubsub_csharp.html
[rti-license]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/installation_guide/license_management.html#installing-license-file-for-c-or-python
[rti-troubleshooting]: https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/getting_started_guide/csharp/intro_pubsub_csharp.html#troubleshooting
