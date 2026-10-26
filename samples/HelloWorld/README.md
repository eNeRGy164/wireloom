# Wireloom Hello World sample

This console application publishes and subscribes to ten HelloWorld samples
using RTI Connext DDS. Wireloom generates the C# contract from the checked-in
IDL as part of the normal .NET build.

## Prerequisites

- The repository-pinned .NET SDK (10.0.400).
- Access to NuGet.org to restore the Wireloom generator and RTI Connext DDS packages.
- For running DDS, RTI's native runtime assets for your platform and a valid
  RTI license for the NuGet.org distribution. A license is not needed to
  restore or compile the sample.

See RTI's [C# license setup](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/installation_guide/license_management.html#installing-license-file-for-c-or-python).

## Restore and build

From the repository root, run:

```shell
dotnet restore samples/HelloWorld/HelloWorld.csproj --locked-mode
dotnet build samples/HelloWorld/HelloWorld.csproj --no-restore --configuration Release
```

The project consumes the released Wireloom.Dds.Generator 0.5.0 package from
NuGet.org. Generated C# remains under the build output; hello_world.idl and
Program.cs are maintained inputs.

## Run publisher and subscriber

Set RTI_LICENSE_FILE to your license path in each terminal. For PowerShell:

```powershell
$env:RTI_LICENSE_FILE = 'C:\licenses\rti_license.dat'
dotnet run --project samples/HelloWorld/HelloWorld.csproj --no-build --configuration Release -- --sub
```

In a second terminal, set the variable again and start the publisher:

```powershell
$env:RTI_LICENSE_FILE = 'C:\licenses\rti_license.dat'
dotnet run --project samples/HelloWorld/HelloWorld.csproj --no-build --configuration Release -- --pub
```

For POSIX shells, use these commands:

```sh
export RTI_LICENSE_FILE=/path/to/rti_license.dat
dotnet run --project samples/HelloWorld/HelloWorld.csproj --no-build --configuration Release -- --sub
```

In the second terminal:

```sh
export RTI_LICENSE_FILE=/path/to/rti_license.dat
dotnet run --project samples/HelloWorld/HelloWorld.csproj --no-build --configuration Release -- --pub
```

Start the subscriber first; it waits for samples for up to 30 seconds.
The publisher waits for a matching subscriber before sending ten samples.

The subscriber prints each received message. The publisher prints each sent message and waits for acknowledgments.
Both processes print a completion count and exit successfully when the exchange finishes. Press Ctrl+C to stop either process.

The build-only CI job runs on Windows and Ubuntu without starting the DDS processes or requiring an RTI license.
Run both roles locally with RTI's native assets and a valid license to check end-to-end communication.
