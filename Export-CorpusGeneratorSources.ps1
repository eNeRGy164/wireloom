#requires -Version 7.4
<#
.SYNOPSIS
Exports Wireloom-generated C# sources for the selected RTI API profiles.

.DESCRIPTION
Runs the corpus compliance project for RTI 6.1.2, 7.3.1, and 7.7.0 by default.
Each version manifest specifies its C# export root: generator/csharp for 7.7.0
and generator for 6.1.2 and 7.3.1. Every profile uses the same generation suite,
checks all inventory entries, and compiles every exported case against its exact
runtime. Detailed oracle comparisons are a separate suite.

The script restores dependencies in locked mode, then tests in Release using
the repository SDK. It stops after the first failed profile and restores the
process environment variables when it exits.

Existing matching files are overwritten, but obsolete files are not deleted.
Use a new -OutputRoot for a clean comparison. A new evidence directory retains
input/output hashes, SDK details, and test logs.

.EXAMPLE
./Export-CorpusGeneratorSources.ps1 -Profile 6.1.2

Exports the 6.1.2 profile through its exact-runtime generation/compilation checks.

.EXAMPLE
./Export-CorpusGeneratorSources.ps1

Exports all three profiles. See docs/corpus/README.md for the corpus layout.
#>

[CmdletBinding()]
param(
    [ValidateSet('6.1.2', '7.3.1', '7.7.0')]
    [string[]]$Profile = @('6.1.2', '7.3.1', '7.7.0'),

    [string]$OutputRoot,

    [string]$EvidenceDirectory
)

$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/docs/corpus/scripts/Evidence.Common.ps1"

$project = 'tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj'

if (!$OutputRoot) {
    $OutputRoot = 'docs/corpus'
}

$output = [IO.Path]::GetFullPath($OutputRoot, $PSScriptRoot)

if (!$EvidenceDirectory) {
    $timestamp = [DateTime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
    $runId = [Guid]::NewGuid().ToString('N').Substring(0, 8)
    $EvidenceDirectory = "artifacts/corpus-export-$timestamp-$runId"
}

$evidence = [IO.Path]::GetFullPath($EvidenceDirectory, $PSScriptRoot)

if (Test-Path -LiteralPath $evidence) {
    throw 'EvidenceDirectory must be new to preserve earlier runs.'
}

New-Item -ItemType Directory -Force -Path $evidence | Out-Null

$inputs = @(
    'src/Wireloom.Dds.Compiler'
    'src/Wireloom.Dds.Generator'
    'tests/Wireloom.Dds.Generator.CorpusCompliance'
    'docs/corpus/idl'
    'docs/corpus/manifest.json'
    'docs/corpus/6.1.2/manifest.json'
    'docs/corpus/7.3.1/manifest.json'
    'docs/corpus/7.7.0/manifest.json'
    'docs/corpus/scripts/Evidence.Common.ps1'
    'Export-CorpusGeneratorSources.ps1'
    'global.json'
    'Directory.Build.props'
    'Directory.Packages.props'
    'tests/Directory.Build.props'
    'NuGet.config'
)

$before = Get-EvidenceSnapshot $PSScriptRoot $inputs
$revision = Get-EvidenceRevision $PSScriptRoot

$previousProfile = [Environment]::GetEnvironmentVariable('CORPUS_RTI_PROFILE', 'Process')
$previousExportRoot = [Environment]::GetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', 'Process')

Push-Location $PSScriptRoot

try {
    $sdkVersion = dotnet --version

    if ($LASTEXITCODE -ne 0) {
        throw 'Cannot identify the .NET SDK version.'
    }

    $sdkVersion | Out-File "$evidence/dotnet-version.txt" -Encoding utf8

    $restoreOutput = & dotnet restore $project --locked-mode 2>&1
    $restoreExitCode = $LASTEXITCODE
    $restoreOutput | Tee-Object "$evidence/restore.txt"

    if ($restoreExitCode -ne 0) {
        throw "Corpus locked restore failed (exit code $restoreExitCode)."
    }

    foreach ($version in $Profile | Select-Object -Unique) {
        $env:CORPUS_RTI_PROFILE = $version
        $profileManifestPath = Join-Path $PSScriptRoot "docs/corpus/$version/manifest.json"
        $profileManifest = Get-Content -LiteralPath $profileManifestPath -Raw | ConvertFrom-Json
        if ([string]::IsNullOrWhiteSpace($profileManifest.generatorSourceRoot)) {
            throw "Missing generatorSourceRoot in the $version corpus manifest."
        }

        $env:CORPUS_GENERATOR_EXPORT_ROOT = Join-Path (Join-Path $output $version) $profileManifest.generatorSourceRoot
        $startedAt = [DateTime]::UtcNow.ToString('o')

        $testArguments = @(
            'run'
            '--project', $project
            '--no-restore'
            '--configuration', 'Release'
            '--'
            '--filter-trait', 'CorpusSuite=Generation'
        )

        Write-Host "Exporting corpus sources for RTI profile $version..."
        & dotnet @testArguments | Tee-Object "$evidence/$version-tests.txt"

        if ($LASTEXITCODE -ne 0) {
            throw "Corpus source export failed for RTI profile $version (exit code $LASTEXITCODE)."
        }

        $after = Get-EvidenceSnapshot $PSScriptRoot $inputs

        if ($after.sha256 -ne $before.sha256) {
            throw 'Export inputs changed during execution; do not promote this run.'
        }

        $outputFiles = @(
            Get-ChildItem -LiteralPath $env:CORPUS_GENERATOR_EXPORT_ROOT -Recurse -File -Filter '*.g.cs' |
                ForEach-Object {
                    [IO.Path]::GetRelativePath($env:CORPUS_GENERATOR_EXPORT_ROOT, $_.FullName)
                }
        )

        $outputSnapshot = Get-EvidenceSnapshot $env:CORPUS_GENERATOR_EXPORT_ROOT $outputFiles
        $relativeOutput = [IO.Path]::GetRelativePath($PSScriptRoot, $env:CORPUS_GENERATOR_EXPORT_ROOT)
        $relativeOutput = $relativeOutput.Replace([IO.Path]::DirectorySeparatorChar, [char]'/')

        $exportEvidence = [ordered]@{
            schemaVersion = 1
            kind = 'wireloom-corpus-export'
            profile = $version
            startedAtUtc = $startedAt
            completedAtUtc = [DateTime]::UtcNow.ToString('o')
            source = $revision
            inputs = $before
            configuration = 'Release'
            sdk = $sdkVersion
            testExitCode = 0
            testArguments = $testArguments
            outputDirectory = $relativeOutput
            output = $outputSnapshot
            staleOutputPolicy = 'Existing files are not deleted. Use a new OutputRoot for an uncontaminated export.'
        }

        Write-EvidenceJson "$evidence/$version.json" $exportEvidence
    }

    Write-Host "Export provenance and test logs: $evidence"
}
finally {
    Pop-Location

    [Environment]::SetEnvironmentVariable('CORPUS_RTI_PROFILE', $previousProfile, 'Process')
    [Environment]::SetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', $previousExportRoot, 'Process')
}
