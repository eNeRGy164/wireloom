# LTS evidence and reproduction

The shared inventory is defined by `idl/` and `manifest.json`. Each version
folder records its RTI oracles, available Wireloom exports, and feature coverage.
Features unavailable in a release are N/A.
See [feature coverage](FEATURE-COVERAGE.md) for current compatibility limits.

| RTI package | API profile    | rtiddsgen | Pinned capture / helper base image                                                                                |
| :---------- | :------------- | :-------- | :---------------------------------------------------------------------------------------------------------------- |
| 6.1.2       | `Rtiddsgen312` | 3.1.2     | Local `wireloom-oracle:6.1.2`, image ID `sha256:bb26f29ea69eee4da28065f1c2e668c9ca5efcb34c64c290945d69a8951171c1` |
| 7.3.1       | `Rtiddsgen431` | 4.3.1     | `rticom/connext-base:7.3.1@sha256:f33a61ec6381a8fb73cbb019fbb5c081f434dc5217eb14283fad995306be0f04`               |
| 7.7.0       | `Rtiddsgen470` | 4.7.0     | `rticom/connext-base:7.7.0@sha256:efe4d35807a4aadaa59f0e84545d4c6b70328af5b0c0ca0498e63ecc1e16e1ca`               |

| RTI package | Feature and oracle evidence                                          | Wireloom source exports              | Runtime / DDS wire evidence                                                                 |
| :---------- | :------------------------------------------------------------------- | :----------------------------------- | :------------------------------------------------------------------------------------------ |
| 6.1.2       | [Feature coverage](6.1.2/FEATURE-COVERAGE.md); C# and C++11 captures | [Retained exports](6.1.2/generator/) | Exact-version runtime and wire evidence; see profile-specific limits and recorded outcomes. |
| 7.3.1       | [Feature coverage](7.3.1/FEATURE-COVERAGE.md); C# and C++11 captures | [Retained exports](7.3.1/generator/) | Exact-version runtime and wire evidence; see profile-specific limits and recorded outcomes. |
| 7.7.0       | [Feature coverage](7.7.0/FEATURE-COVERAGE.md); C# and C++11 captures | [Retained exports](7.7.0/generator/csharp/) | Exact-version runtime and wire evidence; see profile-specific limits and recorded outcomes. |

The resolved RTI assembly selects the API profile. Generation, compilation,
runtime, and wire evidence establish different results for each exact release;
profile selection does not extend qualification to other versions. The root
manifest's `oracle` block describes the 7.7.0
capture, not the selected profile.

## Prerequisites

Run commands from the repository root using PowerShell 7.4+, Git, and the SDK
selected by `global.json`. RTI capture needs Docker and access to the pinned
images. Its container uses `cpp` and installs `g++` if needed, requiring
package access. Use `-DisablePreprocessor` only for IDL without includes or
directives.
Retain generated reference sources with their original notices and provenance.

## Corpus tests and source exports

```powershell
$corpus = 'tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj'

dotnet restore $corpus --locked-mode
foreach ($version in @('6.1.2', '7.3.1', '7.7.0')) {
    $env:CORPUS_RTI_PROFILE = $version
    dotnet run --project $corpus --no-restore --configuration Release -- --filter-trait CorpusSuite=Generation
    if ($LASTEXITCODE -ne 0) { throw "Corpus failed: $version" }
}

# Detailed oracle comparisons use the separately qualified oracle profiles.
foreach ($version in @('7.3.1', '7.7.0')) {
    $env:CORPUS_RTI_PROFILE = $version
    dotnet run --project $corpus --no-restore --configuration Release -- --filter-trait CorpusSuite=Oracle
    if ($LASTEXITCODE -ne 0) { throw "Oracle comparison failed: $version" }
}
Remove-Item Env:CORPUS_RTI_PROFILE
```

Export all three profiles with locked restore, Release tests, logs, SDK details,
and input/output hashes:

```powershell
./Export-CorpusGeneratorSources.ps1 -OutputRoot artifacts/corpus-reproduction
```

Use a new `-OutputRoot` for comparison. C# sources go to the profile's
`generatorSourceRoot` under `<OutputRoot>/<version>/`. Evidence goes to
`artifacts/corpus-export-.../`.
Use `-Profile 6.1.2`, `-Profile 7.3.1`, or `-Profile 7.7.0` for one profile.
Every profile selects `CorpusSuite=Generation`: the same inventory-wide
generation, exact-runtime compilation, and export checks. Detailed oracle
comparisons run separately with `CorpusSuite=Oracle` as shown above.
Without `-OutputRoot`, the script updates the C# export root under
`docs/corpus/<version>/` and leaves obsolete files for manual review.

## Capture RTI C# and C++ oracles

Use the pinned image listed for the selected version. Registry images are
selected by default for 7.3.1 and 7.7.0. The 6.1.2 image is built locally from
the matching trimmed image and official RTI packages; pass its immutable image
ID through `-Image`:

```powershell
docker build --file tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile.wire --build-arg RTI_VERSION=6.1.2 --build-arg RTI_PACKAGE_VERSION=6.1.2.0-5-4bullseye --tag wireloom-wire:6.1.2-trimmed tests/Wireloom.Dds.Generator.WireCompatibility
docker build --file tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile.oracle --tag wireloom-oracle:6.1.2 tests/Wireloom.Dds.Generator.WireCompatibility
$captureImage = docker image inspect wireloom-oracle:6.1.2 --format '{{.Id}}'
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 6.1.2 -Image $captureImage
```

The capture image adds the exact `6.1.2.0-5-4bullseye` RTI JRE and generator
packages from the same official signed feed used by the trimmed image.
Keep the build log and image ID with the capture. It requires no RTI license.

```powershell
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 7.3.1 -CaseId 09-autoid-hash,09-must-understand
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 7.7.0 -CaseId 09-autoid-hash,09-must-understand
```

Omit `-CaseId` for the complete inventory. Use `-Language csharp` or
`-Language cpp` for one language. `-OutputDirectory` must be a new directory
under `artifacts/`.

The script uses manifest defines, include directories, and C++ options,
including `-unboundedSupport` where specified. It uses `-generateIncludeFiles`
only on 7.7.0. The C++ FlatData input adaptation is recorded with input hashes.
Wire-peer workarounds are not applied to oracle captures.

Retain generated dependencies for included IDL as well as root outputs. RTI
6.1.2 and 7.3.1 require generating the included IDL separately; 7.7.0 can place included
output beside the root output directory. C164 retains its independently
generated `autoid-hash-include.idl` output in the case's `includes/` directory.
Its profile manifests record this additional capture and its hashes.

Inspect per-case `outcome`, `exitCode`, and `rtiddsgen.log`: an IDL rejection
does not fail the capture process. Setup errors do. `capture-request.json`
records the request; completed evidence consists of `capture.json`,
`toolchain.txt`, logs, and generated files. Partial output stays in `.capture/`.

Before retaining a capture:

1. Compare output with the matching `<version>/rtiddsgen/` tree and review
   diagnostics, options, hashes, and original notices.
2. Copy reviewed generated sources into the corpus. For a negative case that
   produces no source, retain its `rtiddsgen.log` in the case directory.
   Keep other logs, capture reports, and toolchain records in the artifact bundle.
3. Update the profile's C# and C++ manifests and feature coverage together.

## Compile an RTI C# oracle

Compile one case at a time because corpus roots can share type names. Replace
`$source` with the fresh capture directory to check new output; repeat with
`$version = '7.7.0'` for the other release.

```powershell
$version = '7.3.1'
$probe = 'tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/RtiOracleProbe.csproj'
$source = (Resolve-Path "docs/corpus/$version/rtiddsgen/csharp/features/09-autoid-hash").Path

dotnet restore $probe --locked-mode -p:WireCompatibilityRtiVersion=$version -p:RtiConnextDdsVersion=$version

dotnet build $probe --no-restore --configuration Release -p:WireCompatibilityRtiVersion=$version -p:RtiConnextDdsVersion=$version -p:RtiOracleSourceDir=$source
```

## Runtime, package, and wire verification

The [continuous matrix](../guides/support-and-compatibility.md#continuous-lts-verification)
checks planning and reports for all three profiles and exercises their runtime
contracts and fresh packages on Windows and Linux with .NET 8 and .NET 10. Local commands
are in the [runtime guide](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#run)
and [package guide](../../tests/Wireloom.Dds.Generator.PackageIntegration/README.md).
.NET 8 execution requires its runtime alongside the repository SDK.

For licensed DDS verification, dispatch **Wire compatibility** or follow the
[wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#run-the-matrix)
for local runs, filters, sharding, and peer adaptations. Retain `results.json`,
`results.md`, `plan.json`, `shard.json`, `scenarios.tsv`, and sanitized logs
before CI artifacts expire.

## Evidence records

Generation, compilation, runtime API tests, and DDS exchanges establish
separate results. Record the applicable cases, fixtures, pairings, and peer
adaptations with each result, together with:

- UTC start/end times, source commit, and working-tree state; include the patch
  or source snapshot for a dirty-tree run.
- Input/output hashes, manifest, profile, SDK, exact RTI package and generator,
  image digest, preprocessor, and codegen options.
- Outcomes, exit status, logs, and the retained report location.

Capture and export snapshot hashes use SHA-256 over ordinal-path-sorted UTF-8
lines of `relative/path<TAB>lowercase-file-sha256`, separated by LF without a
final LF. Paths use `/`; file hashes cover raw bytes. Export evidence and log
sidecars are excluded from the generated-source digest.

Keep run reports under `artifacts/` or with the CI artifacts; commit only the
reviewed corpus sources, manifests, and documentation.
