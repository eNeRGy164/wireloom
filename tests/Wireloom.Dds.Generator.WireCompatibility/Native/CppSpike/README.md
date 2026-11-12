# C044 Modern C++ wire regression

This fixture generates an RTI Modern C++ topic from the existing
[C044 IDL](../../../../docs/corpus/idl/features/10-annotations.idl). The
application uses typed RTI topics, readers, and writers. The Wireloom
plugin describes the sample's layout and key to the generic type plugin
factory in the RTI SDK. RTI supplies the serialization and DDS transport.

This fixture covers the C044 two-field scalar shape on **RTI Connext DDS
7.7.0 on Linux x64** and exercises a profile-specific plugin API. It is one
regression case in the broader Modern C++ feature; supported shapes and their
qualification are described in the [generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md)
and [RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md).

## Refresh the retained output

Run from the repository root. This uses the pinned .NET SDK and works
without the RTI SDK or license.

```powershell
$runner = 'tests/Wireloom.Dds.Generator.WireCompatibility.Runner/Wireloom.Dds.Generator.WireCompatibility.Runner.csproj'
dotnet restore $runner --locked-mode
dotnet build $runner -c Release --no-restore
$cpp = 'src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj'
dotnet restore $cpp --locked-mode
dotnet build $cpp -c Release --no-restore
dotnet run --project $cpp -c Release --no-build -- generate `
  docs/corpus/idl/features/10-annotations.idl `
  docs/corpus/7.7.0/generator/cpp/features/10-annotations
```

## Run the licensed wire test

Set `RTI_LICENSE_FILE` to the path of a readable license file on the host.
Use the [wire container recipe](../../README.md#container-variants-and-local-wire-run)
for RTI 7.7.0 package revision `7.7.0.0-1-1bookworm`. The commands below
assume the image is tagged `wireloom-wire:7.7.0-trimmed` and the Release
runner is already built. Publish the Linux x64 native CLI on a Linux host or
use the payload retained by the [C++ package workflow](../../../../.github/workflows/cpp-package.yml)
before starting Docker. The publisher uses the repository's RID lock and
patched AOT runtime; a Windows executable cannot run in this container.

```powershell
$license = (Resolve-Path -LiteralPath $env:RTI_LICENSE_FILE).Path
$evidence = (New-Item -ItemType Directory -Path (
  Join-Path $PWD ('artifacts/cpp-wire-regression/' + [DateTime]::UtcNow.ToString('yyyyMMddTHHmmssfffZ'))
)).FullName
$image = 'wireloom-wire:7.7.0-trimmed'
docker image inspect $image --format '{{.Id}}' |
  Set-Content -LiteralPath (Join-Path $evidence 'image-id.txt')
docker run --rm --workdir /workspace `
  --mount "type=bind,source=$PWD,target=/workspace,readonly" `
  --mount "type=bind,source=$evidence,target=/evidence" `
  --mount "type=bind,source=$license,target=/run/secrets/rti_license.dat,readonly" `
  --env RTI_LICENSE_FILE=/run/secrets/rti_license.dat `
  --env NDDSHOME=/opt/rti.com/rti_connext_dds-7.7.0 `
  --env LD_LIBRARY_PATH=/opt/rti.com/rti_connext_dds-7.7.0/lib/x64Linux4gcc8.5.0 `
  --entrypoint bash $image -lc '
    set -euo pipefail
    cmake -S src/Wireloom.Dds.Cpp.Generator/build -B /evidence/package-build -DWIRELOOM_CPP_PUBLISH_DIR=/workspace/artifacts/cpp-native-package/tools/linux-x64
    cmake --install /evidence/package-build --prefix /evidence/prefix
    dotnet tests/Wireloom.Dds.Generator.WireCompatibility.Runner/bin/Release/net10.0/Wireloom.Dds.Generator.WireCompatibility.Runner.dll --cpp-spike wire /evidence/run /evidence/prefix
  '
```

CMake uses the same installed integration described in the
[package verification guide](../../../Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md).
It compiles the same peer source against fresh Wireloom output and the
[retained RTI control](../../../../docs/corpus/7.7.0/rtiddsgen/cpp/features/10-annotations/).
The runner checks that fresh generation matches the
[retained Wireloom output](../../../../docs/corpus/7.7.0/generator/cpp/features/10-annotations/).
It runs RTI to RTI, Wireloom to RTI, and RTI to Wireloom, each with XCDR1 and
XCDR2 selected explicitly on both endpoints. Each pair uses separate UDPv4
processes. The reader validates two distinct keys, an update and disposal of
the first instance, and a second update to the surviving instance.
Before transport starts, each peer also checks that default construction,
`reset_sample`, and `allocate_sample` preserve the IDL defaults (`id == 0` and
`value == 3`).

Read `run/results.json` for the six scenario outcomes and
`run/source-hashes.json` for the inputs and binaries used. The evidence
also retains CMake and per-peer logs.
